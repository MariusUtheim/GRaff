using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
    static class Contract
    {

        public static void Requires<TException>(bool condition) where TException : Exception { if (!condition) throw Activator.CreateInstance<TException>(); }
        public static void Requires<TException>(bool condition, string msg) where TException : Exception { if (!condition) throw (TException)Activator.CreateInstance(typeof(TException), msg); }
        public static bool ForAll<T>(IEnumerable<T> collection, Func<T, bool> predicate) => collection.All(predicate);

        public static void Ensures(bool condition) { }
        public static T Result<T>() { return default(T); }
        public static void Assume(bool condition) { }
        public static void Invariant(bool condition) { }
    }
}
