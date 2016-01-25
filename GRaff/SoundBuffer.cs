using System;
using System.Diagnostics.Contracts;
using GRaff.Audio;
using GRaff.Synchronization;
using OpenTK.Audio.OpenAL;

namespace GRaff
{
	public sealed class SoundBuffer : IDisposable
	{

		public SoundBuffer(int bitrate, int channels, int frequency, byte[] buffer, double offset)
		{
			Contract.Requires<ArgumentNullException>(buffer != null);
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

		public int IntroId { get; private set; }

		public int MainId { get; private set; }

		public byte[] Buffer { get; private set; }

		/// <summary>
		/// Gets the number of bits per sample of this GRaff.SoundBuffer.
		/// </summary>
		public int Bitrate { get; private set; }

		/// <summary>
		/// Gets the number of channels of this GRaff.SoundBuffer (1 for mono, 2 for stereo)
		/// </summary>
		public int Channels { get; private set; }

		/// <summary>
		/// Gets the duration of this GRaff.SoundBuffer in seconds.
		/// </summary>
		/// <returns></returns>
		public double Duration { get { return Buffer.Length * 8.0 / (Bitrate * Channels * Frequency); } }

		/// <summary>
		/// Gets the frequency of the data in this GRaff.SoundBuffer. That is, the number of samples per second.
		/// </summary>
		public int Frequency { get; private set; }


		public static SoundBuffer Load(string fileName, double offset)
		{
			var soundFile = SoundFileLoader.Load(fileName);
			return new SoundBuffer(soundFile.Bitrate, soundFile.Channels, soundFile.Frequency, soundFile.Buffer, offset);
		}

		public static IAsyncOperation<SoundBuffer> LoadAsync(string fileName, double offset)
		{
			return Async.RunAsync(() => SoundFileLoader.LoadAsync(fileName))
						.Then(soundFile => new SoundBuffer(soundFile.Bitrate, soundFile.Channels, soundFile.Frequency, soundFile.Buffer, offset));
		}

#region IDisposable implementation
		private bool _isDisposed = false;

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
			if (!_isDisposed)
			{
				_isDisposed = true;
				Async.Run(() => AL.DeleteBuffer(IntroId));

				if (disposing)
				{
					Buffer = null;
				}
            }
		}

#endregion
	}
}
