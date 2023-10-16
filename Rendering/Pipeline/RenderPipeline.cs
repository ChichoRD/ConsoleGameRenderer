using ConsoleGameRenderer.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameRenderer.Rendering.Pipeline
{
    internal class RenderPipeline : IRenderPipeline
    {
        private readonly IRenderer _renderer;

        public RenderPipeline(IRenderer renderer)
        {
            _renderer = renderer;
        }

        public void Render(IRenderingContext renderingContext, IList<CameraBundle> cameras)
        {
            foreach (CameraBundle camera in cameras)
                _renderer.Render(renderingContext, camera);
        }
    }
}
