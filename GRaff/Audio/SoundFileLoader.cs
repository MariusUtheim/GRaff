using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OggVorbisDecoder;

namespace GRaff.Audio
{
	internal static class SoundFileLoader
	{
		internal class SoundFile
		{
			public SoundFile(int bitrate, int channels, int frequency, byte[] buffer)
			{
				Bitrate = bitrate;
				Channels = channels;
				Frequency = frequency;
				Buffer = buffer;
			}
			public int Bitrate;
			public int Channels;
			public int Frequency;
			public byte[] Buffer;
		}

		public static SoundFile Load(string fileName)
		{
			var fileInfo = new FileInfo(fileName);

			string header;
			using (var stream = fileInfo.OpenRead())
			{
				Console.WriteLine(stream.Length);
				if (stream.Length < 4)
					throw new FileFormatException("Invalid file format");
				byte[] headerData = new byte[8];
				stream.ReadAsync(headerData, 0, 8);
				header = new string(headerData.Select(b => (char)b).ToArray());
			}

			if (header.Substring(0, 4) != "RIFF" && header.Substring(0, 4) != "OggS")
			{ }
			header = header.Substring(0, 4);

			if (header == "RIFF")
				return LoadWave(fileInfo);
			else if (header == "OggS")
				return LoadOgg(fileInfo);
			else
				throw new FileFormatException("Invalid file format. Only .wav and .ogg is supported.");

		}

		public static async Task<SoundFile> LoadAsync(string fileName)
		{
			var fileInfo = new FileInfo(fileName);

			string header;
			using (var stream = new FileStream(fileName, FileMode.Open))
			{
				if (stream.Length < 4)
					throw new FileFormatException("Invalid file format");
				byte[] headerData = new byte[4];
				await stream.ReadAsync(headerData, 0, 4);
				header = new string(headerData.Select(b => (char)b).ToArray());
			}

			if (header == "RIFF")
				return await LoadWaveAsync(fileInfo);
			else if (header == "OggS")
				return await LoadOggAsync(fileInfo);
			else
				throw new FileFormatException("Invalid file format. Only .wav and .ogg is supported.");
		}

		private static async Task<SoundFile> LoadWaveAsync(FileInfo fileInfo)
		{
			using (var stream = fileInfo.OpenRead())
			using (var reader = new BinaryReader(stream))
			{
				var file = new WaveFile();

				file.riffID = reader.ReadBytes(4);

				var name = new string(file.riffID.Select(b => (char)b).ToArray());

				file.size = reader.ReadUInt32();
				file.wavID = reader.ReadBytes(4);
				file.fmtID = reader.ReadBytes(4);
				file.fmtSize = reader.ReadUInt32();
				file.compressionCode = reader.ReadUInt16();
				file.channels = reader.ReadUInt16();
				file.sampleRate = reader.ReadUInt32();
				file.bytesPerSecond = reader.ReadUInt32();
				file.blockAlign = reader.ReadUInt16();
				file.bitrate = reader.ReadUInt16();
				file.dataID = reader.ReadBytes(4);
				file.dataSize = reader.ReadUInt32();

				file.buffer = new byte[file.dataSize];

				await stream.ReadAsync(file.buffer, 0, (int)file.dataSize);

				return new SoundFile(file.bitrate, file.channels, (int)file.sampleRate, file.buffer);
			}
		}

		private static SoundFile LoadWave(FileInfo fileInfo)
		{
			using (var stream = fileInfo.OpenRead())
			using (var reader = new BinaryReader(stream))
			{
				var file = new WaveFile();

				file.riffID = reader.ReadBytes(4);
				file.size = reader.ReadUInt32();
				file.wavID = reader.ReadBytes(4);
				file.fmtID = reader.ReadBytes(4);
				file.fmtSize = reader.ReadUInt32();
				file.compressionCode = reader.ReadUInt16();
				file.channels = reader.ReadUInt16();
				file.sampleRate = reader.ReadUInt32();
				file.bytesPerSecond = reader.ReadUInt32();
				file.blockAlign = reader.ReadUInt16();
				file.bitrate = reader.ReadUInt16();
				file.dataID = reader.ReadBytes(4);
				file.dataSize = reader.ReadUInt32();

				file.buffer = new byte[file.dataSize];

				stream.Read(file.buffer, 0, (int)file.dataSize);

				return new SoundFile(file.bitrate, file.channels, (int)file.sampleRate, file.buffer);
			}
		}

		private static string toString(byte[] array)
		{
			return new string(array.Select(b => (char)b).ToArray());
		}

		private static async Task<SoundFile> LoadOggAsync(FileInfo fileInfo)
		{
			using (var stream = new OggVorbisFileStream(fileInfo.FullName))
			{
				VorbisInfo info = stream.Info;

				byte[] buffer = new byte[stream.Length];
				using (var outputStream = new OggVorbisMemoryStream(buffer, info, stream.RawLength, info.Duration))
					await stream.CopyToAsync(outputStream);

				return new SoundFile((int)(8 * buffer.Length / (info.Duration * info.Rate * info.Channels)), info.Channels, info.Rate, buffer);
			}
		}

		private static SoundFile LoadOgg(FileInfo fileInfo)
		{
			using (var stream = new OggVorbisFileStream(fileInfo.FullName))
			{
				VorbisInfo info = stream.Info;

				byte[] buffer = new byte[stream.Length];
				using (var outputStream = new OggVorbisMemoryStream(buffer, info, stream.RawLength, info.Duration))
					stream.CopyTo(outputStream);

				return new SoundFile((int)(8 * buffer.Length / (info.Duration * info.Rate * info.Channels)), info.Channels, info.Rate, buffer);
			}
		}
	}
}
