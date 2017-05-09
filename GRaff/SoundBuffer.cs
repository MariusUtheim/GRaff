using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using GRaff.Audio;
using GRaff.Synchronization;
using OpenTK.Audio.OpenAL;

namespace GRaff
{
	public sealed class SoundBuffer : IDisposable
	{
		private readonly List<SoundElement> _instances = new List<SoundElement>();
		private readonly ALFormat _format;

		private SoundBuffer(int bitrate, int channels, int frequency, byte[] buffer, Unit sentinel)
		{
			Contract.Requires<ArgumentOutOfRangeException>(bitrate == 8 || bitrate == 16);
			Contract.Requires<ArgumentOutOfRangeException>(channels == 1 || channels == 2);
			Contract.Requires<ArgumentOutOfRangeException>(frequency > 0);
			Contract.Requires<ArgumentNullException>(buffer != null);
			Contract.Requires<ArgumentException>(buffer.Length > 0);

			Id = AL.GenBuffer();

			this.Buffer = buffer;
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
		}

		public SoundBuffer(int bitrate, int channels, int frequency, byte[] buffer, double offset)
			: this(bitrate, channels, frequency, buffer, Unit._)
		{

			int bytesPerSample = bitrate * channels / 8;
			int offsetBytes = (int)(offset * frequency * bytesPerSample);
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
			
			ALError err;
			if ((err = AL.GetError()) != ALError.NoError)
				throw new Exception(String.Format("An error occurred: {0} (error code {1} {2})", AL.GetErrorString(err), (int)err, Enum.GetName(typeof(ALError), err)));
		}


		public SoundBuffer(int bitrate, int channels, int frequency, byte[] buffer)
			: this(bitrate, channels, frequency, buffer, Unit._)
		{
			AL.BufferData(Id, _format, buffer, buffer.Length, Frequency);
			ALError err;
			if ((err = AL.GetError()) != ALError.NoError)
				throw new Exception(String.Format("An error occurred: {0} (error code {1} {2})", AL.GetErrorString(err), (int)err, Enum.GetName(typeof(ALError), err)));
		}

		public SoundBuffer(SoundBuffer baseBuffer, double offset)
			: this(baseBuffer.Bitrate, baseBuffer.Channels, baseBuffer.Frequency, baseBuffer.Buffer, offset)
		{
			Contract.Requires<ArgumentNullException>(baseBuffer != null);
			Contract.Requires<ObjectDisposedException>(!baseBuffer.IsDisposed);
			Contract.Requires<ArgumentOutOfRangeException>(offset > 0);
		}

		[ContractInvariantMethod]
		private void invariants()
		{
			Contract.Invariant(Bitrate == 8 || Bitrate == 16);
			Contract.Invariant(Channels == 1 || Channels == 2);
			Contract.Invariant(Frequency > 0);
			Contract.Invariant(Duration > 0);
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
						AL.DeleteBuffer(ids.Main);
						if (ids.Intro.HasValue)
							AL.DeleteBuffer(ids.Intro.Value);
					}
					foreach (var instance in _instances.Where(i => !i.IsDisposed))
						instance.Dispose();
				});

				if (disposing)
				{
					Buffer = null;
					_instances.Clear();
				}

				IsDisposed = true;
			}
		}

		internal void Remove(SoundElement instance)
		{
			_instances.Remove(instance);
		}

		public int? IntroId { get; }

		public int Id { get; }

		public byte[] Buffer { get; private set; }

		public int Bitrate { get; }

		public int Channels { get; }

		public double Duration => Buffer.Length * 8.0 / (Bitrate * Channels * Frequency);

		public int Frequency { get; }

		public IReadOnlyCollection<SoundElement> SoundInstances => Array.AsReadOnly(_instances.ToArray());

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
			foreach (var instance in _instances)
				instance.Stop();
		}
	}
}
