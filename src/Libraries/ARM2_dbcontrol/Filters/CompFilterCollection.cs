using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ARM2_dbcontrol.Filters
{
	/// <summary>
	/// Summary description for CompFilterCollection.
	/// </summary>
	public class CompFilterCollection: System.Collections.CollectionBase
	{

		private string filters;

		public CompFilterCollection()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public CompFilterCollection(string filters)
		{
			this.filters = filters;

			foreach(CompFilterEntity filter in this.Deserialize())
			{
					this.Add(filter);
			}			
		}

		#region Base actions Add/Get/GetAll/Update/Delete

		//функция доступа по умолчанию...
		public CompFilterEntity this[int index]
		{ 
			get 
			{ 
				return((CompFilterEntity)this.List[index]); 
			}
		}
		/// <summary>
		/// добавление фильтра компьютеров
		/// </summary>
		/// <param name="filter">фильтр компьютера</param>
		public void Add(CompFilterEntity filter)
		{    
			this.List.Add(filter); 
			this.filters = this.Serialize();
		}
        /// <summary>
        /// получение фильтра компьютера
        /// </summary>
        /// <param name="name">имя фильтра</param>
        /// <returns></returns>
		public CompFilterEntity Get(string name)
		{
			foreach(CompFilterEntity filter in Deserialize())
			{
				if(filter.FilterName ==name) return filter;
			}
			return new CompFilterEntity();
		}
        /// <summary>
        /// изменение фильтра
        /// </summary>
        /// <param name="filter">фильтр</param>
		public void Update(CompFilterEntity filter)
		{
			CompFilterCollection temp = new CompFilterCollection();
			foreach(CompFilterEntity t_filter in this.Deserialize() )
			{
				
				if(t_filter.FilterName != filter.FilterName)
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

        /// <summary>
        ///  удаление фильтра
        /// </summary>
        /// <param name="name">имя фильтра</param>
		public void Delete(string name)
		{
			CompFilterCollection temp = new CompFilterCollection();
			foreach(CompFilterEntity filter in this.Deserialize() )
			{
				if(filter.FilterName != name)
				{
					temp.Add(filter);
				}
			}

			this.filters = temp.filters;
		}

		public CompFilterCollection GetAll()
		{
			return this.Deserialize();
		}
		#endregion


		#region methods - serialization

		public string Serialize() 
		{    
			StringWriter writer = new StringWriter();
			XmlSerializer serializer = new XmlSerializer(this.GetType(), new Type[] { typeof(CompFilterEntity)});
			serializer.Serialize(writer, this);   
			return writer.ToString(); 


		}

		public CompFilterCollection Deserialize()
		{
			try
			{
				XmlSerializer xmlser = new XmlSerializer(this.GetType());
				StringReader reader = new StringReader(this.filters);
				return (CompFilterCollection)xmlser.Deserialize(reader);
			}
			catch
			{
				return new CompFilterCollection();
			}
		}

		#endregion



	}
}
