using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.Vba32CC.Policies.General;
using ARM2_dbcontrol.DataBase;

namespace VirusBlokAda.Vba32CC.Groups
{
    public class PolicyBranchOfTree
    {
        #region Properties

        private Policy _rootPolicy;
        public Policy RootPolicy
        {
            get { return _rootPolicy; }
            set { _rootPolicy = value; }
        }

        private List<BranchOfTree> _branchs;
        public List<BranchOfTree> Branches
        {
            get { return _branchs; }
        }

        private List<ComputersEntity> _computers;
        public List<ComputersEntity> Computers
        {
            get { return _computers; }
        }

        #endregion

        #region Constructors

        public PolicyBranchOfTree()
            : this(new Policy(), new List<BranchOfTree>(), new List<ComputersEntity>())
        { }

        public PolicyBranchOfTree(Policy rootPolicy, List<BranchOfTree> branchs, List<ComputersEntity> computers)
        {
            _rootPolicy = rootPolicy;
            _branchs = branchs;
            _computers = computers;
        }

        public PolicyBranchOfTree(Policy rootPolicy)
            : this(rootPolicy, new List<BranchOfTree>(), new List<ComputersEntity>())
        { }

        #endregion

        #region Methods

        public void AddComputer(ComputersEntity comp)
        {
            if (!this._computers.Contains(comp))
                _computers.Add(comp);
        }

        public void AddComputers(List<ComputersEntity> comps)
        {
            foreach (ComputersEntity comp in comps)
            {
                AddComputer(comp);
            }
        }

        public void AddBranch(BranchOfTree branch)
        {
            BranchOfTree result = GetBranchByRoot(branch.Root);
            if (result != null)
                result.AddBranch(branch);
            else _branchs.Add(branch);
        }

        public void AddBranch(List<BranchOfTree> branches)
        {
            foreach (BranchOfTree branch in branches)
            {
                AddBranch(branch);
            }
        }

        private BranchOfTree GetBranchByRoot(String rootName)
        {
            BranchOfTree result;
            foreach (BranchOfTree branch in _branchs)
            {
                result = branch.GetBranchByRoot(rootName);
                if (result != null) return result;
            }
            return null;
        }

        private BranchOfTree GetBranchByRoot(Group root)
        {
            return GetBranchByRoot(root.Name);
        }

        #endregion
    }
}