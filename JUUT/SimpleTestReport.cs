using System;
using System.Reflection;

namespace JUUT {

    /// <summary>
    /// Represents the status of a runned test.
    /// </summary>
    public enum TestStatus {

        /// <summary>
        /// The test failed because of a failed assertion.
        /// </summary>
        Failed,

        /// <summary>
        /// The test failed because an unexpected exception was thrown.
        /// </summary>
        Error,

        /// <summary>
        /// The test passed successfully.
        /// </summary>
        Passed

    }

    /// <summary>
    /// Represents a report of a runned test.
    /// </summary>
    public class SimpleTestReport : TestReport {

        /// <summary>
        /// The <seealso cref="TestStatus"/> of the runned test.
        /// </summary>
        public TestStatus TestStatus { get; private set; }

        /// <summary>
        /// The summary text of the report.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// The exception raised by the test. Can be <code>null</code> if the test passed successfully.
        /// </summary>
        private readonly Exception RaisedException;

        /// <summary>
        /// The info of the runned test method.
        /// </summary>
        private readonly MemberInfo TestMethod;

        /// <summary>
        /// Creates a new report for <code>testMethod</code> and it's raised exception or <code>null</code>, if the test passed successfully.
        /// </summary>
        /// <param name="testMethod">The info of the runned test method.</param>
        /// <param name="raisedException">The raised exception or <code>null</code>, if the test passed successfully.</param>
        public SimpleTestReport(MethodInfo testMethod, Exception raisedException) {
            if (testMethod == null) {
                throw new ArgumentException("The test method of a test report can't be null.");
            }

            RaisedException = raisedException;
            TestMethod = testMethod;

            SetStatus();
            SetText();
        }

        /// <summary>
        /// Sets the status depending on the raised exception.
        /// </summary>
        private void SetStatus() {
            if (RaisedException == null) {
                TestStatus = TestStatus.Passed;
            } else if (RaisedException is AssertException) {
                TestStatus = TestStatus.Failed;
            } else {
                TestStatus = TestStatus.Error;
            }
        }

        /// <summary>
        /// Sets the report text depending on the <seealso cref="TestStatus"/>.<para />
        /// The <seealso cref="TestStatus"/> (see <seealso cref="SetStatus()"/>), the <seealso cref="TestMethod"/>-Info
        /// and the <seealso cref="RaisedException"/> has to be set before calling this.
        /// </summary>
        private void SetText() {
            switch (TestStatus) {
                case TestStatus.Failed:
                    Text = "The " + TestMethod.Name + "-Test failed: " + RaisedException.Message;
                    break;
                case TestStatus.Error:
                    Text = "The " + TestMethod.Name + "-Test raised an unexpected exception: " + RaisedException.Message;
                    break;
                case TestStatus.Passed:
                    Text = "The " + TestMethod.Name + "-Test passed successfully.";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

    }

}