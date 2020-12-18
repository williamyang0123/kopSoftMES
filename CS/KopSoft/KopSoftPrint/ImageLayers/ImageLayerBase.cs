using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KopSoft.KopSoftPrint
{
    /// <summary>
    /// 图层基类
    /// </summary>
    public class ImageLayerBase : UserControl
    {
        #region 属性

        public bool bCanSelect { get; set; }

        /// <summary>
        /// 图层名称
        /// </summary>
        public string layerName { get; set; }

        /// <summary>
        /// 是否激活  为当前操作控件
        /// </summary>
        public bool isActive { get; set; }

        /// <summary>
        /// 图层类型 Image  Text  Line   BarCode  QRCode
        /// </summary>
        public LayerType layerType { get; set; }

        #endregion 属性

        #region 方法

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="layt"></param>
        public ImageLayerBase(LayerType layt)
        {
            this.layerType = layt;
            this.bCanSelect = true;//默认为可选择
        }

        /// <summary>
        /// 激活图层时画框选中图层
        /// </summary>
        public virtual void DrawRectangle()
        {
            using (Brush p = new SolidBrush(Color.Blue))
            {
                int blockWidth = 5;
                //绘制8个小方框
                this.CreateGraphics().FillRectangle(p, 0, 0, blockWidth, blockWidth);
                this.CreateGraphics().FillRectangle(p, (this.Width - blockWidth) / 2, 0, blockWidth, blockWidth);
                this.CreateGraphics().FillRectangle(p, this.Width - blockWidth, 0, blockWidth, blockWidth);
                this.CreateGraphics().FillRectangle(p, 0, (this.Height - blockWidth) / 2, blockWidth, blockWidth);
                this.CreateGraphics().FillRectangle(p, 0, this.Height - blockWidth, blockWidth, blockWidth);
                this.CreateGraphics().FillRectangle(p, this.Width - blockWidth, this.Height - blockWidth, blockWidth, blockWidth);
                this.CreateGraphics().FillRectangle(p, (this.Width - blockWidth) / 2, this.Height - blockWidth, blockWidth, blockWidth);
                this.CreateGraphics().FillRectangle(p, this.Width - blockWidth, (this.Height - blockWidth) / 2, blockWidth, blockWidth);
            }
        }

        /// <summary>
        /// 获得焦点时绘制选中边框
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            if (!bCanSelect) return;

            if (PagerSetting.LastActiveLayer != null && PagerSetting.LastActiveLayer != this)
            {
                PagerSetting.LastActiveLayer.isActive = false;
                PagerSetting.LastActiveLayer.Refresh();
            }
            isActive = true;
            DrawRectangle();
            PagerSetting.LastActiveLayer = this;
        }

        private Point mouseDownPoint;
        private bool isBeginDrag = false;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (!bCanSelect) return;

            isBeginDrag = true;
            mouseDownPoint.X = e.X;
            mouseDownPoint.Y = e.Y;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!bCanSelect) return;

            if (isBeginDrag)
            {
                this.Left = this.Left + e.X - mouseDownPoint.X;
                this.Top = this.Top + e.Y - mouseDownPoint.Y;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (!bCanSelect) return;

            if (isBeginDrag)
            {
                isBeginDrag = false;
            }
        }

        /// <summary>
        /// 失去焦点时重绘图层
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            if (!bCanSelect) return;

            this.Invalidate();
            isActive = false;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Delete && this.layerType != LayerType.Paper)
            {
                Parent.Controls.Remove(this);
            }
        }

        #endregion 方法
    }

    /// <summary>
    /// 图层类型 Image  Text  Line   BarCode  QRCode  Paper
    /// </summary>
    public enum LayerType
    {
        Image,
        Text,
        Line,
        BarCode,
        Paper
    }
}