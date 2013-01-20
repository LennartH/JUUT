using System;
using System.Reflection;

namespace JUUT.Core.Attributes {
    /// <summary>
    /// Attribute to identify test classes of JUUT. Can't be inherated. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class JUUTTestClassAttribute : JUUTAttribute {

        public JUUTTestClassAttribute() : base(false, "JUUTTestClass") { }

        protected override AttributeMemberValidator GetValidator() {
            return delegate(MemberInfo member) {
                if (!(member is Type)) {
                    throw new ArgumentException("The given member is no class.");
                }

                if (member.GetCustomAttribute<JUUTTestClassAttribute>() != null) {
                    return true;
                }
                throw new ArgumentException("The class " + member.Name + " doesn't have the JUUTTestClass-Attribute.");
            };
        }

    }
}
