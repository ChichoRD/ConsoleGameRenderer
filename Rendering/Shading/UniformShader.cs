namespace ConsoleGameRenderer.Rendering.Shading
{
    internal delegate bool UniformsAssignation<T>(string id, object? passedUniforms, T? storedUniforms, out T? assigned);
    internal class UniformShader<TUniforms, TIn, TOut> : IShader<TIn, TOut>
    {
        private TUniforms? _uniforms;
        private readonly Func<TUniforms?, TIn, TOut> _shader;
        private readonly UniformsAssignation<TUniforms> _uniformsProcessing;

        public UniformShader(Func<TUniforms?, TIn, TOut> shader)
        {
            _shader = shader;
            _uniformsProcessing = (string id, object? passed, TUniforms? stored, out TUniforms? assigned) =>
            {
                if (passed is not TUniforms u)
                {
                    assigned = stored;
                    return false;
                }

                assigned = u;
                return true;
            };

            _uniforms = default;
        }

        public UniformShader(Func<TUniforms?, TIn, TOut> shader, UniformsAssignation<TUniforms> uniformsAssignation)
        {
            _shader = shader;
            _uniformsProcessing = uniformsAssignation;

            _uniforms = default;
        }

        public TOut Shade(TIn input) => _shader(_uniforms, input);
        public bool TryPassUniforms<T>(string id, T uniforms) => _uniformsProcessing(id, uniforms, _uniforms, out _uniforms);
    }
}
