using System;
using System.Collections.Generic;
using System.Text;

namespace ARM2_dbcontrol.Tasks
{
    /// <summary>
    /// ��������� ������ 
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// ������������� �����
        /// </summary>
        void InitFields();

        /// <summary>
        /// �������� ����������� �����
        /// </summary>
        /// <returns>������ � ������ ������</returns>
        bool ValidateFields();

        /// <summary>
        /// ���������� ������� ��������� ���� �����
        /// </summary>
        /// <returns>��������-������</returns>
        TaskUserEntity GetCurrentState();

        /// <summary>
        /// ��������� ���� � ������������ � �������-���������
        /// </summary>
        /// <param name="task">������-��������</param>
        void LoadState(TaskUserEntity task);
      
    }
}
