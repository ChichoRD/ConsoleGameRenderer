using ConsoleGameRenderer.Component;

namespace ConsoleGameRenderer.Rendering.Pipeline
{
    internal interface IRenderer
    {
        void Render(IRenderingContext renderingContext, in CameraBundle cameraBundle);
    }
}
