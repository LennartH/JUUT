using System;
using System.Reflection;

using JUUT.Core.Attributes;

namespace JUUT.Core.Impl.Reports {

    public class TestClassReport : AbstractReport {

        public TestClassReport(MethodInfo runnedMethod, Exception raisedException)
            : base(runnedMethod, ReportRange.TestClass) {
            CheckThatTheArgumentsAreCorrect(raisedException);
            
            SetText(raisedException);
        }

        private void CheckThatTheArgumentsAreCorrect(Exception raisedException) {
            if (raisedException == null) {
                throw new ArgumentException("The exception of a TestClassReport can't be null.");
            }
            if (!IsMethodSetUpOrTearDown()) {
                throw new ArgumentException("The method of a TestClassReport has to have the ClassSetUp-, TestSetUp-, TestTearDown- or ClassTearDown-Attribute.");
            }
        }

        private bool IsMethodSetUpOrTearDown() {
            JUUTAttribute attribute = RunnedMethod.GetCustomAttribute<JUUTAttribute>();
            return attribute != null && attribute.IsSetUpOrTearDown;
        }

        private void SetText(Exception raisedException) {
            Text = "The " + RunnedMethod.GetCustomAttribute<JUUTAttribute>().Name + "-Method " + RunnedMethod.Name +
                   " of the test class " + TestClass.Name +
                   " raised an exception: " + raisedException.Message;
        }

    }

}