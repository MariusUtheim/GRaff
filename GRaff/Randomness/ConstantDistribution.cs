

namespace GRaff.Randomness
{
	public sealed class ConstantDistribution<T> : IDistribution<T>
		where T : struct
	{
		T _constantValue;

		public ConstantDistribution(T constantValue)
		{
			_constantValue = constantValue;
		}

		public T Generate()
		{
			return _constantValue;
		}
	}
}
