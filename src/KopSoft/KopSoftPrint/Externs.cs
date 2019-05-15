using System;
using System.Runtime.InteropServices;

namespace KopSoft.KopSoftPrint
{
    public class Externs
    {
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(String Name);//调用win api将指定名称的打印机设置为默认打印机
    }
}