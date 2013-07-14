using System;

using System.Text.RegularExpressions;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService.DataBase
{
    /// <summary>
    /// ����� ��� ������������ �������
    /// </summary>
    public static class EventsFilterEntity
    {

        /// <summary>
        /// ���������� ������ ������� �� ����
        /// </summary>
        /// <param name="name">��� ���� � ���� ������</param>
        /// <param name="val1">���� "�"</param>
        /// <param name="val2">���� "��"</param>
        /// <returns></returns>
        public static String DateValue(String name, DateTime val1, DateTime val2, String term)
        {
            if ((val1 == DateTime.MinValue) || (val2 == DateTime.MinValue)) { return String.Empty; }

            String final;
            if (term != "NOT")
            {
                final = name + " > = CAST('" + val1.Year + '.' + val1.Month + '.' + val1.Day + ' ' + val1.Hour + ':' + val1.Minute + ':' + val1.Second + "' AS SMALLDATETIME) AND "
                    + name + " < = CAST('" + val2.Year + '.' + val2.Month + '.' + val2.Day + ' ' + val2.Hour + ':' + val2.Minute + ':' + val2.Second + "' AS SMALLDATETIME) ";
            }
            else
            {
                final = name + " < CAST('" + val1.Year + '.' + val1.Month + '.' + val1.Day + ' ' + val1.Hour + ':' + val1.Minute + ':' + val1.Second + "' AS SMALLDATETIME) OR "
                   + name + " > CAST('" + val2.Year + '.' + val2.Month + '.' + val2.Day + ' ' + val2.Hour + ':' + val2.Minute + ':' + val2.Second + "' AS SMALLDATETIME) ";
            }
            return final;
        }
    }
}
