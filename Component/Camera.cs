using System.Numerics;

namespace ConsoleGameRenderer.Component
{
    internal readonly record struct Camera(Vector2 Size, float FieldOfView, float NearPlaneDistance, float FarPlaneDistance)
    {
    }
}
