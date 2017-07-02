using GRaff.Synchronization;
using OpenTK.Audio.OpenAL;
using System;
using System.Linq;
using System.Collections.Generic;

namespace GRaff.Audio
{
    public class SoundSource : IDisposable
    {
        private Queue<SoundBuffer> _queuedBuffers;

        public SoundSource()
        {
            _id = AL.GenSource();
        }

        ~SoundSource()
        {
            Async.Throw(new ObjectDisposedIncorrectlyException(typeof(SoundSource).FullName));
        }

        public void Dispose()
        {
            _notDisposed();

            Buffer = null;

            _Audio.ClearError();

            AL.DeleteSource(Id);
            _Audio.ErrorCheck();
            _id = -1;

            IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        public bool IsDisposed { get; private set; }

        private int _id;
        public int Id
        {
            get
            {
                _notDisposed();
                return _id;
            }
        }
        
        private void _notDisposed()
        {
            Contract.Requires<ObjectDisposedException>(!IsDisposed, nameof(SoundSource));
        }

        public double X
        {
            get => Location.X;
            set => Location = (value, Location.Y);
        }

        public double Y
        {
            get => Location.Y;
            set => Location = (Location.X, value);
        }

        public void QueueBuffers(params SoundBuffer[] buffers)
        {
            Contract.Requires<ArgumentNullException>(buffers != null);

            if (_queuedBuffers == null)
                _queuedBuffers = new Queue<SoundBuffer>();

            AL.SourceQueueBuffers(_id, buffers.Length, buffers.Select(b => b.Id).ToArray());
			_Audio.ErrorCheck();

            foreach (var buffer in buffers)
                _queuedBuffers.Enqueue(buffer);

            if (!IsStreaming)
            { }
        }

        public IEnumerable<SoundBuffer> UnqueueBuffers()
        {
            AL.GetSource(_id, ALGetSourcei.BuffersProcessed, out int buffersProcessed);
            if (buffersProcessed == 0)
                return Enumerable.Empty<SoundBuffer>();

            var unqueuedBuffers = AL.SourceUnqueueBuffers(_id, buffersProcessed);
            _Audio.ErrorCheck();

            var buffers = new SoundBuffer[buffersProcessed];
            for (var i = 0; i < buffers.Length; i++)
                buffers[i] = _queuedBuffers.Dequeue();

            return buffers;
        }

        public Point Location
        {
            get
            {
                _notDisposed();
                AL.GetSource(Id, ALSource3f.Position, out float v1, out float v2, out _);
                return (v1, v2);
            }
            set
            {
                _notDisposed();
                AL.Source(Id, ALSource3f.Position, (float)value.X, (float)value.Y, 0);
            }
        }

        public double ReferenceDistance
        {
            get
            {
                _notDisposed();
                AL.GetSource(Id, ALSourcef.ReferenceDistance, out float value);
                return value;
            }
            set
            {
                _notDisposed();
                AL.Source(Id, ALSourcef.ReferenceDistance, (float)value);
            }
        }

        public bool IsLooping
        {
            get
            {
                _notDisposed();
                AL.GetSource(Id, ALSourceb.Looping, out bool value);
                return value;
            }
            set
            {
                _notDisposed();
                AL.Source(Id, ALSourceb.Looping, value);
            }
        }

        public bool IsPositionRelative
        {
            get
            {
                _notDisposed();
                AL.GetSource(Id, ALSourceb.SourceRelative, out bool value);
                return value;
            }
            set
            {
                _notDisposed();
                AL.Source(Id, ALSourceb.SourceRelative, value);
            }
        }

        public double Pitch
        {
            get
            {
                Contract.Ensures(Contract.Result<double>() > 0);
                _notDisposed();
                AL.GetSource(Id, ALSourcef.Pitch, out float value);
                return value;
            }
            set
            {
                Contract.Requires<ArgumentOutOfRangeException>(value > 0);
                _notDisposed();
                AL.Source(Id, ALSourcef.Pitch, (float)value);
            }
        }

        public double Volume
        {
            get
            {
                Contract.Ensures(Contract.Result<double>() >= 0);
                _notDisposed();
                AL.GetSource(Id, ALSourcef.Gain, out float value);
                return value;
            }
            set
            {
                Contract.Requires<ArgumentOutOfRangeException>(value >= 0);
                AL.Source(Id, ALSourcef.Gain, (float)value);
            }
        }

        public TimeSpan Offset
        {
            get
            {
                _notDisposed();
                AL.GetSource(Id, ALSourcef.SecOffset, out float value);
                return TimeSpan.FromSeconds(value);
            }
            set
            {
                _notDisposed();
                AL.Source(Id, ALSourcef.SecOffset, (float)value.Seconds);
            }
        }

        public int SampleOffset
        {
            get
            {
                _notDisposed();
                AL.GetSource(Id, ALGetSourcei.SampleOffset, out int value);
                return value;
            }
            set
            {
                _notDisposed();
                AL.Source(Id, ALSourcei.SampleOffset, value);
            }
        }

        public int ByteOffset
        {
            get
            {
                _notDisposed();
                AL.GetSource(Id, ALGetSourcei.ByteOffset, out int value);
                return value;
            }
            set
            {
                _notDisposed();
                AL.Source(Id, ALSourcei.ByteOffset, value);
            }
        }

        public SoundState State
        {
            get
            {
                if (IsDisposed)
                    return SoundState.Disposed;
                else
                    return (SoundState)AL.GetSourceState(Id);
            }
        }

        public bool IsStatic
        {
            get
            {
                AL.GetSource(_id, ALGetSourcei.SourceType, out int value);
                return value == (int)ALSourceType.Static;
            }
        }

        public bool IsStreaming
        {
            get
            {
                AL.GetSource(_id, ALGetSourcei.SourceType, out int value);
                return value == (int)ALSourceType.Streaming;
            }
        }
                  

        private SoundBuffer _buffer;
        public SoundBuffer Buffer
        {
            get => _buffer;
            set
            {
                Contract.Requires<InvalidOperationException>(value == null || !value.IsDisposed);
                _notDisposed();
                _buffer = value;
                if (value == null)
                {
                    Stop();
                    AL.Source(Id, ALSourcei.Buffer, 0);
                }
                else
                    AL.Source(Id, ALSourcei.Buffer, value?.Id ?? 0);
                _Audio.ErrorCheck();
            }
        }


        public void Play()
        {
            _notDisposed();
            AL.SourcePlay(Id);
			_Audio.ErrorCheck();
		}

        public void Pause()
        {
            _notDisposed();
            AL.SourcePause(Id);
			_Audio.ErrorCheck();
		}

        public void Stop()
        {
            _notDisposed();
            AL.SourceStop(Id);
			_Audio.ErrorCheck();
		}

        public void Rewind()
        {
            _notDisposed();
            AL.SourceRewind(Id);
			_Audio.ErrorCheck();
		}


    }
}
