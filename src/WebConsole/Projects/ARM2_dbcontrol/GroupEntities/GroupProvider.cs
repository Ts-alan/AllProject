using System;
using System.Collections.Generic;
using System.Text;
using ARM2_dbcontrol.DataBase;
using ARM2_dbcontrol.GroupEntities;
using VirusBlokAda.Vba32CC.Policies.General;

namespace VirusBlokAda.Vba32CC.Groups
{
    public class GroupProvider
    {
        private readonly String connectionString;
        private GroupManager groupMngr;

        public GroupProvider()
        { }

        public GroupProvider(String connectionString)
        {
            this.connectionString = connectionString;
        }

        #region Methods

        #region Administration
        /// <summary>
        /// Add group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public Int32 Add(Group group)
        {
            Int32 id = Int32.MinValue;
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                groupMngr = new GroupManager(conn);
                conn.OpenConnection();

                id = groupMngr.Add(group);

                conn.CloseConnection();
            }

            return id;
        }

        /// <summary>
        /// Delete group
        /// </summary>
        /// <param name="group"></param>
        public void Delete(Group group)
        {
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                groupMngr = new GroupManager(conn);
                conn.OpenConnection();

                groupMngr.Delete(group);

                conn.CloseConnection();
            }
        }

        /// <summary>
        /// Rename group
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="newGroupName"></param>
        public void Update(String groupName, String newGroupName, String newComment, Int32? newParentID)
        {
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                groupMngr = new GroupManager(conn);
                conn.OpenConnection();

                groupMngr.Update(groupName, newGroupName, newComment, newParentID);

                conn.CloseConnection();
            }
        }

        /// <summary>
        /// Move computer between groups
        /// </summary>
        /// <param name="compID"></param>
        /// <param name="NewGroupID"></param>
        public void MoveComputerBetweenGroups(Int32 compID, Int32 NewGroupID)
        {
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                groupMngr = new GroupManager(conn);
                conn.OpenConnection();

                groupMngr.MoveComputerBetweenGroups(compID, NewGroupID);

                conn.CloseConnection();
            }
        }

        /// <summary>
        /// Delete computer from group
        /// </summary>
        /// <param name="compID"></param>
        public void DeleteComputerFromGroup(Int32 compID)
        {
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                groupMngr = new GroupManager(conn);
                conn.OpenConnection();

                groupMngr.DeleteComputerFromGroup(compID);

                conn.CloseConnection();
            }
        }

        /// <summary>
        ///  Add computer into group
        /// </summary>
        /// <param name="compID"></param>
        /// <param name="groupID"></param>
        public void AddComputerInGroup(Int32 compID, Int32 groupID)
        {
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                groupMngr = new GroupManager(conn);
                conn.OpenConnection();

                groupMngr.AddComputerInGroup(compID, groupID);

                conn.CloseConnection();
            }
        }

        /// <summary>
        /// Get computers by group
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public List<ComputersEntity> GetComputersByGroup(Int32 groupID)
        {
            List<ComputersEntity> list = new List<ComputersEntity>();
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                groupMngr = new GroupManager(conn);
                conn.OpenConnection();

                list = groupMngr.GetComputersByGroup(groupID);

                conn.CloseConnection();
            }

            return list;
        }

        /// <summary>
        /// Get group types
        /// </summary>
        /// <returns></returns>
        public List<Group> GetGroups()
        {
            List<Group> list = new List<Group>();
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                groupMngr = new GroupManager(conn);
                conn.OpenConnection();

                list = groupMngr.GetGroups();

                conn.CloseConnection();
            }

            return list;
        }


        /// <summary>
        /// List
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<GroupEx> List(String where, String order, Int32 page, Int32 size)
        {
            List<GroupEx> list = new List<GroupEx>();

            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                groupMngr = new GroupManager(conn);
                conn.OpenConnection();

                list = groupMngr.List(where, order, page, size);

                conn.CloseConnection();
            }

            return list;
        }

        /// <summary>
        /// Get count of records with filter
        /// </summary>
        /// <param name="where">where clause</param>
        /// <returns></returns>
        public Int32 Count(String where)
        {
            Int32 count = 0;
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                groupMngr = new GroupManager(conn);
                conn.OpenConnection();

                count = groupMngr.Count(where);

                conn.CloseConnection();
            }

            return count;
        }

        /// <summary>
        /// Get computers without group
        /// </summary>
        /// <returns></returns>
        public List<ComputersEntity> GetComputersWithoutGroup()
        {
            List<ComputersEntity> list = new List<ComputersEntity>();
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                groupMngr = new GroupManager(conn);
                conn.OpenConnection();

                list = groupMngr.GetComputersWithoutGroup();

                conn.CloseConnection();
            }

            return list;
        }

        /// <summary>
        /// Get computers count without group
        /// </summary>
        /// <param name="where">where clause</param>
        /// <returns></returns>
        public Int32 GetComputersWithoutGroupCount()
        {
            Int32 count = 0;
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                groupMngr = new GroupManager(conn);
                conn.OpenConnection();

                count = groupMngr.GetComputersWithoutGroupCount();

                conn.CloseConnection();
            }

            return count;
        }

        /// <summary>
        /// Get computers with groups
        /// </summary>
        /// <returns></returns>
        public List<ChildParentEntity> GetComputersWithGroups()
        {
            List<ChildParentEntity> list = new List<ChildParentEntity>();
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                groupMngr = new GroupManager(conn);
                conn.OpenConnection();

                list = groupMngr.GetComputersWithGroups();

                conn.CloseConnection();
            }

            return list;
        }

        /// <summary>
        /// Get computersEx without group
        /// </summary>
        /// <returns></returns>
        public List<ComputersEntityEx> GetComputersExWithoutGroup()
        {
            List<ComputersEntityEx> list = new List<ComputersEntityEx>();
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                groupMngr = new GroupManager(conn);
                ComponentsManager cmptMngr = new ComponentsManager(conn);
                conn.OpenConnection();

                foreach (ComputersEntity comp in groupMngr.GetComputersWithoutGroup())
                {
                    list.Add(new ComputersEntityEx(comp, cmptMngr.GetComponentsPageByComputerID(comp.ID)));
                }

                conn.CloseConnection();
            }

            return list;
        }

        /// <summary>
        /// Get computersEx without group
        /// </summary>
        /// <returns></returns>
        public List<ComputersEntityEx> GetComputersExWithoutGroup(String where)
        {
            List<ComputersEntityEx> list = new List<ComputersEntityEx>();
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                groupMngr = new GroupManager(conn);
                ComponentsManager cmptMngr = new ComponentsManager(conn);
                conn.OpenConnection();

                foreach (ComputersEntityEx comp in groupMngr.GetComputersExWithoutGroup(where))
                {
                    list.Add(new ComputersEntityEx(comp, comp.PolicyName, cmptMngr.GetComponentsPageByComputerID(comp.ID)));
                }

                conn.CloseConnection();
            }

            return list;
        }

        /// <summary>
        /// Get computersEx by group
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public List<ComputersEntityEx> GetComputersExByGroup(Int32 groupID)
        {
            List<ComputersEntityEx> list = new List<ComputersEntityEx>();
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                groupMngr = new GroupManager(conn);
                ComponentsManager cmptMngr = new ComponentsManager(conn);
                conn.OpenConnection();

                foreach (ComputersEntity comp in groupMngr.GetComputersByGroup(groupID))
                {
                    list.Add(new ComputersEntityEx(comp, cmptMngr.GetComponentsPageByComputerID(comp.ID)));
                }

                conn.CloseConnection();
            }

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
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                groupMngr = new GroupManager(conn);
                ComponentsManager cmptMngr = new ComponentsManager(conn);
                conn.OpenConnection();

                foreach (ComputersEntityEx comp in groupMngr.GetComputersExByGroup(groupID, where))
                {
                    list.Add(new ComputersEntityEx(comp, comp.PolicyName, cmptMngr.GetComponentsPageByComputerID(comp.ID)));
                }

                conn.CloseConnection();
            }

            return list;
        }

        #endregion

        #region Policy

        /// <summary>
        /// Get computers by group and policy
        /// </summary>
        /// <returns></returns>
        public List<ComputersEntity> GetComputersByGroupAndPolicy(Group? group, Policy? policy)
        {
            List<ComputersEntity> list = new List<ComputersEntity>();
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                groupMngr = new GroupManager(conn);
                conn.OpenConnection();

                list = groupMngr.GetComputersByGroupAndPolicy(group, policy);

                conn.CloseConnection();
            }

            return list;
        }

        /// <summary>
        /// Get group list by computerID
        /// </summary>
        /// <returns></returns>
        public List<Group> GetGroupListByComputerID(Int16 computerID)
        {
            List<Group> list = new List<Group>();
            using (VlslVConnection conn = new VlslVConnection(connectionString))
            {
                groupMngr = new GroupManager(conn);
                conn.OpenConnection();

                list = groupMngr.GetGroupListByComputerID(computerID);

                conn.CloseConnection();
            }

            return list;
        }

        #endregion

        #endregion
    }
}
