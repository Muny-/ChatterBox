using System;
namespace ChatterBox
{
    public static class ColorHelper
    {
        public static Gdk.RGBA Color8888(int a, int r, int g, int b)
        {
            return new Gdk.RGBA() { Alpha = a / 255.0, Red = r / 255.0, Green = g / 255.0, Blue = b / 255.0 };
        }

        public static Gdk.RGBA RandomColor() => RandomColor(100);

        public static Gdk.RGBA RandomColor(int min)
        {
            Random r = new Random();

            return Color8888(255, r.Next(min, 255), r.Next(min, 255), r.Next(min, 255));
        }
    }
}
  