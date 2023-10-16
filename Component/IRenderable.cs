using ConsoleGameRenderer.Rendering;
using ConsoleGameRenderer.Rendering.Pipeline;

namespace ConsoleGameRenderer.Component
{
    internal interface IRenderable<TFragmentOutput>
        where TFragmentOutput : struct
    {
        IMaterial<VertexData, VertexData> VertexMaterial { get; }
        IMaterial<VertexData, TFragmentOutput> FragmentMaterial { get; }

        Transform Transform { get; }
        VertexData[] VerticesData { get; }
        int[] Indices { get; }

        IRenderingStrategy<TFragmentOutput> GetRenderingStrategy();
    }
}
