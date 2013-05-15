using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Scripts {

    public class SphereScript : MonoBehaviour {

        public bool Clicked { get; private set; }

        public void Start() {
            Clicked = false;
        }

        public void Update() {

        }

        public void OnMouseDown() {
            Clicked = true;
        }

    }

}
