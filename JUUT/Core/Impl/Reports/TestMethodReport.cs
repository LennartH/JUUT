using JUUT.Core.Impl.Attributes;

using System;
using System.Reflection;

namespace JUUT.Core.Impl.Reports {

    /// <summary>
    /// Represents a report of a runned test.
    /// </summary>
    public class TestMethodReport : AbstractReport {

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
        public TestMethodReport(MethodInfo testMethod, Exception raisedException)
            : base(testMethod, ReportRange.TestMethod) {
            if (testMethod.GetCustomAttribute(typeof(SimpleTestMethodAttribute)) == null) {
                throw new ArgumentException("Methods of a TestMethodReport have to have the SimpleTestMethod-Attribute.");
            }

            SetText(raisedException);
        }

        /// <summary>
        /// Sets the report text depending on the given exception.<para />
        /// The <seealso cref="AbstractReport.RunnedMethod"/>-Info has to be set before calling this.
        /// <param name="raisedException">The exception raised by the tested method. Can be null.</param>
        /// </summary>
        private void SetText(Exception raisedException) {
            if (raisedException == null) {
                Text = "The " + RunnedMethod.Name + "-Test passed successfully.";
            } else if (raisedException is AssertException) {
                Text = "The " + RunnedMethod.Name + "-Test failed: " + raisedException.Message;
            } else {
                Text = "The " + RunnedMethod.Name + "-Test raised an unexpected exception: " + raisedException.Message;
            }
        }

    }

}