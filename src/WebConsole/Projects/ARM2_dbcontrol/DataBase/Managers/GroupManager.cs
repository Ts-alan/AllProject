using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.DataBase;
using System.Data;
using System.Data.SqlClient;

namespace VirusBlokAda.CC.DataBase
{
    internal sealed class GroupManager
    {
        private readonly String connectionString;
		
		#region Constructors

        public GroupManager(String connectionString)
		{
            this.connectionString = connectionString;
		}

		#endregion

        #region Metods

        #region Administration

        /// <summary>
        /// Add new group to database
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        internal Int32 Add(Group group)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("AddGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GroupName", group.Name);
                cmd.Parameters.AddWithValue("@Comment", group.Comment);
                if (group.ParentID != null)
                cmd.Parameters.AddWithValue("@ParentID", group.ParentID);

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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("RemoveGroupByName", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GroupName", group.Name);

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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GroupName", groupName);
                cmd.Parameters.AddWithValue("@NewGroupName", newGroupName);
                cmd.Parameters.AddWithValue("@NewComment", newComment);
                if (newParentID != null)
                cmd.Parameters.AddWithValue("@NewParentID", newParentID);

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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetListGroupsCount", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Where", where);

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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("MoveComputerBetweenGroups", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ComputerID", compID);
                cmd.Parameters.AddWithValue("@GroupID", NewGroupID);

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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("RemoveComputerFromGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ComputerID", compID);

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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("AddComputerInGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ComputerID", compID);
                cmd.Parameters.AddWithValue("@GroupID", groupID);

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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAllComputersByGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GroupID", groupID);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetComputersByGroupWithFilter", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GroupID", groupID);
                cmd.Parameters.AddWithValue("@Where", where);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetComputersByGroupWithFilter", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GroupID", groupID);
                cmd.Parameters.AddWithValue("@Where", where);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetGroupTypes", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetSubgroupTypes", con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (groupId != 0)
                cmd.Parameters.AddWithValue("@ParentID", groupId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetSubgroupTypes", con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (group != null)
                cmd.Parameters.AddWithValue("@ParentID", ((Group)group).ID);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetListGroups", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Page", page);
                cmd.Parameters.AddWithValue("@RowCount", size);
                cmd.Parameters.AddWithValue("@OrderBy", order);
                cmd.Parameters.AddWithValue("@Where", where);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetComputersWithoutGroupPage", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetComputersWithoutGroupPageWithFilter", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Where", where);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetComputersWithoutGroupPageWithFilter", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Where", where);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetComputersWithoutGroupPageCount", con);
                cmd.CommandType = CommandType.StoredProcedure;

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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetComputersWithGroupPage", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetComputersByGroupAndPolicy", con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (group != null)
                cmd.Parameters.AddWithValue("@GroupID", ((Group)group).ID);
                if (policy != null)
                cmd.Parameters.AddWithValue("@PolicyID", ((Policy)policy).ID);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetGroupListByComputerID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ComputerID", compID);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
