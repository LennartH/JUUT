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
        /// <value></value>
        public string Name { get; private set; }

        /// <summary>
        /// Returns true, if the attribute is ClassSetUp, TestSetUp, TestTearDown or TestSetUp.
        /// </summary>
        /// <value></value>
        public bool IsSetUpOrTearDown { get; private set; }

        protected JUUTAttribute(bool isSetUpOrTearDown, string name) {
            Name = name;
            IsSetUpOrTearDown = isSetUpOrTearDown;
        }

    }
}
