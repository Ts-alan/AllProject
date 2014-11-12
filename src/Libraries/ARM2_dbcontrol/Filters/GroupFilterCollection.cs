using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ARM2_dbcontrol.Filters
{
    /// <summary>
    /// Summary description for GroupFilterCollection.
    /// </summary>
    public class GroupFilterCollection : System.Collections.CollectionBase
    {

        private string filters;

        public GroupFilterCollection()
        {
            //
            // TODO: Add constructor logic here
            //

        }

        public GroupFilterCollection(string filters)
        {
            this.filters = filters;

            foreach (GroupFilterEntity filter in this.Deserialize())
            {
                this.Add(filter);
            }
        }


        #region Base actions Add/Get/GetAll/Update/Delete

        //функция доступа по умолчанию...
        public GroupFilterEntity this[int index]
        {
            get
            {
                return ((GroupFilterEntity)this.List[index]);
            }
        }

        public void Add(GroupFilterEntity filter)
        {
            this.List.Add(filter);
            this.filters = this.Serialize();
        }

        public GroupFilterEntity Get(string name)
        {
            foreach (GroupFilterEntity filter in Deserialize())
            {
                if (filter.FilterName == name) return filter;
            }
            return new GroupFilterEntity();
        }

        public void Update(GroupFilterEntity filter)
        {
            GroupFilterCollection temp = new GroupFilterCollection();
            foreach (GroupFilterEntity t_filter in this.Deserialize())
            {

                if (t_filter.FilterName != filter.FilterName)
                {
                    temp.Add(t_filter);
                }
                else
                {
                    temp.Add(filter);
                }
            }
            this.filters = temp.filters;
        }


        public void Delete(string name)
        {
            GroupFilterCollection temp = new GroupFilterCollection();
            foreach (GroupFilterEntity filter in this.Deserialize())
            {
                if (filter.FilterName != name)
                {
                    temp.Add(filter);
                }
            }

            this.filters = temp.filters;
        }

        public GroupFilterCollection GetAll()
        {
            return this.Deserialize();
        }

        #endregion


        #region methods - serialization

        public string Serialize()
        {
            StringWriter writer = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(this.GetType(), new Type[] { typeof(GroupFilterEntity) });
            serializer.Serialize(writer, this);
            return writer.ToString();


        }

        public GroupFilterCollection Deserialize()
        {
            try
            {
                XmlSerializer xmlser = new XmlSerializer(this.GetType());
                StringReader reader = new StringReader(this.filters);
                return (GroupFilterCollection)xmlser.Deserialize(reader);
            }
            catch
            {
                return new GroupFilterCollection();
            }
        }

        #endregion

    }
}
