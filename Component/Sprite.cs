using System.Numerics;

namespace ConsoleGameRenderer.Component
{
    internal readonly record struct Sprite<TSpriteElement>(Texture<TSpriteElement> Texture, bool DrawCentered = false, bool DrawFlippedX = false, bool DrawFlippedY = false)
        where TSpriteElement : struct
    {
        public TSpriteElement SampleSprite(Vector2 uv)
        {
            if (DrawCentered)
                uv -= new Vector2(0.5f, 0.5f);

            if (DrawFlippedX)
                uv.X = 1.0f - uv.X;

            if (DrawFlippedY)
                uv.Y = 1.0f - uv.Y;

            return Texture.SampleTexture(uv);
        }
    }
}