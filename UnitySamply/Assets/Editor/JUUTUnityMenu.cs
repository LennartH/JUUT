using System;
using System.Reflection;

using JUUT_Core.Scanners;

using JUUT_Unity;

using UnityEditor;

namespace Assets.Editor {

    public class JUUTUnityMenu {

        private static void ClearDebugLog() {
            Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
            Type type = assembly.GetType("UnityEditorInternal.LogEntries");
            MethodInfo method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }

        [MenuItem("JUUT/Run all simple tests %#t")]
        private static void RunAllTests() {
            ClearDebugLog();

            UnityTestSuite suite = new UnityTestSuite();
            suite.RunSimpleTests(AssemblyScanner.CreateSessionFor(AppDomain.CurrentDomain.GetAssemblies()));
        }

    }

}
