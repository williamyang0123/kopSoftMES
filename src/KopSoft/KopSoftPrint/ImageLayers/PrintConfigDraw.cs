using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.Rendering;

namespace KopSoft.KopSoftPrint
{
    /// <summary>
    /// 从配置还原到图像
    /// </summary>
    public class PrintConfigDraw
    {
        public static Image Draw(PrintConfig cfg)
        {
            if (cfg == null || cfg.Layers == null || cfg.Layers.Count == 0) return null;
            //根据配置还原图
            Image img = new Bitmap(cfg.PageWidth, cfg.PageHeight);
            Graphics g = Graphics.FromImage(img);
            for (int i = cfg.Layers.Count - 1; i >= 0; i--)
            {
                string layertype = cfg.Layers[i].GetType().ToString();
                switch (layertype.Substring(layertype.LastIndexOf(".") + 1))
                {
                    case "BarCodeLayerConfig":
                        BarCodeLayerConfig bc = (BarCodeLayerConfig)cfg.Layers[i];
                        BarcodeFormat myBarcodeFormat;
                        EncodingOptions myEncoding;
                        if (bc.CodeType == 1)//二维码
                        {
                            myBarcodeFormat = BarcodeFormat.QR_CODE;
                            myEncoding = new QrCodeEncodingOptions()
                            {
                                Height = bc.Height,
                                Width = bc.Width,
                                Margin = 0,
                                CharacterSet = "UTF-8",
                                PureBarcode = !bc.IsShowText
                            };
                        }
                        else//条形码
                        {
                            myBarcodeFormat = BarcodeFormat.CODE_128;
                            myEncoding = new EncodingOptions()
                            {
                                Height = bc.Height,
                                Width = bc.Width,
                                Margin = 0,
                                PureBarcode = !bc.IsShowText
                            };
                        }
                        BarcodeWriter writer = new BarcodeWriter
                        {
                            Format = myBarcodeFormat,
                            Options = myEncoding,
                            Renderer = (IBarcodeRenderer<Bitmap>)Activator.CreateInstance(typeof(BitmapRenderer))
                        };
                        Bitmap barImg = writer.Write(PagerSetting.Translate(bc.Content));
                        g.DrawImage(barImg, bc.X, bc.Y, bc.Width, bc.Height);
                        break;

                    case "ImageLayerConfig":
                        ImageLayerConfig imglc = (ImageLayerConfig)cfg.Layers[i];
                        g.DrawImage(Image.FromFile(imglc.ImageFilePath), imglc.X, imglc.Y, imglc.Width, imglc.Height);
                        break;

                    case "TextLayerConfig":
                        TextLayerConfig tlc = (TextLayerConfig)cfg.Layers[i];
                        //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;//文本失真

                        g.DrawString(PagerSetting.Translate(tlc.Content), tlc.ContentFont, new SolidBrush(tlc.ContentColor), new PointF(tlc.X, tlc.Y));
                        break;

                    case "LineLayerConfig":
                        LineLayerConfig lc = (LineLayerConfig)cfg.Layers[i];
                        using (Pen p = new Pen(Color.Black, lc.lineWidth))
                        {
                            if (lc.lineDirect == 1)//横
                            {
                                g.DrawLine(p, lc.X, lc.Y, lc.X + lc.lineLength, lc.Y);
                            }
                            else//竖
                            {
                                g.DrawLine(p, lc.X, lc.Y, lc.X, lc.Y + lc.lineLength);
                            }
                        }
                        break;

                    default: break;
                }
            }
            return img;
        }


