using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;

using ARM2_dbcontrol.DataBase;

namespace ARM2_dbcontrol.Diagram
{
    /// <summary>
    /// ����� VisualReport
    /// ����� VisualReport ������������ ��� ������������� �������� ������ �
    /// ���������� ������.
    /// </summary>
    public class VisualReport
    {
        private String _connectionString;
        public String ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        public VisualReport(String connectionString)
        {
            _connectionString = connectionString;
        }

        public List<StatisticEntity> GetStatistics(String groupBy, String where, Int32 size)
        {
            List<StatisticEntity> list = new List<StatisticEntity>();

            using (VlslVConnection conn = new VlslVConnection(_connectionString))
            {
                EventsManager db = new EventsManager(conn);
                conn.OpenConnection();
                conn.CheckConnectionState(true);

                list = db.GetStatistics(groupBy, where, size);

                conn.CloseConnection();
            }

            return list;
        }
    }
}
