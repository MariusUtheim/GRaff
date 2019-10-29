

namespace GRaff.Randomness
{
    /// <summary>
    /// Generator for a constant distribution, i.e. one that always generates the same value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public sealed class ConstantDistribution<T> : IDistribution<T>
		where T : struct
	{
		T _constantValue;

		public ConstantDistribution(T constantValue)
		{
			_constantValue = constantValue;
		}

		public T Generate() => _constantValue;
	}

    public static class ConstantDistribution
    {
        public static ConstantDistribution<T> Create<T>(T constantValue) where T : struct 
            => new ConstantDistribution<T>(constantValue);
    }
}
