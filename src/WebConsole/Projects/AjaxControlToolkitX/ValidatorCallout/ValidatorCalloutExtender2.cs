using System;
using System.Collections.Generic;
using System.Text;
using AjaxControlToolkit;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: WebResource("AjaxControlToolkit.ValidatorCallout.ValidatorCalloutBehavior2.js", "text/javascript")]
[assembly: WebResource("AjaxControlToolkit.ValidatorCallout.ValidatorCallout2.css", "text/css", PerformSubstitution = true)]

namespace AjaxControlToolkit
{
    [RequiredScript(typeof(CommonToolkitScripts))]
    [RequiredScript(typeof(PopupExtender))]
    [RequiredScript(typeof(AnimationExtender))]
    [TargetControlType(typeof(IValidator))]
    [ClientCssResource("AjaxControlToolkit.ValidatorCallout.ValidatorCallout2.css")]
    [ClientScriptResource("AjaxControlToolkit.ValidatorCalloutBehavior2", "AjaxControlToolkit.ValidatorCallout.ValidatorCalloutBehavior2.js")]
    //This functionallity is included in AjaxControlToolkit 3.0.30930.0 and subsequent releases
    //due to the project requirement of .Net 2.0 and corresponding AjaxControlToolkit v. 1.0.20229.0
    //this client library is required
    public class ValidatorCalloutExtender2: ValidatorCalloutExtender
    {
        [ExtenderControlProperty]
        [ClientPropertyName("popupPosition")]
        [DefaultValue(ValidatorCalloutPosition.Right)]
        [Description("Indicates where you want the ValidatorCallout displayed.")]
        public virtual ValidatorCalloutPosition PopupPosition
        {
            get { return GetPropertyValue("PopupPosition", ValidatorCalloutPosition.Right); }
            set { SetPropertyValue("PopupPosition", value); }
        }
    }
}
