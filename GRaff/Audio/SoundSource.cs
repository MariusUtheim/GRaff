using GRaff.Synchronization;
using OpenTK.Audio.OpenAL;
using System;
using System.Linq;
using System.Collections.Generic;

namespace GRaff.Audio
{
#warning Needs testing
    public class SoundSource : IDisposable
    {
        private Queue<SoundBuffer>? _queuedBuffers;

        public SoundSource()
        {
            Id = AL.GenSource();
        }

        public int Id { get; private set; }
     
        public Point Location
        {
            get
            {
                Contract.Requires<ObjectDisposedException>(!IsDisposed);
                AL.GetSource(Id, ALSource3f.Position, out float v1, out float v2, out _);
                return (v1, v2);
            }
            set
            {
                Contract.Requires<ObjectDisposedException>(!IsDisposed);
                AL.Source(Id, ALSource3f.Position, (float)value.X, (float)value.Y, 0);
            }
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


        public double ReferenceDistance
        {
            get
            {
                Contract.Requires<ObjectDisposedException>(!IsDisposed);
                AL.GetSource(Id, ALSourcef.ReferenceDistance, out float value);
                return value;
            }
            set
            {
                Contract.Requires<ObjectDisposedException>(!IsDisposed);
                AL.Source(Id, ALSourcef.ReferenceDistance, (float)value);
            }
        }

        public bool IsLooping
        {
            get
            {
                Contract.Requires<ObjectDisposedException>(!IsDisposed);
                AL.GetSource(Id, ALSourceb.Looping, out bool value);
                return value;
            }
            set
            {
                Contract.Requires<ObjectDisposedException>(!IsDisposed);
                AL.Source(Id, ALSourceb.Looping, value);
            }
        }

        public bool IsPositionRelative
        {
            get
            {
                Contract.Requires<ObjectDisposedException>(!IsDisposed);
                AL.GetSource(Id, ALSourceb.SourceRelative, out bool value);
                return value;
            }
            set
            {
                Contract.Requires<ObjectDisposedException>(!IsDisposed);
                AL.Source(Id, ALSourceb.SourceRelative, value);
            }
        }

        public double Pitch
        {
            get
            {
                Contract.Ensures(Contract.Result<double>() > 0);
                Contract.Requires<ObjectDisposedException>(!IsDisposed);
                AL.GetSource(Id, ALSourcef.Pitch, out float value);
                return value;
            }
            set
            {
                Contract.Requires<ArgumentOutOfRangeException>(value > 0);
                Contract.Requires<ObjectDisposedException>(!IsDisposed);
                AL.Source(Id, ALSourcef.Pitch, (float)value);
            }
        }

        public double Volume
        {
            get
            {
                Contract.Ensures(Contract.Result<double>() >= 0);
                Contract.Requires<ObjectDisposedException>(!IsDisposed);
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
                Contract.Requires<ObjectDisposedException>(!IsDisposed);
                AL.GetSource(Id, ALSourcef.SecOffset, out float value);
                return TimeSpan.FromSeconds(value);
            }
            set
            {
                Contract.Requires<ObjectDisposedException>(!IsDisposed);
                AL.Source(Id, ALSourcef.SecOffset, (float)value.Seconds);
            }
        }

        public int SampleOffset
        {
            get
            {
                Contract.Requires<ObjectDisposedException>(!IsDisposed);
                AL.GetSource(Id, ALGetSourcei.SampleOffset, out int value);
                return value;
            }
            set
            {
                Contract.Requires<ObjectDisposedException>(!IsDisposed);
                AL.Source(Id, ALSourcei.SampleOffset, value);
            }
        }

        public int ByteOffset
        {
            get
            {
                Contract.Requires<ObjectDisposedException>(!IsDisposed);
                AL.GetSource(Id, ALGetSourcei.ByteOffset, out int value);
                return value;
            }
            set
            {
                Contract.Requires<ObjectDisposedException>(!IsDisposed);
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
                Contract.Requires<ObjectDisposedException>(!IsDisposed);
                AL.GetSource(Id, ALGetSourcei.SourceType, out int value);
                return value == (int)ALSourceType.Static;
            }
        }

        public bool IsStreaming
        {
            get
            {
                Contract.Requires<ObjectDisposedException>(!IsDisposed);
                AL.GetSource(Id, ALGetSourcei.SourceType, out int value);
                return value == (int)ALSourceType.Streaming;
            }
        }
                  
   
        public void QueueBuffers(params SoundBuffer[] buffers)
        {
            Contract.Requires<ObjectDisposedException>(!IsDisposed);
            
            if (_queuedBuffers == null)
                _queuedBuffers = new Queue<SoundBuffer>();

            AL.SourceQueueBuffers(Id, buffers.Length, buffers.Select(b => b.Id).ToArray());
			_Audio.ErrorCheck();

            foreach (var buffer in buffers)
                _queuedBuffers.Enqueue(buffer);

#warning What's going on here?
            if (!IsStreaming)
            { }
        }

        public IEnumerable<SoundBuffer> UnqueueBuffers()
        {
            Contract.Requires<ObjectDisposedException>(!IsDisposed);
            if (_queuedBuffers == null)
                return Enumerable.Empty<SoundBuffer>();

            AL.GetSource(Id, ALGetSourcei.BuffersProcessed, out int buffersProcessed);
            if (buffersProcessed == 0)
                return Enumerable.Empty<SoundBuffer>();

#warning Understand this
            var unqueuedBuffers = AL.SourceUnqueueBuffers(Id, buffersProcessed);
            _Audio.ErrorCheck();

            var buffers = new SoundBuffer[buffersProcessed];
            for (var i = 0; i < buffers.Length; i++)
                buffers[i] = _queuedBuffers.Dequeue();

            return buffers;
        }

        private SoundBuffer? _buffer;
        public SoundBuffer? Buffer
        {
            get => _buffer;
            set
            {
                Contract.Requires<InvalidOperationException>(value == null || !value.IsDisposed);
                Contract.Requires<ObjectDisposedException>(!IsDisposed);
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
            Contract.Requires<ObjectDisposedException>(!IsDisposed);
            AL.SourcePlay(Id);
			_Audio.ErrorCheck();
		}

        public void Pause()
        {
            Contract.Requires<ObjectDisposedException>(!IsDisposed);
            AL.SourcePause(Id);
			_Audio.ErrorCheck();
		}

        public void Stop()
        {
            Contract.Requires<ObjectDisposedException>(!IsDisposed);
            AL.SourceStop(Id);
			_Audio.ErrorCheck();
		}

        public void Rewind()
        {
            Contract.Requires<ObjectDisposedException>(!IsDisposed);
            AL.SourceRewind(Id);
			_Audio.ErrorCheck();
		}


        #region IDisposable implementation

        public bool IsDisposed { get; private set; }

        ~SoundSource()
        {
            Async.Throw(new ObjectDisposedIncorrectlyException($"An instance of {typeof(SoundSource).FullName} was garbage collected without being disposed. "
                + "Sound sources may be playing even when there are no references to the object. Therefore, to ensure no sounds stop unexpectedly, "
                + "the instance must be disposed deterministically by calling Dispose."));
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                Buffer = null;

                _Audio.ClearError();

                AL.DeleteSource(Id);
                _Audio.ErrorCheck();

                IsDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        #endregion
    }
}
