using System;

namespace JUUT.Core.Attributes {
    /// <summary>
    /// Attribute to identify test methods of JUUT. Can't be inherated. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class SimpleTestMethodAttribute : JUUTAttribute {

        public override string GetName() {
            return "SimpleTestMethod";
        }

        public override bool IsSetUpOrTearDown() {
            return false;
        }

    }
}
