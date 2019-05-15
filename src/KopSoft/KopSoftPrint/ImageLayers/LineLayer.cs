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
    public class LineLayer : ImageLayerBase
    {
        /// <summary>
        /// 线条宽度
        /// </summary>
        public int lineWidth { get; set; }

        /// <summary>
        /// 线条长度
        /// </summary>
        public int lineLength { get; set; }

        /// <summary>
        /// 线条方向  1 横线  2 竖线
        /// </summary>
        public int lineDirect { get; set; }

        #region 方法

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="w">线条宽度</param>
        /// <param name="l">线条长度</param>
        /// <param name="d">线条方向  1 横线  2 竖线</param>
        public LineLayer(int w, int l, int d)
            : base(LayerType.Line)
        {
            this.lineWidth = w;
            this.lineLength = l;
            this.lineDirect = d;

            //设置控件长宽
            if (d == 1) //横线
            {
                this.Width = l;
                this.Height = w;
            }
            else
            {
                this.Width = w;
                this.Height = l;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //Rectangle rec = new Rectangle(0, 0, this.Width, this.Height);
            using (Pen p = new Pen(Color.Black, this.lineWidth))
            {
                if (this.lineDirect == 1) //横
                {
                    e.Graphics.DrawLine(p, 0, 0, this.lineLength, 0);
                }
                else  //竖
                {
                    e.Graphics.DrawLine(p, 0, 0, 0, this.lineLength);
                }
            }
        }

        public override void DrawRectangle()
        {
            using (Brush p = new SolidBrush(Color.Red))
            {
                int blockWidth = this.lineWidth;
                //绘制2个小方框
                this.CreateGraphics().FillRectangle(p, 0, 0, blockWidth, blockWidth);
                this.CreateGraphics().FillRectangle(p, this.Width - blockWidth, this.Height - blockWidth, blockWidth, blockWidth);
            }
        }

        #endregion 方法
    }
}