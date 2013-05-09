using System;

using NHamcrest;

namespace JUUT_Core {

    /// <summary>
    /// Exception which is thrown by failed assertions.<para />
    /// See <seealso cref="Assert" /> for more information how to assert conditions.
    /// </summary>
    [Serializable]
    public sealed class AssertException : Exception {

        /// <summary>
        /// Creates an new exception with an empty message.
        /// </summary>
        public AssertException() : this("") { }

        /// <summary>
        /// Creates an new exception and constructs an message out of the given descriptions.<para />
        /// This looks like: "Expected is name, but was missmatch."
        /// </summary>
        /// <param name="description">The description of the expected value.</param>
        /// <param name="missmatchDescription">The description of the missmatch.</param>
        public AssertException(IDescription description, IDescription missmatchDescription)
            : this("Expected is " + description + ", but " + missmatchDescription + ".") {
        }

        /// <summary>
        /// Creates an new exception and constructs an message out of the given descriptions and the <code>message</code>.<para />
        /// This looks like:<para />
        /// "Your custom message<para />
        /// Expected: name<para />
        /// But was missmatch"
        /// </summary>
        /// <param name="description">The description of the expected value.</param>
        /// <param name="missmatchDescription">The description of the missmatch.</param>
        /// <param name="message">Your custom message to be displayed in the test report.</param>
        public AssertException(IDescription description, IDescription missmatchDescription, string message)
            : this(message + "\nExpected: " + description + "\nBut " + missmatchDescription) {
        }

        /// <summary>
        /// Creates an new exception with given <code>message</code>.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        public AssertException(string message) : base(message) { }

        private bool Equals(AssertException other) {
            return Message.Equals(other.Message);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            return obj is AssertException && Equals((AssertException) obj);
        }

        public override int GetHashCode() {
            return Message.GetHashCode();
        }

    }
}
