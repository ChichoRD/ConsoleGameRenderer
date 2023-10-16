namespace ConsoleGameRenderer.Rendering.Shading
{
    internal class TransformationShader<TIn, TOut> : IShader<TIn, TOut>
    {
        private readonly Func<TIn, TOut> _shader;

        public TransformationShader(Func<TIn, TOut> shader)
        {
            _shader = shader;
        }

        public TOut Shade(TIn input) => _shader(input);
        public bool TryPassUniforms<TUniforms>(string id, TUniforms uniforms) => true;
    }
}
