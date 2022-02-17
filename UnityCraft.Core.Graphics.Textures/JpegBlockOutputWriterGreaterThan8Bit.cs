using System;
using System.Runtime.CompilerServices;
using JpegLibrary;
using UnityCraft.Core.Extensions;

namespace UnityCraft.Core.Graphics.Textures
{
    internal class JpegBlockOutputWriterGreaterThan8Bit : JpegBlockOutputWriter
    {
        private readonly int width;
        private readonly int height;
        private readonly int shift;
        private readonly int componentsCount;

        private readonly Memory<byte> destination;

        public JpegBlockOutputWriterGreaterThan8Bit(JpegDecoder decoder, Memory<byte> destination)
        {
            if (destination.Length < (decoder.Width * decoder.Height * decoder.NumberOfComponents))
            {
                throw new ArgumentException("Destination buffer is too small.");
            }

            if (decoder.Precision < 8)
            {
                throw new ArgumentOutOfRangeException(nameof(decoder.Precision));
            }

            width = decoder.Width;
            height = decoder.Height;
            shift = decoder.Precision - 8;
            componentsCount = decoder.NumberOfComponents;

            this.destination = destination;
        }

        public override void WriteBlock(ref short blockRef, int componentIndex, int x, int y)
        {
            if (x > width || y > height)
            {
                throw new IndexOutOfRangeException();
            }

            var writeWidth = Math.Min(width - x, 8);
            var writeHeight = Math.Min(height - y, 8);

            var span = destination.Span;
            var baseOffset = (y * width * componentsCount) + (x * componentsCount) + componentIndex;

            for (var destY = 0; destY < writeHeight; destY++)
            {
                var offset = baseOffset + (destY * width * componentsCount);

                for (var destX = 0; destX < writeWidth; destX++)
                {
                    span[offset] = ClampTo8Bit(Unsafe.Add(ref blockRef, destX) >> shift);

                    offset += componentsCount;
                }

                blockRef = ref Unsafe.Add(ref blockRef, 8);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ClampTo8Bit(int input)
        {
            return (byte)MathHelper.Clamp(input, byte.MinValue, byte.MaxValue);
        }
    }
}
