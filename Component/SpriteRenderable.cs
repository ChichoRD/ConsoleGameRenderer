using System.Drawing;
using System.Numerics;
using ConsoleGameRenderer.Rendering;
using ConsoleGameRenderer.Rendering.Pipeline;

namespace ConsoleGameRenderer.Component
{
    internal readonly record struct SpriteRenderable<TSpriteElement>(Sprite<TSpriteElement> Sprite, Transform Transform, IMaterial<VertexData, VertexData> VertexMaterial, IMaterial<VertexData, TSpriteElement> FragmentMaterial)
        : IRenderable<TSpriteElement>
        where TSpriteElement : struct
    {
        public VertexData[] VerticesData { get; } = new VertexData[]
        {
            new VertexData(new Vector3(-0.5f, 0.5f, 0.0f), new Vector2(0.0f, 1.0f)) with { Color = Color.Blue.ToVector4() },
            new VertexData(new Vector3(0.5f, 0.5f, 0.0f), new Vector2(1.0f, 1.0f)) with { Color = Color.Red.ToVector4() },
            new VertexData(new Vector3(0.5f, -0.5f, 0.0f), new Vector2(1.0f, 0.0f)) with { Color = Color.Green.ToVector4() },
            new VertexData(new Vector3(-0.5f, -0.5f, 0.0f), new Vector2(0.0f, 0.0f)) with { Color = Color.Yellow.ToVector4() },
        };

        public int[] Indices { get; } = new int[]
        {
            0, 1, 2,
            0, 2, 3,
        };

        public IRenderingStrategy<TSpriteElement> GetRenderingStrategy()
        {
            return new RasterRenderingStrategy<TSpriteElement>(this);
        }
    }
}