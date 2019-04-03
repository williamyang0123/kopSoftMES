using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KopSoftPrint
{
    public partial class KopSoftPrint : Form
    {
        private List<ProductEntity> Product_Entity { get; set; }
        private PrintDocument pd = new PrintDocument();

        private int rowIndex;

        public KopSoftPrint()
        {
            InitializeComponent();
        }

        private void KopSoftPrint_Load(object sender, EventArgs e)
        {
            string sDefault = pd.PrinterSettings.PrinterName; //默认打印机名
            foreach (string sPrint in PrinterSettings.InstalledPrinters) //获取所有打印机名称
            {
                cbPrint.Items.Add(sPrint);
                if (sPrint == sDefault) cbPrint.SelectedIndex = cbPrint.Items.IndexOf(sPrint);
            }
        }

        public void Print(int Number)
        {
            pd.DefaultPageSettings.PaperSize = new PaperSize("", 999, 999); //设置纸张大小
            StandardPrintController controler = new StandardPrintController();

            if (dataGridView1.CurrentCell != null)
            {
                for (int j = 0; j < dataGridView1.SelectedRows.Count; j++) //遍历所有选中的行
                {
                    rowIndex = dataGridView1.SelectedRows[j].Index;
                    try
                    {
                        pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                        pd.PrintController = controler;
                        for (int i = 0; i < Number; i++)
                        {
                            pd.Print();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }
                    finally
                    {
                        pd.Dispose();
                    }
                }
            }
        }

        public void pd_PrintPage(Object Sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush brush = Brushes.Black;

            string ProductName = "物料名称:" + dataGridView1.Rows[rowIndex].Cells[1].Value.ToString();
            string ProductCode = "物料编码:" + dataGridView1.Rows[rowIndex].Cells[2].Value.ToString();
            string ProductBatch = "物料批次:" + dataGridView1.Rows[rowIndex].Cells[3].Value.ToString();
            string Supplier = "供应商:" + dataGridView1.Rows[rowIndex].Cells[4].Value.ToString();
            string Size = "储存条件:" + dataGridView1.Rows[rowIndex].Cells[5].Value.ToString();
            string Unit = "净重:" + dataGridView1.Rows[rowIndex].Cells[6].Value.ToString();
            string MakeTime = "生产日期:" + dataGridView1.Rows[rowIndex].Cells[7].Value.ToString();
            string LastTime = "有效日期:" + dataGridView1.Rows[rowIndex].Cells[8].Value.ToString();

            string QRCode = string.Format("{0}_{1}_{2}_1_1", dataGridView1.Rows[rowIndex].Cells[2].Value.ToString(), dataGridView1.Rows[rowIndex].Cells[3].Value.ToString(), dataGridView1.Rows[rowIndex].Cells[8].Value.ToString());

            //Pen pen = new Pen(Color.Black, 1);

            //g.DrawLine(pen, 5, 5, 270, 5);
            //g.DrawLine(pen, 5, 30, 270, 30);
            //g.DrawLine(pen, 5, 50, 270, 50);
            //g.DrawLine(pen, 5, 70, 270, 70);

            //g.DrawLine(pen, 110, 90, 270, 90);
            //g.DrawLine(pen, 110, 110, 270, 110);
            //g.DrawLine(pen, 110, 130, 270, 130);
            //g.DrawLine(pen, 110, 150, 270, 150);
            //g.DrawLine(pen, 5, 170, 270, 170);

            //g.DrawLine(pen, 5, 5, 5, 170);
            //g.DrawLine(pen, 110, 70, 110, 170);
            //g.DrawLine(pen, 270, 5, 270, 170);

            g.DrawString(ProductName, new Font("黑体", 7), brush, 10, 15);
            g.DrawString(ProductCode, new Font("黑体", 7), brush, 10, 35);
            g.DrawString("SN:" + QRCode, new Font("黑体", 7), brush, 10, 55);

            QRCode qrCode = new QRCode();
            qrCode.GenerateQRCode(QRCode, pictureBox1);
            g.DrawImage(pictureBox1.Image, 15, 75, 70, 70);

            g.DrawString(ProductBatch, new Font("黑体", 7), brush, 110, 75);
            g.DrawString(Supplier, new Font("黑体", 7), brush, 110, 95);
            g.DrawString(Size, new Font("黑体", 7), brush, 110, 115);
            g.DrawString(Unit, new Font("黑体", 7), brush, 110, 135);
            g.DrawString(MakeTime, new Font("黑体", 7), brush, 110, 155);
            g.DrawString(LastTime, new Font("黑体", 7), brush, 110, 175);
            //g.DrawString("打印日期:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), new Font("黑体", 7), brush, 110, 175);

            //g.DrawString("洗瓶间", new Font("黑体", 60), brush, 5, 50);
            e.HasMorePages = false;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            int Number = Convert.ToInt32(numericUpDown1.Value);
            Print(Number);
        }

        /// <summary>
        /// 设置默认打印机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// 导入Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "文本文件|*.xls";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
                string strCon = "provider=microsoft.jet.oledb.4.0;data source=" + FileName + ";extended properties=excel 8.0";
                System.Data.OleDb.OleDbConnection Con = new System.Data.OleDb.OleDbConnection(strCon); //建立连接
                string strSql = "select * from [Sheet1$]"; //表名的写法也应注意不同，对应的excel表为Sheet1，在这里要在其后加美元符号$，并用中括号
                System.Data.OleDb.OleDbCommand Cmd = new System.Data.OleDb.OleDbCommand(strSql, Con); //建立要执行的命令
                System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(Cmd); //建立数据适配器
                DataSet dataSet = new DataSet(); //新建数据集
                da.Fill(dataSet, "material"); //把数据适配器中的数据读到数据集中的一个表中（此处表名为shyman，可以任取表名）
                dataGridView1.DataSource = dataSet.Tables[0]; //指定dataGridView1的数据源为数据集dataSet的第一张表（也就是shyman表），也可以写dataSet.Table["shyman"]

                dataSet.WriteXml("test.Xml");
            }
        }
    }
}