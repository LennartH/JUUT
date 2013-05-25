using System;
using System.Collections.Generic;
using System.Reflection;

using JUUT_Core.Attributes;
using JUUT_Core.Attributes.Methods;

namespace JUUT_Core.Scanners {

    /// <summary>
    /// Static class to scan JUUT-Tests for methods with the JUUT attributes.
    /// </summary>
    public static class TestClassScanner {

        /// <summary>
        /// Scans the given class for a ClassSetUp-Method (identified with the ClassSetUp-Attribute) and returns it or null, if there is none.<para />
        /// Throws an ArgumentNullException, if the given class is null and an ArgumentException if it doesn't have the JUUTTestClass-Attribute.<para />
        /// Throws an ArgumentException, if there are more than one ClassSetUp-Methods or if the method isn't static or if the method has parameters.
        /// </summary>
        /// <param name="testClass">The test class that is scanned for a ClassSetUp-Method. Can't be null.</param>
        /// <returns>Returns the ClassSetUp-Method or null, if there is none.</returns>
        public static MethodInfo GetClassSetUpOfTestClass(Type testClass) {
            return GetOrganizeMethodOfTestClass(typeof(ClassSetUpAttribute), testClass);
        }

        /// <summary>
        /// Scans the given class for a TestSetUp-Method (identified with the TestSetUp-Attribute) and returns it or null, if there is none.<para />
        /// Throws an ArgumentNullException, if the given class is null and an ArgumentException if it doesn't have the JUUTTestClass-Attribute.<para />
        /// Throws an ArgumentException, if there are more than one TestSetUp-Methods or if the method has parameters.
        /// </summary>
        /// <param name="testClass">The test class that is scanned for a TestSetUp-Method. Can't be null.</param>
        /// <returns>Returns the TestSetUp-Method or null, if there is none.</returns>
        public static MethodInfo GetTestSetUpOfTestClass(Type testClass) {
            return GetOrganizeMethodOfTestClass(typeof(TestSetUpAttribute), testClass);
        }

        /// <summary>
        /// Scans the given class for a TestTearDown-Method (identified with the TestTearDown-Attribute) and returns it or null, if there is none.<para />
        /// Throws an ArgumentNullException, if the given class is null and an ArgumentException if it doesn't have the JUUTTestClass-Attribute.<para />
        /// Throws an ArgumentException, if there are more than one TestTearDown-Methods or if the method has parameters.
        /// </summary>
        /// <param name="testClass">The test class that is scanned for a TestTearDown-Method. Can't be null.</param>
        /// <returns>Returns the TestTearDown-Method or null, if there is none.</returns>
        public static MethodInfo GetTestTearDownOfTestClass(Type testClass) {
            return GetOrganizeMethodOfTestClass(typeof(TestTearDownAttribute), testClass);
        }

        /// <summary>
        /// Scans the given class for a ClassTearDown-Method (identified with the ClassTearDown-Attribute) and returns it or null, if there is none.<para />
        /// Throws an ArgumentNullException, if the given class is null and an ArgumentException if it doesn't have the JUUTTestClass-Attribute.<para />
        /// Throws an ArgumentException, if there are more than one ClassTearDown-Methods or if the method has parameters.
        /// </summary>
        /// <param name="testClass">The test class that is scanned for a ClassTearDown-Method. Can't be null.</param>
        /// <returns>Returns the ClassTearDown-Method or null, if there is none.</returns>
        public static MethodInfo GetClassTearDownOfClass(Type testClass) {
            return GetOrganizeMethodOfTestClass(typeof(ClassTearDownAttribute), testClass);
        }

        /// <summary>
        /// Scans the given class for SimpleTest-Methods (identified with the SimpleTestMethod-Attribute) and returns them or an empty list, if there are none.<para />
        /// Throws an ArgumentNullException, if the given class is null and an ArgumentException if it doesn't have the JUUTTestClass-Attribute.<para />
        /// Throws an ArgumentException, if there are more than one ClassTearDown-Methods or if the method has parameters.
        /// </summary>
        /// <param name="testClass">The test class that is scanned for SimpleTest-Methods. Can't be null.</param>
        /// <returns>Returns the SimpleTest-Methods or null, if there are none.</returns>
        public static List<MethodInfo> GetSimpleTestMethodsOfClass(Type testClass) {
            return GetTestMethodsOfTestClass(typeof(SimpleTestMethodAttribute), testClass);
        }

        //TODO
        public static List<MethodInfo> GetAllTestMethodsOfClass(Type testClass) {
            return GetTestMethodsOfTestClass(typeof(TestAfterAttribute), testClass);
        }

        //TODO 
        public static bool ClassContainsOnlySimpleTests(Type testClass) {
            foreach (MethodInfo test in GetAllTestMethodsOfClass(testClass)) {
                if (test.GetCustomAttribute<JUUTTestMethodAttribute>() is TestAfterAttribute) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Scans the given class for a method with the given test method attribute.<para />
        /// Throws exceptions if the class or there methods aren't valid.
        /// </summary>
        /// <param name="testMethodAttribute"></param>
        /// <param name="testClass"></param>
        /// <returns></returns>
        private static List<MethodInfo> GetTestMethodsOfTestClass(Type testMethodAttribute, Type testClass) {
            JUUTAttribute.IsMemberValidFor(typeof(JUUTTestClassAttribute), testClass);

            List<MethodInfo> testMethods = new List<MethodInfo>();
            foreach (MethodInfo method in testClass.GetMethods()) {
                if (JUUTAttribute.IsMemberValidFor(testMethodAttribute, method)) {
                    testMethods.Add(method);
                }
            }
            return testMethods;
        }

        /// <summary>
        /// Scans the given class for a method with the given organize attribute.<para />
        /// Throws exceptions if the class or there methods aren't valid.
        /// </summary>
        /// <param name="organizeAttribute"></param>
        /// <param name="testClass"></param>
        /// <returns></returns>
        private static MethodInfo GetOrganizeMethodOfTestClass(Type organizeAttribute, Type testClass) {
            JUUTAttribute.IsMemberValidFor(typeof(JUUTTestClassAttribute), testClass);

            MethodInfo organizeMethod = null;
            foreach (MethodInfo method in testClass.GetMethods()) {
                if (JUUTAttribute.IsMemberValidFor(organizeAttribute, method)) {
                    if (organizeMethod != null) {
                        JUUTAttribute attribute = (JUUTAttribute) Activator.CreateInstance(organizeAttribute);
                        throw new ArgumentException("The class " + testClass.Name + " has more than one " + attribute.Name + "-Methods.");
                    }

                    organizeMethod = method;
                }
            }
            return organizeMethod;
        }

    }

}
