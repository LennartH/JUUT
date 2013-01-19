namespace JUUT.Core.Impl.Reports {

    /// <summary>
    /// Class for the different ranges reports can be about.
    /// </summary>
    public abstract class ReportRange {

        /// <summary>
        /// This indicates, that the report is about a test method.
        /// </summary>
        public static readonly ReportRange TestMethod = new TestMethodRange();

        /// <summary>
        /// This indicates, that the report is about a test class.
        /// </summary>
        public static readonly ReportRange TestClass = new TestClassRange();

        protected ReportRange() { }

        public abstract string Name { get; }

        public override string ToString() {
            return GetType().Name;
        }

        private class TestMethodRange : ReportRange {

            public override string Name {
                get { return "TestMethod"; }
            }

        }
        private class TestClassRange : ReportRange {

            public override string Name {
                get { return "TestClass"; }
            }

        }

    }

}