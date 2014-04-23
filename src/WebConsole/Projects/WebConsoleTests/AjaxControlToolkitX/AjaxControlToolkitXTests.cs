using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using AjaxControlToolkit;

namespace WebConsoleTests.AjaxControlToolkitX
{
    [TestFixture]
    public class AjaxControlToolkitXTests
    {
        private ValidatorCalloutExtender2 vce;

        [SetUp]
        public void Init()
        {
            vce = new ValidatorCalloutExtender2();
        }

        [TestCase(ValidatorCalloutPosition.Left, ValidatorCalloutPosition.Left)]
        [TestCase(ValidatorCalloutPosition.BottomRight, ValidatorCalloutPosition.BottomRight)]
        [TestCase(null, ValidatorCalloutPosition.Right)]
        public void ValidatorCalloutTest(ValidatorCalloutPosition sp, ValidatorCalloutPosition gp)
        {
            vce.PopupPosition = sp;
            Assert.That(vce.PopupPosition, Is.EqualTo(gp));
        }
    }
}
