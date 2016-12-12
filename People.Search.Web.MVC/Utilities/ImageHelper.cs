using System.Drawing;
using System.IO;

namespace HealthCatalyst.Utilities
{
    public class ImageHelper
    {
        public byte[] BitmapToByteArray(Bitmap img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}