using ConsoleGameRenderer.Rendering.Shading;

namespace ConsoleGameRenderer.Rendering
{
    internal class Material<TIn, TOut> : IMaterial<TIn, TOut>
    {
        private readonly IShader<TIn, TOut> _shader;

        public Material(IShader<TIn, TOut> shader)
        {
            _shader = shader;
        }

        public IMaterial<TIn, VOut> AddShader<VOut>(IShader<TOut, VOut> shader)
        {
            return new Material<TIn, VOut>(new CompositeShader<TIn, TOut, VOut>(_shader, shader));
        }

        public TOut ApplyShader(TIn input) => _shader.Shade(input);
        public bool TryPassUniforms<TUniforms>(string id, TUniforms uniforms) => _shader.TryPassUniforms(id, uniforms);
    }
}