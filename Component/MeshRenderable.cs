using ConsoleGameRenderer.Rendering;
using ConsoleGameRenderer.Rendering.Pipeline;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameRenderer.Component
{
    internal readonly record struct MeshRenderable(Mesh Mesh, Transform Transform, IMaterial<VertexData, VertexData> VertexMaterial, IMaterial<VertexData, Color> FragmentMaterial)
        : IRenderable<Color>
    {
        public VertexData[] VerticesData => Mesh.VerticesData;

        public int[] Indices => Mesh.Indices;

        public IRenderingStrategy<Color> GetRenderingStrategy()
        {
            return new RasterRenderingStrategy<Color>(this);
        }
    }
}
