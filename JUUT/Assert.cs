using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHamcrest;

namespace JUUT
{
    public class Assert
    {

        public static void Fail() {
            Fail("");
        }

        public static void Fail(string message) {
            throw new AssertException(message);
        }

        public static void That<T>(T actual, IMatcher<T> matcher) {
            That(actual, matcher, null);
        }

        public static void That<T>(T actual, IMatcher<T> matcher, string message) {
            if (matcher.Matches(actual)) {
                return;
            }
            ThrowException(actual, matcher, message);
        }

        private static void ThrowException<T>(T actual, IMatcher<T> matcher, string message) {
            IDescription description = new StringDescription();
            matcher.DescribeTo(description);

            IDescription mismatchDescription = new StringDescription();
            matcher.DescribeMismatch(actual, mismatchDescription);

            if (message == null) {
                throw new AssertException(description, mismatchDescription);
            } else {
                throw new AssertException(description, mismatchDescription, message);
            }
        }

    }
}
