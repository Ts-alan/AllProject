using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
namespace HTMLClearer
{
    public class HTMLClearer : System.IO.Stream
    {
        System.IO.Stream _HTML;
        public HTMLClearer(System.IO.Stream HTML)
        { _HTML = HTML; }
        #region Стандартные методы и свойства
        public override bool CanRead
        { get { return false; } }
        public override bool CanSeek
        { get { return false; } }
        public override bool CanWrite
        { get { return true; } }
        public override long Length
        { get { return _HTML.Length; } }
        public override long Position
        {
            get { return _HTML.Position; }
            set { _HTML.Position = value; }
        }
        public override long Seek(long offset, System.IO.SeekOrigin origin)
        { return _HTML.Seek(offset, origin); }
        public override void SetLength(long value)
        { _HTML.SetLength(value); }
        public override void Flush()
        { _HTML.Flush(); }
        public override int Read(byte[] buffer, int offset, int count)
        { return _HTML.Read(buffer, offset, count); }
        #endregion
        /// <summary> 
        /// Обрабатываем данные поступающие в Response 
        /// </summary> 
        public override void Write(byte[] buffer, int offset, int count)
        {
            //Преобразовываем массив байт в строку 
            string s = System.Text.Encoding.UTF8.GetString(buffer);
            //Используя регулярные выражения убираем все ненужные символы 
            s = Regex.Replace(s, ">(\r\n){0,10} {0,20}\t{0,10}(\r\n){0,10}\t{0,10}(\r\n){0,10} {0,20}(\r\n){0,10} {0,20}<", "><", RegexOptions.Compiled);
            s = Regex.Replace(s, ";(\r\n){0,10} {0,20}\t{0,10}(\r\n){0,10}\t{0,10}", ";", RegexOptions.Compiled);
            s = Regex.Replace(s, "{(\r\n){0,10} {0,20}\t{0,10}(\r\n){0,10}\t{0,10}", "{", RegexOptions.Compiled);
            s = Regex.Replace(s, ">(\r\n){0,10}\t{0,10}<", "><", RegexOptions.Compiled);
            s = Regex.Replace(s, ">\r{0,10}\t{0,10}<", "><", RegexOptions.Compiled);
            //Получивщуюся строку преобразовываем обратно в byte 
            byte[] outdata = System.Text.Encoding.UTF8.GetBytes(s);
            //Записываем ее в Response 
            _HTML.Write(outdata, 0, outdata.Length);
        }
    }
    public class HTTPModule_Clearer : IHttpModule
    {
        #region IHttpModule Members
        public void Dispose()
        {
        }
        /// <summary> 
        /// Подключение обработчиков событий 
        /// </summary> 
        public void Init(HttpApplication context)
        {
            //Подключаем обработчик на событие ReleaseRequestState 
            context.ReleaseRequestState += new EventHandler(this.context_Clear);
            //Подключаем обработчик на событие PreSendRequestHeaders 
            context.PreSendRequestHeaders += new EventHandler(this.context_Clear);
            //Два обработчика необходимы для совместимости с библиотеками сжатия HTML-документов 
        }
        /// <summary> 
        /// Обработчик события PostRequestHandlerExecute 
        /// </summary> 
        void context_Clear(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender; //Получение HTTP Application 
            string realPath = app.Request.Path.Remove(0, app.Request.ApplicationPath.Length + 1); //Получаем имя файла который обрабатывается 
            if (realPath == "WebResource.axd") //Проверяем не является ли он ссылкой на ресурс сборки 
                return;
            if (realPath == "ScriptResource.axd") //Проверяем не является ли он ссылкой на скрипр ресурс
                return;
            if (app.Response.ContentType == "text/html" || app.Response.ContentType == "text/javascript") //Проверяем тип содержимого 
                app.Context.Response.Filter = new HTMLClearer(app.Context.Response.Filter); //Устанавливаем фильтр обработчик 
        }
        #endregion
    }
}
