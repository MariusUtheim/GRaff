using System;
using GRaff.Graphics;

namespace GRaff.Effects
{
    public static class TextureGenerator
    {
        public static Texture Generate(Func<int, int, Color> func, IntVector size)
        {
			var data = new Color[size.X, size.Y];

            for (var x = 0; x < size.X; x++)
                for (var y = 0; y < size.Y; y++)
                    data[x, y] = func(x, y);

            return new Texture(data);
		}

        public static Texture Generate(Func<double, double, Color> func, IntVector size)
        {
            var data = new Color[size.Y, size.X];
            var dsize = (Vector)size;
            var (offsetX, offsetY) = (0.5 / size.X, 0.5 / size.Y);

            for (var x = 0; x < size.X; x++)
                for (var y = 0; y < size.Y; y++)
                    data[y, x] = func(x / dsize.X + offsetX, y / dsize.Y + offsetY);

            return new Texture(data);
        }


        public static Func<double, double, Color> Chessboard(Color white, Color black, IntVector squareCounts)
        {
            return (x, y) =>
            {
                var nx = GMath.Remainder((int)(x * squareCounts.X), 2);
                var ny = GMath.Remainder((int)(y * squareCounts.Y), 2);
                return ((nx ^ ny) == 0) ? white : black;
            };
        }

        public static Func<double, double, Color> Monocolored(Color color)
            => (x, y) => color;

        public static Func<double, double, Color> Linear(Color topLeft, Color topRight, Color bottomLeft, Color bottomRight)
            => (x, y) => Color.Merge(Color.Merge(topLeft, topRight, x), Color.Merge(bottomLeft, bottomRight, x), y);

        public static Func<double, double, Color> Sinusoidal(Color topLeft, Color topRight, Color bottomLeft, Color bottomRight)
        {
            double i(double x) => 0.5 * (1 - GMath.Cos(GMath.Pi * x));
            return (x, y) => Color.Merge(Color.Merge(topLeft, topRight, i(x)), Color.Merge(bottomLeft, bottomRight, i(x)), i(y));
        }
        
    }
}
