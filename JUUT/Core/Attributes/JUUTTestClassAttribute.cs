using System;

namespace JUUT.Core.Attributes {
    /// <summary>
    /// Attribute to identify test classes of JUUT. Can't be inherated. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class JUUTTestClassAttribute : JUUTAttribute {

        public override string GetName() {
            return "JUUTTestClass";
        }

        public override bool IsSetUpOrTearDown() {
            return false;
        }

    }
}
