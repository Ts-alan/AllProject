using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NDbUnit.Core;
using ARM2_dbcontrol;
using VirusBlokAda.CC.DataBase;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace WebConsoleTests.ARM2_DbControl.Database.Provider
{
    [TestFixture]
    class ComputerProviderTests
    {

        INDbUnitTest database;
        DataSet ds;
        PParserProvider providerPP;
        ComputerProvider providerComp;
        String ConnectionString = DBHelper.connectionString;
        [SetUp]
        public void Init()
        {
            providerPP = new PParserProvider(System.Configuration.ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
            providerComp = new ComputerProvider(System.Configuration.ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);

            database = new NDbUnit.Core.SqlClient.SqlDbUnitTest(System.Configuration.ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
            database.ReadXmlSchema(@"ARM2_DbControl\DataBase\TestXML\DataBase.xsd");

            database.PerformDbOperation(NDbUnit.Core.DbOperationFlag.CleanInsert);

            DBHelper.InitDataBase();


            ds = database.GetDataSetFromDb();
        }



        [Test]      
        public void ComputerTest()
        {
            //Insert
            InsertComputerTest<SqlException>(new ComputersEntity() { ComputerName = "testComp", IPAddress = "192.168.1.1", MacAddress = "1", OSName = "Win7"},2,1,false);
            InsertComputerTest<SqlException>(new ComputersEntity() { ComputerName = "testComp", IPAddress = "192.168.1.11", MacAddress = "1", OSName = "Win7" },2,1,false);
            InsertComputerTest<SqlException>(new ComputersEntity() { ComputerName = "testComp2", IPAddress = "192.168.1.2", MacAddress = "2" }, 2,2,false);
            InsertComputerTest<SqlException>(new ComputersEntity() { ComputerName = "testComp3", IPAddress = "192.168.1.1", MacAddress = "3" }, 2,0, true);
            InsertComputerTest<SqlException>(new ComputersEntity() { ComputerName = "testComp", IPAddress = "192.168.1.3", MacAddress = "4" }, 5,0, true);


            //GetRegisteredCompListTest
            List<String> ipAddress = new List<String>();
            ipAddress.Add("192.168.1.11");
            ipAddress.Add("192.168.1.2");
            GetRegisteredCompListTest(ipAddress);

            //Count
            providerPP.InsertSystemInfo(new ComputersEntity() { ComputerName = "testComp4", IPAddress = "192.168.1.12", MacAddress = "4",OSName="Win7" }, 5);

            CountComputerTest<SqlException>("OSName = 'Win7'", 2, false);
            CountComputerTest<SqlException>("OsName = 'Win8'", 0, false);
            CountComputerTest<SqlException>("Os = 'Win8'", 0, true);
            CountComputerTest<SqlException>("qweqew", 0, true);

            //GetComputerID
            GetComputerIDTest("testComp");
            GetComputerIDTest("NonTestComp");

            //GetComputer
            GetComputerTest("testComp");
            GetComputerTest("NonTestComp");

            //GetComputer is full
            /*ComputerAdditionalEntity additional = new ComputerAdditionalEntity() { ControlDeviceType = ControlDeviceTypeEnum.RCS, IsControllable = true, IsRenamed = false, PreviousComputerName = "adminComp" };
            ComputersEntity comp = new ComputersEntity() { ComputerName="testComp5",ControlCenter=true,CPUClock=5,Description="testComp Description", DomainName="domainName", IPAddress="192.168.2.2",
                                                           LatestInfected=DateTime.Now,LatestMalware="malware", LatestUpdate=DateTime.Now, MacAddress="a:a:a:a", OSName="Win7", OSTypeID=1, RAM=100,
                                                           RecentActive=DateTime.Now, UserLogin="admin", Vba32Integrity=true, Vba32KeyValid=false, Vba32Version="3",AdditionalInfo=additional  };
            providerPP.InsertSystemInfo(comp, 10);

            GetComputerIsFullTest(comp);*/

            //DeleteTest
            DeleteComputerTest("testComp4", 2);
            DeleteComputerTest("NonTestComp", 2);



            //UpdateDescriptionTest
         //   UpdateDescriptionTest("testComp", "newDesc");

            
        }



        

        private void GetRegisteredCompListTest(List<String>result)
        {
            Assert.AreEqual(providerComp.GetRegisteredCompList(), result);
        }

        private void InsertComputerTest<T>(ComputersEntity comp, short license, int result, Boolean exceptionExpected) where T : Exception
        {
            if (!exceptionExpected)
            {
                providerPP.InsertSystemInfo(comp, license);
                ds = database.GetDataSetFromDb();

                Assert.AreEqual(ds.Tables["Computers"].Rows.Count, result);
            }
            else
            {
                Assert.Throws<T>(delegate { providerPP.InsertSystemInfo(comp, license); });
            }
        }

        private void CountComputerTest<T>(String query, int result, Boolean exceptionExpected) where T : Exception
        {
            if (!exceptionExpected)
            {
                Assert.AreEqual(providerComp.Count(query), result);
            }
            else Assert.Throws<T>(delegate { providerComp.Count(query); });
        }

        private void GetComputerIDTest(String compName)
        {
            Int16 id=-1;
            foreach(DataRow row in ds.Tables["Computers"].Rows)
            {
                if((String)row.ItemArray[1]==compName)
                    id=(Int16)row.ItemArray[0];
            }
            Assert.AreEqual(providerComp.GetComputerID(compName), id);
        }

        private void DeleteComputerTest(String compName, int result)
        {
            Int16 id = providerComp.GetComputerID(compName);
            providerComp.Delete(id);
            Assert.AreEqual(ds.Tables["Computers"].Rows.Count, result);
        }

        private void UpdateDescriptionTest(String compName, String description)
        {
            Int16 id = providerComp.GetComputerID(compName);
            providerComp.UpdateDescription(id, description);
            Assert.AreEqual(ds.Tables["Computers"].Rows[0], description);
        }

        private void GetComputerTest(String compName)
        {
            Int16 id = providerComp.GetComputerID(compName);
            ComputersEntity comp = providerComp.GetComputer(id);
            if (id == -1)
            {
                Assert.IsNull(comp);
            }
            else
                Assert.AreEqual(comp.ComputerName, compName);

        }

        private void GetComputerIsFullTest(ComputersEntity comp)
        {
            Int16 id = providerComp.GetComputerID(comp.ComputerName);
            ComputersEntity comp2 = providerComp.GetComputer(id);

            Boolean result = true;

         /*   result = result && comp.ComputerName == comp2.ComputerName;
            result = result && comp.ControlCenter == comp2.ControlCenter;
            result = result && comp.CPUClock == comp2.CPUClock;
            result = result && comp.Description == comp2.Description;
            result = result && comp.DomainName == comp2.DomainName;
            result = result && comp.IPAddress == comp2.IPAddress;
            result = result && comp.LatestInfected == comp2.LatestInfected;
            result = result && comp.LatestMalware == comp2.LatestMalware;
            result = result && comp.LatestUpdate == comp2.LatestUpdate;
            result = result && comp.MacAddress == comp2.MacAddress;*/


            Assert.IsTrue(result);
        }
        /*
        [Test]
        public void InsertProcessInfoTest()
        {

            provider.InsertSystemInfo(new ComputersEntity() { ComputerName = "testComp", IPAddress = "192.168.1.1", MacAddress = "1" }, 2);
            provider.InsertProcessInfo("testComp", "testProc", 1);

            ds = database.GetDataSetFromDb();

            Assert.AreEqual(ds.Tables["Processes"].Rows.Count, 1);

            provider.InsertProcessInfo("testComp", "testProc2", 1);

            ds = database.GetDataSetFromDb();

            Assert.AreEqual(ds.Tables["Processes"].Rows.Count, 2);

            provider.InsertSystemInfo(new ComputersEntity() { ComputerName = "testComp2", IPAddress = "192.168.1.1", MacAddress = "2" }, 2);
            provider.InsertProcessInfo("testComp2", "testProc", 1);

            ds = database.GetDataSetFromDb();

            Assert.AreEqual(ds.Tables["Processes"].Rows.Count, 3);

            Assert.Throws<SqlException>(delegate { provider.InsertProcessInfo("testComp3", "testProc3", 1); }); 

        }

        [Test]
        public void DeleteProcessInfoTest()
        {

            provider.InsertSystemInfo(new ComputersEntity() { ComputerName = "testComp", IPAddress = "192.168.1.1", MacAddress = "1" }, 2);
            provider.InsertProcessInfo("testComp", "testProc1", 1);
            provider.InsertProcessInfo("testComp", "testProc2", 1);
            provider.InsertProcessInfo("testComp", "testProc3", 1);

            provider.InsertSystemInfo(new ComputersEntity() { ComputerName = "testComp2", IPAddress = "192.168.1.2", MacAddress = "2" }, 2);
            provider.InsertProcessInfo("testComp2", "testProc1", 1);
            provider.InsertProcessInfo("testComp2", "testProc4", 1);
            ds = database.GetDataSetFromDb();

            Assert.AreEqual(ds.Tables["Processes"].Rows.Count, 5);

            provider.DeleteProcessInfo("testComp");

            ds = database.GetDataSetFromDb();

            Assert.AreEqual(ds.Tables["Processes"].Rows.Count, 2);

            Assert.Throws<SqlException>(delegate { provider.DeleteProcessInfo("NonExistingComp"); });

        }
        [Test]
        [TestCase("comp1","192.168.1.1","1")]
        [TestCase("comp2","192.168.1.2","2")]
        [TestCase("comp2","192.168.1.11","2")]
        public void GetComputerIPAdressTest(String compName,String IP,String Mac)
        {

            provider.InsertSystemInfo(new ComputersEntity() { ComputerName = compName, IPAddress = IP, MacAddress = Mac}, 2);
            
            Assert.AreEqual(provider.GetComputerIPAddress(compName),IP);          

        }
        [Test]
        public void GetComputerIPAdressExceptionTest()
        {
            Assert.Throws<NullReferenceException>(delegate { provider.GetComputerIPAddress("NonExistingComp"); });
        }
        [Test]
        public void InsertEventWithoutNotifyTest()
        {
            provider.InsertSystemInfo(new ComputersEntity() { ComputerName = "testComp", IPAddress = "192.168.1.1", MacAddress = "1" }, 2);
            provider.InsertEventWithoutNotify(new EventsEntity() { ComputerName = "testComp", EventName = "event", ComponentName = "cmponent", EventTime = DateTime.Now, Comment = "comment" });

            DateTime dt = new DateTime(2005, 05, 05);
            provider.InsertEventWithoutNotify(new EventsEntity() { ComputerName = "testComp", EventName = "vba32.program.update.success", ComponentName = "cmponent", EventTime = dt, Comment = "comment" });


            ds = database.GetDataSetFromDb();
            Assert.AreEqual(ds.Tables["Events"].Rows.Count, 2);
            Assert.AreEqual((DateTime)ds.Tables["Computers"].Rows[0]["LatestUpdate"], dt);

            String comment = "VirFound";
            provider.InsertEventWithoutNotify(new EventsEntity() { ComputerName = "testComp", EventName = "vba32.virus.found", ComponentName = "cmponent", EventTime = dt, Comment = comment });
            ds = database.GetDataSetFromDb();
            Assert.AreEqual((DateTime)ds.Tables["Computers"].Rows[0]["LatestInfected"], dt);
            Assert.AreEqual(ds.Tables["Computers"].Rows[0]["LatestMalware"], comment);

        }

        [Test]
        public void InsertEventTest()
        {
            provider.InsertSystemInfo(new ComputersEntity() { ComputerName = "testComp", IPAddress = "192.168.1.1", MacAddress = "1" }, 2);
            provider.InsertEvent(new EventsEntity() { ComputerName = "testComp", EventName = "event", ComponentName = "cmponent", EventTime = DateTime.Now, Comment = "comment" },2);

          

            ds = database.GetDataSetFromDb();
            Assert.AreEqual(ds.Tables["Events"].Rows.Count, 1);

           
            Assert.Throws<SqlException>(delegate
            {
                provider.InsertEvent(new EventsEntity() { ComputerName = "testComp2", EventName = "vba32.device.inserted", ComponentName = "cmp", EventTime = DateTime.Now, Comment = "Comment" }, 2);
            });

            provider.InsertEvent(new EventsEntity() { ComputerName = "testComp", EventName = "vba32.device.inserted", ComponentName = "cmp", EventTime = DateTime.Now, Comment = "Comment", Object="222" }, 2);
            ds = database.GetDataSetFromDb();
            Assert.AreEqual(ds.Tables["Events"].Rows.Count, 2);
            

            
            provider.InsertEvent(new EventsEntity() { ComputerName = "testComp", EventName = "vba32.device.inserted", ComponentName = "cmp", EventTime = DateTime.Now, Comment = "Comment", Object = "222" }, 2);
            ds = database.GetDataSetFromDb();
            Assert.AreEqual(ds.Tables["Events"].Rows.Count, 2);

        }
        */
        [TestFixtureTearDown]
        public void TestTeardown()
        {
        /*    database.PerformDbOperation(NDbUnit.Core.DbOperationFlag.DeleteAll);
            DBHelper.InitDataBase();*/
                   
        }

    }
}
