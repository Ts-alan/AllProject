using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Specialized;

namespace Vba32.ControlCenter.NotificationService.Notification
{
    /// <summary>
    /// Создает сообщение с помощью шаблона пользователя
    /// </summary>
    public static class NotifyMessageBuilder
    {
        /// <summary>
        /// Создает из пришедшего события текст сообщения на основе шаблона
        /// </summary>
        /// <param name="message">событие</param>
        /// <param name="template">шаблон</param>
        /// <returns></returns>
        public static String BuildBody(StringDictionary message, String template)
        {
            String tmp = template.Replace("{Computer}", message["Computer"]);
            tmp = tmp.Replace("{EventName}", message["EventName"]);
            tmp = tmp.Replace("{EventTime}", message["EventTime"]);
            tmp = tmp.Replace("{Component}", message["Component"]);
            tmp = tmp.Replace("{Object}", message["Object"]);
            tmp = tmp.Replace("{Comment}", message["Comment"]);
            tmp = tmp.Replace("{IPAddress}", message["IPAddress"]);

            return tmp;
        }

        /// <summary>
        /// Создает из пришедшего события заголовок сообщения на основе шаблона
        /// </summary>
        /// <param name="message">Событие</param>
        /// <param name="template">Шаблон</param>
        /// <returns></returns>
        public static String BuildSubject(StringDictionary message, String template)
        {
            //Вдруг захочется определить особым образом
            return BuildBody(message, template);
        }

    }
}
