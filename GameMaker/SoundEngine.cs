using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public abstract class SoundEngine
	{
		public static SoundEngine Current { get; internal set; }

		public abstract SoundSample LoadSample(string file);

		public abstract void Play(Sound sound);
	}
}
