using System;

using NHamcrest;

namespace JUUT {

    /// <summary>
    /// Exception to mark a failed assertion.
    /// </summary>
    public sealed class AssertException : Exception {

        public AssertException() : this("") { }

        public AssertException(IDescription description, IDescription missmatchDescription)
            : this("Expected is " + description + ", but " + missmatchDescription + ".") {
        }

        public AssertException(IDescription description, IDescription missmatchDescription, string custom)
            : this(custom + "\nExpected: " + description + "\nBut " + missmatchDescription) {
        }

        public AssertException(string message) : base(message) { }

    }
}
