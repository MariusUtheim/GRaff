using System;
namespace GRaff.Randomness
{
    public class MonteCarloDistribution<T> : IDistribution<T>
    {
        private IDistribution<T> _sampler;
        private Func<T, bool> _accepter;

        public MonteCarloDistribution(IDistribution<T> sampler, Func<T, bool> accepter)
        {
            this._sampler = sampler;
            this._accepter = accepter;
        }

        public T Generate()
        {
            T sample;
            do
                sample = _sampler.Generate();
            while (!_accepter(sample));

            return sample;
        }

        public bool HitMiss() => _accepter(_sampler.Generate());
    }
}
