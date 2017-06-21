using System;

namespace GRaff.Effects
{
    public static class WaveGenerator
    {
        public static SoundBuffer Generate(Func<double, byte> generator, TimeSpan duration, int sampleFrequency = 11025)
        {
            var buffer = new byte[(int)(sampleFrequency * duration.Seconds)];
            for (var i = 0; i < buffer.Length; i++)
                buffer[i] = generator(i * duration.Seconds / (double)sampleFrequency);
            return new SoundBuffer(8, 1, sampleFrequency, buffer);
        }

        public static SoundBuffer Generate(Func<double, double> generator, TimeSpan duration, int sampleFrequency = 11025)
            => Generate(t => (byte)(255 * generator(t)), duration, sampleFrequency);

        public static Func<double, byte> Sine(double pitch)
            => t => (byte)(255 * 0.5 * (1 + GMath.Sin(pitch * GMath.Tau * t)));

        public static Func<double, byte> WhiteNoise()
            => t => (byte)GRandom.Integer(255);

        public static Func<double, byte> Binary(double pitch)
            => t => (pitch * t) % 1 > 0.5 ? (byte)255 : (byte)0;

        public static Func<double, byte> Sawtooth(double pitch)
            => t => (byte)(255 * t * pitch);
    }
}
