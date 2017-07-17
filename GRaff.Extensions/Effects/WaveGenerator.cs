using System;

namespace GRaff.Effects
{
    public static class WaveGenerator
    {
    //    public static SoundBuffer Generate(Func<double, byte> generator, TimeSpan duration, int sampleFrequency = 11025)
    //    {
    //        var buffer = new byte[(int)(sampleFrequency * duration.Seconds)];
    //        for (var i = 0; i < buffer.Length; i++)
    //            buffer[i] = generator(i * duration.Seconds / (double)sampleFrequency);
    //        return new SoundBuffer(8, 1, sampleFrequency, buffer);
    //    }
    //
    //    public static SoundBuffer Generate(Func<double, double> generator, TimeSpan duration, int sampleFrequency = 11025)
    //        => Generate(t => (byte)(255 * generator(t)), duration, sampleFrequency);
    //
    //    public static Func<double, byte> Sine(double pitch)
    //        => t => (byte)(255 * 0.5 * (1 + GMath.Sin(pitch * GMath.Tau * t)));
    //
    //    public static Func<double, byte> WhiteNoise()
    //        => t => (byte)GRandom.Integer(255);
    //
    //    public static Func<double, byte> Binary(double pitch)
    //        => t => (pitch * t) % 1 > 0.5 ? (byte)255 : (byte)0;
    //
    //    public static Func<double, byte> Sawtooth(double pitch)
    //        => t => (byte)(255 * t * pitch);


        public static Func<double, double> Sine(double wavelength = GMath.Tau, double amplitude = 1, double origin = 0, double phase = 0)
            => t => origin + amplitude * GMath.Sin(GMath.Tau * t / wavelength + phase);

        public static Func<double, double> Binary(double wavelength = 2.0, double amplitude = 1, double origin = 0, double phase = 0)
            => t => origin + (GMath.Remainder(0.5 * (t + phase) * 2.0 / wavelength, 1.0) > 0.5 ? amplitude : 0.0);

        public static Func<double, double> Square(double wavelength = 2.0, double amplitude = 1, double origin = 0, double phase = 0)
            => t => origin + (GMath.Remainder(0.5 * (t + phase) * 2.0 / wavelength, 1.0) > 0.5 ? amplitude : -amplitude);

        public static Func<double, bool> BinarySignal(double wavelength = 2.0, double phase = 0)
            => t => GMath.Remainder(0.5 * (t + phase) * 2.0 / wavelength, 1.0) > 0.5;

        public static Func<double, double> Sawtooth(double wavelength = 1.0, double amplitude = 1, double origin = 0, double phase = 0)
            => t => origin + amplitude * (GMath.Remainder((t + phase) / wavelength, 1.0));

        public static Func<double, double> Triangle(double wavelength = 2.0, double amplitude = 1, double origin = 0, double phase = 0)
            => t => origin + 2 * amplitude * (-0.5 + GMath.Abs(GMath.Remainder((t + phase) * 2.0 / wavelength + 0.5, 2.0) - 1.0));
    }
}
