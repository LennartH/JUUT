using System;
using System.Linq;
using System.Reflection;

namespace JUUT_Core.Attributes.Methods {
    /// <summary>
    /// Attribute to identify the class cleaner of a test. Is runned once after all the test methods are runned.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ClassTearDownAttribute : JUUTMethodAttribute {

        public override string Name {
            get { return "ClassTearDown"; }
        }

        public override bool IsSetUpOrTearDown {
            get { return true; }
        }

        protected override bool Validate(MemberInfo member) {
            MethodInfo method = (MethodInfo) member;
            if (method.GetCustomAttribute<ClassTearDownAttribute>() != null) {
                if (!method.IsStatic) {
                    throw new ArgumentException("The method " + method.Name + " isn't static.");
                }
                if (method.GetParameters().Count() != 0) {
                    throw new ArgumentException("The method " + method.Name + " has parameters.");
                }

                return true;
            }
            return false;
        }

    }
}
