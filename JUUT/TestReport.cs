using System.Reflection;

namespace JUUT {

    public interface TestReport {

        /// <summary>
        /// The <seealso cref="TestStatus"/> of the runned test.
        /// </summary>
        TestStatus TestStatus { get; }

        /// <summary>
        /// The summary text of the report.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// The info of the runned test method.
        /// </summary>
        MethodInfo TestMethod { get; }

    }

}