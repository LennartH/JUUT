using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

using JUUT_Core.Attributes;
using JUUT_Core.Reports;
using JUUT_Core.Sessions;

using JUUT_Unity;

using UnityEngine;

using Debug = UnityEngine.Debug;

namespace Assets.Tests.JUUT {

    public class TestStarter : MonoBehaviour {

        private UnityTestSuite Suite;
        private Session Session;
        private bool TestsRunned;

        // Use this to set up the test suite
        public void Start () {
            Session = new Session();
            Session.AddAll(typeof(TestSample));

            Suite = new UnityTestSuite();
            TestsRunned = false;
        }

        private void Update() {
            if (!TestsRunned) {
                Suite.RunSimpleTests(Session);
                TestsRunned = true;
            }
        }

        public void OnGUI() {
            ICollection<MethodReport> failedTests = Suite.FailedTests;
            if (failedTests.Count != 0) {
                int i = 0;
                foreach (MethodReport failedTest in failedTests) {
                    string filePath = null;
                    int lineNumber = -1;
                    StackTrace trace = new StackTrace(failedTest.RaisedException, true);
                    foreach (StackFrame frame in trace.GetFrames()) {
                        if (frame.GetMethod().DeclaringType.GetCustomAttribute<JUUTTestClassAttribute>() != null) {
                            string fullPath = frame.GetFileName();
                            if (fullPath != null) {
                                filePath = fullPath.Substring(fullPath.IndexOf("Assets", System.StringComparison.Ordinal));
                                lineNumber = frame.GetFileLineNumber();
                            }
                            break;
                        }
                    }
                    if (filePath != null) {
                        if (GUI.Button(new Rect(10, 10 + i * 25, 600, 20), failedTest.ShortText)) {
                            JUUTUnityUtil.OpenTestFile(filePath, lineNumber);
                        } 
                    }
                    i++;
                }
            }
        }

    }

}
