using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using JUUT.Core.Impl.Attributes;

namespace JUUT.Core {

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
        public static MethodInfo GetClassSetUpOfTest(Type testClass) {
            EnsureThatTheClassIsValid(testClass);

            MethodInfo classSetUp = null;
            foreach (MethodInfo method in testClass.GetMethods()) {
                if (EnsureThatMethodIsValidClassSetUp(method)) {
                    if (classSetUp != null) {
                        throw new ArgumentException("The class " + testClass.Name + " has more than one ClassSetUp-Methods.");
                    }
                    
                    classSetUp = method;
                }
            }
            return classSetUp;
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
        private static bool EnsureThatMethodIsValidClassSetUp(MethodInfo method) {
            if (method.GetCustomAttribute(typeof(ClassSetUpAttribute)) != null) {
                if (!method.IsStatic) {
                    throw new ArgumentException("The method " + method.Name + " isn't static. ClassSetUp-Methods has to be static.");
                }
                if (method.GetParameters().Count() != 0) {
                    throw new ArgumentException("The method " + method.Name + " has parameters. ClassSetUp-Methods can't have parameters.");
                }

                return true;
            }
            return false;
        }

    }

}
