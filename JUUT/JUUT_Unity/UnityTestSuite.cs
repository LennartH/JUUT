using JUUT_Core.Suites;

namespace JUUT_Unity {

    public class UnityTestSuite : AbstractTestSuite {

        public UnityTestSuite() : base(new UnityTestReporter()) { }

    }

}
