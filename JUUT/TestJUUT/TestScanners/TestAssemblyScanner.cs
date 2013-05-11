using System;
using System.Collections.Generic;
using System.Reflection;

using AssemblyScannerTestProject;

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

    }

}
