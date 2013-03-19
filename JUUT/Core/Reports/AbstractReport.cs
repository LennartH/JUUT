using System;
using System.Reflection;

namespace JUUT.Core.Reports {

    public abstract class AbstractReport : Report {

        public string Text { get; protected set; }

        public Type TestClass {
            get { return RunnedMethod.DeclaringType; }
        }

        public ReportRange Range { get; private set; }

        /// <summary>
        /// The info of the runned method.
        /// </summary>
        protected MethodInfo RunnedMethod { get; private set; }

        /// <summary>
        /// Creates a new report for the given <code>method</code>.<para />
        /// Throws an <code>ArgumentException</code>, when the method is null.
        /// </summary>
        /// <param name="runnedMethod">The runned method this report is about. Can't be null.</param>
        /// <param name="range">The range this report is about.</param>
        protected AbstractReport(MethodInfo runnedMethod, ReportRange range) {
            if (runnedMethod == null) {
                throw new ArgumentException("The test method of a test report can't be null.");
            }

            Range = range;
            RunnedMethod = runnedMethod;
        }

        public override string ToString() {
            return Range.Name + " wide report of " + TestClass.Name + ": " + Text;
        }

        private bool Equals(AbstractReport other) {
            return string.Equals(Text, other.Text) && RunnedMethod.Equals(other.RunnedMethod);
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
            return Equals((AbstractReport) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (Text.GetHashCode() * 397) ^ RunnedMethod.GetHashCode();
            }
        }

    }

}