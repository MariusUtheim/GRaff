﻿using System;
using System.Diagnostics.Contracts;
using GRaff.Synchronization;
using OpenTK.Audio.OpenAL;


namespace GRaff
{
	/// <summary>
	/// Represents an instance of a sound that is currently playing.
	/// </summary>
	#warning Review class
	public class SoundInstance : GameElement, IDisposable
	{
		private int _sid;
		private bool _shouldDropIntro;

		internal SoundInstance(SoundBuffer buffer, int? introBufferId, int mainBufferId, bool looping, double volume, double pitch)
		{
			this.Buffer = buffer;

			_sid = AL.GenSource();
			Console.WriteLine($"Source created: {_sid}");
            this.Looping = looping;
			this.Volume = volume;
			this.Pitch = pitch;


			if (looping)
			{
				if (introBufferId != null)
				{
					AL.SourceQueueBuffer(_sid, mainBufferId);
					this.Play();
				}
				else
				{
					AL.SourceQueueBuffer(_sid, introBufferId.Value);
					this.Play();
					AL.SourceQueueBuffer(_sid, mainBufferId);
					_shouldDropIntro = true;
				}
			}
			else
			{
				AL.Source(_sid, ALSourcei.Buffer, mainBufferId);
				_shouldDropIntro = false;
				this.Play();
			}
		}

		~SoundInstance()
		{
			Dispose(false);
		}

		public bool IsDisposed { get; private set; }

		public void Dispose()
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!IsDisposed)
			{
				Async.Run(() =>
				{
					if (Giraffe.IsRunning)
						AL.DeleteSource(_sid);
					Console.WriteLine("Source deleted: " + _sid.ToString());
				});

				IsDisposed = true;
				Destroy();
				Buffer.Remove(this);
			}
		}

		
		public SoundBuffer Buffer { get; }

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
				Contract.Requires<ObjectDisposedException>(!IsDisposed);
				float value;
				AL.GetSource(_sid, ALSourcef.Pitch, out value);
				return value;
			}

			set
			{
				Contract.Requires<ObjectDisposedException>(!IsDisposed);
				Contract.Requires<ArgumentOutOfRangeException>(value > 0);
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
				Contract.Requires<ArgumentOutOfRangeException>(value >= 0);
				AL.Source(_sid, ALSourcef.Gain, (float)value);
			}
		}

		/// <summary>
		/// Plays this GRaff.SoundInstance. This is called automatically when the instance is created; in other circumstances,
		/// it should only be called after the instance is paused with GRaff.SoundInstance.Pause().
		/// </summary>
		public void Play()
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			AL.SourcePlay(_sid);
		}

		/// <summary>
		/// Stops this GRaff.SoundInstance. It cannot be restarted.
		/// </summary>
		public void Stop()
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			AL.SourceStop(_sid);
			Destroy();
		}

		/// <summary>
		/// Pauses this GRaff.SoundInstance. It can later be resumed by calling GRaff.SoundInstance.Pause().
		/// </summary>
		public void Pause()
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			AL.SourcePause(_sid);
		}

		/// <summary>
		/// Gets the state of this GRaff.SoundInstance.
		/// </summary>
		public SoundState State
		{
			get 
			{
				Contract.Requires<ObjectDisposedException>(!IsDisposed);
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
			if (!IsDisposed)
				Dispose();
		}

		public override void OnStep()
		{
			if (IsDisposed || State == SoundState.Stopped)
			{
				this.Destroy();
			}
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
