using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace VirusBlokAda.CC.DataBase
{
    /// <summary>
    /// Class to connect to database
    /// </summary>
    internal sealed class VlslVConnection : IDisposable
    {
        private SqlConnection vlslvConnect;
        private IDbTransaction vlslvTransaction;
        private static String connectionString;

        public VlslVConnection()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public VlslVConnection(String connStr)
        {
            vlslvConnect = CreateConnection(connStr);
        }

        private static SqlConnection CreateConnection(String connStr)
        {
            connectionString = connStr;
            return new System.Data.SqlClient.SqlConnection(connectionString);
        }

        /// <summary>
        /// Open connection to database
        /// </summary>
        public void OpenConnection()
        {
            CheckConnectionState(false);
            vlslvConnect.Open();
        }
        /// <summary>
        /// Close Connection to database
        /// </summary>
        public void CloseConnection()
        {
            if (vlslvConnect != null)
            {
                CheckConnectionState(true);
                vlslvConnect.Close();
            }
        }

        /// <summary>
        /// Check connection state
        /// </summary>
        /// <param name="isOpen">State</param>
        public void CheckConnectionState(Boolean isOpen)
        {
            if (isOpen)
            {
                if ((vlslvConnect.State & ConnectionState.Open) != ConnectionState.Open)
                    throw new InvalidOperationException("Connection is not open.");
            }
            else
            {
                if ((vlslvConnect.State & ConnectionState.Open) == ConnectionState.Open)
                    throw new InvalidOperationException("Connection is already open.");
            }
        }

        /// <summary>
        /// Add parameter to the command for input or input direction
        /// </summary>
        /// <param name="cmd">Command object</param>
        /// <param name="paramName">Name of parameter</param>
        /// <param name="dbType">Type of parameter</param>
        /// <param name="paramValue">Value of parameter</param>
        /// <param name="direction">Direction of parameter</param>
        /// <returns>Return newly created object of command parameter</returns>
        public IDbDataParameter AddCommandParameter(IDbCommand cmd, String paramName,
            DbType dbType, Object paramValue, ParameterDirection direction)
        {
            CheckConnectionState(true);
            IDbDataParameter parameter = cmd.CreateParameter();
            parameter.ParameterName = paramName;
            parameter.Direction = direction;

            parameter.DbType = dbType;

            if (paramValue != null)
            {
                switch (dbType)
                {
                    case DbType.Int16:
                        {
                            parameter.Value =
                                (Int16)paramValue == Int16.MinValue ? DBNull.Value : paramValue;
                            break;
                        }
                    case DbType.Int32:
                        {
                            parameter.Value =
                                (Int32)paramValue == Int32.MinValue ? DBNull.Value : paramValue;
                            break;
                        }
                    case DbType.Int64:
                        {
                            parameter.Value =
                                (Int64)paramValue == Int64.MinValue ? DBNull.Value : paramValue;
                            break;
                        }
                    case DbType.AnsiStringFixedLength:
                    case DbType.AnsiString:
                    case DbType.String:
                    case DbType.StringFixedLength:
                        {
                            parameter.Value =
                                (String)paramValue == null ? DBNull.Value : paramValue;
                            break;
                        }
                    case DbType.Decimal:
                        {
                            parameter.Value =
                                (SqlDecimal)paramValue == Decimal.MinValue ? DBNull.Value : paramValue;
                            break;
                        }

                    case DbType.Byte:
                        {
                            parameter.Value =
                                (SqlByte)paramValue == SqlByte.Null ? DBNull.Value : paramValue;
                            break;
                        }
                    case DbType.Boolean:
                        {
                            if (paramValue == null) parameter.Value = SqlBoolean.Null;
                            else parameter.Value = new SqlBoolean((Boolean)paramValue);

                            break;
                        }
                    case DbType.Guid:
                        {
                            if (paramValue == null) parameter.Value = SqlGuid.Null;
                            else parameter.Value = (SqlGuid)paramValue;
                            break;
                        }

                    case DbType.Currency:
                        {
                            if (paramValue == null) parameter.Value = SqlMoney.MinValue;
                            else parameter.Value = (SqlMoney)paramValue;
                            break;
                        }

                    case DbType.DateTime:
                    case DbType.Date:
                        {
                            if (paramValue == null) parameter.Value = SqlDateTime.Null;
                            else parameter.Value = new SqlDateTime((DateTime)paramValue);
                            break;
                        }
                    case DbType.Binary:
                        {
                            if (paramValue == null) parameter.Value = SqlBinary.Null;
                            else parameter.Value = (SqlBinary)paramValue;
                            break;
                        }
                    default:
                        parameter.Value = DBNull.Value;
                        break;
                }
            }
            else
            {
                parameter.Value = DBNull.Value;
            }
            cmd.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Create command
        /// </summary>
        /// <param name="commandText">text of command</param>
        /// <param name="isStoredProc">true is stored procedure</param>
        /// <returns>Returns newly created command command objecty</returns>
        public IDbCommand CreateCommand(String commandText, Boolean isStoredProc)
        {
            CheckConnectionState(true);

            IDbCommand cmd = vlslvConnect.CreateCommand();
            cmd.CommandText = commandText;
            cmd.Transaction = vlslvTransaction;
            cmd.CommandTimeout = 120;

            if (isStoredProc)
                cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }

        public void BeginTransaction()
        {
            vlslvTransaction = vlslvConnect.BeginTransaction();
        }

        public void EndTransaction()
        {
            vlslvTransaction.Commit();
        }

        public void RollBackTransaction()
        {
            vlslvTransaction.Rollback();
        }

        /// <summary>
        /// Support to check transaction state...
        /// </summary>
        /// <param name="isOpen">State to check</param>
        public void CheckTransactionState(Boolean isOpen)
        {
            if (isOpen)
            {
            }
            else
            {
            }
        }

        public void Dispose()
        {
            if (vlslvConnect != null)
                vlslvConnect.Dispose();
        }
    }

}
