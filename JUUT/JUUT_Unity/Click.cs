using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using UnityEngine;

namespace JUUT_Unity {
    public static class Click {

        public static void On(GameObject gameObject) {
            foreach (MonoBehaviour behaviour in gameObject.GetComponents<MonoBehaviour>()) {
                MethodInfo onMouseDown = behaviour.GetType().GetMethod("OnMouseDown");
                if (onMouseDown != null) {
                    onMouseDown.Invoke(behaviour, null);
                }
                //Update is called before OnMouseDown. So tests for this will never success.
                //Invoking the OnMouseDown method with reflection instead.
                //behaviour.Invoke("OnMouseDown", 0);
            }
        }

    }
}
