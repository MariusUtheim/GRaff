using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker.IO
{
    public class WaveFile : SoundFile
    {
		private byte[] _buffer;
		private int _frequency;
		private int _channels;
		private int _bitrate;

		public WaveFile(string path)
		{
			try
			{
				using (var stream = File.OpenRead(path))
				using (var reader = new BinaryReader(stream))
				{
					var RIFF = reader.ReadString(4); // Reads RIFF
					if (RIFF != "RIFF")
						throw new FormatException(String.Format("WAVE file format must begin with \"RIFF\" ({0})", path));
					DataSize = reader.ReadUInt32();
					RIFFType = reader.ReadString(4);
					if (RIFFType != "WAVE")
						throw new FormatException(String.Format("RIFF type must be \"WAVE\" ({0})", path));

					while (reader.PeekChar() != (int)'\0' && reader.PeekChar() != 3)
					{
						var chunkId = reader.ReadString(4);
						var chunkSize = (int)reader.ReadUInt32();

						switch (chunkId)
						{
							case "fmt ":
								CompressionCode = reader.ReadUInt16();
#warning WAV loading code cannot handle compressed files
								if (CompressionCode != 1)
									throw new System.IO.InvalidDataException("WAV loading cannot handle compressed files.");
								_channels = reader.ReadUInt16();
								_frequency = (int)reader.ReadUInt32();
								BytesPerSecond = reader.ReadUInt32();
								BlockAlign = reader.ReadUInt16();
								_bitrate = (int)reader.ReadUInt16();
								if (CompressionCode != 1)
									ExtraFormatBytes = reader.ReadBytes(reader.ReadUInt16());
								break;

							case "data":
								_buffer = reader.ReadBytes(chunkSize);
								break;

							default:
								throw new InvalidDataException(String.Format("Unrecognized chunk while loading WAV file: {0}", chunkId));
						}
					}
				}
			}
			catch (EndOfStreamException)
			{
				throw;
			}
		}

		public uint DataSize { get; private set; }
		public string RIFFType { get; private set; }
		public ushort CompressionCode { get; private set; }
		public uint BytesPerSecond { get; private set; }
		public ushort BlockAlign { get; private set; }
		public ushort BitsPerSample { get; private set; }
		public byte[] ExtraFormatBytes { get; private set; }


		public override byte[] Buffer
		{
			get { return _buffer; }
		}

		public override int Channels
		{
			get { return _channels; }
		}

		public override int Frequency
		{
			get { return _frequency; }
		}

		public override int Bitrate
		{
			get { return _bitrate; }
		}
    }
}
