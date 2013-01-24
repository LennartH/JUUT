using System;
using System.Linq;
using System.Reflection;

namespace JUUT.Core.Attributes {
    /// <summary>
    /// Attribute to identify test methods of JUUT. Can't be inherated. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class SimpleTestMethodAttribute : JUUTAttribute {

        public override string Name {
            get { return "SimpleTestMethod"; }
        }

        public override bool IsSetUpOrTearDown {
            get { return false; }
        }

        protected override AttributeMemberValidator GetValidator() {
            return delegate(MemberInfo member) {
                MethodInfo method = (MethodInfo) member;
                if (method.GetCustomAttribute<SimpleTestMethodAttribute>() != null) {
                    if (method.GetParameters().Count() != 0) {
                        throw new ArgumentException("The method " + method.Name + " has parameters.");
                    }

                    return true;
                }
                return false;
            };
        }

    }
}
