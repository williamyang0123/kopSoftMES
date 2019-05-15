using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KopSoft
{
    public partial class MainForm : Form
    {
        private int childFormNumber = 0;

        private KopSoftPrint.KopSoftPrint kopSoftPrint;
        private KopSoftSerialPort.KopSoftSerialPort kopSoftSerialPort;

        public MainForm()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "窗口 " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if ((kopSoftPrint == null) || (kopSoftPrint.IsDisposed)) //如果没有打开过
            {
                kopSoftPrint = new KopSoftPrint.KopSoftPrint();
                kopSoftPrint.MdiParent = this;
                kopSoftPrint.WindowState = FormWindowState.Maximized;
                kopSoftPrint.Show();
            }
            else
            {
                kopSoftPrint.WindowState = FormWindowState.Maximized;
                kopSoftPrint.Activate(); //如果已经打开过就让其获得焦点
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void SupportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://kopsoft.cn/");
        }

        private void RegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Register register = new Register();
            register.ShowDialog();
        }

        private void kopSoftPrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((kopSoftPrint == null) || (kopSoftPrint.IsDisposed)) //如果没有打开过
            {
                kopSoftPrint = new KopSoftPrint.KopSoftPrint();
                kopSoftPrint.MdiParent = this;
                kopSoftPrint.WindowState = FormWindowState.Maximized;
                kopSoftPrint.Show();
            }
            else
            {
                kopSoftPrint.WindowState = FormWindowState.Maximized;
                kopSoftPrint.Activate(); //如果已经打开过就让其获得焦点
            }
        }

        private void kopSoftSerialPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((kopSoftSerialPort == null) || (kopSoftSerialPort.IsDisposed)) //如果没有打开过
            {
                kopSoftSerialPort = new KopSoftSerialPort.KopSoftSerialPort();
                kopSoftSerialPort.MdiParent = this;
                kopSoftSerialPort.WindowState = FormWindowState.Maximized;
                kopSoftSerialPort.Show();
            }
            else
            {
                kopSoftSerialPort.WindowState = FormWindowState.Maximized;
                kopSoftSerialPort.Activate(); //如果已经打开过就让其获得焦点
            }
        }
    }
}