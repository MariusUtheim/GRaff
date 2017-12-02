using System;

namespace GRaff.Effects
{
    public static class WaveGenerator
    {
#warning Needs testing!
        public static Func<double, double> Sine(double wavelength = GMath.Tau, double amplitude = 1, double origin = 0, double phase = 0)
            => t => origin + amplitude * GMath.Sin(GMath.Tau * t / wavelength + phase);

        public static Func<double, double> Sine(Rectangle box, double waveNumber = 1.0)
            => Sine(box.Width / waveNumber, box.Height / 2, box.Center.Y, -box.Left);

        public static Func<double, double> Binary(double wavelength = 2.0, double amplitude = 1, double origin = 0, double phase = 0)
            => t => origin + (GMath.Remainder(0.5 * t * 2.0 / wavelength + phase, 1.0) > 0.5 ? amplitude : 0.0);

        public static Func<double, double> Square(double wavelength = 2.0, double amplitude = 1, double origin = 0, double phase = 0)
            => t => origin + (GMath.Remainder(0.5 * t * 2.0 / wavelength + phase, 1.0) > 0.5 ? amplitude : -amplitude);

        public static Func<double, double> Square(Rectangle box, double waveNumber = 1.0)
            => Square(box.Width / waveNumber, box.Height / 2, box.Center.Y, -box.Left);

        public static Func<double, bool> BinarySignal(double wavelength = 2.0, double phase = 0)
            => t => GMath.Remainder(0.5 * (t + phase) * 2.0 / wavelength, 1.0) > 0.5;

        public static Func<double, double> Sawtooth(double wavelength = 1.0, double amplitude = 1, double origin = 0, double phase = 0)
            => t => origin + amplitude * (GMath.Remainder((t + phase) / wavelength, 1.0));

        public static Func<double, double> Sawtooth(Rectangle box, double waveNumber = 1.0)
            => Sawtooth(box.Width / waveNumber, box.Height / 2, box.Center.Y, -box.Left);

        public static Func<double, double> Triangle(double wavelength = 2.0, double amplitude = 1, double origin = 0, double phase = 0)
            => t => origin + 2 * amplitude * (-0.5 + GMath.Abs(GMath.Remainder((t + phase) * 2.0 / wavelength + 0.5, 2.0) - 1.0));

        public static Func<double, double> Triangle(Rectangle box, double waveNumber = 1.0)
            => Triangle(box.Width / waveNumber, box.Height / 2, box.Center.Y, -box.Left);
    }
}
