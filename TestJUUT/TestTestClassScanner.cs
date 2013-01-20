using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;

using JUUT.Core;
using JUUT.Core.Attributes;
using JUUT.Core.Impl;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT {

    [TestClass]
    public class TestTestClassScanner {

        [TestMethod]
        public void GetClassSetUp() {
            MethodInfo classSetUpMethod = TestClassScanner.GetClassSetUpOfTest(typeof(TestClassWithNoOrganizeMethods));
            AssertEx.That(classSetUpMethod, Is.Null());

            classSetUpMethod = TestClassScanner.GetClassSetUpOfTest(typeof(TestClass));
            AssertEx.That(classSetUpMethod, Is.EqualTo(typeof(TestClass).GetMethod("ClassSetUp")));

            AssertEx.That(() => TestClassScanner.GetClassSetUpOfTest(null), Throws.An<ArgumentNullException>());

            AssertEx.That(() => TestClassScanner.GetClassSetUpOfTest(typeof(NotAJUUTTest)), Throws.An<ArgumentException>());
            AssertEx.That(() => TestClassScanner.GetClassSetUpOfTest(typeof(TestClassWithNonStaticClassSetUp)), Throws.An<ArgumentException>());
            AssertEx.That(() => TestClassScanner.GetClassSetUpOfTest(typeof(TestClassWithOrganizeMethodsWithParameters)), Throws.An<ArgumentException>());
            AssertEx.That(() => TestClassScanner.GetClassSetUpOfTest(typeof(TestClassWithMoreThanOneOrganizeMethod)), Throws.An<ArgumentException>());
        }
        
        [TestMethod]
        public void GetTestSetUp() {
            MethodInfo testSetUpMethod = TestClassScanner.GetTestSetUpOfTest(typeof(TestClassWithNoOrganizeMethods));
            AssertEx.That(testSetUpMethod, Is.Null());
            
            testSetUpMethod = TestClassScanner.GetTestSetUpOfTest(typeof(TestClass));
            AssertEx.That(testSetUpMethod, Is.EqualTo(typeof(TestClass).GetMethod("SetUp")));

            AssertEx.That(() => TestClassScanner.GetTestSetUpOfTest(null), Throws.An<ArgumentNullException>());
            
            AssertEx.That(() => TestClassScanner.GetTestSetUpOfTest(typeof(NotAJUUTTest)), Throws.An<ArgumentException>());
            AssertEx.That(() => TestClassScanner.GetTestSetUpOfTest(typeof(TestClassWithOrganizeMethodsWithParameters)), Throws.An<ArgumentException>());
            AssertEx.That(() => TestClassScanner.GetTestSetUpOfTest(typeof(TestClassWithMoreThanOneOrganizeMethod)), Throws.An<ArgumentException>());
        }

        [JUUTTestClass]
        private class TestClass {

            [ClassSetUp]
            public static void ClassSetUp() { }
            [TestSetUp]
            public void SetUp() { }

            [SimpleTestMethod]
            public void Foo() { }
            [SimpleTestMethod]
            public void Bar() { }

            [TestTearDown]
            public void TearDown() { }
            [ClassTearDown]
            public static void ClassTearDown() { }

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

        }

        [JUUTTestClass]
        private class TestClassWithNoOrganizeMethods {
            
        }

        private class NotAJUUTTest {

            public void Foo() { }
            public void Bar() { }

        }

    }

}
