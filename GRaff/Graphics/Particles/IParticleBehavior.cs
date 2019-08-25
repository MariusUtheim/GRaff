

namespace GRaff.Graphics.Particles
{
	public interface IParticleBehavior
	{
        void Initialize(Particle particle);

		void Update(Particle particle);
	}

	
}