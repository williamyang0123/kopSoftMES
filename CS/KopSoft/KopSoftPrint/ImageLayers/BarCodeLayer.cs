using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.Rendering;

namespace KopSoft.KopSoftPrint
{
    /// <summary>
    /// 图片类图层
    /// </summary>
    public class BarCodeLayer : ImageLayerBase
    {
        /// <summary>
        /// 原始文字内容  带变量
        /// </summary>
        public string VarContent { get; set; }

        /// <summary>
        /// 文字内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 条码类型  1：二维码  2：条形码
        /// </summary>
        public int CodeType { get; set; }

        /// <summary>
        /// 是否显示文字
        /// </summary>
        public bool IsShowText { get; set; }

        #region 方法

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="width">长</param>
        /// <param name="height">高</param>
        /// <param name="codetype">条码类型  1：二维码  2：条形码</param>
        public BarCodeLayer(string content, int width, int height, int codetype, bool show)
: base(LayerType.BarCode)
        {
            this.Width = width;
            this.Height = height;
            this.VarContent = content;
            this.CodeType = codetype;
            this.Content = PagerSetting.Translate(content);
            this.IsShowText = show;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            BarcodeFormat myBarcodeFormat;
            EncodingOptions myEncoding;
            if (this.CodeType == 1)//二维码
            {
                myBarcodeFormat = BarcodeFormat.QR_CODE;
                myEncoding = new QrCodeEncodingOptions()
                {
                    Height = this.Height,
                    Width = this.Width,
                    Margin = 0,
                    CharacterSet = "UTF-8",
                    PureBarcode = !this.IsShowText
                };
            }
            else//条形码
            {
                myBarcodeFormat = BarcodeFormat.CODE_128;
                myEncoding = new EncodingOptions()
                {
                    Height = this.Height,
                    Width = this.Width,
                    Margin = 0,
                    PureBarcode = !this.IsShowText
                };
            }
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = myBarcodeFormat,
                Options = myEncoding,
                Renderer = (IBarcodeRenderer<Bitmap>)Activator.CreateInstance(typeof(BitmapRenderer))
            };
            Bitmap barImg = writer.Write(this.Content);
            e.Graphics.DrawImage(barImg, 0, 0, this.Width, this.Height);
        }

        #endregion 方法
    }
}