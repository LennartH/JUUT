using System;

using JUUT.Core.Impl.Reports;

namespace JUUT.Core {

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
        /// The range the report is about. See <seealso cref="ReportRange"/> for more information.
        /// </summary>
        ReportRange Range { get; }

    }

}