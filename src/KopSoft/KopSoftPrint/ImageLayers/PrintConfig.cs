using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;

namespace KopSoft.KopSoftPrint
{
    /// <summary>
    /// 打印配置  用于持久化到xml中  最终只通过此对象就可以还原打印所需的所有参数
    /// </summary>
    [XmlInclude(typeof(ImageLayerConfig))]
    [XmlInclude(typeof(TextLayerConfig))]
    [XmlInclude(typeof(LineLayerConfig))]
    [XmlInclude(typeof(BarCodeLayerConfig))]
    //[XmlRoot("PrintConfig")]
    public class PrintConfig
    {
        //[XmlElement("PageWidth")]
        /// <summary>
        /// 页面宽度 单位px 像素
        /// </summary>
        public int PageWidth = 0;

        /// <summary>
        /// 页面高度
        /// </summary>
        //[XmlElement("PageHeight")]
        public int PageHeight = 0;

        /// <summary>
        /// 页面边距
        /// </summary>
        //[XmlElement("PagePadding")]
        public int PagePadding = 0;

        /// <summary>
        /// 图层
        /// </summary>
        //[XmlElement("Layers")]
        public List<object> Layers;
    }

    [XmlRoot("ImageLayer")]
    public class ImageLayerConfig
    {
        /// <summary>
        /// 图片路径
        /// </summary>
        [XmlAttribute("ImageFilePath")]
        public string ImageFilePath;

        /// <summary>
        /// 图片宽 px
        /// </summary>
        [XmlAttribute("Width")]
        public int Width;

        /// <summary>
        /// 图片高 px
        /// </summary>
        [XmlAttribute("Height")]
        public int Height;

        /// <summary>
        /// x坐标
        /// </summary>
        [XmlAttribute("X")]
        public int X;

        /// <summary>
        /// y坐标
        /// </summary>
        [XmlAttribute("Y")]
        public int Y;
    }

    [XmlRoot("TextLayer")]
    public class TextLayerConfig
    {
        [XmlAttribute("Content")]
        public string Content;

        [XmlIgnore()]
        public Font ContentFont;

        [XmlIgnore()]
        public Color ContentColor;

        [XmlElement("Font")]
        public XmlFont XmlFontObject
        {
            get
            {
                return SerializeHelper.SerializeFont(ContentFont);
            }
            set
            {
                ContentFont = SerializeHelper.DeserializeFont(value);
            }
        }

        [XmlElement("Color")]
        public string XmlColorType
        {
            get
            {
                return SerializeHelper.SerializeColor(ContentColor);
            }
            set
            {
                ContentColor = SerializeHelper.DeserializeColor(value);
            }
        }

        /// <summary>
        /// x坐标
        /// </summary>
        [XmlAttribute("X")]
        public int X;

        /// <summary>
        /// y坐标
        /// </summary>
        [XmlAttribute("Y")]
        public int Y;
    }

    [XmlRoot("LineLayer")]
    public class LineLayerConfig
    {
        /// <summary>
        /// 线条宽度
        /// </summary>
        [XmlAttribute("lineWidth")]
        public int lineWidth;

        /// <summary>
        /// 线条长度
        /// </summary>
        [XmlAttribute("lineLength")]
        public int lineLength;

        /// <summary>
        /// 线条方向  1 横线  2 竖线
        /// </summary>
        [XmlAttribute("lineDirect")]
        public int lineDirect;

        /// <summary>
        /// x坐标
        /// </summary>
        [XmlAttribute("X")]
        public int X;

        /// <summary>
        /// y坐标
        /// </summary>
        [XmlAttribute("Y")]
        public int Y;
    }

    [XmlRoot("BarCodeLayer")]
    public class BarCodeLayerConfig
    {
        /// <summary>
        /// 文字内容
        /// </summary>
        [XmlAttribute("Content")]
        public string Content;

        /// <summary>
        /// 条码类型  1：二维码  2：条形码
        /// </summary>
        [XmlAttribute("CodeType")]
        public int CodeType;

        /// <summary>
        /// 是否显示文字
        /// </summary>
        [XmlAttribute("IsShowText")]
        public bool IsShowText;

        /// <summary>
        /// 图片宽 px
        /// </summary>
        [XmlAttribute("Width")]
        public int Width;

        /// <summary>
        /// 图片高 px
        /// </summary>
        [XmlAttribute("Height")]
        public int Height;

        /// <summary>
        /// x坐标
        /// </summary>
        [XmlAttribute("X")]
        public int X;

        /// <summary>
        /// y坐标
        /// </summary>
        [XmlAttribute("Y")]
        public int Y;
    }

    public struct XmlFont
    {
        public string FontFamily;
        public GraphicsUnit GraphicsUnit;
        public float Size;
        public FontStyle Style;

        public XmlFont(Font f)
        {
            FontFamily = f.FontFamily.Name;
            GraphicsUnit = f.Unit;
            Size = f.Size;
            Style = f.Style;
        }

        public Font ToFont()
        {
            return new Font(FontFamily, Size, Style, GraphicsUnit);
        }
    }

    public enum ColorFormat
    {
        NamedColor,
        ARGBColor
    }
}