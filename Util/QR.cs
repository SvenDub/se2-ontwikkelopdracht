using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using QRCoder;

namespace Util
{
    /// <summary>
    ///     Utility class for generating QR codes.
    /// </summary>
    public static class QR
    {
        /// <summary>
        ///     Generates a QR code for an url.
        /// </summary>
        /// <param name="url">The url to encode.</param>
        /// <returns>A base64 encoded PNG.</returns>
        public static string GenerateBase64(string url)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            using (Bitmap bitMap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    return "data:image/png;base64," + Convert.ToBase64String(byteImage);
                }
            }
        }
    }
}