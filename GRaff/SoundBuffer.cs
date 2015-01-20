using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OggVorbisDecoder;
using OpenTK.Audio.OpenAL;

namespace GRaff
{
	public sealed class SoundBuffer : IDisposable
	{

		public SoundBuffer(int bitrate, int channels, double duration, int frequency, byte[] buffer)
		{
			Id = AL.GenBuffer();

			this.Bitrate = bitrate;
			this.Channels = channels;
			this.Duration = duration;
			this.Frequency = frequency;
			this.Buffer = buffer;

			ALFormat format;
			switch (Channels + Bitrate)
			{
				case 1 + 8: format = ALFormat.Mono8; break;
				case 1 + 16: format = ALFormat.Mono16; break;
				case 2 + 8: format = ALFormat.Stereo8; break;
				case 2 + 16: format = ALFormat.Stereo16; break;
				default: throw new NotSupportedException(String.Format("Ogg files must have exactly 1 or 2 channels, and a bitrate of exacty 8 or 16 bits per sample (you have {0} channel(s) and {1} bit(s) per sample).", Channels, Bitrate));
			}

			AL.BufferData(Id, format, Buffer, Buffer.Length, Frequency);

			ALError err;/*C#6.0*/
			if ((err = AL.GetError()) != ALError.NoError)
				throw new Exception(String.Format("An error occurred: {0} (error code {1} {2})", AL.GetErrorString(err), (int)err, Enum.GetName(typeof(ALError), err)));
		}

		public int Id { get; private set; }

		public int Bitrate { get; private set; }

		public int Channels { get; private set; }

		public double Duration { get; private set; } 

		public int Frequency { get; private set; }

		/// <summary>
		/// Gets the data buffer of this GRaff.SoundBuffer.
		/// </summary>
		public byte[] Buffer { get; private set; }

		public static SoundBuffer Load(string file)
		{
			using (var stream = new OggVorbisFileStream(file))
			{
				VorbisInfo info = stream.Info;

				byte[] buffer = new byte[stream.Length];
				using (var outputStream = new OggVorbisMemoryStream(buffer, info, stream.RawLength, info.Duration))
					stream.CopyTo(outputStream);

				return new SoundBuffer((int)(8 * buffer.Length / (info.Duration * info.Rate)), info.Channels, info.Duration, info.Rate, buffer);
            }
		}

		public static async Task<SoundBuffer> LoadAsync(string file)
		{
			using (var stream = new OggVorbisFileStream(file))
			{
				var info = stream.Info;

				byte[] buffer = new byte[stream.Length];
				using (var outputStream = new OggVorbisMemoryStream(buffer, info, stream.RawLength, info.Duration))
					await stream.CopyToAsync(outputStream);

				return await Async.MainThreadDispatcher.InvokeAsync(() => new SoundBuffer(info.BitrateNominal, info.Channels, (double)info.Duration, info.Rate, buffer));
            }
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
				Async.Run(() => AL.DeleteBuffer(Id));

				if (disposing)
				{
					Buffer = null;
				}
            }
		}

#endregion
	}
}
