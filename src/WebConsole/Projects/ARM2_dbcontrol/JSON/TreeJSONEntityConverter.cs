﻿using System;
using System.Collections.Generic;
using System.Text;
using ARM2_dbcontrol.DataBase;
using VirusBlokAda.Vba32CC.Groups;
using VirusBlokAda.Vba32CC.Policies.General;
using VirusBlokAda.Vba32CC.JSON.Entities;

namespace VirusBlokAda.Vba32CC.JSON
{
    public static class TreeJSONEntityConverter
    {
        public static TreeNodeJSONEntity ConvertToTreeNodeJsonEntity(
                        Group group,
                        Boolean? isChecked,
                        Boolean isAllowDrag,
                        Boolean isAllowDrop,
                        Boolean isLeaf,
                        Boolean isExpanded,
                        Boolean isShortQTip)
        {
            QtipJSONEntity qtip = null;
            if (isShortQTip)
                qtip = new QtipJSONEntity(GenerateShortQTIP(group));
            else
                qtip = new QtipJSONEntity(GenerateQTIP(group));

            return new TreeNodeJSONEntity(group.Name, String.Format("Group_{0}", group.ID), "group", qtip, isChecked, isAllowDrag, isAllowDrop,
                                          isLeaf, isExpanded);
        }

        public static TreeNodeJSONEntity ConvertToTreeNodeJsonEntity(
                        ComputersEntity comp,
                        Boolean? isChecked,
                        Boolean isAllowDrag,
                        Boolean isAllowDrop,
                        Boolean isLeaf,
                        Boolean isExpanded,
                        Boolean isShortQTip)
        {
            String iconStyle;
            if (!String.IsNullOrEmpty(comp.OSName) && comp.OSName.ToLower().Contains("server"))
                iconStyle = "server";
            else
                iconStyle = "computer";

            QtipJSONEntity qtip = null;
            if (isShortQTip)
                qtip = new QtipJSONEntity(GenerateShortQTIP(comp));
            else
                qtip = new QtipJSONEntity(GenerateQTIP(comp));

            return new TreeNodeJSONEntity(comp.ComputerName, comp.ID.ToString(), iconStyle, qtip, isChecked, isAllowDrag, isAllowDrop,
                                          isLeaf, isExpanded, null);
        }

        public static TreeNodeJSONEntity ConvertToTreeNodeJsonEntity(
                        ComputersEntityEx compEx,
                        Boolean? isChecked,
                        Boolean isAllowDrag,
                        Boolean isAllowDrop,
                        Boolean isLeaf,
                        Boolean isExpanded,
                        Boolean isShortQTip)
        {
            QtipJSONEntity qtip = null;
            if (isShortQTip)
                qtip = new QtipJSONEntity(GenerateShortQTIP(compEx));
            else
                qtip = new QtipJSONEntity(GenerateQTIP(compEx));

            return new TreeNodeJSONEntity(compEx.ComputerName, compEx.ID.ToString(), GetComputerState(compEx), qtip, isChecked, isAllowDrag, isAllowDrop,
                                          isLeaf, isExpanded, null, new CompAdditionalInfo(compEx));
        }

        public static TreeNodeJSONEntity ConvertToTreeNodeJsonEntity(
                        Policy policy,
                        Boolean? isChecked,
                        Boolean isAllowDrag,
                        Boolean isAllowDrop,
                        Boolean isLeaf,
                        Boolean isExpanded,
                        Boolean isShortQTip)
        {
            QtipJSONEntity qtip = null;
            if (isShortQTip)
                qtip = new QtipJSONEntity(GenerateShortQTIP(policy));
            else
                qtip = new QtipJSONEntity(GenerateQTIP(policy));

            return new TreeNodeJSONEntity(policy.Name, String.Format("Policy_{0}", policy.ID), "folder", qtip, isChecked, isAllowDrag, isAllowDrop,
                                          isLeaf, isExpanded);
        }

        public static TreeNodeJSONEntity ConvertToTreeNodeJsonEntity(
                        String Text,
                        Boolean? isChecked,
                        Boolean isAllowDrag,
                        Boolean isAllowDrop,
                        Boolean isLeaf,
                        Boolean isExpanded,
                        Boolean isShortQTip)
        {
            String iconStyle;
            if (isLeaf)
                iconStyle = "computer";
            else
                iconStyle = "folder";
            
            return new TreeNodeJSONEntity(Text, Text, iconStyle, new QtipJSONEntity(Text), isChecked, isAllowDrag, isAllowDrop,
                                          isLeaf, isExpanded);
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