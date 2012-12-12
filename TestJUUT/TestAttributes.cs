using System;
using System.Collections.Generic;
using System.Reflection;

using JUUT.Attributes;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHamcrest.Core;
using Is = NHamcrest.Core.Is;

namespace TestJUUT {
    [TestClass]
    public class TestAttributes {

        [TestMethod]
        public void JUUTTest() {
            Attribute JUUTTest = typeof (JUUTTestAttributedMock).GetCustomAttribute(typeof (JUUTTestAttribute), true);
            AssertEx.That(JUUTTest, Is.NotNull());

            JUUTTest = typeof(NotAttributedMock).GetCustomAttribute(typeof(JUUTTestAttribute), true);
            AssertEx.That(JUUTTest, Is.Null());
        }

        [TestMethod]
        public void ClassSetUp() {
            List<MethodInfo> classSetUps = GetMethodsOfTypeWithAttribute(typeof(JUUTTestAttributedMock), typeof(ClassSetUpAttribute));
            AssertEx.That(classSetUps.Count, Is.Not(0), "Registered no class set up method.");
            AssertEx.That(classSetUps.Count, Is.EqualTo(1), "Registered more than one class set up method.");
            AssertEx.That(classSetUps[0].Name, Is.EqualTo("MockSetUp"));

            classSetUps = GetMethodsOfTypeWithAttribute(typeof(NotAttributedMock), typeof(ClassSetUpAttribute));
            AssertEx.That(classSetUps.Count, Is.EqualTo(0), "Registered a class set up method in NotAttributedMock.");
        }

        [TestMethod]
        public void ClassTearDown() {
            List<MethodInfo> classTearDowns = GetMethodsOfTypeWithAttribute(typeof(JUUTTestAttributedMock), typeof(ClassTearDownAttribute));
            AssertEx.That(classTearDowns.Count, Is.Not(0), "Registered no class tear down method.");
            AssertEx.That(classTearDowns.Count, Is.EqualTo(1), "Registered more than one class tear down method.");
            AssertEx.That(classTearDowns[0].Name, Is.EqualTo("MockTearDown"));

            classTearDowns = GetMethodsOfTypeWithAttribute(typeof(NotAttributedMock), typeof(ClassTearDownAttribute));
            AssertEx.That(classTearDowns.Count, Is.EqualTo(0), "Registered a class tear down method in NotAttributedMock.");
        }

        [TestMethod]
        public void TestSetUp() {
            List<MethodInfo> testSetUps = GetMethodsOfTypeWithAttribute(typeof(JUUTTestAttributedMock), typeof(TestSetUpAttribute));
            AssertEx.That(testSetUps.Count, Is.Not(0), "Registered nor test set up method.");
            AssertEx.That(testSetUps.Count, Is.EqualTo(1), "Registered more than one test set up method.");
            AssertEx.That(testSetUps[0].Name, Is.EqualTo("MockTestSetUp"));

            testSetUps = GetMethodsOfTypeWithAttribute(typeof(NotAttributedMock), typeof(ClassTearDownAttribute));
            AssertEx.That(testSetUps.Count, Is.EqualTo(0), "Registered a test set up method in NotAttributedMock.");
        }

        private List<MethodInfo> GetMethodsOfTypeWithAttribute(Type type, Type attribute) {
            List<MethodInfo> methods = new List<MethodInfo>();

            foreach (MethodInfo method in type.GetMethods()) {
                if (method.GetCustomAttribute(attribute) != null) {
                    methods.Add(method);
                }
            }

            return methods;
        }

        [JUUTTest]
        private class JUUTTestAttributedMock {

            [ClassSetUp]
            public void MockSetUp() { }
            [ClassTearDown]
            public void MockTearDown() { }
            
        }

        private class NotAttributedMock {

            public void Foo() {}
            public void Bar() {}

        }
    }
}
