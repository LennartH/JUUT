using System;

namespace JUUT.Core.Attributes {
    /// <summary>
    /// Attribute to identify the class cleaner of a test. Is runned once after all the test methods are runned.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ClassTearDownAttribute : JUUTAttribute {

        public override string Name {
            get { return "ClassTearDown"; }
        }

        public override bool IsSetUpOrTearDown {
            get { return true; }
        }

        protected override AttributeMemberValidator GetValidator() {
            throw new NotImplementedException();
        }

    }
}
