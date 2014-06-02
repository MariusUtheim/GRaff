using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameMaker.IO;
using OpenTK.Audio.OpenAL;

namespace GameMaker.OpenAL
{
	class OpenALSoundSample : SoundSample
	{
		private byte[] _buffer;


		public OpenALSoundSample(string path)
		{
			SoundFile file = SoundFile.OpenFile(path);
			this.Id = AL.GenBuffer();
			AL.BufferData<byte>(Id, (file.Bitrate == 8 ? ALFormat.Mono8 : ALFormat.Mono16)
									| (file.Channels == 1 ? ALFormat.Mono8 : ALFormat.Stereo8), file.Buffer, (int)file.Buffer.Length, (int)file.Frequency);
			this._buffer = file.Buffer;
		}

		internal int Id { get; set; }

		public override byte[] Buffer
		{
			get { return _buffer; }
		}

		public override SoundInstance Play()
		{
			OpenALSoundInstance instance = new OpenALSoundInstance(Id);
			instance.Play();
			return instance;
		}


		public override int Frequency
		{
			get
			{
				int value;
				AL.GetBuffer(Id, ALGetBufferi.Frequency, out value);
				return value;
			}
		}

		public override int Bitrate
		{
			get
			{
				int value;
				AL.GetBuffer(Id, ALGetBufferi.Bits, out value);
				return value;
			}
		}

		public override int Channels
		{
			get 
			{
				int value;
				AL.GetBuffer(Id, ALGetBufferi.Channels, out value);
				return value;
			}
		}

		public override double Duration
		{
			get
			{
				int sz;
				AL.GetBuffer(Id, ALGetBufferi.Size, out sz);
				return (double)_buffer.Length / (Frequency * Bitrate * Channels / 8);
			}
		}
	}
}
