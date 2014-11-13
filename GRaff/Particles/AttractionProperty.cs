using System;

namespace GRaff.Particles
{
	public class AttractionProperty : IParticleProperty
	{
		private Point _origin;
		private double _vx = 0, _vy = 0;
		private double _strength;

		public AttractionProperty(Point _origin, double _strength)
		{
			this._origin = _origin;
			this._strength = _strength;
		}

		public void Update(Particle particle)
		{
			double dx = _origin.X - particle.TransformationMatrix.M02, dy = _origin.Y - particle.TransformationMatrix.M12;
			double m = dx * dx + dy * dy;
			_vx += dx / m;
			_vy += dy / m;
			particle.TransformationMatrix.M02 += _vx;
			particle.TransformationMatrix.M12 += _vy;
		}
	}
}