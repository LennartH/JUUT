using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JUUT.Core.Suites {

    public interface TestSuite {

        /// <summary>
        /// Adds all tests of the given test class to be runned by the suite.
        /// </summary>
        void AddAll(Type testClass);

        /// <summary>
        /// Adds the given test method to be runned by the suite.
        /// </summary>
        void Add(MethodInfo test);

        /// <summary>
        /// Runs all contained tests and reports the results with it's reporter.
        /// </summary>
        void Run();

    }

}
