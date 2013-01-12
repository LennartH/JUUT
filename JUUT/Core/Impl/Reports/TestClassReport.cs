using System;
using System.Reflection;

using JUUT.Core.Impl.Attributes;

namespace JUUT.Core.Impl.Reports {

    public class TestClassReport : AbstractReport {

        public TestClassReport(MethodInfo runnedMethod, Exception raisedException)
            : base(runnedMethod, ReportRange.TestClass) {
            CheckThatTheArgumentsAreCorrect(runnedMethod, raisedException);
        }

        private void CheckThatTheArgumentsAreCorrect(MethodInfo runnedMethod, Exception raisedException) {
            if (raisedException == null) {
                throw new ArgumentException("The exception of a TestClassReport can't be null.");
            }
            if (IsMethodNotSetUpAndNotTearDown(runnedMethod)) {
                throw new ArgumentException("The method of a TestClassReport has to have the ClassSetUp-, TestSetUp-, TestTearDown- or ClassTearDown-Attribute.");
            }
        }

        private static bool IsMethodNotSetUpAndNotTearDown(MethodInfo runnedMethod) {
            return runnedMethod.GetCustomAttribute(typeof(ClassSetUpAttribute)) == null &&
                   runnedMethod.GetCustomAttribute(typeof(TestSetUpAttribute)) == null &&
                   runnedMethod.GetCustomAttribute(typeof(TestTearDownAttribute)) == null &&
                   runnedMethod.GetCustomAttribute(typeof(ClassTearDownAttribute)) == null;
        }

    }

}