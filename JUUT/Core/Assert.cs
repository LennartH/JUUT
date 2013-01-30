using JUUT.Core.Impl;

using NHamcrest;

namespace JUUT.Core {

    /// <summary>
    /// Class to assert conditions to tests.
    /// </summary>
    public static class Assert {

        /// <summary>
        /// Causes the test to fail without a message.
        /// </summary>
        public static void Fail() {
            Fail("");
        }

        /// <summary>
        /// Causes the test to fail with the given <code>message</code>.
        /// </summary>
        /// <param name="message">The message displayed in the test report.</param>
        public static void Fail(string message) {
            throw new AssertException(message);
        }

        /// <summary>
        /// Assert that the <code>actual</code> object matches something specified by the <code>matcher</code>.<para />
        /// Causes the test to fail, if it doesn't matches.
        /// </summary>
        /// <param name="actual">Object to be tested.</param>
        /// <param name="matcher">Specifies what the <code>actual</code> object has to match.</param>
        public static void That<T>(T actual, IMatcher<T> matcher) {
            That(actual, matcher, null);
        }

        /// <summary>
        /// Assert that the <code>actual</code> object matches something specified by the <code>matcher</code>.<para />
        /// Causes the test to fail, if it doesn't matches and reports the given message.
        /// </summary>
        /// <param name="actual">Object to be tested.</param>
        /// <param name="matcher">Specifies what the <code>actual</code> object has to match.</param>
        /// <param name="message">The message displayed in the test report.</param>
        public static void That<T>(T actual, IMatcher<T> matcher, string message) {
            if (matcher.Matches(actual)) {
                return;
            }
            throw ConstructException(actual, matcher, message);
        }

        /// <summary>
        /// Constructs the exception for the given information.
        /// </summary>
        /// <param name="actual">The tested object.</param>
        /// <param name="matcher">The matcher which tested <code>actual</code>.</param>
        /// <param name="message">The custom message given by the <code>Assert.That</code>-Caller.</param>
        private static AssertException ConstructException<T>(T actual, IMatcher<T> matcher, string message) {
            IDescription description = new StringDescription();
            matcher.DescribeTo(description);

            IDescription mismatchDescription = new StringDescription();
            matcher.DescribeMismatch(actual, mismatchDescription);

            return message == null ? new AssertException(description, mismatchDescription) : new AssertException(description, mismatchDescription, message);
        }

    }

}
