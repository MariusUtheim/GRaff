using System;
using System.Collections.Generic;


namespace GRaff.Randomness
{
	public interface IDistribution<out T>
	{
		T Generate();
	}

	public static class DistributionExtensions
	{
		public static IEnumerable<T> Generate<T>(this IDistribution<T> distribution, int count)
		{
			for (int i = 0; i < count; i++)
				yield return distribution.Generate();
		}

        public static IDistribution<TOut> Transform<TIn, TOut>(this IDistribution<TIn> distribution, Func<TIn, TOut> transformer)
            => new FuncDistribution<TOut>(() => transformer(distribution.Generate()));
	}
}
