using System;
using System.Linq;
using System.Reflection;

namespace JUUT_Core.Attributes.Methods {
    /// <summary>
    /// Attribute to identify test methods of JUUT. Can't be inherated. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class SimpleTestMethodAttribute : JUUTMethodAttribute {

        public override string Name {
            get { return "SimpleTestMethod"; }
        }

        public override bool IsSetUpOrTearDown {
            get { return false; }
        }

        protected override bool Validate(MemberInfo member) {
            MethodInfo method = (MethodInfo) member;
            if (method.GetCustomAttribute<SimpleTestMethodAttribute>() != null) {
                if (method.GetParameters().Count() != 0) {
                    throw new ArgumentException("The method " + method.Name + " has parameters.");
                }

                return true;
            }
            return false;
        }

    }
}
