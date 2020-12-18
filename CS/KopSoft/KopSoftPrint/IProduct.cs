using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KopSoft.KopSoftPrint
{
    public interface IProduct
    {
        /// <summary>
        /// 获取打印的数据源
        /// </summary>
        /// <returns></returns>
        List<ProductEntity> GetList();
    }
}