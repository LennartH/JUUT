using System;

namespace JUUT.Core.Attributes {
    /// <summary>
    /// Attribute to identify the test cleaner of a test class. Is runned every time after a test method is runned.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TestTearDownAttribute : JUUTAttribute {

        public override string Name {
            get { return "TestTearDown"; }
        }

        public override bool IsSetUpOrTearDown {
            get { return true; }
        }

        protected override AttributeMemberValidator GetValidator() {
            throw new NotImplementedException();
        }

    }
}
