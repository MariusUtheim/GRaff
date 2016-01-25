using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
		private readonly List<SoundInstance> _instances = new List<SoundInstance>();
		private IAsyncOperation _loadingOperation;

		/// <summary>
		/// Initializes a new instance of the GRaff.Sound class.
		/// </summary>
		/// <param name="fileName">The sound file that is loaded from disk.</param>
		/// <param name="preload">If true, the sound is automatically loaded when the instance is initialized. The default value is true.</param>
		/// <exception cref="System.IO.FileNotFoundException">preload is true and the file is not found.</exception>
		public Sound(string fileName, bool isLooping, double loopOffset = 0)
		{
			this.FileName = fileName;
			this.IsLooping = isLooping;
			this.LoopOffset = loopOffset;
		}

		public static Sound Load(string fileName, bool isLooping, double loopOffset = 0)
		{
			var sound = new Sound(fileName, isLooping, loopOffset);
			sound.Load();
			return sound;
		}

		public static IAsyncOperation<Sound> LoadAsync(string fileName, bool isLooping, double loopOffset = 0)
		{
			var sound = new Sound(fileName, isLooping, loopOffset);
			return sound.LoadAsync().ThenWait(() => sound);
		}

		public string FileName { get; private set; }

		public bool IsLoaded { get; private set; }

		public bool IsLooping { get; private set; }

		public double LoopOffset { get; private set; }

		/// <summary>
		/// Gets the bitrate of this GRaff.Sound class. That is, the number of bits of data in one second of this sample.
		/// </summary>
		public int Bitrate
		{
			get
			{
				Contract.Requires<InvalidOperationException>(IsLoaded);
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
				Contract.Requires<InvalidOperationException>(IsLoaded);
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
				Contract.Requires<InvalidOperationException>(IsLoaded);
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
				Contract.Requires<InvalidOperationException>(IsLoaded);
				return _buffer.Frequency;
			}
		}

		private void _load(SoundBuffer buffer)
		{
			_buffer = buffer;
			IsLoaded = true;
		}

		public IAsyncOperation LoadAsync()
		{
			if (!IsLoaded)
				return _loadingOperation;

			return _loadingOperation = SoundBuffer.LoadAsync(FileName, LoopOffset).Then(_load);
		}

		public void Unload()
		{
			if (_loadingOperation != null)
				_loadingOperation.Abort();
			if (!IsLoaded)
				return;

			StopAll();
			_instances.Clear();
			IsLoaded = false;

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
		public SoundInstance Play(double volume = 1.0, double pitch = 1.0)
		{
			if (!IsLoaded) throw new InvalidOperationException("The sound is not loaded.");
			if (volume < 0) throw new ArgumentOutOfRangeException("volume", "Must be greater than or equal to zero.");

			var instance = Instance.Create(new SoundInstance(this, _buffer.IntroId, _buffer.MainId, IsLooping, volume, pitch));
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
