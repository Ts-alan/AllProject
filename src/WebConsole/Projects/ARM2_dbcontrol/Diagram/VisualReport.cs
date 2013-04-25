using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;

using ARM2_dbcontrol.DataBase;

namespace ARM2_dbcontrol.Diagram
{
    /// <summary>
    /// Класс VisualReport
    /// Класс VisualReport используется для представления элемента данных в
    /// визуальном отчете.
    /// </summary>
    public class VisualReport
    {
        private string _categoryName;
        private decimal _sales;

        public string CategoryName
        {
            get { return _categoryName; }
            set { _categoryName = value; }
        }

        public decimal Sales
        {
            get { return _sales; }
            set { _sales = value; }
        }

        public static VisualReportCollection GetVirusStat(string connStr,
            string virusFoundEventName, DateTime date)
        {
            VisualReportCollection items = new VisualReportCollection();

            List<StatisticEntity> list = new List<StatisticEntity>();

            using (VlslVConnection conn = new VlslVConnection(connStr))
            {
                EventsManager db = new EventsManager(conn);
                conn.OpenConnection();
                conn.CheckConnectionState(true);

                list = db.GetStatisticVirusList(date, virusFoundEventName, "Comment");

                conn.CloseConnection();

            }

            foreach (StatisticEntity stat in list)
            {
                VisualReport item = new VisualReport();
                item.CategoryName = stat.Name;
                item.Sales = stat.Count;
                items.Add(item);
            }

            return items;
        }

        // Метод GetCategorySales извлекает общее количество всех элементов в категории
        // из базы данных и перед возвращением результата вызвавшей метод функции 
        // преобразует его в пользовательский набор VisualReportCollection.
        public static VisualReportCollection GetEvent(string connStr, string where)
        {
            VisualReportCollection items = new VisualReportCollection();

            List<StatisticEntity> list = new List<StatisticEntity>();

            using (VlslVConnection conn = new VlslVConnection(connStr))
            {
                EventsManager db = new EventsManager(conn);
                conn.OpenConnection();
                conn.CheckConnectionState(true);

                list = db.GetStatisticList("ComputerName", where, 10);

                conn.CloseConnection();

            }

            foreach (StatisticEntity stat in list)
            {
                VisualReport item = new VisualReport();
                item.CategoryName = stat.Name;
                item.Sales = stat.Count;
                items.Add(item);
            }
         
            return items;
        }

    }
}
