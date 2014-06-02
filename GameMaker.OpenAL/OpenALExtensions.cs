using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Audio.OpenAL;

namespace GameMaker.OpenAL
{
	public static class OpenALExtensions
	{
		public static ALFormat ALFormat(this Sound sound)
		{
			if (sound.Channels == 1)
			{
				if (sound.Bitrate == 8)
					return OpenTK.Audio.OpenAL.ALFormat.Mono8;
				else if (sound.Bitrate == 16)
					return OpenTK.Audio.OpenAL.ALFormat.Mono16;
			}
			else if (sound.Channels == 2)
			{
				if (sound.Bitrate == 8)
					return OpenTK.Audio.OpenAL.ALFormat.Stereo8;
				else if (sound.Bitrate == 16)
					return OpenTK.Audio.OpenAL.ALFormat.Stereo16;
			}

			throw new FormatException(String.Format("Format is not supported by OpenAL (Channels={0}, Bitrate={1})", sound.Channels, sound.Bitrate));
		}
	}
}
