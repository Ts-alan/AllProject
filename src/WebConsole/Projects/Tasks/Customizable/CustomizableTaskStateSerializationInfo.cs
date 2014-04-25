using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Tasks.Entities;

namespace VirusBlokAda.CC.Tasks.Customizable
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
