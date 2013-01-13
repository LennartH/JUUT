using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;

using JUUT.Core;
using JUUT.Core.Impl.Attributes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

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

            AssertEx.That(() => TestClassScanner.GetClassSetUpOfTest(typeof(ClassWithNoTests)), Throws.An<ArgumentException>());
            AssertEx.That(() => TestClassScanner.GetClassSetUpOfTest(typeof(TestClassWithNonStaticClassSetUp)), Throws.An<ArgumentException>());
            AssertEx.That(() => TestClassScanner.GetClassSetUpOfTest(typeof(TestClassWithClassSetUpWithParameters)), Throws.An<ArgumentException>());
            AssertEx.That(() => TestClassScanner.GetClassSetUpOfTest(typeof(TestClassWithMoreThanOneClassSetUps)), Throws.An<ArgumentException>());
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
        private class TestClassWithNonStaticClassSetUp {

            [ClassSetUp]
            public void ClassSetUp() { }

        }

        [JUUTTestClass]
        private class TestClassWithClassSetUpWithParameters {

            [ClassSetUp]
            public static void ClassSetUp(object parameter) { }

        }

        [JUUTTestClass]
        private class TestClassWithMoreThanOneClassSetUps {

            [ClassSetUp]
            public static void ClassSetUp1() { }
            [ClassSetUp]
            public static void ClassSetUp2() { }

        }

        [JUUTTestClass]
        private class TestClassWithNoOrganizeMethods {
            
        }

        private class ClassWithNoTests {

            public void Foo() { }
            public void Bar() { }

        }

    }

}
