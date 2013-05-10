using System;
using System.Collections.Generic;
using System.Reflection;

using JUUT_Core.Attributes;
using JUUT_Core.Attributes.Methods;
using JUUT_Core.Scanners;

namespace JUUT_Core.Sessions {

    public class TestClassSession {

        public HashSet<MethodInfo> TestsToRun { get; private set; }
        public Type TestClass { get; private set; }

        public TestClassSession(Type testClass) {
            if (testClass == null) {
                throw new ArgumentException("The test class of a TestRunner mustn't be null.");
            }
            if (testClass.GetCustomAttribute<JUUTTestClassAttribute>() == null) {
                throw new ArgumentException("The test class of a TestRunner needs the JUUTTestClass-Attribute.");
            }

            TestClass = testClass;
            TestsToRun = new HashSet<MethodInfo>();
        }

        /// <summary>
        /// Adds all tests of the test class to the session.
        /// </summary>
        public void AddAll() {
            foreach (MethodInfo test in TestClassScanner.GetSimpleTestMethodsOfClass(TestClass)) {
                TestsToRun.Add(test);
            }
        }

        /// <summary>
        /// Adds the given test to the session. This test has to be a member of the test class and needs a TestMethod Attribute.
        /// </summary>
        public void Add(MethodInfo test) {
            if (test.GetCustomAttribute<SimpleTestMethodAttribute>() == null) {
                throw new ArgumentException("Tests to be added to a TestRunner needs a TestMethod-Attribute.");
            }
            if (test.DeclaringType != TestClass) {
                throw new ArgumentException("The given method isn't a member of the test class.");
            }
            TestsToRun.Add(test);
        }

    }

}