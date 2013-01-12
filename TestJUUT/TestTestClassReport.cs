using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Collections.Generic;

using JUUT.Core;
using JUUT.Core.Impl.Attributes;
using JUUT.Core.Impl.Reports;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

namespace TestJUUT {

    [TestClass]
    public class TestTestClassReport {

        private MethodInfo ClassSetUp;
        private MethodInfo ClassTearDown;
        private MethodInfo SetUp;
        private MethodInfo TearDown;

        [TestInitialize]
        public void InitializeTheMethodInfos() {
            Type testOwnerMock = typeof(TestOwnerMock);

            ClassSetUp = testOwnerMock.GetMethod("ClassSetUp");
            ClassTearDown = testOwnerMock.GetMethod("ClassTearDown");
            SetUp = testOwnerMock.GetMethod("SetUp");
            TearDown = testOwnerMock.GetMethod("TearDown");
        }

        [TestMethod]
        public void Creation() {
            Exception raisedException = new NullReferenceException("Exception text");

            Report report = new TestClassReport(ClassSetUp, raisedException);
            AssertEx.That(report.TestClassType, Is.EqualTo(typeof(TestOwnerMock)));
            AssertEx.That(report.Range, Is.EqualTo(ReportRange.TestClass));

            //Test for illegal null arguments
            AssertEx.That(() => { new TestClassReport(null, raisedException); }, Throws.An<ArgumentException>());
            AssertEx.That(() => { new TestClassReport(ClassTearDown, null); }, Throws.An<ArgumentException>());

            //Test that a TestClassReport can only be initiated with a method with the ClassSetUp, TestSetUp, TestTearDown or ClassTearDown attribute.
            MethodInfo illegalMethod = new DynamicMethod("MethodWithoutAttribute", null, null);
            AssertEx.That(() => { new TestClassReport(illegalMethod, raisedException); }, Throws.An<ArgumentException>());
        }

        [JUUTTestClass]
        private class TestOwnerMock {

            [ClassSetUp]
            public static void ClassSetUp() { }
            [TestSetUp]
            public void SetUp() { }

            [TestTearDown]
            public void TearDown() { }
            [ClassTearDown]
            public static void ClassTearDown() { }

        }

    }

}
