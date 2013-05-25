using System;
using System.Collections.Generic;
using System.Reflection;

using JUUT_Core.Attributes;
using JUUT_Core.Attributes.Methods;
using JUUT_Core.Reports;
using JUUT_Core.Scanners;
using JUUT_Core.Sessions;

namespace JUUT_Core.Runners {

    public class SimpleTestRunner : TestRunner {

        /// <summary>
        /// Runs all tests and returns the reports of the runned tests.
        /// </summary>
        /// <param name="session"></param>
        public ClassReport Run(TestClassSession session) {
            Type testClass = session.TestClass;
            ClassReport report = new SimpleClassReport(testClass);

            MethodReport methodReport = RunClassSetUp(testClass);
            if (methodReport != null) {
                report.Add(methodReport);
                return report;
            }

            object testInstance = Activator.CreateInstance(testClass);
            foreach (MethodInfo test in session.TestsToRun) {
                methodReport = RunTestSetUp(testInstance, testClass);
                if (methodReport != null) {
                    report.Add(methodReport);
                    return report;
                }

                report.Add(RunTest(testInstance, test));

                methodReport = RunTestTearDown(testInstance, testClass);
                if (methodReport != null) {
                    report.Add(methodReport);
                    return report;
                }
            }

            methodReport = RunClassTearDown(testClass);
            if (methodReport != null) {
                report.Add(methodReport);
            }
            return report;
        }

        #region Helper Methods
        /// <summary>
        /// Runs the class set up method of the test class and adjusts the report, if an exception is thrown.
        /// </summary>
        /// <param name="testClass"></param>
        private static MethodReport RunClassSetUp(Type testClass) {
            MethodInfo classSetUp = TestClassScanner.GetClassSetUpOfTestClass(testClass);
            if (classSetUp == null) {
                return null;
            }

            return RunStaticOrganizeMethod(classSetUp);
        }

        /// <summary>
        /// Runs the test set up method for the given test class instance and adjusts the report, if an exception is thrown.
        /// </summary>
        private static MethodReport RunTestSetUp(object instance, Type testClass) {
            MethodInfo testSetUp = TestClassScanner.GetTestSetUpOfTestClass(testClass);
            if (testSetUp == null) {
                return null;
            }

            return RunInstanceMethod(instance, testSetUp, false);
        }

        /// <summary>
        /// Runs the test method for the given test class instance and adjusts the report.
        /// </summary>
        private static MethodReport RunTest(object instance, MethodInfo testMethod) {
            return RunInstanceMethod(instance, testMethod, true);
        }

        /// <summary>
        /// Runs the test tear down method for the given test class instance and adjusts the report, if an exception is thrown.
        /// </summary>
        private static MethodReport RunTestTearDown(object instance, Type testClass) {
            MethodInfo testTearDown = TestClassScanner.GetTestTearDownOfTestClass(testClass);
            if (testTearDown == null) {
                return null;
            }

            return RunInstanceMethod(instance, testTearDown, false);
        }

        /// <summary>
        /// Runs the class tear down method of the test class and adjusts the report, if an exception is thrown.
        /// </summary>
        /// <param name="testClass"></param>
        private static MethodReport RunClassTearDown(Type testClass) {
            MethodInfo classTearDown = TestClassScanner.GetClassTearDownOfClass(testClass);
            if (classTearDown == null) {
                return null;
            }

            return RunStaticOrganizeMethod(classTearDown);
        }

        /// <summary>
        /// Runs the given static organize method and adjusts the report, if an exception is thrown.
        /// </summary>
        /// <param name="staticOrganizeMethod"></param>
        private static MethodReport RunStaticOrganizeMethod(MethodInfo staticOrganizeMethod) {
            try {
                staticOrganizeMethod.Invoke(null, null);
                return null;
            } catch (Exception raisedException) {
                if (raisedException is TargetInvocationException) {
                    raisedException = raisedException.InnerException;
                }
                return new MethodReport(staticOrganizeMethod, raisedException);
            }
        }

        /// <summary>
        /// Runs the method (test, test set up or test tear down) for the given test class instance and
        /// adjusts the report, if an exception is thrown or allwaysReport is true.
        /// </summary>
        private static MethodReport RunInstanceMethod(object testInstance, MethodInfo method, bool allwaysReport) {
            try {
                method.Invoke(testInstance, null);
                if (allwaysReport) {
                    return new MethodReport(method);
                }
                return null;
            } catch (Exception raisedException) {
                if (raisedException is TargetInvocationException) {
                    raisedException = raisedException.InnerException;
                }
                return new MethodReport(method, raisedException);
            }
        }
        #endregion

    }

}
