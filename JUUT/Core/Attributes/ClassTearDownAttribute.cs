using System;

namespace JUUT.Core.Attributes {
    /// <summary>
    /// Attribute to identify the class cleaner of a test. Is runned once after all the test methods are runned.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ClassTearDownAttribute : JUUTAttribute {

        public override string GetName() {
            return "ClassTearDown";
        }

        public override bool IsSetUpOrTearDown() {
            return true;
        }

    }
}
