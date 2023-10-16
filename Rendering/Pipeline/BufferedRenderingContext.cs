using ConsoleGameRenderer.Component;
using ConsoleGameRenderer.Rendering.Shading;
using System.Numerics;

namespace ConsoleGameRenderer.Rendering.Pipeline
{
    internal class BufferedRenderingContext<TBufferColor> : IRenderingContext
        where TBufferColor : struct
    {
        private readonly CommandBuffer _commandBuffer;
        private readonly Texture<TBufferColor> _colorBuffer;
        private readonly Texture<float> _depthBuffer;

        private readonly static Material<Vector2, float> s_clearDepthMaterial = new(new UniformShader<float, Vector2, float>((d, _) => d));
        private readonly static Material<Vector2, TBufferColor> s_clearColorMaterial = new(new UniformShader<TBufferColor, Vector2, TBufferColor>((c, _) => c));
        private readonly static Material<Vector2, TBufferColor> s_copyColorMaterial = new(new UniformShader<Texture<TBufferColor>, Vector2, TBufferColor>((c, uv) => c.SampleTexture(uv)));
        private readonly static Material<Vector2, float> s_copyDepthMaterial = new(new UniformShader<Texture<float>, Vector2, float>((d, uv) => d.SampleTexture(uv)));

        public BufferedRenderingContext(Texture<TBufferColor> colorBuffer)
        {
            _colorBuffer = colorBuffer;
            _depthBuffer = new Texture<float>(colorBuffer.Width, colorBuffer.Height);

            _commandBuffer = new CommandBuffer();
        }

        public BufferedRenderingContext(Texture<TBufferColor> colorBuffer, Texture<float> depthBuffer)
        {
            if (colorBuffer.Width != depthBuffer.Width || colorBuffer.Height != depthBuffer.Height)
                throw new ArgumentException("Color and depth buffers must have the same dimensions.");

            _colorBuffer = colorBuffer;
            _depthBuffer = depthBuffer;

            _commandBuffer = new CommandBuffer();
        }

        public void ExecuteCommandBuffer(CommandBuffer commandBuffer) => commandBuffer.Execute();

        public IEnumerable<IRenderable<TColor>> GetVisibleRenderables<TColor>(in CameraBundle cameraBundle)
            where TColor : struct
        {
            throw new NotImplementedException();
        }

        public void ScheduleClear<TColor>(bool clearColor, bool clearDepth, TColor clearingColor)
            where TColor : struct
        {
            Texture<TBufferColor> temporaryColorBuffer = new Texture<TBufferColor>(_colorBuffer.Width, _colorBuffer.Height);
            Texture<float> temporaryDepthBuffer = new Texture<float>(_depthBuffer.Width, _depthBuffer.Height);

            _commandBuffer.Copy(_colorBuffer, temporaryColorBuffer);
            _commandBuffer.Copy(_depthBuffer, temporaryDepthBuffer);

            s_copyColorMaterial.TryPassUniforms(string.Empty, temporaryColorBuffer);
            s_clearColorMaterial.TryPassUniforms(string.Empty, clearingColor);

            s_copyDepthMaterial.TryPassUniforms(string.Empty, temporaryDepthBuffer);
            s_clearDepthMaterial.TryPassUniforms(string.Empty, 1.0f);

            Action command = (clearColor, clearDepth) switch
            {
                (true, true) => () =>
                {
                    _commandBuffer.Blit(temporaryColorBuffer, _colorBuffer, s_clearColorMaterial);
                    _commandBuffer.Blit(temporaryDepthBuffer, _depthBuffer, s_clearDepthMaterial);
                },
                (true, false) => () =>
                {
                    _commandBuffer.Blit(temporaryColorBuffer, _colorBuffer, s_clearColorMaterial);
                    _commandBuffer.Blit(temporaryDepthBuffer, _depthBuffer, s_copyDepthMaterial);
                },
                (false, true) => () =>
                {
                    _commandBuffer.Blit(temporaryColorBuffer, _colorBuffer, s_copyColorMaterial);
                    _commandBuffer.Blit(temporaryDepthBuffer, _depthBuffer, s_clearDepthMaterial);
                },
                (false, false) => () =>
                {
                    _commandBuffer.Blit(temporaryColorBuffer, _colorBuffer, s_copyColorMaterial);
                    _commandBuffer.Blit(temporaryDepthBuffer, _depthBuffer, s_copyDepthMaterial);
                },
            };
            command();
        }

        public void ScheduleRenderersDrawing<TColor>(IEnumerable<IRenderable<TColor>> renderables, in CameraBundle cameraBundle)
            where TColor : struct
        {
            foreach (var renderable in renderables)
            {
                var renderingStrategy = renderable.GetRenderingStrategy();
                renderingStrategy.Render(_commandBuffer,
                                         cameraBundle,
                                         _colorBuffer,
                                         _depthBuffer,
                                         new TransformationShader<TColor, TBufferColor>((c) => c is TBufferColor b ? b : default));
            }
        }

        public void ScheduleSkyboxDrawing(in CameraBundle cameraBundle)
        {
            throw new NotImplementedException();
        }

        public void Submit() => ExecuteCommandBuffer(_commandBuffer);
    }
}
