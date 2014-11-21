﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace VirusBlokAda.CC.DataBase
{
    internal sealed class DeviceClassManager
    {
        private readonly String connectionString;
        private readonly DbProviderFactory factory;

        #region Constructors
        public DeviceClassManager(String connectionString):this(connectionString,"System.Data.SqlClient")
        {            
        }

        public DeviceClassManager(String connectionString,String DbFactoryName):this(connectionString,DbProviderFactories.GetFactory(DbFactoryName))
        {            
        }
        public DeviceClassManager(String connectionString, DbProviderFactory factory)
        {
            this.connectionString = connectionString;
            this.factory = factory;
            
        }
        #endregion

        #region Methods

        /// <summary>
        /// Deserialize Base64 string to CLASS_INFO
        /// </summary>
        /// <param name="text">Base64 string</param>
        /// <returns>CLASS_INFO entity</returns>
        internal static CLASS_INFO DeserializeFromBase64(String text)
        {
            Byte[] buffer = Convert.FromBase64String(text);

            CLASS_INFO ci = new CLASS_INFO();

            ci.dev_class = buffer[0];
            ci.mount = buffer[1];

            return ci;
        }

        /// <summary>
        /// Serialize CLASS_INFO entity to Base64 string
        /// </summary>
        /// <param name="di">CLASS_INFO entity</param>
        /// <returns>Base64 string</returns>
        internal static String SerializeToBase64(CLASS_INFO ci)
        {
            Byte[] buffer = new Byte[2];

            buffer[0] = ci.dev_class;
            buffer[1] = ci.mount;

            return Convert.ToBase64String(buffer);
        }

        #region Device Class

        /// <summary>
        /// Get device class entity by uid
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        internal DeviceClass GetDeviceClass(String uid)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDeviceClass";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ID";
                param.Value = uid;
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                DeviceClass d = null;
                while (reader.Read())
                {
                    d = new DeviceClass();
                    if (reader.GetValue(0) != DBNull.Value)
                        d.ID = reader.GetInt16(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        d.UID = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        d.Class = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        d.ClassName = reader.GetString(3);
                }
                reader.Close();
                return d;
            }
        }

        /// <summary>
        /// Add new device class to DB
        /// </summary>
        /// <param name="deviceClass"></param>
        /// <returns></returns>
        internal DeviceClass Add(DeviceClass deviceClass)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddNewDeviceClass";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@UID";
                param.Value = deviceClass.UID;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@Class";
                param.Value=deviceClass.Class;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@ClassName";
                param.Value=deviceClass.ClassName;
                cmd.Parameters.Add(param); 

                DeviceClass d = (DeviceClass)deviceClass.Clone();
                con.Open();
                d.ID = Convert.ToInt16(cmd.ExecuteScalar());
                return d;
            }
        }

        /// <summary>
        /// Delete class of devices by UID
        /// </summary>
        /// <param name="deviceClass"></param>
        internal void Delete(DeviceClass deviceClass)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteDeviceClassByUID";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@UID";
                param.Value = deviceClass.UID;
                cmd.Parameters.Add(param);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Change comment
        /// </summary>
        /// <param name="deviceClass"></param>
        internal void ChangeComment(DeviceClass deviceClass)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ChangeCommentDeviceClassByUID";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@UID";
                param.Value = deviceClass.UID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@Comment";
                param.Value = deviceClass.Class;
                cmd.Parameters.Add(param);

                con.Open();
                cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Get Device class list
        /// </summary>
        /// <returns></returns>
        internal List<DeviceClass> GetDeviceClassList()
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDeviceClassList";

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<DeviceClass> list = new List<DeviceClass>();
                DeviceClass deviceClass;
                while (reader.Read())
                {
                    deviceClass = new DeviceClass();
                    if (reader.GetValue(0) != DBNull.Value)
                        deviceClass.ID = reader.GetInt16(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        deviceClass.UID = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        deviceClass.Class = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        deviceClass.ClassName = reader.GetString(3);

                    list.Add(deviceClass);
                }
                reader.Close();

                return list;
            }
        }

        /// <summary>
        /// Get Device class list by filter
        /// </summary>
        /// <returns></returns>
        internal List<DeviceClass> GetDeviceClassListByFilter(String where, String orderBy, Int32 index, Int32 pageCount)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDeviceClassListByFilter";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@Page";
                param.Value = index;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@RowCount";
                param.Value = pageCount;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@OrderBy";
                param.Value = orderBy;
                cmd.Parameters.Add(param);
 
                param=cmd.CreateParameter();
                param.ParameterName="@Where";
                param.Value=where;
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<DeviceClass> list = new List<DeviceClass>();
                DeviceClass deviceClass;
                while (reader.Read())
                {
                    deviceClass = new DeviceClass();
                    if (reader.GetValue(0) != DBNull.Value)
                        deviceClass.ID = reader.GetInt16(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        deviceClass.UID = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        deviceClass.Class = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        deviceClass.ClassName = reader.GetString(3);

                    list.Add(deviceClass);
                }
                reader.Close();

                return list;
            }
        }

        /// <summary>
        /// Get Device class count by filter
        /// </summary>
        /// <returns></returns>
        internal Int32 GetDeviceClassCount(String where)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDeviceClassCount";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@Where";
                param.Value = where;
                cmd.Parameters.Add(param);

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        #endregion

        #region Device Class Policy
        #region Add
        /// <summary>
        /// Add new device class policy to computer
        /// </summary>
        /// <param name="policy">ComputerID, DeviceClassID, DeviceClassMode</param>
        /// <returns></returns>
        internal DeviceClassPolicy AddPolicy(DeviceClassPolicy policy)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddDeviceClassPolicy";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerID";
                param.Value = policy.Computer.ID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@DeviceClassID";
                param.Value = policy.ClassOfDevice.ID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@DeviceClassMode";
                param.Value = policy.Mode.ToString();
                cmd.Parameters.Add(param); 

                DeviceClassPolicy d = (DeviceClassPolicy)policy.Clone();
                con.Open();
                d.ID = Convert.ToInt32(cmd.ExecuteScalar());

                return d;
            }
        }

        /// <summary>
        /// Add new device class policy to group
        /// </summary>
        /// <param name="policy">DeviceClassID, DeviceClassMode</param>
        /// <param name="group">groupID</param>
        /// <returns>added policy count</returns>
        internal Int32 AddPolicy(DeviceClassPolicy policy, Group group)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddDeviceClassPolicyToGroup";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@GroupID";
                param.Value = group.ID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@DeviceClassID";
                param.Value = policy.ClassOfDevice.ID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@DeviceClassMode";
                param.Value = policy.Mode.ToString();
                cmd.Parameters.Add(param);

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// Add new device class policy to group
        /// </summary>
        /// <param name="policy">DeviceClassID, DeviceClassMode</param>
        /// <param name="group">groupID</param>
        internal Int32 AddPolicyWithoutGroup(DeviceClassPolicy policy)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddDeviceClassPolicyWithoutGroup";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@DeviceClassID";
                param.Value = policy.ClassOfDevice.ID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@DeviceClassMode";
                param.Value = policy.Mode.ToString();
                cmd.Parameters.Add(param);

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// Change mode to device class policy to group
        /// </summary>
        /// <param name="policy">DeviceClassID, DeviceClassMode</param>
        /// <param name="group">groupID</param>
        internal void ChangeMode(DeviceClassPolicy policy, Group group)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ChangeDeviceClassModeToGroup";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@GroupID";
                param.Value = group.ID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@DeviceClassID";
                param.Value = policy.ClassOfDevice.ID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@DeviceClassMode";
                param.Value = policy.Mode.ToString();
                cmd.Parameters.Add(param); 

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Change mode to device class policy without group
        /// </summary>
        /// <param name="policy">DeviceClassID, DeviceClassMode</param>
        internal void ChangeModeWithoutGroup(DeviceClassPolicy policy)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ChangeDeviceClassModeWithoutGroup";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@DeviceClassID";
                param.Value = policy.ClassOfDevice.ID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@DeviceClassMode";
                param.Value = policy.Mode.ToString();
                cmd.Parameters.Add(param);             

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Change mode to device class policy
        /// </summary>
        /// <param name="policy"></param>
        /// <returns>ID updated item, 0 - not update</returns>
        internal Int32 ChangeMode(DeviceClassPolicy policy)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ChangeModeToDeviceClassPolicy";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerID";
                param.Value = policy.Computer.ID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@DeviceClassID";
                param.Value = policy.ClassOfDevice.ID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@DeviceClassMode";
                param.Value = policy.Mode.ToString();
                cmd.Parameters.Add(param); 

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        #endregion
        #region Delete
        /// <summary>
        /// Delete device class policy from computer
        /// </summary>
        /// <param name="policy">ComputerID, DeviceClassID</param>
        internal void DeletePolicy(DeviceClassPolicy policy)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteDeviceClassPolicy";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerID";
                param.Value = policy.Computer.ID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@DeviceClassID";
                param.Value = policy.ClassOfDevice.ID;
                cmd.Parameters.Add(param);
              
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Delete device class policy by DeviceClassID
        /// </summary>
        /// <param name="deviceClass">ID</param>        
        internal void DeletePolicy(DeviceClass deviceClass)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteDeviceClassPolicyByDeviceClassID";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@DeviceClassID";
                param.Value = deviceClass.ID;
                cmd.Parameters.Add(param);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Delete device class policy By ComputerID
        /// </summary>
        /// <param name="computer">ID</param>
        internal void DeletePolicy(ComputersEntity computer)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteDeviceClassPolicyByComputerID";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerID";
                param.Value = computer.ID;
                cmd.Parameters.Add(param);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Delete device class policy from group
        /// </summary>
        /// <param name="deviceClass">ID</param>
        /// <param name="group">ID</param>
        internal void DeletePolicy(DeviceClass deviceClass, Group group)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteDeviceClassPolicyFromGroup";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@GroupID";
                param.Value = group.ID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@DeviceClassID";
                param.Value = deviceClass.ID;
                cmd.Parameters.Add(param);
              
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Delete device class policy from group
        /// </summary>
        /// <param name="deviceClass">ID</param>
        /// <param name="group">ID</param>
        internal void DeletePolicyWithoutGroup(DeviceClass deviceClass)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteDeviceClassPolicyWithoutGroup";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@DeviceClassID";
                param.Value = deviceClass.ID;
                cmd.Parameters.Add(param);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        
        /// <summary>
        /// Delete all device class policy from group
        /// </summary>
        /// <param name="group">ID</param>
        internal void DeletePolicy(Group group)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DeleteDeviceClassPolicyFromGroupByGroupID";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@GroupID";
                param.Value = group.ID;
                cmd.Parameters.Add(param);
                
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        #endregion
        #region Get
        /// <summary>
        /// Get device class policy list by computerID
        /// </summary>
        /// <param name="computer">ID</param>
        /// <returns></returns>
        internal List<DeviceClassPolicy> GetPolicyList(ComputersEntity computer)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDeviceClassPolicyListByComputerID";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerID";
                param.Value = computer.ID;
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<DeviceClassPolicy> list = new List<DeviceClassPolicy>();
                DeviceClassPolicy policy;
                while (reader.Read())
                {
                    policy = new DeviceClassPolicy();
                    policy.Computer.ID = computer.ID;
                    if (reader.GetValue(0) != DBNull.Value)
                        policy.ID = reader.GetInt32(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        policy.ClassOfDevice.ID = reader.GetInt16(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        policy.ClassOfDevice.UID = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        policy.ClassOfDevice.Class = reader.GetString(3);
                    if (reader.GetValue(4) != DBNull.Value)
                        policy.ClassOfDevice.ClassName = reader.GetString(4);
                    if (reader.GetValue(5) != DBNull.Value)
                        policy.Mode = DeviceClassModeExtensions.Get(reader.GetString(5));

                    list.Add(policy);
                }
                reader.Close();

                return list;
            }
        }

        /// <summary>
        /// Get device class policy list by groupID
        /// </summary>
        /// <param name="group">ID</param>
        /// <returns>DeviceClassPolicy.ID is [All]-flag (1 - this policy is applied to all computer from group)</returns>
        internal List<DeviceClassPolicy> GetPolicyList(Group group)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDeviceClassPolicyListByGroupID";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@GroupID";
                param.Value = group.ID;
                cmd.Parameters.Add(param);
                
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<DeviceClassPolicy> list = new List<DeviceClassPolicy>();
                DeviceClassPolicy policy;
                while (reader.Read())
                {
                    policy = new DeviceClassPolicy();
                    if (reader.GetValue(0) != DBNull.Value)
                        policy.ClassOfDevice.ID = reader.GetInt16(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        policy.ClassOfDevice.UID = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        policy.ClassOfDevice.Class = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        policy.ClassOfDevice.ClassName = reader.GetString(3);
                    if (reader.GetValue(4) != DBNull.Value)
                        policy.Mode = DeviceClassModeExtensions.Get(reader.GetString(4));
                    if (reader.GetValue(5) != DBNull.Value)
                        policy.ID = reader.GetInt32(5);

                    list.Add(policy);
                }
                reader.Close();

                return list;
            }
        }

        /// <summary>
        /// Get device class policy list without groups
        /// </summary>
        /// <returns>DeviceClassPolicy.ID is [All]-flag (1 - this policy is applied to all computer from group)</returns>
        internal List<DeviceClassPolicy> GetPolicyList()
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDeviceClassPolicyListWithoutGroup";
              
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<DeviceClassPolicy> list = new List<DeviceClassPolicy>();
                DeviceClassPolicy policy;
                while (reader.Read())
                {
                    policy = new DeviceClassPolicy();
                    if (reader.GetValue(0) != DBNull.Value)
                        policy.ClassOfDevice.ID = reader.GetInt16(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        policy.ClassOfDevice.UID = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        policy.ClassOfDevice.Class = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        policy.ClassOfDevice.ClassName = reader.GetString(3);
                    if (reader.GetValue(4) != DBNull.Value)
                        policy.Mode = DeviceClassModeExtensions.Get(reader.GetString(4));
                    if (reader.GetValue(5) != DBNull.Value)
                        policy.ID = reader.GetInt32(5);

                    list.Add(policy);
                }
                reader.Close();

                return list;
            }
        }

        /// <summary>
        /// Get computer list by deviceID
        /// </summary>
        /// <param name="deviceClass">ID</param>
        /// <returns>DeviceClassPolicy.ID is [GroupID] (0 - if computer without group)</returns>
        internal List<DeviceClassPolicy> GetComputerList(DeviceClass deviceClass)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetComputerListByDeviceClassID";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@DeviceClassID";
                param.Value = deviceClass.ID;
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<DeviceClassPolicy> list = new List<DeviceClassPolicy>();
                DeviceClassPolicy policy;
                while (reader.Read())
                {
                    policy = new DeviceClassPolicy();
                    policy.ID = -1;
                    if (reader.GetValue(0) != DBNull.Value)
                        policy.ClassOfDevice.ID = reader.GetInt16(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        policy.Computer.ID = reader.GetInt16(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        policy.Computer.ComputerName = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        policy.Mode = DeviceClassModeExtensions.Get(reader.GetString(3));
                    if (reader.GetValue(4) != DBNull.Value)
                        policy.ID = reader.GetInt32(4);

                    list.Add(policy);
                }
                reader.Close();

                return list;
            }
        }

        /// <summary>
        /// Get list of device class mode
        /// </summary>
        /// <returns></returns>
        internal List<String> GetDeviceClassModeList()
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;
                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetDeviceClassModeList";

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<String> list = new List<String>();
                while (reader.Read())
                {
                    /* [ID]
                    if (reader.GetValue(0) != DBNull.Value)
                        reader.GetInt16(0);
                    */
                    if (reader.GetValue(1) != DBNull.Value)
                        list.Add(reader.GetString(1));
                }
                reader.Close();

                return list;
            }
        }

        #endregion
        #endregion

        #endregion
    }
}