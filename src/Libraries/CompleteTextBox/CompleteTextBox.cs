using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.ComponentModel;
namespace CompleteTextBox
{
    public class CompleteTextBox : System.Web.UI.WebControls.TextBox
    {
        private string dataURL = @"HttpHandlers\EventDDLHandler.ashx?prefix=";//Здесь хранится путь к источнику даннных и название параметра
        private string field = "EventName";
        /// <summary>
        /// URL с которого будет получена информация (по умолчанию DropDownListHandler.ashx?prefix=)
        /// </summary>
        [Browsable(true),
         Category("Misc"),
         Description("URL с которого будет получена информация (по умолчанию \"DropDownListHandler.ashx?prefix=\")"),
        DefaultValue("DropDownListHandler.ashx?prefix=")]
        
        public string DataURL
        {
            get { return this.dataURL; }
            set { this.dataURL = value; }
        }

        public string Field
        {
            get { return this.field; }
            set { this.field = value; }
        }


        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            string SelectName = base.ClientID + "_Selector"; //Определяем название контрола который будет содержать список подстановки

            writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.Position, "absolute");//Устанавливаем стиль Position:absolute
            writer.AddAttribute(System.Web.UI.HtmlTextWriterAttribute.Id, SelectName); //Задаем имя элименту
            writer.AddAttribute("onkeydown", string.Format("SelectItem(this,'{0}',false,event);", base.ClientID)); //Устанавливаем обработчик события onkeydown
            writer.AddAttribute("ondblclick", string.Format("SelectItem(this,'{0}',true,event);", base.ClientID)); //Устанавливаем обработчик события ondblclick
            
            writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.Display, "none"); //В текущем представлении элемент не будет показан
            writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Select); //Элемент будет <select>
            writer.RenderEndTag(); //Закрываем элемент для отображения списка подстановки
            writer.AddAttribute("AUTOCOMPLETE", "off"); //Устанавливаем параметр AUTOCOMPLETE="off", для того что бы поле не сохраняло предыдущие значения
            writer.AddAttribute("onkeyup", string.Format("SendData(this,'{0}',event);", SelectName)); //Устанавливаем обработчик события onkeyup
            writer.AddAttribute("onkeydown", string.Format("return TextKeyDown(this,'{0}',event);", SelectName));//Устанавливаем обработчик события onkeydown
            base.Render(writer); //Рендрим из этого всего HTML
            //Регистрируем скрипт необходимый для работы всей этой системы
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CompleteScript", ResourceScript.JavaScript.Replace("{$server$}", dataURL), true); 
        }
    }
}
