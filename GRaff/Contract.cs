﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GRaff
{
#nullable disable
    public static class Contract
    {
        [Conditional("DEBUG")]
        public static void Requires(bool predicate) { if (!predicate) throw new Exception(); }

        [Conditional("DEBUG")]
        public static void Requires<TException>(bool predicate) where TException : Exception
        {
            if (!predicate) throw Activator.CreateInstance<TException>();
        }

        [Conditional("DEBUG")]
        public static void Requires<TException>(bool predicate, string message) where TException : Exception
        {
            if (!predicate) throw (TException)Activator.CreateInstance(typeof(TException), message);
        }

        [Conditional("DEBUG")]
        public static void Ensures(bool _) { }

        public static T Result<T>() => default;

        [Conditional("DEBUG")]
        public static void Invariant(object _) { }

        public static bool ForAll<T>(IEnumerable<T> collection, Func<T, bool> predicate) => collection.All(predicate);

        [Conditional("DEBUG")]
        public static void Assume(bool predicate) { if (!predicate) throw new Exception(); }
    }
}
