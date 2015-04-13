using System;
using OpenTK.Audio.OpenAL;


namespace GRaff
{
	/// <summary>
	/// Represents an instance of a sound that is currently playing.
	/// </summary>
	#warning Review class
	public sealed class SoundInstance : GameElement
	{
		private int _sid;
		private bool _isDisposed = false;
		private bool _shouldDropIntro;

		/// <summary>
		/// Initializes a new instance of the GRaff.SoundInstance class with the specified sound reference, buffer id, looping, volume and pitch.
		/// </summary>
		/// <param name="sound">The GRaff.Sound that this GRaff.SoundInstance is an instance of.</param>
		/// <param name="bid">The buffer id of the data.</param>
		/// <param name="looping">Specifies whether the instance should loop.</param>
		/// <param name="volume">Specifies the volume of the sound instance.</param>
		/// <param name="pitch">Specifies the pitch shift of the sound instance.</param>
		internal SoundInstance(Sound sound, int introBufferId, int mainBufferId, bool looping, double volume, double pitch)
		{
			this.Sound = sound;

			AL.GenSources(1, out _sid);

		//	this.Looping = looping;
			this.Volume = volume;
			this.Pitch = pitch;


			if (looping)
			{
				AL.SourceQueueBuffer(_sid, introBufferId);
				this.Play();
				AL.SourceQueueBuffer(_sid, mainBufferId);
				_shouldDropIntro = true;
			}
			else
			{
				AL.Source(_sid, ALSourcei.Buffer, mainBufferId);
				_shouldDropIntro = false;
				this.Play();
			}
		}


		/// <summary>
		/// Gets the GRaff.Sound that this GRaff.SoundInstance is an instance of.
		/// </summary>
		public Sound Sound
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets whether this GRaff.SoundInstance is looping.
		/// </summary>
		public bool Looping
		{
			get
			{
				bool value;
				AL.GetSource(_sid, ALSourceb.Looping, out value);
				return value;
			}
			set
			{
				AL.Source(_sid, ALSourceb.Looping, value);
			}
		}

		/// <summary>
		/// Gets or sets the pitch of this GRaff.SoundInstance. The value should be greater than 0.
		/// </summary>
		public double Pitch
		{
			get
			{
				if (_isDisposed) throw new ObjectDisposedException("GRaff.SoundInstance");
				float value;
				AL.GetSource(_sid, ALSourcef.Pitch, out value);
				return value;
			}

			set
			{
				if (_isDisposed) throw new ObjectDisposedException("GRaff.SoundInstance");
				if (value <= 0)
					throw new ArgumentOutOfRangeException("value", "Must be greater than 0");
				AL.Source(_sid, ALSourcef.Pitch, (float)value);
			}
		}

		/// <summary>
		/// Gets or sets the volume of this GRaff.SoundInstance. The value should be greater than or equal to 0.
		/// </summary>
		public double Volume
		{
			get
			{
				float value;
				AL.GetSource(_sid, ALSourcef.Gain, out value);
				return value;
			}
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException("value", "Must be greater than or equal to 0");
				AL.Source(_sid, ALSourcef.Gain, (float)value);
			}
		}

		/// <summary>
		/// Plays this GRaff.SoundInstance. This is called automatically when the instance is created; in other circumstances,
		/// it should only be called after the instance is paused with GRaff.SoundInstance.Pause().
		/// </summary>
		public void Play()
		{
			AL.SourcePlay(_sid);
		}

		/// <summary>
		/// Stops this GRaff.SoundInstance. It cannot be restarted.
		/// </summary>
		public void Stop()
		{
			AL.SourceStop(_sid);
			_isDisposed = true;
			Destroy();
		}

		/// <summary>
		/// Pauses this GRaff.SoundInstance. It can later be resumed by calling GRaff.SoundInstance.Pause().
		/// </summary>
		public void Pause()
		{
			AL.SourcePause(_sid);
		}

		/// <summary>
		/// Gets the state of this GRaff.SoundInstance.
		/// </summary>
		public SoundState State
		{
			get 
			{
				ALSourceState state;
				switch (state = AL.GetSourceState(_sid))
				{
					case ALSourceState.Initial:
					case ALSourceState.Stopped:
						return SoundState.Stopped;

					case ALSourceState.Paused:
						return SoundState.Paused;

					case ALSourceState.Playing:
						return SoundState.Playing;

					default:
						ALError err;
						if ((err = AL.GetError()) != ALError.NoError)
							throw new InvalidOperationException(String.Format("An AL error occurred: {0} ({1})", AL.GetErrorString(err), err));
                        throw new NotSupportedException(String.Format("GRaff.SoundInstance.SoundState encountered an unknown state '{0}' (code: {1}).", Enum.GetName(typeof(ALSourceState), state), (int)state));
				}
			}
		}

		public override void OnDestroy()
		{
			AL.DeleteSources(1, ref _sid);
		}

		public override void OnStep()
		{
			if (State == SoundState.Stopped)
				this.Destroy();
			else if (State == SoundState.Playing && _shouldDropIntro)
			{
				int buffersProcessed;
				AL.GetSource(_sid, ALGetSourcei.BuffersProcessed, out buffersProcessed);
				if (buffersProcessed > 0)
				{
					Console.WriteLine("[SoundInstance] Unqueueing buffer");
					AL.SourceUnqueueBuffers(_sid, 1);
					_shouldDropIntro = false;
					Looping = true;
				}
			}
		}
	}
}
