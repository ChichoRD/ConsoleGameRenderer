using ConsoleGameRenderer.Rendering;
using System.Numerics;

namespace ConsoleGameRenderer.Component
{
    internal readonly record struct VertexData(Vector4 Position, Vector4 Color, Vector4 Normal, Vector4 Tangent, params Vector4[] UVs)
    {
        public VertexData(Vector4 position, Vector4 color, Vector4 normal, Vector4 tangent, Vector4 uv) : this(position, color, normal, tangent, new Vector4[] { uv }) { }

        public VertexData(Vector3 position, Vector4 color, Vector3 normal, Vector3 tangent, Vector2 uv) : this(new Vector4(position, 1.0f), color, new Vector4(normal, 0.0f), new Vector4(tangent, 0.0f), new Vector4(uv, 0.0f, 1.0f)) { }

        public VertexData(Vector3 position, Vector2 uv) : this(new Vector4(position, 1.0f), System.Drawing.Color.Pink.ToVector4(), Vector4.UnitZ, Vector4.UnitX, new Vector4(uv, 0.0f, 1.0f)) { }

        public Vector2 UV0 => UVs[0].ToVector2();

        public static VertexData Lerp(VertexData a, VertexData b, float t) => new VertexData(
            Vector4.Lerp(a.Position, b.Position, t),
            Vector4.Lerp(a.Color, b.Color, t),
            Vector4.Lerp(a.Normal, b.Normal, t),
            Vector4.Lerp(a.Tangent, b.Tangent, t),
            a.UVs.Zip(b.UVs, (a, b) => Vector4.Lerp(a, b, t)).ToArray());

        public static VertexData operator +(VertexData a, VertexData b) => new VertexData(
            a.Position + b.Position,
            a.Color + b.Color,
            a.Normal + b.Normal,
            a.Tangent + b.Tangent,
            a.UVs.Zip(b.UVs, (a, b) => a + b).ToArray());

        public static VertexData operator -(VertexData a, VertexData b) => new VertexData(
            a.Position - b.Position,
            a.Color - b.Color,
            a.Normal - b.Normal,
            a.Tangent - b.Tangent,
            a.UVs.Zip(b.UVs, (a, b) => a - b).ToArray());

        public static VertexData operator *(VertexData a, float b) => new VertexData(
            a.Position * b,
            a.Color * b,
            a.Normal * b,
            a.Tangent * b,
            a.UVs.Select(uv => uv * b).ToArray());

        public static VertexData operator /(VertexData a, float b) => new VertexData(
            a.Position / b,
            a.Color / b,
            a.Normal / b,
            a.Tangent / b,
            a.UVs.Select(uv => uv / b).ToArray());
    }
}
