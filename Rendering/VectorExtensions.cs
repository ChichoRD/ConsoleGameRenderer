using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameRenderer.Rendering
{
    internal static class VectorExtensions
    {
        public static Color ToColor(this Vector4 vector4)
        {
            return Color.FromArgb((int)(vector4.W * 255), (int)(vector4.X * 255), (int)(vector4.Y * 255), (int)(vector4.Z * 255));
        }

        public static Color ToColorUnnormalized(this Vector4 vector4)
        {
            return Color.FromArgb((int)vector4.W, (int)vector4.X, (int)vector4.Y, (int)vector4.Z);
        }

        public static Vector4 Clamp01(this Vector4 vector4) => new Vector4(Math.Clamp(vector4.X, 0, 1), Math.Clamp(vector4.Y, 0, 1), Math.Clamp(vector4.Z, 0, 1), Math.Clamp(vector4.W, 0, 1));

        public static T[,] Trasnpose<T>(this T[,] matrix)
        {
            int width = matrix.GetLength(0);
            int height = matrix.GetLength(1);

            T[,] transposed = new T[height, width];

            Parallel.For(0, width, x =>
            {
                Parallel.For(0, height, y =>
                {
                    transposed[y, x] = matrix[x, y];
                });
            });

            return transposed;
        }

        public static Vector3 ToVector3(this Vector4 vector4) => new Vector3(vector4.X, vector4.Y, vector4.Z);
        public static Vector2 ToVector2(this Vector4 vector4) => new Vector2(vector4.X, vector4.Y);
        public static Vector2 ToVector2(this Vector3 vector3) => new Vector2(vector3.X, vector3.Y);
    }
}
