using System;
using System.Reflection;

using UnityEditor;

namespace Assets.Tests.JUUT {

    public static class JUUTUnityUtil {

        public static void OpenTestFile(string testFilePath, int lineNumber) {
            AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath(testFilePath, typeof(MonoScript)), lineNumber);
        }

        public static T GetCustomAttribute<T>(this MemberInfo memberInfo) where T : Attribute {
            Attribute attribute = Attribute.GetCustomAttribute(memberInfo, typeof(T));
            return attribute == null ? null : attribute as T;
        }

    }

}
