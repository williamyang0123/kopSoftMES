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
    public class PageLayer : ImageLayerBase
    {
        /// <summary>
        /// 页面边距
        /// </summary>
        public int PagePadding { get; set; }

        #region 方法

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="padding">边距</param>
        public PageLayer(int width, int height, int padding)
            : base(LayerType.Paper)
        {
            this.Width = width;
            this.Height = height;
            this.PagePadding = padding;
            this.bCanSelect = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //放在最后面
            base.OnPaint(e);

            Rectangle rec = new Rectangle(0, 0, this.Width, this.Height);
            Rectangle xrec = new Rectangle(PagePadding, PagePadding, this.Width - PagePadding * 2, this.Height - PagePadding * 2);
            using (Brush bush = new SolidBrush(Color.White))
            {
                e.Graphics.FillRectangle(bush, rec);
            }
            using (Pen pen = new Pen(Color.Gray, 1))
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
                pen.DashPattern = new float[] { 5, 5 };
                e.Graphics.DrawRectangle(pen, xrec);
            }
        }

        #endregion 方法
    }
}