using System;

namespace JUUT.Core.Attributes {
    /// <summary>
    /// Attribute to identify the test initializer of a test class. Is runned every time before a test method is runned.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TestSetUpAttribute : JUUTAttribute {

        public override string GetName() {
            return "TestSetUp";
        }

        public override bool IsSetUpOrTearDown() {
            return true;
        }

    }
}
