using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml.Packaging;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Dynamic;
using System.Data;
using DocumentFormat.OpenXml;
using System.Globalization;
namespace StudentTracker.Core.Utilities
{
    /// <summary>
    /// Handler for working with Excel documents
    /// </summary>
    public class ExcelHandler : IDisposable
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelHandler"/> class.
        /// </summary>
        public ExcelHandler() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelHandler"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        public ExcelHandler(string fileName, string sheetName)
        {
            OpenSheet(fileName, sheetName);
        }
        public ExcelHandler(string fileName, string sheetName, bool editMode)
        {
            OpenSheet(fileName, sheetName, editMode);
        }
        #endregion

        #region Declares

        #region Properties

        /// <summary>
        /// Gets or sets the current spreadsheet document.
        /// </summary>
        /// <value>The current document.</value>
        public SpreadsheetDocument CurrentDocument { get; private set; }
        /// <summary>
        /// Gets or sets the current filename.
        /// </summary>
        /// <value>The current filename.</value>
        public string CurrentFilename { get; private set; }
        /// <summary>
        /// Gets or sets the current sheet.
        /// </summary>
        /// <value>The current sheet.</value>
        public WorksheetPart CurrentSheet { get; set; }

        #endregion

        /// <summary>
        /// Static list of columns from A - Z
        /// </summary>
        private static readonly string[] ColumnNames = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        /// <summary>
        /// Generic list with Column names
        /// </summary>
        private static readonly List<string> ColumnNameList = new List<string>();

        #endregion

        #region Public IO functions

        #region Generel

        /// <summary>
        /// Opens the document.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="editable">if set to <c>true</c> [editable].</param>
        /// <returns></returns>
        public bool OpenDocument(string fileName, bool editable)
        {
            if (!File.Exists(fileName)) return false;
            SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(fileName, editable);
            CurrentDocument = spreadSheet;
            CurrentFilename = fileName;
            return true;
        }
        /// <summary>
        /// Opens the document.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public bool OpenDocument(string fileName)
        {
            return OpenDocument(fileName, false);
        }

        /// <summary>
        /// Opens the spreadsheet.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <returns></returns>
        public bool OpenSheet(string fileName, string sheetName)
        {
            return OpenSheet(fileName, sheetName, false);
        }

        public bool OpenSheet(string fileName, string sheetName, bool editMode)
        {
            OpenDocument(fileName, editMode);
            return OpenSheet(sheetName);
        }
        /// <summary>
        /// Opens the spreadsheet.
        /// </summary>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <returns></returns>
        public bool OpenSheet(string sheetName)
        {
            if (CurrentDocument == null) return false;
            IEnumerable<Sheet> sheets = CurrentDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>().Where(s => s.Name == sheetName);
            if (sheets.Count() == 0) return false;
            string relationshipId = sheets.First().Id.Value;
            CurrentSheet = (WorksheetPart)CurrentDocument.WorkbookPart.GetPartById(relationshipId);
            return true;
        }

        #endregion

        #region Save

        /// <summary>
        /// Saves the document.
        /// </summary>
        /// <returns>True if saved</returns>
        public bool SaveDocument()
        {
            CurrentSheet = null;
            if (CurrentDocument != null) CurrentDocument.Dispose();
            CurrentDocument = null;
            CurrentFilename = null;
            return true;
        }

        #endregion

        #region Read

