using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KopSoft.KopSoftPrint
{
    /// <summary>
    /// 图片类图层
    /// </summary>
    public class ImageLayer : ImageLayerBase
    {
        /// <summary>
        /// 图片
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImagePath { get; set; }

        #region 方法

        /// <summary>
        /// 构造函数  创建图片层
        /// </summary>
        /// <param name="img">图片</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        public ImageLayer(string imgPath, int width, int height) : base(LayerType.Image)
        {
            this.Width = width;
            this.Height = height;
            this.Image = Image.FromFile(imgPath);
            this.ImagePath = imgPath;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //放在最后面
            base.OnPaint(e);

            Graphics g = e.Graphics;
            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);
            //绘制图片
            g.DrawImage(this.Image, 0, 0, this.Width, this.Height);

            //画选中框
            if (isActive)
            {
                DrawRectangle();
            }
        }

        #endregion 方法
    }
}