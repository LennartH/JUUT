using System;

namespace JUUT.Core.Reports {

    public abstract class ReportStatus {

        /// <summary>
        /// Creates the correct status for the given exception.
        /// </summary>
        /// <param name="raisedException">The raised exception. Can be null.</param>
        public static ReportStatus Create(Exception raisedException) {
            if (raisedException == null) {
                return new Success();
            }
            if (raisedException is AssertException) {
                return new Failed();
            }
            return new Error();
        }

        public string Name { get; private set; }

        private ReportStatus(string name) {
            Name = name;
        }

        public override string ToString() {
            return Name;
        }

        #region Error
        /// <summary>
        /// The test raised an unexpected exception.
        /// </summary>
        public class Error : ReportStatus {
            internal Error() : base("Error") { }
        }
        #endregion

        #region Failed
        /// <summary>
        /// The test raised an AssertException.
        /// </summary>
        public class Failed : ReportStatus {
            internal Failed() : base("Failed") { }
        }
        #endregion

        #region Success
        /// <summary>
        /// The test was successful.
        /// </summary>
        public class Success : ReportStatus {
            internal Success() : base("Success") { }
        }
        #endregion

    }

}
