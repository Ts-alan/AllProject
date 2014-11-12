using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using Vba32.ControlCenter.NotificationService;
using Microsoft.Win32;
using VirusBlokAda.CC.DataBase;

namespace Vba32CC
{
    public partial class PacketParser
    {
        #region Notification functions

        private const String mc_ipc_url = "ipc://Vba32NS/Vba32NS.rem";

        private Boolean OnEventInsert(StringDictionary name_value_map)
        {
            Boolean result = true;
            try
            {
                String ip_address = "";
                result = GetComputerIPAddress(name_value_map["Computer"], ref ip_address);
                if (!result || ip_address == null || ip_address == "")
                {
                    return result;
                }
                else
                {
                    name_value_map.Add("IPAddress", ip_address);
                }

                /*if (name_value_map["EventName"] == "vba32.device.inserted")
                {
                    OnDeviceInsert(new EventsEntity(name_value_map));
                }*/

                Boolean notification_is_needed = false;
                result = GetEventTypeNotify(name_value_map["EventName"], ref notification_is_needed);

                if ((!result || !notification_is_needed) &&
                    (name_value_map["EventName"] != virusFoundEvent))
                {
                    return result;
                }


                if (IsNeedSendNotify(ref name_value_map))
                {
                    //virus.found has been detected, but not it marked to send and no epidemy
                    if ((name_value_map["EventName"] == virusFoundEvent) &&
                         (!notification_is_needed))
                        return result;

                    LoggerPP.log.Debug("Need send event " + name_value_map["EventName"]);
                    INotification notifier = (Vba32.ControlCenter.NotificationService.INotification)Activator.GetObject(
                        typeof(Vba32.ControlCenter.NotificationService.INotification), mc_ipc_url);



                    notifier.OnRegisteredMessage(name_value_map);
                }
                else
                {
                    LoggerPP.log.Debug("Sending event was skipped: " + name_value_map["EventName"]);
                }
            }
            catch (Exception exception)
            {
                m_error_info = exception.Message;
                LoggerPP.log.Error(m_error_info);
                result = false;
            }
            return result;
        }

        // Get computer ip-address by computer name
        private Boolean GetComputerIPAddress(String computer_name, ref String ip_address)
        {
            Boolean result = true;
            try
            {
                ip_address = db.GetComputerIPAddress(computer_name);
            }
            catch (Exception exception)
            {
                m_error_info = exception.Message;
                result = false;
            }
            return result;
        }

        // Check if the notification is needed
        private Boolean GetEventTypeNotify(String event_name, ref Boolean notification_is_needed)
        {
            Boolean result = true;
            try
            {
                notification_is_needed = db.GetEventTypeNotify(event_name);
            }
            catch (Exception exception)
            {
                m_error_info = exception.Message;
                result = false;
            }
            return result;
        }

        #endregion

        #region Flow analysis

        private const String notifyRegistryKey = "SOFTWARE\\Vba32\\ControlCenter\\Notification";
        private const String globalEpidemyEvent = "vba32.cc.GlobalEpidemy";
        private const String localHearthEvent = "vba32.cc.LocalHearth";
        private const String virusFoundEvent = "vba32.virus.found";

        private Boolean globalNotifyStatus = false; //Поле оптимизировать

