using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;


public static class ControlsExtensions
{
        /// <summary>        
        /// recursively finds a child control of the specified parent.    
        /// ! change to extension method if upgraded to .Net 3.0
        /// </summary>        
        /// <param name="control"></param>        
        /// <param name="id"></param>        
        /// <returns></returns>         
        public static Control FindControlRecursive(Control control, string id)        
        {            
            if (control == null) return null;            
            //try to find the control at the current level      
            Control ctrl = control.FindControl(id);       
            if (ctrl == null)           
            {               
                //search the children         
                foreach (Control child in control.Controls)      
                {                    
                    ctrl = FindControlRecursive(child, id);   
                    if (ctrl != null) break;                
                }            
            }             
            return ctrl;       
        }

        /// <summary>        
        /// recursively finds a child control on the page.    
        /// ! change to extension method if upgraded to 3.0
        /// </summary>        
        /// <param name="control"></param>        
        /// <param name="id"></param>        
        /// <returns></returns>         
        public static Control FindControlRecursive(Page page, string id)
        {
            return FindControlRecursive(page.Form, id);
        }

        public static Control FindControl(Control control, string controlID)
        {
            Control namingContainer = control;
            Control result = null;
            if (control != control.Page)
            {
                while ((result == null) && (namingContainer != control.Page))
                {
                    namingContainer = namingContainer.NamingContainer;
                    if (namingContainer == null)
                    {
                        throw new HttpException(String.Format(
                            "The {0} control '{1}' does not have a naming container. Ensure that the control is added to the page.",
                            control.GetType().Name, control.ID));
                    }
                    result = namingContainer.FindControl(controlID);
                }
                return result;
            }
            return control.FindControl(controlID);
        }

 

}