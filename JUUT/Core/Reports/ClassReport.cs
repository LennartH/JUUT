namespace JUUT.Core.Reports {

    public interface ClassReport : Report {

        /// <summary>
        /// The number of all runned tests.
        /// </summary>
        int RunnedTests { get; }
        /// <summary>
        /// The number of tests, that failed. Can have status Failed or Error.
        /// </summary>
        int FailedTests { get; set; }
        /// <summary>
        /// The number of succeeded tests.
        /// </summary>
        int SucceededTests { get; set; }

        /// <summary>
        /// Adds the report to the class report.<para />
        /// The given report has to be a member of the class.<para />
        /// If a report for a method is added twice, the old one will be overwritten.
        /// </summary>
        void Add(MethodReport report);

    }

}