using System.Drawing;

namespace UnityCraft.Core.Graphics.Textures
{
    internal interface ITextureContent
    {
        Color[,] GetPixels(int mipmapLevel);
    }
}
