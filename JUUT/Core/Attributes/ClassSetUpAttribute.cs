using System;

namespace JUUT.Core.Attributes {
    /// <summary>
    /// Attribute to identify the class initializer of a test class. Is runned once before the test methods are runned.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ClassSetUpAttribute : JUUTAttribute {

        public ClassSetUpAttribute() : base(true, "ClassSetUp") { }

        protected override AttributeMemberValidator GetValidator() {
            throw new NotImplementedException();
        }

    }
}
