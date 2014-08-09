using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker.IO
{
	public abstract class SoundFile
	{
		public abstract int Frequency { get; }
		public abstract int Channels { get; }
		public abstract byte[] Buffer { get; }
		public abstract int Bitrate { get; }

		public static SoundFile OpenFile(string filename)
		{
			if (filename.EndsWith(".wav"))
				return new WaveFile(filename);
			if (filename.EndsWith(".ogg"))
				return new OggFile(filename);

			throw new FormatException("Unrecognized format: " + filename.Substring(filename.LastIndexOf('.')));
		}
	}
}
