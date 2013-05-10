using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JUUT_Core {

    internal static class Util {

        public static T GetCustomAttribute<T>(this MemberInfo memberInfo) where T : Attribute {
            Attribute attribute = Attribute.GetCustomAttribute(memberInfo, typeof(T));
            return attribute == null ? null : attribute as T;
        }

    }

}
