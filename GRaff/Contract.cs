using System;
using System.Collections.Generic;
using System.Linq;

namespace GRaff
{
    public static class Contract
    {
        public static void Requires(bool predicate) { if (!predicate) throw new Exception(); }
        public static void Requires<TException>(bool predicate) where TException : Exception
        {
            if (!predicate) throw Activator.CreateInstance<TException>();
        }
        public static void Requires<TException>(bool predicate, string message) where TException : Exception
        {
            if (!predicate) throw (TException)Activator.CreateInstance(typeof(TException), message);
        }

        public static void Ensures(bool _) { }
        public static T Result<T>() => default(T);
        public static void Invariant(object _) { }
        public static bool ForAll<T>(IEnumerable<T> collection, Func<T, bool> predicate) => collection.All(predicate);

        public static void Assume(bool predicate) { if (!predicate) throw new Exception(); }
    }
}
