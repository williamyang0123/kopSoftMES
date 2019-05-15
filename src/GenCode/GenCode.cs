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
    public partial class GenCode : Form
    {
        public GenCode()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var startDate = dateTimePicker1.Text.Trim();
            var endTime = dateTimePicker2.Text.Trim();

            if (tbCustomerCode.Text.Trim().Length < 8)
            {
                MessageBox.Show("客户编码不能小于8位");
                return;
            }
            if (string.IsNullOrWhiteSpace(startDate))
            {
                MessageBox.Show("注册日期不能为空");
                return;
            }
            if (string.IsNullOrWhiteSpace(endTime))
            {
                MessageBox.Show("到期时间不能为空");
                return;
            }

            string customerCode = tbCustomerCode.Text.Trim();
            //string cpuId = Encryption.GetCpuId();//cpuId
            //string guid = Guid.NewGuid().ToString();
            int day = (dateTimePicker2.Value - dateTimePicker1.Value).Days;
            day = 0;

            string str = $"{customerCode}|{startDate}|{endTime}|{day}";
            string des = Encryption.DesEncrypt(str, customerCode);
            textBox1.Text = des;

            //string s1 = Encryption.DesDecrypt(des, customerCode);
        }
    }
}