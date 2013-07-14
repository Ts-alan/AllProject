using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService.TaskAssignment.Entities
{
    /// <summary>
    /// ������ ���������� ��� ������ ������ com-������� � ��������� ������
    /// </summary>
    class TaskInfo
    {
        protected String service = String.Empty;//��� �������
        protected Object serv = null;           //������ com
        protected Type servType = null;         //������ com
        protected BindingFlags bindFlags;       //����� ��� InvokeMember
        protected String method = String.Empty; // ��� ������
        protected Object[] arguments = null;    //���������

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
