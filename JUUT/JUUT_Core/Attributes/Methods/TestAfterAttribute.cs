using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Spring.Aop.Framework;

namespace JUUT_Core.Attributes.Methods {

    /// <summary>
    /// Attribute to identify test methods of JUUT that will be runned after another method is called. Can't be inherated. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestAfterAttribute : JUUTTestMethodAttribute {

        private bool ReadyToRun;

        public override bool IsTestReadyToRun {
            get { return ReadyToRun; }
        }

        public override string Name {
            get { return "TestAfter"; }
        }

        public override bool IsSetUpOrTearDown {
            get { return false; }
        }

        //TODO Delete when time! Currently needed for member validation
        public TestAfterAttribute() {
        }

        public TestAfterAttribute(Type targetClass, string targetMethod) {
            ReadyToRun = false;
        }

        protected override bool Validate(MemberInfo member) {
            MethodInfo method = (MethodInfo) member;
            if (method.GetCustomAttribute<TestAfterAttribute>() != null) {
                if (method.GetParameters().Count() != 0) {
                    throw new ArgumentException("The method " + method.Name + " has parameters.");
                }

                return true;
            }
            return false;
        }

    }

}
