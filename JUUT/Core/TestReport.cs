using System;

using JUUT.Core.Impl;

namespace JUUT.Core {

    public interface TestReport {

        /// <summary>
        /// The summary text of the report.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// The type of the test class, which contains the <seealso cref="TestMethod"/>.<para />
        /// Can be null, if the method has no declaring type.
        /// </summary>
        Type TestClassType { get; }

    }

}