using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.QrCode;

namespace KopSoftPrint
{
    internal class QRCode
    {
        public void GenerateQRCode(string contents, PictureBox qrimage)
        {
            BarcodeWriter barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    DisableECI = true,
                    CharacterSet = "UTF-8",
                    Width = qrimage.Width,
                    Height = qrimage.Height,
                    Margin = 1 //二维码边距
                }
            };

            Bitmap bitmap = barcodeWriter.Write(contents);
            qrimage.Image = bitmap;
        }
    }
}