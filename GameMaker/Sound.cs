using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameMaker.IO;
using OpenTK.Audio.OpenAL;

namespace GameMaker
{
	/// <summary>
	/// Represents a sound resource. This includes the image file, and metadata such as bitrate and number of channels.
	/// The sound itself does not have to be loaded into memory at the time this class is instantiated.
	/// <remarks>Supported file formats are OGG.</remarks>
	/// </summary>
	public class Sound
	{
		private string _filename;
		private bool _isLoaded;
		private List<SoundInstance> _instances;
		private int _bufferId;

		private static IntPtr _device = Alc.OpenDevice("");
		private static OpenTK.ContextHandle context = Alc.CreateContext(_device, new int[0]);

		static Sound()
		{
			Alc.MakeContextCurrent(context);
		}

		/// <summary>
		/// Initializes a new instance of the GameMaker.Sound class.
		/// </summary>
		/// <param name="filename">The sound file that is loaded from disk.</param>
		/// <param name="preload">If true, the sound is automatically loaded when the instance is initialized. The default value is true.</param>
		/// <exception cref="System.IO.FileNotFoundException">preload is true and the file is not found.</exception>
		public Sound(string filename, bool preload = true)
		{
			this._filename = filename;
			this._instances = new List<SoundInstance>();
			this._bufferId = AL.GenBuffer();
			if (preload)		
				Load();
		}

		/// <summary>
		/// Gets the bitrate of this GameMaker.Sound class. That is, the number of bits of data in one second of this sample.
		/// </summary>
		public int Bitrate
		{
			get;
			private set;
		}

		private byte[] buffer;

		/// <summary>
		/// Gets the data buffer of this GameMaker.Sound.
		/// </summary>
		public IEnumerable<byte> Buffer => buffer;

		/// <summary>
		/// Gets the number of channels of this GameMaker.Sound (e.g. 1=Mono, 2=Stereo, etc.).
		/// </summary>
		public int Channels
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the duration of this GameMaker.Sound in milliseconds.
		/// </summary>
		public double Duration
		{
			get { throw new NotImplementedException(); }
			private set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets the sampling frequency of this GameMaker.Sound.
		/// </summary>
		public int Frequency
		{
			get;
			private set;
		}

		/// <summary>
		/// Loads the sound.
		/// </summary>
		/// <exception cref="System.IO.FileNotFoundException">The sound file does not exists.</exception>
		public void Load()
		{
			if (_isLoaded)
				return;

			var file = new OggFile(_filename);
			this.buffer = file.Buffer;
			this.Bitrate = file.Bitrate;
			this.Channels = file.Channels;
#warning TODO: Set Duration
			this.Frequency = file.Frequency;

			AL.BufferData(_bufferId, ALFormat.Mono16, buffer, buffer.Length, Frequency);
			_isLoaded = true;
		}

		/// <summary>
		/// Plays the sound.
		/// </summary>
		/// <param name="loop">Specifies whether the instance should loop. The default value is false.</param>
		/// <param name="volume">The volume of the instance. The default value is 1.0; doubling this corresponds to increasing the volume by 20 dB. The value should be greater than or equal to zero.</param>
		/// <param name="pitch">The pitch shift of the instance. The default value is 1.0; the playback speed is proportional to this value. The value should be greater than 0.</param>
		/// <returns>A GameMaker.SoundInstance representing the sound being played.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">volume is less than zero, or pitch is less than or equal to zero.</exception>
		/// <exception cref="System.InvalidOperationException">The sound is not loaded.</exception>
		public SoundInstance Play(bool loop = false, double volume = 1.0, double pitch = 1.0)
		{
			if (!_isLoaded) throw new InvalidOperationException("The sound is not loaded.");
			if (volume < 0) throw new ArgumentOutOfRangeException("volume", "Must be greater than or equal to zero.");
			return new SoundInstance(this, _bufferId, loop, volume, pitch);
		}
	}
}
