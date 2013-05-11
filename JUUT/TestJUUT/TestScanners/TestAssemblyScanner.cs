using System;
using System.Collections.Generic;
using System.Reflection;

using AssemblyScannerTestProject;

using JUUT_Core.Scanners;
using JUUT_Core.Sessions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT.TestScanners {

    [TestClass]
    public class TestAssemblyScanner {

        [TestMethod]
        public void ScanForTestClasses() {
            Assembly assembly = Assembly.GetAssembly(typeof(AssemblyTestMock1));

            HashSet<Type> expectedTestClasses = new HashSet<Type> {typeof(AssemblyTestMock1), typeof(AssemblyTestMock2)};
            AssertEx.That(expectedTestClasses.SetEquals(AssemblyScanner.GetTestClassesIn(assembly)), Is.True());
        }

        [TestMethod]
        public void CreateSessionForAssembly() {
            Assembly assembly = Assembly.GetAssembly(typeof(AssemblyTestMock1));

            HashSet<TestClassSession> expectedClassSessions = new HashSet<TestClassSession>();
            TestClassSession classSession = new TestClassSession(typeof(AssemblyTestMock1));
            classSession.AddAll();
            expectedClassSessions.Add(classSession);
            classSession = new TestClassSession(typeof(AssemblyTestMock2));
            classSession.AddAll();
            expectedClassSessions.Add(classSession);

            AssertEx.That(expectedClassSessions.SetEquals(AssemblyScanner.CreateSessionFor(assembly).TestClassSessions), Is.True());
        }

    }

}
