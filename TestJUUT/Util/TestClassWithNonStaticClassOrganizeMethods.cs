using JUUT.Core.Attributes;

namespace TestJUUT.Util {

    [JUUTTestClass]
    internal class TestClassWithNonStaticClassOrganizeMethods {

        [ClassSetUp]
        public void ClassSetUp() { }
        [ClassTearDown]
        public void ClassTearDown() { }

    }

}