using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KopSoft
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length <= 0)
            {
                MessageBox.Show("DES长度不能小于0位，请重新输入！");
                return;
            }
            string path = Path.Combine(Application.StartupPath, "Reg.KopSoft");
            if (!File.Exists(path))
            {
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(textBox1.Text.Trim());
                    bw.Flush();
                }
            }
            else
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(textBox1.Text.Trim());
                    bw.Flush();
                }
            }
            Application.Restart();
        }

        private void Register_Load(object sender, EventArgs e)
        {
            if (Global.endTime > DateTime.Now)
            {
                label4.Visible = false;
                textBox1.Visible = false;
                button1.Visible = false;
                lbCustomerCode.Text = "客户 " + Global.customerCode;
                label2.Text = "许可证：已应用产品密钥";
                label3.Text = "注册日期 " + Global.startDate + "，到期时间 " + Global.endTime;
            }
        }
    }
}