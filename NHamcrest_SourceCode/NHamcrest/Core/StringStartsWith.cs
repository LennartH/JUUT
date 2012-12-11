namespace NHamcrest.Core
{
    public class StringStartsWith : SubstringMatcher
    {
        public StringStartsWith(string substring) : base(substring)
        {
        }

        protected override bool EvalSubstringOf(string @string)
        {
            return @string.StartsWith(Substring, StringComparison);
        }

        protected override string Relationship()
        {
            return "starting with";
        }
    }

    public static class Starts
    {
        public static StringStartsWith With(string substring)
        {
            return new StringStartsWith(substring);
        }
    }
}