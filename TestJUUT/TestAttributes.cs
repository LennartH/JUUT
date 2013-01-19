using System;
using System.Collections.Generic;
using System.Reflection;

using JUUT.Core.Attributes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

namespace TestJUUT {
    [TestClass]
    public class TestAttributes {

        [TestMethod]
        public void JUUTAttribute() {
            JUUTAttribute JUUTAttribute = typeof(AttributedMock).GetCustomAttribute<JUUTAttribute>();
            AssertEx.That(JUUTAttribute, Is.NotNull(), "Registered no JUUT-Attribute method.");

            List<MethodInfo> attributedMethods = GetMethodsOfTypeWithAttribute(typeof(AttributedMock), typeof(JUUTAttribute));
            AssertEx.That(attributedMethods.Count, Is.EqualTo(6), "There should be 6 attributed methods in " + typeof(AttributedMock).Name);
        }

        [TestMethod]
        public void JUUTTestClass() {
            JUUTAttribute JUUTTest = typeof(AttributedMock).GetCustomAttribute<JUUTTestClassAttribute>();
            AssertEx.That(JUUTTest, Is.NotNull());

            AssertEx.That(JUUTTest.GetName(), Is.EqualTo("JUUTTestClass"));
            AssertEx.That(JUUTTest.IsSetUpOrTearDown(), Is.False());

            JUUTTest = typeof(NotAttributedMock).GetCustomAttribute<JUUTTestClassAttribute>();
            AssertEx.That(JUUTTest, Is.Null());
        }

        [TestMethod]
        public void ClassSetUp() {
            List<MethodInfo> classSetUps = GetMethodsOfTypeWithAttribute(typeof(AttributedMock), typeof(ClassSetUpAttribute));
            AssertEx.That(classSetUps.Count, Is.Not(0), "Registered no class set up method.");
            AssertEx.That(classSetUps.Count, Is.EqualTo(1), "Registered more than one class set up method.");
            AssertEx.That(classSetUps[0], Is.EqualTo(typeof(AttributedMock).GetMethod("MockSetUp")));

            JUUTAttribute classSetUp = classSetUps[0].GetCustomAttribute<ClassSetUpAttribute>();
            AssertEx.That(classSetUp.GetName(), Is.EqualTo("ClassSetUp"));
            AssertEx.That(classSetUp.IsSetUpOrTearDown(), Is.True());

            classSetUps = GetMethodsOfTypeWithAttribute(typeof(NotAttributedMock), typeof(ClassSetUpAttribute));
            AssertEx.That(classSetUps.Count, Is.EqualTo(0), "Registered a class set up method in NotAttributedMock.");
        }

        [TestMethod]
        public void ClassTearDown() {
            List<MethodInfo> classTearDowns = GetMethodsOfTypeWithAttribute(typeof(AttributedMock), typeof(ClassTearDownAttribute));
            AssertEx.That(classTearDowns.Count, Is.Not(0), "Registered no class tear down method.");
            AssertEx.That(classTearDowns.Count, Is.EqualTo(1), "Registered more than one class tear down method.");
            AssertEx.That(classTearDowns[0], Is.EqualTo(typeof(AttributedMock).GetMethod("MockTearDown")));

            JUUTAttribute classTearDown = classTearDowns[0].GetCustomAttribute<ClassTearDownAttribute>();
            AssertEx.That(classTearDown.GetName(), Is.EqualTo("ClassTearDown"));
            AssertEx.That(classTearDown.IsSetUpOrTearDown(), Is.True());

            classTearDowns = GetMethodsOfTypeWithAttribute(typeof(NotAttributedMock), typeof(ClassTearDownAttribute));
            AssertEx.That(classTearDowns.Count, Is.EqualTo(0), "Registered a class tear down method in NotAttributedMock.");
        }

        [TestMethod]
        public void TestSetUp() {
            List<MethodInfo> testSetUps = GetMethodsOfTypeWithAttribute(typeof(AttributedMock), typeof(TestSetUpAttribute));
            AssertEx.That(testSetUps.Count, Is.Not(0), "Registered no test set up method.");
            AssertEx.That(testSetUps.Count, Is.EqualTo(1), "Registered more than one test set up method.");
            AssertEx.That(testSetUps[0], Is.EqualTo(typeof(AttributedMock).GetMethod("MockTestSetUp")));

            JUUTAttribute testSetUp = testSetUps[0].GetCustomAttribute<TestSetUpAttribute>();
            AssertEx.That(testSetUp.GetName(), Is.EqualTo("TestSetUp"));
            AssertEx.That(testSetUp.IsSetUpOrTearDown(), Is.True());

            testSetUps = GetMethodsOfTypeWithAttribute(typeof(NotAttributedMock), typeof(TestSetUpAttribute));
            AssertEx.That(testSetUps.Count, Is.EqualTo(0), "Registered a test set up method in NotAttributedMock.");
        }

        [TestMethod]
        public void TestTearDown() {
            List<MethodInfo> testTearDowns = GetMethodsOfTypeWithAttribute(typeof(AttributedMock), typeof(TestTearDownAttribute));
            AssertEx.That(testTearDowns.Count, Is.Not(0), "Registered no test tear down method.");
            AssertEx.That(testTearDowns.Count, Is.EqualTo(1), "Registered more than one test tear down method.");
            AssertEx.That(testTearDowns[0], Is.EqualTo(typeof(AttributedMock).GetMethod("MockTestTearDown")));

            JUUTAttribute testTearDown = testTearDowns[0].GetCustomAttribute<TestTearDownAttribute>();
            AssertEx.That(testTearDown.GetName(), Is.EqualTo("TestTearDown"));
            AssertEx.That(testTearDown.IsSetUpOrTearDown(), Is.True());

            testTearDowns = GetMethodsOfTypeWithAttribute(typeof(NotAttributedMock), typeof(TestTearDownAttribute));
            AssertEx.That(testTearDowns.Count, Is.EqualTo(0), "Registered a test tear down method in NotAttributedMock.");
        }

        [TestMethod]
        public void SimpleTestMethod() {
            List<MethodInfo> simpleTests = GetMethodsOfTypeWithAttribute(typeof (AttributedMock), typeof (SimpleTestMethodAttribute));
            AssertEx.That(simpleTests.Count, Is.Not(0), "No simple test methods found in " + typeof(AttributedMock).Name + ".");
            foreach (MethodInfo simpleTest in simpleTests) {
                AssertEx.That(simpleTest, Matches.AnyOf(Is.EqualTo(typeof(AttributedMock).GetMethod("FirstTestMethod")),
                                                        Is.EqualTo(typeof(AttributedMock).GetMethod("SecondTestMethod"))));
            }

            JUUTAttribute testTearDown = simpleTests[0].GetCustomAttribute<SimpleTestMethodAttribute>();
            AssertEx.That(testTearDown.GetName(), Is.EqualTo("SimpleTestMethod"));
            AssertEx.That(testTearDown.IsSetUpOrTearDown(), Is.False());

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
