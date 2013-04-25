using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Specialized;

namespace Vba32.ControlCenter.NotificationService.Notification
{
    /// <summary>
    /// ������� ��������� � ������� ������� ������������
    /// </summary>
    public static class NotifyMessageBuilder
    {
        /// <summary>
        /// ������� �� ���������� ������� ����� ��������� �� ������ �������
        /// </summary>
        /// <param name="message">�������</param>
        /// <param name="template">������</param>
        /// <returns></returns>
        public static string BuildBody(StringDictionary message, string template)
        {
            string tmp = template.Replace("{Computer}", message["Computer"]);
            tmp = tmp.Replace("{EventName}", message["EventName"]);
            tmp = tmp.Replace("{EventTime}", message["EventTime"]);
            tmp = tmp.Replace("{Component}", message["Component"]);
            tmp = tmp.Replace("{Object}", message["Object"]);
            tmp = tmp.Replace("{Comment}", message["Comment"]);
            tmp = tmp.Replace("{IPAddress}", message["IPAddress"]);

            return tmp;
        }

        /// <summary>
        /// ������� �� ���������� ������� ��������� ��������� �� ������ �������
        /// </summary>
        /// <param name="message">�������</param>
        /// <param name="template">������</param>
        /// <returns></returns>
        public static string BuildSubject(StringDictionary message, string template)
        {
            //����� ��������� ���������� ������ �������
            return BuildBody(message, template);
        }

    }
}
