﻿using System;
using System.Reflection;

namespace JUUT_Core {

    internal static class Util {

        public static T GetCustomAttribute<T>(this MemberInfo memberInfo) where T : Attribute {
            Attribute attribute = Attribute.GetCustomAttribute(memberInfo, typeof(T));
            return attribute == null ? null : attribute as T;
        }

    }

}
