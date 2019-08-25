using System;
namespace GRaff.Randomness
{
    public class FuncDistribution<T> : IDistribution<T>
    {

        public FuncDistribution(Func<T> generator)
        {
            this.Generator = generator;
        }

        public Func<T> Generator { get; }

        public T Generate() => Generator();
    }
}
