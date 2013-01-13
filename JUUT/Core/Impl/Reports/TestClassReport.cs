using System;
using System.Reflection;

using JUUT.Core.Impl.Attributes;

namespace JUUT.Core.Impl.Reports {

    public class TestClassReport : AbstractReport {

        public TestClassReport(MethodInfo runnedMethod, Exception raisedException)
            : base(runnedMethod, ReportRange.TestClass) {
            CheckThatTheArgumentsAreCorrect(raisedException);
            
            SetText(raisedException);
        }

        private void CheckThatTheArgumentsAreCorrect(Exception raisedException) {
            if (raisedException == null) {
                throw new ArgumentException("The exception of a TestClassReport can't be null.");
            }
            if (IsMethodNotSetUpAndNotTearDown()) {
                throw new ArgumentException("The method of a TestClassReport has to have the ClassSetUp-, TestSetUp-, TestTearDown- or ClassTearDown-Attribute.");
            }
        }

        private bool IsMethodNotSetUpAndNotTearDown() {
            return RunnedMethod.GetCustomAttribute(typeof(ClassSetUpAttribute)) == null &&
                   RunnedMethod.GetCustomAttribute(typeof(TestSetUpAttribute)) == null &&
                   RunnedMethod.GetCustomAttribute(typeof(TestTearDownAttribute)) == null &&
                   RunnedMethod.GetCustomAttribute(typeof(ClassTearDownAttribute)) == null;
        }

        private void SetText(Exception raisedException) {
            Text = "The " + GetAttributeName() + "-Method " + RunnedMethod.Name +
                   " of the test class " + TestClass.Name +
                   " raised an exception: " + raisedException.Message;
        }

        private string GetAttributeName() {
            if (RunnedMethod.GetCustomAttribute(typeof(ClassSetUpAttribute)) != null) {
                return "ClassSetUp";
            }
            if (RunnedMethod.GetCustomAttribute(typeof(TestSetUpAttribute)) != null) {
                return "TestSetUp";
            }
            if (RunnedMethod.GetCustomAttribute(typeof(TestTearDownAttribute)) != null) {
                return "TestTearDown";
            }
            if (RunnedMethod.GetCustomAttribute(typeof(ClassTearDownAttribute)) != null) {
                return "ClassTearDown";
            }

            throw new NotImplementedException("The attribute of the runned method isn't supported.");
        }

    }

}