using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.DataBase;

namespace VirusBlokAda.CC.JSON
{
    public static class TreeJSONEntityConverter
    {
        public static TreeNodeJSONEntity ConvertToTreeNodeJsonEntity(
                        Group group,
                        Boolean? isChecked,
                        Boolean isExpanded,
                        Boolean isShortQTip)
        {
            String qtip = String.Empty;
            if (isShortQTip)
                qtip = GenerateShortQTIP(group);
            else
                qtip = GenerateQTIP(group);
            if(group.ID<0)
                return new TreeNodeJSONEntity(group.Name, String.Format("Group_{0}", group.ID), "root", qtip, new NodeState(isExpanded, isChecked, false));
            return new TreeNodeJSONEntity(group.Name, String.Format("Group_{0}", group.ID), "group", qtip,new NodeState(isExpanded,isChecked,false));
        }

        public static TreeNodeJSONEntity ConvertToTreeNodeJsonEntity(
                        ComputersEntity comp,
                        Boolean? isChecked,                       
                        Boolean isExpanded,
                        Boolean isShortQTip)
        {
            String nodeType;
            if (!String.IsNullOrEmpty(comp.OSName) && comp.OSName.ToLower().Contains("server"))
                nodeType = "server";
            else
                nodeType = "computer";

            String qtip = String.Empty;
            if (isShortQTip)
                qtip = GenerateShortQTIP(comp);
            else
                qtip = GenerateQTIP(comp);

            return new TreeNodeJSONEntity(comp.ComputerName, comp.ID.ToString(), nodeType, qtip, new NodeState(isExpanded, isChecked, false),comp.IPAddress,comp.OSName, null);
        }

        public static TreeNodeJSONEntity ConvertToTreeNodeJsonEntity(
                        ComputersEntityEx compEx,
                        Boolean? isChecked,                       
                        Boolean isExpanded,
                        Boolean isShortQTip)
        {
            String qtip = String.Empty;
            if (isShortQTip)
                qtip = GenerateShortQTIP(compEx);
            else
                qtip = GenerateQTIP(compEx);

            return new TreeNodeJSONEntity(compEx.ComputerName, compEx.ID.ToString(), GetComputerState(compEx), qtip, new NodeState(isExpanded, isChecked, false),compEx.IPAddress,compEx.OSName, null);
        }

        public static TreeNodeJSONEntity ConvertToTreeNodeJsonEntity(
                        Policy policy,
                        Boolean? isChecked,                        
                        Boolean isExpanded,
                        Boolean isShortQTip)
        {
            String qtip = String.Empty;
            if (isShortQTip)
                qtip = GenerateShortQTIP(policy);
            else
                qtip = GenerateQTIP(policy);

            return new TreeNodeJSONEntity(policy.Name, String.Format("Policy_{0}", policy.ID), "folder", qtip, new NodeState(isExpanded, isChecked, false));
        }

        public static TreeNodeJSONEntity ConvertToTreeNodeJsonEntity(
                        String Text,
                        Boolean? isChecked,
                        Boolean isLeaf,
                        Boolean isExpanded,
                        Boolean isShortQTip)
        {
            String nodeType;
            if (isLeaf)
                nodeType = "computer";
            else
                nodeType = "folder";

            return new TreeNodeJSONEntity(Text, Text, nodeType, Text, new NodeState(isExpanded, isChecked, false));
        }

        #region QTip
        private static String GenerateShortQTIP(ComputersEntity comp)
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("{0}: {1}<br />", "IPAddress", comp.IPAddress);
            result.AppendFormat("{0}: {1}<br />", "DomainName", comp.DomainName);
            result.AppendFormat("{0}: {1}", "OSType", TransformOS(comp.OSName));

            return result.ToString();
        }
        private static String GenerateShortQTIP(Group group)
        {
            return !String.IsNullOrEmpty(group.Comment) ? group.Comment : "No comment";
        }
        private static String GenerateShortQTIP(Policy policy)
        {
            return policy.Name;
        }
        private static String GenerateQTIP(ComputersEntity comp)
        {
            return "Generate normal qtip!";
        }
        private static String GenerateQTIP(Group group)
        {
            return "Generate normal qtip!";
        }
        private static String GenerateQTIP(Policy policy)
        {
            return policy.Name;
        }
        private static String TransformOS(String OSName)
        {
            StringBuilder result = new StringBuilder(OSName);
            result.Replace("Microsoft", "");
            result.Replace("Service Pack ", "SP");
            result.Replace("®", "");
            result.Replace("©", "");
            result.Replace("™", "");
            return result.ToString();
        }
        #endregion

        #region Icon style
        private static String GetComputerState(ComputersEntityEx entity)
        {
            TimeSpan time = DateTime.Now.Subtract(entity.RecentActive);
            if (time.Days != 0 || time.Hours >= 2) //Time active more than 2 hours
                return "computerGrey";

            if (entity.Components != null)
            {
                Boolean isMonitorOn = false;
                Boolean isLoaderOn = false;

                foreach (ComponentsEntity cmpt in entity.Components)
                {
                    if (cmpt.ComponentName == "Vba32 Loader")
                        isLoaderOn = (cmpt.ComponentState == "On");

                    if (cmpt.ComponentName == "Vba32 Monitor")
                        isMonitorOn = (cmpt.ComponentState == "On");
                }

                if (entity.Vba32KeyValid && entity.Vba32Integrity && isLoaderOn && isMonitorOn) return "computerGreen";
                else
                {
                    if (entity.Vba32KeyValid && entity.Vba32Integrity && isLoaderOn) return "computerYellow";
                    else
                    {
                        if (!entity.Vba32KeyValid || !entity.Vba32Integrity || !isLoaderOn) return "computerRed";
                    }
                }
            }

            return "computerGrey";
        }
        #endregion
    }
}
