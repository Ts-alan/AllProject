using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public class BranchOfTree
    {
        #region Properties

        private Group _root;
        public Group Root
        {
            get { return _root; }
            set { _root = value; }
        }

        private List<BranchOfTree> _childrenBranchs;
        public List<BranchOfTree> ChildrenBranchs
        {
            get { return _childrenBranchs; }
        }

        private List<ComputersEntity> _computers;
        public List<ComputersEntity> Computers
        {
            get { return _computers; }
        }

        #endregion

        #region Constructors

        public BranchOfTree()
        {
            _root = new Group();
            _childrenBranchs = new List<BranchOfTree>();
            _computers = new List<ComputersEntity>();
        }

        public BranchOfTree(Group root)
            : this()
        {
            _root = root;
        }

        #endregion

        #region Methods

        public void AddBranch(BranchOfTree branch)
        {
            if (!IsRootExist(branch.Root.Name))
                _childrenBranchs.Add(branch);
            else
            {
                BranchOfTree resultBranch = GetBranchByRoot(branch.Root);
                //Computers
                resultBranch.AddComputers(branch.Computers);
                //Branches
                resultBranch.AddBranches(branch.ChildrenBranchs);
            }
        }

        public void AddBranches(List<BranchOfTree> branches)
        {
            foreach (BranchOfTree childBranch in branches)
            {
                AddBranch(childBranch);
            }
        }

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

        private Boolean FindComputer(String computerName)
        {
            foreach (ComputersEntity compEnt in _computers)
            {
                if (compEnt.ComputerName == computerName) return true;
            }

            return false;
        }

        private Boolean FindComputer(ComputersEntity comp)
        {
            return FindComputer(comp.ComputerName);
        }

        internal Group? RecursiveFindRoot(String compName)
        {
            if (this.FindComputer(compName)) return this._root;

            Group? result;
            foreach (BranchOfTree branchEnt in this._childrenBranchs)
            {
                result = branchEnt.RecursiveFindRoot(compName);
                if (result != null) return result;
            }
            return null;
        }

        internal Group? RecursiveFindRoot(ComputersEntity comp)
        {
            return this.RecursiveFindRoot(comp.ComputerName);
        }

        public Boolean IsRootExist(String rootName)
        {
            if (_root.Name == rootName) return true;
            foreach (BranchOfTree branch in this._childrenBranchs)
            {
                if (branch.IsRootExist(rootName)) return true;
            }
            return false;
        }

        public BranchOfTree GetBranchByRoot(String rootName)
        {
            if (_root.Name == rootName) return this;
            BranchOfTree result;
            foreach (BranchOfTree branch in this._childrenBranchs)
            {
                result = branch.GetBranchByRoot(rootName);
                if (result != null) return result;
            }
            return null;
        }

        public BranchOfTree GetBranchByRoot(Group root)
        {
            return GetBranchByRoot(root.Name);
        }

        #endregion
    }
}