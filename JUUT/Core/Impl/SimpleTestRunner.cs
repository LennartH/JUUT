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

        public List<Report> RunAll() {
            List<Report> resultReports = new List<Report>();

            //return resultReports;
            throw new NotImplementedException();
        }

        public Report Run(string testMethodName) {
            throw new NotImplementedException();
        }

    }

}
