using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NDbUnit.Core;
using ARM2_dbcontrol;
using VirusBlokAda.CC.DataBase;
using System.Data;

namespace WebConsoleTests.ARM2_DbControl.Database.Provider
{
    //[TestFixture]
    //class PPacketProviderTests
    //{
    //    INDbUnitTest database;
    //    DataSet ds;
    //    PParserProvider provider;
        
    //    [SetUp]
    //    public void Init()
    //    {
    //        provider = new PParserProvider(System.Configuration.ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
    //        database = new NDbUnit.Core.SqlClient.SqlDbUnitTest(System.Configuration.ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
    //        database.ReadXmlSchema(@"ARM2_DbControl\DataBase\TestXML\DataBase.xsd");
    //      //  database.PerformDbOperation(NDbUnit.Core.DbOperationFlag.CleanInsert);
    //        ds = database.GetDataSetFromDb();
    //    }
    //    [Test]
    //    [TestCase( "testComp", "192.168.1.1" ,"1", 2, 1,TestName="1")]
    //    [TestCase("testComp", "192.168.1.1", "1", 2, 1, TestName = "2")]
    //    [TestCase("testComp2", "192.168.1.2", "2", 2, 2, TestName = "3")]
    //    [TestCase("testComp3", "192.168.1.3", "3", 2, 1, TestName = "4")]
    //    [TestCase("testComp", "192.168.1.5", "4", 2, 1, TestName = "5")]
    //    public void InsertSystemInfoTest(String compName,String compIp,String CompMAC, Int16 licenseCount, Int32 compCount)
    //    {

    //        provider.InsertSystemInfo(new ComputersEntity() { ComputerName = compName, IPAddress = compIp, MacAddress = CompMAC }, licenseCount);
    //        ds = database.GetDataSetFromDb();
            
    //        Assert.AreEqual(ds.Tables["Computers"].Rows.Count, compCount);

    //    }
    //    [TestFixtureTearDown]
    //    public void TestTeardown()
    //    {
    //  //      database.PerformDbOperation(NDbUnit.Core.DbOperationFlag.DeleteAll);
    //    }

    //}
}
