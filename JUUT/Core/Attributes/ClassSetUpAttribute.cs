using System;

namespace JUUT.Core.Attributes {
    /// <summary>
    /// Attribute to identify the class initializer of a test class. Is runned once before the test methods are runned.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ClassSetUpAttribute : JUUTAttribute {

        public override string Name {
            get { return "ClassSetUp"; }
        }

        public override bool IsSetUpOrTearDown {
            get { return true; }
        }

        protected override AttributeMemberValidator GetValidator() {
            throw new NotImplementedException();
        }

    }
}
