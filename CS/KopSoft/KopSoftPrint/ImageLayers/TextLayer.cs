using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace KopSoft.KopSoftPrint
{
    /// <summary>
    /// 文字类图层
    /// </summary>
    public class TextLayer : ImageLayerBase
    {
        /// <summary>
        /// 原始文字内容  带变量
        /// </summary>
        public string VarContent { get; set; }

        /// <summary>
        /// 文字内容
        /// </summary>
        public string Content { get; set; }

        public TextBox Tbox;
        public Label TLabel;

        #region 方法

        public TextLayer(string content)
            : base(LayerType.Text)
        {
            init(content, false, null, Color.Black);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public TextLayer(string content, bool isSetFont, Font font, Color color)
            : base(LayerType.Text)
        {
            init(content, isSetFont, font, color);
        }

        private void init(string content, bool isSetFont, Font font, Color color)
        {
            this.VarContent = content;//重点属性：内容
            this.BackColor = Color.White;
            this.Content = PagerSetting.Translate(content);  //转换后的内容
            if (isSetFont)
            {
                this.Font = font;  //重点属性：字体
                this.ForeColor = color; //重点属性：颜色    //另外  重点属性：坐标   实时计算
            }

            TLabel = new Label();
            TLabel.AutoSize = true;
            TLabel.Location = new Point(0, 0);
            TLabel.Text = this.Content;
            if (isSetFont)
            {
                TLabel.Font = font;
                TLabel.ForeColor = color;
            }
            TLabel.BackColor = Color.White;
            this.Controls.Add(TLabel);

            Tbox = new TextBox();
            Tbox.Location = new Point(0, 0);
            Tbox.BorderStyle = BorderStyle.None;
            Tbox.Width = TLabel.Width;
            if (isSetFont)
            {
                Tbox.Font = font;
                Tbox.ForeColor = color;
            }
            Tbox.Text = this.VarContent;
            this.Controls.Add(Tbox);
            this.Height = TLabel.Height;
            this.Width = TLabel.Width + 20;
            TLabel.BringToFront();

            TLabel.Click += TLabel_Click;
            Tbox.Leave += Tbox_Leave;
            Tbox.KeyDown += Tbox_KeyDown;
        }

        private void Tbox_Leave(object sender, EventArgs e)
        {
            base.OnLeave(e);

            Tbox.SendToBack();
            this.Content = PagerSetting.Translate(Tbox.Text);
            this.VarContent = Tbox.Text;  //这里要还原
            TLabel.Text = this.Content;
            Tbox.Width = TLabel.Width;
            this.Width = TLabel.Width + 20;
        }

        private void TLabel_Click(object sender, EventArgs e)
        {
            Tbox.BringToFront();
            base.OnEnter(e);
        }

        private void Tbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                Parent.Controls.Remove(this);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //Rectangle rec = new Rectangle(0, 0, this.Width, this.Height);
            //绘制图片
            //e.Graphics.DrawImage(this.Image, 0, 0, this.Width, this.Height);
        }

        #endregion 方法
    }
}