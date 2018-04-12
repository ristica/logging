using System;
using System.Data;
using System.Data.SqlClient;
using Demo.Contracts.Logging;
using Demo.Contracts.Persistance;
using Demo.Persistance.Data;
using Demo.Persistance.Logger;
using Microsoft.Extensions.Configuration;

namespace Demo.Persistance
{
    public class PersistanceService : IPersistanceService
    {
        #region Fields

        private const string _connectionStringKey = "ConnectionStrings:PersistanceConnectionString";
        private readonly ILoggingService _loggingService;
        private readonly IConfiguration _configuration;
        private readonly string _connectionStringValue;

        #endregion

        #region Properties
        #endregion

        #region C-Tor

        public PersistanceService(ILoggingService loggingService, IConfiguration configuration)
        {
            this._configuration = configuration;
            this._loggingService = loggingService;
            this._connectionStringValue = _configuration.GetSection(_connectionStringKey).Value;
        }

        #endregion

        #region IPersistanceService implementation

        public void Create(IPersistanceData entityToCreate)
        {
            using (var connection = new SqlConnectionWithExceptionLogger(this._loggingService, this._connectionStringValue))
            {
                connection.Open();
                connection.TryExecute(() =>
                {
                    CreateStateData(connection, entityToCreate);
                });
            }
        }

        public IPersistanceData Get(long persistanceDataId)
        {
            IPersistanceData result = null;
            using (var connection = new SqlConnectionWithExceptionLogger(this._loggingService, this._connectionStringValue))
            {
                connection.Open();
                connection.TryExecute(() =>
                {
                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        "SELECT [SomeData] FROM [dbo].[ufn_GetPersistedData](@PersistanceDataId)";
                    command.Parameters.AddWithValue("@PersistanceDataId", persistanceDataId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new PersistanceData
                            {
                                SomeData = reader.GetString(0)
                            };
                        }
                    }
                });
            }
            return result;
        }

        public IPersistanceData Update(IPersistanceData entityToUpdate)
        {
            using (var connection = new SqlConnectionWithExceptionLogger(this._loggingService, this._connectionStringValue))
            {
                connection.Open();
                if (entityToUpdate.PersistanceDataId == 0)
                    CreateStateData(connection, entityToUpdate);
                else
                    UpdateStateData(connection, entityToUpdate);
            }
            // Get from DB ? nein..ev. neue werte sind nur IDs aus output, somit alles da..
            return entityToUpdate;
        }

        public void Delete(long persistanceDataId)
        {
            using (var connection = new SqlConnectionWithExceptionLogger(this._loggingService, this._connectionStringValue))
            {
                connection.Open();
                connection.TryExecute(() =>
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "[dbo].[usp_DeleteData]";
                        command.Parameters.AddWithValue("@PersistanceDataId", persistanceDataId);
                        command.ExecuteNonQuery();
                    }
                });
            }
        }

        #endregion

        #region Helpers

        private static void CreateStateData(SqlConnectionWithExceptionLogger connection, IPersistanceData entityToCreate)
        {
            connection.TryExecute(() =>
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[usp_InsertData]";
                    command.Parameters.AddWithValue("@Data", entityToCreate.SomeData);
                    var outPutParameter = new SqlParameter
                    {
                        ParameterName = "@PersistanceDataId",
                        SqlDbType = SqlDbType.BigInt,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outPutParameter);
                    command.ExecuteNonQuery();
                    entityToCreate.PersistanceDataId = (long)outPutParameter.Value;
                }
            });
        }

        private static void UpdateStateData(SqlConnectionWithExceptionLogger connection, IPersistanceData entityToUpdate)
        {
            connection.TryExecute(() =>
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[usp_UpdateData]";
                    command.Parameters.AddWithValue("@PersistanceProcessId", entityToUpdate.PersistanceDataId);
                    command.ExecuteNonQuery();
                }
            });
        }


        #endregion
    }
}
