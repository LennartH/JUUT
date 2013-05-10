using JUUT_Unity;

using UnityEngine;

namespace Assets.Tests.JUUT {

    public class TestStarter : MonoBehaviour {

        // Use this to set up the test suite
        void Start () {
            UnityTestSuite suite = new UnityTestSuite();
            suite.AddAll(typeof(TestSample));
            suite.Run();
        }

    }

}
