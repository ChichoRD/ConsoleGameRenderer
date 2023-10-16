using ConsoleGameRenderer.Component;

namespace ConsoleGameRenderer.Rendering.Pipeline
{
    internal class Renderer<TColor> : IRenderer
        where TColor : struct
    {
        private readonly TColor _clearingColor;

        public Renderer(TColor clearingColor)
        {
            _clearingColor = clearingColor;
        }

        public void Render(IRenderingContext renderingContext, in CameraBundle cameraBundle)
        {
            renderingContext.ScheduleClear(true, true, _clearingColor);
            renderingContext.ScheduleSkyboxDrawing(cameraBundle);

            var visibleRenderables = renderingContext.GetVisibleRenderables<TColor>(cameraBundle);
            renderingContext.ScheduleRenderersDrawing(visibleRenderables, cameraBundle);

            renderingContext.Submit();
        }
    }
}
