using System;
using System.Linq;
using System.Reflection;

using JUUT.Core.Attributes;

namespace JUUT.Core.Impl {

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
            return GetOrganizeMethodOfTest(typeof(ClassSetUpAttribute), testClass);
        }

        /// <summary>
        /// Scans the given class for a TestSetUp-Method (identified with the TestSetUp-Attribute) and returns it or null, if there is none.<para />
        /// Throws an ArgumentNullException, if the given class is null and an ArgumentException if it doesn't have the JUUTTestClass-Attribute.<para />
        /// Throws an ArgumentException, if there are more than one TestSetUp-Methods or if the method has parameters.
        /// </summary>
        /// <param name="testClass">The test class that is scanned for a TestSetUp-Method. Can't be null.</param>
        /// <returns>Returns the TestSetUp-Method or null, if there is none.</returns>
        public static MethodInfo GetTestSetUpOfTest(Type testClass) {
            return GetOrganizeMethodOfTest(typeof(TestSetUpAttribute), testClass);
        }

        /// <summary>
        /// Scans the given class for a method with the given organize attribute.<para />
        /// Throws exceptions if the class isn't valid 
        /// </summary>
        /// <param name="organizeAttribute"></param>
        /// <param name="testClass"></param>
        /// <returns></returns>
        private static MethodInfo GetOrganizeMethodOfTest(Type organizeAttribute, Type testClass) {
            JUUTAttribute.IsMemberValidFor(typeof(JUUTTestClassAttribute), testClass);

            MethodInfo classSetUp = null;
            foreach (MethodInfo method in testClass.GetMethods()) {
                if (JUUTAttribute.IsMemberValidFor(organizeAttribute, method)) {
                    if (classSetUp != null) {
                        JUUTAttribute attribute = (JUUTAttribute) method.GetCustomAttribute(organizeAttribute);
                        throw new ArgumentException("The class " + testClass.Name + " has more than one " + attribute.Name + "-Methods.");
                    }

                    classSetUp = method;
                }
            }
            return classSetUp;
        }

    }

}
