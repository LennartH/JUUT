using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JUUT.Core.Impl {

    public class SimpleTestRunner : TestRunner {

        public Type TestClassInfo { get; private set; }

        public SimpleTestRunner(Type testClass) {
            TestClassInfo = testClass;
        }

        public List<TestReport> RunAll() {
            List<TestReport> resultReports = new List<TestReport>();

            //return resultReports;
            throw new NotImplementedException();
        }

        public TestReport Run(string testMethodName) {
            throw new NotImplementedException();
        }

    }

}
