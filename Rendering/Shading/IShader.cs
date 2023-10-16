namespace ConsoleGameRenderer.Rendering.Shading
{
    internal interface IShader<in TIn, out TOut>
    {
        bool TryPassUniforms<TUniforms>(string id, TUniforms uniforms);
        TOut Shade(TIn input);
    }
}
