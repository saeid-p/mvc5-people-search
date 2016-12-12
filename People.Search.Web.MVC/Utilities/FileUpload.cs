using System;
using System.IO;
using System.Web;

namespace HealthCatalyst.Utilities
{
    public static class FileUpload
    {
        public static byte[] GetBytes(this HttpPostedFileBase file)
        {
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                return binaryReader.ReadBytes(file.ContentLength);
            }
        }

        public static bool HasValidImageFormat(this HttpPostedFileBase file)
        {
            switch (file.ContentType.ToLower())
            {
                case "image/jpg":
                case "image/jpeg":
                case "image/pjpeg":
                case "image/gif":
                case "image/x-png":
                case "image/png":
                    return true;
                default:
                    return TryParseUnknownImageFile(file);
            }
        }

        private static bool TryParseUnknownImageFile(HttpPostedFileBase file)
        {
            try
            {
                using (new System.Drawing.Bitmap(file.InputStream))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}