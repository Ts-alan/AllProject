using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace VirusBlokAda.CC.DataBase
{
    public class GroupProvider
    {
        public const String ProviderName = "GroupProvider";

        private GroupManager groupMngr;
        private ComponentsManager cmptMngr;
        private ComputersManager compMngr;

        #region Constructors
        public GroupProvider(String connectionString)
        {
            InitManagers(connectionString);
        }

        private void InitManagers(String connectionString)
        {
            groupMngr = new GroupManager(connectionString);
            cmptMngr = new ComponentsManager(connectionString);
            compMngr = new ComputersManager(connectionString);
        }

        public GroupProvider(String connectionString,DbProviderFactory factory)
        {
            InitManagers(connectionString,factory);
        }

        private void InitManagers(String connectionString,DbProviderFactory factory)
        {
            groupMngr = new GroupManager(connectionString,factory);
            cmptMngr = new ComponentsManager(connectionString,factory);
            compMngr = new ComputersManager(connectionString,factory);
        }

        public GroupProvider(String connectionString,String factoryName)
        {
            InitManagers(connectionString,factoryName);
        }

        private void InitManagers(String connectionString,String factoryName)
        {
            groupMngr = new GroupManager(connectionString,factoryName);
            cmptMngr = new ComponentsManager(connectionString,factoryName);
            compMngr = new ComputersManager(connectionString,factoryName);
        }
        #endregion
        #region Methods

        #region Administration
        /// <summary>
        /// Add group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public Int32 Add(Group group)
        {
            return groupMngr.Add(group);
        }

        /// <summary>
        /// Delete group
        /// </summary>
        /// <param name="group"></param>
        public void Delete(Group group)
        {
            groupMngr.Delete(group);
        }

        /// <summary>
        /// Rename group
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="newGroupName"></param>
        public void Update(String groupName, String newGroupName, String newComment, Int32? newParentID)
        {
            groupMngr.Update(groupName, newGroupName, newComment, newParentID);
        }

        /// <summary>
        /// Move computer between groups
        /// </summary>
        /// <param name="compID"></param>
        /// <param name="NewGroupID"></param>
        public void MoveComputerBetweenGroups(Int16 compID, Int32 NewGroupID)
        {
            groupMngr.MoveComputerBetweenGroups(compID, NewGroupID);
        }

        /// <summary>
        /// Delete computer from group
        /// </summary>
        /// <param name="compID"></param>
        public void DeleteComputerFromGroup(Int16 compID)
        {
            groupMngr.DeleteComputerFromGroup(compID);
        }

        /// <summary>
        ///  Add computer into group
        /// </summary>
        /// <param name="compID"></param>
        /// <param name="groupID"></param>
        public void AddComputerInGroup(Int16 compID, Int32 groupID)
        {
            groupMngr.AddComputerInGroup(compID, groupID);
        }

        /// <summary>
        /// Get computers by group
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public List<ComputersEntity> GetComputersByGroup(Int32 groupID)
        {
            return compMngr.GetComputers(String.Format("GroupID = {0}", groupID.ToString()), null);
        }

        /// <summary>
        /// Get group types
        /// </summary>
        /// <returns></returns>
        public List<Group> GetGroups()
        {
            return groupMngr.GetGroups();
        }

        /// <summary>
        /// Get Subgroup types
        /// </summary>
        /// <returns></returns>
        public List<Group> GetSubgroups(Group? group)
        {
            return groupMngr.GetSubgroups(group);
        }


        /// <summary>
        /// Get Subgroup types
        /// </summary>
        /// <returns></returns>
        public List<Group> GetSubgroups(int groupId)
        {
            return groupMngr.GetSubgroups(groupId);
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
            return groupMngr.List(where, order, page, size);
        }

        /// <summary>
        /// Get count of records with filter
        /// </summary>
        /// <param name="where">where clause</param>
        /// <returns></returns>
        public Int32 Count(String where)
        {
            return groupMngr.Count(where);
        }

        /// <summary>
        /// Get computers without group
        /// </summary>
        /// <returns></returns>
        public List<ComputersEntity> GetComputersWithoutGroup()
        {
            return groupMngr.GetComputersWithoutGroup();
        }

        /// <summary>
        /// Get computers count without group
        /// </summary>
        /// <param name="where">where clause</param>
        /// <returns></returns>
        public Int32 GetComputersWithoutGroupCount()
        {
            return groupMngr.GetComputersWithoutGroupCount();
        }

        /// <summary>
        /// Get computers with groups
        /// </summary>
        /// <returns></returns>
        public List<ChildParentEntity> GetComputersWithGroups()
        {
            return groupMngr.GetComputersWithGroups();
        }

        /// <summary>
        /// Get computersEx without group
        /// </summary>
        /// <returns></returns>
        public List<ComputersEntityEx> GetComputersExWithoutGroup()
        {
            List<ComputersEntityEx> list = new List<ComputersEntityEx>();

            foreach (ComputersEntity comp in groupMngr.GetComputersWithoutGroup())
            {
                list.Add(new ComputersEntityEx(comp, cmptMngr.GetComponentsPageByComputerID(comp.ID)));
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
            
            foreach (ComputersEntityEx comp in groupMngr.GetComputersExWithoutGroup(where))
            {
                list.Add(new ComputersEntityEx(comp, comp.Group, comp.Policy, cmptMngr.GetComponentsPageByComputerID(comp.ID)));
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
            
            foreach (ComputersEntity comp in compMngr.GetComputers(String.Format("GroupID = {0}", groupID), null))
            {
                list.Add(new ComputersEntityEx(comp, cmptMngr.GetComponentsPageByComputerID(comp.ID)));
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

            foreach (ComputersEntityEx comp in groupMngr.GetComputersExByGroup(groupID, where))
            {
                list.Add(new ComputersEntityEx(comp, comp.Group, comp.Policy, cmptMngr.GetComponentsPageByComputerID(comp.ID)));
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
            return groupMngr.GetComputersByGroupAndPolicy(group, policy);
        }

        /// <summary>
        /// Get group list by computerID
        /// </summary>
        /// <returns></returns>
        public List<Group> GetGroupListByComputerID(Int16 computerID)
        {
            return groupMngr.GetGroupListByComputerID(computerID);
        }

        #endregion

        #endregion
    }
}
