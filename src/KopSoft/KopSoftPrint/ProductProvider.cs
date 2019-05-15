using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KopSoft.KopSoftPrint
{
    public partial class ProductProvider : IProduct
    {
        private string FilePath;

        public ProductProvider(string filePath)
        {
            this.FilePath = filePath;
        }

        public List<ProductEntity> GetList()
        {
            List<ProductEntity> listResult = new List<ProductEntity>();

            FileStream fs = new FileStream(this.FilePath, FileMode.OpenOrCreate);
            HSSFWorkbook workbook = new HSSFWorkbook(fs);
            ISheet sheet1 = workbook.GetSheetAt(0);

            int rowCount = sheet1.LastRowNum;
            for (int i = 1; i <= sheet1.LastRowNum; i++)
            {
                IRow row = sheet1.GetRow(i);
                if (row != null)
                {
                    string ProductName = string.Empty;
                    string ProductCode = string.Empty;
                    string ProductPrice = string.Empty;
                    string ProductUnit = string.Empty;

                    string ProductSize = string.Empty;
                    string ProductColor = string.Empty;
                    string ProductSupplier = string.Empty;
                    string ProductBatch = string.Empty;

                    string StartDate = string.Empty;
                    string EndTime = string.Empty;
                    string Remark = string.Empty;
                    string Address = string.Empty;
                    string CompanyName = string.Empty;
                    string Logo = string.Empty;

                    try
                    {
                        ProductName = row.Cells[0].ToString();
                    }
                    catch (Exception)
                    {
                        ProductName = string.Empty;
                    }
                    try
                    {
                        ProductCode = row.Cells[1].ToString();
                    }
                    catch (Exception)
                    {
                        ProductCode = string.Empty;
                    }
                    try
                    {
                        ProductPrice = row.Cells[2].ToString();
                    }
                    catch (Exception)
                    {
                        ProductPrice = string.Empty;
                    }
                    try
                    {
                        ProductUnit = row.Cells[3].ToString();
                    }
                    catch (Exception)
                    {
                        ProductUnit = string.Empty;
                    }

                    try
                    {
                        ProductSize = row.Cells[4].ToString();
                    }
                    catch (Exception)
                    {
                        ProductSize = string.Empty;
                    }
                    try
                    {
                        ProductColor = row.Cells[5].ToString();
                    }
                    catch (Exception)
                    {
                        ProductColor = string.Empty;
                    }
                    try
                    {
                        ProductSupplier = row.Cells[6].ToString();
                    }
                    catch (Exception)
                    {
                        ProductSupplier = string.Empty;
                    }
                    try
                    {
                        ProductBatch = row.Cells[7].ToString();
                    }
                    catch (Exception)
                    {
                        ProductBatch = string.Empty;
                    }

                    try
                    {
                        StartDate = row.Cells[8].ToString();
                    }
                    catch (Exception)
                    {
                        StartDate = string.Empty;
                    }
                    try
                    {
                        EndTime = row.Cells[9].ToString();
                    }
                    catch (Exception)
                    {
                        EndTime = string.Empty;
                    }
                    try
                    {
                        Remark = row.Cells[10].ToString();
                    }
                    catch (Exception)
                    {
                        Remark = string.Empty;
                    }
                    try
                    {
                        Address = row.Cells[11].ToString();
                    }
                    catch (Exception)
                    {
                        Address = string.Empty;
                    }
                    try
                    {
                        CompanyName = row.Cells[12].ToString();
                    }
                    catch (Exception)
                    {
                        CompanyName = string.Empty;
                    }
                    try
                    {
                        Logo = row.Cells[13].ToString();
                    }
                    catch (Exception)
                    {
                        Logo = string.Empty;
                    }

                    ProductEntity product = new ProductEntity();
                    product.ProductName = ProductName;
                    product.ProductCode = ProductCode;
                    product.ProductPrice = ProductPrice;
                    product.ProductUnit = ProductUnit;

                    product.ProductSize = ProductSize;
                    product.ProductColor = ProductColor;
                    product.ProductSupplier = ProductSupplier;
                    product.ProductBatch = ProductBatch;

                    product.StartDate = StartDate;
                    product.EndTime = EndTime;
                    product.Remark = Remark;
                    product.Address = Address;
                    product.CompanyName = CompanyName;
                    product.Logo = Logo;

                    listResult.Add(product);
                }
            }
            return listResult;
        }

        public DataTable Read(string filePath, int sheetIndex)
        {
            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
            HSSFWorkbook workbook = new HSSFWorkbook(fs);
            ISheet sheet1 = workbook.GetSheetAt(sheetIndex);
            DataTable table = new DataTable();
            IRow row1 = sheet1.GetRow(0);
            int cellCount = row1.LastCellNum;
            for (int i = row1.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(row1.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            int rowCount = sheet1.LastRowNum;
            for (int i = 5; i <= sheet1.LastRowNum; i++)
            {
                IRow row = sheet1.GetRow(i);
                if (row != null)
                {
                    DataRow dataRow = table.NewRow();

                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                            dataRow[j] = row.GetCell(j).ToString();
                    }

                    table.Rows.Add(dataRow);
                }
            }
            workbook = null;
            sheet1 = null;
            return table;
        }
    }
}