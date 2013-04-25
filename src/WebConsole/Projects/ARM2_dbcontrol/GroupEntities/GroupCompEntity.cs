using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.Vba32CC.Groups
{
    /// <summary>
    /// General group entity
    /// </summary>
    public struct ChildParentEntity
    {
        #region Property

        private int _childId;

        public int ChildID
        {
            get { return _childId; }
            set { _childId = value; }
        }

        private string _childName;

        public string ChildName
        {
            get { return _childName; }
            set { _childName = value; }
        }

        private int? _parentId;

        public int? ParentID
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        #endregion

        public ChildParentEntity(int childId, string childName):
            this(childId, childName, null)
        {
        }

        public ChildParentEntity(int childId, string childName, int? parentId)
        {
            _childId = childId;
            _childName = childName;
            _parentId = parentId;
        }

    }
}
