
 
namespace GRaff.Graphics.Particles
{
	public class FadeoutDescriptor : IParticleTypeDescriptor
	{

        public static FadeoutDescriptor Default { get; } = new FadeoutDescriptor();

        class FadeoutBehavior : IParticleBehavior

        {
            public void Initialize(Particle particle) { }
            public void Update(Particle particle)
                => particle.Blend = particle.Blend.Transparent(1 - particle.Lifetime / particle.TotalLifetime);
        }

        private static FadeoutBehavior _theBehavior = new FadeoutBehavior();
        public IParticleBehavior  MakeBehavior() => _theBehavior;
	}
}
