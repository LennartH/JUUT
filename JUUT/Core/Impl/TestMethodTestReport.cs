using System;
using System.Reflection;

namespace JUUT.Core.Impl {

    /// <summary>
    /// Represents a report of a runned test.
    /// </summary>
    public class TestMethodTestReport : TestReport {

        public string Text { get; private set; }

        public Type TestClassType {
            get { return TestMethod.DeclaringType; }
        }

        /// <summary>
        /// The info of the runned test method.
        /// </summary>
        private MethodInfo TestMethod { get; set; }

        /// <summary>
        /// Creates a new report for the <code>testedMethod</code>, which passed successfully.
        /// </summary>
        /// <param name="testedMethod">The info of the runned test method.</param>
        public TestMethodTestReport(MethodInfo testedMethod) : this(testedMethod, null) { }

        /// <summary>
        /// Creates a new report for the <code>testedMethod</code> and it's raised exception or <code>null</code>, if the test passed successfully.
        /// </summary>
        /// <param name="testedMethod">The info of the runned test method.</param>
        /// <param name="raisedException">The raised exception or <code>null</code>, if the test passed successfully.</param>
        public TestMethodTestReport(MethodInfo testedMethod, Exception raisedException) {
            if (testedMethod == null) {
                throw new ArgumentException("The test method of a test report can't be null.");
            }

            TestMethod = testedMethod;
            SetText(raisedException);
        }

        /// <summary>
        /// Sets the report text depending on the given exception.<para />
        /// The <seealso cref="TestMethod"/>-Info has to be set before calling this.
        /// <param name="raisedException">The exception raised by the tested method. Can be null.</param>
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

        private bool Equals(TestMethodTestReport other) {
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
            return Equals((TestMethodTestReport) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (Text.GetHashCode() * 397) ^ TestMethod.GetHashCode();
            }
        }

    }

}