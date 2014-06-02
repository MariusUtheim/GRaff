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

		public static SoundFile OpenFile(string path)
		{
			if (path.EndsWith(".wav"))
				return new WaveFile(path);
			if (path.EndsWith(".ogg"))
				return new OggFile(path);

			throw new FormatException("Unrecognized format: " + path.Substring(path.LastIndexOf('.')));
		}
	}
}
