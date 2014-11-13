using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GRaff.Synchronization;
using OpenTK;
using OpenTK.Audio.OpenAL;

namespace GRaff
{
	/// <summary>
	/// Represents a sound resource. This includes the image file, and metadata such as bitrate and number of channels.
	/// The sound itself does not have to be loaded into memory at the time this class is instantiated.
	/// <remarks>Supported file formats are OGG.</remarks>
	/// </summary>
	public sealed class Sound : IAsset
	{
		private SoundBuffer _buffer;
		private List<SoundInstance> _instances;

		/// <summary>
		/// Initializes a new instance of the GRaff.Sound class.
		/// </summary>
		/// <param name="filename">The sound file that is loaded from disk.</param>
		/// <param name="preload">If true, the sound is automatically loaded when the instance is initialized. The default value is true.</param>
		/// <exception cref="System.IO.FileNotFoundException">preload is true and the file is not found.</exception>
		public Sound(string filename, bool preload = true)
		{
			this.FileName = filename;
			this._instances = new List<SoundInstance>();
			if (preload)		
				Load();
		}

		public string FileName { get; private set; }

		public AssetState ResourceState { get; private set; }

		/// <summary>
		/// Gets the bitrate of this GRaff.Sound class. That is, the number of bits of data in one second of this sample.
		/// </summary>
		public int Bitrate
		{
			get
			{
				if (ResourceState != AssetState.Loaded) throw new InvalidOperationException("The texture is not loaded.");
				return _buffer.Bitrate;
			}
		}


		/// <summary>
		/// Gets the number of channels of this GRaff.Sound (e.g. 1=Mono, 2=Stereo, etc.).
		/// </summary>
		public int Channels
		{
			get
			{
				if (ResourceState != AssetState.Loaded) throw new InvalidOperationException("The texture is not loaded.");
				return _buffer.Channels;
			}
		}

		/// <summary>
			/// Gets the duration of this GRaff.Sound in milliseconds.
			/// </summary>
		public double Duration
		{
			get
			{
				if (ResourceState != AssetState.Loaded) throw new InvalidOperationException("The texture is not loaded.");
				return _buffer.Duration;
			}
		}

		/// <summary>
			/// Gets the sampling frequency of this GRaff.Sound.
			/// </summary>
		public int Frequency
		{
			get
			{
				if (ResourceState != AssetState.Loaded) throw new InvalidOperationException("The texture is not loaded.");
				return _buffer.Frequency;
			}
		}

		private void _load(SoundBuffer buffer)
		{
			_buffer = buffer;
			ResourceState = AssetState.Loaded;
		}

		/// <summary>
		/// Loads the sound.
		/// </summary>
		/// <exception cref="System.IO.FileNotFoundException">The sound file does not exists.</exception>
		public void Load()
		{
			lock (this)
			{
				if (ResourceState == AssetState.Loaded)
					return;
				ResourceState = AssetState.Loaded;
			}

			var soundBuffer = SoundBuffer.Load(FileName);
			_load(soundBuffer);
		}

		public async Task LoadAsync()
		{
			if (ResourceState != AssetState.NotLoaded)
				return;

			ResourceState = AssetState.LoadingAsync;

			var soundBuffer = await SoundBuffer.LoadAsync(FileName);

			if (ResourceState == AssetState.LoadingAsync)
				_load(soundBuffer);
		}

		public void Unload()
		{
			lock (this)
			{
				if (ResourceState == AssetState.NotLoaded)
					return;
				ResourceState = AssetState.NotLoaded;
			}

			StopAll();
			_instances.Clear();
			if (_buffer != null)
			{
				_buffer.Dispose();
				_buffer = null;
			}
		}

		/// <summary>
		/// Plays the sound.
		/// </summary>
		/// <param name="loop">Specifies whether the instance should loop. The default value is false.</param>
		/// <param name="volume">The volume of the instance. The default value is 1.0; doubling this corresponds to increasing the volume by 20 dB. The value should be greater than or equal to zero.</param>
		/// <param name="pitch">The pitch shift of the instance. The default value is 1.0; the playback speed is proportional to this value. The value should be greater than 0.</param>
		/// <returns>A GRaff.SoundInstance representing the sound being played.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">volume is less than zero, or pitch is less than or equal to zero.</exception>
		/// <exception cref="System.InvalidOperationException">The sound is not loaded.</exception>
		public SoundInstance Play(bool loop = false, double volume = 1.0, double pitch = 1.0)
		{
			if (ResourceState != AssetState.Loaded) throw new InvalidOperationException("The sound is not loaded.");
			if (volume < 0) throw new ArgumentOutOfRangeException("volume", "Must be greater than or equal to zero.");
			var instance = new SoundInstance(this, _buffer.Id, loop, volume, pitch);
			_instances.Add(instance);
			return instance;
		}

		public void StopAll()
		{
			foreach (var instance in _instances)
				instance.Stop();
		}
	}
}
