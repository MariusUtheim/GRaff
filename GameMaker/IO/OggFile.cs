using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OggVorbisDecoder;

namespace GameMaker.IO
{
	public class OggFile : SoundFile
	{
		private byte[] _buffer;
		private VorbisInfo _info;

		public OggFile(string filename)
		{
			using (var stream = new OggVorbisFileStream(filename))
			{
				this._info = stream.Info;
				this._buffer = new byte[stream.Length];
				using (var output = new OggVorbisMemoryStream(this._buffer, stream.Info, stream.RawLength, stream.Duration))
					stream.CopyTo(output);
			}
		}

		public VorbisInfo VorbisInfo
		{
			get { return _info; }
		}

		public override int Frequency
		{
			get { return _info.Rate; }
		}

		public override int Channels
		{
			get { return _info.Channels; }
		}

		public override byte[] Buffer
		{
			get { return _buffer; }
		}

		public override int Bitrate
		{
			get { return _info.BitrateNominal / _info.Rate; }
		}
	}
}
