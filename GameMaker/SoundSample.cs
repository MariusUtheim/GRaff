using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	public abstract class SoundSample
	{
		public abstract byte[] Buffer { get; }

		public abstract SoundInstance Play();

		public abstract int Frequency { get; }

		public abstract int Bitrate { get; }

		public abstract int Channels { get; }

		/// <summary>
		/// Gets the duration of this sample, in seconds
		/// </summary>
		public abstract double Duration { get; }


	}
}
