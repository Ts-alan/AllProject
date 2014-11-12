using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    /// <summary>
    /// General group entity
    /// </summary>
    public struct GroupEx
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

        private Int32 _totalCount;
        public Int32 TotalCount
        {
            get { return _totalCount; }
            set { _totalCount = value; }
        }

        private Int32 _activeCount;
        public Int32 ActiveCount
        {
            get { return _activeCount; }
            set { _activeCount = value; }
        }

        private String _parentName;
        public String ParentName
        {
            get { return _parentName; }
            set { _parentName = value; }
        }
        #endregion

        #region Constructors
        public GroupEx(String name, String comment) :
            this(0, name, "", comment, 0, 0)
        {
        }

        public GroupEx(Int32 id, String name, String parentName, String comment,
            Int32 totalCount, Int32 activeCount)
        {
            _id = id;
            _name = name;
            _comment = comment;
            _totalCount = totalCount;
            _activeCount = activeCount;
            _parentName = parentName;
        }
        #endregion
    }
}
