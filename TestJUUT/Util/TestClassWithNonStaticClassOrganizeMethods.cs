using JUUT.Core.Attributes;
using JUUT.Core.Attributes.Methods;

namespace TestJUUT.Util {

    [JUUTTestClass]
    internal class TestClassWithNonStaticClassOrganizeMethods {

        [ClassSetUp]
        public void ClassSetUp() { }
        [ClassTearDown]
        public void ClassTearDown() { }

    }

}