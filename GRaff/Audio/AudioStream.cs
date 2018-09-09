using System;
using System.IO;
using System.Linq;
using NVorbis;

namespace GRaff.Audio
{
    public abstract class AudioStream : Stream
    {

        protected AudioStream() { }

        public static AudioStream Open(string path)
        {
            var headerBuffer = new byte[4];
            using (var stream = File.OpenRead(path))
                if (stream.Read(headerBuffer, 0, 4) < 4)
                    throw new FileFormatException("Invalid file format. Only .wav and .ogg are supported.");

            var header = new String(headerBuffer.Select(b => (char)b).ToArray());

            if (header == "RIFF")
                return new WaveAudioStream(path);
            else if (header == "OggS")
                return new OggAudioStream(path);
            else
                throw new FileFormatException("Invalid file format. Only .wav and .ogg are supported.");
        }
        

        public abstract int Bitrate { get; }

        public abstract int Channels { get; }

        public abstract int Frequency { get; }
        

    }
}
