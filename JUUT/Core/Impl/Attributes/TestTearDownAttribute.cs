using System;

namespace JUUT.Core.Impl.Attributes {
    /// <summary>
    /// Attribute to identify the test cleaner of a test class. Is runned every time after a test method is runned.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TestTearDownAttribute : Attribute {
    }
}
