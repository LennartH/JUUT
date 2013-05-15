using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace JUUT_Unity {
    public static class Click {

        public static void On(GameObject gameObject) {
            foreach (MonoBehaviour behaviour in gameObject.GetComponents<MonoBehaviour>()) {
                behaviour.Invoke("OnMouseDown", 0);
            }
        }

    }
}
