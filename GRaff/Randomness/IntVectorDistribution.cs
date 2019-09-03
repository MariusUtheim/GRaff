using System;
namespace GRaff.Randomness
{
    /// <summary>
    /// Generator for IntVectors where the x and y coordinates are independently distributed values
    /// </summary>
    public sealed class IntVectorDistribution : IDistribution<IntVector>
    {
        private readonly IDistribution<int> _first, _second;

        public IntVectorDistribution(IDistribution<int> first, IDistribution<int> second)
        {
            _first = first;
            _second = second;
        }

        public IntVectorDistribution(int firstLowerInclusive, int firstUpperExclusive, int secondLowerInclusive, int secondUpperExclusive)
            : this(GRandom.Source, firstLowerInclusive, firstUpperExclusive, secondLowerInclusive, secondUpperExclusive)
        { }

        public IntVectorDistribution(Random rnd, int firstLowerInclusive, int firstUpperExclusive, int secondLowerInclusive, int secondUpperExclusive)
            : this(new IntegerDistribution(rnd, firstLowerInclusive, firstUpperExclusive),
                   new IntegerDistribution(rnd, secondLowerInclusive, secondUpperExclusive))
        { }

        public IntVectorDistribution(Random rnd, IntRectangle region)
            : this(rnd, region.Left, region.Right, region.Top, region.Bottom)
        { }

        public IntVectorDistribution(IntRectangle region)
            : this(GRandom.Source, region)
        { }

        public IntVector Generate() => new IntVector(_first.Generate(), _second.Generate());
    }
}
