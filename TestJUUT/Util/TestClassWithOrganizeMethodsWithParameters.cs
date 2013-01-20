using JUUT.Core.Attributes;

namespace TestJUUT.Util {

    [JUUTTestClass]
    internal class TestClassWithOrganizeMethodsWithParameters {

        [ClassSetUp]
        public static void ClassSetUp(object parameter) { }
        [TestSetUp]
        public static void SetUp(object parameter) { }

        [TestTearDown]
        public static void TearDown(object parameter) { }
        [ClassTearDown]
        public static void ClassTearDown(object parameter) { }

    }

}