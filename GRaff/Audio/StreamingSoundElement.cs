using System;
using System.IO;
using System.Linq;

namespace GRaff.Audio
{
    public class StreamingSoundElement : SoundElement
    {
        private readonly AudioStream _stream;
        private byte[] _buffer;
        private bool _looping;

        public StreamingSoundElement(string path, bool looping, int bufferSize = 8 * 4096)
        {
            _stream = new AudioStream(path);
            _buffer = new byte[bufferSize];
            _looping = looping;

            Source.IsLooping = true;

            for (var i = 0; i < 3; i++)
                _queueNext();

            
            Source.Play();

            _Audio.ErrorCheck();
        }

        public bool IsDone { get; private set; }

        private void _queueNext(int additionalLength)
        {
            var expandedBuffer = new byte[additionalLength + _buffer.Length];
            Array.Copy(_buffer, expandedBuffer, additionalLength);
            var bytesRead = _stream.Read(expandedBuffer, additionalLength, _buffer.Length);

            if (bytesRead == 0)
            { }
            else
                Source.QueueBuffers(new SoundBuffer(_stream.Bitrate, _stream.Channels, _stream.Frequency, _buffer, bytesRead));

        }

        private void _queueNext()
        {
            var bytesRead = 0;

            while (bytesRead < _buffer.Length)
            {
                var sz = _stream.Read(_buffer, bytesRead, _buffer.Length - bytesRead);

                if (sz == 0)
                {
                    //_stream.Seek(0, SeekOrigin.Begin);
                    IsDone = true;
                    break;
                }
                else
                    bytesRead += sz;
            }

            if (bytesRead == 0)
                return;
            else if (bytesRead < _buffer.Length)
            { }
            Source.QueueBuffers(new SoundBuffer(_stream.Bitrate, _stream.Channels, _stream.Frequency, _buffer, bytesRead));

        }

        public override void OnStep()
        {
            var unqueuedBuffers = Source.UnqueueBuffers();

            foreach (var buffer in unqueuedBuffers)
            {
                buffer.Dispose();
                _queueNext();
            }

            if (IsDone && Source.State == SoundState.Stopped)
                this.Destroy();

            foreach (var buffer in unqueuedBuffers)
            {
                buffer.Dispose();
                _queueNext();
            }
			
        }

        protected override void OnDestroy()
        {
            Source.Dispose();
        }
    }
}
