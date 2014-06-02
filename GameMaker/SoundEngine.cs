using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public abstract class SoundEngine
	{
		public SoundEngine()
		{
			SoundEngine.Current = this;
		}

		public static SoundEngine Current { get; internal set; }

		public abstract SoundSample LoadSample(string path);

		public abstract SoundInstance Play(SoundSample sound, bool loop, double volume, double pitch);
	}
}
