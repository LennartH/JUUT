using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JUUT_Core.Attributes {

    public abstract class JUUTTestMethodAttribute : JUUTMethodAttribute {

        /// <summary>
        /// Returns true, if the test with this attribute is ready to run.
        /// </summary>
        public abstract bool IsTestReadyToRun { get; }

    }

}