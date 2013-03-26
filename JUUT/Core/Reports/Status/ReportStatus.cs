using System;

namespace JUUT.Core.Reports.Status {

    public abstract class ReportStatus {

        public static ReportStatus Create(Exception raisedException) {
            if (raisedException == null) {
                return new ReportStatusSuccess();
            }
            if (raisedException is AssertException) {
                return new ReportStatusFailed();
            }
            return new ReportStatusError();
        }

        public string Name { get; private set; }

        protected ReportStatus(string name) {
            Name = name;
        }

        public override string ToString() {
            return Name;
        }

    }

}
