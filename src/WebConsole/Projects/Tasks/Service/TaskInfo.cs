using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection;

namespace Tasks.Service
{
    /// <summary>
    /// ������ ���������� ��� ������ ������ com-������� � ��������� ������
    /// </summary>
    class TaskInfo
    {
        protected string service = String.Empty;//��� �������
        protected object serv = null;           //������ com
        protected Type servType = null;         //������ com
        protected BindingFlags bindFlags;       //����� ��� InvokeMember
        protected string method = String.Empty; // ��� ������
        protected object[] arguments = null;    //���������

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
