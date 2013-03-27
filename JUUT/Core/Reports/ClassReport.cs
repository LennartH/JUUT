using System;
using System.Collections.Generic;
using System.Reflection;

using JUUT.Core.Attributes;

namespace JUUT.Core.Reports {

    public class ClassReport : Report {

        public string Text {
            get {
                if (FailedTests == 0 && SucceededTests == 0 && NotRunnedTests == 0) {
                    return "No tests have been runned.";
                }
                return "";
            }
        }

        private Dictionary<MethodInfo, MethodReport> Reports; 
        private int FailedTests;
        private int SucceededTests;
        private int NotRunnedTests;

        public Type ClassType { get; private set; }

        public ReportStatus Status { get; private set; }

        public ClassReport(Type classType) {
            if (classType == null) {
                throw new ArgumentException("The class type of a ClassReport mustn't be null.");
            }
            if (classType.GetCustomAttribute<JUUTTestClassAttribute>() == null) {
                throw new ArgumentException("The class needs the JUUTTestClass-Attribute.");
            }

            Reports = new Dictionary<MethodInfo, MethodReport>();
            ClassType = classType;
            Status = new ReportStatus.NotRunned();
        }

        public void Add(MethodReport report) {
            Status = report.Status.IsWorseThan(Status) ? report.Status : Status;
        }

    }

}