using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using QRCoder;

namespace Util
{
    public static class QR
    {
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