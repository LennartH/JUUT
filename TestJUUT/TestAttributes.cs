using System;
using System.Collections.Generic;
using System.Reflection;

using JUUT.Core.Impl.Attributes;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Is = NHamcrest.Core.Is;
using Matches = NHamcrest.Core.Matches;

namespace TestJUUT {
    [TestClass]
    public class TestAttributes {

        [TestMethod]
        public void JUUTTest() {
            Attribute JUUTTest = typeof (AttributedMock).GetCustomAttribute(typeof (JUUTTestClassAttribute), true);
            AssertEx.That(JUUTTest, Is.NotNull());

            JUUTTest = typeof(NotAttributedMock).GetCustomAttribute(typeof(JUUTTestClassAttribute), true);
            AssertEx.That(JUUTTest, Is.Null());
        }

        [TestMethod]
        public void ClassSetUp() {
            List<MethodInfo> classSetUps = GetMethodsOfTypeWithAttribute(typeof(AttributedMock), typeof(ClassSetUpAttribute));
            AssertEx.That(classSetUps.Count, Is.Not(0), "Registered no class set up method.");
            AssertEx.That(classSetUps.Count, Is.EqualTo(1), "Registered more than one class set up method.");
            AssertEx.That(classSetUps[0].Name, Is.EqualTo("MockSetUp"));

            classSetUps = GetMethodsOfTypeWithAttribute(typeof(NotAttributedMock), typeof(ClassSetUpAttribute));
            AssertEx.That(classSetUps.Count, Is.EqualTo(0), "Registered a class set up method in NotAttributedMock.");
        }

        [TestMethod]
        public void ClassTearDown() {
            List<MethodInfo> classTearDowns = GetMethodsOfTypeWithAttribute(typeof(AttributedMock), typeof(ClassTearDownAttribute));
            AssertEx.That(classTearDowns.Count, Is.Not(0), "Registered no class tear down method.");
            AssertEx.That(classTearDowns.Count, Is.EqualTo(1), "Registered more than one class tear down method.");
            AssertEx.That(classTearDowns[0].Name, Is.EqualTo("MockTearDown"));

            classTearDowns = GetMethodsOfTypeWithAttribute(typeof(NotAttributedMock), typeof(ClassTearDownAttribute));
            AssertEx.That(classTearDowns.Count, Is.EqualTo(0), "Registered a class tear down method in NotAttributedMock.");
        }

        [TestMethod]
        public void TestSetUp() {
            List<MethodInfo> testSetUps = GetMethodsOfTypeWithAttribute(typeof(AttributedMock), typeof(TestSetUpAttribute));
            AssertEx.That(testSetUps.Count, Is.Not(0), "Registered no test set up method.");
            AssertEx.That(testSetUps.Count, Is.EqualTo(1), "Registered more than one test set up method.");
            AssertEx.That(testSetUps[0].Name, Is.EqualTo("MockTestSetUp"));

            testSetUps = GetMethodsOfTypeWithAttribute(typeof(NotAttributedMock), typeof(TestSetUpAttribute));
            AssertEx.That(testSetUps.Count, Is.EqualTo(0), "Registered a test set up method in NotAttributedMock.");
        }

        [TestMethod]
        public void TestTearDown() {
            List<MethodInfo> testTearDowns = GetMethodsOfTypeWithAttribute(typeof(AttributedMock), typeof(TestTearDownAttribute));
            AssertEx.That(testTearDowns.Count, Is.Not(0), "Registered no test tear down method.");
            AssertEx.That(testTearDowns.Count, Is.EqualTo(1), "Registered more than one test tear down method.");
            AssertEx.That(testTearDowns[0].Name, Is.EqualTo("MockTestTearDown"));

            testTearDowns = GetMethodsOfTypeWithAttribute(typeof(NotAttributedMock), typeof(TestTearDownAttribute));
            AssertEx.That(testTearDowns.Count, Is.EqualTo(0), "Registered a test tear down method in NotAttributedMock.");
        }

        [TestMethod]
        public void SimpleTestMethod() {
            List<MethodInfo> simpleTests = GetMethodsOfTypeWithAttribute(typeof (AttributedMock), typeof (SimpleTestMethodAttribute));
            AssertEx.That(simpleTests.Count, Is.Not(0), "No simple test methods found in " + typeof(AttributedMock).Name + ".");
            foreach (MethodInfo simpleTest in simpleTests) {
                AssertEx.That(simpleTest.Name, Matches.AnyOf(Is.EqualTo("FirstTestMethod"), Is.EqualTo("SecondTestMethod")));
            }

            simpleTests = GetMethodsOfTypeWithAttribute(typeof(NotAttributedMock), typeof(SimpleTestMethodAttribute));
            AssertEx.That(simpleTests.Count, Is.EqualTo(0), "Found a simple test method in " + typeof(NotAttributedMock).Name + ".");
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

        [JUUTTestClass]
        private class AttributedMock {

            [ClassSetUp]
            public void MockSetUp() { }
            [TestSetUp]
            public void MockTestSetUp() { }

            [SimpleTestMethod]
            public void FirstTestMethod() { }
            [SimpleTestMethod]
            public void SecondTestMethod() { }

            [TestTearDown]
            public void MockTestTearDown() { }
            [ClassTearDown]
            public void MockTearDown() { }
            
        }

        private class NotAttributedMock {

            public void Foo() { }
            public void Bar() { }

        }
    }
}
