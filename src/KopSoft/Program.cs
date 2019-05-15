using KopSoft.KopSoftPrint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KopSoft
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new KopSoftPrint.KopSoftPrint()); return;
            //Application.Run(new KopSoftSerialPort.KopSoftSerialPort()); return;
            Application.Run(new MainForm()); return;

            try
            {
                string path = Path.Combine(Application.StartupPath, "Reg.KopSoft");
                if (File.Exists(path))
                {
                    using (var fs = new FileStream(path, FileMode.Open))
                    {
                        BinaryReader br = new BinaryReader(fs);
                        string str = br.ReadString();
                        if (string.IsNullOrWhiteSpace(str))
                        {
                            Reg();
                        }
                        else
                        {
                            string str1 = Encryption.DesDecrypt(str, Global.customerCode);
                            var arr = str1.Split('|');
                            //if (arr[0].ToString() != Encryption.GetCpuId())
                            //{
                            //    MessageBox.Show("软件未在本机授权");
                            //    Reg();
                            //}

                            if (arr[0].ToString() != Global.customerCode)
                            {
                                MessageBox.Show("客户未授权");
                                Reg();
                            }
                            Global.startDate = Convert.ToDateTime(arr[1]);
                            Global.endTime = Convert.ToDateTime(arr[2]);
                            if (Global.endTime < DateTime.Now)
                            {
                                MessageBox.Show("软件已到期，请联系管理员");
                                return;
                            }
                            else
                            {
                                Application.Run(new MainForm());
                            }
                        }
                    }
                }
                else
                {
                    Register register = new Register();
                    register.ShowDialog();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("软件注册不成功，请联系管理员！");
                return;
            }
        }

        private static void Reg()
        {
            Register register = new Register();
            //GenCode reg = new GenCode();
            if (register.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new MainForm());
            }
        }
    }
}