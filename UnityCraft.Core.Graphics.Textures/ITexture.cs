using System.Drawing;

namespace UnityCraft.Core.Graphics.Textures
{
    public interface ITexture
    {
        uint AlphaBits { get; }

        Size Size { get; }

        int MipmapCount { get; }

        Color[,] GetPixels(int mipmapLevel);

        Color[,] GetPixelsFromBytes(Size size, byte[] bytes);

        byte[] GetRawBytes(int mipmapLevel);
    }
}
