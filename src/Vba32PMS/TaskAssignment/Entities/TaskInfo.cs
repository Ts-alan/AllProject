using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService.TaskAssignment.Entities
{
    /// <summary>
    /// Хранит информацию для вызова метода com-объекта в отдельном потоке
    /// </summary>
    class TaskInfo
    {
        protected String service = String.Empty;//имя сервиса
        protected Object serv = null;           //объект com
        protected Type servType = null;         //объект com
        protected BindingFlags bindFlags;       //флаги для InvokeMember
        protected String method = String.Empty; // имя метода
        protected Object[] arguments = null;    //аргументы

        public TaskInfo(String service,String method, BindingFlags bindFlags,
            Object[] arg)
        {
            this.service = service;
            this.method = method;
            this.bindFlags = bindFlags;
            this.arguments = arg;
        }

        #region Property

        public String Service
        {
            get { return this.service; }
        }

        public Object Serv
        {
            get { return this.serv; }
            set { this.serv = value; }
        }

        public Type ServType
        {
            get { return this.servType; }
            set { this.servType = value; }
        }

        public String Method
        {
            get { return this.method; }
        }

        public BindingFlags BindFlags
        {
            get { return this.bindFlags; }
        }

        public Object[] Arguments
        {
            get { return this.arguments; }
        }

        #endregion

    }
}
