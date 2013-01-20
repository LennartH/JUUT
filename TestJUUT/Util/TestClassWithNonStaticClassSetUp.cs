using JUUT.Core.Attributes;

namespace TestJUUT.Util {

    [JUUTTestClass]
    internal class TestClassWithNonStaticClassSetUp {

        [ClassSetUp]
        public void ClassSetUp() { }

    }

}