﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JUUT.Core {
    
    /// <summary>
    /// Interface which defines the methods of a test reporter.<para />
    /// A test reporter should take all reports of a test run and be able to present them.
    /// </summary>
    public interface TestReporter {

        /// <summary>
        /// Adds a new report to the reporter.
        /// </summary>
        /// <param name="report">The new report.</param>
        void AddReport(TestReport report);

        /// <summary>
        /// The reports contained by the reporter.<para />
        /// The reports are structered by the type of the test class, which owns the test method.
        /// </summary>
        /// <returns>The reports contained by the reporter.</returns>
        Dictionary<Type, IList<TestReport>> Reports { get; }

        /// <summary>
        /// Presents the reports contained by the reporter.
        /// </summary>
        void PresentReports();

    }
}