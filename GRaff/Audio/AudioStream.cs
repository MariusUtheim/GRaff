using System;
using System.IO;
using System.Linq;
using OggVorbisDecoder;

namespace GRaff.Audio
{
    internal class AudioStream : Stream
    {
        private Stream _underlyingStream;
        private long _offset;

        public AudioStream(string path)
        {
            var headerBuffer = new byte[4];
            using (var stream = File.OpenRead(path))
                if (stream.Read(headerBuffer, 0, 4) < 4)
                    throw new FileFormatException("Invalid file format. Only .wav and .ogg are supported.");
            
            var header = new String(headerBuffer.Select(b => (char)b).ToArray());

            if (header == "RIFF")
                _streamWave(path);
            else if (header == "OggS")
                _streamOgg(path);
            else
                throw new FileFormatException("Invalid file format. Only .wav and .ogg are supported.");
        }


        private void _streamWave(string path)
        {
            _underlyingStream = new FileStream(path, FileMode.Open, FileAccess.Read);

            using (var reader = new BinaryReader(_underlyingStream, new System.Text.UTF8Encoding(), true))
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

                this.Bitrate = file.bitrate;
                this.Channels = file.channels;
                this.Frequency = (int)file.sampleRate;
                this._length = file.dataSize;
            }

            _offset = _underlyingStream.Position;
        }

        private void _streamOgg(string path)
        {
            var oggStream = new OggVorbisFileStream(path);

			var info = oggStream.Info;

            this.Bitrate = (int)(8 * oggStream.Length / (info.Duration * info.Rate * info.Channels));
            this.Channels = info.Channels;
            this.Frequency = info.Rate;

            _underlyingStream = oggStream;
		}


        public int Bitrate { get; private set; }

        public int Channels { get; private set; }

        public int Frequency { get; private set; }


        #region Stream implementation
        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        private long _length;
        public override long Length => _length;

        public override long Position 
        { 
            get => _underlyingStream.Position - _offset;
            set => _underlyingStream.Position = value + _offset; 
        }

        public override void Flush() => _underlyingStream.Flush();

        public override int Read(byte[] buffer, int offset, int count) => _underlyingStream.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin)
        {
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, nameof(offset));
            switch (origin)
            {
                case SeekOrigin.Begin:
                    return _underlyingStream.Seek(offset + _offset, SeekOrigin.Begin) - _offset;

                case SeekOrigin.Current:
                    if (_underlyingStream.Position + offset < _offset)
                        return _underlyingStream.Seek(_offset, SeekOrigin.Begin) - _offset;
                    else
                        return _underlyingStream.Seek(offset, origin);

                case SeekOrigin.End:
                    if (_underlyingStream.Length + offset < _offset)
                        return _underlyingStream.Seek(_offset, SeekOrigin.Begin) - _offset;
                    else
                        return _underlyingStream.Seek(offset, origin);

                default:
                    return _underlyingStream.Seek(offset, origin);
            }
                 
        }

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        #endregion

    }
}
