using JUUT.Core.Attributes;
using JUUT.Core.Attributes.Methods;

namespace TestJUUT.Util {

    [JUUTTestClass]
    internal class TestClassMock {

        [ClassSetUp]
        public static void MockSetUp() { }
        [TestSetUp]
        public void MockTestSetUp() { }

        [SimpleTestMethod]
        public void FirstTestMethod() { }
        [SimpleTestMethod]
        public void SecondTestMethod() { }

        [TestTearDown]
        public void MockTestTearDown() { }
        [ClassTearDown]
        public static void MockTearDown() { }
            
    }

}