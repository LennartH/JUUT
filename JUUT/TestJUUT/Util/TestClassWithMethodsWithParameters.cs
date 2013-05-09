using JUUT_Core.Attributes;
using JUUT_Core.Attributes.Methods;

namespace TestJUUT.Util {

    [JUUTTestClass]
    internal class TestClassWithMethodsWithParameters {

        [ClassSetUp]
        public static void ClassSetUp(object parameter) { }
        [TestSetUp]
        public static void SetUp(object parameter) { }

        [SimpleTestMethod]
        public static void TestMethod(object parameter) { }

        [TestTearDown]
        public static void TearDown(object parameter) { }
        [ClassTearDown]
        public static void ClassTearDown(object parameter) { }

    }

}