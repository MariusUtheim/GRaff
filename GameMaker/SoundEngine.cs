using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public abstract class SoundEngine
	{
		public abstract SoundSample LoadSoundSample(string file);
	}
}
