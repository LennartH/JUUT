using System;

using JUUT.Core.Impl;

namespace JUUT.Core {

    /// <summary>
    /// Enum for the different ranges reports can be about.
    /// </summary>
    public enum ReportRange {

        /// <summary>
        /// This indicates, that the report is about a test method.
        /// </summary>
        TestMethod,

        /// <summary>
        /// This indicates, that the report is about a test class.
        /// </summary>
        TestClass

    }

    /// <summary>
    /// Interface for reports, that are created during the run of tests.
    /// </summary>
    public interface Report {

        /// <summary>
        /// The summary text of the report.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// The type of the test class, which contains the <seealso cref="TestMethod"/>.<para />
        /// Can be null, if the method has no declaring type.
        /// </summary>
        Type TestClassType { get; }

        /// <summary>
        /// The range the report is about. See <seealso cref="ReportRange"/> for more information.
        /// </summary>
        ReportRange Range { get; }

    }

}