using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;

using VirusBlokAda.CC.DataBase;

namespace VirusBlokAda.CC.Diagram
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
            EventProvider db = new EventProvider(ConnectionString);
                return db.GetStatistics(groupBy, where, size);
        }
    }
}
