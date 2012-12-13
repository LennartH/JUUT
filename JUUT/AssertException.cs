using System;

namespace JUUT {

    /// <summary>
    /// Exception to mark a failed assertion.
    /// </summary>
    public sealed class AssertException : Exception {

        public AssertException() : base("") { }

        public AssertException(string message) : base(message) { }

    }
}
