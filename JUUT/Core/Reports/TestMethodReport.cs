using System;
using System.Reflection;

using JUUT.Core.Attributes;
using JUUT.Core.Attributes.Methods;

namespace JUUT.Core.Reports {

    /// <summary>
    /// Represents a report of a runned test.
    /// </summary>
    public class TestMethodReport : Report {

        public string Text { get; private set; }

        public Type TestClass {
            get { return TestMethod.DeclaringType; }
        }

        public ReportStatus Status { get; private set; }

        /// <summary>
        /// The info of the runned method.
        /// </summary>
        private MethodInfo TestMethod { get; set; }

        /// <summary>
        /// Creates a new report for the <code>testMethod</code>, which passed successfully.
        /// </summary>
        /// <param name="testMethod">The info of the runned test method.</param>
        public TestMethodReport(MethodInfo testMethod) : this(testMethod, null) { }

        /// <summary>
        /// Creates a new report for the <code>testMethod</code> and it's raised exception or <code>null</code>, if the test passed successfully.
        /// </summary>
        /// <param name="testMethod">The info of the runned test method.</param>
        /// <param name="raisedException">The raised exception or <code>null</code>, if the test passed successfully.</param>
        public TestMethodReport(MethodInfo testMethod, Exception raisedException) {
            if (testMethod == null) {
                throw new ArgumentException("The test method of a test report can't be null.");
            }

            if (testMethod.GetCustomAttribute(typeof(SimpleTestMethodAttribute)) == null) {
                throw new ArgumentException("Methods of a TestMethodReport have to have the SimpleTestMethod-Attribute.");
            }

            TestMethod = testMethod;
            Status = ReportStatus.Create(raisedException);
            Text = GenerateText(TestMethod, raisedException);
        }

        /// <summary>
        /// Generates the text for the given information.
        /// <param name="raisedException">The exception raised by the tested method. Can be null.</param>
        /// </summary>
        private string GenerateText(MethodInfo testMethod, Exception raisedException) {
            if (raisedException == null) {
                return "The " + testMethod.Name + "-Test passed successfully.";
            }
            if (raisedException is AssertException) {
                return "The " + testMethod.Name + "-Test failed: " + raisedException.Message;
            }

            return "The " + testMethod.Name + "-Test raised an unexpected exception: " + raisedException.Message;
        }

        public override string ToString() {
            return Text;
        }

        ////////////////////////////////////////////////////////
        // Generated GetHashCode and Equals                   //
        ////////////////////////////////////////////////////////

        private bool Equals(TestMethodReport other) {
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
            return Equals((TestMethodReport) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (Text.GetHashCode() * 397) ^ TestMethod.GetHashCode();
            }
        }

    }

}