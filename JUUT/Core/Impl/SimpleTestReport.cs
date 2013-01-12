using System;
using System.Reflection;

namespace JUUT.Core.Impl {

    /// <summary>
    /// Represents a report of a runned test.
    /// </summary>
    public class SimpleTestReport : TestReport {

        public string Text { get; private set; }

        public Type TestClassType {
            get { return TestMethod.DeclaringType; }
        }

        /// <summary>
        /// The info of the runned test method.
        /// </summary>
        private MethodInfo TestMethod { get; set; }

        /// <summary>
        /// Creates a new report for <code>testMethod</code> and it's raised exception or <code>null</code>, if the test passed successfully.
        /// </summary>
        /// <param name="testMethod">The info of the runned test method.</param>
        /// <param name="raisedException">The raised exception or <code>null</code>, if the test passed successfully.</param>
        public SimpleTestReport(MethodInfo testMethod, Exception raisedException) {
            if (testMethod == null) {
                throw new ArgumentException("The test method of a test report can't be null.");
            }

            TestMethod = testMethod;
            SetText(raisedException);
        }

        /// <summary>
        /// Sets the report text depending on the <seealso cref="TestStatus"/>.<para />
        /// The <seealso cref="TestStatus"/> (see <seealso cref="SetStatus()"/>), the <seealso cref="TestMethod"/>-Info
        /// and the <seealso cref="RaisedException"/> has to be set before calling this.
        /// </summary>
        private void SetText(Exception raisedException) {
            if (raisedException == null) {
                Text = "The " + TestMethod.Name + "-Test passed successfully.";
            } else if (raisedException is AssertException) {
                Text = "The " + TestMethod.Name + "-Test failed: " + raisedException.Message;
            } else {
                Text = "The " + TestMethod.Name + "-Test raised an unexpected exception: " + raisedException.Message;
            }
        }

        private bool Equals(SimpleTestReport other) {
            return string.Equals(Text, other.Text) && TestMethod.Equals(other.TestMethod);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj.GetType() != GetType()) {
                return false;
            }
            return Equals((SimpleTestReport) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (Text.GetHashCode() * 397) ^ TestMethod.GetHashCode();
            }
        }

    }

}