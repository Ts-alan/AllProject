using System;
using System.Collections;

namespace ARM2_dbcontrol.Diagram
{
    /// <summary>
    /// Класс VisualReportCollection
    /// Это структурный класс для создания сортируемого набора.
    /// </summary>
    public class VisualReportCollection : ArrayList
    {
        public enum VisualReportFields
        {
            // Помещение новых перечислений
        }

        public void Sort(VisualReportFields sortField, bool isAscending)
        {
            // Реализация метода Sort
        }
        // Размещение реализаций IComparer
    }
}