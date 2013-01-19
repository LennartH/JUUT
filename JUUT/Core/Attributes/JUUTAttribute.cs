using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JUUT.Core.Attributes {
    public abstract class JUUTAttribute : Attribute {

        /// <summary>
        /// Returns the name of the attribute.
        /// </summary>
        /// <returns></returns>
        public abstract string GetName();

        /// <summary>
        /// Returns true, if the attribute is ClassSetUp, TestSetUp, TestTearDown or TestSetUp.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsSetUpOrTearDown();

    }
}
