using System;
using System.Collections.Generic;
using System.Reflection;

using JUUT.Core.Reports;
using JUUT.Core.Scanners;

namespace JUUT.Core.Runners {

    public class SimpleTestRunner {// : TestRunner {

        //public Type ClassType { get; private set; }

        //public SimpleTestRunner(Type testClass) {
        //    ClassType = testClass;
        //}

        ///// <summary>
        ///// Runs all tests and returns the reports of the runned tests.
        ///// </summary>
        ///// <returns>The reports of the runned tests.</returns>
        //public List<Report> RunAll() {
        //    List<Report> resultReports = new List<Report>();
        //    Report report = RunClassSetUp();
        //    if (report != null) {
        //        resultReports.Add(report);
        //        return resultReports;
        //    }

        //    return resultReports;
        //}

        ///// <summary>TODO Was passiert wenns es keine Methode mit diesem namen gibt?
        ///// Runs the test with the given name and returns the reports of the run.
        ///// </summary>
        ///// <param name="testMethod"></param>
        ///// <returns>Report of the runned test.</returns>
        //public Report Run(MethodInfo testMethod) {
        //    Report report = RunClassSetUp();
        //    if (report != null) {
        //        return report;
        //    }

        //    report = RunTest(testMethod);
        //    if (report != null) {
        //        return report;
        //    }

        //    return report;
        //}

        ///// <summary>
        ///// TODO
        ///// </summary>
        ///// <param name="testMethod"></param>
        ///// <returns></returns>
        //private Report RunTest(MethodInfo testMethod) {
        //    throw new NotImplementedException();
        //}

        ///// <summary>
        ///// Runs the class set up method of the test class and returns a report, if an exception is thrown.
        ///// If no exception is thrown, null is returned.
        ///// </summary>
        ///// <returns>A report if an exception is thrown. Otherwhise null.</returns>
        //private TestClassReport RunClassSetUp() {
        //    try {
        //        TestClassScanner.GetClassSetUpOfTestClass(ClassType).Invoke(null, null);
        //    } catch (Exception raisedException) {
        //        if (raisedException is TargetInvocationException) {
        //            raisedException = raisedException.InnerException;
        //        }
        //        return new TestClassReport(TestClassScanner.GetClassSetUpOfTestClass(ClassType), raisedException);
        //    }

        //    return null;
        //}

    }

}
