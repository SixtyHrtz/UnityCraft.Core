using System;
using System.IO;
using System.Text;

namespace UnityCraft.Core.Graphics.Models
{
    public class Model
    {
        private Stream stream;

        public Model(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            this.stream = stream;

            using (var reader = new BinaryReader(stream, Encoding.ASCII, true))
            {

            }
        }
    }
}
