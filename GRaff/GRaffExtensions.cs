using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Synchronization;

namespace GRaff
{
	internal static class GRaffExtensions
	{

        internal static IEnumerable<TOut> SelectMany<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn, ValueTuple<TOut, TOut>> map)
        {
            foreach (var v in enumerable)
            {
                var w = map(v);
                yield return w.Item1;
                yield return w.Item2;
            }
        }

        internal static IEnumerable<TOut> SelectMany<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn, ValueTuple<TOut, TOut, TOut>> map)
        {
            foreach (var v in enumerable)
            {
                var w = map(v);
                yield return w.Item1;
                yield return w.Item2;
                yield return w.Item3;
            }
        }

        internal static T Reduce<T>(this IEnumerable<T> enumerable, Func<T, T, T> reducer)
        {
            if (!enumerable.Any())
                throw new ArgumentException("Cannot reduce an empty list");

            var current = enumerable.First();
            foreach (var v in enumerable.Skip(1))
                current = reducer(current, v);
            return current;
        }

	}
}
