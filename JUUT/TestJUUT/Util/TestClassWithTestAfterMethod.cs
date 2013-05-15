using JUUT_Core.Attributes;
using JUUT_Core.Attributes.Methods;

namespace TestJUUT.Util {

    [JUUTTestClass]
    public class TestClassWithTestAfterMethod {

        [TestAfter]
        public void TestAfterMethod() { }

    }

}