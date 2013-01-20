using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JUUT.Core.Attributes {
    public abstract class JUUTAttribute : Attribute {

        /// <summary>
        /// Returns the name of the attribute.
        /// </summary>
        /// <value></value>
        public string Name { get; private set; }

        /// <summary>
        /// Returns true, if the attribute is ClassSetUp, TestSetUp, TestTearDown or TestSetUp.
        /// </summary>
        /// <value></value>
        public bool IsSetUpOrTearDown { get; private set; }

        protected delegate bool AttributeMemberValidator(MemberInfo member);

        protected JUUTAttribute(bool isSetUpOrTearDown, string name) {
            Name = name;
            IsSetUpOrTearDown = isSetUpOrTearDown;
        }

        /// <summary>
        /// Returns true, if the given member is valid for the given attribute.<para />
        /// If the member is critically wrong an exception is thrown.
        /// </summary>
        /// <param name="attribute">The attribute for which the member should be checked. Has to be a type of a JUUTAttribute.</param>
        /// <param name="memberToCheck">The member to check.</param>
        /// <returns></returns>
        public static bool IsMemberValidFor(Type attribute, MemberInfo memberToCheck) {
            AttributeMemberValidator validator = ((JUUTAttribute) Activator.CreateInstance(attribute)).GetValidator();
            return validator(memberToCheck);
        }

        protected abstract AttributeMemberValidator GetValidator();

    }
}
