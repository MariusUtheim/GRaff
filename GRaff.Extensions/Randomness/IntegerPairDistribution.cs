using System;
namespace GRaff.Randomness
{
    public sealed class IntegerPairDistribution : IDistribution<IntVector>
    {
        private readonly IDistribution<int> _first, _second;

        public IntegerPairDistribution(IDistribution<int> first, IDistribution<int> second)
        {
            _first = first;
            _second = second;
        }

        public IntegerPairDistribution(int firstLowerInclusive, int firstUpperExclusive, int secondLowerInclusive, int secondUpperExclusive)
            : this(GRandom.Source, firstLowerInclusive, firstUpperExclusive, secondLowerInclusive, secondUpperExclusive)
        { }

        public IntegerPairDistribution(Random rnd, int firstLowerInclusive, int firstUpperExclusive, int secondLowerInclusive, int secondUpperExclusive)
            : this(new IntegerDistribution(rnd, firstLowerInclusive, firstUpperExclusive),
                   new IntegerDistribution(rnd, secondLowerInclusive, secondUpperExclusive))
        { }

        public IntegerPairDistribution(Random rnd, IntRectangle region)
            : this(rnd, region.Left, region.Right, region.Top, region.Bottom)
        { }

        public IntegerPairDistribution(IntRectangle region)
            : this(GRandom.Source, region)
        { }

        public IntVector Generate()
        {
            return new IntVector(_first.Generate(), _second.Generate());
        }
    }
}
