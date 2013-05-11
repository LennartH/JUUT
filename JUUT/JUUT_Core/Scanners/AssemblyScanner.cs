using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using JUUT_Core.Attributes;
using JUUT_Core.Sessions;

namespace JUUT_Core.Scanners {

    public static class AssemblyScanner {

        public static HashSet<Type> GetTestClassesIn(Assembly assembly) {
            HashSet<Type> testClasses = new HashSet<Type>();
            foreach (Type type in assembly.GetTypes()) {
                if (type.GetCustomAttribute<JUUTTestClassAttribute>() != null) {
                    testClasses.Add(type);
                }
            }
            return testClasses;
        }

        public static Session CreateSessionFor(Assembly assembly) {
            Session session = new Session();
            foreach (Type testClass in GetTestClassesIn(assembly)) {
                session.AddAll(testClass);
            }
            return session;
        }

        public static Session CreateSessionFor(IEnumerable<Assembly> assemblies) {
            Session session = new Session();
            foreach (Assembly assembly in assemblies) {
                foreach (Type testClass in GetTestClassesIn(assembly)) {
                    session.AddAll(testClass);
                } 
            }
            return session;
        }

    }

}
