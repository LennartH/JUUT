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
        private MethodInfo TestSetUp;
        private MethodInfo TestTearDown;

        [TestInitialize]
        public void InitializeTheMethodInfos() {
            Type testOwnerMock = typeof(TestOwnerMock);

            ClassSetUp = testOwnerMock.GetMethod("ClassSetUp");
            ClassTearDown = testOwnerMock.GetMethod("ClassTearDown");
            TestSetUp = testOwnerMock.GetMethod("SetUp");
            TestTearDown = testOwnerMock.GetMethod("TearDown");
        }

        [TestMethod]
        public void Creation() {
            Exception raisedException = new NullReferenceException("Exception text");

            Report report = new TestClassReport(ClassSetUp, raisedException);
            AssertEx.That(report.TestClass, Is.EqualTo(typeof(TestOwnerMock)));
            AssertEx.That(report.Range, Is.EqualTo(ReportRange.TestClass));

            //Test for illegal null arguments
            AssertEx.That(() => { new TestClassReport(null, raisedException); }, Throws.An<ArgumentException>());
            AssertEx.That(() => { new TestClassReport(ClassTearDown, null); }, Throws.An<ArgumentException>());

            //Test that a TestClassReport can only be initiated with a method with the ClassSetUp, TestSetUp,
            //TestTearDown or ClassTearDown attribute.
            MethodInfo illegalMethod = new DynamicMethod("MethodWithoutAttribute", null, null);
            AssertEx.That(() => { new TestClassReport(illegalMethod, raisedException); }, Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void MessageCreation() {
            Exception raisedException = new NullReferenceException("Exception text");
            string testClassName = typeof(TestOwnerMock).Name;

            Report report = new TestClassReport(ClassSetUp, raisedException);
            AssertEx.That(report.Text, Is.EqualTo("The ClassSetUp-Method " + ClassSetUp.Name + " of the test class " +
                                                  testClassName + " raised an exception: " + raisedException.Message));

            report = new TestClassReport(TestSetUp, raisedException);
            AssertEx.That(report.Text, Is.EqualTo("The TestSetUp-Method " + TestSetUp.Name + " of the test class " + testClassName +
                                                  " raised an exception: " + raisedException.Message));

            report = new TestClassReport(TestTearDown, raisedException);
            AssertEx.That(report.Text, Is.EqualTo("The TestTearDown-Method " + TestTearDown.Name + " of the test class " + testClassName +
                                                  " raised an exception: " + raisedException.Message));

            report = new TestClassReport(ClassTearDown, raisedException);
            AssertEx.That(report.Text, Is.EqualTo("The ClassTearDown-Method " + ClassTearDown.Name + " of the test class " + testClassName +
                                                  " raised an exception: " + raisedException.Message));
        }

        [TestMethod]
        public void ReportToString() {
            Exception raisedException = new NullReferenceException("Exception text");
            Report report = new TestClassReport(ClassSetUp, raisedException);
            AssertEx.That(report.ToString(), Is.EqualTo("Class wide report of " + typeof(TestOwnerMock).Name + ": " + report.Text));
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
