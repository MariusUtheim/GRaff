using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Audio.OpenAL;

namespace GameMaker
{
	/// <summary>
	/// Represents an instance of a sound that is currently playing.
	/// </summary>
	public class SoundInstance
	{
		private int _sid;

		/// <summary>
		/// Initializes a new instance of the GameMaker.SoundInstance class with the specified sound reference, buffer id, looping, volume and pitch.
		/// </summary>
		/// <param name="sound">The GameMaker.Sound that this GameMaker.SoundInstance is an instance of.</param>
		/// <param name="bid">The buffer id of the data.</param>
		/// <param name="looping">Specifies whether the instance should loop.</param>
		/// <param name="volume">Specifies the volume of the sound instance.</param>
		/// <param name="pitch">Specifies the pitch shift of the sound instance.</param>
#warning CA1801
		internal SoundInstance(Sound sound, int bid, bool looping, double volume, double pitch)
		{
			this.Sound = sound;
			this._sid = AL.GenSource();
			AL.Source(_sid, ALSourcei.Buffer, bid);

			this.Looping = looping;
			this.Volume = volume;
			this.Pitch = pitch;
			this.Play();
		}

		/// <summary>
		/// Gets the GameMaker.Sound that this GameMaker.SoundInstance is an instance of.
		/// </summary>
		public Sound Sound
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets whether this GameMaker.SoundInstance is looping.
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
		/// Gets or sets the pitch of this GameMaker.SoundInstance. The value should be greater than 0.
		/// </summary>
		public double Pitch
		{
			get
			{
				float value;
				AL.GetSource(_sid, ALSourcef.Pitch, out value);
				return value;
			}

			set
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException("value", "Must be greater than 0");
				AL.Source(_sid, ALSourcef.Pitch, (float)value);
			}
		}

		/// <summary>
		/// Gets or sets the volume of this GameMaker.SoundInstance. The value should be greater than or equal to 0.
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
		/// Plays this GameMaker.SoundInstance. This is called automatically when the instance is created; in other circumstances,
		/// it should only be called after the instance is paused with GameMaker.SoundInstance.Pause().
		/// </summary>
		public void Play()
		{
			AL.SourcePlay(_sid);
		}

		/// <summary>
		/// Stops this GameMaker.SoundInstance. It can be restarted by calling GameMaker.SoundInstance.Play().
		/// </summary>
#warning TODO: Check what happens if calling Play() after Stop(). Also, figure the difference between Stop and Pause.
		public void Stop()
		{
			AL.SourceStop(_sid);
		}

		/// <summary>
		/// Pauses this GameMaker.SoundInstance. It can later be resumed by calling GameMaker.SoundInstance.Pause().
		/// </summary>
		public void Pause()
		{
			AL.SourcePause(_sid);
		}

		/// <summary>
		/// Gets the state of this GameMaker.SoundInstance.
		/// </summary>
		public SoundState State
		{
			get 
			{
				switch (AL.GetSourceState(_sid))
				{
					case ALSourceState.Initial:
					case ALSourceState.Stopped:
						return SoundState.Stopped;

					case ALSourceState.Paused:
						return SoundState.Paused;

					case ALSourceState.Playing:
						return SoundState.Playing;

					default:
						return 0; // This will never hit
				}
			}
		}
	}
}
