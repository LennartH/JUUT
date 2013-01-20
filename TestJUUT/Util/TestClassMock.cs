using JUUT.Core.Attributes;

namespace TestJUUT.Util {

    [JUUTTestClass]
    internal class TestClassMock {

        [ClassSetUp]
        public void MockSetUp() { }
        [TestSetUp]
        public void MockTestSetUp() { }

        [SimpleTestMethod]
        public void FirstTestMethod() { }
        [SimpleTestMethod]
        public void SecondTestMethod() { }

        [TestTearDown]
        public void MockTestTearDown() { }
        [ClassTearDown]
        public void MockTearDown() { }
            
    }

}