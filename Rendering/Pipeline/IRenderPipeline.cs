using ConsoleGameRenderer.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameRenderer.Rendering.Pipeline
{
    internal interface IRenderPipeline
    {
        void Render(IRenderingContext renderingContext, IList<CameraBundle> cameras);
    }
}