        //test by Alex
        public bool ReadDocument(ref List<ExpandoObject> rows, bool hasColHeaders)
        {
            try
            {
                SharedStringTablePart sstPart = CurrentDocument.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                SharedStringTable ssTable = sstPart.SharedStringTable;
                WorkbookStylesPart workbookStylesPart = CurrentDocument.WorkbookPart.GetPartsOfType<WorkbookStylesPart>().First();
                CellFormats cellFormats = (CellFormats)workbookStylesPart.Stylesheet.CellFormats;

                rows = new List<ExpandoObject>();
                ExtractRowsData(rows, CurrentSheet.Worksheet, ssTable, cellFormats, hasColHeaders);

                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Reads the document.
        /// </summary>
        /// <param name="columnCount">Number of columns to read.</param>
        /// /// <param name="ignoreRowCount">Number of rows to ignore</param>
        /// <param name="result">The result list.</param>
        /// <returns>True if succes</returns>
        public bool ReadDocument(int ignoreRowCount, ref List<string[]> result)
        {
            var dataResult = new DataResult(DataResult.DataResultType.ListType);
            if (!ExecuteReadDocument(ignoreRowCount, ref dataResult)) return false;
            result = dataResult.GetList();
            return true;
        }

        /// <summary>
        /// Reads the document.
        /// </summary>
        /// <param name="columnCount">Number of columns to read.</param>
        /// /// <param name="ignoreRowCount">Number of rows to ignore</param>
        /// <param name="result">The result as table.</param>
        /// <returns>True if succes</returns>
        public bool ReadDocument(int ignoreRowCount, ref DataTable result)
        {
            var dataResult = new DataResult(DataResult.DataResultType.DataTableType);
            if (!ExecuteReadDocument(ignoreRowCount, ref dataResult)) return false;
            result = dataResult.GetDataTable();
            return true;
        }

        #endregion

        #endregion

        #region Public functions

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        /// <param name="colIndex">Index of the collumn (starting at 0)</param>
        /// <returns></returns>
        public static string GetColumnName(int colIndex)
        {
            if (colIndex < 0) return "#";
            if (ColumnNameList.Count <= colIndex)
            {
                for (int index = ColumnNameList.Count; index < (colIndex + 1); index++)
                {
                    string colName;
                    if (index >= ColumnNames.Length)
                    {
                        var subIndex = (int)Math.Floor((double)index / ColumnNames.Length) - 1;
                        int sufIndex = (index - ((subIndex + 1) * ColumnNames.Length));
                        colName = GetColumnName(subIndex) + GetColumnName(sufIndex);
                    }
                    else colName = ColumnNames[index];
                    ColumnNameList.Add(colName);
                }
            }
            return ColumnNameList[colIndex];
        }

        /// <summary>
        /// Creates the XML font.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <returns>Font</returns>
        public Font CreateXmlFont(System.Drawing.Font font)
        {
            var xmlFont = new Font();
            xmlFont.Append(new FontName { Val = font.Name });
            xmlFont.Append(new FontSize { Val = font.SizeInPoints });
            //xmlFont.Append(new FontFamily() {Val = font.FontFamily.Name});
            if (font.Bold) xmlFont.Append(new Bold());
            if (font.Italic) xmlFont.Append(new Italic());
            if (font.Underline) xmlFont.Append(new Underline());
            return xmlFont;
        }

        #endregion

        #region Public commands

        /// <summary>
        /// Inserts the value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="column">The column.</param>
        /// <param name="rowindex">The rowindex.</param>
        /// <param name="align">The align.</param>
        /// <param name="valueType">Type of the value.</param>
        public void InsertValue(string text, string column, int rowindex, HorizontalAlignmentValues? align, CellValues valueType)
        {
            if (CurrentSheet == null) throw new Exception("No sheet selected");
            Cell cell = LocateCell(column, rowindex);
            cell.CellValue = new CellValue(text);
            cell.DataType = new EnumValue<CellValues>(valueType);
            CurrentSheet.Worksheet.Save();
        }
        /// <summary>
        /// Inserts the value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="column">The column.</param>
        /// <param name="rowindex">The rowindex.</param>
        /// <param name="align">The align.</param>
        public void InsertValue(string text, string column, int rowindex, HorizontalAlignmentValues? align)
        {
            InsertValue(text, column, rowindex, align, CellValues.String);
        }
        /// <summary>
        /// Inserts the value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="column">The column.</param>
        /// <param name="rowindex">The rowindex.</param>
        /// <param name="valueType">Type of the value.</param>
        public void InsertValue(string text, string column, int rowindex, CellValues valueType)
        {
            InsertValue(text, column, rowindex, null, valueType);
        }
        /// <summary>
        /// Inserts the value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="column">The column.</param>
        /// <param name="rowindex">The rowindex.</param>
        public void InsertValue(string text, string column, int rowindex)
        {
            InsertValue(text, column, rowindex, CellValues.String);
        }

        /// <summary>
        /// Inserts the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="column">The column.</param>
        /// <param name="rowindex">The rowindex.</param>
        /// <param name="align">The align.</param>
        public void InsertValue(int value, string column, int rowindex, HorizontalAlignmentValues? align)
        {
            InsertValue((double)value, column, rowindex, align);
        }
        /// <summary>
        /// Inserts the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="column">The column.</param>
        /// <param name="rowindex">The rowindex.</param>
        public void InsertValue(int value, string column, int rowindex)
        {
            InsertValue((double)value, column, rowindex);
        }

        /// <summary>
        /// Inserts the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="column">The column.</param>
        /// <param name="rowindex">The rowindex.</param>
        /// <param name="align">The align.</param>
        public void InsertValue(double value, string column, int rowindex, HorizontalAlignmentValues? align)
        {
            InsertValue(value.ToString(), column, rowindex, align, CellValues.Number);
        }
        /// <summary>
        /// Inserts the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="column">The column.</param>
        /// <param name="rowindex">The rowindex.</param>
        public void InsertValue(double value, string column, int rowindex)
        {
            InsertValue(value, column, rowindex, HorizontalAlignmentValues.Right);
        }

        /// <summary>
        /// Inserts the value.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <param name="column">The column.</param>
        /// <param name="rowindex">The rowindex.</param>
        /// <param name="align">The align.</param>
        public void InsertValue(bool value, string column, int rowindex, HorizontalAlignmentValues? align)
        {
            string svalue = "0";
            if (value) svalue = "1";
            InsertValue(svalue, column, rowindex, align, CellValues.Boolean);
        }
        /// <summary>
        /// Inserts the value.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <param name="column">The column.</param>
        /// <param name="rowindex">The rowindex.</param>
        public void InsertValue(bool value, string column, int rowindex)
        {
            InsertValue(value, column, rowindex, HorizontalAlignmentValues.Center);
        }

        /// <summary>
        /// Inserts the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="column">The column.</param>
        /// <param name="rowindex">The rowindex.</param>
        public void InsertValue(DateTime value, string column, int rowindex)
        {
            InsertValue(value, column, rowindex, HorizontalAlignmentValues.Center);
        }
        /// <summary>
        /// Inserts the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="column">The column.</param>
        /// <param name="rowindex">The rowindex.</param>
        /// <param name="align">The align.</param>
        public void InsertValue(DateTime value, string column, int rowindex, HorizontalAlignmentValues? align)
        {
            if (CurrentSheet == null) throw new Exception("No sheet selected");
            string svalue;
            if (value.Hour == 0 && value.Minute == 0 && value.Millisecond == 0)
            {
                svalue = value.ToString("dd/MM-yyyy");
            }
            else svalue = value.ToString("dd/MM-yyyy HH:mm:ss");
            InsertValue(svalue, column, rowindex, align, CellValues.String);
        }

        /// <summary>
        /// Deletes the text from cell.
        /// </summary>
        /// <param name="colName">Name of the col.</param>
        /// <param name="rowIndex">Index of the row.</param>
        public void DeleteTextFromCell(string colName, uint rowIndex)
        {
            // Open the document for editing.
            if (CurrentSheet == null) return;
            // Get the cell at the specified column and row.
            Cell cell = GetSpreadsheetCell(CurrentSheet.Worksheet, colName, rowIndex);
            if (cell == null) return;

            cell.Remove();
            CurrentSheet.Worksheet.Save();

            if (cell.DataType == null) return;
            // Clean up the shared string table.
            if (cell.DataType.Value == CellValues.SharedString)
            {
                int shareStringId = int.Parse(cell.CellValue.Text);
                RemoveSharedStringItem(shareStringId);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            SaveDocument();
        }

        #endregion

        #region Private functions

        /// <summary>
        /// Reads all rows in document.
        /// </summary>
        /// <param name="columnCount">Number of columns to read.</param>
        /// <param name="ignoreRowCount">Number of rows to ignore</param>
        /// <param name="result">The result data.</param>
        /// <returns>True is succes</returns>
        private bool ExecuteReadDocument(int ignoreRowCount, ref DataResult result)
        {
            if (CurrentSheet == null) throw new Exception("No sheet selected");
            var stringTableList = GetSharedStringPart().SharedStringTable.ChildElements.ToList();
            var lastRow = CurrentSheet.Worksheet.Descendants<Row>().LastOrDefault();
            var firstRow = CurrentSheet.Worksheet.Descendants<Row>().FirstOrDefault();
            Int32 columnCount = firstRow.Descendants<Cell>().Count();
            if (lastRow == null) return false;
            var allRows = CurrentSheet.Worksheet.Descendants<Row>().ToList();
            for (var rowIndex = (1 + ignoreRowCount); rowIndex <= lastRow.RowIndex; rowIndex++)
            {
                var cellList = new List<string>();
                var cellValues = (from c in
                                      (from rows in allRows
                                       where rows.RowIndex.Value == rowIndex
                                       select rows).FirstOrDefault().Descendants<Cell>()
                                  select c).ToList();
                for (var cellIndex = 0; cellIndex <= columnCount - 1; cellIndex++)
                {
                    var colName = GetColumnName(cellIndex);
                    var cell = (from c in cellValues
                                where c.CellReference.Value.Equals(colName + rowIndex, StringComparison.CurrentCultureIgnoreCase)
                                select c).FirstOrDefault();
                    cellList.Add(GetCellValue(cell, stringTableList));
                }
                result.AddRow(cellList.ToArray());
            }
            return true;
        }


        /// <summary>
        /// Get the data using the first row as columns and the rest of the rows as data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="worksheet"></param>
        /// <param name="ssTable"></param>
        /// <param name="cellFormats"></param>
        private static void ExtractRowsData(List<ExpandoObject> data, Worksheet worksheet, SharedStringTable ssTable, CellFormats cellFormats, bool hasColHeaders)
        {
            var columnHeaders = worksheet.Descendants<Row>().First().Descendants<Cell>().Select(c => Convert.ToString(ProcessCellValue(c, ssTable, cellFormats))).ToArray();
            var columnHeadersCellReference = worksheet.Descendants<Row>().First().Descendants<Cell>().Select(c => c.CellReference.InnerText.Replace("1", string.Empty)).ToArray();
            if (!hasColHeaders)
            {
                for (int i = 0; i < columnHeaders.Length; i++)
                {
                    columnHeaders[i] = "Column" + i;
                }
            }
            var spreadsheetData = from row in worksheet.Descendants<Row>()
                                  where row.RowIndex > (hasColHeaders ? 1 : 0)
                                  select row;

            foreach (var dataRow in spreadsheetData)
            {
                dynamic row = new ExpandoObject();
                Cell[] rowCells = dataRow.Descendants<Cell>().ToArray();
                for (int i = 0; i < columnHeaders.Length; i++)
                {
                    // Find and add the correct cell to the row object
                    Cell cell = dataRow.Descendants<Cell>().Where(c => c.CellReference == columnHeadersCellReference[i] + dataRow.RowIndex).FirstOrDefault();
                    if (cell != null)
                        ((IDictionary<String, Object>)row).Add(new KeyValuePair<String, Object>(columnHeaders[i], ProcessCellValue(cell, ssTable, cellFormats)));
                    else
                        ((IDictionary<String, Object>)row).Add(new KeyValuePair<String, Object>(columnHeaders[i], ""));
                }
                data.Add(row);
            }
        }

        /// <summary>
        /// Process the valus of a cell and return a .NET value
        /// </summary>
        static Func<Cell, SharedStringTable, CellFormats, Object> ProcessCellValue = (c, ssTable, cellFormats) =>
        {
            if (c.CellValue != null)
            {
                // If there is no data type, this must be a string that has been formatted as a number
                if (c.DataType == null)
                {
                    if (c.StyleIndex != null)
                    {
                        CellFormat cf = cellFormats.Descendants<CellFormat>().ElementAt<CellFormat>(Convert.ToInt32(c.StyleIndex.Value));
                        if (cf.NumberFormatId >= 0 && cf.NumberFormatId <= 13) // This is a number
                            return Convert.ToDecimal(c.CellValue.Text);
                        else if (cf.NumberFormatId >= 14 && cf.NumberFormatId <= 22) // This is a date
                            return DateTime.FromOADate(Convert.ToDouble(c.CellValue.Text)).ToShortDateString();
                        else
                            return c.CellValue.Text;
                    }
                    else
                        return c.CellValue.Text;
                }

                switch (c.DataType.Value)
                {
                    case CellValues.SharedString:
                        return ssTable.ChildElements[Convert.ToInt32(c.CellValue.Text)].InnerText;
                    case CellValues.Boolean:
                        return c.CellValue.Text == "1" ? true : false;
                    case CellValues.Date:
                        return DateTime.FromOADate(Convert.ToDouble(c.CellValue.Text));
                    case CellValues.Number:
                        return Convert.ToDecimal(c.CellValue.Text);
                    default:
                        if (c.CellValue != null)
                            return c.CellValue.Text;
                        return string.Empty;
                }
            }
            else
                return string.Empty;

        };

        private string GetCellValue(Cell cell, List<OpenXmlElement> stringTableList)
        {
            string value = "";
            if (cell == null) return "";
            value = cell.InnerText;
            if (cell.DataType == null)
            {
                //test by alex
                //string date = DateTime.FromOADate(double.Parse(value)).ToShortDateString();
                //CellFormat cellFormat = (CellFormat)CurrentDocument.WorkbookPart.WorkbookStylesPart.Stylesheet.CellFormats.ElementAt((int)cell.StyleIndex.Value);
                //string format = CurrentDocument.WorkbookPart.WorkbookStylesPart.Stylesheet.NumberingFormats.Elements<NumberingFormat>()
                //    .Where(i => i.NumberFormatId.ToString() == cellFormat.NumberFormatId.ToString())
                //    .First().FormatCode;
                //SharedStringTablePart sstPart = CurrentDocument.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                //SharedStringTable ssTable = sstPart.SharedStringTable;
                //WorkbookStylesPart workbookStylesPart = CurrentDocument.WorkbookPart.GetPartsOfType<WorkbookStylesPart>().First();
                //CellFormats cellFormats = (CellFormats)workbookStylesPart.Stylesheet.CellFormats;
                //object temp = ProcessCellValue(cell, ssTable, cellFormats);

                return value;
            }
            if (cell.DataType.Value != CellValues.SharedString) return value;
            int sharedStrIndex;
            if (int.TryParse(value, out sharedStrIndex))
            {
                if (sharedStrIndex < stringTableList.Count)
                    value = stringTableList[sharedStrIndex].InnerText;
            }
            return value;
        }

        private static bool IsDateFormat(string dateStr)
        {
            //Remove the trailing timestamp
            dateStr = dateStr.Split(' ')[0];

            DateTime date;
            if (DateTime.TryParse(dateStr, out date))
                return true;

            if (DateTime.TryParse(dateStr, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out date))
            {
                return true;
            }

            if (DateTime.TryParse(dateStr, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowWhiteSpaces, out date))
            {
                return true;
            }

            var dateArr = dateStr.Split('-');
            if (dateArr.Length == 3)
            {
                int no;
                return int.TryParse(dateArr[0], out no) && int.TryParse(dateArr[1], out no) && int.TryParse(dateArr[2], out no);
            }

            return false;
        }

        public static string ExcelSerialDateToDMY(int nSerialDate)
        {
            // Excel/Lotus 123 have a problem with 29-02-1900. 1900 is not a leap year, but Excel/Lotus 123 think it is...

            int nDay;
            int nMonth;
            int nYear;

            if (nSerialDate == 60)
            {
                nDay = 29;
                nMonth = 2;
                nYear = 1900;

                return nDay + "-" + nMonth + "-" + nYear;
            }

            if (nSerialDate < 60)
            {
                // Because of the 29-02-1900 problem, any serial date under 60 is one off... Compensate.
                nSerialDate++;
            }

            // Modified Julian to DMY calculation with an addition of 2415019
            var l = nSerialDate + 68569 + 2415019;
            var n = (4 * l) / 146097;
            l = l - (146097 * n + 3) / 4;
            var i = (4000 * (l + 1)) / 1461001;
            l = l - (1461 * i) / 4 + 31;
            var j = (80 * l) / 2447;
            nDay = l - (2447 * j) / 80;
            l = j / 11;
            nMonth = j + 2 - (12 * l);
            nYear = 100 * (n - 49) + i + l;

            return nDay + "-" + nMonth + "-" + nYear;
        }


        /// <summary>
        /// Gets the SharedStringPart table object.
        /// </summary>
        /// <returns></returns>
        private SharedStringTablePart GetSharedStringPart()
        {
            return CurrentDocument.WorkbookPart.SharedStringTablePart;
        }

        /// <summary>
        /// Locates the cell.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="rowindex">The rowindex.</param>
        /// <returns></returns>
        private Cell LocateCell(string column, int rowindex)
        {
            if (CurrentSheet == null) throw new Exception("No sheet selected");
            Cell cell = GetSpreadsheetCell(CurrentSheet.Worksheet, column, (uint)rowindex) ??
                        InsertCellInWorksheet(column, (uint)rowindex, CurrentSheet);
            return cell;
        }

        /// <summary>
        /// Gets the spreadsheet cell.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns></returns>
        private static Cell GetSpreadsheetCell(Worksheet worksheet, string columnName, uint rowIndex)
        {
            IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Elements<Row>().Where(r => r.RowIndex == rowIndex);
            if (rows.Count() == 0) return null;

            IEnumerable<Cell> cells = rows.First().Elements<Cell>().Where(c => string.Compare(c.CellReference.Value, columnName + rowIndex, true) == 0);
            if (cells.Count() == 0) return null;

            return cells.First();
        }

        /// <summary>
        /// Inserts the cell in worksheet.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="worksheetPart">The worksheet part.</param>
        /// <returns></returns>
        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            var sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
            Cell refCell = row.Elements<Cell>().FirstOrDefault(cell => string.Compare(cell.CellReference.Value, cellReference, true) > 0);

            var newCell = new Cell { CellReference = cellReference };
            row.InsertBefore(newCell, refCell);

            worksheet.Save();
            return newCell;
        }

        #endregion

        #region Private commands

        /// <summary>
        /// Removes the shared string item.
        /// </summary>
        /// <param name="shareStringId">The share string id.</param>
        private void RemoveSharedStringItem(int shareStringId)
        {
            var remove = true;
            foreach (var worksheet in
                CurrentDocument.WorkbookPart.GetPartsOfType<WorksheetPart>().Select(part => part.Worksheet))
            {
                remove = worksheet.GetFirstChild<SheetData>().Descendants<Cell>().All(cell => cell.DataType == null || cell.DataType.Value != CellValues.SharedString || cell.CellValue.Text != shareStringId.ToString());
                if (!remove) break;
            }

            // Other cells in the document do not reference the item. Remove the item.
            if (!remove) return;
            var shareStringTablePart = GetSharedStringPart();
            if (shareStringTablePart == null) return;
            var item = shareStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(shareStringId);
            if (item == null) return;
            item.Remove();
            // Refresh all the shared string references.
            foreach (var worksheet in
                CurrentDocument.WorkbookPart.GetPartsOfType<WorksheetPart>().Select(part => part.Worksheet))
            {
                foreach (var cell in worksheet.GetFirstChild<SheetData>().Descendants<Cell>())
                {
                    if (cell.DataType == null || cell.DataType.Value != CellValues.SharedString) continue;
                    var itemIndex = int.Parse(cell.CellValue.Text);
                    if (itemIndex > shareStringId) cell.CellValue.Text = (itemIndex - 1).ToString();
                }
                worksheet.Save();
            }
            GetSharedStringPart().SharedStringTable.Save();
        }
        #endregion
    }

    #region Shared class

    public class DataResult
    {
        public enum DataResultType { DataTableType, ListType }

        private readonly DataResultType _dataResultType;
        private readonly DataTable _table;
        private readonly List<string[]> _list;
        public DataResult(DataResultType resultType)
        {
            _dataResultType = resultType;
            switch (resultType)
            {
                case DataResultType.DataTableType:
                    _table = new DataTable();
                    break;
                case DataResultType.ListType:
                    _list = new List<string[]>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("DataResult.DataResultType does not exist");
            }
        }
        public void AddRow(string[] rowData)
        {
            switch (_dataResultType)
            {
                case DataResultType.DataTableType:
                    while (_table.Columns.Count < rowData.Length)
                    {
                        _table.Columns.Add(ExcelHandler.GetColumnName(_table.Columns.Count));
                    }
                    _table.Rows.Add(rowData);
                    break;
                case DataResultType.ListType:
                    _list.Add(rowData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("DataResult.DataResultType does not exist");
            }
        }
        public DataTable GetDataTable()
        {
            return _table;
        }
        public List<string[]> GetList()
        {
            return _list;
        }
    }

    #endregion
}