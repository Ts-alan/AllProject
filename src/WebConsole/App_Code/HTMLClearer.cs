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
        #region ����������� ������ � ��������
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
        /// ������������ ������ ����������� � Response 
        /// </summary> 
        public override void Write(byte[] buffer, int offset, int count)
        {
            //��������������� ������ ���� � ������ 
            string s = System.Text.Encoding.UTF8.GetString(buffer);
            //��������� ���������� ��������� ������� ��� �������� ������� 
            s = Regex.Replace(s, ">(\r\n){0,10} {0,20}\t{0,10}(\r\n){0,10}\t{0,10}(\r\n){0,10} {0,20}(\r\n){0,10} {0,20}<", "><", RegexOptions.Compiled);
            s = Regex.Replace(s, ";(\r\n){0,10} {0,20}\t{0,10}(\r\n){0,10}\t{0,10}", ";", RegexOptions.Compiled);
            s = Regex.Replace(s, "{(\r\n){0,10} {0,20}\t{0,10}(\r\n){0,10}\t{0,10}", "{", RegexOptions.Compiled);
            s = Regex.Replace(s, ">(\r\n){0,10}\t{0,10}<", "><", RegexOptions.Compiled);
            s = Regex.Replace(s, ">\r{0,10}\t{0,10}<", "><", RegexOptions.Compiled);
            //������������ ������ ��������������� ������� � byte 
            byte[] outdata = System.Text.Encoding.UTF8.GetBytes(s);
            //���������� �� � Response 
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
        /// ����������� ������������ ������� 
        /// </summary> 
        public void Init(HttpApplication context)
        {
            //���������� ���������� �� ������� ReleaseRequestState 
            context.ReleaseRequestState += new EventHandler(this.context_Clear);
            //���������� ���������� �� ������� PreSendRequestHeaders 
            context.PreSendRequestHeaders += new EventHandler(this.context_Clear);
            //��� ����������� ���������� ��� ������������� � ������������ ������ HTML-���������� 
        }
        /// <summary> 
        /// ���������� ������� PostRequestHandlerExecute 
        /// </summary> 
        void context_Clear(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender; //��������� HTTP Application 
            string realPath = app.Request.Path.Remove(0, app.Request.ApplicationPath.Length + 1); //�������� ��� ����� ������� �������������� 
            if (realPath == "WebResource.axd") //��������� �� �������� �� �� ������� �� ������ ������ 
                return;
            if (realPath == "ScriptResource.axd") //��������� �� �������� �� �� ������� �� ������ ������
                return;
            if (app.Response.ContentType == "text/html" || app.Response.ContentType == "text/javascript") //��������� ��� ����������� 
                app.Context.Response.Filter = new HTMLClearer(app.Context.Response.Filter); //������������� ������ ���������� 
        }
        #endregion
    }
}
