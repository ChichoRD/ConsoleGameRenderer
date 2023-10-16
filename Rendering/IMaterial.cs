using ConsoleGameRenderer.Rendering.Shading;

namespace ConsoleGameRenderer.Rendering
{
    internal interface IMaterial<in TIn, out TOut>
    {
        TOut ApplyShader(TIn input);
        IMaterial<TIn, VOut> AddShader<VOut>(IShader<TOut, VOut> shader);
        bool TryPassUniforms<TUniforms>(string id, TUniforms uniforms);
    }
}