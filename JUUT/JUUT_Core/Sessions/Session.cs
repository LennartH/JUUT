using System;
using System.Collections.Generic;
using System.Reflection;

namespace JUUT_Core.Sessions {
    public class Session {

        private readonly Dictionary<Type, TestClassSession> ClassSessionsDictionary;

        /// <summary>
        /// Returns the test class sessions contained by this session.
        /// </summary>
        public IEnumerable<TestClassSession> TestClassSessions {
            get { return ClassSessionsDictionary.Values; }
        }

        public Session() {
            ClassSessionsDictionary = new Dictionary<Type, TestClassSession>();
        }

        /// <summary>
        /// Adds all tests of the given test class to be runned by the suite.
        /// </summary>
        public void AddAll(Type testClass) {
            if (!ClassSessionsDictionary.ContainsKey(testClass)) {
                ClassSessionsDictionary.Add(testClass, new TestClassSession(testClass));
            }
            ClassSessionsDictionary[testClass].AddAll();
        }

        /// <summary>
        /// Adds the given test method to be runned by the suite.
        /// </summary>
        public void Add(MethodInfo test) {
            if (test.DeclaringType == null) {
                return;
            }

            if (!ClassSessionsDictionary.ContainsKey(test.DeclaringType)) {
                ClassSessionsDictionary.Add(test.DeclaringType, new TestClassSession(test.DeclaringType));
            }
            ClassSessionsDictionary[test.DeclaringType].Add(test);
        }

    }
}
