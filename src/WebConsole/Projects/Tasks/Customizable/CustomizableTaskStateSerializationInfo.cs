using System;
using System.Collections.Generic;
using System.Text;
using Tasks.Entities;

namespace Tasks.Customizable
{
    public class CustomizableTaskStateSerializationInfo
    {
        private string _typeAssemblyQualifiedName;
        public string TypeAssemblyQualifiedName {
            get { return _typeAssemblyQualifiedName;}
            set { _typeAssemblyQualifiedName = value;}
        }

        private List<NamedTaskEntity> _namedTaskEntityList;
        public List<NamedTaskEntity> NamedTaskEntityList
        {
            get { return _namedTaskEntityList; }
            set { _namedTaskEntityList = value; }
        }

        public CustomizableTaskStateSerializationInfo()
        {
            _namedTaskEntityList = new List<NamedTaskEntity>();
        }
    }
}
