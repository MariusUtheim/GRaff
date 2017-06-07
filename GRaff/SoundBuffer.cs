using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using GRaff.Audio;
using GRaff.Synchronization;
using OpenTK.Audio.OpenAL;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace GRaff
{
	public sealed class SoundBuffer : IDisposable
	{
		private readonly MutableList<SoundElement> _instances = new MutableList<SoundElement>();
		private readonly ALFormat _format;

        #region Loading
        private SoundBuffer(int bitrate, int channels, int frequency, byte[] buffer, Unit sentinel)
		{
			Contract.Requires<ArgumentOutOfRangeException>(bitrate == 8 || bitrate == 16);
			Contract.Requires<ArgumentOutOfRangeException>(channels == 1 || channels == 2);
			Contract.Requires<ArgumentOutOfRangeException>(frequency > 0);
			Contract.Requires<ArgumentNullException>(buffer != null);
			Contract.Requires<ArgumentException>(buffer.Length > 0);

			Id = AL.GenBuffer();

			this._buffer = buffer;
			this.Bitrate = bitrate;
			this.Channels = channels;
			this.Frequency = frequency;

			switch (Channels + Bitrate)
			{
				case 1 + 8: _format = ALFormat.Mono8; break;
				case 1 + 16: _format = ALFormat.Mono16; break;
				case 2 + 8: _format = ALFormat.Stereo8; break;
				case 2 + 16: _format = ALFormat.Stereo16; break;
				default: throw new NotSupportedException($"Sound files must have exactly 1 or 2 channels, and a bitrate of exacty 8 or 16 bits per sample (you have {Channels} channel(s) and {Bitrate} bit(s) per sample).");
			}

            _Audio.ErrorCheck();
		}


		public SoundBuffer(int bitrate, int channels, int frequency, byte[] buffer, double offset)
			: this(bitrate, channels, frequency, buffer, Unit._)
		{

			var bytesPerSample = bitrate * channels / 8;
			var offsetBytes = (int)(offset * frequency * bytesPerSample);
			offsetBytes -= offsetBytes % bytesPerSample;

			IntroId = AL.GenBuffer();
            
			unsafe
			{
				fixed (byte *p = buffer)
				{
					AL.BufferData(IntroId.Value, _format, new IntPtr(p), offsetBytes, Frequency);
					AL.BufferData(Id, _format, new IntPtr(p + offsetBytes), buffer.Length - offsetBytes, Frequency);
				}
			}

            _Audio.ErrorCheck();
		}


		public SoundBuffer(int bitrate, int channels, int frequency, byte[] buffer)
			: this(bitrate, channels, frequency, buffer, Unit._)
		{
			AL.BufferData(Id, _format, buffer, buffer.Length, Frequency);
            _Audio.ErrorCheck();
        }


		public SoundBuffer(SoundBuffer baseBuffer, double offset)
			: this(baseBuffer.Bitrate, baseBuffer.Channels, baseBuffer.Frequency, baseBuffer._buffer, offset)
		{
			Contract.Requires<ArgumentNullException>(baseBuffer != null);
			Contract.Requires<ObjectDisposedException>(!baseBuffer.IsDisposed);
			Contract.Requires<ArgumentOutOfRangeException>(offset > 0);
		}

		

		public static SoundBuffer Load(string fileName)
		{
			var soundFile = SoundFileLoader.Load(fileName);
			return new SoundBuffer(soundFile.Bitrate, soundFile.Channels, soundFile.Frequency, soundFile.Buffer);
		}


		public static SoundBuffer LoadWithOffset(string fileName, double offset)
		{
			var soundFile = SoundFileLoader.Load(fileName);
			return new SoundBuffer(soundFile.Bitrate, soundFile.Channels, soundFile.Frequency, soundFile.Buffer, offset);
		}


		public static IAsyncOperation<SoundBuffer> LoadAsync(string fileName)
		{
			return Async.RunAsync(() => SoundFileLoader.LoadAsync(fileName))
						.ThenQueue(soundFile => new SoundBuffer(soundFile.Bitrate, soundFile.Channels, soundFile.Frequency, soundFile.Buffer));
		}


		public static IAsyncOperation<SoundBuffer> LoadWithOffsetAsync(string fileName, double offset)
		{
			return Async.RunAsync(() => SoundFileLoader.LoadAsync(fileName))
						.ThenQueue(soundFile => new SoundBuffer(soundFile.Bitrate, soundFile.Channels, soundFile.Frequency, soundFile.Buffer, offset));
		}

        #endregion

		public int Id { get; }

		public int? IntroId { get; }

        private byte[] _buffer;
        public IReadOnlyCollection<byte> Buffer => Array.AsReadOnly(_buffer);

		public int Bitrate { get; }

		public int Channels { get; }

		public double Duration => _buffer.Length * 8.0 / (double)(Bitrate * Channels * Frequency);

		public int Frequency { get; }

		public IReadOnlyCollection<SoundElement> SoundInstances => Array.AsReadOnly(_instances.ToArray());


        public bool IsDisposed { get; private set; }


        ~SoundBuffer()
        {
            Dispose(false);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        private void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                Async.Run(new { Main = this.Id, Intro = this.IntroId }, ids =>
                {
                    if (Giraffe.IsRunning)
                    {
                        _Audio.ClearError();
                        foreach (var instance in _instances)
                            instance.Destroy();
                        //var instances = _instances.Where(i => !i._isDisposed).ToArray();
                        //if (instances.Count() != _instances.Count)
                        //{ }
                        //for (var i = 0; i < instances.Length; i++)
                        //    instances[i].Destroy();

                        _Audio.ClearError();
                        
                        AL.DeleteBuffer(ids.Main);
#warning
                        //if (ids.Intro.HasValue)
                        //	AL.DeleteBuffer(ids.Intro.Value);
                        try { _Audio.ErrorCheck(); }
                        catch
                        {
                            throw;
                        }


                        if (disposing)
                            _buffer = null;
                    }
                });

                IsDisposed = true;
            }
        }

        internal void Remove(SoundElement instance)
        {
            _instances.Remove(instance);
        }



        public SoundElement Play(bool looping, double volume = 1.0, double pitch = 1.0)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			Contract.Requires<ArgumentOutOfRangeException>(volume >= 0 && pitch > 0);
			Contract.Ensures(Contract.Result<SoundElement>() != null);
			
			var instance = Instance.Create(new SoundElement(this, IntroId, Id, looping, volume, pitch));
			_instances.Add(instance);
			return instance;
		}

		public void StopAll()
		{
            throw new NotImplementedException();
			//foreach (var instance in _instances)
			//	instance.Stop();
		}





        [ContractInvariantMethod]
        private void invariants()
        {
            Contract.Invariant(Bitrate == 8 || Bitrate == 16);
            Contract.Invariant(Channels == 1 || Channels == 2);
            Contract.Invariant(Frequency > 0);
            Contract.Invariant(Duration > 0);
        }
    }
}
