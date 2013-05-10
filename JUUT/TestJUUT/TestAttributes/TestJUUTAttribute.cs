using System.Reflection;
using System.Collections.Generic;

using JUUT_Core.Attributes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NHamcrest.Core;

using TestJUUT.Util;

namespace TestJUUT.TestAttributes {

    [TestClass]
    public class TestJUUTAttribute {

        [TestMethod]
        public void Marking() {
            JUUTAttribute JUUTAttribute = typeof(TestClassMock).GetCustomAttribute<JUUTAttribute>();
            AssertEx.That(JUUTAttribute, Is.NotNull(), "Registered no JUUT-Attribute method.");

            List<MethodInfo> attributedMethods = new List<MethodInfo>();
            foreach (MethodInfo method in typeof(TestClassMock).GetMethods()) {
                if (method.GetCustomAttribute(typeof(JUUTAttribute)) != null) {
                    attributedMethods.Add(method);
                }
            }
            AssertEx.That(attributedMethods.Count, Is.EqualTo(6), "There should be 6 attributed methods in " + typeof(TestClassMock).Name);
        }

    }
}
