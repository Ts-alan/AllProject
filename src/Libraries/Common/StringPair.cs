using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.Common
{
    public class StringPair
    {
        private String _name;
        private String _value;

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public String Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public StringPair(String name, String value)
        {
            _name = name;
            _value = value;
        }

        public StringPair()
        { }
    }
}
