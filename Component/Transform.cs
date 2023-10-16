using System.Numerics;

namespace ConsoleGameRenderer.Component
{
    internal readonly record struct Transform(Vector3 Position, Quaternion Rotation, Vector3 Scale)
    {
        public Transform() : this(Vector3.Zero, Quaternion.Identity, Vector3.One) { }

        public Transform(Vector3 position) : this(position, Quaternion.Identity, Vector3.One) { }

        public Transform(Vector3 position, Quaternion rotation) : this(position, rotation, Vector3.One) { }

        public Matrix4x4 ScaleMatrix { get; } = Matrix4x4.CreateScale(Scale);
        public Matrix4x4 RotationMatrix { get; } = Matrix4x4.CreateFromQuaternion(Rotation);
        public Matrix4x4 TranslationMatrix { get; } = Matrix4x4.CreateTranslation(Position);

        public Matrix4x4 ObjectToWorldMatrix { get; } = Matrix4x4.CreateScale(Scale) * Matrix4x4.CreateFromQuaternion(Rotation) * Matrix4x4.CreateTranslation(Position);
        public Matrix4x4 WorldToObjectMatrix { get; } = Matrix4x4.CreateTranslation(-Position) * Matrix4x4.CreateFromQuaternion(Quaternion.Inverse(Rotation)) * Matrix4x4.CreateScale(Vector3.One / Scale);
    }
}
