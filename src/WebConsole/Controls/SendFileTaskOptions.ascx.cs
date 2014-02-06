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
using Tasks.Common;
using Tasks.Entities;

using Subgurim.Controles;

/// <summary>
/// Задача передачи файла с сервера на клиент
/// </summary>
public partial class Controls_SendFileTaskOptions : System.Web.UI.UserControl, ITaskOptions, ITaskOptionsHelper<SendFileTaskEntity>
{
    #region Page LifeCycle
    protected void Page_Load(object sender, EventArgs e)
    {
        if (fileUploaderAJAX1.IsPosting)
            ManagePosting();
        
       

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        

        ScriptManager.RegisterClientScriptBlock(this, GetType(), "aaa", "alert(444); ", true);

    }

    private void ManagePosting()
   // protected void lbtnUpload_Click(object sender, EventArgs e)
    {
        
       HttpPostedFileAJAX pf = fileUploaderAJAX1.PostedFile;
        
     /*  if (fuClient.HasFile == false)
        {
            // No file uploaded!
            lblDetails.Text = Resources.Resource.NoSelectFile;
        }
        else
        {*/
            // Display the uploaded file's details
           // lblDetails.Text = string.Format(Resources.Resource.SendFileUploadFileInfo,
          //      pf.FileName, pf.ContentLength,pf.ContentType);
            // Save the file 

            string fileName = Guid.NewGuid().ToString();

            string filePath = Server.MapPath("~/Downloads/" + fileName);
            fileUploaderAJAX1.SaveAs("~/Downloads",fileName);

           // fuClient.SaveAs(filePath);
            String sourceText = ParseUrl(Request.Url.AbsoluteUri) + "Downloads/" + fileName;
            String destText = "%VBA32%" + pf.FileName;
            String infoText = string.Format(Resources.Resource.SendFileUploadFileInfo,
                pf.FileName, pf.ContentLength, pf.ContentType);

            ScriptManager.RegisterStartupScript(this, GetType(), "aaa", "alert(2345); ", true);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "SetTxtValues", "SetTxtValues(\"" + sourceText + "\",\"" + destText + "\",\"" + infoText + "\"); ", true);
           // ScriptManager.RegisterStartupScript(this, GetType(), "SetTxtValues", "SetTxtValues(\"" + sourceText + "\",\"" + destText + "\",\"" + infoText + "\"); ", true);
           // tboxSource.Text = ParseUrl(Request.Url.AbsoluteUri) + "Downloads/" + fileName;
           // if (tboxDestination.Text == String.Empty)
           //     tboxDestination.Text = "%VBA32%" + pf.FileName;


    }
    #endregion

    #region Control LifeCycle
    protected override void OnInit(EventArgs e)
    {
        Visible = false;
        base.OnInit(e);
    }

    #endregion

    #region ITaskOptions
    public void LoadTaskEntity(TaskEntity entity)
    {
        LoadTaskEntity(ConvertTaskEntity(entity));
    }

    public TaskEntity SaveTaskEntity(TaskEntity oldEntity, out bool changed)
    {
        return SaveTaskEntity(ConvertTaskEntity(oldEntity), out changed);
    }

    public string DivOptionsClientID
    {
        get
        {
            return tskSendFile.ClientID;
        }
    }

    public Type TaskType
    {
        get { return typeof(SendFileTaskEntity); }
    }
    #endregion

    #region ITaskOptionsHelper
    public void LoadTaskEntity(SendFileTaskEntity entity)
    {
        lblDetails.Text = entity.Information;
        tboxSource.Text = entity.SourceFile;
        tboxDestination.Text = entity.DestinationFile;
    }

    public SendFileTaskEntity SaveTaskEntity(SendFileTaskEntity oldEntity, out bool changed)
    {
        SendFileTaskEntity entity = new SendFileTaskEntity();
        entity.Information = lblDetails.Text;
        entity.SourceFile = tboxSource.Text;
        entity.DestinationFile = tboxDestination.Text;
        changed = !oldEntity.Equals(entity);
        return entity;
    }

    public SendFileTaskEntity ConvertTaskEntity(TaskEntity entity)
    {
        SendFileTaskEntity processEntity = entity as SendFileTaskEntity;
        if ((entity as SendFileTaskEntity) == null)
        {
            processEntity = new SendFileTaskEntity();
        }
        return processEntity;
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



  /* private string tag_Details = "Details";
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

        ARM2_dbcontrol.Generation.XmlBuilder xml =
            new ARM2_dbcontrol.Generation.XmlBuilder("task");
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

        XmlTaskParser pars = new XmlTaskParser(task.Param);

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
}
*/