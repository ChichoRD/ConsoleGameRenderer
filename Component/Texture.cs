using System.Numerics;

namespace ConsoleGameRenderer.Component
{
    internal readonly record struct Texture<TTextureElement>(TTextureElement[,] TextureElements, WarpMode WarpMode = WarpMode.Clamp)
        where TTextureElement : struct
    {
        public int Width { get; } = TextureElements.GetLength(0);
        public int Height { get; } = TextureElements.GetLength(1);

        public Texture(int width, int height, WarpMode warpMode = WarpMode.Clamp) : this(new TTextureElement[width, height], warpMode)
        {
            Width = width;
            Height = height;
        }

        public TTextureElement GetTextureElement(int x, int y) => WarpMode switch
        {
            WarpMode.Default => IsInBounds(x, y) ? TextureElements[x, y] : default,
            WarpMode.Clamp => TextureElements[Math.Clamp(x, 0, Width - 1), Math.Clamp(y, 0, Height - 1)],
            WarpMode.Warp => TextureElements[(uint)x % Width, (uint)y % Height],
            WarpMode.Mirror => TextureElements[Width - (uint)x % Width, Height - (uint)y % Height],
            _ => TextureElements[Math.Clamp(x, 0, Width - 1), Math.Clamp(y, 0, Height - 1)],
        };
        public TTextureElement SampleTexture(float u, float v) => GetTextureElement((int)(u * Width), (int)(v * Height));
        public TTextureElement SampleTexture(Vector2 uv) => SampleTexture(uv.X, uv.Y);

        private bool IsInBounds(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;
    }
}