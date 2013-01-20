using System;
using System.Linq;
using System.Reflection;

using JUUT.Core.Attributes;

namespace JUUT.Core.Impl {

    /// <summary>
    /// Static class to scan JUUT-Tests for methods with the JUUT attributes.
    /// </summary>
    public static class TestClassScanner {

        //TODO Extract method which scans a class dynamic for a given organize method

        /// <summary>
        /// Scans the given class for a ClassSetUp-Method (identified with the ClassSetUp-Attribute) and returns it or null, if there is none.<para />
        /// Throws an ArgumentNullException, if the given class is null and an ArgumentException if it doesn't have the JUUTTestClass-Attribute.<para />
        /// Throws an ArgumentException, if there are more than one ClassSetUp-Methods or if the method isn't static or if the method has parameters.
        /// </summary>
        /// <param name="testClass">The test class that is scanned for a ClassSetUp-Method. Can't be null.</param>
        /// <returns>Returns the ClassSetUp-Method or null, if there is none.</returns>
        public static MethodInfo GetClassSetUpOfTest(Type testClass) {
            EnsureThatTheClassIsValid(testClass);

            MethodInfo classSetUp = null;
            foreach (MethodInfo method in testClass.GetMethods()) {
                if (IsMethodAValidClassSetUp(method)) {
                    if (classSetUp != null) {
                        throw new ArgumentException("The class " + testClass.Name + " has more than one ClassSetUp-Methods.");
                    }
                    
                    classSetUp = method;
                }
            }
            return classSetUp;
        }

        /// <summary>
        /// Scans the given class for a TestSetUp-Method (identified with the TestSetUp-Attribute) and returns it or null, if there is none.<para />
        /// Throws an ArgumentNullException, if the given class is null and an ArgumentException if it doesn't have the JUUTTestClass-Attribute.<para />
        /// Throws an ArgumentException, if there are more than one TestSetUp-Methods or if the method has parameters.
        /// </summary>
        /// <param name="testClass">The test class that is scanned for a TestSetUp-Method. Can't be null.</param>
        /// <returns>Returns the TestSetUp-Method or null, if there is none.</returns>
        public static MethodInfo GetTestSetUpOfTest(Type testClass) {
            EnsureThatTheClassIsValid(testClass);

            MethodInfo testSetUp = null;
            foreach (MethodInfo method in testClass.GetMethods()) {
                if (IsMethodAValidTestSetUp(method)) {
                    if (testSetUp != null) {
                        throw new ArgumentException("The class " + testClass.Name + " has more than one TestSetUp-Methods.");
                    }

                    testSetUp = method;
                }
            }
            return testSetUp;
        }

        /// <summary>
        /// Checks that the given class isn't null and has the JUUTTestClass-Attribute.<para />
        /// Throws an ArgumentNullException, if the given class is null and an ArgumentException if it doesn't have the JUUTTestClass-Attribute.
        /// </summary>
        /// <param name="testClass"></param>
        private static void EnsureThatTheClassIsValid(Type testClass) {
            if (testClass == null) {
                throw new ArgumentNullException();
            }
            if (testClass.GetCustomAttribute(typeof(JUUTTestClassAttribute)) == null) {
                throw new ArgumentException("The class " + testClass.Name + " doesn't have the JUUTTestClass-Attribute.");
            }
        }

        /// <summary>
        /// Checks that the given method has the ClassSetUp-Attribute, that it is static and it has no parameters.<para />
        /// Throws an ArgumentException, if the given method isn't static or has parameters.
        /// </summary>
        /// <param name="method">The method to check, that it is a ClassSetUp-Method.</param>
        /// <returns>True, if the method is a valid ClassSetUp-Method.</returns>
        private static bool IsMethodAValidClassSetUp(MethodInfo method) {
            return EnsureThat(method, hasThe: typeof(ClassSetUpAttribute), andIsStatic: true, andHasNoParameters: true);
        }

        /// <summary>
        /// Checks that the given method has the TestSetUp-Attribute and that it has no parameters.<para />
        /// Throws an ArgumentException, if the given method has parameters.
        /// </summary>
        /// <param name="method">The method to check, that it is a TestSetUp-Method.</param>
        /// <returns>True, if the method is a valid TestSetUp-Method.</returns>
        private static bool IsMethodAValidTestSetUp(MethodInfo method) {
            return EnsureThat(method, hasThe: typeof(TestSetUpAttribute), andIsStatic: false, andHasNoParameters: true);
        }

        /// <summary>
        /// Ensures that the given method has an attribute of the type hasThe, that it is static 
        /// (if andIsStatic is true) and that it has no parameters (if andHasNoParameters is true).<para />
        /// Throws ArgumentExceptions, if one of the constraints is false.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="hasThe"></param>
        /// <param name="andIsStatic"></param>
        /// <param name="andHasNoParameters"></param>
        /// <returns>True, if the method has an attribute of the given type. False otherwhise.</returns>
        private static bool EnsureThat(MethodInfo method, Type hasThe, bool andIsStatic, bool andHasNoParameters) {
            if (method.GetCustomAttribute(hasThe) != null) {
                if (andIsStatic && !method.IsStatic) {
                    throw new ArgumentException("The method " + method.Name + " isn't static.");
                }
                if (andHasNoParameters && method.GetParameters().Count() != 0) {
                    throw new ArgumentException("The method " + method.Name + " has parameters.");
                }

                return true;
            }
            return false;
        }

    }

}
