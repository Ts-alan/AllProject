using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Common.Collection;
using System.IO;
using System.Xml.Serialization;

namespace VirusBlokAda.CC.Filters.Composite
{
    public class CompositeFilterStateCollection
    {
        private CompositeFilterStateCollection()
        {
            filterCollection = new SerializableDictionary<string, CompositeFilterState>();
        }

        private SerializableDictionary<string, CompositeFilterState> filterCollection;

        #region Serilization
        public string Serialize()
        {
            List<CompositeFilterState> list = new List<CompositeFilterState>();
            foreach (CompositeFilterState next in filterCollection.Values)
            {
                list.Add(next);
            }
            StringWriter writer = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(typeof(List<CompositeFilterState>),
                new Type[] { typeof(CompositeFilterState) });
            serializer.Serialize(writer, list);
            return writer.ToString();
        }

        public static CompositeFilterStateCollection Deserialize(string rawData)
        {
            try
            {
                XmlSerializer xmlser = new XmlSerializer(typeof(List<CompositeFilterState>),
                    new Type[] { typeof(CompositeFilterState) });
                StringReader reader = new StringReader(rawData);
                List<CompositeFilterState> list = (List<CompositeFilterState>)xmlser.Deserialize(reader);
                CompositeFilterStateCollection result = new CompositeFilterStateCollection();
                foreach (CompositeFilterState next in list)
                {
                    result.Add(next);
                }
                return result;
            }
            catch
            {
                return new CompositeFilterStateCollection();
            }
        }
        #endregion

        #region Add/Get/Update/Delete

        public CompositeFilterState this[string name]
        {
            get
            {
                return filterCollection[name];
            }
        }

        public void Add(CompositeFilterState userFilter)
        {
            filterCollection.Add(userFilter.Name, userFilter);
        }

        public CompositeFilterState Get(string name)
        {
            CompositeFilterState userFilter;
            if (!filterCollection.TryGetValue(name, out userFilter))
            {
                userFilter = new CompositeFilterState();
            }
            return userFilter;
        }

        public void Update(CompositeFilterState userFilter)
        {
            filterCollection[userFilter.Name] = userFilter;
        }


        public void Delete(string name)
        {
            filterCollection.Remove(name);
        }

        public ICollection<string> GetNames()
        {
            return filterCollection.Keys;
        }

        #endregion
    }
}