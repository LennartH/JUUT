using JUUT_Core.Sessions;

using JUUT_Unity;

using UnityEngine;

namespace Assets.Tests.JUUT {

    public class TestStarter : MonoBehaviour {

        // Use this to set up the test suite
        void Start () {
            Session session = new Session();
            session.AddAll(typeof(TestSample));

            UnityTestSuite suite = new UnityTestSuite();
            suite.RunSimpleTests(session);
        }

    }

}
