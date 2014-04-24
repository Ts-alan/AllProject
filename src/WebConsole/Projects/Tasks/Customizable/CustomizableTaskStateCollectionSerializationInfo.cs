using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.Vba32CC.Common.Collection;

namespace Tasks.Customizable
{
    public class CustomizableTaskStateCollectionSerializationInfo
    {
        private SerializableDictionary<string, CustomizableTaskStateSerializationInfo> _collection;
        public SerializableDictionary<string, CustomizableTaskStateSerializationInfo> Collection{
            get { return _collection; }
            set { _collection = value; }
        }

        public CustomizableTaskStateCollectionSerializationInfo()
        {
            _collection = new SerializableDictionary<string, CustomizableTaskStateSerializationInfo>();
        }
    }
}
