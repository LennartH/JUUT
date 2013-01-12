using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JUUT.Core {
    public interface TestRunner {

        Type TestClassInfo { get; }

        List<Report> RunAll();

        Report Run(string testMethodName);

    }
}
