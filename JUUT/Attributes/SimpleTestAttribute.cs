using System;

namespace JUUT.Attributes {
    /// <summary>
    /// Attribute to identify test classes of JUUT. Can't be inherated. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class SimpleTestAttribute : Attribute {

    }
}
