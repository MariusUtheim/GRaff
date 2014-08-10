using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameMaker.IO;
using OpenTK.Audio.OpenAL;

namespace GameMaker
{
	public class Sound
	{
		private string _filename;
		private bool _isLoaded;
		private List<SoundInstance> _instances;
		private int _bufferId;

		private static IntPtr device;
		private static OpenTK.ContextHandle context;

		static Sound()
		{
			device = Alc.OpenDevice("");
			context = Alc.CreateContext(device, new int[0]);
			Alc.MakeContextCurrent(context);
		}

		public Sound(string filename)
		{
			this._filename = filename;
			this._isLoaded = false;
			this._instances = new List<SoundInstance>();
			this._bufferId = AL.GenBuffer();
		}

		public int Bitrate
		{
			get;
			private set;
		}

		public byte[] Buffer
		{
			get;
			private set;
		}

		public int Channels
		{
			get;
			private set;
		}

		public double Duration
		{
			get;
			private set;
		}

		public int Frequency
		{
			get;
			private set;
		}

		public void Load()
		{
			if (_isLoaded)
				return;

			var file = new WaveFile(_filename);
			this.Buffer = file.Buffer;
			this.Bitrate = file.Bitrate;
			this.Channels = file.Channels;
#warning TODO: Set Duration
			this.Frequency = file.Frequency;

			AL.BufferData(_bufferId, ALFormat.Mono16, Buffer, Buffer.Length, Frequency);
		}

		public SoundInstance Play(bool loop = false, double volume = 1.0, double pitch = 1.0)
		{
			return new SoundInstance(_bufferId, loop, volume, pitch);
		}
	}
}
