using System;
using System.Diagnostics.Contracts;
using GRaff.Synchronization;
using OpenTK.Audio.OpenAL;
using GRaff.Audio;

namespace GRaff
{
    /// <summary>
    /// Represents an instance of a sound that is currently playing.
    /// </summary>
#warning Review class
#warning How about positioning?;
    public class SoundElement : GameElement
	{
		private int _sid;
		private bool _shouldDropIntro;
        internal bool _isDisposed = false;

		internal SoundElement(SoundBuffer buffer, int? introBufferId, int mainBufferId, bool looping, double volume, double pitch)
		{
			this.Buffer = buffer;

			_sid = AL.GenSource();

			this.Looping = looping;
			this.Volume = volume;
			this.Pitch = pitch;

			if (looping)
			{
				if (introBufferId == null)
				{
					AL.SourceQueueBuffer(_sid, mainBufferId);
					AL.SourcePlay(_sid);
				}
				else
				{
					AL.SourceQueueBuffer(_sid, introBufferId.Value);
					AL.SourcePlay(_sid);
					AL.SourceQueueBuffer(_sid, mainBufferId);
					_shouldDropIntro = true;
				}
			}
			else
			{
				AL.Source(_sid, ALSourcei.Buffer, mainBufferId);
				_shouldDropIntro = false;
				AL.SourcePlay(_sid);
			}

            Console.WriteLine($"[{nameof(SoundElement)}] Created {_sid}");
		}


		public bool IsStopped => _isDisposed || State == SoundState.Stopped;
		
		public SoundBuffer Buffer { get; }

		public bool Looping
		{
			get
			{
                AL.GetSource(_sid, ALSourceb.Looping, out bool value);
                return value;
			}
			private set
			{
				AL.Source(_sid, ALSourceb.Looping, value);
			}
		}

        public double Position
        {
            get
            {
                //Contract.Requires<ObjectDisposedException>(!_isDisposed);
                if (_isDisposed)
                    return 1;
                AL.GetSource(_sid, ALSourcef.SecOffset, out float value);
                return value;
            }
        }

        public double Completion => Position / Buffer.Duration;

		/// <summary>
		/// Gets or sets the pitch of this GRaff.SoundInstance. The value should be greater than 0.
		/// </summary>
		public double Pitch
		{
			get
			{
				Contract.Requires<ObjectDisposedException>(!_isDisposed);
				Contract.Ensures(Contract.Result<double>() > 0);
#warning Documentation says range is [0.5, 2.0]?
                AL.GetSource(_sid, ALSourcef.Pitch, out float value);
                return value;
			}

			set
			{
				Contract.Requires<ObjectDisposedException>(!_isDisposed);
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
                Contract.Requires<ObjectDisposedException>(!_isDisposed);
				Contract.Ensures(Contract.Result<double>() >= 0);
                AL.GetSource(_sid, ALSourcef.Gain, out float value);
                return value;
			}
			set
			{
                Contract.Requires<ObjectDisposedException>(!_isDisposed);
                Contract.Requires<ArgumentOutOfRangeException>(value >= 0);
				AL.Source(_sid, ALSourcef.Gain, (float)value);
			}
		}

		/// <summary>
		/// Resumes playing this GRaff.SoundElement if it is paused; if it is not paused, this function does nothing.
		/// </summary>
		public void Resume()
		{
            if (State == SoundState.Paused)
            {
                AL.SourcePlay(_sid);
                _Audio.ErrorCheck();
            }
		}

		/// <summary>
		/// Pauses this GRaff.SoundInstance. It can later be resumed by calling GRaff.SoundInstance.Pause().
		/// </summary>
		public void Pause()
		{
			Contract.Requires<ObjectDisposedException>(IsStopped || !_isDisposed);
			AL.SourcePause(_sid);
            _Audio.ErrorCheck();
		}

		/// <summary>
		/// Gets the state of this GRaff.SoundInstance.
		/// </summary>
		public SoundState State
		{
			get 
			{
				if (_isDisposed)
					return SoundState.Stopped;
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
                        _Audio.ErrorCheck();
                        throw new NotSupportedException($"GRaff.SoundInstance.SoundState encountered an unknown state '{Enum.GetName(typeof(ALSourceState), state)}' (code: {(int)state}).");
				}
			}
		}

		public override void OnStep()
		{
			if (IsStopped)
			{
                Console.WriteLine("[SoundElement] Stopped by itself");
				this.Destroy();
			}
			//else if (_shouldDropIntro && State == SoundState.Playing)
			//{
            //    AL.GetSource(_sid, ALGetSourcei.BuffersProcessed, out int buffersProcessed);
            //    if (buffersProcessed > 0)
			//	{
			//		Console.WriteLine("[SoundElement] Unqueueing buffer");
			//		AL.SourceUnqueueBuffers(_sid, 1);
			//		_shouldDropIntro = false;
			//		Looping = true;
			//	}
			//}
		}

		public override void OnDestroy()
		{
            Console.WriteLine("[SoundElement] Destroyed " + _sid.ToString());
            if (!_isDisposed)
            {
                _isDisposed = true;

                AL.SourceStop(_sid);
                _Audio.ErrorCheck();


                if (Giraffe.IsRunning)
                {
                    AL.Source(_sid, ALSourcei.Buffer, 0);
                    AL.DeleteSource(_sid);
                    _Audio.ErrorCheck();
                }
                Console.WriteLine("[SourceElement] Source deleted: " + _sid.ToString());
                
                Buffer.Remove(this);

                _Audio.ErrorCheck();
            }
            _Audio.ClearError();
        }
	}
}
