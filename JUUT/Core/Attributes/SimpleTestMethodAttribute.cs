using System;

namespace JUUT.Core.Attributes {
    /// <summary>
    /// Attribute to identify test methods of JUUT. Can't be inherated. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class SimpleTestMethodAttribute : JUUTAttribute {

        public SimpleTestMethodAttribute() : base(false, "SimpleTestMethod") { }

        protected override AttributeMemberValidator GetValidator() {
            throw new NotImplementedException();
        }

    }
}
