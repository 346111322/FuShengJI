using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;

namespace UnityEditor
{
    public class ExcelFileReader
    {
        ExcelFileReader()
        {

        }

        public static DataTable ReadExcelToDataTable(string filename, string sheetName)
        {
            DataTable dt = new DataTable();
            IWorkbook workBook = ExcelFileReader.ReadExcelFile(filename, sheetName);
            if (workBook != null)
            {
                ISheet sheet = workBook.GetSheetAt(0);
                //System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                IRow headerRow = sheet.GetRow(0);
                int cellcount = headerRow.LastCellNum;
                for (int j = 0; j < cellcount; j++)
                {
                    ICell cell = headerRow.GetCell(j);
                    dt.Columns.Add(cell.ToString());

                }

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if ((row != null) && (row.GetCell(0) != null) && (row.GetCell(0).ToString() != ""))
                    {
                        DataRow dataRow = dt.NewRow();

                        for (int j = row.FirstCellNum; j < cellcount; j++)
                        {
                            if (row.GetCell(j) != null)
                            {
                                ICell cell = row.GetCell(j);
                                if (cell.CellType == CellType.Numeric)
                                {
                                    dataRow[j] = cell.NumericCellValue;
                                }
                                else if (cell.CellType == CellType.String)
                                {
                                    dataRow[j] = cell.StringCellValue;
                                }
                                else if (cell.CellType == CellType.Formula)
                                {
                                    if (cell.CachedFormulaResultType == CellType.Numeric)
                                    {
                                        dataRow[j] = cell.NumericCellValue;
                                    }
                                    else if (cell.CachedFormulaResultType == CellType.String)
                                    {
                                        dataRow[j] = cell.StringCellValue;
                                    }
                                    else
                                    {
                                        dataRow[j] = cell.ToString();
                                    }

                                }
                                else
                                {
                                    dataRow[j] = cell.ToString();
                                }

                            }
                        }

                        dt.Rows.Add(dataRow);
                    }

                }
            }

            return dt;
        }
        public static IWorkbook ReadExcelFile(string filename, string sheetName)
        {
            IWorkbook workbook = null;
            string fileExt = Path.GetExtension(filename);
            try
            {
                using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    if (fileExt == ".xls")
                    {
                        workbook = new HSSFWorkbook(file);
                    }
                    else if (fileExt == ".xlsx")
                    {
                        workbook = /*WorkbookFactory.Create(file);//*/ new XSSFWorkbook(file);
                    }
                }
                return workbook;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }
    }
}

