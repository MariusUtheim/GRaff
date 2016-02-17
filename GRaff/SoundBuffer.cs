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
		private readonly List<SoundInstance> _instances = new List<SoundInstance>();

		public SoundBuffer(int bitrate, int channels, int frequency, byte[] buffer, double offset)
		{
			Contract.Requires<ArgumentOutOfRangeException>(bitrate == 8 || bitrate == 16);
			Contract.Requires<ArgumentOutOfRangeException>(channels == 1 || channels == 2);
			Contract.Requires<ArgumentOutOfRangeException>(frequency > 0);
			Contract.Requires<ArgumentNullException>(buffer != null);
			Contract.Requires<ArgumentException>(buffer.Length > 0);

			IntroId = AL.GenBuffer();
			MainId = AL.GenBuffer();

			Buffer = buffer;
			this.Bitrate = bitrate;
			this.Channels = channels;
			this.Frequency = frequency;

			ALFormat format;
			switch (Channels + Bitrate)
			{
				case 1 + 8: format = ALFormat.Mono8; break;
				case 1 + 16: format = ALFormat.Mono16; break;
				case 2 + 8: format = ALFormat.Stereo8; break;
				case 2 + 16: format = ALFormat.Stereo16; break;
				default: throw new NotSupportedException($"Sound files must have exactly 1 or 2 channels, and a bitrate of exacty 8 or 16 bits per sample (you have {Channels} channel(s) and {Bitrate} bit(s) per sample).");
			}

			int bytesPerSample = bitrate / 8 * channels;
			int offsetBytes = (int)(offset * frequency * bytesPerSample);
			offsetBytes -= offsetBytes % bytesPerSample;

			unsafe
			{
				fixed (byte *p = buffer)
				{
					AL.BufferData(IntroId, format, new IntPtr(p), offsetBytes, Frequency);
					AL.BufferData(MainId, format, new IntPtr(p + offsetBytes), buffer.Length - offsetBytes, Frequency);
				}
			}
			
			ALError err;
			if ((err = AL.GetError()) != ALError.NoError)
				throw new Exception(String.Format("An error occurred: {0} (error code {1} {2})", AL.GetErrorString(err), (int)err, Enum.GetName(typeof(ALError), err)));
		}

		[ContractInvariantMethod]
		private void invariants()
		{
			Contract.Invariant(Bitrate == 8 || Bitrate == 16);
			Contract.Invariant(Channels == 1 || Channels == 2);
			Contract.Invariant(Frequency > 0);
			Contract.Invariant(Duration > 0);
		}
		
		public static SoundBuffer Load(string fileName, double offset = 0)
		{
			var soundFile = SoundFileLoader.Load(fileName);
			return new SoundBuffer(soundFile.Bitrate, soundFile.Channels, soundFile.Frequency, soundFile.Buffer, offset);
		}

		public static IAsyncOperation<SoundBuffer> LoadAsync(string fileName, double offset = 0)
		{
			return Async.RunAsync(() => SoundFileLoader.LoadAsync(fileName))
						.Then(soundFile => new SoundBuffer(soundFile.Bitrate, soundFile.Channels, soundFile.Frequency, soundFile.Buffer, offset));
		}


		public bool IsDisposed { get; private set; }

		~SoundBuffer()
		{
			Dispose(false);
		}

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
						AL.DeleteBuffer(IntroId);
					foreach (var instance in _instances.Where(i => !i.IsDisposed))
						instance.Dispose();
				});

				if (disposing)
				{
					Buffer = null;
				}

				IsDisposed = true;
			}
		}

		internal void Remove(SoundInstance instance)
		{
			_instances.Remove(instance);
		}

		public int IntroId { get; }

		public int MainId { get; }

		public byte[] Buffer { get; private set; }

		public int Bitrate { get; }

		public int Channels { get; }

		public double Duration => Buffer.Length * 8.0 / (Bitrate * Channels * Frequency);

		public int Frequency { get; }

		public IReadOnlyCollection<SoundInstance> SoundInstances => Array.AsReadOnly(_instances.ToArray());

		public SoundInstance Play(bool loop = false, double volume = 1.0, double pitch = 1.0)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			Contract.Requires<ArgumentOutOfRangeException>(volume >= 0 && pitch > 0);
			Contract.Ensures(Contract.Result<SoundInstance>() != null);
			
			var instance = Instance.Create(new SoundInstance(this, IntroId, MainId, loop, volume, pitch));
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
