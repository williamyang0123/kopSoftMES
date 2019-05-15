using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KopSoft.KopSoftSerialPort
{
    public partial class KopSoftSerialPort : Form
    {
        private SerialPort serialPort = new SerialPort();

        public delegate void UpdateString(object NewData);

        public KopSoftSerialPort()
        {
            InitializeComponent();

            cbPortName.Items.AddRange(SerialPort.GetPortNames());//获取串口
            if (cbPortName.Items.Count > 0)
            {
                cbPortName.SelectedIndex = 0;
            }

            cbBaudRate.SelectedIndex = 5;
            cbDataBits.SelectedIndex = 2;
            cbParity.SelectedIndex = 0;
            cbStopBits.SelectedIndex = 0;

            pictureBox1.Image = KopSoft.Properties.Resources.red;
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (cbPortName.Items.Count <= 0)
            {
                MessageBox.Show("没有发现串口,请检查线路！");
                return;
            }

            btnSend.Enabled = false;
            serialPort.Encoding = Encoding.GetEncoding("gb2312");//解决串口接收中文乱码

            if (serialPort.IsOpen == false)
            {
                serialPort.PortName = cbPortName.SelectedItem.ToString();
                serialPort.BaudRate = Convert.ToInt32(cbBaudRate.SelectedItem.ToString());
                serialPort.Parity = (Parity)Convert.ToInt32(cbParity.SelectedIndex.ToString());
                serialPort.DataBits = Convert.ToInt32(cbDataBits.SelectedItem.ToString());
                serialPort.StopBits = (StopBits)Convert.ToInt32(cbStopBits.SelectedItem.ToString());
                try
                {
                    serialPort.Open();
                    btnSend.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                btnOpen.Text = "关闭串口";
                pictureBox1.Image = KopSoft.Properties.Resources.green;

                serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived); //绑定事件
            }
            else
            {
                try
                {
                    serialPort.Close();
                    btnSend.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                btnOpen.Text = "打开串口";
                pictureBox1.Image = KopSoft.Properties.Resources.red;
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            UpdateRecevie(serialPort.ReadExisting());
        }

        public void UpdateRecevie(object NewData)
        {
            if (this.InvokeRequired)//等待异步
            {
                UpdateString myInvoke = new UpdateString(UpdateRecevie);
                Invoke(myInvoke, new object[] { NewData });
            }
            else
            {
                tbReceiveData.AppendText(NewData.ToString());
                //tbReceiveData.SelectionStart = tbReceiveData.Text.Length - 1;
                tbReceiveData.ScrollToCaret();
            }
        }

        /// <summary>
        /// 清除窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            tbReceiveData.Clear();
            tbTransmitData.Clear();
            textBox1.Clear();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 发送数据 发送时需要转换成16进制再进行发送
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="HexCmd"></param>
        private void Send(string cmd, bool HexCmd)
        {
            if (cmd == null)
            {
                return;
            }
            if (cmd.Length > 0)
            {
                if (serialPort.IsOpen == true)
                {
                    byte[] SendBytes = null;
                    string SendData = cmd;
                    if (HexCmd == true)
                    {
                        //16进制发送
                        try
                        {
                            SendData = SendData.Replace(" ", "");
                            if (SendData.Length % 2 == 1)
                            {
                                //奇数个字符
                                SendData = SendData.Remove(SendData.Length - 1, 1); //去除末位字符
                            }
                            List<string> SendDataList = new List<string>();
                            for (int i = 0; i < SendData.Length; i = i + 2)
                            {
                                SendDataList.Add(SendData.Substring(i, 2));
                            }
                            SendBytes = new byte[SendDataList.Count];
                            for (int j = 0; j < SendBytes.Length; j++)
                            {
                                SendBytes[j] = (byte)(Convert.ToInt32(SendDataList[j], 16));
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    else
                    {
                        System.Text.Encoding chs = System.Text.Encoding.GetEncoding("gb2312");
                        byte[] bytes = chs.GetBytes(cmd);
                        string str = "";
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            str += string.Format("{0:X2}", bytes[i]);
                        }
                        List<string> SendDataList = new List<string>();
                        for (int i = 0; i < str.Length; i = i + 2)
                        {
                            SendDataList.Add(str.Substring(i, 2));
                        }
                        SendDataList.Add("0D");
                        SendBytes = new byte[SendDataList.Count];
                        for (int j = 0; j < SendBytes.Length; j++)
                        {
                            SendBytes[j] = (byte)(Convert.ToInt32(SendDataList[j], 16));
                        }
                    }
                    serialPort.Write(SendBytes, 0, SendBytes.Length); //发送数据
                }
                else
                {
                    MessageBox.Show("请打开串口！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}