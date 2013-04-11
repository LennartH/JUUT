using System;

namespace JUUT.Core.Reports {

    public abstract class ReportStatus {

        /// <summary>
        /// Creates the correct status for the given exception.
        /// </summary>
        /// <param name="raisedException">The raised exception. Can be null.</param>
        /// <returns>Returns Success, Failed or Error, depending on the exception.</returns>
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

        public string DescribingText { get; private set; }

        private ReportStatus(string name, string describingText) {
            Name = name;
            DescribingText = describingText;
        }

        public abstract bool IsWorseThan(ReportStatus status);

        public abstract void IncrementCounterFor(ClassReport classReport);
        public abstract void DecrementCounterFor(ClassReport classReport);

        public override string ToString() {
            return Name;
        }

        #region Not Runned
        /// <summary>
        /// The status for a method or class, which wasn't run.
        /// </summary>
        public class NotRunned : ReportStatus {
            public NotRunned() : base("Not Runned", "wasn't run") { }

            public override bool IsWorseThan(ReportStatus status) {
                return false;
            }

            public override void IncrementCounterFor(ClassReport classReport) { }
            public override void DecrementCounterFor(ClassReport classReport) { }

        }
        #endregion

        #region Success
        /// <summary>
        /// The test was successful.
        /// </summary>
        public class Success : ReportStatus {
            public Success() : base("Success", "passed successfully") { }

            public override bool IsWorseThan(ReportStatus status) {
                return status is NotRunned;
            }

            public override void IncrementCounterFor(ClassReport classReport) {
                classReport.SucceededTests++;
            }

            public override void DecrementCounterFor(ClassReport classReport) {
                classReport.SucceededTests--;
            }

        }
        #endregion

        #region Failed
        /// <summary>
        /// The test raised an AssertException.
        /// </summary>
        public class Failed : ReportStatus {
            public Failed() : base("Failed", "failed") { }

            public override bool IsWorseThan(ReportStatus status) {
                return !(status is Failed) && !(status is Error);
            }

            public override void IncrementCounterFor(ClassReport classReport) {
                classReport.FailedTests++;
            }

            public override void DecrementCounterFor(ClassReport classReport) {
                classReport.FailedTests--;
            }

        }
        #endregion

        #region Error
        /// <summary>
        /// The test raised an unexpected exception.
        /// </summary>
        public class Error : ReportStatus {
            public Error() : base("Error", "threw an unexcepcted exception") { }

            public override bool IsWorseThan(ReportStatus status) {
                return !(status is Error);
            }

            public override void IncrementCounterFor(ClassReport classReport) {
                classReport.FailedTests++;
            }

            public override void DecrementCounterFor(ClassReport classReport) {
                classReport.FailedTests--;
            }

        }
        #endregion

    }

}
