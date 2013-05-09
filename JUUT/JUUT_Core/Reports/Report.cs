using System;

namespace JUUT_Core.Reports {

    /// <summary>
    /// Interface for reports, that are created during the run of tests.
    /// </summary>
    public interface Report {

        /// <summary>
        /// The summary text of the report.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// The type of the test class, which refers to the report.<para />
        /// Can be null, if the method has no declaring type.
        /// </summary>
        Type TestClass { get; }

        /// <summary>
        /// The status of the report, which represents the status of the runned test.<para />
        /// If there could be more than one status, than it's the worst.
        /// </summary>
        ReportStatus Status { get; }

    }

}