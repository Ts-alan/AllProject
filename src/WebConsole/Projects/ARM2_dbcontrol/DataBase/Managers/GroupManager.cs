using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.DataBase;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace VirusBlokAda.CC.DataBase
{
    internal sealed class GroupManager
    {
        private readonly String connectionString;
		private readonly DbProviderFactory factory;

        #region Constructors
        public GroupManager(String connectionString):this(connectionString,"System.Data.SqlClient")
        {            
        }

        public GroupManager(String connectionString,String DbFactoryName):this(connectionString,DbProviderFactories.GetFactory(DbFactoryName))
        {            
        }
        public GroupManager(String connectionString, DbProviderFactory factory)
        {
            this.connectionString = connectionString;
            this.factory = factory;
            
        }
        #endregion

        #region Methods

        #region Administration

        /// <summary>
        /// Add new group to database
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        internal Int32 Add(Group group)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddGroup";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@GroupName";
                param.Value = group.Name;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@Comment";
                param.Value = group.Comment;
                cmd.Parameters.Add(param);
               
                param=cmd.CreateParameter();
                param.ParameterName="@ParentID";
                param.Value=group.ParentID;
                cmd.Parameters.Add(param);

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// Remove group
        /// </summary>
        /// <param name="group"></param>
        internal void Delete(Group group)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "RemoveGroupByName";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@GroupName";
                param.Value = group.Name;
                cmd.Parameters.Add(param);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Update group
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="newGroupName"></param>
        internal void Update(String groupName, String newGroupName, String newComment, Int32? newParentID)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UpdateGroup";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@GroupName";
                param.Value = groupName;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@NewGroupName";
                param.Value = newGroupName;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@NewComment";
                param.Value = newComment;
                cmd.Parameters.Add(param);

                if (newParentID != null)
                {
                    param = cmd.CreateParameter();
                    param.ParameterName = "@NewParentID";
                    param.Value = newParentID;
                    cmd.Parameters.Add(param);
                }

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Get count of records with filter
        /// </summary>
        /// <param name="where">where clause</param>
        /// <returns></returns>
        internal Int32 Count(String where)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetListGroupsCount";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@Where";
                param.Value = where;
                cmd.Parameters.Add(param);
                
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// Move computer between groups
        /// </summary>
        /// <param name="compID"></param>
        /// <param name="NewGroupID"></param>
        internal void MoveComputerBetweenGroups(Int16 compID, Int32 NewGroupID)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MoveComputerBetweenGroups";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerID";
                param.Value = compID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@GroupID";
                param.Value = NewGroupID;
                cmd.Parameters.Add(param);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Delete computer from group
        /// </summary>
        /// <param name="compID"></param>
        internal void DeleteComputerFromGroup(Int16 compID)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "RemoveComputerFromGroup";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerID";
                param.Value = compID;
                cmd.Parameters.Add(param);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Add computer into group
        /// </summary>
        /// <param name="compID"></param>
        /// <param name="groupID"></param>
        internal void AddComputerInGroup(Int16 compID, Int32 groupID)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddComputerInGroup";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerID";
                param.Value = compID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@GroupID";
                param.Value = groupID;
                cmd.Parameters.Add(param);
               
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
                        
        /// <summary>
        /// Get all computers by group
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        internal List<String> GetAllComputersNameByGroup(Int32 groupID)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetAllComputersByGroup";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@GroupID";
                param.Value = groupID;
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<String> list = new List<String>();
                while (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                        list.Add(reader.GetString(0));
                }
                reader.Close();
                return list;
            }
        }

        /// <summary>
        /// Get computers by group
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        internal List<ComputersEntity> GetComputersByGroup(Int32 groupID, String where)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetComputersByGroupWithFilter";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@GroupID";
                param.Value = groupID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@Where";
                param.Value = where;
                cmd.Parameters.Add(param);
               
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<ComputersEntity> list = ComputersManager.GetComputersFromReader(reader);
                reader.Close();
                return list;
            }
        }

        /// <summary>
        /// Get computersEx by group
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        internal List<ComputersEntityEx> GetComputersExByGroup(Int32 groupID, String where)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetComputersByGroupWithFilter";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@GroupID";
                param.Value = groupID;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@Where";
                param.Value = where;
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<ComputersEntityEx> list = ComputersManager.GetComputersExFromReader(reader);
                reader.Close();
                return list;
            }
        }

        /// <summary>
        /// Get group types
        /// </summary>
        /// <returns></returns>
        internal List<Group> GetGroups()
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetGroupTypes";

                
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<Group> list = new List<Group>();
                Group gr;
                while (reader.Read())
                {
                    gr = new Group();
                    if (reader.GetValue(0) != DBNull.Value)
                        gr.ID = reader.GetInt32(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        gr.Name = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        gr.Comment = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        gr.ParentID = reader.GetInt32(3);
                    list.Add(gr);
                }
                reader.Close();
                return list;
            }
        }

        /// <summary>
        /// Get subgroups types
        /// </summary>
        /// <returns></returns>
        internal List<Group> GetSubgroups(Int32 groupId)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetSubgroupTypes";



                if (groupId != 0)
                {
                    IDbDataParameter param = cmd.CreateParameter();
                    param.ParameterName = "@ParentID";
                    param.Value = groupId;
                    cmd.Parameters.Add(param);
                }

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<Group> list = new List<Group>();
                Group gr;
                while (reader.Read())
                {
                    gr = new Group();
                    if (reader.GetValue(0) != DBNull.Value)
                        gr.ID = reader.GetInt32(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        gr.Name = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        gr.Comment = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        gr.ParentID = reader.GetInt32(3);
                    list.Add(gr);
                }
                reader.Close();
                return list;
            }
        }

        /// <summary>
        /// Get subgroups types
        /// </summary>
        /// <returns></returns>
        internal List<Group> GetSubgroups(Group? group)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetSubgroupTypes";

                if (group != null)
                {
                    IDbDataParameter param = cmd.CreateParameter();
                    param.ParameterName = "@ParentID";
                    param.Value = ((Group)group).ID;
                    cmd.Parameters.Add(param);
                }
             
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<Group> list = new List<Group>();
                Group gr;
                while (reader.Read())
                {
                    gr = new Group();
                    if (reader.GetValue(0) != DBNull.Value)
                        gr.ID = reader.GetInt32(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        gr.Name = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        gr.Comment = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        gr.ParentID = reader.GetInt32(3);
                    list.Add(gr);
                }
                reader.Close();
                return list;
            }
        }
        
        /// <summary>
        /// Get group types
        /// </summary>
        /// <returns></returns>
        internal List<GroupEx> List(String where, String order, Int32 page, Int32 size)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetListGroups";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@Page";
                param.Value = page;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.ParameterName = "@RowCount";
                param.Value = size;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@OrderBy";
                param.Value=order;
                cmd.Parameters.Add(param);

                param=cmd.CreateParameter();
                param.ParameterName="@Where";
                param.Value=where;
                cmd.Parameters.Add(param);

                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<GroupEx> list = new List<GroupEx>();
                GroupEx gr;
                while (reader.Read())
                {
                    gr = new GroupEx();
                    if (reader.GetValue(0) != DBNull.Value)
                        gr.ID = reader.GetInt32(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        gr.Name = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        gr.Comment = reader.GetString(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        gr.ParentName = reader.GetString(3);
                    if (reader.GetValue(4) != DBNull.Value)
                        gr.TotalCount = reader.GetInt32(4);
                    if (reader.GetValue(5) != DBNull.Value)
                        gr.ActiveCount = reader.GetInt32(5);
                    list.Add(gr);
                }
                reader.Close();
                return list;
            }
        }

        /// <summary>
        /// Get computers without group
        /// </summary>
        /// <returns></returns>
        internal List<ComputersEntity> GetComputersWithoutGroup()
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetComputersWithoutGroupPage";

                con.Open();
                
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<ComputersEntity> list = ComputersManager.GetComputersFromReader(reader);
                reader.Close();

                return list;
            }
        }

        /// <summary>
        /// Get computers without group
        /// </summary>
        /// <returns></returns>
        internal List<ComputersEntity> GetComputersWithoutGroup(String where)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetComputersWithoutGroupPageWithFilter";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@Where";
                param.Value = where;
                cmd.Parameters.Add(param);
               
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<ComputersEntity> list = ComputersManager.GetComputersFromReader(reader);
                reader.Close();

                return list;
            }
        }

        /// <summary>
        /// Get computersEx without group
        /// </summary>
        /// <returns></returns>
        internal List<ComputersEntityEx> GetComputersExWithoutGroup(String where)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetComputersWithoutGroupPageWithFilter";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@Where";
                param.Value = where;
                cmd.Parameters.Add(param);
                
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<ComputersEntityEx> list = ComputersManager.GetComputersExFromReader(reader);
                reader.Close();

                return list;
            }
        }

        /// <summary>
        /// Get count computers without group
        /// </summary>
        /// <returns></returns>
        internal Int32 GetComputersWithoutGroupCount()
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetComputersWithoutGroupPageCount";
             
                con.Open();
               
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// Get computers with groups
        /// </summary>
        /// <returns></returns>
        internal List<ChildParentEntity> GetComputersWithGroups()
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetComputersWithGroupPage";
               
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<ChildParentEntity> list = new List<ChildParentEntity>();
                ChildParentEntity ent;
                while (reader.Read())
                {
                    ent = new ChildParentEntity();
                    if (reader.GetValue(0) != DBNull.Value)
                        ent.ChildID = reader.GetInt16(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        ent.ChildName = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        ent.ParentID = reader.GetInt32(2);

                    list.Add(ent);
                }
                reader.Close();

                return list;
            }
        }

        #endregion

        #region Policy

        /// <summary>
        /// Get computers without group
        /// </summary>
        /// <returns></returns>
        internal List<ComputersEntity> GetComputersByGroupAndPolicy(Group? group, Policy? policy)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetComputersByGroupAndPolicy";

                IDbDataParameter param = cmd.CreateParameter();
                if (group != null){
                param.ParameterName = "@GroupID";
                param.Value = ((Group)group).ID;
                cmd.Parameters.Add(param);
                }
                if (policy != null){
                param = cmd.CreateParameter();
                param.ParameterName = "@PolicyID";
                param.Value = ((Policy)policy).ID;
                cmd.Parameters.Add(param);
                }
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<ComputersEntity> list = ComputersManager.GetComputersFromReader(reader);
                reader.Close();

                return list;
            }
        }

        /// <summary>
        /// Get group list by ComputerID
        /// </summary>
        /// <returns></returns>
        internal List<Group> GetGroupListByComputerID(Int16 compID)
        {
            using (IDbConnection con = factory.CreateConnection())
            {
                con.ConnectionString = connectionString;

                IDbCommand cmd = factory.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetGroupListByComputerID";

                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@ComputerID";
                param.Value = compID;
                cmd.Parameters.Add(param);
               
                con.Open();
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<Group> list = new List<Group>();
                Group ent;
                while (reader.Read())
                {
                    ent = new Group();
                    if (reader.GetValue(0) != DBNull.Value)
                        ent.ID = reader.GetInt32(0);
                    if (reader.GetValue(1) != DBNull.Value)
                        ent.Name = reader.GetString(1);
                    if (reader.GetValue(2) != DBNull.Value)
                        ent.ParentID = reader.GetInt32(2);
                    if (reader.GetValue(3) != DBNull.Value)
                        ent.Comment = reader.GetString(3);
                    list.Add(ent);
                }
                reader.Close();

                return list;
            }
        }

        #endregion

        #endregion
    }
}
