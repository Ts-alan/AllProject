using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.IO;

using ARM2_dbcontrol.Generation;
using ARM2_dbcontrol.Tasks;
using ARM2_dbcontrol.Filters;

/// <summary>
/// Задача передачи файла с сервера на клиент
/// </summary>
public partial class Controls_TaskSendFile : System.Web.UI.UserControl, ITask
{
    private string tag_Details = "Details";
    private string tag_Source = "Source";
    private string tag_Destination = "Destination";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            InitFields();
    }

    private bool _hideHeader = false;

    public bool HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }

    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;
        //lblDetails.Text = "";
        lbtnUpload.Text = Resources.Resource.Upload;
    }

    public bool ValidateFields()
    {
        Validation vld = new Validation(tboxSource.Text);
        if ((!vld.CheckStringToTask()) || (tboxSource.Text == ""))
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                        + Resources.Resource.SourceFile);
        vld.Value = tboxDestination.Text;
        if ((!vld.CheckStringToTask()) || (tboxDestination.Text == ""))
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                        + Resources.Resource.DestinationFile);

        return true;
    }


    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();

        task.Type = TaskType.SendFile;

        VirusBlokAda.CC.Common.Xml.XmlBuilder xml =
            new VirusBlokAda.CC.Common.Xml.XmlBuilder("task");
        xml.AddNode(tag_Details, lblDetails.Text);
        xml.AddNode(tag_Source,tboxSource.Text);
        xml.AddNode(tag_Destination, tboxDestination.Text);
        xml.AddNode("Vba32CCUser", Anchor.GetStringForTaskGivedUser());
        xml.AddNode("Type","SendFile");
        xml.Generate();

        task.Param = xml.Result;

        return task;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.SendFile)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        VirusBlokAda.CC.Common.Xml.XmlTaskParser pars = new VirusBlokAda.CC.Common.Xml.XmlTaskParser(task.Param);

        tboxSource.Text = pars.GetValue(tag_Source);
        tboxDestination.Text = pars.GetValue(tag_Destination);
        lblDetails.Text = pars.GetValue(tag_Details);
        if (lblDetails.Text == String.Empty)
            lblDetails.Text ="";
    }

    #region Property

    public string TagSource
    {
        get { return this.tag_Source; }
    }

    public string TagDestination
    {
        get { return this.tag_Destination; }
    }

    #endregion  

   

    /// <summary>
    /// Загрузка файла на сервер
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnUpload_Click(object sender, EventArgs e)
    {
        if (fuClient.HasFile == false)
        {
            // No file uploaded!
            lblDetails.Text = Resources.Resource.NoSelectFile;
        }
        else
        {
            // Display the uploaded file's details
            lblDetails.Text = string.Format(Resources.Resource.SendFileUploadFileInfo,
                fuClient.FileName, fuClient.FileBytes.Length, fuClient.PostedFile.ContentType);
            // Save the file

            string fileName = Guid.NewGuid().ToString();

            string filePath = Server.MapPath("~/Downloads/" + fileName);
            fuClient.SaveAs(filePath);

            tboxSource.Text = ParseUrl(Request.Url.AbsoluteUri) + "Downloads/" + fileName;
            if (tboxDestination.Text == String.Empty)
                tboxDestination.Text = "%VBA32%" + fuClient.FileName;
        }
    }
    /// <summary>
    /// Этот метод возвращает имя сервера, приложения, без текущей страницы
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private string ParseUrl(string url)
    {
        //!-OPTM: Возможно, вот этот подход будет универсальнее
        //   public string BaseUrl
        //{
        //   get
        //   {
        //      string url = this.Request.ApplicationPath;
        //      if (url.EndsWith("/"))
        //         return url;
        //      else
        //         return url + "/";
        //   }
        //}

        //public string FullBaseUrl
        //{
        //   get
        //   {
        //      return this.Request.Url.AbsoluteUri.Replace(
        //         this.Request.Url.PathAndQuery, "") + this.BaseUrl;
        //   }
        //}


        string source = String.Empty;
        System.Text.RegularExpressions.Regex reg =
            new System.Text.RegularExpressions.Regex(@"^http://[a-zA-Z_0-9-_/.:]+[^.aspx]");
        System.Text.RegularExpressions.Match match = reg.Match(url);
        if (match.Success)
            source = match.Value;

        int trim = 0;
        for (int i = source.Length - 1; i >= 0; i--)
        {
            if (source[i] != '/')
            {
                trim++;
            }
            else
            {
                break;
            }
        }

        return source.Remove(source.Length - trim, trim);

    }
   

}
