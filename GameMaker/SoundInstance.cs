using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	public abstract class SoundInstance
	{
		public abstract bool Looping { get; set; }
		public abstract double Pitch { get; set; }
		public abstract double Volume { get; set; }
		public abstract void Play();
		public abstract void Stop();
		public abstract void Pause();
		public abstract SoundState State { get; }
	}
}
