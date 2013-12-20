using System;
using System.Collections.Generic;
using System.Text;
using ARM2_dbcontrol.DataBase;
using System.Data;
using System.Data.SqlClient;
using ARM2_dbcontrol.GroupEntities;
using VirusBlokAda.Vba32CC.Policies.General;

namespace VirusBlokAda.Vba32CC.Groups
{
    public class GroupManager
    {
        VlslVConnection database; 
		
		#region Constructors

		public GroupManager()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public GroupManager(VlslVConnection l_database)
		{
			database=l_database;
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
            IDbCommand cmd = database.CreateCommand("AddGroup", true);
            //cmd.Parameters.Add("@GroupName", SqlDbType.NVarChar).Value = groupName;
            database.AddCommandParameter(cmd, "@GroupName",
                DbType.String, group.Name, ParameterDirection.Input);
            database.AddCommandParameter(cmd, "@Comment",
                DbType.String, group.Comment, ParameterDirection.Input);
            if (group.ParentID != null)
                database.AddCommandParameter(cmd, "@ParentID",
                    DbType.Int32, group.ParentID, ParameterDirection.Input);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        /// <summary>
        /// Remove group
        /// </summary>
        /// <param name="group"></param>
        internal void Delete(Group group)
        {
            IDbCommand cmd = database.CreateCommand("RemoveGroupByName", true);
            database.AddCommandParameter(cmd, "@GroupName", DbType.String, group.Name, ParameterDirection.Input);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Update group
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="newGroupName"></param>
        internal void Update(String groupName, String newGroupName, String newComment, Int32? newParentID)
        {
            IDbCommand cmd = database.CreateCommand("UpdateGroup", true);
            database.AddCommandParameter(cmd, "@GroupName", DbType.String, groupName, ParameterDirection.Input);
            database.AddCommandParameter(cmd, "@NewGroupName", DbType.String, newGroupName, ParameterDirection.Input);
            database.AddCommandParameter(cmd, "@NewComment", DbType.String, newComment, ParameterDirection.Input);
            if (newParentID != null)
                database.AddCommandParameter(cmd, "@NewParentID", DbType.Int32, newParentID, ParameterDirection.Input);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Get count of records with filter
        /// </summary>
        /// <param name="where">where clause</param>
        /// <returns></returns>
        public Int32 Count(String where)
        {
            IDbCommand cmd = database.CreateCommand("GetListGroupsCount", true);
            database.AddCommandParameter(cmd, "@Where", DbType.String, where, ParameterDirection.Input);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        /// <summary>
        /// Move computer between groups
        /// </summary>
        /// <param name="compID"></param>
        /// <param name="NewGroupID"></param>
        internal void MoveComputerBetweenGroups(Int32 compID, Int32 NewGroupID)
        {
            IDbCommand cmd = database.CreateCommand("MoveComputerBetweenGroups", true);
            database.AddCommandParameter(cmd, "@ComputerID", DbType.Int16, Convert.ToInt16(compID), ParameterDirection.Input);
            database.AddCommandParameter(cmd, "@GroupID", DbType.Int32, NewGroupID, ParameterDirection.Input);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Delete computer from group
        /// </summary>
        /// <param name="compID"></param>
        internal void DeleteComputerFromGroup(Int32 compID)
        {
            IDbCommand cmd = database.CreateCommand("RemoveComputerFromGroup", true);
            database.AddCommandParameter(cmd, "@ComputerID", DbType.Int16, Convert.ToInt16(compID), ParameterDirection.Input);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Add computer into group
        /// </summary>
        /// <param name="compID"></param>
        /// <param name="groupID"></param>
        internal void AddComputerInGroup(Int32 compID, Int32 groupID)
        {
            IDbCommand cmd = database.CreateCommand("AddComputerInGroup", true);
            database.AddCommandParameter(cmd, "@ComputerID", DbType.Int16, Convert.ToInt16(compID), ParameterDirection.Input);
            database.AddCommandParameter(cmd, "@GroupID", DbType.Int32, groupID, ParameterDirection.Input);

            cmd.ExecuteNonQuery();
        }

                
        /// <summary>
        /// Get all computers by group
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public List<String> GetAllComputersNameByGroup(Int32 groupID)
        {
            List<String> list = new List<String>();

            IDbCommand cmd = database.CreateCommand("GetAllComputersByGroup", true);
            database.AddCommandParameter(cmd, "@GroupID", DbType.Int32, groupID, ParameterDirection.Input);

            SqlDataReader reader = cmd.ExecuteReader() as SqlDataReader;            
            while (reader.Read())
            {
                if (reader.GetValue(0) != DBNull.Value)
                    list.Add(reader.GetString(0));
            }
            reader.Close();
            return list;
        }

        /// <summary>
        /// Get computers by group
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public List<ComputersEntity> GetComputersByGroup(Int32 groupID)
        {
            List<ComputersEntity> list = new List<ComputersEntity>();

            IDbCommand cmd = database.CreateCommand("GetComputersByGroup", true);
            database.AddCommandParameter(cmd, "@GroupID", DbType.Int32, groupID, ParameterDirection.Input);

            SqlDataReader reader = cmd.ExecuteReader() as SqlDataReader;
            ComputersEntity ent;
            while (reader.Read())
            {
                ent = new ComputersEntity();
                if (reader.GetValue(0) != DBNull.Value)
                    ent.ID = reader.GetInt16(0);
                if (reader.GetValue(1) != DBNull.Value)
                    ent.ComputerName = reader.GetString(1);
                if (reader.GetValue(2) != DBNull.Value)
                    ent.IPAddress = reader.GetString(2);
                if (reader.GetValue(3) != DBNull.Value)
                    ent.ControlCenter = reader.GetBoolean(3);
                if (reader.GetValue(4) != DBNull.Value)
                    ent.DomainName = reader.GetString(4);
                if (reader.GetValue(5) != DBNull.Value)
                    ent.UserLogin = reader.GetString(5);
                if (reader.GetValue(6) != DBNull.Value)
                    ent.OSName = reader.GetString(6);
                if (reader.GetValue(7) != DBNull.Value)
                    ent.RAM = reader.GetInt16(7);
                if (reader.GetValue(8) != DBNull.Value)
                    ent.CPUClock = reader.GetInt16(8);
                if (reader.GetValue(9) != DBNull.Value)
                    ent.RecentActive = reader.GetDateTime(9);
                if (reader.GetValue(10) != DBNull.Value)
                    ent.LatestUpdate = reader.GetDateTime(10);
                if (reader.GetValue(11) != DBNull.Value)
                    ent.Vba32Version = reader.GetString(11);
                if (reader.GetValue(12) != DBNull.Value)
                    ent.LatestInfected = reader.GetDateTime(12);
                if (reader.GetValue(13) != DBNull.Value)
                    ent.LatestMalware = reader.GetString(13);
                if (reader.GetValue(14) != DBNull.Value)
                    ent.Vba32Integrity = reader.GetBoolean(14);
                if (reader.GetValue(15) != DBNull.Value)
                    ent.Vba32KeyValid = reader.GetBoolean(15);
                if (reader.GetValue(16) != DBNull.Value)
                    ent.Description = reader.GetString(16);
                if (reader.GetValue(17) != DBNull.Value)
                    ent.AdditionalInfo.ControlDeviceType = ControlDeviceTypeEnumExtensions.Get(reader.GetString(17));

                list.Add(ent);
            }
            reader.Close();
            return list;
        }

        /// <summary>
        /// Get computers by group
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public List<ComputersEntity> GetComputersByGroup(Int32 groupID, String where)
        {
            List<ComputersEntity> list = new List<ComputersEntity>();

            IDbCommand cmd = database.CreateCommand("GetComputersByGroupWithFilter", true);
            database.AddCommandParameter(cmd, "@GroupID", DbType.Int32, groupID, ParameterDirection.Input);
            database.AddCommandParameter(cmd, "@Where", DbType.String, where, ParameterDirection.Input);

            SqlDataReader reader = cmd.ExecuteReader() as SqlDataReader;
            ComputersEntity ent;
            while (reader.Read())
            {
                ent = new ComputersEntity();
                if (reader.GetValue(0) != DBNull.Value)
                    ent.ID = reader.GetInt16(0);
                if (reader.GetValue(1) != DBNull.Value)
                    ent.ComputerName = reader.GetString(1);
                if (reader.GetValue(2) != DBNull.Value)
                    ent.IPAddress = reader.GetString(2);
                if (reader.GetValue(3) != DBNull.Value)
                    ent.ControlCenter = reader.GetBoolean(3);
                if (reader.GetValue(4) != DBNull.Value)
                    ent.DomainName = reader.GetString(4);
                if (reader.GetValue(5) != DBNull.Value)
                    ent.UserLogin = reader.GetString(5);
                if (reader.GetValue(6) != DBNull.Value)
                    ent.OSName = reader.GetString(6);
                if (reader.GetValue(7) != DBNull.Value)
                    ent.RAM = reader.GetInt16(7);
                if (reader.GetValue(8) != DBNull.Value)
                    ent.CPUClock = reader.GetInt16(8);
                if (reader.GetValue(9) != DBNull.Value)
                    ent.RecentActive = reader.GetDateTime(9);
                if (reader.GetValue(10) != DBNull.Value)
                    ent.LatestUpdate = reader.GetDateTime(10);
                if (reader.GetValue(11) != DBNull.Value)
                    ent.Vba32Version = reader.GetString(11);
                if (reader.GetValue(12) != DBNull.Value)
                    ent.LatestInfected = reader.GetDateTime(12);
                if (reader.GetValue(13) != DBNull.Value)
                    ent.LatestMalware = reader.GetString(13);
                if (reader.GetValue(14) != DBNull.Value)
                    ent.Vba32Integrity = reader.GetBoolean(14);
                if (reader.GetValue(15) != DBNull.Value)
                    ent.Vba32KeyValid = reader.GetBoolean(15);
                if (reader.GetValue(16) != DBNull.Value)
                    ent.Description = reader.GetString(16);

                list.Add(ent);
            }
            reader.Close();
            return list;
        }

        /// <summary>
        /// Get computersEx by group
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public List<ComputersEntityEx> GetComputersExByGroup(Int32 groupID, String where)
        {
            List<ComputersEntityEx> list = new List<ComputersEntityEx>();

            IDbCommand cmd = database.CreateCommand("GetComputersExByGroupWithFilter", true);
            database.AddCommandParameter(cmd, "@GroupID", DbType.Int32, groupID, ParameterDirection.Input);
            database.AddCommandParameter(cmd, "@Where", DbType.String, where, ParameterDirection.Input);

            SqlDataReader reader = cmd.ExecuteReader() as SqlDataReader;
            ComputersEntityEx ent;
            while (reader.Read())
            {
                ent = new ComputersEntityEx();
                if (reader.GetValue(0) != DBNull.Value)
                    ent.ID = reader.GetInt16(0);
                if (reader.GetValue(1) != DBNull.Value)
                    ent.ComputerName = reader.GetString(1);
                if (reader.GetValue(2) != DBNull.Value)
                    ent.IPAddress = reader.GetString(2);
                if (reader.GetValue(3) != DBNull.Value)
                    ent.ControlCenter = reader.GetBoolean(3);
                if (reader.GetValue(4) != DBNull.Value)
                    ent.DomainName = reader.GetString(4);
                if (reader.GetValue(5) != DBNull.Value)
                    ent.UserLogin = reader.GetString(5);
                if (reader.GetValue(6) != DBNull.Value)
                    ent.OSName = reader.GetString(6);
                if (reader.GetValue(7) != DBNull.Value)
                    ent.RAM = reader.GetInt16(7);
                if (reader.GetValue(8) != DBNull.Value)
                    ent.CPUClock = reader.GetInt16(8);
                if (reader.GetValue(9) != DBNull.Value)
                    ent.RecentActive = reader.GetDateTime(9);
                if (reader.GetValue(10) != DBNull.Value)
                    ent.LatestUpdate = reader.GetDateTime(10);
                if (reader.GetValue(11) != DBNull.Value)
                    ent.Vba32Version = reader.GetString(11);
                if (reader.GetValue(12) != DBNull.Value)
                    ent.LatestInfected = reader.GetDateTime(12);
                if (reader.GetValue(13) != DBNull.Value)
                    ent.LatestMalware = reader.GetString(13);
                if (reader.GetValue(14) != DBNull.Value)
                    ent.Vba32Integrity = reader.GetBoolean(14);
                if (reader.GetValue(15) != DBNull.Value)
                    ent.Vba32KeyValid = reader.GetBoolean(15);
                if (reader.GetValue(16) != DBNull.Value)
                    ent.Description = reader.GetString(16);
                if (reader.GetValue(17) != DBNull.Value)
                    ent.PolicyName = reader.GetString(17);

                list.Add(ent);
            }
            reader.Close();
            return list;
        }

        /// <summary>
        /// Get group types
        /// </summary>
        /// <returns></returns>
        internal List<Group> GetGroups()
        {
            List<Group> list = new List<Group>();

            IDbCommand cmd = database.CreateCommand("GetGroupTypes", true);

            SqlDataReader reader = cmd.ExecuteReader() as SqlDataReader;
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


        /// <summary>
        /// Get subgroups types
        /// </summary>
        /// <returns></returns>
        internal List<Group> GetSubgroups(int groupId)
        {
            List<Group> list = new List<Group>();

            IDbCommand cmd = database.CreateCommand("GetSubgroupTypes", true);
            if (groupId != 0)
                database.AddCommandParameter(cmd, "@ParentID", DbType.Int32, groupId, ParameterDirection.Input);

            SqlDataReader reader = cmd.ExecuteReader() as SqlDataReader;
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



        /// <summary>
        /// Get subgroups types
        /// </summary>
        /// <returns></returns>
        internal List<Group> GetSubgroups(Group? group)
        {
            List<Group> list = new List<Group>();

            IDbCommand cmd = database.CreateCommand("GetSubgroupTypes", true);
            if (group != null)
                database.AddCommandParameter(cmd, "@ParentID", DbType.Int32, ((Group)group).ID, ParameterDirection.Input);

            SqlDataReader reader = cmd.ExecuteReader() as SqlDataReader;
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
        /// <summary>
        /// Get group types
        /// </summary>
        /// <returns></returns>
        public List<GroupEx> List(String where, String order, Int32 page, Int32 size)
        {
            List<GroupEx> list = new List<GroupEx>();

            IDbCommand cmd = database.CreateCommand("GetListGroups", true);
            database.AddCommandParameter(cmd, "@Page", DbType.Int32, page, ParameterDirection.Input);
            database.AddCommandParameter(cmd, "@RowCount", DbType.Int32, size, ParameterDirection.Input);
            database.AddCommandParameter(cmd, "@OrderBy", DbType.String, order, ParameterDirection.Input);
            database.AddCommandParameter(cmd, "@Where", DbType.String, where, ParameterDirection.Input);

            SqlDataReader reader = cmd.ExecuteReader() as SqlDataReader;
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

        /// <summary>
        /// Get computers without group
        /// </summary>
        /// <returns></returns>
        public List<ComputersEntity> GetComputersWithoutGroup()
        {
            List<ComputersEntity> list = new List<ComputersEntity>();
            IDbCommand cmd = database.CreateCommand("GetComputersWithoutGroupPage", true);

            ComputersEntity ent;
            SqlDataReader reader = cmd.ExecuteReader() as SqlDataReader;
            while (reader.Read())
            {
                ent = new ComputersEntity();
                if (reader.GetValue(0) != DBNull.Value)
                    ent.ID = reader.GetInt16(0);
                if (reader.GetValue(1) != DBNull.Value)
                    ent.ComputerName = reader.GetString(1);
                if (reader.GetValue(2) != DBNull.Value)
                    ent.IPAddress = reader.GetString(2);
                if (reader.GetValue(3) != DBNull.Value)
                    ent.ControlCenter = reader.GetBoolean(3);
                if (reader.GetValue(4) != DBNull.Value)
                    ent.DomainName = reader.GetString(4);
                if (reader.GetValue(5) != DBNull.Value)
                    ent.UserLogin = reader.GetString(5);
                if (reader.GetValue(6) != DBNull.Value)
                    ent.OSName = reader.GetString(6);
                if (reader.GetValue(7) != DBNull.Value)
                    ent.RAM = reader.GetInt16(7);
                if (reader.GetValue(8) != DBNull.Value)
                    ent.CPUClock = reader.GetInt16(8);
                if (reader.GetValue(9) != DBNull.Value)
                    ent.RecentActive = reader.GetDateTime(9);
                if (reader.GetValue(10) != DBNull.Value)
                    ent.LatestUpdate = reader.GetDateTime(10);
                if (reader.GetValue(11) != DBNull.Value)
                    ent.Vba32Version = reader.GetString(11);
                if (reader.GetValue(12) != DBNull.Value)
                    ent.LatestInfected = reader.GetDateTime(12);
                if (reader.GetValue(13) != DBNull.Value)
                    ent.LatestMalware = reader.GetString(13);
                if (reader.GetValue(14) != DBNull.Value)
                    ent.Vba32Integrity = reader.GetBoolean(14);
                if (reader.GetValue(15) != DBNull.Value)
                    ent.Vba32KeyValid = reader.GetBoolean(15);
                if (reader.GetValue(16) != DBNull.Value)
                    ent.Description = reader.GetString(16);
                list.Add(ent);
            }
            reader.Close();

            return list;
        }

        /// <summary>
        /// Get computers without group
        /// </summary>
        /// <returns></returns>
        public List<ComputersEntity> GetComputersWithoutGroup(String where)
        {
            List<ComputersEntity> list = new List<ComputersEntity>();
            IDbCommand cmd = database.CreateCommand("GetComputersWithoutGroupPageWithFilter", true);
            database.AddCommandParameter(cmd, "@Where", DbType.String, where, ParameterDirection.Input);

            ComputersEntity ent;
            SqlDataReader reader = cmd.ExecuteReader() as SqlDataReader;
            while (reader.Read())
            {
                ent = new ComputersEntity();
                if (reader.GetValue(0) != DBNull.Value)
                    ent.ID = reader.GetInt16(0);
                if (reader.GetValue(1) != DBNull.Value)
                    ent.ComputerName = reader.GetString(1);
                if (reader.GetValue(2) != DBNull.Value)
                    ent.IPAddress = reader.GetString(2);
                if (reader.GetValue(3) != DBNull.Value)
                    ent.ControlCenter = reader.GetBoolean(3);
                if (reader.GetValue(4) != DBNull.Value)
                    ent.DomainName = reader.GetString(4);
                if (reader.GetValue(5) != DBNull.Value)
                    ent.UserLogin = reader.GetString(5);
                if (reader.GetValue(6) != DBNull.Value)
                    ent.OSName = reader.GetString(6);
                if (reader.GetValue(7) != DBNull.Value)
                    ent.RAM = reader.GetInt16(7);
                if (reader.GetValue(8) != DBNull.Value)
                    ent.CPUClock = reader.GetInt16(8);
                if (reader.GetValue(9) != DBNull.Value)
                    ent.RecentActive = reader.GetDateTime(9);
                if (reader.GetValue(10) != DBNull.Value)
                    ent.LatestUpdate = reader.GetDateTime(10);
                if (reader.GetValue(11) != DBNull.Value)
                    ent.Vba32Version = reader.GetString(11);
                if (reader.GetValue(12) != DBNull.Value)
                    ent.LatestInfected = reader.GetDateTime(12);
                if (reader.GetValue(13) != DBNull.Value)
                    ent.LatestMalware = reader.GetString(13);
                if (reader.GetValue(14) != DBNull.Value)
                    ent.Vba32Integrity = reader.GetBoolean(14);
                if (reader.GetValue(15) != DBNull.Value)
                    ent.Vba32KeyValid = reader.GetBoolean(15);
                if (reader.GetValue(16) != DBNull.Value)
                    ent.Description = reader.GetString(16);
                list.Add(ent);
            }
            reader.Close();

            return list;
        }

        /// <summary>
        /// Get computersEx without group
        /// </summary>
        /// <returns></returns>
        public List<ComputersEntityEx> GetComputersExWithoutGroup(String where)
        {
            List<ComputersEntityEx> list = new List<ComputersEntityEx>();
            IDbCommand cmd = database.CreateCommand("GetComputersExWithoutGroupPageWithFilter", true);
            database.AddCommandParameter(cmd, "@Where", DbType.String, where, ParameterDirection.Input);

            ComputersEntityEx ent;
            SqlDataReader reader = cmd.ExecuteReader() as SqlDataReader;
            while (reader.Read())
            {
                ent = new ComputersEntityEx();
                if (reader.GetValue(0) != DBNull.Value)
                    ent.ID = reader.GetInt16(0);
                if (reader.GetValue(1) != DBNull.Value)
                    ent.ComputerName = reader.GetString(1);
                if (reader.GetValue(2) != DBNull.Value)
                    ent.IPAddress = reader.GetString(2);
                if (reader.GetValue(3) != DBNull.Value)
                    ent.ControlCenter = reader.GetBoolean(3);
                if (reader.GetValue(4) != DBNull.Value)
                    ent.DomainName = reader.GetString(4);
                if (reader.GetValue(5) != DBNull.Value)
                    ent.UserLogin = reader.GetString(5);
                if (reader.GetValue(6) != DBNull.Value)
                    ent.OSName = reader.GetString(6);
                if (reader.GetValue(7) != DBNull.Value)
                    ent.RAM = reader.GetInt16(7);
                if (reader.GetValue(8) != DBNull.Value)
                    ent.CPUClock = reader.GetInt16(8);
                if (reader.GetValue(9) != DBNull.Value)
                    ent.RecentActive = reader.GetDateTime(9);
                if (reader.GetValue(10) != DBNull.Value)
                    ent.LatestUpdate = reader.GetDateTime(10);
                if (reader.GetValue(11) != DBNull.Value)
                    ent.Vba32Version = reader.GetString(11);
                if (reader.GetValue(12) != DBNull.Value)
                    ent.LatestInfected = reader.GetDateTime(12);
                if (reader.GetValue(13) != DBNull.Value)
                    ent.LatestMalware = reader.GetString(13);
                if (reader.GetValue(14) != DBNull.Value)
                    ent.Vba32Integrity = reader.GetBoolean(14);
                if (reader.GetValue(15) != DBNull.Value)
                    ent.Vba32KeyValid = reader.GetBoolean(15);
                if (reader.GetValue(16) != DBNull.Value)
                    ent.Description = reader.GetString(16);
                if (reader.GetValue(17) != DBNull.Value)
                    ent.PolicyName = reader.GetString(17);
                list.Add(ent);
            }
            reader.Close();

            return list;
        }

        /// <summary>
        /// Get count computers without group
        /// </summary>
        /// <returns></returns>
        internal Int32 GetComputersWithoutGroupCount()
        {
            IDbCommand cmd = database.CreateCommand("GetComputersWithoutGroupPageCount", true);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        /// <summary>
        /// Get computers with groups
        /// </summary>
        /// <returns></returns>
        internal List<ChildParentEntity> GetComputersWithGroups()
        {
            List<ChildParentEntity> list = new List<ChildParentEntity>();
            IDbCommand cmd = database.CreateCommand("GetComputersWithGroupPage", true);

            SqlDataReader reader = cmd.ExecuteReader() as SqlDataReader;
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

        #endregion

        #region Policy

        /// <summary>
        /// Get computers without group
        /// </summary>
        /// <returns></returns>
        public List<ComputersEntity> GetComputersByGroupAndPolicy(Group? group, Policy? policy)
        {
            List<ComputersEntity> list = new List<ComputersEntity>();
            IDbCommand cmd = database.CreateCommand("GetComputersByGroupAndPolicy", true);
            if (group != null)
                database.AddCommandParameter(cmd, "@GroupID", DbType.Int32, ((Group)group).ID, ParameterDirection.Input);
            if (policy != null)
                database.AddCommandParameter(cmd, "@PolicyID", DbType.Int16, ((Policy)policy).ID, ParameterDirection.Input);

            ComputersEntity ent;
            SqlDataReader reader = cmd.ExecuteReader() as SqlDataReader;
            while (reader.Read())
            {
                ent = new ComputersEntity();
                if (reader.GetValue(0) != DBNull.Value)
                    ent.ID = reader.GetInt16(0);
                if (reader.GetValue(1) != DBNull.Value)
                    ent.ComputerName = reader.GetString(1);
                if (reader.GetValue(2) != DBNull.Value)
                    ent.IPAddress = reader.GetString(2);
                if (reader.GetValue(3) != DBNull.Value)
                    ent.ControlCenter = reader.GetBoolean(3);
                if (reader.GetValue(4) != DBNull.Value)
                    ent.DomainName = reader.GetString(4);
                if (reader.GetValue(5) != DBNull.Value)
                    ent.UserLogin = reader.GetString(5);
                if (reader.GetValue(6) != DBNull.Value)
                    ent.OSName = reader.GetString(6);
                if (reader.GetValue(7) != DBNull.Value)
                    ent.RAM = reader.GetInt16(7);
                if (reader.GetValue(8) != DBNull.Value)
                    ent.CPUClock = reader.GetInt16(8);
                if (reader.GetValue(9) != DBNull.Value)
                    ent.RecentActive = reader.GetDateTime(9);
                if (reader.GetValue(10) != DBNull.Value)
                    ent.LatestUpdate = reader.GetDateTime(10);
                if (reader.GetValue(11) != DBNull.Value)
                    ent.Vba32Version = reader.GetString(11);
                if (reader.GetValue(12) != DBNull.Value)
                    ent.LatestInfected = reader.GetDateTime(12);
                if (reader.GetValue(13) != DBNull.Value)
                    ent.LatestMalware = reader.GetString(13);
                if (reader.GetValue(14) != DBNull.Value)
                    ent.Vba32Integrity = reader.GetBoolean(14);
                if (reader.GetValue(15) != DBNull.Value)
                    ent.Vba32KeyValid = reader.GetBoolean(15);
                if (reader.GetValue(16) != DBNull.Value)
                    ent.Description = reader.GetString(16);
                list.Add(ent);
            }
            reader.Close();

            return list;
        }


        /// <summary>
        /// Get group list by ComputerID
        /// </summary>
        /// <returns></returns>
        public List<Group> GetGroupListByComputerID(Int16 compID)
        {
            List<Group> list = new List<Group>();
            IDbCommand cmd = database.CreateCommand("GetGroupListByComputerID", true);
            database.AddCommandParameter(cmd, "@ComputerID", DbType.Int16, compID, ParameterDirection.Input);

            Group ent;
            SqlDataReader reader = cmd.ExecuteReader() as SqlDataReader;
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

        #endregion

        #endregion
    }
}
