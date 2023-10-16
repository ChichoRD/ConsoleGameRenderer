using System.Drawing;
using System.Numerics;

namespace ConsoleGameRenderer.Rendering
{
    internal static class ColorExtensions
    {
        public static ConsoleColor ToConsoleColor(this Color color)
        {
            int index = (color.R > 128 | color.G > 128 | color.B > 128) ? 8 : 0; // Bright bit
            index |= (color.R > 64) ? 4 : 0; // Red bit
            index |= (color.G > 64) ? 2 : 0; // Green bit
            index |= (color.B > 64) ? 1 : 0; // Blue bit
            return (ConsoleColor)index;
        }

        public static Vector4 ToVector4(this Color color)
        {
            return new Vector4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
        }

        public static Vector4 ToVector4Unnormalized(this Color color)
        {
            return new Vector4(color.R, color.G, color.B, color.A);
        }
    }
}
