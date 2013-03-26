using System;
using System.Reflection;

using JUUT.Core.Attributes;

namespace JUUT.Core.Reports {

    public class ClassReport : Report {

        public string Text { get; private set; }

        public Type ClassType { get; private set; }

        public ReportStatus Status { get; private set; }

        public ClassReport(Type classType) {
            if (classType == null) {
                throw new ArgumentException("The class type of a ClassReport mustn't be null.");
            }
            if (classType.GetCustomAttribute<JUUTTestClassAttribute>() == null) {
                throw new ArgumentException("The class needs the JUUTTestClass-Attribute.");
            }

            ClassType = classType;
            Status = new ReportStatus.NotRunned();
        }

    }

}