using DLL.Negocio;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

public class cExcel
{
    protected static IWorkbook workbook;
    protected static DataFormatter dataFormatter;
    protected static IFormulaEvaluator formulaEvaluator;

    public static DataTable ExcelToDataTable(string path)
    {
        HSSFWorkbook hssfworkbook = new HSSFWorkbook();
        XSSFWorkbook xssfworkbook = new XSSFWorkbook();
        ISheet sheet;
        bool flagXlsx = false;

        string fileExt = Path.GetExtension(path);

        if (fileExt == ".xls")
        {
            //Use the NPOI Excel xls object
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }

            //Assign the sheet
            sheet = hssfworkbook.GetSheetAt(0);
        }
        else //.xlsx extension
        {
            //Use the NPOI Excel xlsx object
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                xssfworkbook = new XSSFWorkbook(file);
            }

            //Assign the sheet
            sheet = xssfworkbook.GetSheetAt(0);
            flagXlsx = true;
        }

        System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

        #region Cantidad de columnas
        System.Collections.IEnumerator _rows = sheet.GetRowEnumerator();
        int cantColumn = 0;
        int count = 0;
        //Primero hay que inicializar _rows.
        while (_rows.MoveNext())
        {
            IRow _row;
            if (flagXlsx == false)
                _row = (HSSFRow)_rows.Current;
            else
                _row = (XSSFRow)_rows.Current;

            //Obtengo la cantidad de columnas, y se termina el while.
            cantColumn = _row.Cells.Count;
            count++;
        }
        #endregion

        DataTable dt = new DataTable();
        for (int j = 0; j < cantColumn; j++)
        {
            dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString());
        }
        rows.MoveNext();
        while (rows.MoveNext())
        {
            IRow row;
            if (flagXlsx == false)
                row = (HSSFRow)rows.Current;
            else
                row = (XSSFRow)rows.Current;

            DataRow dr = dt.NewRow();

            for (int i = 0; i < cantColumn; i++)
            {
                ICell cell = row.GetCell(i);

                if (cell == null)
                {
                    dr[i] = null;
                }
                else
                {
                    dr[i] = cell.ToString();
                }
            }
            dt.Rows.Add(dr);
        }

        return dt;
    }


    public static DataTable ExcelToDataTablePrecios(string path)
    {
        HSSFWorkbook hssfworkbook = new HSSFWorkbook();
        XSSFWorkbook xssfworkbook = new XSSFWorkbook();
        ISheet sheet;
        bool flagXlsx = false;

        string fileExt = Path.GetExtension(path);

        if (fileExt == ".xls")
        {
            //Use the NPOI Excel xls object
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }

            //Assign the sheet
            sheet = hssfworkbook.GetSheetAt(0);
        }
        else //.xlsx extension
        {
            //Use the NPOI Excel xlsx object
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                xssfworkbook = new XSSFWorkbook(file);
            }

            //Assign the sheet
            sheet = xssfworkbook.GetSheetAt(0);
            flagXlsx = true;
        }

        System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

        #region Cantidad de columnas
        System.Collections.IEnumerator _rows = sheet.GetRowEnumerator();
        int cantColumn = 0;
        int count = 0;
        //Primero hay que inicializar _rows.
        while (_rows.MoveNext())
        {
            IRow _row;
            if (flagXlsx == false)
                _row = (HSSFRow)_rows.Current;
            else
                _row = (XSSFRow)_rows.Current;

            //Obtengo la cantidad de columnas, y se termina el while.
            cantColumn = _row.Cells.Count;
            count++;
        }
        #endregion

        DataTable dt = new DataTable();
        for (int j = 0; j < 14; j++)
        {
            dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString());
        }
        rows.MoveNext();
        while (rows.MoveNext())
        {
            IRow row;
            if (flagXlsx == false)
                row = (HSSFRow)rows.Current;
            else
                row = (XSSFRow)rows.Current;

            DataRow dr = dt.NewRow();

            for (int i = 0; i < 14; i++)
            {
                ICell cell = row.GetCell(i);

                if (cell == null)
                {
                    dr[i] = null;
                }
                else
                {
                    if (cell.CellType == CellType.Formula)
                    {

                        dr[i] = GetUnformattedValue(cell);

                    } else {

                        dr[i] = cell.ToString();

                    }
                   
                }
            }
            dt.Rows.Add(dr);
        }

        return dt;
    }

    /// <summary>
    /// Lee la segunda hoja del Excel
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static DataTable ExcelToDataTableRead2Sheet(string path)
    {
        HSSFWorkbook hssfworkbook = new HSSFWorkbook();
        XSSFWorkbook xssfworkbook = new XSSFWorkbook();
        ISheet sheet;
        bool flagXlsx = false;

        string fileExt = Path.GetExtension(path);

        if (fileExt == ".xls")
        {
            //Use the NPOI Excel xls object
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }

            //Assign the sheet
            sheet = hssfworkbook.GetSheetAt(0);
        }
        else //.xlsx extension
        {
            //Use the NPOI Excel xlsx object
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                xssfworkbook = new XSSFWorkbook(file);
            }

            //Assign the sheet
            sheet = xssfworkbook.GetSheetAt(0);
            flagXlsx = true;
        }

        System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

        IRow headerRow = sheet.GetRow(0);

        #region Cantidad de columnas
        System.Collections.IEnumerator _rows = sheet.GetRowEnumerator();
        int cantColumn = 0;
        int count = 0;
        //Primero hay que inicializar _rows.
        while (_rows.MoveNext())
        {
            IRow _row;
            if (flagXlsx == false)
                _row = (HSSFRow)_rows.Current;
            else
                _row = (XSSFRow)_rows.Current;

            //Obtengo la cantidad de columnas, y se termina el while.
            if (cantColumn < _row.Cells.Count)
                cantColumn = _row.Cells.Count;
            count++;
        }
        #endregion

        DataTable dt = new DataTable();
        for (int j = 0; j < cantColumn; j++)
        {
            dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString());
        }
        rows.MoveNext();
        while (rows.MoveNext())
        {
            IRow row;
            if (flagXlsx == false)
                row = (HSSFRow)rows.Current;
            else
                row = (XSSFRow)rows.Current;

            DataRow dr = dt.NewRow();

            for (int i = 0; i < cantColumn; i++)
            {
                ICell cell = row.GetCell(i);

                if (cell == null)
                {
                    dr[i] = null;
                }
                else
                {
                    if (i == 4 || i == 5 || i == 6 || i == 7)
                    {
                        try
                        {
                            if (cAuxiliar.IsNumeric(Convert.ToString(cell.NumericCellValue)))
                                dr[i] = String.Format("{0:#,#0.00}", cell.NumericCellValue);
                            else
                                dr[i] = cell.ToString();
                        }
                        catch {
                            dr[i] = cell.ToString();
                        }                        
                    }
                    else
                        dr[i] = cell.ToString();
                }
            }
            dt.Rows.Add(dr);
        }

        return dt;
    }

    public static void DataTableToExcelUnidades(List<DataTable> tabla, String fileName, string supCubierta, string supBalcon, string supTerraza, string supTotal, string porcentaje, string precio)
    {
        try {
            //Parámetros:
            //tabla: listado de las tablas que se tienen que exportar
            //fileName: nombre del archivo resultante
            //sheet: cantidad de hojas que se tienen que crear en el Excel

            //Make a new npoi workbook
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();

            //Here I am making sure that I am giving the file name the right extension:
            string filename = "";

            if (fileName.EndsWith(".xls"))
                filename = fileName;
            else
                filename = fileName + ".xls";

            //This starts the dialogue box that allows the user to download the file
            System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            Response.Clear();

            int index = 0;

            //make a new sheet – name it any excel-compliant string you want
            ISheet sheet1 = hssfworkbook.CreateSheet("Sheet 1");

            //make a header row
            IRow row1 = sheet1.CreateRow(0);
            DataTable dt = tabla[0];

            #region Estilo
            ICellStyle styleHead = hssfworkbook.CreateCellStyle();
            IFont font2 = hssfworkbook.CreateFont();
            font2.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.None;
            font2.FontHeightInPoints = 11;
            font2.FontName = "Arial";
            styleHead.SetFont(font2);
            #endregion

            #region Columnas
            ICell cell_Encabezado = row1.CreateCell(0);
            cell_Encabezado.SetCellValue("Cod U.F.");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(1);
            cell_Encabezado.SetCellValue("Tipo Unidad");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(2);
            cell_Encabezado.SetCellValue("Nivel");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(3);
            cell_Encabezado.SetCellValue("Nro. Unidad");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(4);
            cell_Encabezado.SetCellValue("Ambiente");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(5);
            cell_Encabezado.SetCellValue("Sup. Cubierta (m²)");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(6);
            cell_Encabezado.SetCellValue("Balcon (m²)");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(7);
            cell_Encabezado.SetCellValue("Terraza (m²)");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(8);
            cell_Encabezado.SetCellValue("Sup. Total (m²)");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(9);
            cell_Encabezado.SetCellValue("Porcentaje (%)");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(10);
            cell_Encabezado.SetCellValue("Moneda");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(11);
            cell_Encabezado.SetCellValue("Precio Base");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(12);
            cell_Encabezado.SetCellValue("Estado");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(13);
            cell_Encabezado.SetCellValue("VALOR");
            cell_Encabezado.CellStyle = styleHead;
            #endregion

            string rowName = null;
            int auxRow = dt.Rows.Count;
            //loops through data
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row.CreateCell(j);
                    String columnName = dt.Columns[j].ToString();

                    if (j == 12)
                        rowName = cUnidad.Estado(dt.Rows[i][columnName].ToString());
                    else
                    {
                        if (j == 10)
                            rowName = cUnidad.MonedaById(dt.Rows[i][columnName].ToString());
                        else
                            rowName = dt.Rows[i][columnName].ToString();
                    }

                    cell.SetCellValue(rowName);
                }
            }

            #region Totales
            IRow rowTotal = sheet1.CreateRow(auxRow + 1);
            ICell cellSupCubierta = rowTotal.CreateCell(5);
            cellSupCubierta.SetCellValue(supCubierta);
            ICell cellSupBalcon = rowTotal.CreateCell(6);
            cellSupBalcon.SetCellValue(supBalcon);
            ICell cellSupTerraza = rowTotal.CreateCell(7);
            cellSupTerraza.SetCellValue(supTerraza);
            ICell cellSupTotal = rowTotal.CreateCell(8);
            cellSupTotal.SetCellValue(supTotal);
            ICell cellPorcentaje = rowTotal.CreateCell(9);
            cellPorcentaje.SetCellValue(porcentaje);
            ICell cellPrecio = rowTotal.CreateCell(11);
            cellPrecio.SetCellValue(precio);
            #endregion

            index++;

            AutoSizeColumn(sheet1);

            //writing the data to binary from memory
            Response.BinaryWrite(WriteToStream(hssfworkbook).GetBuffer());
            Response.End();


        }
        catch(Exception ex){
        }
    }

    public static void DataTableToExcelComprobantes(List<DataTable> tabla, String fileName, string total)
    {
        try
        {
            //Parámetros:
            //tabla: listado de las tablas que se tienen que exportar
            //fileName: nombre del archivo resultante
            //sheet: cantidad de hojas que se tienen que crear en el Excel

            //Make a new npoi workbook
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();

            //Here I am making sure that I am giving the file name the right extension:
            string filename = "";

            if (fileName.EndsWith(".xls"))
                filename = fileName;
            else
                filename = fileName + ".xls";

            //This starts the dialogue box that allows the user to download the file
            System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            Response.Clear();

            int index = 0;

            //make a new sheet – name it any excel-compliant string you want
            ISheet sheet1 = hssfworkbook.CreateSheet("Sheet 1");

            #region Estilo
            ICellStyle styleTitle = hssfworkbook.CreateCellStyle();
            IFont font1 = hssfworkbook.CreateFont();
            font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            font1.FontHeightInPoints = 12;
            font1.FontName = "Calibri";
            styleTitle.SetFont(font1);

            ICellStyle styleTotal = hssfworkbook.CreateCellStyle();
            IFont fontTotal = hssfworkbook.CreateFont();
            fontTotal.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.None;
            fontTotal.FontHeightInPoints = 12;
            fontTotal.FontName = "Calibri";
            styleTotal.SetFont(fontTotal);
            styleTotal.DataFormat = hssfworkbook.CreateDataFormat().GetFormat("$#,##0.00");

            ICellStyle styleHead = hssfworkbook.CreateCellStyle();
            styleHead.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.DarkRed.Index;
            styleHead.FillPattern = FillPattern.SolidForeground;
            IFont font2 = hssfworkbook.CreateFont();
            font2.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.None;
            font2.FontHeightInPoints = 12;
            font2.FontName = "Calibri";
            font2.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
            styleHead.SetFont(font2);
            #endregion

            #region Total
            IRow rowTitle = sheet1.CreateRow(0);
            ICell cellTitle = rowTitle.CreateCell(0);
            cellTitle.SetCellValue("Total:");
            cellTitle.CellStyle = styleTitle;
            ICell cellTotal = rowTitle.CreateCell(1);
            cellTotal.SetCellValue(Convert.ToDouble(total));
            cellTotal.CellStyle = styleTotal;
            #endregion
            
            //make a header row
            IRow row1 = sheet1.CreateRow(2);
            DataTable dt = tabla[0];

            #region Columnas
            ICell cell_Encabezado = row1.CreateCell(0);
            cell_Encabezado.SetCellValue("NRO.");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(1);
            cell_Encabezado.SetCellValue("FECHA");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(2);
            cell_Encabezado.SetCellValue("CLIENTE");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(3);
            cell_Encabezado.SetCellValue("IMPORTE");
            cell_Encabezado.CellStyle = styleHead;
            #endregion
            
            string rowName = null;
            int auxRow = dt.Rows.Count;
            int file = 3;
            //loops through data
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(file + i);
                for (int j = 0; j < dt.Columns.Count; j++)
                {                    
                    String columnName = dt.Columns[j].ToString();

                    if (columnName == "Nro")
                    {
                        ICell cell = row.CreateCell(0);
                        rowName = dt.Rows[i][columnName].ToString();
                        cell.SetCellValue(Convert.ToDouble(rowName));
                    }

                    if (columnName == "Fecha")
                    {
                        ICell cell1 = row.CreateCell(1);
                        rowName = dt.Rows[i][columnName].ToString();
                        cell1.SetCellValue(String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(rowName)));
                    }

                    if (columnName == "GetEmpresa")
                    {
                        ICell cell2 = row.CreateCell(2);
                        rowName = dt.Rows[i][columnName].ToString();
                        cell2.SetCellValue(rowName);
                    }

                    if (columnName == "Monto")
                    {
                        ICellStyle cellDateStyle = hssfworkbook.CreateCellStyle(); //create custom style
                        cellDateStyle.DataFormat = hssfworkbook.CreateDataFormat().GetFormat("#,##0.00");
                        
                        ICell cell3 = row.CreateCell(3);
                        rowName = dt.Rows[i][columnName].ToString();                        
                        cell3.SetCellType(CellType.Numeric);
                       
                         cell3.SetCellValue(Convert.ToDouble(rowName));
                        cell3.CellStyle = cellDateStyle;
                    }
                }
            }
            
            index++;

            AutoSizeColumn(sheet1);

            //writing the data to binary from memory
            Response.BinaryWrite(WriteToStream(hssfworkbook).GetBuffer());
            Response.End();
        }
        catch (Exception ex)
        {
        }
    }

    public static void DataTableToExcelAllComprobantes(List<DataTable> tabla, List<DataTable> tablaNC, List<DataTable> tablaND, List<DataTable> tablaC, String fileName, string totalR, string totalNC, string totalND, string totalC)
    {
        try
        {
            //Make a new npoi workbook
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();

            //Here I am making sure that I am giving the file name the right extension:
            string filename = "";

            if (fileName.EndsWith(".xls"))
                filename = fileName;
            else
                filename = fileName + ".xls";

            //This starts the dialogue box that allows the user to download the file
            System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            Response.Clear();

            //make a new sheet – name it any excel-compliant string you want
            ISheet sheet1 = hssfworkbook.CreateSheet("Sheet 1");

            #region Estilo
            ICellStyle styleComprobante = hssfworkbook.CreateCellStyle();
            IFont fontComprobante = hssfworkbook.CreateFont();
            fontComprobante.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            fontComprobante.FontHeightInPoints = 14;
            fontComprobante.FontName = "Calibri";
            styleComprobante.SetFont(fontComprobante);

            ICellStyle styleTitle = hssfworkbook.CreateCellStyle();
            IFont font1 = hssfworkbook.CreateFont();
            font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            font1.FontHeightInPoints = 12;
            font1.FontName = "Calibri";
            styleTitle.SetFont(font1);

            ICellStyle styleTotal = hssfworkbook.CreateCellStyle();
            IFont fontTotal = hssfworkbook.CreateFont();
            fontTotal.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.None;
            fontTotal.FontHeightInPoints = 12;
            fontTotal.FontName = "Calibri";
            styleTotal.SetFont(fontTotal);
            styleTotal.DataFormat = hssfworkbook.CreateDataFormat().GetFormat("#,##0.00");

            ICellStyle styleTotalComprobante = hssfworkbook.CreateCellStyle();
            IFont fontTotalComprobante = hssfworkbook.CreateFont();
            fontTotalComprobante.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.None;
            fontTotalComprobante.FontHeightInPoints = 14;
            fontTotalComprobante.FontName = "Calibri";
            styleTotalComprobante.SetFont(fontTotalComprobante);
            styleTotalComprobante.DataFormat = hssfworkbook.CreateDataFormat().GetFormat("#,##0.00");

            ICellStyle styleHead = hssfworkbook.CreateCellStyle();
            styleHead.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.DarkRed.Index;
            styleHead.FillPattern = FillPattern.SolidForeground;
            IFont font2 = hssfworkbook.CreateFont();
            font2.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.None;
            font2.FontHeightInPoints = 12;
            font2.FontName = "Calibri";
            font2.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
            styleHead.SetFont(font2);
            #endregion

            #region Tabla Recibos
            #region Total
            IRow rowRecibo = sheet1.CreateRow(0);
            ICell cellRecibo = rowRecibo.CreateCell(0);
            cellRecibo.SetCellValue("Recibos");
            cellRecibo.CellStyle = styleComprobante;

            IRow rowTitle = sheet1.CreateRow(1);
            ICell cellTitle = rowTitle.CreateCell(0);
            cellTitle.SetCellValue("Total:");
            cellTitle.CellStyle = styleTitle;
            ICell cellTotal = rowTitle.CreateCell(1);
            cellTotal.SetCellValue(Convert.ToDouble(totalR));
            cellTotal.CellStyle = styleTotal;
            #endregion

            //make a header row
            IRow row1 = sheet1.CreateRow(3);
            DataTable dt = tabla[0];

            #region Columnas
            ICell cell_Encabezado = row1.CreateCell(0);
            cell_Encabezado.SetCellValue("NRO.");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(1);
            cell_Encabezado.SetCellValue("FECHA");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(2);
            cell_Encabezado.SetCellValue("CLIENTE");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(3);
            cell_Encabezado.SetCellValue("IMPORTE");
            cell_Encabezado.CellStyle = styleHead;
            #endregion

            #region Filas
            string rowName = null;
            int auxRow = dt.Rows.Count;
            int file = 4;
            //loops through data
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(file + i);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    String columnName = dt.Columns[j].ToString();

                    if (columnName == "Nro")
                    {
                        ICell cell = row.CreateCell(0);
                        rowName = dt.Rows[i][columnName].ToString();
                        cell.SetCellValue(Convert.ToDouble(rowName));
                    }

                    if (columnName == "Fecha")
                    {
                        ICell cell1 = row.CreateCell(1);
                        rowName = dt.Rows[i][columnName].ToString();
                        cell1.SetCellValue(String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(rowName)));
                    }

                    if (columnName == "GetEmpresa")
                    {
                        ICell cell2 = row.CreateCell(2);
                        rowName = dt.Rows[i][columnName].ToString();
                        cell2.SetCellValue(rowName);
                    }

                    if (columnName == "Monto")
                    {
                        ICellStyle cellDateStyle = hssfworkbook.CreateCellStyle(); //create custom style
                        cellDateStyle.DataFormat = hssfworkbook.CreateDataFormat().GetFormat("#,##0.00");

                        ICell cell3 = row.CreateCell(3);
                        rowName = dt.Rows[i][columnName].ToString();
                        cell3.SetCellType(CellType.Numeric);

                        cell3.SetCellValue(Convert.ToDouble(rowName));
                        cell3.CellStyle = cellDateStyle;
                    }
                }
            }
            #endregion
            #endregion

            auxRow = auxRow + 5;
                 
            #region Tabla Notas de crédito
            #region Total
            IRow _rowNC = sheet1.CreateRow(auxRow);
            ICell cellNC = _rowNC.CreateCell(0);
            cellNC.SetCellValue("Notas de crédito");
            cellNC.CellStyle = styleComprobante;

            auxRow = auxRow + 1;

            IRow rowTitleNC = sheet1.CreateRow(auxRow);
            ICell cellTitleNC = rowTitleNC.CreateCell(0);
            cellTitleNC.SetCellValue("Total:");
            cellTitleNC.CellStyle = styleTitle;
            ICell cellTotalNC = rowTitleNC.CreateCell(1);
            cellTotalNC.SetCellValue(Convert.ToDouble(totalNC));
            cellTotalNC.CellStyle = styleTotal;
            #endregion

            auxRow = auxRow + 2;
            IRow rowNC = sheet1.CreateRow(auxRow);
            DataTable dtNC = tablaNC[0];
            
            #region Columnas
            ICell cell_EncabezadoNC = rowNC.CreateCell(0);
            cell_EncabezadoNC.SetCellValue("NRO.");
            cell_EncabezadoNC.CellStyle = styleHead;

            cell_EncabezadoNC = rowNC.CreateCell(1);
            cell_EncabezadoNC.SetCellValue("FECHA");
            cell_EncabezadoNC.CellStyle = styleHead;

            cell_EncabezadoNC = rowNC.CreateCell(2);
            cell_EncabezadoNC.SetCellValue("CLIENTE");
            cell_EncabezadoNC.CellStyle = styleHead;

            cell_EncabezadoNC = rowNC.CreateCell(3);
            cell_EncabezadoNC.SetCellValue("IMPORTE");
            cell_EncabezadoNC.CellStyle = styleHead;
            #endregion

            #region Filas
            string rowNameNC = null;
            auxRow = auxRow + 1;
            int fileNC = auxRow;
            //loops through data
            for (int i = 0; i < dtNC.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(fileNC + i);
                for (int j = 0; j < dtNC.Columns.Count; j++)
                {
                    String columnName = dtNC.Columns[j].ToString();

                    if (columnName == "Nro")
                    {
                        ICell cell = row.CreateCell(0);
                        rowNameNC = dtNC.Rows[i][columnName].ToString();
                        cell.SetCellValue(Convert.ToDouble(rowNameNC));
                    }

                    if (columnName == "Fecha")
                    {
                        ICell cell1 = row.CreateCell(1);
                        rowNameNC = dtNC.Rows[i][columnName].ToString();
                        cell1.SetCellValue(String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(rowNameNC)));
                    }

                    if (columnName == "GetEmpresa")
                    {
                        ICell cell2 = row.CreateCell(2);
                        rowNameNC = dtNC.Rows[i][columnName].ToString();
                        cell2.SetCellValue(rowNameNC);
                    }

                    if (columnName == "Monto")
                    {
                        ICellStyle cellDateStyle = hssfworkbook.CreateCellStyle(); //create custom style
                        cellDateStyle.DataFormat = hssfworkbook.CreateDataFormat().GetFormat("#,##0.00");
                        
                        ICell cell3 = row.CreateCell(3);
                        rowNameNC = dtNC.Rows[i][columnName].ToString();
                        cell3.SetCellType(CellType.Numeric);

                        cell3.SetCellValue(Convert.ToDouble(rowNameNC));
                        cell3.CellStyle = cellDateStyle;
                    }
                }
            }
            #endregion
            #endregion

            auxRow = auxRow + 7;

            #region Tabla Notas de débito
            #region Total
            IRow _rowND = sheet1.CreateRow(auxRow);
            ICell cellND = _rowND.CreateCell(0);
            cellND.SetCellValue("Notas de débito");
            cellND.CellStyle = styleComprobante;

            auxRow = auxRow + 1;

            IRow rowTitleND = sheet1.CreateRow(auxRow);
            ICell cellTitleND = rowTitleND.CreateCell(0);
            cellTitleND.SetCellValue("Total:");
            cellTitleND.CellStyle = styleTitle;
            ICell cellTotalND = rowTitleND.CreateCell(1);
            cellTotalND.SetCellValue(Convert.ToDouble(totalND));
            cellTotalND.CellStyle = styleTotal;
            #endregion

            auxRow = auxRow + 2;
            IRow rowND = sheet1.CreateRow(auxRow);
            DataTable dtND = tablaND[0];
            
            #region Columnas
            ICell cell_EncabezadoND = rowND.CreateCell(0);
            cell_EncabezadoND.SetCellValue("NRO.");
            cell_EncabezadoND.CellStyle = styleHead;

            cell_EncabezadoND = rowND.CreateCell(1);
            cell_EncabezadoND.SetCellValue("FECHA");
            cell_EncabezadoND.CellStyle = styleHead;

            cell_EncabezadoND = rowND.CreateCell(2);
            cell_EncabezadoND.SetCellValue("CLIENTE");
            cell_EncabezadoND.CellStyle = styleHead;

            cell_EncabezadoND = rowND.CreateCell(3);
            cell_EncabezadoND.SetCellValue("IMPORTE");
            cell_EncabezadoND.CellStyle = styleHead;
            #endregion

            #region Filas
            string rowNameND = null;
            auxRow = auxRow + 1;
            int fileND = auxRow;
            //loops through data
            for (int i = 0; i < dtND.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(fileND + i);
                for (int j = 0; j < dtND.Columns.Count; j++)
                {
                    String columnName = dtND.Columns[j].ToString();

                    if (columnName == "Nro")
                    {
                        ICell cell = row.CreateCell(0);
                        rowNameND = dtND.Rows[i][columnName].ToString();
                        cell.SetCellValue(Convert.ToDouble(rowNameND));
                    }

                    if (columnName == "Fecha")
                    {
                        ICell cell1 = row.CreateCell(1);
                        rowNameND = dtND.Rows[i][columnName].ToString();
                        cell1.SetCellValue(String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(rowNameND)));
                    }

                    if (columnName == "GetEmpresa")
                    {
                        ICell cell2 = row.CreateCell(2);
                        rowNameND = dtND.Rows[i][columnName].ToString();
                        cell2.SetCellValue(rowNameND);
                    }

                    if (columnName == "Monto")
                    {
                        ICellStyle cellDateStyle = hssfworkbook.CreateCellStyle(); //create custom style
                        cellDateStyle.DataFormat = hssfworkbook.CreateDataFormat().GetFormat("#,##0.00");
                        
                        ICell cell3 = row.CreateCell(3);
                        rowNameND = dtND.Rows[i][columnName].ToString();
                        cell3.SetCellType(CellType.Numeric);

                        cell3.SetCellValue(Convert.ToDouble(rowNameND));
                        cell3.CellStyle = cellDateStyle;
                    }
                }
            }
            #endregion
            #endregion

            auxRow = auxRow + 2;
            
            #region Tabla Condonaciones
            #region Total
            IRow _rowC = sheet1.CreateRow(auxRow);
            ICell cellC = _rowC.CreateCell(0);
            cellC.SetCellValue("Condonaciones");
            cellC.CellStyle = styleComprobante;

            auxRow = auxRow + 1;

            IRow rowTitleC = sheet1.CreateRow(auxRow);
            ICell cellTitleC = rowTitleC.CreateCell(0);
            cellTitleC.SetCellValue("Total:");
            cellTitleC.CellStyle = styleTitle;
            ICell cellTotalC = rowTitleC.CreateCell(1);
            cellTotalC.SetCellValue(Convert.ToDouble(totalC));
            cellTotalC.CellStyle = styleTotal;
            #endregion

            auxRow = auxRow + 2;
            IRow rowC = sheet1.CreateRow(auxRow);
            DataTable dtC = tablaC[0];

            #region Columnas
            ICell cell_EncabezadoC = rowC.CreateCell(0);
            cell_EncabezadoC.SetCellValue("NRO.");
            cell_EncabezadoC.CellStyle = styleHead;

            cell_EncabezadoC = rowC.CreateCell(1);
            cell_EncabezadoC.SetCellValue("FECHA");
            cell_EncabezadoC.CellStyle = styleHead;

            cell_EncabezadoC = rowC.CreateCell(2);
            cell_EncabezadoC.SetCellValue("CLIENTE");
            cell_EncabezadoC.CellStyle = styleHead;

            cell_EncabezadoC = rowC.CreateCell(3);
            cell_EncabezadoC.SetCellValue("IMPORTE");
            cell_EncabezadoC.CellStyle = styleHead;
            #endregion

            #region Filas
            string rowNameC = null;
            auxRow = auxRow + 1;
            int fileC = auxRow;
            //loops through data
            for (int i = 0; i < dtC.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(fileC + i);
                for (int j = 0; j < dtC.Columns.Count; j++)
                {
                    String columnName = dtC.Columns[j].ToString();

                    if (columnName == "Nro")
                    {
                        ICell cell = row.CreateCell(0);
                        rowNameC = dtC.Rows[i][columnName].ToString();
                        cell.SetCellValue(Convert.ToDouble(rowNameC));
                    }

                    if (columnName == "Fecha")
                    {
                        ICell cell1 = row.CreateCell(1);
                        rowNameC = dtC.Rows[i][columnName].ToString();
                        cell1.SetCellValue(String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(rowNameC)));
                    }

                    if (columnName == "GetEmpresa")
                    {
                        ICell cell2 = row.CreateCell(2);
                        rowNameC = dtC.Rows[i][columnName].ToString();
                        cell2.SetCellValue(rowNameC);
                    }

                    if (columnName == "Monto")
                    {
                        ICellStyle cellDateStyle = hssfworkbook.CreateCellStyle(); //create custom style
                        cellDateStyle.DataFormat = hssfworkbook.CreateDataFormat().GetFormat("#,##0.00");
                        
                        ICell cell3 = row.CreateCell(3);
                        rowNameC = dtC.Rows[i][columnName].ToString();
                        cell3.SetCellType(CellType.Numeric);

                        cell3.SetCellValue(Convert.ToDouble(rowNameC));
                        cell3.CellStyle = cellDateStyle;
                    }
                }
            }
            #endregion
            #endregion

            auxRow = auxRow + 3;
            
            #region Total
            IRow rowTitleTotalComprobantes = sheet1.CreateRow(auxRow);
            ICell cellTitleComprobantes = rowTitleTotalComprobantes.CreateCell(0);
            cellTitleComprobantes.SetCellValue("Total Comprobantes:");
            cellTitleComprobantes.CellStyle = styleComprobante;
            ICell cellTotalComprobantes = rowTitleTotalComprobantes.CreateCell(1);
            Decimal _total = Convert.ToDecimal(totalR) + Convert.ToDecimal(totalNC) + Convert.ToDecimal(totalND) + Convert.ToDecimal(totalC);
            cellTotalComprobantes.SetCellValue(Convert.ToDouble(_total));
            cellTotalComprobantes.CellStyle = styleTotalComprobante;
            #endregion

            AutoSizeColumn(sheet1);

            //writing the data to binary from memory
            Response.BinaryWrite(WriteToStream(hssfworkbook).GetBuffer());
            Response.End();
        }
        catch (Exception ex)
        {
        }
    }

    public static void DataTableToExcelClientes(DataTable tabla, String fileName)
    {
        try
        {
            //Make a new npoi workbook
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();

            //Here I am making sure that I am giving the file name the right extension:
            string filename = "";

            if (fileName.EndsWith(".xls"))
                filename = fileName;
            else
                filename = fileName + ".xls";

            //This starts the dialogue box that allows the user to download the file
            System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            Response.Clear();

            //make a new sheet – name it any excel-compliant string you want
            ISheet sheet1 = hssfworkbook.CreateSheet("Sheet 1");

            #region Estilo
            ICellStyle styleHead = hssfworkbook.CreateCellStyle();
            styleHead.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.DarkRed.Index;
            styleHead.FillPattern = FillPattern.SolidForeground;
            styleHead.Alignment = HorizontalAlignment.Center;
            IFont font2 = hssfworkbook.CreateFont();
            font2.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.None;
            font2.FontHeightInPoints = 12;
            font2.FontName = "Calibri";
            font2.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
            styleHead.SetFont(font2);
            #endregion
            
            //make a header row
            IRow row1 = sheet1.CreateRow(0);
            DataTable dt = tabla;

            #region Columnas
            ICell cell_Encabezado = row1.CreateCell(0);
            cell_Encabezado.SetCellValue("CLIENTE");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(1);
            cell_Encabezado.SetCellValue("DOCUMENTO");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(2);
            cell_Encabezado.SetCellValue("TELÉFONO");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(3);
            cell_Encabezado.SetCellValue("MAIL");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(4);
            cell_Encabezado.SetCellValue("CUIT");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(5);
            cell_Encabezado.SetCellValue("CONDICIÓN DE IVA");
            cell_Encabezado.CellStyle = styleHead;

            cell_Encabezado = row1.CreateCell(6);
            cell_Encabezado.SetCellValue("CARÁCTER");
            cell_Encabezado.CellStyle = styleHead;
            #endregion

            string rowName = null;
            int auxRow = dt.Rows.Count;
            //loops through data
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row.CreateCell(j);
                    String columnName = dt.Columns[j].ToString();

                    rowName = dt.Rows[i][columnName].ToString();
                
                    cell.SetCellValue(rowName);
                }
            }

            AutoSizeColumn(sheet1);

            //writing the data to binary from memory
            Response.BinaryWrite(WriteToStream(hssfworkbook).GetBuffer());
            Response.End();
        }
        catch (Exception ex)
        {
        }
    }

    static MemoryStream WriteToStream(HSSFWorkbook hssfworkbook)
    {
        //Write the stream data of workbook to the root directory
        MemoryStream file = new MemoryStream();
        hssfworkbook.Write(file);

        return file;
    }

    public static void AutoSizeColumn(ISheet sheet1)
    {
        for (int i = 0; i < 40; i++)
        {
            sheet1.AutoSizeColumn(i);
        }
    }

    public static DataTable ConvertToDataTable<T>(IList<T> data)
    {
        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
        DataTable table = new DataTable();
        foreach (PropertyDescriptor prop in properties)
            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        foreach (T item in data)
        {
            DataRow row = table.NewRow();
            foreach (PropertyDescriptor prop in properties)
                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
            table.Rows.Add(row);
        }
        return table;
    }


    // Obtiene el valor de la celda cuando la misma tiene formulas
    protected static string GetUnformattedValue(ICell cell)
    {
        string returnValue = string.Empty;
        if (cell != null)
        {
            try
            {
                // Get evaluated cell value
                returnValue = (cell.CellType == CellType.Numeric ||
                (cell.CellType == CellType.Formula &&
                cell.CachedFormulaResultType == CellType.Numeric)) ?
                    formulaEvaluator.EvaluateInCell(cell).NumericCellValue.ToString() :
                    dataFormatter.FormatCellValue(cell, formulaEvaluator);
            }
            catch
            {
                // When failed in evaluating the formula, use stored values instead...
                // and set cell value for reference from formulae in other cells...
                if (cell.CellType == CellType.Formula)
                {
                    switch (cell.CachedFormulaResultType)
                    {
                        case CellType.String:
                            returnValue = cell.StringCellValue;
                            cell.SetCellValue(cell.StringCellValue);
                            break;
                        case CellType.Numeric:
                            returnValue = cell.NumericCellValue.ToString();
                            cell.SetCellValue(cell.NumericCellValue);
                            break;
                        case CellType.Boolean:
                            returnValue = cell.BooleanCellValue.ToString();
                            cell.SetCellValue(cell.BooleanCellValue);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        return (returnValue ?? string.Empty).Trim();
    }

    public static DataTable ToDataTable<T>(List<T> iList)
    {
        DataTable dataTable = new DataTable();
        PropertyDescriptorCollection propertyDescriptorCollection =
            TypeDescriptor.GetProperties(typeof(T));
        for (int i = 0; i < propertyDescriptorCollection.Count; i++)
        {
            PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
            Type type = propertyDescriptor.PropertyType;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = Nullable.GetUnderlyingType(type);


            dataTable.Columns.Add(propertyDescriptor.Name, type);
        }
        object[] values = new object[propertyDescriptorCollection.Count];
        foreach (T iListItem in iList)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
            }
            dataTable.Rows.Add(values);
        }
        return dataTable;
    }
}