        public static void Draw(Graphics g, PrintConfig cfg)
        {
            if (cfg == null || cfg.Layers == null || cfg.Layers.Count == 0) return;
            //根据配置还原图
            Image img = new Bitmap(cfg.PageWidth, cfg.PageHeight);
            //文字抗锯齿降噪处理
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PageUnit = GraphicsUnit.Display;

            //Graphics g = Graphics.FromImage(img);
            for (int i = cfg.Layers.Count - 1; i >= 0; i--)
            {
                string layertype = cfg.Layers[i].GetType().ToString();
                switch (layertype.Substring(layertype.LastIndexOf(".") + 1))
                {
                    case "BarCodeLayerConfig":
                        BarCodeLayerConfig bc = (BarCodeLayerConfig)cfg.Layers[i];
                        BarcodeFormat myBarcodeFormat;
                        EncodingOptions myEncoding;
                        if (bc.CodeType == 1)//二维码
                        {
                            myBarcodeFormat = BarcodeFormat.QR_CODE;
                            myEncoding = new QrCodeEncodingOptions()
                            {
                                Height = bc.Height,
                                Width = bc.Width,
                                Margin = 0,
                                CharacterSet = "UTF-8",
                                PureBarcode = !bc.IsShowText
                            };
                        }
                        else//条形码
                        {
                            myBarcodeFormat = BarcodeFormat.CODE_128;
                            myEncoding = new EncodingOptions()
                            {
                                Height = bc.Height,
                                Width = bc.Width,
                                Margin = 0,
                                PureBarcode = !bc.IsShowText
                            };
                        }
                        BarcodeWriter writer = new BarcodeWriter
                        {
                            Format = myBarcodeFormat,
                            Options = myEncoding,
                            Renderer = (IBarcodeRenderer<Bitmap>)Activator.CreateInstance(typeof(BitmapRenderer))
                        };
                        Bitmap barImg = writer.Write(PagerSetting.Translate(bc.Content));
                        g.DrawImage(barImg, bc.X, bc.Y, bc.Width, bc.Height);
                        break;

                    case "ImageLayerConfig":
                        ImageLayerConfig imglc = (ImageLayerConfig)cfg.Layers[i];
                        g.DrawImage(Image.FromFile(imglc.ImageFilePath), imglc.X, imglc.Y, imglc.Width, imglc.Height);
                        break;

                    case "TextLayerConfig":
                        TextLayerConfig tlc = (TextLayerConfig)cfg.Layers[i];
                        //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;//文本失真

                        g.DrawString(PagerSetting.Translate(tlc.Content), tlc.ContentFont, new SolidBrush(tlc.ContentColor), new PointF(tlc.X, tlc.Y));
                        break;

                    case "LineLayerConfig":
                        LineLayerConfig lc = (LineLayerConfig)cfg.Layers[i];
                        using (Pen p = new Pen(Color.Black, lc.lineWidth))
                        {
                            if (lc.lineDirect == 1)//横
                            {
                                g.DrawLine(p, lc.X, lc.Y, lc.X + lc.lineLength, lc.Y);
                            }
                            else//竖
                            {
                                g.DrawLine(p, lc.X, lc.Y, lc.X, lc.Y + lc.lineLength);
                            }
                        }
                        break;

                    default: break;
                }
            }
            //return img;
        }

        /// <summary>
        /// 从配置还原到控件
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="parentCtl"></param>
        public static void ReturnContrl(PrintConfig cfg, Control parentCtl)
        {
            if (cfg == null || cfg.Layers == null || cfg.Layers.Count == 0) return;
            parentCtl.Controls.Clear();
            PagerSetting.PageHeight = cfg.PageHeight;
            PagerSetting.PageWidth = cfg.PageWidth;
            PagerSetting.PagePadding = cfg.PagePadding;
            //根据配置还原图
            PageLayer pl = new PageLayer(PagerSetting.PageWidth, PagerSetting.PageHeight, PagerSetting.PagePadding);
            parentCtl.Controls.Add(pl);
            //计算控件起始坐标
            int startX = (int)((parentCtl.Width - PagerSetting.PageWidth) / 2);
            int startY = (int)((parentCtl.Height - PagerSetting.PageHeight) / 2);
            pl.Left = startX;
            pl.Top = startY;

            for (int i = cfg.Layers.Count - 1; i >= 0; i--)
            {
                string layertype = cfg.Layers[i].GetType().ToString();
                switch (layertype.Substring(layertype.LastIndexOf(".") + 1))
                {
                    case "BarCodeLayerConfig":
                        BarCodeLayerConfig bc = (BarCodeLayerConfig)cfg.Layers[i];
                        BarCodeLayer barlayer = new BarCodeLayer(bc.Content, bc.Width, bc.Height, bc.CodeType, bc.IsShowText);
                        parentCtl.Controls.Add(barlayer);
                        barlayer.BringToFront();  //一定要加这句将层放到最上面  不然默认是放在最下面的
                        barlayer.Left = bc.X + startX;
                        barlayer.Top = bc.Y + startY;
                        break;

                    case "ImageLayerConfig":
                        ImageLayerConfig imglc = (ImageLayerConfig)cfg.Layers[i];
                        ImageLayer imglayer = new ImageLayer(imglc.ImageFilePath, imglc.Width, imglc.Height);
                        parentCtl.Controls.Add(imglayer);
                        imglayer.BringToFront();  //一定要加这句将层放到最上面  不然默认是放在最下面的
                        imglayer.Left = imglc.X + startX;
                        imglayer.Top = imglc.Y + startY;
                        break;

                    case "TextLayerConfig":
                        TextLayerConfig tlc = (TextLayerConfig)cfg.Layers[i];
                        TextLayer tl = new TextLayer(tlc.Content, true, tlc.ContentFont, tlc.ContentColor);
                        parentCtl.Controls.Add(tl);
                        tl.BringToFront();  //一定要加这句将层放到最上面  不然默认是放在最下面的
                        tl.Left = tlc.X + startX;
                        tl.Top = tlc.Y + startY;
                        break;

                    case "LineLayerConfig":
                        LineLayerConfig lc = (LineLayerConfig)cfg.Layers[i];
                        LineLayer ll = new LineLayer(lc.lineWidth, lc.lineLength, lc.lineDirect);
                        parentCtl.Controls.Add(ll);
                        ll.BringToFront();  //一定要加这句将层放到最上面  不然默认是放在最下面的
                        ll.Left = lc.X + startX;
                        ll.Top = lc.Y + startY;
                        break;

                    default: break;
                }
            }
        }
    }
}