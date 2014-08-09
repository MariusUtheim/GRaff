using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public class Sound
	{
		private string _filename;
		private bool _isLoaded;
		private List<SoundInstance> _instances;
		private SoundSample _sample;

		public Sound(string filename)
		{
			this._filename = filename;
			this._isLoaded = false;
			this._instances = new List<SoundInstance>();
		}

		public byte[] Buffer { get { Load(); return _sample.Buffer; } }

		public int Frequency { get { Load(); return _sample.Frequency; } }

		public int Bitrate { get { Load(); return _sample.Bitrate; } }

		public int Channels { get { Load(); return _sample.Channels; } }

		public double Duration { get { Load(); return _sample.Duration; } }

		public void Load()
		{
			if (_isLoaded)
				return;
			_sample = SoundEngine.Current.LoadSample(_filename);
		}

		public SoundInstance Play(bool loop = false, double volume = 1.0, double pitch = 1.0)
		{
			return SoundEngine.Current.Play(_sample, loop, volume, pitch);
		}
	}
}
