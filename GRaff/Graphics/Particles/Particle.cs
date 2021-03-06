﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Graphics.Particles
{
	public sealed class Particle
	{
		internal readonly List<IParticleBehavior> Behaviors = new List<IParticleBehavior>();

		public Particle(double x, double y, int lifetime)
		{
			Location = new Point(x, y);
			TransformationMatrix = new Matrix();
			Blend = Colors.White;
			TotalLifetime = lifetime;
		}

		public Point Location { get; set; }

		public Vector Velocity { get; set; }

		public Matrix TransformationMatrix { get; set; }

		public Color Blend { get; set; }

		public int TotalLifetime { get; set; }

		public int Lifetime { get; set; }


		internal bool Update()
		{
			if (++Lifetime >= TotalLifetime)
				return false;

			Location += Velocity;
			foreach (var behavior in Behaviors)
				behavior.Update(this);

			return true;
		}

		public void AttachBehavior(IParticleBehavior behavior)
		{
			Behaviors.Add(behavior);
            behavior.Initialize(this);
		}
	}
}
