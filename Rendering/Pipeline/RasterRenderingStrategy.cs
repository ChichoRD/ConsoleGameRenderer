using ConsoleGameRenderer.Component;
using ConsoleGameRenderer.Rendering.Shading;
using System.Numerics;

namespace ConsoleGameRenderer.Rendering.Pipeline
{
    internal class RasterRenderingStrategy<TColor> : IRenderingStrategy<TColor>
        where TColor : struct
    {
        private readonly IRenderable<TColor> _renderable;

        public RasterRenderingStrategy(IRenderable<TColor> renderable)
        {
            _renderable = renderable;
        }

        public void Render(CommandBuffer commandBuffer, CameraBundle cameraBundle, Texture<TColor> colorBuffer, Texture<float> depthBuffer)
        {
            _renderable.VertexMaterial.TryPassUniforms(string.Empty, _renderable);
            _renderable.VertexMaterial.TryPassUniforms(string.Empty, cameraBundle);

            VertexData[] verticesNDC = new VertexData[_renderable.VerticesData.Length];

            lock (_renderable.VerticesData)
            {
                Parallel.For(0, _renderable.VerticesData.Length, i =>
                {
                    verticesNDC[i] = _renderable.VertexMaterial.ApplyShader(_renderable.VerticesData[i]);
                });
            }

            int trianglesAmount = _renderable.Indices.Length / 3;
            _renderable.FragmentMaterial.TryPassUniforms(string.Empty, _renderable);

            int width = colorBuffer.Width;
            int height = colorBuffer.Height;

            for (int i = 0; i < trianglesAmount; i++)
            {
                int index0 = _renderable.Indices[i * 3];
                int index1 = _renderable.Indices[i * 3 + 1];
                int index2 = _renderable.Indices[i * 3 + 2];

                VertexData vertexData0SS = verticesNDC[index0] with { Position = (verticesNDC[index0].Position * 0.5f + new Vector4(0.5f, 0.5f, 0.0f, 0.0f)) * new Vector4(width, height, 1.0f, 1.0f), };
                VertexData vertexData1SS = verticesNDC[index1] with { Position = (verticesNDC[index1].Position * 0.5f + new Vector4(0.5f, 0.5f, 0.0f, 0.0f)) * new Vector4(width, height, 1.0f, 1.0f), };
                VertexData vertexData2SS = verticesNDC[index2] with { Position = (verticesNDC[index2].Position * 0.5f + new Vector4(0.5f, 0.5f, 0.0f, 0.0f)) * new Vector4(width, height, 1.0f, 1.0f), };
                commandBuffer.FillTriangle(vertexData0SS,
                                           vertexData1SS,
                                           vertexData2SS, colorBuffer, _renderable.FragmentMaterial);
            }
        }

        public void Render<TRenderOutput>(CommandBuffer commandBuffer, CameraBundle cameraBundle, Texture<TRenderOutput> colorBuffer, Texture<float> depthBuffer, IShader<TColor, TRenderOutput> adapterShader) where TRenderOutput : struct
        {
            _renderable.VertexMaterial.TryPassUniforms(string.Empty, _renderable);
            _renderable.VertexMaterial.TryPassUniforms(string.Empty, cameraBundle);

            VertexData[] verticesNDC = new VertexData[_renderable.VerticesData.Length];

            lock (verticesNDC)
            {
                Parallel.For(0, _renderable.VerticesData.Length, i =>
                {
                    verticesNDC[i] = _renderable.VertexMaterial.ApplyShader(_renderable.VerticesData[i]);
                });
            }

            int trianglesAmount = _renderable.Indices.Length / 3;
            var fragmentMaterial = _renderable.FragmentMaterial.AddShader(adapterShader);
            fragmentMaterial.TryPassUniforms(string.Empty, _renderable);

            int width = colorBuffer.Width;
            int height = colorBuffer.Height;

            for (int i = 0; i < trianglesAmount; i++)
            {
                int index0 = _renderable.Indices[i * 3];
                int index1 = _renderable.Indices[i * 3 + 1];
                int index2 = _renderable.Indices[i * 3 + 2];

                VertexData vertexData0SS = verticesNDC[index0] with { Position = (verticesNDC[index0].Position * 0.5f + new Vector4(0.5f, 0.5f, 0.0f, 0.0f)) * new Vector4(width, height, 1.0f, 1.0f), };
                VertexData vertexData1SS = verticesNDC[index1] with { Position = (verticesNDC[index1].Position * 0.5f + new Vector4(0.5f, 0.5f, 0.0f, 0.0f)) * new Vector4(width, height, 1.0f, 1.0f), };
                VertexData vertexData2SS = verticesNDC[index2] with { Position = (verticesNDC[index2].Position * 0.5f + new Vector4(0.5f, 0.5f, 0.0f, 0.0f)) * new Vector4(width, height, 1.0f, 1.0f), };
                commandBuffer.FillTriangle(vertexData0SS,
                                           vertexData1SS,
                                           vertexData2SS, colorBuffer, fragmentMaterial);
            }
        }
    }
}
