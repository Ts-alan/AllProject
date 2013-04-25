using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

using System.Runtime.InteropServices;
using System.Threading;
using System.IO;

using Vba32.ControlCenter.PeriodicalMaintenanceService.Xml;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService
{
    public partial class Vba32PMS : ServiceBase
    {
        //������
        private Thread thread;
        private ManualResetEvent shutdownEvent;
        private TimeSpan delay;

        #region Variables
        private bool isShutdown = false;
        private bool maintenanceEnabled = true;
        private string connectionString = String.Empty; //������ �����������
        private string path;                            //���� � ������
        private string server;                          //����������� ������
        private int port=666;                           //����
        private int maxFileLength=8000666;              //������������ ������ ������ � ���������(�������, ��� ������� ������� �� ����� ���� ������)
        private string filePrefix = "ETS-";             //������� ������


        private DateTime lastSelectDate;                //����� ��������� �������
        private DateTime lastSendDate;                  //����� ��������� �������� ������� ������
        private DateTime nextSendDate;                  //����� ������� �� ���������� 

        private int deliveryTimeoutCheck=30;            //�������� ������
        private int dataSendInterval = 66;              //�������� ������� ���������
        private int daysToDelete = 66;
        private int taskDaysToDelete = 66;
        private int allowLog = 0;                       //��������, ����� ���������� ������������ ��� ������ ������������
        private int hourIntervalToSend = 4;             //�������� ������� ��������� � �����

        private string registryControlCenterKeyName;     //���� � ���������� ������ ���������� 
        private string machineName;                     //��� �����. ��� ���� ������ ����������� �� ������������ ����� �������
        private string ipAddress = String.Empty;        //IP ����� � ����� SystemInfo

        private Logger log;                             //�����, ����������� ���������� � ���� �� ����� ������-���������
        
        #endregion


        #region �����������, OnStart, OnStop
        public Vba32PMS()
        {
            InitializeComponent();
            try
            {
                //�������� �� �������� ��
                if (Marshal.SizeOf(typeof(IntPtr)) == 8)
                    registryControlCenterKeyName = "SOFTWARE\\Wow6432Node\\Vba32\\ControlCenter\\";
                else
                    registryControlCenterKeyName = "SOFTWARE\\Vba32\\ControlCenter\\";

                path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName)+"\\";
                log = new Logger(path + AppDomain.CurrentDomain.FriendlyName + ".log", Encoding.Default, true);
                path += "PMSDeferredEvents\\";
                LogMessage("Vba32PMS.Constructor()::���������");
            }
            catch (Exception ex)
            {
                string errorMessage = "Vba32PMS():: " + ex.Message;
                LogMessage(errorMessage);
                EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName, errorMessage, EventLogEntryType.Error);   
            }

        }

        protected override void OnStart(string[] args)
        {
            try
            {
                LogMessage("Vba32PMS.OnStart()::�������");

                machineName = Environment.MachineName;
                ipAddress = GetIP(machineName);

                //������� ���� � ���
                StringBuilder builder = new StringBuilder(128);
                builder.AppendFormat("NetBIOS ���: {0}, IP-�����: {1}", machineName, ipAddress);
                LogMessage(builder.ToString());

                if (!ReadSettingsFromRegistry())
                {
                    LogError("Vba32PMS.OnStart()::������ ��� �������� �������� �� �������. ���������� ������������� ����������",
                        EventLogEntryType.Error);
                    return;
                }

                delay = new TimeSpan(0, 0, deliveryTimeoutCheck);

                ThreadStart ts = new ThreadStart(this.ServiceMain);
                shutdownEvent = new ManualResetEvent(false);
                thread = new Thread(ts);
                thread.Start();
            }
            catch (Exception ex)
            {
                LogError("Vba32PMS.OnStart()::" + ex.Message,
                    EventLogEntryType.Error);
            }
        }

        protected override void OnStop()
        {
            LogMessage("Vba32PMS.OnStop():: �������");

            StopService();
            base.OnStop();
        }

        protected override void OnShutdown()
        {
            LogMessage("Vba32PMS.OnShutdown():: �������");

            StopService();
            base.OnShutdown();
        }

        private void StopService()
        {
            isShutdown = true;

            if (shutdownEvent != null)
                shutdownEvent.Set();

           if (thread != null)
                thread.Join(TimeSpan.FromSeconds(10));
        }

        #endregion

        protected void ServiceMain()
        {
            LogMessage("Vba32PMS.ServiceMain():: �������");
            bool signaled = false;
            bool returnCode = false;

            while (true)
            {
                returnCode = Execute();
                signaled = shutdownEvent.WaitOne(delay, true);
                if (signaled == true)
                {
                    //LogMessage("������ ������ � ����������, ������� �� �����");
                    break;
                }
                
            }
        }

        protected bool Execute()
        {
            try
            {
                //��������� ������������� ���������� ��������..
                if (IsReRead())
                    if(ReadSettingsFromRegistry())
                        SkipReRead();//��������� ������� �������, ������� ����

                if (!isShutdown)
                    CheckDeliveryState(connectionString);
                else
                    return false;

                if (maintenanceEnabled)
                {
                    // LogMessage("1. ������ ������ �����.. ");
                    
                    //LogMessage("2. ��������� ������������� ������� �������.. ");
                    LogMessage("DateTime.Now=" + DateTime.Now + " nextSendDate=" + nextSendDate);
                    if (DateTime.Compare(DateTime.Now, nextSendDate) == 1)
                    {
                        LogMessage("���������� �������� �������");
                        if (!DataBaseToXml())
                            LogMessage("����� DataBaseToXml ������ false");
                        if (SendSystemInfo())
                        {
                            if (!SendEventsFromFiles())
                                LogMessage("����� SendEventsFromFiles ������ false");
                            else
                            {
                                LogMessage("���������� �������� ���� ��������� �������");
                                while (DateTime.Compare(DateTime.Now, nextSendDate) == 1)
                                {
                                    switch (dataSendInterval)
                                    {
                                        case 0:
                                            nextSendDate = nextSendDate.AddDays(1);
                                            break;
                                        case 1:
                                            nextSendDate = nextSendDate.AddDays(7);
                                            break;
                                        case 2:
                                            nextSendDate = nextSendDate.AddMonths(1);
                                            break;
                                        case 3:
                                            nextSendDate = nextSendDate.AddHours(hourIntervalToSend);
                                            break;
                                        default:
                                            nextSendDate = nextSendDate.AddDays(1);
                                            break;
                                    }
                                }
                                WriteSettingsToRegistry();
                            }
                        }
                        else
                            LogMessage("����� SendSystemInfo ������ false");
                    }
                    else
                    {
                        LogMessage("������������� � ������� ������� ���");
                    }
                }

                    //LogMessage("3. ������ �� ������ �������.. ");
                    if (!isShutdown)
                        ClearOldEvents(connectionString);
                    else
                        return false;

                    //LogMessage("4. ������ �� ������ �����.. ");
                    if (!isShutdown)
                        ClearOldTasks(connectionString);
                    else
                        return false;

                    //LogMessage("5. ������ ����... ");
                    if (!isShutdown)
                        CompressDB(connectionString);
                
            }
            catch(Exception ex)
            {
                LogError("Vba32PMS.Execute()::" + ex.Message,
                    EventLogEntryType.Error);
               return false;
            }
            
            return true;
        }

        #region Logging
        private void LogError(string errorMessage, EventLogEntryType eventLogType)
        {
            try
            {
                Debug.WriteLine(errorMessage);
                log.Write(errorMessage);
                EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName, errorMessage, eventLogType);
            }
            catch
            {
            }

        }

        private void LogMessage(string errorMessage)
        {
            try
            {
                Debug.WriteLine(errorMessage);
                if (allowLog > 0)
                {
                    log.Write(errorMessage);
                }
            }
            catch
            {
            }
        }
        #endregion

    }
}
