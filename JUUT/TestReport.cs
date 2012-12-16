using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JUUT {

    public enum TestStatus {

        Failed,
        Error,
        Passed

    }

    public class TestReport {

        public TestStatus TestStatus { get; private set; }
        public string Text { get; private set; }

        private readonly Exception RaisedException;
        private readonly MemberInfo TestMethod;

        public TestReport(MethodInfo testMethod, Exception raisedException) {
            if (testMethod == null) {
                throw new ArgumentException("The test method of a test report can't be null.");
            }

            this.RaisedException = raisedException;
            this.TestMethod = testMethod;

            SetStatus();
            SetText();
        }

        private void SetText() {
            if (RaisedException != null) {
                switch (TestStatus) {
                    case TestStatus.Failed:
                        Text = "The " + TestMethod.Name + "-Test failed: " + RaisedException.Message;
                        break;
                    case TestStatus.Error:
                        Text = "The " + TestMethod.Name + "-Test raised an unexpected exception: " + RaisedException.Message;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            } else {
                Text = "The " + TestMethod.Name + "-Test passed successfully.";
            }
        }

        private void SetStatus() {
            if (RaisedException == null) {
                TestStatus = TestStatus.Passed;
            } else if (RaisedException is AssertException) {
                TestStatus = TestStatus.Failed;
            } else {
                TestStatus = TestStatus.Error;
            }
        }

    }

}