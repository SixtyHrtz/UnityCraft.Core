using System;
using System.Drawing;
using System.IO;

namespace UnityCraft.Core.Extensions
{
    public static class BinaryReaderExtensions
    {
        public static T ReadUInt32<T>(this BinaryReader reader) where T : Enum
        {
            var result = (T)(object)reader.ReadUInt32();

            var isDefined = Enum.IsDefined(typeof(T), result);
            if (!isDefined)
            {
                throw new InvalidDataException($"Value {result} is not defined for enum of type {typeof(T).Name}");
            }

            return result;
        }

        public static Color ReadColorRgba(this BinaryReader reader)
        {
            var red = reader.ReadByte();
            var green = reader.ReadByte();
            var blue = reader.ReadByte();
            var alpha = reader.ReadByte();

            return Color.FromArgb(alpha, red, green, blue);
        }

        public static uint[] ReadUInt32Array(this BinaryReader reader, int count)
        {
            var result = new uint[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = reader.ReadUInt32();
            }

            return result;
        }

        public static Color[] ReadColorRgbaArray(this BinaryReader reader, int count)
        {
            var result = new Color[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = reader.ReadColorRgba();
            }

            return result;
        }
    }
}
