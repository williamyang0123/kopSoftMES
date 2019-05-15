using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace KopSoft.KopSoftPrint
{
    /// <summary>
    /// 页面大小、边距等参数设置
    /// </summary>
    public class PagerSetting
    {
        public static bool IsPagerLayerCreated = false;

        /// <summary>
        /// 页面宽度 单位px 像素
        /// </summary>
        public static int PageWidth = 0;

        /// <summary>
        /// 页面高度 单位px 像素
        /// </summary>
        public static int PageHeight = 0;

        /// <summary>
        /// 页面边距 单位px 像素
        /// </summary>
        public static int PagePadding = 0;

        /// <summary>
        /// 编辑器中最后选中的对象
        /// </summary>
        public static ImageLayerBase LastActiveLayer = null;

        /// <summary>
        /// 1英寸=25.4毫米
        /// </summary>
        private const double millimererTopixel = 25.4;

        /// <summary>
        /// 毫米转换成像素
        /// </summary>
        /// <param name="handle">父窗体handle</param>
        /// <param name="length">length是毫米</param>
        /// <param name="direct">1代表x方向  2代表y方向</param>
        /// <returns></returns>
        public static double MillimeterToPixel(IntPtr handle, int length, int direct)
        {
            //System.Windows.Forms.Panel p = new System.Windows.Forms.Panel();
            System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(handle);
            float dpi = g.DpiX;
            if (direct == 2)
            {
                dpi = g.DpiY;
            }
            //1英寸=25.4mm=96DPI，那么1mm=96/25.4DPI
            return (((double)dpi / millimererTopixel) * (double)length);
        }

        /// <summary>
        /// 变量释义转换
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Translate(string source)
        {
            Regex reg = new Regex(@"\{\w+\}");
            if (!reg.IsMatch(source))
            {
                return source;
            }
            string result = source;
            foreach (Match v in reg.Matches(source))
            {
                switch (v.Value)
                {
                    case "{当前时间}":
                        result = result.Replace("{当前时间}", DateTime.Now.ToString("HH:mm:ss"));
                        break;

                    case "{当前日期}":
                        result = result.Replace("{当前日期}", DateTime.Now.ToString("yyyy-MM-dd"));
                        break;

                    case "{产品名称}":
                        string ProductName = KopSoftPrint.ListSource?[KopSoftPrint.rowIndex].ProductName;
                        //string ProductName = Global.ProductName;

                        //dataGridView1

                        result = result.Replace("{产品名称}", ProductName);
                        break;

                    case "{产品编码}":
                        string ProductCode = KopSoftPrint.ListSource?[KopSoftPrint.rowIndex].ProductCode;
                        //string ProductCode = Global.ProductCode;
                        result = result.Replace("{产品编码}", ProductCode);
                        break;

                    case "{产品价格}":
                        string ProductPrice = KopSoftPrint.ListSource?[KopSoftPrint.rowIndex].ProductPrice;
                        //string ProductPrice = Global.ProductPrice;
                        result = result.Replace("{产品价格}", ProductPrice);
                        break;

                    case "{产品单位}":
                        string ProductUnit = KopSoftPrint.ListSource?[KopSoftPrint.rowIndex].ProductUnit;
                        result = result.Replace("{产品单位}", ProductUnit);
                        break;

                    case "{产品规格}":
                        string ProductSize = KopSoftPrint.ListSource?[KopSoftPrint.rowIndex].ProductSize;
                        result = result.Replace("{产品规格}", ProductSize);
                        break;

                    case "{产品颜色}":
                        //string ProductColor = null;
                        string ProductColor = KopSoftPrint.ListSource?[KopSoftPrint.rowIndex].ProductColor;
                        result = result.Replace("{产品颜色}", ProductColor);
                        break;

                    case "{产品供应商}":
                        //string ProductSupplier = null;
                        string ProductSupplier = KopSoftPrint.ListSource?[KopSoftPrint.rowIndex].ProductSupplier;
                        result = result.Replace("{产品供应商}", ProductSupplier);
                        break;

                    case "{产品批次}":
                        //string ProductBatch = null;
                        string ProductBatch = KopSoftPrint.ListSource?[KopSoftPrint.rowIndex].ProductBatch;
                        result = result.Replace("{产品批次}", ProductBatch);
                        break;

                    case "{生产日期}":
                        //string StartDate = null;
                        string StartDate = KopSoftPrint.ListSource?[KopSoftPrint.rowIndex].StartDate;
                        result = result.Replace("{生产日期}", StartDate);
                        break;

                    case "{过期时间}":
                        //string EndTime = null;
                        string EndTime = KopSoftPrint.ListSource?[KopSoftPrint.rowIndex].EndTime;
                        result = result.Replace("{过期时间}", EndTime);
                        break;

                    case "{备注}":
                        //string Remark = null;
                        string Remark = KopSoftPrint.ListSource?[KopSoftPrint.rowIndex].Remark;
                        result = result.Replace("{备注}", Remark);
                        break;

                    case "{地址}":
                        //string Address = null;
                        string Address = KopSoftPrint.ListSource?[KopSoftPrint.rowIndex].Address;
                        result = result.Replace("{地址}", Address);
                        break;

                    case "{公司名称}":
                        //string CompanyName = null;
                        string CompanyName = KopSoftPrint.ListSource?[KopSoftPrint.rowIndex].CompanyName;
                        result = result.Replace("{公司名称}", CompanyName);
                        break;

                    case "{商标}":
                        //string Logo = null;
                        string Logo = KopSoftPrint.ListSource?[KopSoftPrint.rowIndex].Logo;
                        result = result.Replace("{商标}", Logo);
                        break;

                    default: break;
                }
            }
            return result;
        }
    }
}