using System;
using System.Reflection;

namespace JUUT.Core.Attributes {
    /// <summary>
    /// Attribute to identify test classes of JUUT. Can't be inherated. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class JUUTTestClassAttribute : JUUTAttribute {

        public override string Name {
            get { return "JUUTTestClass"; }
        }

        public override bool IsSetUpOrTearDown {
            get { return false; }
        }

        protected override bool Validate(MemberInfo member) {
            if (member == null) {
                throw new ArgumentNullException();
            }
            if (!(member is Type)) {
                throw new ArgumentException("The given member is no class.");
            }

            if (member.GetCustomAttribute<JUUTTestClassAttribute>() != null) {
                return true;
            }
            throw new ArgumentException("The class " + member.Name + " doesn't have the JUUTTestClass-Attribute.");
        }

    }
}
