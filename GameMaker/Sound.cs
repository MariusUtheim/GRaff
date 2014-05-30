using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public class Sound
	{
		private string _path;
		private bool _isLoaded;

		public Sound(string path)
		{
			this._path = path;
			this._isLoaded = false;
		}

		public SoundSample Sample { get; private set; }

		public void Load()
		{
			if (_isLoaded)
				return;

			Sample = SoundEngine.Current.LoadSample(_path);
		}

		public void Play()
		{

		}
	}
}
