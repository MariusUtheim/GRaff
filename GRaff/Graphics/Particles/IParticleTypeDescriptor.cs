

using System;
using System.Diagnostics.Contracts;

namespace GRaff.Graphics.Particles
{
    /// <summary>
    /// 
    /// </summary>
	public interface IParticleTypeDescriptor
	{
        /// <summary>
        /// Get a GRaff.Graphics.Particles.IParticleBehavior to be attached to a particle.
        /// </summary>
        IParticleBehavior  MakeBehavior();
	}

}
