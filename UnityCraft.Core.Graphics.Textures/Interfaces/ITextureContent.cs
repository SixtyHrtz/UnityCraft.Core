using System.Drawing;

namespace UnityCraft.Core.Graphics.Textures.Interfaces
{
    internal interface ITextureContent
    {
        Color[,] GetPixels(int mipmapLevel);
    }
}
