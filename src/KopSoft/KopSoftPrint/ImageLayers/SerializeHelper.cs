using System;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace KopSoft.KopSoftPrint
{
    public class SerializeHelper
    {
        //序列化到xml文件  注意：文件将保存到应用程序同级目录
        public static bool Serialize(Type t, object tValue)
        {
            //序列化
            try
            {
                FileStream fs = new FileStream(t.ToString() + ".xml", FileMode.Create, FileAccess.ReadWrite);
                XmlSerializer xs = new XmlSerializer(t);
                xs.Serialize(fs, tValue);
                fs.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //从xml文件反序列化  注意：文件放在应用程序同级目录
        public static object DeSerialize(Type t)
        {
            if (Global.cbTemplate_SelectedIndex == "默认模板")
            {
                try
                {
                    FileStream fs = new FileStream(t.ToString() + ".xml", FileMode.Open, FileAccess.Read);
                    XmlSerializer xs = new XmlSerializer(t);
                    return xs.Deserialize(fs);
                }
                catch
                {
                    return null;
                }
            }
            else if (Global.cbTemplate_SelectedIndex == "30#20mm")
            {
                try
                {
                    FileStream fs = new FileStream(t.ToString() + "_30#20mm.xml", FileMode.Open, FileAccess.Read);
                    XmlSerializer xs = new XmlSerializer(t);
                    return xs.Deserialize(fs);
                }
                catch
                {
                    return null;
                }
            }
            else if (Global.cbTemplate_SelectedIndex == "30#25mm")
            {
                try
                {
                    FileStream fs = new FileStream(t.ToString() + "_30#25mm.xml", FileMode.Open, FileAccess.Read);
                    XmlSerializer xs = new XmlSerializer(t);
                    return xs.Deserialize(fs);
                }
                catch
                {
                    return null;
                }
            }
            else if (Global.cbTemplate_SelectedIndex == "30#40mm")
            {
                try
                {
                    FileStream fs = new FileStream(t.ToString() + "_30#40mm.xml", FileMode.Open, FileAccess.Read);
                    XmlSerializer xs = new XmlSerializer(t);
                    return xs.Deserialize(fs);
                }
                catch
                {
                    return null;
                }
            }
            else if (Global.cbTemplate_SelectedIndex == "35#20mm")
            {
                try
                {
                    FileStream fs = new FileStream(t.ToString() + "_35#20mm.xml", FileMode.Open, FileAccess.Read);
                    XmlSerializer xs = new XmlSerializer(t);
                    return xs.Deserialize(fs);
                }
                catch
                {
                    return null;
                }
            }
            else if (Global.cbTemplate_SelectedIndex == "40#30mm")
            {
                try
                {
                    FileStream fs = new FileStream(t.ToString() + "_40#30mm.xml", FileMode.Open, FileAccess.Read);
                    XmlSerializer xs = new XmlSerializer(t);
                    return xs.Deserialize(fs);
                }
                catch
                {
                    return null;
                }
            }
            else if (Global.cbTemplate_SelectedIndex == "40#60mm_Var09")
            {
                try
                {
                    FileStream fs = new FileStream(t.ToString() + "_40#60mm_Var09.xml", FileMode.Open, FileAccess.Read);
                    XmlSerializer xs = new XmlSerializer(t);
                    return xs.Deserialize(fs);
                }
                catch
                {
                    return null;
                }
            }
            else if (Global.cbTemplate_SelectedIndex == "40#60mm_Var10")
            {
                try
                {
                    FileStream fs = new FileStream(t.ToString() + "_40#60mm_Var10.xml", FileMode.Open, FileAccess.Read);
                    XmlSerializer xs = new XmlSerializer(t);
                    return xs.Deserialize(fs);
                }
                catch
                {
                    return null;
                }
            }
            else if (Global.cbTemplate_SelectedIndex == "40#60mm_Var11")
            {
                try
                {
                    FileStream fs = new FileStream(t.ToString() + "_40#60mm_Var11.xml", FileMode.Open, FileAccess.Read);
                    XmlSerializer xs = new XmlSerializer(t);
                    return xs.Deserialize(fs);
                }
                catch
                {
                    return null;
                }
            }
            else if (Global.cbTemplate_SelectedIndex == "40#60mm_Var13")
            {
                try
                {
                    FileStream fs = new FileStream(t.ToString() + "_40#60mm_Var13.xml", FileMode.Open, FileAccess.Read);
                    XmlSerializer xs = new XmlSerializer(t);
                    return xs.Deserialize(fs);
                }
                catch
                {
                    return null;
                }
            }
            else if (Global.cbTemplate_SelectedIndex == "40#60mm_Var15")
            {
                try
                {
                    FileStream fs = new FileStream(t.ToString() + "_40#60mm_Var15.xml", FileMode.Open, FileAccess.Read);
                    XmlSerializer xs = new XmlSerializer(t);
                    return xs.Deserialize(fs);
                }
                catch
                {
                    return null;
                }
            }
            else if (Global.cbTemplate_SelectedIndex == "105#50mm")
            {
                try
                {
                    FileStream fs = new FileStream(t.ToString() + "_105#50mm.xml", FileMode.Open, FileAccess.Read);
                    XmlSerializer xs = new XmlSerializer(t);
                    return xs.Deserialize(fs);
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 从xml文件反序列化  指定路径
        /// </summary>
        /// <param name="t"></param>
        /// <param name="cfgPath"></param>
        /// <returns></returns>
        public static object DeSerialize(Type t, string cfgPath)
        {
            if (!File.Exists(cfgPath)) return null;
            try
            {
                FileStream fs = new FileStream(cfgPath, FileMode.Open, FileAccess.Read);
                XmlSerializer xs = new XmlSerializer(t);
                return xs.Deserialize(fs);
            }
            catch
            {
                return null;
            }
        }

        #region 序列化颜色和字体

        public static string SerializeColor(Color color)
        {
            if (color.IsNamedColor)
                return string.Format("{0}:{1}", ColorFormat.NamedColor, color.Name);
            else
                return string.Format("{0}:{1}:{2}:{3}:{4}", ColorFormat.ARGBColor, color.A, color.R, color.G, color.B);
        }

        public static Color DeserializeColor(string color)
        {
            byte a, r, g, b;

            string[] pieces = color.Split(new char[] { ':' });

            ColorFormat colorType = (ColorFormat)Enum.Parse(typeof(ColorFormat), pieces[0], true);

            switch (colorType)
            {
                case ColorFormat.NamedColor:
                    return Color.FromName(pieces[1]);

                case ColorFormat.ARGBColor:
                    a = byte.Parse(pieces[1]);
                    r = byte.Parse(pieces[2]);
                    g = byte.Parse(pieces[3]);
                    b = byte.Parse(pieces[4]);

                    return Color.FromArgb(a, r, g, b);
            }
            return Color.Empty;
        }

        public static XmlFont SerializeFont(Font font)
        {
            return new XmlFont(font);
        }

        public static Font DeserializeFont(XmlFont font)
        {
            return font.ToFont();
        }

        #endregion 序列化颜色和字体
    }
}