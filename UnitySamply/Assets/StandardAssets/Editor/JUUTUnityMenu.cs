using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using JUUT_Core.Scanners;

using JUUT_Unity;

using UnityEditor;

using UnityEngine;

namespace Assets.StandardAssets.Editor {

    public class JUUTUnityMenu {

        private static void ClearDebugLog() {
            Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
            Type type = assembly.GetType("UnityEditorInternal.LogEntries");
            MethodInfo method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }

        [MenuItem("JUUT/Run All Tests %#t")]
        private static void RunAllTests() {
            ClearDebugLog();

            UnityTestSuite suite = new UnityTestSuite();
            suite.Run(AssemblyScanner.CreateSessionFor(AppDomain.CurrentDomain.GetAssemblies()));
        }

    }

}
