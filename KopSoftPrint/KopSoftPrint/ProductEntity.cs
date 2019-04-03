using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KopSoftPrint
{
    internal class ProductEntity
    {
        /// <summary>
        /// 物料名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 物料批次
        /// </summary>
        public string ProductBatch { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public string MakeTime { get; set; }

        /// <summary>
        /// 有效日期
        /// </summary>
        public string LastTime { get; set; }

        /// <summary>
        /// 二维码
        /// </summary>
        public string QRCode { get; set; }
    }
}