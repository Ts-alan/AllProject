using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.Vba32CC.Groups
{
    /// <summary>
    /// General group entity
    /// </summary>
    public struct Group
    {
        #region Property

        private Int32 _id;
        public Int32 ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private String _name;
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private String _comment;
        public String Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        private Int32? _parentID;
        public Int32? ParentID
        {
            get { return _parentID; }
            set { _parentID = value; }
        }

        #endregion

        public Group(String name, String comment) :
            this(0, name, comment, null)
        {
        }

        public Group(String name, String comment, Int32? parentID) :
            this(0, name, comment, parentID)
        {
        }

        public Group(Int32 id, String name, String comment, Int32? parentID)
        {
            _id = id;
            _name = name;
            _comment = comment;
            _parentID = parentID;
        }

    }
}
