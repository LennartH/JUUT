using JUUT_Core.Reporters;

using UnityEngine;

namespace JUUT_Unity {

    public class UnityTestReporter : ConsoleReporter {

        public override void PresentReports() {
            Debug.Log(CreateText());
        }

    }

}
