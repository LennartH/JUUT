using System;
using System.Reflection;

using JUUT.Core.Attributes;

namespace JUUT.Core.Reports {

    /// <summary>
    /// Represents a report of a runned test.
    /// </summary>
    public class MethodReport : Report {

        public string Text { get; private set; }
        /// <summary>
        /// Short information about the report status as text.
        /// </summary>
        public string ShortText { get; private set; }

        public Type TestClass {
            get { return Method.DeclaringType; }
        }

        public ReportStatus Status { get; private set; }

        /// <summary>
        /// The info of the runned method.
        /// </summary>
        internal MethodInfo Method { get; private set; }

        /// <summary>
        /// Creates a new report for the <code>method</code>, which passed successfully.
        /// </summary>
        /// <param name="method">The info of the runned test method.</param>
        public MethodReport(MethodInfo method) : this(method, null) { }

        /// <summary>
        /// Creates a new report for the <code>method</code> and it's raised exception or <code>null</code>, if the test passed successfully.
        /// </summary>
        /// <param name="method">The info of the runned test method.</param>
        /// <param name="raisedException">The raised exception or <code>null</code>, if the test passed successfully.</param>
        public MethodReport(MethodInfo method, Exception raisedException) {
            if (method == null) {
                throw new ArgumentException("The method of a report can't be null.");
            }

            if (method.GetCustomAttribute(typeof(JUUTMethodAttribute)) == null) {
                throw new ArgumentException("Methods of a MethodReport have to have an attribute of type JUUTMethodAttribute.");
            }

            Method = method;
            Status = ReportStatus.Create(raisedException);
            InitializeTexts(Method, Status, raisedException);
        }

        /// <summary>
        /// Initializes the values for Text and ShortText.
        /// </summary>
        private void InitializeTexts(MethodInfo method, ReportStatus status, Exception raisedException) {
            string commonText = "The " + method.Name + "-Method " + status.DescribingText;
            ShortText = commonText + ".";
            Text = commonText + ": " + raisedException.Message;
        }

        public override string ToString() {
            return Text;
        }

        ////////////////////////////////////////////////////////
        // Generated GetHashCode and Equals                   //
        ////////////////////////////////////////////////////////

        private bool Equals(MethodReport other) {
            return string.Equals(Text, other.Text) && Method.Equals(other.Method);
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
            return Equals((MethodReport) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (Text.GetHashCode() * 397) ^ Method.GetHashCode();
            }
        }

    }

}