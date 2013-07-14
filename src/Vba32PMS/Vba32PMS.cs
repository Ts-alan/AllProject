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
        private Boolean isShutdown = false;
        private Boolean maintenanceEnabled = true;
        private String connectionString = String.Empty; //������ �����������
        private String path;                            //���� � ������
        private String server;                          //����������� ������
        private Int32 port=666;                           //����
        private Int32 maxFileLength=8000666;              //������������ ������ ������ � ���������(�������, ��� ������� ������� �� ����� ���� ������)
        private String filePrefix = "ETS-";             //������� ������


        private DateTime lastSelectDate;                //����� ��������� �������
        private DateTime lastSendDate;                  //����� ��������� �������� ������� ������
        private DateTime nextSendDate;                  //����� ������� �� ���������� 

        private Int32 deliveryTimeoutCheck=30;            //�������� ������
        private Int32 dataSendInterval = 66;              //�������� ������� ���������
        private Int32 daysToDelete = 66;
        private Int32 taskDaysToDelete = 66;
        private LogLevel allowLog = LogLevel.Info;
        private Int32 hourIntervalToSend = 4;             //�������� ������� ��������� � �����

        private String registryControlCenterKeyName;     //���� � ���������� ������ ���������� 
        private String machineName;                     //��� �����. ��� ���� ������ ����������� �� ������������ ����� �������
        private String ipAddress = String.Empty;        //IP ����� � ����� SystemInfo
        
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
                Logger.Path = path + AppDomain.CurrentDomain.FriendlyName + ".log";
                path += "PMSDeferredEvents\\";
                Logger.Info("Vba32PMS.Constructor()::���������");
            }
            catch (Exception ex)
            {
                Logger.Error(String.Format("Vba32PMS():: {0}", ex.Message));
            }
        }

        protected override void OnStart(String[] args)
        {
            try
            {
                Logger.Info("Vba32PMS.OnStart()::�������");

                machineName = Environment.MachineName;
                ipAddress = GetIP(machineName);

                //������� ���� � ���
                StringBuilder builder = new StringBuilder(128);
                builder.AppendFormat("NetBIOS ���: {0}, IP-�����: {1}", machineName, ipAddress);
                Logger.Info(builder.ToString());

                if (!ReadSettingsFromRegistry())
                {
                    Logger.Fatal("Vba32PMS.OnStart()::������ ��� �������� �������� �� �������. ���������� ������������� ����������");
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
                Logger.Error("Vba32PMS.OnStart()::" + ex.Message);
            }
        }

        protected override void OnStop()
        {
            Logger.Info("Vba32PMS.OnStop():: �������");

            StopService();
            base.OnStop();
        }

        protected override void OnShutdown()
        {
            Logger.Info("Vba32PMS.OnShutdown():: �������");

            StopService();
            base.OnShutdown();
        }

        private void StopService()
        {
            Logger.Debug("StopService():: �������");
            isShutdown = true;

            if (shutdownEvent != null)
                shutdownEvent.Set();

           if (thread != null)
                thread.Join(TimeSpan.FromSeconds(10));
        }

        #endregion

        protected void ServiceMain()
        {
            Logger.Debug("Vba32PMS.ServiceMain():: �������");
            Boolean signaled = false;
            Boolean returnCode = false;

            while (true)
            {
                returnCode = Execute();
                Logger.Debug("�������...");
                signaled = shutdownEvent.WaitOne(delay, true);
                if (signaled == true)
                {
                    Logger.Debug("������ ������ � ����������, ������� �� �����");
                    break;
                }
            }
        }

        protected Boolean Execute()
        {
            try
            {
                Logger.Debug("��������� ������������� ���������� ��������");
                if (IsReRead())
                    if(ReadSettingsFromRegistry())
                        SkipReRead();//��������� ������� �������, ������� ����

                if (!isShutdown)
                    CheckDeliveryState(connectionString);
                else
                    return false;

                if (maintenanceEnabled)
                {
                    Logger.Debug("2. ���������� ������ �� ���������������� ������..");
                    if (!isShutdown)
                        ConfigureAgent(connectionString);
                    else
                        return false;

                    Logger.Debug("3. ��������� ������������� ������� �������.. ");
                    Logger.Debug("DateTime.Now=" + DateTime.Now + " nextSendDate=" + nextSendDate);
                    if (DateTime.Compare(DateTime.Now, nextSendDate) == 1)
                    {
                        Logger.Debug("���������� �������� �������");
                        if (!DataBaseToXml())
                            Logger.Debug("����� DataBaseToXml ������ false");
                        if (SendSystemInfo())
                        {
                            if (!SendEventsFromFiles())
                                Logger.Debug("����� SendEventsFromFiles ������ false");
                            else
                            {
                                Logger.Debug("���������� �������� ���� ��������� �������");
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
                            Logger.Debug("����� SendSystemInfo ������ false");
                    }
                    else
                    {
                        Logger.Debug("������������� � ������� ������� ���");
                    }
                }

                    Logger.Debug("4. ������ �� ������ �������.. ");
                    if (!isShutdown)
                        ClearOldEvents(connectionString);
                    else
                        return false;

                    Logger.Debug("5. ������ �� ������ �����.. ");
                    if (!isShutdown)
                        ClearOldTasks(connectionString);
                    else
                        return false;

                /*
                    Logger.Debug("6. ������ ����... ");
                    if (!isShutdown)
                        CompressDB(connectionString);
                 */
                
            }
            catch(Exception ex)
            {
                Logger.Error("Vba32PMS.Execute()::" + ex.Message);
               return false;
            }
            return true;
        }
    }
}
