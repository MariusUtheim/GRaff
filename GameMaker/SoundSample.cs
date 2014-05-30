using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	public abstract class SoundSample
	{
		public abstract byte[] Buffer { get; }
		public int Frequency { get; set; }
		public int Bitrate { get; set; }
	}
}
