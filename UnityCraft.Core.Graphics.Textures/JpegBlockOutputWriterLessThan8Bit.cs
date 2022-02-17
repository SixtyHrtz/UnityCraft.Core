using System;
using System.Runtime.CompilerServices;
using JpegLibrary;
using UnityCraft.Core.Extensions;

namespace UnityCraft.Core.Graphics.Textures
{
    internal class JpegBlockOutputWriterLessThan8Bit : JpegBlockOutputWriter
    {
        private const int TargetBitCount = 8;

        private readonly int width;
        private readonly int height;
        private readonly int precision;
        private readonly int componentsCount;

        private readonly Memory<byte> destination;

        public JpegBlockOutputWriterLessThan8Bit(JpegDecoder decoder, Memory<byte> destination)
        {
            if (destination.Length < (decoder.Width * decoder.Height * decoder.NumberOfComponents))
            {
                throw new ArgumentException("Destination buffer is too small.");
            }

            if (decoder.Precision > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(decoder.Precision));
            }

            width = decoder.Width;
            height = decoder.Height;
            precision = decoder.Precision;
            componentsCount = decoder.NumberOfComponents;

            this.destination = destination;
        }

        public override void WriteBlock(ref short blockRef, int componentIndex, int x, int y)
        {
            if (x > width || y > height)
            {
                throw new IndexOutOfRangeException();
            }

            var max = (1 << precision) - 1;

            var writeWidth = Math.Min(width - x, 8);
            var writeHeight = Math.Min(height - y, 8);

            var span = destination.Span;
            var baseOffset = (y * width * componentsCount) + (x * componentsCount) + componentIndex;

            for (var destY = 0; destY < writeHeight; destY++)
            {
                var offset = baseOffset + (destY * width * componentsCount);

                for (var destX = 0; destX < writeWidth; destX++)
                {
                    var value = MathHelper.Clamp(Unsafe.Add(ref blockRef, destX), 0, max);
                    span[offset] = (byte)ExpandBits((uint)value, precision);

                    offset += componentsCount;
                }

                blockRef = ref Unsafe.Add(ref blockRef, 8);
            }
        }

        private static uint ExpandBits(uint bits, int bitCount)
        {
            var currentBitCount = bitCount;

            while (currentBitCount < TargetBitCount)
            {
                bits = (bits << bitCount) | bits;
                currentBitCount += bitCount;
            }

            if (currentBitCount > TargetBitCount)
            {
                bits >>= bitCount;
                currentBitCount -= bitCount;
                bits = FastExpandBits(bits, currentBitCount);
            }

            return bits;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint FastExpandBits(uint bits, int bitCount)
        {
            var remainingBits = TargetBitCount - bitCount;
            return (bits << remainingBits) | (bits & ((uint)(1 << remainingBits) - 1));
        }
    }
}
