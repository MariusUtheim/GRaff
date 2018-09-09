using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Audio
{
    public class WaveAudioStream : AudioStream
    {
        private Stream _underlyingStream;
        private long _beginOffset;

        public WaveAudioStream(string path)
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
            }

            _beginOffset = _underlyingStream.Position;
        }


        public override int Bitrate { get; }

        public override int Channels { get; }

        public override int Frequency { get; }

        protected override void Dispose(bool disposing)
        {
            _underlyingStream.Dispose();
            base.Dispose(disposing);
        }

        #region Stream implementation

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;
        
        public override long Length => _underlyingStream.Length;

        public override long Position
        {
            get => _underlyingStream.Position - _beginOffset;
            set => _underlyingStream.Position = value + _beginOffset;
        }

        public override void Flush() => _underlyingStream.Flush();

        public override int Read(byte[] buffer, int offset, int count) => _underlyingStream.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin)
        {
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, nameof(offset));
            switch (origin)
            {
                case SeekOrigin.Begin:
                    return _underlyingStream.Seek(offset + _beginOffset, SeekOrigin.Begin) - _beginOffset;

                case SeekOrigin.Current:
                    if (_underlyingStream.Position + offset < _beginOffset)
                        return _underlyingStream.Seek(_beginOffset, SeekOrigin.Begin) - _beginOffset;
                    else
                        return _underlyingStream.Seek(offset, origin);

                case SeekOrigin.End:
                    if (_underlyingStream.Length + offset < _beginOffset)
                        return _underlyingStream.Seek(_beginOffset, SeekOrigin.Begin) - _beginOffset;
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
