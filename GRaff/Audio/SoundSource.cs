using GRaff.Synchronization;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Audio
{
    public class SoundSource : IDisposable
    {

        public SoundSource()
        {
            _id = AL.GenSource();
            var s = AL.GetSourceType(Id);
        }

        ~SoundSource()
        {
            Async.Throw(new ObjectDisposedIncorrectlyException(typeof(SoundSource).FullName));
        }

        public void Dispose()
        {
            Contract.Requires<ObjectDisposedException>(!IsDisposed);

            Buffer = null;
            AL.DeleteSource(Id);
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
            Contract.Requires<ObjectDisposedException>(!IsDisposed);
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

        public bool Looping
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

        public double SecondsOffset
        {
            get
            {
                _notDisposed();
                AL.GetSource(Id, ALSourcef.SecOffset, out float value);
                return value;
            }
            set
            {
                _notDisposed();
                AL.Source(Id, ALSourcef.SecOffset, (float)value);
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
                _notDisposed();
                return (SoundState)AL.GetSourceState(Id);
            }
        }

        private SoundBuffer _buffer;
        public SoundBuffer Buffer
        {
            get => _buffer;
            set
            {
                Contract.Requires<InvalidOperationException>(!value.IsDisposed);
                _notDisposed();
                _buffer = value;
                AL.Source(Id, ALSourcei.Buffer, value?.Id ?? 0);
            }
        }


        public void Play()
        {
            _notDisposed();
            AL.SourcePlay(Id);
        }

        public void Pause()
        {
            _notDisposed();
            AL.SourcePause(Id);
        }

        public void Stop()
        {
            _notDisposed();
            AL.SourceStop(Id);
        }

        public void Rewind()
        {
            _notDisposed();
            AL.SourceRewind(Id);
        }


    }
}
