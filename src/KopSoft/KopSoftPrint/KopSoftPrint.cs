using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KopSoft.KopSoftPrint
{
    public partial class KopSoftPrint : Form
    {
        private FontDialog fontDialog = new FontDialog();
        private OpenFileDialog openFileDialog = new OpenFileDialog();
        private PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
        private PrintDocument printDocument = new PrintDocument();
        public static List<ProductEntity> ListSource { get; set; }//public static
        public static int rowIndex = 0;

        public KopSoftPrint()
        {
            InitializeComponent();
        }

        private int m_startX, m_startY; //控件起始放置坐标
        int PageWidth = 0, PageHeight = 0, PageMargins = 0;//毫米为单位

        private void btnInsertBarCode_Click(object sender, EventArgs e)
        {
            if (!PagerSetting.IsPagerLayerCreated)
            {
                MessageBox.Show("请先设置纸张尺寸！");
                return;
            }
            if (string.IsNullOrEmpty(tbBarCodeContent.Text))
            {
                MessageBox.Show("内容不能为空！");
                return;
            }
            if (rbText.Checked)
            {
                TextLayer tl;
                if (hasSetFont)
                {
                    tl = new TextLayer(tbBarCodeContent.Text, true, fontDialog.Font, fontDialog.Color);
                }
                else
                {
                    tl = new TextLayer(tbBarCodeContent.Text);
                }
                panel1.Controls.Add(tl);
                tl.BringToFront();//一定要加这句将层放到最上面，不然默认是放在最下面的
                tl.Left = m_startX;
                tl.Top = m_startY;
            }
            else
            {
                int type = 1;
                if (rbBarCode.Checked)
                {
                    type = 2;
                }
                BarCodeLayer barlayer = new BarCodeLayer(tbBarCodeContent.Text, int.Parse(tbBarWidth.Text), int.Parse(tbBarHeight.Text), type, cbIsShowText.Checked);
                panel1.Controls.Add(barlayer);
                barlayer.BringToFront();//一定要加这句将层放到最上面，不然默认是放在最下面的
                barlayer.Left = m_startX;
                barlayer.Top = m_startY;
            }
        }

        private void rbQrCode_CheckedChanged(object sender, EventArgs e)
        {
            if (rbQrCode.Checked)
            {
                tbBarWidth.Text = "50";
                tbBarHeight.Text = "50";
            }
            else if (rbBarCode.Checked)
            {
                tbBarWidth.Text = "100";
                tbBarHeight.Text = "50";
            }
        }

        private void btnInsertImg_Click(object sender, EventArgs e)
        {
            if (!PagerSetting.IsPagerLayerCreated)
            {
                MessageBox.Show("请先设置纸张尺寸！");
                return;
            }

            ImageLayer imglayer = new ImageLayer(openFileDialog.FileName, int.Parse(tbImageWidth.Text), int.Parse(tbImageHeight.Text));
            panel1.Controls.Add(imglayer);
            imglayer.BringToFront();//一定要加这句将层放到最上面，不然默认是放在最下面的
            imglayer.Left = m_startX;
            imglayer.Top = m_startY;
        }

        private bool hasSetFont = false;

        private void btnSetFont_Click(object sender, EventArgs e)
        {
            fontDialog.ShowColor = true;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(tbBarCodeContent.Text))
                {
                    lbFont.Text = "示例：生产日期";
                }
                lbFont.Font = fontDialog.Font;
                lbFont.ForeColor = fontDialog.Color;
                hasSetFont = true;
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            if (panel1.Controls.Count < 2)
            {
                MessageBox.Show("没有可保存的内容，请设计好模板后再保存配置！");
                return;
            }

            PrintConfig pcongif = new PrintConfig();
            pcongif.PageHeight = PagerSetting.PageHeight;
            pcongif.PageWidth = PagerSetting.PageWidth;
            pcongif.PagePadding = PagerSetting.PagePadding;
            List<object> layerConfigs = new List<object>();

            //先得到纸张属性
            int pageX = 0;
            int pageY = 0;
            if (((ImageLayerBase)panel1.Controls[panel1.Controls.Count - 1]).layerType == LayerType.Paper)
            {
                pageX = panel1.Controls[panel1.Controls.Count - 1].Left;
                pageY = panel1.Controls[panel1.Controls.Count - 1].Top;
            }

            for (int i = 0; i < panel1.Controls.Count; i++)
            {
                LayerType lt = ((ImageLayerBase)panel1.Controls[i]).layerType;
                switch (lt)
                {
                    case LayerType.BarCode:
                        BarCodeLayer blayer = (BarCodeLayer)panel1.Controls[i];
                        BarCodeLayerConfig bc = new BarCodeLayerConfig();
                        bc.X = blayer.Left - pageX;
                        bc.Y = blayer.Top - pageY;
                        bc.Width = blayer.Width;
                        bc.Height = blayer.Height;
                        bc.Content = blayer.VarContent;
                        bc.CodeType = blayer.CodeType;
                        bc.IsShowText = blayer.IsShowText;
                        layerConfigs.Add(bc);
                        break;

                    case LayerType.Image:
                        ImageLayer imglayer = (ImageLayer)panel1.Controls[i];
                        ImageLayerConfig imglc = new ImageLayerConfig();
                        imglc.X = imglayer.Left - pageX;
                        imglc.Y = imglayer.Top - pageY;
                        imglc.Width = imglayer.Width;
                        imglc.Height = imglayer.Height;
                        imglc.ImageFilePath = imglayer.ImagePath;
                        layerConfigs.Add(imglc);
                        break;

                    case LayerType.Text:
                        TextLayer tl = (TextLayer)panel1.Controls[i];
                        TextLayerConfig tlc = new TextLayerConfig();
                        tlc.X = tl.Left - pageX;
                        tlc.Y = tl.Top - pageY;
                        tlc.Content = tl.VarContent;
                        tlc.ContentColor = tl.ForeColor;
                        tlc.ContentFont = tl.Font;
                        layerConfigs.Add(tlc);
                        break;

                    case LayerType.Line:
                        LineLayer ll = (LineLayer)panel1.Controls[i];
                        LineLayerConfig lc = new LineLayerConfig();
                        lc.X = ll.Left - pageX;
                        lc.Y = ll.Top - pageY;
                        lc.lineWidth = ll.lineWidth;
                        lc.lineLength = ll.lineLength;
                        lc.lineDirect = ll.lineDirect;
                        layerConfigs.Add(lc);
                        break;

                    default: break;
                }
            }

            pcongif.Layers = layerConfigs;

            if (SerializeHelper.Serialize(typeof(PrintConfig), pcongif))
            {
                m_printConfig = pcongif;
                MessageBox.Show("保存成功！");
            }
            else
            {
                MessageBox.Show("保存失败！");
            }
        }

        private bool ReadConfig()
        {
            object obj = SerializeHelper.DeSerialize(typeof(PrintConfig));
            if (obj == null)
            {
                MessageBox.Show("读取模板配置文件失败！");
                return false;
            }
            m_printConfig = (PrintConfig)obj;
            m_printImg = PrintConfigDraw.Draw(m_printConfig);
            return true;
        }

        /// <summary>
        /// 读取模板配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            Global.cbTemplate_SelectedIndex = cbTemplate.SelectedItem.ToString();

            string cbTemplateSelectedValue = cbTemplate.SelectedItem.ToString();
            string Message = "模板中有变量，请先设置数据源后，再选择对应模板！";
            if (cbTemplateSelectedValue == "默认模板")
            {
                //m_printImg = null;
                ReadConfig();
                PrintConfigDraw.ReturnContrl(m_printConfig, panel1);
                return;
            }

            if (dataGridView1.CurrentCell == null)
            {
                if (cbTemplateSelectedValue == "30#20mm")
                {
                    MessageBox.Show(Message);
                    return;
                }
                if (cbTemplateSelectedValue == "30#25mm")
                {
                    MessageBox.Show(Message);
                    return;
                }
                if (cbTemplateSelectedValue == "30#40mm")
                {
                    MessageBox.Show(Message);
                    return;
                }
                if (cbTemplateSelectedValue == "35#20mm")
                {
                    MessageBox.Show(Message);
                    return;
                }
                if (cbTemplateSelectedValue == "40#30mm")
                {
                    MessageBox.Show(Message);
                    return;
                }
                if (cbTemplateSelectedValue == "40#60mm_Var09")
                {
                    MessageBox.Show(Message);
                    return;
                }
                if (cbTemplateSelectedValue == "40#60mm_Var10")
                {
                    MessageBox.Show(Message);
                    return;
                }
                if (cbTemplateSelectedValue == "40#60mm_Var11")
                {
                    MessageBox.Show(Message);
                    return;
                }
                if (cbTemplateSelectedValue == "40#60mm_Var13")
                {
                    MessageBox.Show(Message);
                    return;
                }
                if (cbTemplateSelectedValue == "40#60mm_Var15")
                {
                    MessageBox.Show(Message);
                    return;
                }
                if (cbTemplateSelectedValue == "105#50mm")
                {
                    MessageBox.Show(Message);
                    return;
                }
            }
            else
            {
                m_printImg = null;
                ReadConfig();
                PrintConfigDraw.ReturnContrl(m_printConfig, panel1);
            }
        }

        private PrintConfig m_printConfig = null;
        private Image m_printImg = null;

        private StandardPrintController controler = new StandardPrintController();

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                for (int j = 0; j < dataGridView1.SelectedRows.Count; j++)//遍历所有选中的行
                {
                    rowIndex = dataGridView1.SelectedRows[j].Index;

                    if (!ReadConfig())
                    {
                        return;
                    }

                    Margins margin = new Margins(0, 0, 0, 0);
                    printDocument.DefaultPageSettings.Margins = margin;
                    printDocument.DefaultPageSettings.PaperSize = new PaperSize("热敏纸", m_printImg.Width, m_printImg.Height);
                    int Number = Convert.ToInt32(numericUpDown1.Value);
                    try
                    {
                        printDocument.PrintPage += new PrintPageEventHandler(PrintPageEvent);
                        printDocument.PrintController = controler;
                        for (int i = 0; i < Number; i++)
                        {
                            printDocument.Print();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                    finally
                    {
                        printDocument.Dispose();
                    }
                }
            }
            else
            {
                MessageBox.Show("请先导入Excel");
                return;
            }

        }

        public void PrintPageEvent(object Sender, PrintPageEventArgs e)
        {
            if (m_printImg == null) return;

            //在指定区域打印
            Rectangle destRect = new Rectangle(0, 0, m_printConfig.PageWidth, m_printConfig.PageHeight);
            e.Graphics.DrawImage(m_printImg, destRect, 0, 0, m_printImg.Width, m_printImg.Height, GraphicsUnit.Pixel);
            Brush brush = Brushes.Black;

            //string ProductName = "物料名称:" + dataGridView1.Rows[rowIndex].Cells[1].Value.ToString();
            //e.Graphics.DrawString(ProductName, new Font("黑体", 7), brush, 10, 15);

            e.Graphics.DrawString("试用版", new Font("黑体", 5), brush, 5, 5);




            //pictureBox1.Image = m_printImg;
        }

        private void KopSoftPrint_Load(object sender, EventArgs e)
        {
            string sDefault = printDocument.PrinterSettings.PrinterName;//设置默认打印机名
            foreach (string sPrint in PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                cbPrint.Items.Add(sPrint);
                if (sPrint == sDefault) cbPrint.SelectedIndex = cbPrint.Items.IndexOf(sPrint);
            }


            try
            {
                PageMargins = int.Parse(tbPageMargins.Text);//页面边距
                PagerSetting.PagePadding = (int)PagerSetting.MillimeterToPixel(panel1.Handle, PageMargins, 1);

                PageWidth = int.Parse(tbPageWidth.Text);
                PageHeight = int.Parse(tbPageHeight.Text);
                if (PageWidth > 0 && PageHeight > 0)//进行尺码单位毫米（mm）转换成像素（Px）
                {
                    PagerSetting.PageHeight = (int)PagerSetting.MillimeterToPixel(panel1.Handle, PageHeight, 2);
                    PagerSetting.PageWidth = (int)PagerSetting.MillimeterToPixel(panel1.Handle, PageWidth, 1);
                }

                //重绘pager纸张
                PageLayer pl = new PageLayer(PagerSetting.PageWidth, PagerSetting.PageHeight, PagerSetting.PagePadding);
                //先把旧纸张去掉
                if (panel1.Controls.Count > 0)
                {
                    for (int i = 0; i < panel1.Controls.Count; i++)
                    {
                        if (((ImageLayerBase)panel1.Controls[i]).layerType == LayerType.Paper)
                        {
                            panel1.Controls.RemoveAt(i);
                            break;
                        }
                    }
                }
                panel1.Controls.Add(pl);
                PagerSetting.IsPagerLayerCreated = true;

                //计算控件起始坐标
                int startX = (int)((panel1.Width - PagerSetting.PageWidth) / 2);
                int startY = (int)((panel1.Height - PagerSetting.PageHeight) / 2);
                pl.Left = startX;
                pl.Top = startY;
                m_startX = startX + PagerSetting.PagePadding;
                m_startY = startY + PagerSetting.PagePadding;

                lbMessage.Text = "设置成功，纸张尺寸" + PagerSetting.PageWidth + "*" + PagerSetting.PageHeight + "px 页面边距" + PagerSetting.PagePadding + "px";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintView_Click(object sender, EventArgs e)
        {
            if (!ReadConfig()) return;

            Margins margin = new Margins(0, 0, 0, 0);
            printDocument.DefaultPageSettings.Margins = margin;
            printDocument.DefaultPageSettings.PaperSize = new PaperSize("热敏纸", m_printImg.Height, m_printImg.Width);

            printDocument.PrintPage += new PrintPageEventHandler(PrintPageEvent);//

            printPreviewDialog.Document = printDocument;
            printPreviewDialog.ShowDialog();
        }

        private int _imgW;
        private int _imgH;

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "图片|*.jpg;*.png;*.bmp;*.jpeg;";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                tbImgFile.Text = openFileDialog.FileName;
                //自动计算图片大小
                Image _imgToInsert = Image.FromFile(openFileDialog.FileName);
                _imgW = _imgToInsert.Width;
                _imgH = _imgToInsert.Height;
                tbImageWidth.Text = _imgToInsert.Width.ToString();
                tbImageHeight.Text = _imgToInsert.Height.ToString();
            }
        }

        private void cbPrint_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPrint.SelectedItem != null) //判断是否有选中值
            {
                if (Externs.SetDefaultPrinter(cbPrint.SelectedItem.ToString())) //设置默认打印机
                {
                    //MessageBox.Show(cbPrint.SelectedItem.ToString() + "设置为默认打印机成功！");
                }
                else
                {
                    MessageBox.Show(cbPrint.SelectedItem.ToString() + "设置为默认打印机失败！");
                }
            }
        }

        /// <summary>
        /// 导入EXCEL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "文本文件|*.xls";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;

                IProduct Provider = new ProductProvider(FileName);
                ListSource = Provider.GetList();//Excel赋值到ProductEntity
                SetTable(ListSource);//赋值到dataGridView
            }
        }

        /// <summary>
        /// 赋值到dataGridView
        /// </summary>
        /// <param name="listSource"></param>
        private void SetTable(List<ProductEntity> listSource)
        {
            this.dataGridView1.Rows.Clear();
            if (listSource != null)
            {
                foreach (ProductEntity productEntity in listSource)
                {
                    dataGridView1.Rows.Add(
                                            productEntity.ProductName,
                                            productEntity.ProductCode,
                                            productEntity.ProductPrice,
                                            productEntity.ProductUnit,
                                            productEntity.ProductSize,
                                            productEntity.ProductColor,
                                            productEntity.ProductSupplier,
                                            productEntity.ProductBatch,
                                            productEntity.StartDate,
                                            productEntity.EndTime,
                                            productEntity.Remark,
                                            productEntity.Address,
                                            productEntity.CompanyName,
                                            productEntity.Logo);
                }
            }
            else
            {
                MessageBox.Show("赋值到dataGridView失败！");
            }
        }

        private void cbVar_SelectedIndexChanged(object sender, EventArgs e)
        {
            string varStr = "{" + cbVar.Text + "}";
            string s = tbBarCodeContent.Text;
            int idx = tbBarCodeContent.SelectionStart;
            tbBarCodeContent.Text = s.Insert(idx, varStr);
        }

        private void cbInsertLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!PagerSetting.IsPagerLayerCreated)
            {
                MessageBox.Show("请先设置纸张尺寸！");
                return;
            }
            int m_LineDirect = 0;//线条方向  1 横线  2 竖线
            if (cbInsertLine.SelectedItem.ToString() == "插入横线")
            {
                m_LineDirect = 1;
            }
            else if (cbInsertLine.SelectedItem.ToString() == "插入竖线")
            {
                m_LineDirect = 2;
            }
            LineLayer ll = new LineLayer(int.Parse(tbLineWidth.Text), int.Parse(tbLinelength.Text), m_LineDirect);
            panel1.Controls.Add(ll);
            ll.BringToFront();//一定要加这句将层放到最上面，不然默认是放在最下面的
            ll.Left = m_startX + 30;
            ll.Top = m_startY + 50;
        }

        private void tbPageWidth_TextChanged(object sender, EventArgs e)
        {
            try
            {
                PageMargins = int.Parse(tbPageMargins.Text);//页面边距
                PagerSetting.PagePadding = (int)PagerSetting.MillimeterToPixel(panel1.Handle, PageMargins, 1);

                PageWidth = int.Parse(tbPageWidth.Text);
                PageHeight = int.Parse(tbPageHeight.Text);
                if (PageWidth > 0 && PageHeight > 0)//进行尺码单位毫米（mm）转换成像素（Px）
                {
                    PagerSetting.PageHeight = (int)PagerSetting.MillimeterToPixel(panel1.Handle, PageHeight, 2);
                    PagerSetting.PageWidth = (int)PagerSetting.MillimeterToPixel(panel1.Handle, PageWidth, 1);
                }

                //重绘pager纸张
                PageLayer pl = new PageLayer(PagerSetting.PageWidth, PagerSetting.PageHeight, PagerSetting.PagePadding);
                //先把旧纸张去掉
                if (panel1.Controls.Count > 0)
                {
                    for (int i = 0; i < panel1.Controls.Count; i++)
                    {
                        if (((ImageLayerBase)panel1.Controls[i]).layerType == LayerType.Paper)
                        {
                            panel1.Controls.RemoveAt(i);
                            break;
                        }
                    }
                }
                panel1.Controls.Add(pl);
                PagerSetting.IsPagerLayerCreated = true;

                //计算控件起始坐标
                int startX = (int)((panel1.Width - PagerSetting.PageWidth) / 2);
                int startY = (int)((panel1.Height - PagerSetting.PageHeight) / 2);
                pl.Left = startX;
                pl.Top = startY;
                m_startX = startX + PagerSetting.PagePadding;
                m_startY = startY + PagerSetting.PagePadding;

                lbMessage.Text = "设置成功，纸张尺寸" + PagerSetting.PageWidth + "*" + PagerSetting.PageHeight + "px 页面边距" + PagerSetting.PagePadding + "px";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbPageHeight_TextChanged(object sender, EventArgs e)
        {
            try
            {
                PageMargins = int.Parse(tbPageMargins.Text);//页面边距
                PagerSetting.PagePadding = (int)PagerSetting.MillimeterToPixel(panel1.Handle, PageMargins, 1);

                PageWidth = int.Parse(tbPageWidth.Text);
                PageHeight = int.Parse(tbPageHeight.Text);
                if (PageWidth > 0 && PageHeight > 0)//进行尺码单位毫米（mm）转换成像素（Px）
                {
                    PagerSetting.PageHeight = (int)PagerSetting.MillimeterToPixel(panel1.Handle, PageHeight, 2);
                    PagerSetting.PageWidth = (int)PagerSetting.MillimeterToPixel(panel1.Handle, PageWidth, 1);
                }

                //重绘pager纸张
                PageLayer pl = new PageLayer(PagerSetting.PageWidth, PagerSetting.PageHeight, PagerSetting.PagePadding);
                //先把旧纸张去掉
                if (panel1.Controls.Count > 0)
                {
                    for (int i = 0; i < panel1.Controls.Count; i++)
                    {
                        if (((ImageLayerBase)panel1.Controls[i]).layerType == LayerType.Paper)
                        {
                            panel1.Controls.RemoveAt(i);
                            break;
                        }
                    }
                }
                panel1.Controls.Add(pl);
                PagerSetting.IsPagerLayerCreated = true;

                //计算控件起始坐标
                int startX = (int)((panel1.Width - PagerSetting.PageWidth) / 2);
                int startY = (int)((panel1.Height - PagerSetting.PageHeight) / 2);
                pl.Left = startX;
                pl.Top = startY;
                m_startX = startX + PagerSetting.PagePadding;
                m_startY = startY + PagerSetting.PagePadding;

                lbMessage.Text = "设置成功，纸张尺寸" + PagerSetting.PageWidth + "*" + PagerSetting.PageHeight + "px 页面边距" + PagerSetting.PagePadding + "px";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 纸张尺寸设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}