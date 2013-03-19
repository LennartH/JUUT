using System;
using System.Collections.Generic;
using System.Reflection;

using JUUT.Core.Reports;

namespace JUUT.Core.Runners {
    public interface TestRunner {

        Type TestClass { get; }

        List<Report> RunAll();

        Report Run(MethodInfo testMethod);

    }
}
