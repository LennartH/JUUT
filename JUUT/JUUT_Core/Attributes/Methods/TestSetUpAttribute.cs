using System;
using System.Linq;
using System.Reflection;

namespace JUUT_Core.Attributes.Methods {
    /// <summary>
    /// Attribute to identify the test initializer of a test class. Is runned every time before a test method is runned.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TestSetUpAttribute : JUUTMethodAttribute {

        public override string Name {
            get { return "TestSetUp"; }
        }

        public override bool IsSetUpOrTearDown {
            get { return true; }
        }

        protected override bool Validate(MemberInfo member) {
            MethodInfo method = (MethodInfo) member;
            if (method.GetCustomAttribute<TestSetUpAttribute>() != null) {
                if (method.GetParameters().Count() != 0) {
                    throw new ArgumentException("The method " + method.Name + " has parameters.");
                }

                return true;
            }
            return false;
        }

    }
}
