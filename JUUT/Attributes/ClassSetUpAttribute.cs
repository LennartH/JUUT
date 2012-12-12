using System;

namespace JUUT.Attributes {
    /// <summary>
    /// Attribute to identify the class initializer of a test. Is runned once before the test methods are runned.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ClassSetUpAttribute : Attribute {
    }
}
