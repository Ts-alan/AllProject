using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpectedObjects;
using ListOfProcess.Controllers;
using ListOfProcess.Services;
using Moq;
using NUnit.Framework;
using ListOfProcess.ConnectionLogic;
using ListOfProcess.Models;

namespace ListOfProcess.Tests.Nunit
{
    [TestFixture]
    public class NunitTests
    {
        [Test]
        public void LocalComputer()
        {
            //Организация
            ConnectionLogic.ConnectionLogic n = new ConnectionLogic.ConnectionLogic();
            HomeController controller = new HomeController(n);

            //Действие
            List<object> result = (List<object>)controller.LocalComputer().Model;

            //Утверждение
            Assert.IsNotNull(result);
        }
        [Test]
        public void RemoteComputer()
        {
            //Организация
            IEnumerable<object> list = new List<object> { "Kayak", "Soccer", "Stadium" };
            Mock<IConnection> mock = new Mock<IConnection>();
            Exception Ex;
            mock.Setup(m => m.GetProcessOnRemoteMachine(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), out  Ex))
                .Returns(list);
            HomeController controller = new HomeController(mock.Object);
            RemoteModel model = new RemoteModel() { Ip = "192.168.1.10", Login = "den4ik", Password = "werra" };

            //Действие
            List<object> result = (List<object>)controller.RemoteComputer(model).Model;


            //Утверждение
            Object[] proObjects = result.ToArray();
            Assert.IsTrue(proObjects.Length == 3);
            Assert.AreEqual(proObjects[0].ToString(), "Kayak");
        }

        [Test]
        public void StartProcess()
        {
            //Организация
            Mock<IConnection> mock = new Mock<IConnection>();
            HomeController controller = new HomeController(mock.Object);
            string[] mass = new string[] { "Kayak", "Soccer", "Stadium" };
            
            //Действие
            controller.StartProcess(mass);

            //Утверждение
            mock.Verify(m => m.StartProcess("Kayak"), Times.Exactly(1));
        }



    }


}

