using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection;

namespace Tasks.Service
{
    /// <summary>
    /// Хранит информацию для вызова метода com-объекта в отдельном потоке
    /// </summary>
    class TaskInfo
    {
        protected string service = String.Empty;//имя сервиса
        protected object serv = null;           //объект com
        protected Type servType = null;         //объект com
        protected BindingFlags bindFlags;       //флаги для InvokeMember
        protected string method = String.Empty; // имя метода
        protected object[] arguments = null;    //аргументы

        public TaskInfo(string service,string method, BindingFlags bindFlags,
            object[] arg)
        {
            this.service = service;
            this.method = method;
            this.bindFlags = bindFlags;
            this.arguments = arg;
        }

        #region Property

        public string Service
        {
            get { return this.service; }
        }

        public object Serv
        {
            get { return this.serv; }
            set { this.serv = value; }
        }

        public Type ServType
        {
            get { return this.servType; }
            set { this.servType = value; }
        }

        public string Method
        {
            get { return this.method; }
        }

        public BindingFlags BindFlags
        {
            get { return this.bindFlags; }
        }

        public object[] Arguments
        {
            get { return this.arguments; }
        }

        #endregion

    }
}
