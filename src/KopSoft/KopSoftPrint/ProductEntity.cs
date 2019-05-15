using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KopSoft.KopSoftPrint
{
    public partial class ProductEntity
    {
        public ProductEntity()
        {
        }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品价格
        /// </summary>
        public string ProductPrice { get; set; }

        /// <summary>
        /// 产品单位
        /// </summary>
        public string ProductUnit { get; set; }

        /// <summary>
        /// 产品规格
        /// </summary>
        public string ProductSize { get; set; }

        /// <summary>
        /// 产品颜色
        /// </summary>
        public string ProductColor { get; set; }

        /// <summary>
        /// 产品供应商
        /// </summary>
        public string ProductSupplier { get; set; }

        /// <summary>
        /// 产品批次
        /// </summary>
        public string ProductBatch { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 商标
        /// </summary>
        public string Logo { get; set; }
    }
}