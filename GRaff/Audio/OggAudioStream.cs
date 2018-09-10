using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NVorbis;

namespace GRaff.Audio
{
#warning Should have some more testing of different variations on the format (e.g. concatenated files)
    public class OggAudioStream : AudioStream
    {
        private VorbisReader _reader;

        public OggAudioStream(string path)
        {
            _reader = new VorbisReader(path);

            this.Channels = _reader.Channels;
            this.Frequency = _reader.SampleRate;
            this.Bitrate = 16;
        }

        protected override void Dispose(bool disposing)
        {
            _reader.Dispose();
            base.Dispose(disposing);
        }

        #region Stream implementation

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

		public override long Length => sizeof(float) * _reader.TotalSamples;

        public override long Position
        {
            get => _reader.DecodedPosition;
            set => _reader.DecodedPosition = value;
        }

        public override void Flush() => throw new NotSupportedException();

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (count % 2 == 1)
                count -= 1;

            var fbuf = new float[count / 2];
            var result = _reader.ReadSamples(fbuf, 0, fbuf.Length);
            
            for (var i = 0; i < fbuf.Length; i++)
            {
                var sample = (int)(32767 * fbuf[i]);
                buffer[offset + 2 * i] = (byte)(sample & 0xFF);
                buffer[offset + 2 * i + 1] = (byte)((sample >> 8) & 0xFF);
            }

            return result * 2;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        #endregion
    }
}