        private Boolean IsNeedSendNotify(ref StringDictionary dic)
        {
            LoggerPP.log.Debug("IsNeedSendNotify is called");

            LoggerPP.log.Debug("read values from registry...");

            int useFlowAnalysis = ReadRegistryDwordValue(
                Registry.LocalMachine,
                notifyRegistryKey,
                "UseFlowAnalysis");

            //Не используем обработку потока уведомлений
            if ((useFlowAnalysis < 1) || (useFlowAnalysis == 10)) return true;

            db.GlobalEpidemyLimit = ReadRegistryDwordValue(
                Registry.LocalMachine,
                notifyRegistryKey,
                "GlobalEpidemyLimit");
            //Debug.WriteLine("GlobalEpidemyLimit is " + flow.GlobalEpidemyLimit);

            db.GlobalEpidemyTimeLimit = ReadRegistryDwordValue(
                Registry.LocalMachine,
                notifyRegistryKey,
                "GlobalEpidemyTimeLimit");
            //Debug.WriteLine("GlobalEpidemyTimeLimit is " + flow.GlobalEpidemyTimeLimit);

            db.LocalHearthLimit = ReadRegistryDwordValue(
                Registry.LocalMachine,
                notifyRegistryKey,
                "LocalHearthLimit");
            // Debug.WriteLine("LocalHearthLimit is " + flow.LocalHearthLimit);

            db.LocalHearthTimeLimit = ReadRegistryDwordValue(
                Registry.LocalMachine,
                notifyRegistryKey,
                "LocalHearthTimeLimit");
            //Debug.WriteLine("LocalHearthTimeLimit is " + flow.LocalHearthTimeLimit);

            db.Limit = ReadRegistryDwordValue(
                Registry.LocalMachine,
                notifyRegistryKey,
                "Limit");
            LoggerPP.log.Debug("Limit is " + db.Limit);

            db.TimeLimit = ReadRegistryDwordValue(
                Registry.LocalMachine,
                notifyRegistryKey,
                "TimeLimit");
            // Debug.WriteLine("TimeLimit is " + flow.TimeLimit);

            db.GlobalEpidemyCompCount = ReadRegistryDwordValue(
               Registry.LocalMachine,
               notifyRegistryKey,
               "GlobalEpidemyCompCount");
            LoggerPP.log.Debug("TimeLimit is " + db.GlobalEpidemyCompCount);

            EventsEntity ev = new EventsEntity(dic);

            LoggerPP.log.Debug("Event is " + ev.EventName);

            if (ev.EventName == virusFoundEvent)
            {
                //Впервые обнаружена глобальная эпидемия
                Boolean gResult = db.IsGlobalEpidemy(ev);
                if (gResult && (!globalNotifyStatus))
                {
                    LoggerPP.log.Debug("is first vba32.cc.GlobalEpidemy");
                    globalNotifyStatus = true;
                    dic["EventName"] = globalEpidemyEvent;
                    dic["Object"] = db.GlobalEpidemyCompList;
                    try
                    {
                        db.InsertEventWithoutNotify(new EventsEntity(dic));
                    }
                    catch (Exception e)
                    {
                        LoggerPP.log.Error("IsNeedSendNotify() :: " + e.Message);
                    }
                    return true;
                }
                else
                    if (!gResult)
                    {
                        globalNotifyStatus = false;
                    }
                    else
                    {
                        LoggerPP.log.Debug("is older vba32.cc.GlobalEpidemy");
                        return false;
                    }
                /* //Повторно обнаружена глоб. эпидемия. Уведомлять не следует
                 if (gResult && (!flow.IsNeedSendGlobalEpidemyWarning))
                 {
                     Debug.WriteLine("is older vba32.cc.GlobalEpidemy");
                     return false;
                 }*/

                Boolean lResult = db.IsLocalHearth(ev);

                //Впервые обнаружен очаг заражения.
                if (lResult && (db.IsNeedSendLocalHearthWarning))
                {
                    LoggerPP.log.Debug("is first vba32.cc.LocalHearth");
                    dic["EventName"] = localHearthEvent;
                    try
                    {
                        db.InsertEventWithoutNotify(new EventsEntity(dic));
                    }
                    catch(Exception e)
                    {
                        LoggerPP.log.Error("IsNeedSendNotify() :: " + e.Message);
                    }
                    return true;
                }
                else
                    //Повторно обнаружен очаг заражения. Уведомлять не следует
                    if (lResult && (!db.IsNeedSendLocalHearthWarning))
                    {
                        LoggerPP.log.Debug("is older vba32.cc.LocalHearth");
                        return false;
                    }

                //Обрубим дальнейшую обработку события vba32.virus.found
                LoggerPP.log.Debug("is simply virus.found event");
                return true;
            }

            LoggerPP.log.Debug("flow analysis...");
            return db.FlowAnalysis(ev);
        }

        /// <summary>
        /// Считывает из реестра dword значение
        /// </summary>
        /// <param name="registry_key">Ветка реестра</param>
        /// <param name="sub_key">Раздел</param>
        /// <param name="var_name">ключ</param>
        /// <returns>Значение:10 в случае ошибки</returns>
        private Int32 ReadRegistryDwordValue(RegistryKey registry_key, String sub_key, String var_name)
        {
            RegistryKey reg_key = registry_key;
            try
            {
                reg_key = reg_key.OpenSubKey(sub_key);
                Int32? tmp = (Int32?)reg_key.GetValue(var_name);

                if (tmp.HasValue)
                    return tmp.Value;

            }
            catch (Exception e)
            {
                m_error_info = e.Message;
            }
            return 10;
        }

        #endregion
    }
}
