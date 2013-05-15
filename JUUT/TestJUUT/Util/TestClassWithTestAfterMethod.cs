using JUUT_Core.Attributes;
using JUUT_Core.Attributes.Methods;

namespace TestJUUT.Util {

    /// <summary>
    /// Test class mock with an test, that is runned after TargetMethod of TestClassTarget has been runned.
    /// </summary>
    [JUUTTestClass]
    public class TestClassWithTestAfterMethod {

        [TestAfter(typeof(TestClassTarget), "TargetMethod")]
        public void TestAfterMethod() { }

    }

}