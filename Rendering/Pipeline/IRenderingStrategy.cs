using ConsoleGameRenderer.Component;
using ConsoleGameRenderer.Rendering.Shading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameRenderer.Rendering.Pipeline
{
    internal interface IRenderingStrategy<TStrategyOutput>
        where TStrategyOutput : struct
    {
        void Render(CommandBuffer commandBuffer,
                    CameraBundle cameraBundle,
                    Texture<TStrategyOutput> colorBuffer,
                    Texture<float> depthBuffer);

        void Render<TRenderOutput>(CommandBuffer commandBuffer,
                                   CameraBundle cameraBundle,
                                   Texture<TRenderOutput> colorBuffer,
                                   Texture<float> depthBuffer,
                                   IShader<TStrategyOutput, TRenderOutput> adapterShader)
            where TRenderOutput : struct;
    }
}