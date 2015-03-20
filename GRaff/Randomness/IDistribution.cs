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
	}
}
