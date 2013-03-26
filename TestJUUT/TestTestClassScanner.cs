using System;
using System.Collections.Generic;
using System.Reflection;

using JUUT.Core.Attributes;
using JUUT.Core.Attributes.Methods;
using JUUT.Core.Scanners;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT {

    [TestClass]
    public class TestTestClassScanner {

        [TestMethod]
        public void GetClassSetUp() {
            MethodInfo classSetUpMethod = TestClassScanner.GetClassSetUpOfTestClass(typeof(TestClassWithNoJUUTMethods));
            AssertEx.That(classSetUpMethod, Is.Null());

            classSetUpMethod = TestClassScanner.GetClassSetUpOfTestClass(typeof(TestClassMock));
            AssertEx.That(classSetUpMethod, Is.EqualTo(typeof(TestClassMock).GetMethod("MockSetUp")));

            AssertEx.That(() => TestClassScanner.GetClassSetUpOfTestClass(null), Throws.An<ArgumentNullException>());

            AssertEx.That(() => TestClassScanner.GetClassSetUpOfTestClass(typeof(NotAJUUTTest)), Throws.An<ArgumentException>());
            AssertEx.That(() => TestClassScanner.GetClassSetUpOfTestClass(typeof(TestClassWithNonStaticClassOrganizeMethods)), Throws.An<ArgumentException>());
            AssertEx.That(() => TestClassScanner.GetClassSetUpOfTestClass(typeof(TestClassWithMethodsWithParameters)), Throws.An<ArgumentException>());
            AssertEx.That(() => TestClassScanner.GetClassSetUpOfTestClass(typeof(TestClassWithMoreThanOneOrganizeMethod)), Throws.An<ArgumentException>());
        }
        
        [TestMethod]
        public void GetTestSetUp() {
            MethodInfo testSetUpMethod = TestClassScanner.GetTestSetUpOfTestClass(typeof(TestClassWithNoJUUTMethods));
            AssertEx.That(testSetUpMethod, Is.Null());

            testSetUpMethod = TestClassScanner.GetTestSetUpOfTestClass(typeof(TestClassMock));
            AssertEx.That(testSetUpMethod, Is.EqualTo(typeof(TestClassMock).GetMethod("MockTestSetUp")));

            AssertEx.That(() => TestClassScanner.GetTestSetUpOfTestClass(null), Throws.An<ArgumentNullException>());
            
            AssertEx.That(() => TestClassScanner.GetTestSetUpOfTestClass(typeof(NotAJUUTTest)), Throws.An<ArgumentException>());
            AssertEx.That(() => TestClassScanner.GetTestSetUpOfTestClass(typeof(TestClassWithMethodsWithParameters)), Throws.An<ArgumentException>());
            AssertEx.That(() => TestClassScanner.GetTestSetUpOfTestClass(typeof(TestClassWithMoreThanOneOrganizeMethod)), Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void GetTestTearDown() {
            MethodInfo testTearDownMethod = TestClassScanner.GetTestTearDownOfTestClass(typeof(TestClassWithNoJUUTMethods));
            AssertEx.That(testTearDownMethod, Is.Null());

            testTearDownMethod = TestClassScanner.GetTestTearDownOfTestClass(typeof(TestClassMock));
            AssertEx.That(testTearDownMethod, Is.EqualTo(typeof(TestClassMock).GetMethod("MockTestTearDown")));

            AssertEx.That(() => TestClassScanner.GetTestTearDownOfTestClass(null), Throws.An<ArgumentNullException>());

            AssertEx.That(() => TestClassScanner.GetTestTearDownOfTestClass(typeof(NotAJUUTTest)), Throws.An<ArgumentException>());
            AssertEx.That(() => TestClassScanner.GetTestTearDownOfTestClass(typeof(TestClassWithMethodsWithParameters)), Throws.An<ArgumentException>());
            AssertEx.That(() => TestClassScanner.GetTestTearDownOfTestClass(typeof(TestClassWithMoreThanOneOrganizeMethod)), Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void GetClassTearDown() {
            MethodInfo classTearDownMethod = TestClassScanner.GetClassTearDownMethodOfClass(typeof(TestClassWithNoJUUTMethods));
            AssertEx.That(classTearDownMethod, Is.Null());

            classTearDownMethod = TestClassScanner.GetClassTearDownMethodOfClass(typeof(TestClassMock));
            AssertEx.That(classTearDownMethod, Is.EqualTo(typeof(TestClassMock).GetMethod("MockTearDown")));

            AssertEx.That(() => TestClassScanner.GetClassTearDownMethodOfClass(null), Throws.An<ArgumentNullException>());

            AssertEx.That(() => TestClassScanner.GetClassTearDownMethodOfClass(typeof(NotAJUUTTest)), Throws.An<ArgumentException>());
            AssertEx.That(() => TestClassScanner.GetClassTearDownMethodOfClass(typeof(TestClassWithMethodsWithParameters)), Throws.An<ArgumentException>());
            AssertEx.That(() => TestClassScanner.GetClassTearDownMethodOfClass(typeof(TestClassWithMoreThanOneOrganizeMethod)), Throws.An<ArgumentException>());
        }

        [TestMethod]
        public void GetSimpleTestMethods() {
            List<MethodInfo> simpleTestMethods = TestClassScanner.GetSimpleTestMethodsOfClass(typeof(TestClassWithNoJUUTMethods));
            AssertEx.That(simpleTestMethods.Count == 0, Is.True(), "The amount of test methods should be 0.");

            simpleTestMethods = TestClassScanner.GetSimpleTestMethodsOfClass(typeof(TestClassMock));
            foreach (MethodInfo testMethod in simpleTestMethods) {
                AssertEx.That(testMethod, Matches.AnyOf(Is.EqualTo(typeof(TestClassMock).GetMethod("FirstTestMethod")),
                                                        Is.EqualTo(typeof(TestClassMock).GetMethod("SecondTestMethod"))));
            }

            AssertEx.That(() => TestClassScanner.GetSimpleTestMethodsOfClass(null), Throws.An<ArgumentNullException>());

            AssertEx.That(() => TestClassScanner.GetSimpleTestMethodsOfClass(typeof(NotAJUUTTest)), Throws.An<ArgumentException>());
            AssertEx.That(() => TestClassScanner.GetSimpleTestMethodsOfClass(typeof(TestClassWithMethodsWithParameters)), Throws.An<ArgumentException>());
        }

        [JUUTTestClass]
        private class TestClassWithMoreThanOneOrganizeMethod {

            [ClassSetUp]
            public static void ClassSetUp1() { }
            [ClassSetUp]
            public static void ClassSetUp2() { }

            [TestSetUp]
            public void SetUp1() { }
            [TestSetUp]
            public void SetUp2() { }

            [TestTearDown]
            public void TearDown1() { }
            [TestTearDown]
            public void TearDown2() { }

            [ClassTearDown]
            public void ClassTearDown1() { }
            [ClassTearDown]
            public void ClassTearDown2() { }

        }

        [JUUTTestClass]
        private class TestClassWithNoJUUTMethods {
            
        }

        private class NotAJUUTTest {

            public void Foo() { }
            public void Bar() { }

        }

    }

}
