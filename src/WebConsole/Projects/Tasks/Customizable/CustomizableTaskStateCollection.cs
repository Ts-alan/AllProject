using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Common;
using System.Web;
using System.Configuration;
using VirusBlokAda.CC.Common.Collection;
using System.IO;
using System.Xml.Serialization;
using VirusBlokAda.CC.Tasks.Entities;

namespace VirusBlokAda.CC.Tasks.Customizable
{
    internal class CustomizableTaskStateCollection
    {
        private CustomizableTaskStateCollection()
        {
            tasksCollection = new Dictionary<string, CustomizableTaskState>();
        }

        private Dictionary<string, CustomizableTaskState> tasksCollection;

        #region Serilization
        public string Serialize()
        {
            CustomizableTaskStateCollectionSerializationInfo collectionSI = 
                new CustomizableTaskStateCollectionSerializationInfo();
            foreach (CustomizableTaskState next in tasksCollection.Values)
            {
                CustomizableTaskStateSerializationInfo nextSI = 
                    next.ConvertToCustomizableTaskStateSerializationInfo();
                collectionSI.Collection.Add(nextSI.TypeAssemblyQualifiedName, nextSI);
            }
            StringWriter writer = new StringWriter();
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CustomizableTaskStateCollectionSerializationInfo));
                serializer.Serialize(writer, collectionSI);
                return writer.ToString();
            }
            finally
            {
                writer.Close();
            }
        }

        public static CustomizableTaskStateCollection Deserialize(string rawData)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CustomizableTaskStateCollectionSerializationInfo));
                StringReader reader = new StringReader(rawData);
                try
                {
                    CustomizableTaskStateCollectionSerializationInfo collectionSI =
                        (CustomizableTaskStateCollectionSerializationInfo)serializer.Deserialize(reader);
                    CustomizableTaskStateCollection result = new CustomizableTaskStateCollection();

                    foreach (CustomizableTaskStateSerializationInfo next in collectionSI.Collection.Values)
                    {
                        CustomizableTaskState state =
                            CustomizableTaskState.LoadFromCustomizableTaskStateSerializationInfo(next);
                        result.Add(state);
                    }
                    return result;
                }
                finally
                {
                    reader.Close();
                }
            }
            catch
            {
                return new CustomizableTaskStateCollection();
            }
        }
        #endregion

        #region Get/Update
        public CustomizableTaskState Get(string taskType)
        {
            CustomizableTaskState state = null;
            tasksCollection.TryGetValue(taskType, out state);
            return state;
        }

        public void Update(CustomizableTaskState state)
        {
            tasksCollection[state.GetTaskType().AssemblyQualifiedName] = state;
        }
        #endregion

        #region Add/Delete
        private void Add(CustomizableTaskState state)
        {
            tasksCollection.Add(state.GetTaskType().AssemblyQualifiedName, state);
        }
        private void Delete(string taskType)
        {
            tasksCollection.Remove(taskType);
        }
        #endregion
    }
}
