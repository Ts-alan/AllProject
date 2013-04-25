using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.Vba32CC.Policies.General
{
    /// <summary>
    /// General policy entity
    /// </summary>
    public struct Policy
    {

        #region Property


        private int _id;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }


        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        private string _content;

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        private string _comment;

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        #endregion

        public Policy(string name, string content, string comment):
            this(0,name,content,comment)
        {
   
        }

        public Policy(int id, string name, string content, string comment)
        {
            _id = id;
            _name = name;
            _content = content;
            _comment = comment;
        }

      
    }
}
