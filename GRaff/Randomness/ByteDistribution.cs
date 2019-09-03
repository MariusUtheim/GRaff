using System;

namespace GRaff.Randomness
{
    /// <summary>
    /// Generator for bytes according to a uniform distribution.
    /// </summary>
	public sealed class ByteDistribution : IDistribution<byte>
    {
        private readonly Random _rnd;

        public ByteDistribution()
            : this(GRandom.Source) { }

        public ByteDistribution(Random rnd)
        {
            _rnd = rnd;
        }

        public byte Generate() => _rnd.Byte();
    }
}
