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
        private String connectionString;
        private readonly Object lockToken = new Object();

        public VlslVConnection(String connStr)
        {
            connectionString = connStr;
            CheckConnectionState();
        }

        /// <summary>
        /// Close Connection to database
        /// </summary>
        private void CloseConnection()
        {
            if (vlslvConnect != null)
            {
                if (vlslvConnect.State == ConnectionState.Open)
                    vlslvConnect.Close();
            }
        }

        /// <summary>
        /// Check connection state
        /// </summary>
        public void CheckConnectionState()
        {
            lock (lockToken)
            {
                if (vlslvConnect == null)
                {
                    vlslvConnect = new System.Data.SqlClient.SqlConnection(connectionString);
                    vlslvConnect.Open();
                }

                if ((vlslvConnect.State == ConnectionState.Closed) ||
                    (vlslvConnect.State == ConnectionState.Broken))
                {
                    if (vlslvConnect.State == ConnectionState.Broken)
                        vlslvConnect.Close();
                    //SqlConnection.ClearPool(_connection); //??
                    vlslvConnect.Open();
                }
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
            CheckConnectionState();

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
            CloseConnection();
            if (vlslvConnect != null)
                vlslvConnect.Dispose();
        }

        ~VlslVConnection()
        {
            Dispose();
        }
    }

}
