using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using GRaff.Audio;
using GRaff.Synchronization;
using OpenTK.Audio.OpenAL;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace GRaff
{
    public sealed class SoundBuffer : IDisposable
    {
        private readonly MutableList<SoundElement> _instances = new MutableList<SoundElement>();
        private readonly ALFormat _format;

        public SoundBuffer(int bitrate, int channels, int frequency, byte[] buffer, int? length = null)
        {
            Contract.Requires<ArgumentOutOfRangeException>(bitrate == 8 || bitrate == 16);
            Contract.Requires<ArgumentOutOfRangeException>(channels == 1 || channels == 2);
            Contract.Requires<ArgumentOutOfRangeException>(frequency > 0);
            Contract.Requires<ArgumentNullException>(buffer != null);
			Contract.Requires<ArgumentException>(buffer.Length > 0);

            Id = AL.GenBuffer();
            this.Buffer = buffer;
            this.Bitrate = bitrate;
            this.Channels = channels;
            this.Frequency = frequency;

            switch (Channels + Bitrate)
            {
                case 1 + 8: _format = ALFormat.Mono8; break;
                case 1 + 16: _format = ALFormat.Mono16; break;
                case 2 + 8: _format = ALFormat.Stereo8; break;
                case 2 + 16: _format = ALFormat.Stereo16; break;
                default: throw new NotSupportedException($"Sound files must have exactly 1 or 2 channels, and a bitrate of exacty 8 or 16 bits per sample (you have {Channels} channel(s) and {Bitrate} bit(s) per sample).");
            }

            _Audio.ErrorCheck();

            AL.BufferData(Id, _format, buffer, length ?? buffer.Length, Frequency);

            _Audio.ErrorCheck();
        }


        public static SoundBuffer Load(string path)
        {
            using (var stream = AudioStream.Open(path))
            {
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);
                return new SoundBuffer(stream.Bitrate, stream.Channels, stream.Frequency, buffer);
            }
        }

        public static IAsyncOperation<SoundBuffer> LoadAsync(string path)
        {
            return Async.RunAsync(async () =>
            {
                using (var stream = AudioStream.Open(path))
                {
                    var buffer = new byte[stream.Length];
                    await stream.ReadAsync(buffer, 0, (int)stream.Length);
                    return (bitrate: stream.Bitrate, channels: stream.Channels, frequency: stream.Frequency, buffer: buffer);
                }
            }).ThenQueue(soundFile => new SoundBuffer(soundFile.bitrate, soundFile.channels, soundFile.frequency, soundFile.buffer));
        }

        public static SoundElement Stream(string fileName)
        {
            return Instance.Create(new StreamingSoundElement(fileName, true));
        }

        public int Id { get; }

        public byte[] Buffer { get; }

        public int Bitrate { get; }

        public int Channels { get; }

        public int Frequency { get; }

        public int Size => Buffer.Length;

        public int BytesPerSample => Bitrate / 8 * Channels;

        public int SampleCount => Size / BytesPerSample;

        public TimeSpan Duration => TimeSpan.FromSeconds(SampleCount / (double)Frequency);

        public IEnumerable<SoundElement> SoundInstances => _instances.ToList();

        public bool IsDisposed { get; private set; }


        ~SoundBuffer()
        {
            Dispose(false);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        private void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                Async.Capture(Id).ThenQueue(id =>
                {
                    if (Game.IsRunning)
                    {
                        _Audio.ClearError();
                        foreach (var instance in _instances)
                            instance.Destroy();

                        _Audio.ClearError();

                        AL.DeleteBuffer(id);
                        _Audio.ErrorCheck();
                    }
                });

                IsDisposed = true;
            }
        }

        internal void Remove(SoundElement instance)
        {
            _instances.Remove(instance);
        }


        private SoundElement _create(bool looping, bool playing, Point location, double volume, double pitch)
        {
            Contract.Requires<ObjectDisposedException>(!IsDisposed);
            Contract.Requires<ArgumentOutOfRangeException>(volume >= 0 && pitch > 0);
            Contract.Ensures(Contract.Result<SoundElement>() != null);

            var instance = Instance.Create(new SimpleSoundElement(this, looping, volume, pitch, location));
            if (playing)
                instance.Source.Play();

            _instances.Add(instance);
            return instance;
        }

        public SoundElement Play(bool looping = false, double volume = 1.0, double pitch = 1.0)
            => _create(looping, true, (0, 0), volume, pitch);

        public SoundElement Play(Point location, bool looping = false, double volume = 1.0, double pitch = 1.0)
        {
#warning Add error message - test that this applies only to mono (also see PlayPaused)
			Contract.Requires<InvalidOperationException>(Channels == 1);
            return _create(looping, true, location, volume, pitch);
        }

        public SoundElement PlayPaused(bool looping = false, double volume = 1.0, double pitch = 1.0)
            => _create(looping, false, (0, 0), volume, pitch);

		public SoundElement PlayPaused(Point location, bool looping = false, double volume = 1.0, double pitch = 1.0)
		{
			Contract.Requires<InvalidOperationException>(Channels == 1);
			return _create(looping, false, location, volume, pitch);
		}

        public void StopAll()
		{
            foreach (var instance in _instances)
				instance.Destroy();
		}
       

        [ContractInvariantMethod]
        private void invariants()
        {
            Contract.Invariant(Bitrate == 8 || Bitrate == 16);
            Contract.Invariant(Channels == 1 || Channels == 2);
            Contract.Invariant(Frequency > 0);
        }
    }
}
