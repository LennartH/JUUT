using MbUnit.Framework;
using NHamcrest.Core;
using Assert = NHamcrest.Tests.Internal.Assert;

namespace NHamcrest.Tests.Core
{
    public class StringEndsWithTests
    {
        [Test]
        public void Match_if_string_ends_with_substring()
        {
            Assert.That("the cat sat on the mat", Ends.With("mat"));
        }

        [Test]
        public void Case_insensitive_match_if_string_ends_with_substring()
        {
            Assert.That("the cat sat on the mat", Ends.With("Mat").CaseInsensitive());
        }

        [Test]
        public void No_match_if_string_does_not_end_with_substring()
        {
            var matcher = Ends.With("bob");

            var matches = matcher.Matches("the cat sat on the mat");

            Assert.That(matches, Is.False());
        }

        [Test]
        public void Describe_mismatch()
        {
            var matcher = Ends.With("bob");
            var description = new StringDescription();

            matcher.DescribeMismatch("the cat sat on the mat", description);

            Assert.That(description.ToString(), Is.EqualTo("was \"the cat sat on the mat\""));
        }

        [Test]
        public void Describe_to()
        {
            var matcher = Ends.With("bob");
            var description = new StringDescription();

            matcher.DescribeTo(description);

            Assert.That(description.ToString(), Is.EqualTo("a string ending with \"bob\""));
        }
    }
}