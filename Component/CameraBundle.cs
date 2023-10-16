using System.Numerics;

namespace ConsoleGameRenderer.Component
{
    internal readonly record struct CameraBundle(Transform Transform, Camera Camera)
    {
        public Matrix4x4 ViewMatrix => Transform.WorldToObjectMatrix;
        public Matrix4x4 InverseViewMatrix => Transform.ObjectToWorldMatrix;
    }
}
