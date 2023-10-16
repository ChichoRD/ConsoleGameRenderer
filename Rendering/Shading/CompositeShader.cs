using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameRenderer.Rendering.Shading
{
    internal class CompositeShader<TIn, T, TOut> : IShader<TIn, TOut>
    {
        private readonly IShader<TIn, T> _shader0;
        private readonly IShader<T, TOut> _shader1;

        public CompositeShader(IShader<TIn, T> shader0, IShader<T, TOut> shader1)
        {
            _shader0 = shader0;
            _shader1 = shader1;
        }

        public TOut Shade(TIn input) => _shader1.Shade(_shader0.Shade(input));
        public bool TryPassUniforms<TUniforms>(string id, TUniforms uniforms) => _shader0.TryPassUniforms(id, uniforms)
                                                                                 & _shader1.TryPassUniforms(id, uniforms);
    }
}
