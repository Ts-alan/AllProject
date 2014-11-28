using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ARM2_dbcontrol.Tasks
{
    /// <summary>
    /// Коллекция пользовательских задач
    /// </summary>
    public class TaskUserCollection : System.Collections.CollectionBase
    {

        private string tasks;

        public TaskUserCollection()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public TaskUserCollection(string tasks)
        {
            this.tasks = tasks;

            foreach (TaskUserEntity task in this.Deserialize())
            {
                this.Add(task);
            }
        }


        #region Base actions Add/Get/GetAll/Update/Delete

        //функция доступа по умолчанию...
        public TaskUserEntity this[int index]
        {
            get
            {
                return ((TaskUserEntity)this.List[index]);
            }
        }
        /// <summary>
        /// Добавление задач
        /// </summary>
        /// <param name="task">задача</param>
        public void Add(TaskUserEntity task)
        {
            this.List.Add(task);
            this.tasks = this.Serialize();
        }
        /// <summary>
        /// получение задач
        /// </summary>
        /// <param name="name">имя задачи</param>
        /// <returns></returns>
        public TaskUserEntity Get(string name)
        {
            foreach (TaskUserEntity task in Deserialize())
            {
                if (task.Name == name) return task;
            }
            return new TaskUserEntity();
        }
        /// <summary>
        /// Изменение задачи
        /// </summary>
        /// <param name="task">задача</param>
        public void Update(TaskUserEntity task)
        {
            TaskUserCollection temp = new TaskUserCollection();
            foreach (TaskUserEntity t_task in this.Deserialize())
            {

                if (t_task.Name != task.Name)
                {
                    temp.Add(t_task);
                }
                else
                {
                    temp.Add(task);
                }
            }
            this.tasks = temp.tasks;
        }

        /// <summary>
        /// удаление задачи
        /// </summary>
        /// <param name="name">имя задачи</param>
        public void Delete(string name)
        {
            TaskUserCollection temp = new TaskUserCollection();
            foreach (TaskUserEntity task in this.Deserialize())
            {
                if (task.Name != name)
                {
                    temp.Add(task);
                }
            }

            this.tasks = temp.tasks;
        }
        /// <summary>
        /// получение всех задач
        /// </summary>
        /// <returns></returns>
        public TaskUserCollection GetAll()
        {
            return this.Deserialize();
        }

        #endregion


        #region methods - serialization

        public string Serialize()
        {
            StringWriter writer = new StringWriter();
            XmlSerializer serializer =
                new XmlSerializer(this.GetType(), new Type[] { typeof(TaskUserEntity) });
            serializer.Serialize(writer, this);
            return writer.ToString();


        }

        public TaskUserCollection Deserialize()
        {
            try
            {
                XmlSerializer xmlser = new XmlSerializer(this.GetType());
                StringReader reader = new StringReader(this.tasks);
                return (TaskUserCollection)xmlser.Deserialize(reader);
            }
            catch
            {
                return new TaskUserCollection();
            }
        }

        #endregion

    }
}
