using ConsoleGameRenderer.Component;

namespace ConsoleGameRenderer.Rendering.Pipeline
{
    internal interface IRenderingContext
    {
        void ScheduleClear<TColor>(bool clearColor, bool clearDepth, TColor clearingColor)
            where TColor : struct;
        void ScheduleSkyboxDrawing(in CameraBundle cameraBundle);
        void ScheduleRenderersDrawing<TColor>(IEnumerable<IRenderable<TColor>> renderables, in CameraBundle cameraBundle)
            where TColor : struct;

        IEnumerable<IRenderable<TColor>> GetVisibleRenderables<TColor>(in CameraBundle cameraBundle)
            where TColor : struct;
        void ExecuteCommandBuffer(CommandBuffer commandBuffer);
        void Submit();
    }
}