using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web.Mvc;
using Corum.Models;
using Corum.Models.ViewModels;
using Corum.Models.Interfaces;
using System.Globalization;
using GemBox.Spreadsheet;
using System.Drawing;
using System;
using System.Text.RegularExpressions;
using Corum.RestRenderModels;
using Corum.Models.ViewModels.Orders;
using Corum.Models.ViewModels.OrderConcurs;
using QRCoder;
using System.Dynamic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Reflection;
using Microsoft.CSharp.RuntimeBinder;
using Corum.Models.ViewModels.Tender;

namespace Corum.RestReports
{
    public class RestReportRenderer : IReportRenderer
    {
        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        public static object GetProperty1(object o, string member)
        {
            if (o == null) throw new ArgumentNullException("o");
            if (member == null) throw new ArgumentNullException("member");
            Type scope = o.GetType();
            IDynamicMetaObjectProvider provider = o as IDynamicMetaObjectProvider;
            if (provider != null)
            {
                ParameterExpression param = Expression.Parameter(typeof(object));
                DynamicMetaObject mobj = provider.GetMetaObject(param);
                GetMemberBinder binder = (GetMemberBinder)Microsoft.CSharp.RuntimeBinder.Binder.GetMember(0, member, scope, new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(0, null) });
                DynamicMetaObject ret = mobj.BindGetMember(binder);
                BlockExpression final = Expression.Block(
                    Expression.Label(CallSiteBinder.UpdateLabel),
                    ret.Expression
                );
                LambdaExpression lambda = Expression.Lambda(final, param);
                Delegate del = lambda.Compile();
                return del.DynamicInvoke(o);
            }
            else
            {
                return o.GetType().GetProperty(member, BindingFlags.Public | BindingFlags.Instance).GetValue(o, null);
            }
        }

        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public byte[] RenderReport<T>(RestHeaderInfo Header, RestDataInfo<T> Data, RestFooterInfo Footer, RestParamsInfo Params)
        {
            CultureInfo ci = new CultureInfo(Params.Language);

            ExcelFile ef = new ExcelFile();
            ExcelWorksheet WorkSheet = ef.Worksheets.Add("report");

            int RowCountParams = 1;//количество строк данных
            //****************   отображение выбранных фильтров   *****************************************************/            
            RenderReportParams(Params, WorkSheet, ref RowCountParams);

            int RowCount = Data.Rows.Count();//количество строк данных
            int HeaderCount = Header.Headers.Count();

            /***************************  стили  **********************************************************************/
            RenderReportStyle(Header, WorkSheet, RowCount, HeaderCount, RowCountParams);

            //заголовок отчета
            WorkSheet.Cells["F1"].Value = Params.MainHeader;

            //Дата и время создания файла            
            WorkSheet.Cells["K1"].Value = "Дата и время создания файла:" + DateTime.Now;
            WorkSheet.Cells["K1"].Style.Font.Color = Color.Black;

            Dictionary<int, int> columnNumber = new Dictionary<int, int>();
            RenderReportHeader(Header, WorkSheet, RowCountParams, ref columnNumber);

            // *************************     запись данных в таблицу    ************************************************/
            RenderReportBody<T>(Header, Data, WorkSheet, RowCountParams);

            //************************** итоговые цифры  **************************************************************/         
            RowCount = RowCount + 2 + RowCountParams;
            RenderReportFooter(Header, Footer, WorkSheet, RowCount, columnNumber);

            byte[] fileContents;
            var options = SaveOptions.XlsxDefault;

            using (var stream = new MemoryStream())
            {
                ef.Save(stream, options);
                fileContents = stream.ToArray();
            }
            return fileContents;
        }
        private void RenderReportStyle(RestHeaderInfo Header, ExcelWorksheet WorkSheet, int RowCount, int HeaderCount, int RowCountParams)
        {
            int i, j = 1;
            //шрифт 10 для всех ячеек
            WorkSheet.Cells.Style.Font.Size = 10 * 20;
            //шрифт заголовка + сделать жирным
            WorkSheet.Cells["F1"].Style.Font.Size = 12 * 20;
            WorkSheet.Cells["F1"].Style.Font.Weight = ExcelFont.BoldWeight;

            //оформление ширины колонок и цвета текста в них 
            int columnOrder = 2;
            foreach (var headers in Header.Headers.OrderBy(o => o.columnOrder))
            {
                WorkSheet.Columns[GetExcelColumnName(columnOrder)].Width = headers.columnWidth * 256;
                columnOrder++;
            }

            WorkSheet.Cells["F1"].Style.Font.Color = Color.Black;//для заголовка          
            WorkSheet.Cells["F1"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;//для заголовка

            //стиль заголовков таблицы            
            CellStyle tmpStyle = new CellStyle();
            tmpStyle.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            tmpStyle.VerticalAlignment = VerticalAlignmentStyle.Center;
            tmpStyle.FillPattern.SetSolid(Color.Chocolate);
            tmpStyle.Font.Weight = ExcelFont.BoldWeight;
            tmpStyle.Font.Color = Color.Black;
            tmpStyle.Font.Size = 10 * 20;
            tmpStyle.WrapText = true;

            //оформление стиля заголовков таблицы
            for (i = 1; i <= HeaderCount; i++)
            {
                WorkSheet.Cells[RowCountParams, i].Style = tmpStyle;
            }

            // границы (для всех заполненных ячеек рисуем ВСЕ границы)
            for (i = RowCountParams; i < RowCount + 2 + RowCountParams; i++)
            {
                for (j = 1; j <= HeaderCount; j++)
                {
                    WorkSheet.Cells[i, j].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.Black, LineStyle.Thin);
                    WorkSheet.Cells[i, j].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
                    WorkSheet.Cells[i, j].Style.WrapText = true;

                }
            }

            // границы (для определенных столбцов делаем жирной правую границу)       
            for (i = RowCountParams; i < RowCount + 2 + RowCountParams; i++)
            {
                int headerColumnOrder = 2;
                //левая граница у таблицы
                WorkSheet.Cells[i, headerColumnOrder - 1].Style.Borders.SetBorders(MultipleBorders.Left, Color.Black, LineStyle.Medium);

                foreach (var headers in Header.Headers.OrderBy(o => o.columnOrder))
                {
                    WorkSheet.Columns[GetExcelColumnName(headerColumnOrder)].Width = headers.columnWidth * 256;

                    if (headers.ColumnBlockStart)
                        WorkSheet.Cells[i, headerColumnOrder - 1].Style.Borders.SetBorders(MultipleBorders.Left, Color.Black, LineStyle.Medium);

                    if (headers.ColumnBlockEnd)
                        WorkSheet.Cells[i, headerColumnOrder - 1].Style.Borders.SetBorders(MultipleBorders.Right, Color.Black, LineStyle.Medium);
                    headerColumnOrder++;
                }
                //правая граница у таблицы
                if (headerColumnOrder > 3)
                    WorkSheet.Cells[i, headerColumnOrder - 2].Style.Borders.SetBorders(MultipleBorders.Right, Color.Black, LineStyle.Medium);
            }

            //границы (последняя итоговая строка имеет жирный шрифт + верхняя граница жирная + выравнивание по правому краю)
            for (j = 1; j <= HeaderCount; j++)
            {
                WorkSheet.Cells[RowCount + 1 + RowCountParams, j].Style.Borders.SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Medium);
                WorkSheet.Cells[RowCount + 1 + RowCountParams, j].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells[RowCount + 1 + RowCountParams, j].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                // WorkSheet.Cells[RowCount + 2, j].Value = "Итог";
            }
        }
        private void RenderReportHeader(RestHeaderInfo Header, ExcelWorksheet WorkSheet, int RowCountParams, ref Dictionary<int, int> columnNumber)
        {
            String NumRowStr = (RowCountParams + 1).ToString();
            int columnOrder = 2;
            foreach (var headers in Header.Headers.OrderBy(o => o.columnOrder))
            {
                if (headers.ColumnType == 0)
                {
                    WorkSheet.Columns[GetExcelColumnName(columnOrder)].Style.NumberFormat = "0.00";
                }
                WorkSheet.Cells[GetExcelColumnName(columnOrder) + NumRowStr].Value = headers.columnName;

                //заполняем таблицу соответствия индекса из headers и рельного индекса при выводе отчета
                columnNumber.Add(headers.columnOrder, columnOrder);

                columnOrder++;
                //WorkSheet.Cells[GetExcelColumnName(headers.columnOrder) + NumRowStr].Value = headers.columnName;
            }
        }
        private void RenderReportBody<T>(RestHeaderInfo Header, RestDataInfo<T> Data, ExcelWorksheet WorkSheet, int RowCountParams)
        {

            foreach (var data in Data.Rows)
            {
                String NumRowStr = (Data.Rows.IndexOf(data) + 2 + RowCountParams).ToString();
                int columnOrder = 2;
                foreach (var headers in Header.Headers.OrderBy(o => o.columnOrder))
                {
                    if (headers.columnField.Length > 0)
                        WorkSheet.Cells[GetExcelColumnName(columnOrder) + NumRowStr].Value = GetPropValue(data, headers.columnField);
                    else
                        WorkSheet.Cells[GetExcelColumnName(columnOrder) + NumRowStr].Value = 0;
                    columnOrder++;
                }
            }
        }
        private void RenderReportParams(RestParamsInfo Param, ExcelWorksheet WorkSheet, ref int RowCountParams)
        {
            //  int RowEnd = RowCount + 4;//заголовок отчета + заголовок таблицы + кол-во строк в таблице + итог

            foreach (var param in Param.Params)
            {
                WorkSheet.Cells[RowCountParams, 1].Value = param.Key + param.Value;
                RowCountParams++;
            }
        }
        private void RenderReportFooter(RestHeaderInfo Header, RestFooterInfo Footer, ExcelWorksheet WorkSheet, int RowCount, Dictionary<int, int> columnNumber)
        {
            string NumRowStrAll = RowCount.ToString();

            foreach (var footers in Footer.Footers.OrderBy(o => o.Key))
            {
                var FieldNumber = columnNumber.Where(o => o.Key == footers.Key).FirstOrDefault();

                if (!(string.IsNullOrEmpty(footers.Value.ToString())))
                {
                    var FieldType = Header.Headers.Where(o => o.columnOrder == footers.Key).FirstOrDefault();

                    if (FieldType.ColumnType == 0)
                        WorkSheet.Cells[GetExcelColumnName(FieldNumber.Value) + NumRowStrAll].Value = (Decimal)footers.Value;
                    else if (FieldType.ColumnType == 1)
                        WorkSheet.Cells[GetExcelColumnName(FieldNumber.Value) + NumRowStrAll].Value = (int)footers.Value;
                    //else if (FieldType.ColumnType == 2)
                    //   WorkSheet.Cells[GetExcelColumnName(FieldNumber.Value) + NumRowStrAll].Value = (DateTime)footers.Value;                   
                    else if ((FieldType.ColumnType == 2) && (FieldType.ColumnType == 3) && (FieldType.ColumnType == 4)) WorkSheet.Cells[GetExcelColumnName(FieldNumber.Value) + NumRowStrAll].Value = (String)footers.Value;
                }
                else WorkSheet.Cells[GetExcelColumnName(FieldNumber.Value) + NumRowStrAll].Value = footers.Value;
            }
        }
        public byte[] FinalReportRenderReport<T>(RestDataInfo<T> Data, RestParamsInfo Params, List<string> orderStatus)
        {
            ExcelFile ef = new ExcelFile();
            CultureInfo ci = new CultureInfo(Params.Language);
            ExcelWorksheet WorkSheet = ef.Worksheets.Add("report");
            WorkSheet.PrintOptions.Portrait = false;
            WorkSheet.PrintOptions.PaperType = PaperType.A4;
            WorkSheet.PrintOptions.FitWorksheetWidthToPages = 1;

            //шрифт 10 для всех ячеек
            WorkSheet.Cells.Style.Font.Size = 11 * 20;

            //ширина колонок            
            WorkSheet.Columns[0].Width = 15 * 256;
            WorkSheet.Columns[1].Width = 17 * 256;

            //шрифт заголовка + сделать жирным
            WorkSheet.Cells["E3"].Style.Font.Size = 14 * 20;
            WorkSheet.Cells["E3"].Style.Font.Weight = ExcelFont.BoldWeight;

            //заголовок отчета
            WorkSheet.Cells["E3"].Value = Params.MainHeader;

            //Дата и время создания файла            
            WorkSheet.Cells["K2"].Value = "Дата и время создания файла:" + DateTime.Now;
            WorkSheet.Cells["K2"].Style.Font.Size = 8 * 20;

            //шапка
            WorkSheet.Cells["A4"].Value = "Общее число";
            WorkSheet.Cells["B4"].Value = "Заявка в работе";

            for (int j = 3; j < orderStatus.Count() + 3; j++)
            {
                WorkSheet.Cells[GetExcelColumnName(j) + 4].Value = orderStatus[j - 3];
            }

            //оформление стиля заголовков таблицы
            CellStyle tmpStyle = new CellStyle();
            tmpStyle.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            tmpStyle.VerticalAlignment = VerticalAlignmentStyle.Center;
            tmpStyle.FillPattern.SetSolid(Color.Chocolate);
            tmpStyle.Font.Weight = ExcelFont.BoldWeight;
            tmpStyle.Font.Color = Color.Black;
            tmpStyle.Font.Size = 10 * 20;
            tmpStyle.WrapText = true;

            for (int i = 1; i <= orderStatus.Count() + 2; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + 4].Style = tmpStyle;
                WorkSheet.Cells[GetExcelColumnName(i) + 4 /*+ j*/].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);
            }

            //вывод данных
            int RowCount = 5;

            foreach (var data in Data.Rows)
            {
                /*  Dictionary<string, object> values = data.GetType()
                                       .GetProperties()
                                       .ToDictionary(p => p.Name, p => p.GetValue(data));

                  var len = values.Count();*/
                WorkSheet.Cells["A" + RowCount].Value = GetProperty1(data, "CntAll"); //GetPropValue(data, "CntAll"); //1

                WorkSheet.Cells["B" + RowCount].Value = GetProperty1(data, "CntAllNotFinal"); //2
                var lengthData = data.GetType().GetProperties();

                for (int j = 3; j < orderStatus.Count() + 3; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(j) + RowCount].Value = GetProperty1(data, "NewProp" + j.ToString()); //2
                }

                RowCount++;
            }

            //даже если данные не выводились, шапку добавить надо           
            for (int i = 1; i <= orderStatus.Count() + 2; i++)
            {
                for (int j = 5; j < RowCount; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.WrapText = true;
                }
            }
            /*
            for (int i = 1; i <= 2; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.FillPattern.SetSolid(Color.Chocolate);
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
            }*/

            byte[] fileContents;
            var options = SaveOptions.XlsxDefault;

            using (var stream = new MemoryStream())
            {
                ef.Save(stream, options);
                fileContents = stream.ToArray();
            }
            return fileContents;
        }
        public byte[] OrdersReportRenderReport<T>(RestDataInfo<T> Data, RestParamsInfo Params, List<string> balanceKeepers)
        {
            ExcelFile ef = new ExcelFile();
            CultureInfo ci = new CultureInfo(Params.Language);
            ExcelWorksheet WorkSheet = ef.Worksheets.Add("report");
            WorkSheet.PrintOptions.Portrait = false;
            WorkSheet.PrintOptions.PaperType = PaperType.A4;
            WorkSheet.PrintOptions.FitWorksheetWidthToPages = 1;

            //шрифт 10 для всех ячеек
            WorkSheet.Cells.Style.Font.Size = 11 * 20;

            //ширина колонок            
            WorkSheet.Columns[0].Width = 15 * 256;
            WorkSheet.Columns[1].Width = 17 * 256;

            //шрифт заголовка + сделать жирным
            WorkSheet.Cells["E3"].Style.Font.Size = 14 * 20;
            WorkSheet.Cells["E3"].Style.Font.Weight = ExcelFont.BoldWeight;

            //заголовок отчета
            WorkSheet.Cells["E3"].Value = Params.MainHeader;

            //Дата и время создания файла            
            WorkSheet.Cells["K2"].Value = "Дата и время создания файла:" + DateTime.Now;
            WorkSheet.Cells["K2"].Style.Font.Size = 8 * 20;

            //шапка
            WorkSheet.Cells["A4"].Value = "Выполнение заявок (плановые/срочные)";
            WorkSheet.Cells["B4"].Value = "Кол-во заявок";

            for (int j = 3; j < balanceKeepers.Count() + 3; j++)
            {
                WorkSheet.Cells[GetExcelColumnName(j) + 4].Value = balanceKeepers[j - 3];
            }

            //оформление стиля заголовков таблицы
            CellStyle tmpStyle = new CellStyle();
            tmpStyle.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            tmpStyle.VerticalAlignment = VerticalAlignmentStyle.Center;
            tmpStyle.FillPattern.SetSolid(Color.Chocolate);
            tmpStyle.Font.Weight = ExcelFont.BoldWeight;
            tmpStyle.Font.Color = Color.Black;
            tmpStyle.Font.Size = 10 * 20;
            tmpStyle.WrapText = true;

            for (int i = 1; i <= balanceKeepers.Count() + 2; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + 4].Style = tmpStyle;
                WorkSheet.Cells[GetExcelColumnName(i) + 4 /*+ j*/].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);
            }

            //вывод данных
            int RowCount = 5;
            foreach (var data in Data.Rows)
            {

                WorkSheet.Cells["A" + RowCount].Value = GetProperty1(data, "CntName");
                WorkSheet.Cells["B" + RowCount].Value = GetProperty1(data, "CntOrders");

                var lengthData = data.GetType().GetProperties();
                for (int j = 3; j < balanceKeepers.Count() + 3; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(j) + RowCount].Value = GetProperty1(data, "NewProp" + j.ToString()); //2
                }

                RowCount++;
            }

            //даже если данные не выводились, шапку добавить надо           

            for (int i = 1; i <= balanceKeepers.Count() + 2; i++)
            {
                for (int j = 5; j < RowCount; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.WrapText = true;
                }
            }

            byte[] fileContents;
            var options = SaveOptions.XlsxDefault;

            using (var stream = new MemoryStream())
            {
                ef.Save(stream, options);
                fileContents = stream.ToArray();
            }
            return fileContents;
        }
        public byte[] BaseReportRenderReport<T>(RestDataInfo<T> Data, RestParamsInfo Params, Dictionary<string, int> orderFinalStatusesDict, int SumPlanCarNumber, int SumFactCarNumber)
        {
            ExcelFile ef = new ExcelFile();
            CultureInfo ci = new CultureInfo(Params.Language);
            ExcelWorksheet WorkSheet = ef.Worksheets.Add("report");
            WorkSheet.PrintOptions.Portrait = false;
            WorkSheet.PrintOptions.PaperType = PaperType.A4;
            WorkSheet.PrintOptions.FitWorksheetWidthToPages = 1;

            //шрифт 10 для всех ячеек
            WorkSheet.Cells.Style.Font.Size = 11 * 20;

            //ширина колонок            
            WorkSheet.Columns[0].Width = 15 * 256;
            WorkSheet.Columns[1].Width = 15 * 256;

            WorkSheet.Columns[2].Width = 15 * 256;
            WorkSheet.Columns[3].Width = 17 * 256;
            WorkSheet.Columns[4].Width = 19 * 256;
            WorkSheet.Columns[5].Width = 19 * 256;
            WorkSheet.Columns[6].Width = 21 * 256;
            WorkSheet.Columns[7].Width = 21 * 256;
            WorkSheet.Columns[8].Width = 19 * 256;
            WorkSheet.Columns[9].Width = 12 * 256;
            WorkSheet.Columns[10].Width = 28 * 256;
            WorkSheet.Columns[11].Width = 28 * 256;
            WorkSheet.Columns[12].Width = 28 * 256;

            WorkSheet.Columns[13].Width = 16 * 256;
            WorkSheet.Columns[14].Width = 16 * 256;

            WorkSheet.Columns[15].Width = 14 * 256;
            WorkSheet.Columns[16].Width = 20 * 256;
            WorkSheet.Columns[17].Width = 15 * 256;
            WorkSheet.Columns[18].Width = 13 * 256;

            //шрифт заголовка + сделать жирным
            WorkSheet.Cells["E3"].Style.Font.Size = 14 * 20;
            WorkSheet.Cells["E3"].Style.Font.Weight = ExcelFont.BoldWeight;

            //заголовок отчета
            WorkSheet.Cells["E3"].Value = Params.MainHeader;

            //Дата и время создания файла            
            WorkSheet.Cells["K3"].Value = "Дата и время создания файла:" + DateTime.Now;
            WorkSheet.Cells["K3"].Style.Font.Size = 8 * 20;

            //шапка
            WorkSheet.Cells["A4"].Value = "Номер заявки";
            WorkSheet.Cells["B4"].Value = "Тип поездки";

            WorkSheet.Cells["C4"].Value = "Дата подачи заявки";
            WorkSheet.Cells["D4"].Value = "Дата плановой подачи авто";
            WorkSheet.Cells["E4"].Value = "Плательщик перевозки";
            WorkSheet.Cells["F4"].Value = "ЦФО";
            WorkSheet.Cells["G4"].Value = "Подразделение";
            WorkSheet.Cells["H4"].Value = "Тип груза";
            WorkSheet.Cells["I4"].Value = "Груз";

            WorkSheet.Cells["J4"].Value = "Вес груза,т.";
            WorkSheet.Cells["K4"].Value = "Грузоотправитель";
            WorkSheet.Cells["L4"].Value = "Пункт отправления";

            WorkSheet.Cells["M4"].Value = "Грузополучатель";
            WorkSheet.Cells["N4"].Value = "Пункт прибытия";

            WorkSheet.Cells["O4"].Value = "Тип авто / кузова";
            WorkSheet.Cells["P4"].Value = "Признак срочной заявки";
            WorkSheet.Cells["Q4"].Value = "Автор заявки";

            //оформление стиля заголовков таблицы
            CellStyle tmpStyle = new CellStyle();
            tmpStyle.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            tmpStyle.VerticalAlignment = VerticalAlignmentStyle.Center;
            tmpStyle.FillPattern.SetSolid(Color.Chocolate);
            tmpStyle.Font.Weight = ExcelFont.BoldWeight;
            tmpStyle.Font.Color = Color.Black;
            tmpStyle.Font.Size = 10 * 20;
            tmpStyle.WrapText = true;

            for (int i = 1; i <= 17; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + 4].Style = tmpStyle;
                WorkSheet.Cells[GetExcelColumnName(i) + 4 /*+ j*/].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);
            }

            //вывод данных
            int RowCount = 5;
            int ii = 13;
            foreach (var data in Data.Rows)
            {
                // for (int i = 1; i <= 14; i++)
                //{
                WorkSheet.Cells["A" + RowCount].Value = GetPropValue(data, "Id"); //1

                WorkSheet.Cells["B" + RowCount].Value = GetPropValue(data, "OrderTypename"); //2


                WorkSheet.Cells["C" + RowCount].Value = GetPropValue(data, "OrderDate"); //1

                WorkSheet.Cells["D" + RowCount].Value = GetPropValue(data, "AcceptDate"); //2
                WorkSheet.Cells["E" + RowCount].Value = GetPropValue(data, "PayerName"); //3 

                WorkSheet.Cells["F" + RowCount].Value = GetPropValue(data, "ClientCenterName").ToString();

                WorkSheet.Cells["G" + RowCount].Value = GetPropValue(data, "ClientName"); //4

                WorkSheet.Cells["H" + RowCount].Value = GetPropValue(data, "TruckTypeName"); //5
                WorkSheet.Cells["I" + RowCount].Value = GetPropValue(data, "TruckDescription"); //6
                WorkSheet.Cells["J" + RowCount].Value = GetPropValue(data, "Weight"); //7
                WorkSheet.Cells["K" + RowCount].Value = GetPropValue(data, "Shipper").ToString();
                WorkSheet.Cells["L" + RowCount].Value = GetPropValue(data, "FromInfo").ToString();


                WorkSheet.Cells["M" + RowCount].Value = GetPropValue(data, "Consignee").ToString();
                WorkSheet.Cells["N" + RowCount].Value = GetPropValue(data, "ToInfo").ToString();


                WorkSheet.Cells["O" + RowCount].Value = GetPropValue(data, "VehicleTypeName"); //10

                string PriorityType = GetPropValue(data, "PriorityType").ToString(); //11

                if (PriorityType == "0")
                    WorkSheet.Cells["P" + RowCount].Value = "Плановая";
                else
                    WorkSheet.Cells["P" + RowCount].Value = "Срочная";
                WorkSheet.Cells["Q" + RowCount].Value = GetPropValue(data, "OrdersAuthor"); //12

                ii = 15 + 3;
                foreach (var finStatus in orderFinalStatusesDict)
                {
                    string CurrentOrderStatusName = GetPropValue(data, "CurrentOrderStatusName").ToString();
                    WorkSheet.Cells[GetExcelColumnName(ii) + 4].Value = finStatus.Key; //CurrentOrderStatusName;
                    WorkSheet.Cells[GetExcelColumnName(ii) + 4].Style = tmpStyle;
                    WorkSheet.Cells[GetExcelColumnName(ii) + 4].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);

                    if (finStatus.Key == CurrentOrderStatusName)
                        WorkSheet.Cells[GetExcelColumnName(ii) + RowCount].Value = "1";
                    else
                        WorkSheet.Cells[GetExcelColumnName(ii) + RowCount].Value = "0";

                    WorkSheet.Cells[GetExcelColumnName(ii) + RowCount].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);

                    WorkSheet.Cells[GetExcelColumnName(ii) + RowCount].Style.HorizontalAlignment =
                        HorizontalAlignmentStyle.Right;

                    ii++;
                }

                WorkSheet.Cells[GetExcelColumnName(ii) + 4].Value = "Примечание/Дислокация груза";
                WorkSheet.Cells[GetExcelColumnName(ii) + 4].Style = tmpStyle;
                WorkSheet.Cells[GetExcelColumnName(ii) + 4].Style.Borders.SetBorders(
                    MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);

                WorkSheet.Cells[GetExcelColumnName(ii) + RowCount].Value = GetPropValue(data, "ExecuterNotes"); //
                WorkSheet.Cells[GetExcelColumnName(ii) + RowCount].Style.Borders.SetBorders(
                    MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);

                ii++;
                WorkSheet.Cells[GetExcelColumnName(ii) + 4].Value = "Признак отказа заказчика";
                WorkSheet.Cells[GetExcelColumnName(ii) + 4].Style = tmpStyle;
                WorkSheet.Cells[GetExcelColumnName(ii) + 4].Style.Borders.SetBorders(
                    MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);

                WorkSheet.Cells[GetExcelColumnName(ii) + RowCount].Value = GetPropValue(data, "FinalComment");
                WorkSheet.Cells[GetExcelColumnName(ii) + RowCount].Style.Borders.SetBorders(
                    MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);


                ii++;
                WorkSheet.Cells[GetExcelColumnName(ii) + 4].Value = "Авто план/факт";
                WorkSheet.Cells[GetExcelColumnName(ii) + 4].Style = tmpStyle;
                WorkSheet.Cells[GetExcelColumnName(ii) + 4].Style.Borders.SetBorders(
                    MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);

                WorkSheet.Cells[GetExcelColumnName(ii) + RowCount].Value = GetPropValue(data, "AvtoPlanFact");
                WorkSheet.Cells[GetExcelColumnName(ii) + RowCount].Style.Borders.SetBorders(
                    MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);

                RowCount++;
            }

            //даже если данные не выводились, шапку добавить надо
            if (Data.Rows.Count == 0)
            {
                WorkSheet.Cells[GetExcelColumnName(ii) + 4].Value = "Проблемы при выполнении";
                WorkSheet.Cells[GetExcelColumnName(ii) + 4].Style = tmpStyle;
                WorkSheet.Cells[GetExcelColumnName(ii) + 4].Style.Borders.SetBorders(
                    MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);

                ii++;
                WorkSheet.Cells[GetExcelColumnName(ii) + 4].Value = "Признак отказа заказчика";
                WorkSheet.Cells[GetExcelColumnName(ii) + 4].Style = tmpStyle;
                WorkSheet.Cells[GetExcelColumnName(ii) + 4].Style.Borders.SetBorders(
                    MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);

                ii++;
                WorkSheet.Cells[GetExcelColumnName(ii) + 4].Value = "Авто план/факт";
                WorkSheet.Cells[GetExcelColumnName(ii) + 4].Style = tmpStyle;
                WorkSheet.Cells[GetExcelColumnName(ii) + 4].Style.Borders.SetBorders(
                    MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);

            }



            for (int i = 1; i <= 17; i++)
            {
                for (int j = 5; j <= RowCount; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.WrapText = true;
                }
            }

            for (int i = 1; i <= 17; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.FillPattern.SetSolid(Color.Chocolate);
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
            }


            for (int i = 17; i <= ii; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.FillPattern.SetSolid(Color.Chocolate);
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;

                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.Borders.SetBorders(
                MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                Color.Black, LineStyle.Thin);
            }

            //итог
            WorkSheet.Cells["A" + RowCount].Value = "Общий итог";

            ii = 15 + 3;
            foreach (var finStatus in orderFinalStatusesDict)
            {
                WorkSheet.Cells[GetExcelColumnName(ii) + RowCount].Value = finStatus.Value;
                ii++;
            }

            ii = ii + 2;
            WorkSheet.Cells[GetExcelColumnName(ii) + RowCount].Value = SumPlanCarNumber.ToString() + "/" + SumFactCarNumber.ToString();

            byte[] fileContents;
            var options = SaveOptions.XlsxDefault;

            using (var stream = new MemoryStream())
            {
                ef.Save(stream, options);
                fileContents = stream.ToArray();
            }
            return fileContents;
        }
        public byte[] OrderRenderReport<T>(OrderBaseViewModel OrderTypeModel, OrdersPassTransportViewModel extOrderTypeModel, string AcceptDate, OrderClientsViewModel orderClientInfo, RestParamsInfo Params, string AdressFrom, string AdressTo, string ContractName, OrdersTruckTransportViewModel extOrderTypeModel2, int OrderType, List<OrderUsedCarViewModel> carList)
        {
            //легковой транспорт
            if (OrderType == 6)
                return PassOrderRenderReport(OrderTypeModel, extOrderTypeModel, AcceptDate, orderClientInfo, Params,
                    AdressFrom, AdressTo, ContractName, carList);
            else
                //грузовой транспорт
                return TruckOrderRenderReport(OrderTypeModel, AcceptDate, orderClientInfo, Params, AdressFrom, AdressTo,
                    ContractName, extOrderTypeModel2, carList);

        }

        public byte[] OrderRenderReport<T>(OrderBaseViewModel OrderTypeModel, OrdersPassTransportViewModel extOrderTypeModel, string AcceptDate, OrderClientsViewModel orderClientInfo, RestParamsInfo Params, string AdressFrom, string AdressTo, string ContractName, OrdersTruckTransportViewModel extOrderTypeModel2, int OrderType, List<OrderUsedCarViewModel> carList, DataToAndFromContragent data)
        {
            //легковой транспорт
            if (OrderType == 6)
                return PassOrderRenderReport(OrderTypeModel, extOrderTypeModel, AcceptDate, orderClientInfo, Params,
                    AdressFrom, AdressTo, ContractName, carList);
            else
                //грузовой транспорт
                return TruckOrderRenderReport(OrderTypeModel, AcceptDate, orderClientInfo, Params, AdressFrom, AdressTo,
                    ContractName, extOrderTypeModel2, carList, data);

        }
        public byte[] OrderRenderReport<T>(OrderBaseViewModel OrderTypeModel, OrdersPassTransportViewModel extOrderTypeModel, string AcceptDate, OrderClientsViewModel orderClientInfo, RestParamsInfo Params, string AdressFrom, string AdressTo, string ContractName, OrdersTruckTransportViewModel extOrderTypeModel2, int OrderType, List<OrderUsedCarViewModel> carList, List<DataToAndFromContragent> data)
        {
            //легковой транспорт
            if (OrderType == 6)
                return PassOrderRenderReport(OrderTypeModel, extOrderTypeModel, AcceptDate, orderClientInfo, Params,
                    AdressFrom, AdressTo, ContractName, carList);
            else
                //грузовой транспорт
                return TruckOrderRenderReport(OrderTypeModel, AcceptDate, orderClientInfo, Params, AdressFrom, AdressTo,
                    ContractName, extOrderTypeModel2, carList, data);

        }
        private byte[] PassOrderRenderReport(OrderBaseViewModel OrderTypeModel, OrdersPassTransportViewModel extOrderTypeModel, string AcceptDate, OrderClientsViewModel orderClientInfo, RestParamsInfo Params, string AdressFrom, string AdressTo, string ContractName, List<OrderUsedCarViewModel> carList)
        {
            //Пример генерации QR кода
            string UrlForEncoding =
#if DEBUG

                                $"http://uh218479-1.ukrdomen.com/Orders/UpdateOrder/{OrderTypeModel.Id}";
#else
                              
                                $"https://corumsource.com/Orders/UpdateOrder/{OrderTypeModel.Id}";
#endif


            ExcelFile ef = new ExcelFile();
            CultureInfo ci = new CultureInfo(Params.Language);

            ExcelWorksheet WorkSheet = ef.Worksheets.Add("report");


            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(UrlForEncoding, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            WorkSheet.PrintOptions.PaperType = PaperType.A4;
            WorkSheet.PrintOptions.FitWorksheetWidthToPages = 1;

            var imgStream = new MemoryStream();
            qrCodeImage.Save(imgStream, System.Drawing.Imaging.ImageFormat.Png);


            WorkSheet.Pictures.Add(imgStream,
                    PositioningMode.MoveAndSize,
                    new AnchorCell(WorkSheet.Columns[5], WorkSheet.Rows[0], 5, 5, LengthUnit.Pixel),
                    new AnchorCell(WorkSheet.Columns[5], WorkSheet.Rows[3], 85, 85, LengthUnit.Pixel),
                    ExcelPictureFormat.Png);
            //шрифт 10 для всех ячеек
            WorkSheet.Cells.Style.Font.Size = 11 * 20;
            WorkSheet.Cells.Style.Font.Name = "Times New Roman";


            //ширина колонок            
            WorkSheet.Columns[0].Width = 4 * 256;
            WorkSheet.Columns[1].Width = 17 * 256;
            WorkSheet.Columns[2].Width = 22 * 256; //18 * 256;
            WorkSheet.Columns[3].Width = 33 * 256;
            WorkSheet.Columns[4].Width = 20 * 256;
            WorkSheet.Columns[5].Width = 14 * 256;
            WorkSheet.Columns[6].Width = 21 * 256;
            WorkSheet.Columns[7].Width = 24 * 256;
            WorkSheet.Columns[8].Width = 25 * 256;

            //шрифт заголовка + сделать жирным
            WorkSheet.Cells["C1"].Style.Font.Size = 14 * 20;
            WorkSheet.Cells["C1"].Style.Font.Weight = ExcelFont.BoldWeight;

            //заголовок отчета
            WorkSheet.Cells["C1"].Value = Params.MainHeader + " № " + OrderTypeModel.Id.ToString();

            //Дата и время создания файла            
            WorkSheet.Cells["G1"].Value = "Дата и время создания файла:" + DateTime.Now;
            WorkSheet.Cells["G1"].Style.Font.Size = 8 * 20;

            WorkSheet.Cells["B2"].Value = "Дата заявки";
            WorkSheet.Cells["C2"].Value = OrderTypeModel.OrderDate;
            WorkSheet.Cells["B2"].Style.Font.Size = 8 * 20;
            WorkSheet.Cells["C2"].Style.Font.Size = 8 * 20;

            WorkSheet.Cells["B3"].Value = "Дата утверждения";
            WorkSheet.Cells["C3"].Value = AcceptDate;
            WorkSheet.Cells["B3"].Style.Font.Size = 8 * 20;
            WorkSheet.Cells["C3"].Style.Font.Size = 8 * 20;


            for (int i = 4; i <= 13; i++)
            {
                WorkSheet.Cells["B" + i].Style.Font.Weight = ExcelFont.BoldWeight;
            }

            WorkSheet.Cells["B4"].Value = "Заказчик";

            WorkSheet.Cells["D4"].Value = OrderTypeModel.PayerName;//ClientBalanceKeeperName;
            WorkSheet.Cells.GetSubrangeAbsolute(3, 3, 3, 7).Merged = true;
            WorkSheet.Cells["B5"].Value = "договор №";
            WorkSheet.Cells["D5"].Value = ContractName;
            WorkSheet.Cells["B6"].Value = "Ф.И.О. должность контактного лица";
            WorkSheet.Cells["D6"].Value = OrderTypeModel.CreatorPosition;
            WorkSheet.Cells.GetSubrangeAbsolute(5, 3, 5, 7).Merged = true;
            WorkSheet.Cells["B7"].Value = "контактный телефон Заказчика";
            WorkSheet.Cells["D7"].Value = OrderTypeModel.CreatorContact;

            WorkSheet.Cells["B8"].Value = "Подразделение Заказчика";
            string clientName = OrderTypeModel.ClientName;
            WorkSheet.Cells["D8"].Value = clientName;
            WorkSheet.Cells["D8"].Style.WrapText = true;

            //группировка
            WorkSheet.Rows[8].OutlineLevel = 1;
            WorkSheet.Rows[8].Hidden = true;
            WorkSheet.Rows[9].OutlineLevel = 2;
            WorkSheet.Rows[9].Hidden = true;
            WorkSheet.Rows[10].OutlineLevel = 2;
            WorkSheet.Rows[10].Hidden = true;
            WorkSheet.Rows[11].OutlineLevel = 2;
            WorkSheet.Rows[11].Hidden = true;

            WorkSheet.Rows[12].Collapsed = true;

            WorkSheet.Cells["B9"].Value = "Код затрат (управленческого учета)";

            WorkSheet.Cells["B10"].Value = "Маршрут движения";
            WorkSheet.Cells["D10"].Value = OrderTypeModel.TotalDistanceDescription;

            WorkSheet.Cells["B11"].Value = "Расстояние, км";
            WorkSheet.Cells["D11"].Value = OrderTypeModel.TotalDistanceLenght;

            WorkSheet.Cells["B12"].Value = "Стоимость перевозки, грн.";
            WorkSheet.Cells["D12"].Value = OrderTypeModel.TotalCost;

            WorkSheet.Cells["B13"].Value = "!!! Подписанная Заявка является основанием для поездки . При посадке отдать водителю !!!!";
            WorkSheet.Cells["B13"].Style.Font.Size = 8 * 20;

            WorkSheet.Cells["B14"].Value = "Дата выезда";
            WorkSheet.Cells["C14"].Value = "Время выезда";
            WorkSheet.Cells["D14"].Value = "Адрес подачи автомашины и маршрут движения";
            WorkSheet.Cells["E14"].Value = "Дата прибытия (плановая)";
            WorkSheet.Cells["F14"].Value = "Время прибытия(плановое)";
            WorkSheet.Cells["G14"].Value = "Список пассажиров (Ф.И.О./должность/подразделение/телефон)";
            //WorkSheet.Cells["H11"].Value = "";
            WorkSheet.Cells["H14"].Value = "Цель служебной поездки";

            //шапка -  форматирование           
            for (int i = 2; i <= 8; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + 14].Style.Font.Size = 10 * 20;
                WorkSheet.Cells[GetExcelColumnName(i) + 14].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells[GetExcelColumnName(i) + 14].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.Black, LineStyle.Medium);
                WorkSheet.Cells[GetExcelColumnName(i) + 14].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                WorkSheet.Cells[GetExcelColumnName(i) + 14].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                WorkSheet.Cells[GetExcelColumnName(i) + 14].Style.WrapText = true;
                WorkSheet.Cells[GetExcelColumnName(i) + 14].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                        SpreadsheetColor.FromName(ColorName.Background2Darker10Pct),
                        SpreadsheetColor.FromName(ColorName.Automatic));
            }

            WorkSheet.Cells["B15"].Value = extOrderTypeModel.StartDateTimeOfTrip;
            WorkSheet.Cells["C15"].Value = extOrderTypeModel.StartDateTimeExOfTrip;

            //выделяем жирным организацию
            string[] idListAdressFrom = AdressFrom.Split(new char[] { ',' });
            int OrgFrom = idListAdressFrom[0].Length;

            string[] idListAdressTo = AdressTo.Split(new char[] { ',' });
            int OrgTo = idListAdressTo[0].Length;

            WorkSheet.Cells["D15"].Value = AdressFrom + " - " + AdressTo;
            WorkSheet.Cells["D15"].GetCharacters(0, OrgFrom).Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D15"].GetCharacters(AdressFrom.Length + 3, OrgTo).Font.Weight = ExcelFont.BoldWeight;

            WorkSheet.Cells["E15"].Value = extOrderTypeModel.StartDateTimeOfTrip;
            WorkSheet.Cells["F15"].Value = extOrderTypeModel.FinishDateTimeExOfTrip;
            WorkSheet.Cells["G15"].Value = extOrderTypeModel.PassInfo;
            WorkSheet.Cells["H15"].Value = extOrderTypeModel.TripDescription;

            for (int i = 2; i <= 8; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + 15].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Bottom, Color.Black, LineStyle.Thin);
                WorkSheet.Cells[GetExcelColumnName(i) + 15].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                WorkSheet.Cells[GetExcelColumnName(i) + 15].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                WorkSheet.Cells[GetExcelColumnName(i) + 15].Style.WrapText = true;
                WorkSheet.Cells[GetExcelColumnName(i) + 15].Style.Font.Size = 12 * 20;

                if (i == 2)
                    WorkSheet.Cells[GetExcelColumnName(i) + 15].Style.Borders.SetBorders(
                   MultipleBorders.Left, Color.Black, LineStyle.Medium);

                if (i == 8)
                    WorkSheet.Cells[GetExcelColumnName(i) + 15].Style.Borders.SetBorders(
                        MultipleBorders.Right, Color.Black, LineStyle.Medium);

            }

            WorkSheet.Cells["D15"].Style.Font.Size = 9 * 20;

            if (extOrderTypeModel.NeedReturn)
            {
                WorkSheet.Cells["B16"].Value = extOrderTypeModel.StartDateTimeOfTrip;
                WorkSheet.Cells["C16"].Value = extOrderTypeModel.FinishDateTimeExOfTrip;
                WorkSheet.Cells["D16"].Value = "ожидание";

                WorkSheet.Cells["E16"].Value = extOrderTypeModel.ReturnStartDateTimeExOfTrip;

                WorkSheet.Cells["B17"].Value = extOrderTypeModel.ReturnStartDateTimeOfTrip;
                WorkSheet.Cells["C17"].Value = extOrderTypeModel.ReturnStartDateTimeExOfTrip;


                //выделяем жирным организацию                
                WorkSheet.Cells["D17"].Value = AdressTo + " - " + AdressFrom;
                WorkSheet.Cells["D17"].GetCharacters(0, OrgTo).Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["D17"].GetCharacters(AdressTo.Length + 3, OrgFrom).Font.Weight = ExcelFont.BoldWeight;


                WorkSheet.Cells["E17"].Value = extOrderTypeModel.ReturnFinishDateTimeOfTrip;
                WorkSheet.Cells["F17"].Value = extOrderTypeModel.ReturnFinishDateTimeExOfTrip;
                WorkSheet.Cells["G17"].Value = extOrderTypeModel.PassInfo;
                WorkSheet.Cells["H17"].Value = extOrderTypeModel.TripDescription;

                for (int i = 2; i <= 8; i++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + 16].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                        SpreadsheetColor.FromName(ColorName.Background2Darker10Pct),
                        SpreadsheetColor.FromName(ColorName.Automatic));


                    for (int j = 16; j <= 17; j++)
                    {
                        WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                            MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                            Color.Black, LineStyle.Thin);

                        if (j == 17)
                        {
                            WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                            MultipleBorders.Bottom, Color.Black, LineStyle.Medium);
                        }

                        if (i == 2)
                            WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                           MultipleBorders.Left, Color.Black, LineStyle.Medium);

                        if (i == 8)
                            WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                           MultipleBorders.Right, Color.Black, LineStyle.Medium);

                        WorkSheet.Cells[GetExcelColumnName(i) + j].Style.VerticalAlignment =
                            VerticalAlignmentStyle.Center;
                        WorkSheet.Cells[GetExcelColumnName(i) + j].Style.HorizontalAlignment =
                            HorizontalAlignmentStyle.Center;
                        WorkSheet.Cells[GetExcelColumnName(i) + j].Style.WrapText = true;
                        WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Font.Size = 12 * 20;
                    }
                }

                WorkSheet.Cells["D16"].Style.Font.Size = 9 * 20;
                WorkSheet.Cells["D17"].Style.Font.Size = 9 * 20;
            }

            int RowFooterCount = 17;
            if (extOrderTypeModel.NeedReturn) RowFooterCount = 19;
            WorkSheet.Cells["B" + RowFooterCount].Value = "Тип заявки";
            WorkSheet.Cells["B" + RowFooterCount].Style.Font.Size = 12 * 20;
            WorkSheet.Cells["C" + RowFooterCount].Value = "Плановая/срочная";
            WorkSheet.Cells["C" + RowFooterCount].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D" + RowFooterCount].Value =
                "Плановая - не позднее, чем за один рабочий день до дня, в котором планируется поездка, с указанием даты, времени, адреса подачи и " +
                "возвращения автомобиля и других данных, необходимых для размещения заявки. Заявка должна быть подписана руководителем по должности не ниже чем Руководителя " +
                "департамента, начальника отдела, службы. )" +
                " Срочная - выходные, праздничные дни, нерабочее время, поданная в срок позже плановой";

            WorkSheet.Cells["D" + RowFooterCount].Style.WrapText = true;
            WorkSheet.Cells["D" + RowFooterCount].Style.Font.Size = 9 * 20;
            int RowStart = RowFooterCount - 1;
            int RowEnd = RowFooterCount + 1;
            WorkSheet.Cells.GetSubrangeAbsolute(RowStart, 3, RowEnd, 7).Merged = true;


            RowFooterCount = RowFooterCount + 4;
            WorkSheet.Cells["B" + RowFooterCount].Value = "Согласовал (инициатор)";
            WorkSheet.Cells["B" + RowFooterCount].Style.Font.Size = 12 * 20;
            WorkSheet.Cells["D" + RowFooterCount].Value = "_______________________________";
            WorkSheet.Cells["E" + RowFooterCount].Value = OrderTypeModel.CreatorPosition;

            RowFooterCount++;
            WorkSheet.Cells["D" + RowFooterCount].Value = "подпись";
            WorkSheet.Cells["D" + RowFooterCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells["D" + RowFooterCount].Style.VerticalAlignment = VerticalAlignmentStyle.Bottom;
            WorkSheet.Cells["D" + RowFooterCount].Style.Font.ScriptPosition = ScriptPosition.Superscript;
            WorkSheet.Cells["E" + RowFooterCount].Value = "ФИО";
            WorkSheet.Cells["E" + RowFooterCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells["E" + RowFooterCount].Style.VerticalAlignment = VerticalAlignmentStyle.Bottom;
            WorkSheet.Cells["E" + RowFooterCount].Style.Font.ScriptPosition = ScriptPosition.Superscript;

            WorkSheet.Rows[RowFooterCount].OutlineLevel = 1;
            RowFooterCount++;
            WorkSheet.Rows[RowFooterCount].OutlineLevel = 2;
            WorkSheet.Cells["B" + RowFooterCount].Value = "Руководитель Департамента";

            WorkSheet.Cells["B" + RowFooterCount].Style.Font.Size = 12 * 20;
            WorkSheet.Cells["D" + RowFooterCount].Value = "_______________________________";

            RowFooterCount++;
            WorkSheet.Cells["D" + RowFooterCount].Value = "подпись";
            WorkSheet.Cells["D" + RowFooterCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells["D" + RowFooterCount].Style.VerticalAlignment = VerticalAlignmentStyle.Bottom;
            WorkSheet.Cells["D" + RowFooterCount].Style.Font.ScriptPosition = ScriptPosition.Superscript;
            WorkSheet.Cells["E" + RowFooterCount].Value = "ФИО";
            WorkSheet.Cells["E" + RowFooterCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells["E" + RowFooterCount].Style.VerticalAlignment = VerticalAlignmentStyle.Bottom;
            WorkSheet.Cells["E" + RowFooterCount].Style.Font.ScriptPosition = ScriptPosition.Superscript;

            RowFooterCount = RowFooterCount + 2;
            WorkSheet.Rows[RowFooterCount].OutlineLevel = 1;
            RowFooterCount++;
            WorkSheet.Rows[RowFooterCount].OutlineLevel = 2;
            WorkSheet.Cells["B" + RowFooterCount].Value = "Директор";
            WorkSheet.Cells["D" + RowFooterCount].Value = "_______________________________";

            RowFooterCount++;
            WorkSheet.Cells["D" + RowFooterCount].Value = "подпись";
            WorkSheet.Cells["D" + RowFooterCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells["D" + RowFooterCount].Style.VerticalAlignment = VerticalAlignmentStyle.Bottom;
            WorkSheet.Cells["D" + RowFooterCount].Style.Font.ScriptPosition = ScriptPosition.Superscript;


            WorkSheet.Cells["E" + RowFooterCount].Value = "ФИО";
            WorkSheet.Cells["E" + RowFooterCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells["E" + RowFooterCount].Style.VerticalAlignment = VerticalAlignmentStyle.Bottom;
            WorkSheet.Cells["E" + RowFooterCount].Style.Font.ScriptPosition = ScriptPosition.Superscript;

            RowFooterCount++;
            WorkSheet.Cells["B" + RowFooterCount].Value = "Дата утверждения";
            WorkSheet.Cells["D" + RowFooterCount].Value = AcceptDate;
            WorkSheet.Cells["B" + RowFooterCount].Style.Font.Size = 8 * 20;
            WorkSheet.Cells["D" + RowFooterCount].Style.Font.Size = 8 * 20;

            RowFooterCount = RowFooterCount + 2;
            WorkSheet.Cells["B" + RowFooterCount].Value = "Исп. ";
            WorkSheet.Cells["B" + RowFooterCount].Style.Font.Size = 8 * 20;

            RowFooterCount++;
            WorkSheet.Cells["B" + RowFooterCount].Value = OrderTypeModel.CreatorPosition;
            WorkSheet.Cells["B" + RowFooterCount].Style.Font.Size = 8 * 20;
            //WorkSheet.Cells["B24"].Value = "Исп. ";

            byte[] fileContents;
            var options = SaveOptions.XlsxDefault;

            using (var stream = new MemoryStream())
            {
                ef.Save(stream, options);
                fileContents = stream.ToArray();
            }
            return fileContents;
        }
        public byte[] ConcursRenderReport<T>(RestDataInfo<T> Data, CompetitiveListViewModel concursHeader, RestParamsInfo Params, OrderBaseViewModel OrderTypeModel, OrdersPassTransportViewModel extOrderTypeModel1, OrdersTruckTransportViewModel extOrderTypeModel2)
        {
            ExcelFile ef = new ExcelFile();
            CultureInfo ci = new CultureInfo(Params.Language);

            ExcelWorksheet WorkSheet = ef.Worksheets.Add("КЛ");
            ExcelWorksheet WorkSheet2 = ef.Worksheets.Add("Данные для учета");
            WorkSheet.PrintOptions.PaperType = PaperType.A4;
            WorkSheet.PrintOptions.FitWorksheetWidthToPages = 1;

            //шрифт заголовка + сделать жирным
            WorkSheet.Cells["C1"].Style.Font.Size = 12 * 20;
            WorkSheet.Cells["C1"].Style.Font.Weight = ExcelFont.BoldWeight;

            //заголовок отчета
            WorkSheet.Cells["C1"].Value = Params.MainHeader + " № ";

            WorkSheet.Cells["I1"].Value = concursHeader.Id.ToString();
            WorkSheet.Cells["I1"].Style.Font.Size = 12 * 20;
            WorkSheet.Cells["I1"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(0, 8, 0, 9).Merged = true;
            WorkSheet.Cells["I1"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 9; i <= 10; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "1"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            for (int i = 9; i <= 15; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + 1].Style.Borders.SetBorders(MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);
            }

            //Дата и время создания файла            
            WorkSheet.Cells["E2"].Value = "Дата и время создания файла:" + DateTime.Now;
            WorkSheet.Cells["E2"].Style.Font.Size = 8 * 20;

            //ширина колонок            
            WorkSheet.Columns[0].Width = 33 * 256;
            WorkSheet.Columns[1].Width = 33 * 256; //A
            WorkSheet.Columns[2].Width = 7 * 256; //B
            WorkSheet.Columns[3].Width = 13 * 256; //C
            WorkSheet.Columns[4].Width = 12 * 256; //D
            WorkSheet.Columns[5].Width = 12 * 256; //E
            WorkSheet.Columns[6].Width = 15 * 256; //F
            WorkSheet.Columns[7].Width = 18 * 256; //G
            WorkSheet.Columns[8].Width = 18 * 256; //H
            WorkSheet.Columns[9].Width = 15 * 256; //I
            WorkSheet.Columns[10].Width = 12 * 256; //J
            WorkSheet.Columns[11].Width = 9 * 256; //K
            WorkSheet.Columns[12].Width = 13 * 256; //L
            WorkSheet.Columns[13].Width = 12 * 256; //M
            WorkSheet.Columns[14].Width = 12 * 256; //N
            WorkSheet.Columns[15].Width = 22 * 256; //O
            WorkSheet.Columns[16].Width = 28 * 256; //P
            WorkSheet.Columns[17].Width = 11 * 256; //Q

            WorkSheet.Cells["A3"].Style.Font.Size = 11 * 20;
            WorkSheet.Cells["A3"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            WorkSheet.Cells["A3"].Value = "Дата ";

            /*for (int i = 1; i <= 3; i++)
            {*/
            WorkSheet.Cells["B3"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Thin);
            //}

            WorkSheet.Cells["B3"].Value = concursHeader.OrderDate;
            WorkSheet.Cells["B3"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(2, 1, 2, 3).Merged = true;
            WorkSheet.Cells["B3"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

            WorkSheet.Cells["A5"].Value = "Плательщик/Заказчик";
            WorkSheet.Cells["D5"].Value = concursHeader.PayerName;
            WorkSheet.Cells.GetSubrangeAbsolute(4, 3, 4, 16).Merged = true;
            for (int i = 4; i <= 17; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "5"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["A6"].Value = "Наименование груза, вес, габариты, упаковка";
            WorkSheet.Cells["D6"].Value = concursHeader.TruckDescription;
            WorkSheet.Cells.GetSubrangeAbsolute(5, 3, 5, 7).Merged = true;

            for (int i = 4; i <= 8; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "6"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["I6"].Value = "вес";
            WorkSheet.Cells["J6"].Value = concursHeader.Weight;

            WorkSheet.Cells["J6"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["K6"].Value = "тн";
            WorkSheet.Cells["O6"].Value = "габариты, упаковка";
            WorkSheet.Cells.GetSubrangeAbsolute(5, 16, 5, 16).Merged = true;

            for (int i = 16; i <= 17; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "6"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["I6"].Style.Borders.SetBorders(MultipleBorders.Bottom | MultipleBorders.Top | MultipleBorders.Right | MultipleBorders.Left,
                   Color.Black, LineStyle.Thin);

            WorkSheet.Cells["K6"].Style.Borders.SetBorders(MultipleBorders.Left,
                   Color.Black, LineStyle.Thin);

            WorkSheet.Cells["O6"].Style.Borders.SetBorders(MultipleBorders.Right,
                Color.Black, LineStyle.Thin);

            WorkSheet.Cells["P6"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells["P6"].Value = concursHeader.Dimenssion + ", " + concursHeader.BoxingDescription;


            WorkSheet.Cells["A7"].Value = "Дата подачи / выгрузки";
            WorkSheet.Cells["D7"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            WorkSheet.Cells["D7"].Value = "подачи";
            WorkSheet.Cells["D7"].Style.Borders.SetBorders(MultipleBorders.Bottom | MultipleBorders.Top | MultipleBorders.Right | MultipleBorders.Left,
                 Color.Black, LineStyle.Thin);
            WorkSheet.Cells["H7"].Style.Borders.SetBorders(MultipleBorders.Bottom | MultipleBorders.Top | MultipleBorders.Right | MultipleBorders.Left,
                 Color.Black, LineStyle.Thin);
            WorkSheet.Cells.GetSubrangeAbsolute(6, 4, 6, 6).Merged = true;
            WorkSheet.Cells["E7"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells["E7"].Value = concursHeader.FromDate;
            for (int i = 5; i <= 7; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "7"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["O7"].Style.Borders.SetBorders(MultipleBorders.Right,
                Color.Black, LineStyle.Thin);

            WorkSheet.Cells["H7"].Value = "выгрузки";
            WorkSheet.Cells["H7"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            WorkSheet.Cells.GetSubrangeAbsolute(6, 8, 6, 14).Merged = true;
            WorkSheet.Cells["I7"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells["I7"].Value = concursHeader.ToDate;
            for (int i = 9; i <= 15; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "7"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["A8"].Value = "Маршрут";
            WorkSheet.Cells["D8"].Value = concursHeader.Route;
            WorkSheet.Cells.GetSubrangeAbsolute(7, 3, 7, 14).Merged = true;
            WorkSheet.Cells["D8"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            for (int i = 4; i <= 15; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "8"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["A9"].Value = "Расстояние, км.";
            WorkSheet.Cells["D9"].Value = concursHeader.TotalDistanceLenght;
            WorkSheet.Cells.GetSubrangeAbsolute(8, 3, 8, 8).Merged = true;
            WorkSheet.Cells["D9"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            for (int i = 4; i <= 15; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "9"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["A10"].Value = "Требуемое кол-во автомобилей";
            WorkSheet.Cells["D10"].Value = concursHeader.CarNumber;
            for (int i = 4; i <= 15; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "10"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["A11"].Value = "Тип заявки (плановая/срочная)";
            if (concursHeader.PriorityType == 0)
            {
                WorkSheet.Cells["D11"].Value = "Плановая";
            }
            else
            {
                WorkSheet.Cells["D11"].Value = "Срочная";
            }

            WorkSheet.Cells["D11"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["E11"].Value = "обоснование";
            for (int i = 6; i <= 15; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "11"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["A12"].Value = "Особые условия (негабарит, трал,без тента, прочее)";
            WorkSheet.Cells.GetSubrangeAbsolute(11, 3, 11, 14).Merged = true;
            for (int i = 4; i <= 15; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "12"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            for (int j = 3; j <= 12; j++)
            {
                WorkSheet.Cells["A" + j].Style.Font.Weight = ExcelFont.BoldWeight;
                if (j > 3)
                    WorkSheet.Cells["A" + j].Style.Font.Size = 12 * 20;
            }

            for (int i = 4; i <= 16; i++)
            {
                for (int j = 5; j <= 12; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Font.Size = 12 * 20;
                }
            }

            for (int i = 1; i <= 15; i++)
            {
                for (int j = 4; j <= 11; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);

                    if ((i == 3) && (j > 4))
                        WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(MultipleBorders.Right,
                       Color.Black, LineStyle.Thin);
                }
            }

            for (int i = 4; i <= 17; i++)
            {

                WorkSheet.Cells[GetExcelColumnName(i) + 5].Style.Borders.SetBorders(MultipleBorders.Bottom | MultipleBorders.Top | MultipleBorders.Right,
                    Color.Black, LineStyle.Thin);
                WorkSheet.Cells[GetExcelColumnName(i) + 6].Style.Borders.SetBorders(MultipleBorders.Bottom | MultipleBorders.Top,
                Color.Black, LineStyle.Thin);
            }

            WorkSheet.Cells["Q6"].Style.Borders.SetBorders(MultipleBorders.Right,
                Color.Black, LineStyle.Thin);

            WorkSheet.Cells["A13"].Value = "Наименование услуги";
            WorkSheet.Cells["A13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["A13"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells.GetSubrangeAbsolute(12, 0, 13, 0).Merged = true;


            WorkSheet.Cells["B13"].Value = "Перевозчик/ Экспедитор";
            WorkSheet.Cells["B13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B13"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells.GetSubrangeAbsolute(12, 1, 13, 1).Merged = true;

            WorkSheet.Cells["C13"].Value = "Грузоподъемность автомобиля, тонн";
            WorkSheet.Cells["C13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["C13"].Style.Rotation = 90;
            WorkSheet.Cells.GetSubrangeAbsolute(12, 2, 13, 2).Merged = true;

            WorkSheet.Cells["D13"].Value = "Предложено транс-ных единиц, шт.";
            WorkSheet.Cells["D13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(12, 3, 13, 3).Merged = true;

            WorkSheet.Cells["E13"].Value = "Акцептовано транс-ных единиц, шт.";
            WorkSheet.Cells["E13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(12, 4, 13, 4).Merged = true;

            WorkSheet.Cells["F13"].Value = "НДС";
            WorkSheet.Cells["F13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(12, 5, 13, 5).Merged = true;

            WorkSheet.Cells["G13"].Value = "Стоимость одного автомобиля, грн. без НДС (согласно договору)";
            WorkSheet.Cells.GetSubrangeAbsolute(12, 6, 13, 6).Merged = true;

            WorkSheet.Cells["H13"].Value = "Стоимость одного автомобиля, грн. без  НДС (согласно КП) региональные: 1 тур рассылка первому ранжированному перевозчику или экспорт, спецтр - т – 1 этап редукцион";
            WorkSheet.Cells.GetSubrangeAbsolute(12, 7, 13, 7).Merged = true;

            WorkSheet.Cells["I13"].Value = "Стоимость одного автомобиля, грн. без  НДС (согласно КП) региональные: 2 тур рассылка на всех перевозчиков) или экспорт, спецтр - т – 2 этап редукцион";
            WorkSheet.Cells.GetSubrangeAbsolute(12, 8, 13, 8).Merged = true;

            WorkSheet.Cells["J13"].Value = "Отсрочка платежей (договорная/КП1/КП2)";
            WorkSheet.Cells["J13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["J13"].Style.Rotation = 90;
            WorkSheet.Cells.GetSubrangeAbsolute(12, 9, 13, 9).Merged = true;

            WorkSheet.Cells["K13"].Value = "Приведение стоимости";
            WorkSheet.Cells.GetSubrangeAbsolute(12, 10, 12, 14).Merged = true;
            WorkSheet.Cells["K13"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["P13"].Value = "Примечание";
            WorkSheet.Cells["P13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["P13"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells.GetSubrangeAbsolute(12, 15, 13, 15).Merged = true;

            WorkSheet.Cells["Q13"].Value = "Средняя цена за грн./ км.";
            WorkSheet.Cells["Q13"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells["Q13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["Q13"].Style.Rotation = 90;
            WorkSheet.Cells.GetSubrangeAbsolute(12, 16, 13, 16).Merged = true;
            for (int i = 11; i <= 16; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + 13].Style.Borders.SetBorders(
                    MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);
            }

            WorkSheet.Cells["K14"].Value = "эффект от отсрочки";
            WorkSheet.Cells["L14"].Value = "предоплата дней";
            WorkSheet.Cells["M14"].Value = "cумма предоплаты";
            //   WorkSheet.Cells["N14"].Value = "предоплата дней";
            WorkSheet.Cells["N14"].Value = "эффект от предоплаты";
            WorkSheet.Cells["O14"].Value = "Стоимость одного автомобиля грн. с учетом стоимости денег (Базис приведения-оплата по факту перевозки без отсрочкой платежа)";
            WorkSheet.Cells["O14"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["O14"].Style.WrapText = true;
            for (int i = 11; i <= 14; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + 14].Style.Rotation = 90;
                WorkSheet.Cells[GetExcelColumnName(i) + 14].Style.WrapText = true;
                WorkSheet.Cells[GetExcelColumnName(i) + 14].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells[GetExcelColumnName(i) + 14].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            }

            for (int i = 1; i <= 17; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + 13].Style.WrapText = true;
                WorkSheet.Cells[GetExcelColumnName(i) + 13].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            }

            for (int i = 1; i <= 17; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + 15].Value = i;
            }

            for (int i = 1; i <= 17; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + 13].Style.Borders.SetBorders(MultipleBorders.Top,
                Color.Black, LineStyle.Medium);

                WorkSheet.Cells[GetExcelColumnName(i) + 15].Style.Borders.SetBorders(MultipleBorders.Bottom | MultipleBorders.Top,
                Color.Black, LineStyle.Medium);

                WorkSheet.Cells[GetExcelColumnName(i) + 15].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells[GetExcelColumnName(i) + 15].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            }

            for (int i = 1; i <= 17; i++)
            {
                for (int j = 14; j <= 15; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right,
                Color.Black, LineStyle.Thin);
                }
            }

            //вывод данных
            int RowCount = 16;
            foreach (var data in Data.Rows)
            {
                int i = 1;
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Value = GetProperty1(data, "NameSpecification");
                i++;
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Value = GetProperty1(data, "ExpeditorName");
                i++;
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Value = GetProperty1(data, "CarryCapacity");
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                i++;
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Value = GetProperty1(data, "CarsOffered");
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                i++;
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Value = GetProperty1(data, "CarsAccepted");
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                i++;

                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Value = GetProperty1(data, "NDS") + "%";
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                i++;

                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Value = GetProperty1(data, "CarCostDog");
                i++;

                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Value = GetProperty1(data, "CarCost7");
                i++;
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Value = GetProperty1(data, "CarCost");
                i++;
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Value = GetProperty1(data, "DaysDelaySteps");
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                i++;
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Value = GetProperty1(data, "DelayEffect");
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                i++;
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Value = GetProperty1(data, "Prepayment");
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                i++;
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Value = GetProperty1(data, "Prepayment2");
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                i++;
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Value = GetProperty1(data, "PrepaymentEffect");
                i++;
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Value = GetProperty1(data, "CarCostWithMoneyCost");
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                i++;
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Value = GetProperty1(data, "Comments");
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                i++;
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Value = GetProperty1(data, "AverageCost");
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                i++;
                WorkSheet.Cells["E" + RowCount].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                        SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                        SpreadsheetColor.FromName(ColorName.Automatic));

                WorkSheet.Cells["I" + RowCount].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                        SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                        SpreadsheetColor.FromName(ColorName.Automatic));

                RowCount++;
            }

            //группировка
            WorkSheet.Columns[11].OutlineLevel = 1;
            WorkSheet.Columns[11].Hidden = true;
            WorkSheet.Columns[12].OutlineLevel = 1;
            WorkSheet.Columns[12].Hidden = true;
            WorkSheet.Columns[13].OutlineLevel = 1;
            WorkSheet.Columns[13].Hidden = true;
            //WorkSheet.Columns[14].OutlineLevel = 1;
            //WorkSheet.Columns[14].Hidden = true;

            WorkSheet.Columns[14].Collapsed = false;

            for (int i = 1; i <= 17; i++)
            {
                //одну строку добавляем для итогов
                for (int j = 16; j <= RowCount; j++)
                {
                    if (j == 16)
                        WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(MultipleBorders.Bottom | MultipleBorders.Left | MultipleBorders.Right,
                            Color.Black, LineStyle.Thin);
                    else
                        WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(MultipleBorders.Bottom | MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right,
                        Color.Black, LineStyle.Thin);
                }
            }

            WorkSheet.Cells["A" + RowCount].Value = "Итого по заявке";
            WorkSheet.Cells["A" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;

            RowCount++;
            WorkSheet.Cells["A" + RowCount].Value = "Выбран перевозчик:";

            RowCount = RowCount + 2;
            WorkSheet.Cells["A" + RowCount].Value = "Составил ______________________";
            WorkSheet.Cells["B" + RowCount].Value = concursHeader.OrderExecuterName;
            RowCount = RowCount + 2;
            WorkSheet.Cells["A" + RowCount].Value = "Согласовал ____________________";
            RowCount = RowCount + 2;
            WorkSheet.Cells["A" + RowCount].Value = "Согласовано ____________________";
            RowCount++;
            WorkSheet.Cells["A" + RowCount].Value = "Согласовано ___________________";
            for (int i = 3; i <= 5; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.Borders.SetBorders(
                        MultipleBorders.Bottom, Color.Black, LineStyle.Thin);
            }

            RowCount = RowCount + 2;
            WorkSheet.Cells["A" + RowCount].Value = "Согласовано ___________________";
            for (int i = 3; i <= 5; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.Borders.SetBorders(
                        MultipleBorders.Bottom, Color.Black, LineStyle.Thin);
            }

            //группировка
            WorkSheet.Rows[RowCount - 3].OutlineLevel = 1;
            WorkSheet.Rows[RowCount - 3].Hidden = true;
            WorkSheet.Rows[RowCount - 2].OutlineLevel = 1;
            WorkSheet.Rows[RowCount - 2].Hidden = true;
            WorkSheet.Rows[RowCount - 1].OutlineLevel = 1;
            WorkSheet.Rows[RowCount - 1].Hidden = true;
            WorkSheet.Rows[RowCount].OutlineLevel = 1;
            WorkSheet.Rows[RowCount].Hidden = true;

            WorkSheet.Rows[RowCount + 1].Collapsed = false;

            /*********** 2 лист **********/
            //легковой транспорт
            if ((OrderTypeModel.OrderType == 1) || (OrderTypeModel.OrderType == 3) || (OrderTypeModel.OrderType == 6))
            //return PassOrderRenderReport(OrderTypeModel, extOrderTypeModel, AcceptDate, orderClientInfo, Params,
            //  AdressFrom, AdressTo, ContractName, carList);
            {
                //ширина колонок            
                WorkSheet2.Columns[0].Width = 14 * 256;
                WorkSheet2.Columns[1].Width = 10 * 256;
                WorkSheet2.Columns[2].Width = 20 * 256;
                WorkSheet2.Columns[3].Width = 20 * 256;
                WorkSheet2.Columns[4].Width = 18 * 256;
                WorkSheet2.Columns[5].Width = 18 * 256;
                WorkSheet2.Columns[6].Width = 15 * 256;
                WorkSheet2.Columns[7].Width = 22 * 256;
                WorkSheet2.Columns[8].Width = 16 * 256; //I
                WorkSheet2.Columns[9].Width = 8 * 256;
                WorkSheet2.Columns[10].Width = 8 * 256;
                WorkSheet2.Columns[11].Width = 8 * 256;
                WorkSheet2.Columns[12].Width = 8 * 256;
                WorkSheet2.Columns[13].Width = 8 * 256;
                WorkSheet2.Columns[14].Width = 10 * 256; //O
                WorkSheet2.Columns[15].Width = 10 * 256;
                WorkSheet2.Columns[16].Width = 12 * 256;
                WorkSheet2.Columns[17].Width = 12 * 256; //I
                WorkSheet2.Columns[18].Width = 8 * 256;
                WorkSheet2.Columns[19].Width = 8 * 256; //T
                WorkSheet2.Columns[20].Width = 13 * 256;
                WorkSheet2.Columns[21].Width = 9 * 256;
                WorkSheet2.Columns[22].Width = 11 * 256;
                //WorkSheet2.Columns[23].Width = 13 * 256;

                //форматирование
                for (int i = 1; i <= 23; i++)
                {
                    WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Font.Weight = ExcelFont.BoldWeight;
                    WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.HorizontalAlignment =
                        HorizontalAlignmentStyle.Center;
                    WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.WrapText = true;
                    WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Borders.SetBorders(MultipleBorders.Bottom,
                        Color.Black, LineStyle.Medium);
                    WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right, Color.Black, LineStyle.Thin);
                    WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                        SpreadsheetColor.FromName(ColorName.Accent1Lighter80Pct),
                        SpreadsheetColor.FromName(ColorName.Automatic));

                    WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Font.Size = 10 * 20;

                    WorkSheet2.Cells[GetExcelColumnName(i) + 2].Style.HorizontalAlignment =
                        HorizontalAlignmentStyle.Center;
                    WorkSheet2.Cells[GetExcelColumnName(i) + 2].Style.WrapText = true;
                    WorkSheet2.Cells[GetExcelColumnName(i) + 2].Style.Font.Size = 11 * 20;

                }

                WorkSheet2.Cells["A1"].Value =
                    "Номер заявки (при разбитие заявки на строки использовать нумерацию с \" / \")";
                WorkSheet2.Cells["B1"].Value = "Дата указанная в заявке";
                WorkSheet2.Cells["C1"].Value = "Заказчик/Плательщик (по справочнику) ";
                WorkSheet2.Cells["D1"].Value = "Автор заявки";
                WorkSheet2.Cells["E1"].Value = "Организация отправитель";
                WorkSheet2.Cells["F1"].Value = "Организация прибытия";
                WorkSheet2.Cells["G1"].Value = "Пункт отправления (по справочника)";
                WorkSheet2.Cells["H1"].Value = "Пункт прибытия (по справочнику)";
                WorkSheet2.Cells["I1"].Value = "Список пассажиров";
                WorkSheet2.Cells["J1"].Value = "Цель поездки";

                WorkSheet2.Cells["K1"].Value = "Дата отправления";
                WorkSheet2.Cells["L1"].Value = "Время отправления";
                WorkSheet2.Cells["M1"].Value = "Дата прибытия";
                WorkSheet2.Cells["N1"].Value = "Время прибытия";

                WorkSheet2.Cells["O1"].Value = "Дата обратного отправления";
                WorkSheet2.Cells["P1"].Value = "Время обратного отправления";
                WorkSheet2.Cells["Q1"].Value = "Дата окончания поездки";
                WorkSheet2.Cells["R1"].Value = "Время окончания поездки";
                WorkSheet2.Cells["S1"].Value = "Дата подачи заявки";
                WorkSheet2.Cells["T1"].Value = "Время подачи заявки";

                WorkSheet2.Cells["U1"].Value = "Сумма точек загрузки и выгрузки";
                WorkSheet2.Cells["V1"].Value = "Длина марш., км";
                WorkSheet2.Cells["W1"].Value = "Тип маршрута (по справочнику)";

                int PassRowCount = 2;
                // foreach (var OrderTypeModel in ordersPassList)
                // {

                WorkSheet2.Cells["A" + PassRowCount].Value = OrderTypeModel.Id.ToString();
                WorkSheet2.Cells["B" + PassRowCount].Value = OrderTypeModel.OrderDate;
                WorkSheet2.Cells["C" + PassRowCount].Value = OrderTypeModel.PayerName;
                WorkSheet2.Cells["D" + PassRowCount].Value = OrderTypeModel.CreatorPosition;
                WorkSheet2.Cells["E" + PassRowCount].Value = extOrderTypeModel1.OrgFrom;
                WorkSheet2.Cells["F" + PassRowCount].Value = extOrderTypeModel1.OrgTo;
                if (extOrderTypeModel1.TripType == 2)
                    WorkSheet2.Cells["G" + PassRowCount].Value = extOrderTypeModel1.CountryFromName + " " +
                                                                 extOrderTypeModel1.CityFrom + " " +
                                                                 extOrderTypeModel1.AdressFrom;
                else
                    WorkSheet2.Cells["G" + PassRowCount].Value = extOrderTypeModel1.CityFrom + " " +
                                                                 extOrderTypeModel1.AdressFrom;

                if (extOrderTypeModel1.TripType == 2)
                    WorkSheet2.Cells["H" + PassRowCount].Value = extOrderTypeModel1.CountryToName + " " +
                                                                 extOrderTypeModel1.CityTo + " " +
                                                                 extOrderTypeModel1.AdressTo;
                else
                    WorkSheet2.Cells["H" + PassRowCount].Value = extOrderTypeModel1.CityTo + " " +
                                                                 extOrderTypeModel1.AdressTo;
                WorkSheet2.Cells["I" + PassRowCount].Value = extOrderTypeModel1.PassInfo;

                WorkSheet2.Cells["J" + PassRowCount].Value = extOrderTypeModel1.TripDescription;

                WorkSheet2.Cells["K" + PassRowCount].Value = extOrderTypeModel1.StartDateTimeOfTrip;
                WorkSheet2.Cells["L" + PassRowCount].Value = extOrderTypeModel1.StartDateTimeExOfTrip;
                WorkSheet2.Cells["M" + PassRowCount].Value = extOrderTypeModel1.FinishDateTimeOfTrip;
                WorkSheet2.Cells["N" + PassRowCount].Value = extOrderTypeModel1.FinishDateTimeExOfTrip;

                WorkSheet2.Cells["O" + PassRowCount].Value = extOrderTypeModel1.ReturnStartDateTimeOfTrip;
                WorkSheet2.Cells["P" + PassRowCount].Value = extOrderTypeModel1.ReturnStartDateTimeExOfTrip;
                WorkSheet2.Cells["Q" + PassRowCount].Value = extOrderTypeModel1.ReturnFinishDateTimeOfTrip;
                WorkSheet2.Cells["R" + PassRowCount].Value = extOrderTypeModel1.ReturnFinishDateTimeExOfTrip;

                WorkSheet2.Cells["S" + PassRowCount].Value = extOrderTypeModel1.CreateDatetime.ToShortDateString();
                WorkSheet2.Cells["T" + PassRowCount].Value = extOrderTypeModel1.CreateDatetime.ToShortTimeString();
                //   PassRowCount++;
                //}

            }
            else
            //грузовой транспорт
            //  return TruckOrderRenderReport(OrderTypeModel, AcceptDate, orderClientInfo, Params, AdressFrom, AdressTo,
            //   ContractName, extOrderTypeModel2, carList);

            {
                //ширина колонок            
                WorkSheet2.Columns[0].Width = 14 * 256;
                WorkSheet2.Columns[1].Width = 10 * 256;
                WorkSheet2.Columns[2].Width = 20 * 256;
                WorkSheet2.Columns[3].Width = 20 * 256;
                WorkSheet2.Columns[4].Width = 18 * 256;
                WorkSheet2.Columns[5].Width = 18 * 256;
                WorkSheet2.Columns[6].Width = 15 * 256;
                WorkSheet2.Columns[7].Width = 22 * 256;
                WorkSheet2.Columns[8].Width = 16 * 256; //I

                WorkSheet2.Columns[9].Width = 8 * 256;
                WorkSheet2.Columns[10].Width = 8 * 256;
                WorkSheet2.Columns[11].Width = 8 * 256;
                WorkSheet2.Columns[12].Width = 8 * 256;
                WorkSheet2.Columns[13].Width = 8 * 256;
                WorkSheet2.Columns[14].Width = 10 * 256; //O

                WorkSheet2.Columns[15].Width = 10 * 256;
                WorkSheet2.Columns[16].Width = 12 * 256;
                WorkSheet2.Columns[17].Width = 12 * 256; //I
                WorkSheet2.Columns[18].Width = 8 * 256;
                WorkSheet2.Columns[19].Width = 8 * 256; //T
                WorkSheet2.Columns[20].Width = 13 * 256;

                WorkSheet2.Columns[21].Width = 9 * 256;
                WorkSheet2.Columns[22].Width = 11 * 256;
                WorkSheet2.Columns[23].Width = 13 * 256;

                //форматирование
                for (int i = 1; i <= 24; i++)
                {
                    WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Font.Weight = ExcelFont.BoldWeight;
                    WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.HorizontalAlignment =
                        HorizontalAlignmentStyle.Center;
                    WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.WrapText = true;
                    WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Borders.SetBorders(MultipleBorders.Bottom,
                        Color.Black, LineStyle.Medium);
                    WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right, Color.Black, LineStyle.Thin);
                    WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                        SpreadsheetColor.FromName(ColorName.Accent1Lighter80Pct),
                        SpreadsheetColor.FromName(ColorName.Automatic));

                    WorkSheet2.Cells[GetExcelColumnName(i) + 2].Style.HorizontalAlignment =
                        HorizontalAlignmentStyle.Center;
                    WorkSheet2.Cells[GetExcelColumnName(i) + 2].Style.WrapText = true;
                    WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Font.Size = 10 * 20;
                    WorkSheet2.Cells[GetExcelColumnName(i) + 2].Style.Font.Size = 11 * 20;
                }

                WorkSheet2.Cells["A1"].Value =
                    "Номер заявки (при разбитие заявки на строки использовать нумерацию с \" / \")";
                WorkSheet2.Cells["A2"].Value = OrderTypeModel.Id.ToString();
                WorkSheet2.Cells["B1"].Value = "Дата указанная в заявке";
                WorkSheet2.Cells["B2"].Value = OrderTypeModel.OrderDate;
                WorkSheet2.Cells["C1"].Value = "Заказчик/Плательщик (по справочнику) ";
                WorkSheet2.Cells["C2"].Value = OrderTypeModel.PayerName;
                WorkSheet2.Cells["D1"].Value = "Автор заявки";
                WorkSheet2.Cells["D2"].Value = OrderTypeModel.CreatorPosition;
                WorkSheet2.Cells["E1"].Value = "Грузоотправитель (по справочнику)";
                WorkSheet2.Cells["E2"].Value = extOrderTypeModel2.Shipper;

                WorkSheet2.Cells["F1"].Value = "Грузополучатель (по справочнику)";
                WorkSheet2.Cells["F2"].Value = extOrderTypeModel2.Consignee;
                WorkSheet2.Cells["G1"].Value = "Пункт отправления (по справочника)";

                if (extOrderTypeModel2.TripType == 2)
                    WorkSheet2.Cells["G2"].Value = extOrderTypeModel2.ShipperCountryName + " " +
                                                   extOrderTypeModel2.ShipperCity + " " +
                                                   extOrderTypeModel2.ShipperAdress;
                else
                    WorkSheet2.Cells["G2"].Value = extOrderTypeModel2.ShipperCity + " " +
                                                   extOrderTypeModel2.ShipperAdress;

                WorkSheet2.Cells["H1"].Value = "Пункт прибытия (по справочнику)";
                if (extOrderTypeModel2.TripType == 2)
                    WorkSheet2.Cells["H2"].Value = extOrderTypeModel2.ConsigneeCountryName + " " +
                                                   extOrderTypeModel2.ConsigneeCity + " " +
                                                   extOrderTypeModel2.ConsigneeAdress;
                else
                    WorkSheet2.Cells["H2"].Value = extOrderTypeModel2.ConsigneeCity + " " +
                                                   extOrderTypeModel2.ConsigneeAdress;

                WorkSheet2.Cells["I1"].Value = "Наименование груза";
                WorkSheet2.Cells["I2"].Value = extOrderTypeModel2.TruckDescription;

                WorkSheet2.Cells["J1"].Value = "Вес груза";
                WorkSheet2.Cells["J2"].Value = extOrderTypeModel2.Weight;
                WorkSheet2.Cells["K1"].Value = "Тип авто/кузова";
                WorkSheet2.Cells["K2"].Value = extOrderTypeModel2.VehicleTypeName;
                WorkSheet2.Cells["L1"].Value = "Вид загрузки";
                WorkSheet2.Cells["L2"].Value = extOrderTypeModel2.LoadingTypeName;

                WorkSheet2.Cells["M1"].Value = "Ограничения по выгрузке";
                WorkSheet2.Cells["M2"].Value = extOrderTypeModel2.UnloadingTypeName;
                WorkSheet2.Cells["N1"].Value = "К-во авто к подаче";
                WorkSheet2.Cells["N2"].Value = extOrderTypeModel2.CarNumber;
                WorkSheet2.Cells["O1"].Value = "Дата подачи авто по заявке";
                WorkSheet2.Cells["O2"].Value = extOrderTypeModel2.FromShipperDate;

                WorkSheet2.Cells["P1"].Value = "Время подачи авто по заявке";
                WorkSheet2.Cells["P2"].Value = extOrderTypeModel2.FromShipperTime;

                WorkSheet2.Cells["Q1"].Value = "Дата доставки груза по заявке";
                WorkSheet2.Cells["Q2"].Value = extOrderTypeModel2.ToConsigneeDate;

                WorkSheet2.Cells["R1"].Value = "Время доставки груза по заявке";
                WorkSheet2.Cells["R2"].Value = extOrderTypeModel2.ToConsigneeTime;

                WorkSheet2.Cells["S1"].Value = "Дата подачи заявки";
                WorkSheet2.Cells["S2"].Value = OrderTypeModel.CreateDatetime.ToShortDateString();

                WorkSheet2.Cells["T1"].Value = "Время подачи заявки";
                WorkSheet2.Cells["T2"].Value = OrderTypeModel.CreateDatetime.ToShortTimeString();

                WorkSheet2.Cells["U1"].Value = "Тип груза";
                WorkSheet2.Cells["U2"].Value = extOrderTypeModel2.TruckTypeName;

                WorkSheet2.Cells["V1"].Value = "Сумма точек загрузки и выгрузки";

                WorkSheet2.Cells["W1"].Value = "Длина марш., км";

                WorkSheet2.Cells["X1"].Value = "Тип маршрута (по справочнику)";
            }

            byte[] fileContents;
            var options = SaveOptions.XlsxDefault;

            using (var stream = new MemoryStream())
            {
                ef.Save(stream, options);
                fileContents = stream.ToArray();
            }
            return fileContents;


        }

        private byte[] TruckOrderRenderReport(OrderBaseViewModel OrderTypeModel, string AcceptDate,
            OrderClientsViewModel orderClientInfo, RestParamsInfo Params, string AdressFrom, string AdressTo,
            string ContractName, OrdersTruckTransportViewModel extOrderTypeModel, List<OrderUsedCarViewModel> carList)
        {
            //Пример генерации QR кода
            string UrlForEncoding =
#if DEBUG

                $"http://uh218479-1.ukrdomen.com/Orders/UpdateOrder/{OrderTypeModel.Id}";
#else
                              
                                $"https://corumsource.com/Orders/UpdateOrder/{OrderTypeModel.Id}";
#endif


            ExcelFile ef = new ExcelFile();
            CultureInfo ci = new CultureInfo(Params.Language);

            ExcelWorksheet WorkSheet = ef.Worksheets.Add("Заявка грузовой");
            ExcelWorksheet WorkSheet2 = ef.Worksheets.Add("Данные для учета");

            WorkSheet.PrintOptions.PaperType = PaperType.A4;
            WorkSheet.PrintOptions.FitWorksheetWidthToPages = 1;

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(UrlForEncoding, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);


            var imgStream = new MemoryStream();
            qrCodeImage.Save(imgStream, System.Drawing.Imaging.ImageFormat.Png);

            WorkSheet.Pictures.Add(imgStream,
                PositioningMode.MoveAndSize,
                new AnchorCell(WorkSheet.Columns[6], WorkSheet.Rows[0], 10, 10, LengthUnit.Pixel),
                new AnchorCell(WorkSheet.Columns[6], WorkSheet.Rows[2], 70, 70, LengthUnit.Pixel),
                ExcelPictureFormat.Png);
            //шрифт 10 для всех ячеек
            WorkSheet.Cells.Style.Font.Size = 11 * 20;

            //ширина колонок            
            WorkSheet.Columns[0].Width = 3 * 256;
            WorkSheet.Columns[1].Width = 8 * 256;
            WorkSheet.Columns[2].Width = 25 * 256;
            WorkSheet.Columns[3].Width = 7 * 256;
            WorkSheet.Columns[4].Width = 11 * 256;
            WorkSheet.Columns[5].Width = 34 * 256;
            WorkSheet.Columns[6].Width = 27 * 256;
            WorkSheet.Columns[7].Width = 24 * 256;
            WorkSheet.Columns[8].Width = 25 * 256;

            //шрифт заголовка + сделать жирным
            WorkSheet.Cells["C1"].Style.Font.Size = 14 * 20;
            WorkSheet.Cells["C1"].Style.Font.Weight = ExcelFont.BoldWeight;

            //заголовок отчета
            WorkSheet.Cells["C1"].Value = Params.MainHeader;

            //Дата и время создания файла            
            WorkSheet.Cells["E2"].Value = "Дата и время создания файла:" + DateTime.Now;
            WorkSheet.Cells["E2"].Style.Font.Size = 8 * 20;

            WorkSheet.Cells["B3"].Value = "ДАТА:";
            WorkSheet.Cells["D3"].Value = OrderTypeModel.OrderDate;
            WorkSheet.Cells["B3"].Style.Font.Size = 13 * 20;
            WorkSheet.Cells["B3"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D3"].Style.Font.Size = 14 * 20;
            for (int i = 3; i <= 6; i++)
            {
                for (int j = 3; j <= 4; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                }
            }

            WorkSheet.Cells["B4"].Value = "СОСТАВИЛ:";
            WorkSheet.Cells["D4"].Value = OrderTypeModel.CreatorPosition;
            WorkSheet.Cells["B4"].Style.Font.Size = 13 * 20;
            WorkSheet.Cells["D4"].Style.Font.Size = 12 * 20;
            WorkSheet.Cells["F4"].Style.Font.Size = 12 * 20;
            WorkSheet.Cells["F4"].Value = OrderTypeModel.CreatorContact;
            WorkSheet.Cells["B4"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D4"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["F4"].Style.Font.Weight = ExcelFont.BoldWeight;

            WorkSheet.Cells["D5"].Value = "/ФИО, Должность/";
            WorkSheet.Cells["D5"].Style.Font.Size = 8 * 20;

            WorkSheet.Cells["B6"].Value = "Заполняется заказчиком";
            WorkSheet.Cells["B6"].Style.Font.Size = 16 * 20;
            WorkSheet.Cells["B6"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B6"].Style.Font.Italic = true;
            WorkSheet.Cells["B6"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + "6"].Style.Borders.SetBorders(
                    MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);
                WorkSheet.Cells[GetExcelColumnName(i) + "6"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));
            }

            WorkSheet.Cells.GetSubrangeAbsolute(5, 1, 5, 6).Merged = true;

            WorkSheet.Cells["B7"].Value = "Заказчик (Плательщик) за транспортировку: ";
            WorkSheet.Cells["B7"].Style.Font.Size = 13 * 20;
            WorkSheet.Cells["B7"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B7"].Style.Font.Italic = true;
            WorkSheet.Cells.GetSubrangeAbsolute(6, 1, 6, 6).Merged = true;
            WorkSheet.Cells["B7"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + "7"].Style.Borders.SetBorders(
                    MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Medium);

                WorkSheet.Cells[GetExcelColumnName(i) + "7"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));
            }

            WorkSheet.Cells["B8"].Value = "Наименование организации";
            WorkSheet.Cells.GetSubrangeAbsolute(7, 1, 7, 2).Merged = true;
            WorkSheet.Cells["B8"].Style.Font.Weight = ExcelFont.BoldWeight;

            WorkSheet.Cells["D8"].Style.Borders.SetBorders(
                  MultipleBorders.Bottom,
                  Color.Black, LineStyle.Thin);


            WorkSheet.Cells["B8"].Style.Borders.SetBorders(MultipleBorders.Left, Color.Black, LineStyle.Medium);
            WorkSheet.Cells["B8"].Style.Borders.SetBorders(MultipleBorders.Right, Color.Black, LineStyle.Thin);
            WorkSheet.Cells["G8"].Style.Borders.SetBorders(MultipleBorders.Right, Color.Black, LineStyle.Medium);

            WorkSheet.Cells["B9"].Value = "Контактное лицо/ тел.";
            WorkSheet.Cells.GetSubrangeAbsolute(8, 1, 8, 2).Merged = true;
            WorkSheet.Cells["B9"].Style.Font.Weight = ExcelFont.BoldWeight;


            WorkSheet.Cells["B9"].Style.Borders.SetBorders(MultipleBorders.Left, Color.Black, LineStyle.Medium);
            WorkSheet.Cells["G9"].Style.Borders.SetBorders(MultipleBorders.Right, Color.Black, LineStyle.Medium);
            WorkSheet.Cells["D9"].Style.Borders.SetBorders(MultipleBorders.Left, Color.Black, LineStyle.Thin);

            for (int i = 2; i <= 7; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + "9"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                    Color.Black, LineStyle.Medium);
                WorkSheet.Cells[GetExcelColumnName(i) + "9"].Style.Borders.SetBorders(MultipleBorders.Top,
                 Color.Black, LineStyle.Thin);
            }

            WorkSheet.Cells["D8"].Value = OrderTypeModel.PayerName;
            WorkSheet.Cells["D8"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(7, 3, 7, 6).Merged = true;
            WorkSheet.Cells["D8"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            //WorkSheet.Cells["D9"].Style.Borders.SetBorders(MultipleBorders.Top | MultipleBorders.Bottom, Color.Black, LineStyle.Thin);

            WorkSheet.Cells["D9"].Value = OrderTypeModel.CreatorPosition + "/" + OrderTypeModel.CreatorContact;
            WorkSheet.Cells["D9"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(8, 3, 8, 6).Merged = true;
            WorkSheet.Cells["D9"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["B11"].Value = "Информация о грузе:";
            WorkSheet.Cells["B11"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B11"].Style.Font.Italic = true;
            WorkSheet.Cells.GetSubrangeAbsolute(10, 1, 10, 6).Merged = true;
            WorkSheet.Cells["B11"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "11"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["B12"].Value = "Наименование груза";
            WorkSheet.Cells["B12"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(11, 1, 11, 2).Merged = true;

            WorkSheet.Cells["D12"].Value = extOrderTypeModel.TruckDescription;
            WorkSheet.Cells["D12"].Style.WrapText = true;
            WorkSheet.Cells.GetSubrangeAbsolute(11, 3, 11, 5).Merged = true;
            WorkSheet.Cells["D12"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["D12"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D12"].Style.Font.Italic = true;

            WorkSheet.Cells["G12"].Value = extOrderTypeModel.TruckTypeName;
            WorkSheet.Cells["G12"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["G12"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            //WorkSheet.Cells["G12"].Style.FillPattern.SetPattern(FillPatternStyle.Solid, SpreadsheetColor.FromName(ColorName.LightGreen), SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["B13"].Value = "Вес, т:";
            WorkSheet.Cells["B13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["C13"].Value = extOrderTypeModel.Weight;
            WorkSheet.Cells["C13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B13"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells["C13"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells["C13"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["D13"].Value = "Объем, м3";
            WorkSheet.Cells["D13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D13"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells["E13"].Value = extOrderTypeModel.Volume;
            WorkSheet.Cells["E13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["E13"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells["E13"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["F13"].Value = "Упаковка ";
            WorkSheet.Cells["G13"].Value = extOrderTypeModel.BoxingDescription;
            WorkSheet.Cells["F13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["F13"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells["G13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["G13"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells["G13"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["B14"].Value = "Габариты / L x W x H / см /негабарит";
            WorkSheet.Cells["B14"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B14"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells.GetSubrangeAbsolute(13, 1, 13, 2).Merged = true;

            WorkSheet.Cells["D14"].Value = extOrderTypeModel.DimenssionL + " x " + extOrderTypeModel.DimenssionW + " x " +
                                           extOrderTypeModel.DimenssionH;
            WorkSheet.Cells["D14"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D14"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells.GetSubrangeAbsolute(13, 3, 13, 4).Merged = true;
            WorkSheet.Cells["D14"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["F14"].Value = "Количество мест";
            WorkSheet.Cells["F14"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["F14"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells["G14"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["B15"].Value = "Необходимое кол-во автомобилей *";
            WorkSheet.Cells["B15"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B15"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells.GetSubrangeAbsolute(14, 3, 14, 4).Merged = true;
            WorkSheet.Cells.GetSubrangeAbsolute(14, 1, 14, 2).Merged = true;
            WorkSheet.Cells["D15"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells["D15"].Value = OrderTypeModel.CarNumber;

            WorkSheet.Cells["B16"].Value = "Тип авто/вид загрузки/выгрузки";
            WorkSheet.Cells["B16"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B16"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells.GetSubrangeAbsolute(15, 1, 15, 2).Merged = true;

            WorkSheet.Cells["D16"].Value = extOrderTypeModel.VehicleTypeName;
            //WorkSheet.Cells["D16"].Style.FillPattern.SetPattern(FillPatternStyle.Solid, SpreadsheetColor.FromName(ColorName.LightGreen), SpreadsheetColor.FromName(ColorName.Automatic));
            WorkSheet.Cells.GetSubrangeAbsolute(15, 3, 15, 4).Merged = true;
            WorkSheet.Cells["D16"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["F16"].Value = extOrderTypeModel.LoadingTypeName;
            //WorkSheet.Cells["F16"].Style.FillPattern.SetPattern(FillPatternStyle.Solid, SpreadsheetColor.FromName(ColorName.LightGreen), SpreadsheetColor.FromName(ColorName.Automatic));
            WorkSheet.Cells["F16"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["G16"].Value = extOrderTypeModel.UnloadingTypeName;
            //WorkSheet.Cells["G16"].Style.FillPattern.SetPattern(FillPatternStyle.Solid, SpreadsheetColor.FromName(ColorName.LightGreen), SpreadsheetColor.FromName(ColorName.Automatic));
            WorkSheet.Cells["G16"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
            {
                for (int j = 12; j <= 16; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                }
            }

            WorkSheet.Cells["B11"].Style.Borders.SetBorders(
                MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top,
                Color.Black, LineStyle.Medium);
            for (int i = 12; i <= 16; i++)
            {
                WorkSheet.Cells["B" + i].Style.Borders.SetBorders(MultipleBorders.Left,
                    Color.Black, LineStyle.Medium);

                WorkSheet.Cells["G" + i].Style.Borders.SetBorders(MultipleBorders.Right,
                    Color.Black, LineStyle.Medium);
            }

            WorkSheet.Cells["B16"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);
            WorkSheet.Cells["D16"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);
            WorkSheet.Cells["F16"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);
            WorkSheet.Cells["G16"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);

            WorkSheet.Cells["B18"].Value = "Сроки подачи/доставки";
            WorkSheet.Cells["B18"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B18"].Style.Font.Italic = true;
            WorkSheet.Cells.GetSubrangeAbsolute(17, 1, 17, 6).Merged = true;
            WorkSheet.Cells["B18"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "18"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells.GetSubrangeAbsolute(18, 1, 18, 2).Merged = true;
            WorkSheet.Cells["D19"].Value = "Дата";
            WorkSheet.Cells["D19"].Style.Font.Size = 8 * 20;
            WorkSheet.Cells["D19"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D19"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells.GetSubrangeAbsolute(18, 3, 18, 5).Merged = true;

            WorkSheet.Cells["G19"].Value = "Время";
            WorkSheet.Cells["G19"].Style.Font.Size = 8 * 20;
            WorkSheet.Cells["G19"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["G19"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["B20"].Value = "Дата и время подачи автомобиля(ей) грузоотправителю *";
            WorkSheet.Cells["B20"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B20"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells.GetSubrangeAbsolute(19, 1, 19, 2).Merged = true;
            WorkSheet.Cells["B20"].Style.WrapText = true;
            WorkSheet.Cells["B20"].Row.Height = 600;

            WorkSheet.Cells["D20"].Value = extOrderTypeModel.FromShipperDate;
            WorkSheet.Cells["D20"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D20"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells.GetSubrangeAbsolute(19, 3, 19, 5).Merged = true;

            WorkSheet.Cells["G20"].Value = extOrderTypeModel.FromShipperTime;
            WorkSheet.Cells["G20"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["G20"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["B21"].Value = "Дата и время доставки груза грузополучателю *";
            //WorkSheet.Cells["B21"].Row.AutoFit();            

            WorkSheet.Cells["B21"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B21"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells.GetSubrangeAbsolute(20, 1, 20, 2).Merged = true;
            WorkSheet.Cells["B21"].Style.WrapText = true;
            WorkSheet.Cells["B21"].Row.Height = 600;

            WorkSheet.Cells["D21"].Value = extOrderTypeModel.ToConsigneeDate;
            WorkSheet.Cells["D21"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D21"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells.GetSubrangeAbsolute(20, 3, 20, 5).Merged = true;
            WorkSheet.Cells.GetSubrangeAbsolute(21, 3, 21, 6).Merged = true;

            WorkSheet.Cells["G21"].Value = extOrderTypeModel.ToConsigneeTime;
            WorkSheet.Cells["G21"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["G21"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["B22"].Value = "Особые условия перевозки";
            WorkSheet.Cells["B22"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(21, 1, 21, 2).Merged = true;
            WorkSheet.Cells.GetSubrangeAbsolute(22, 1, 22, 2).Merged = true;

            WorkSheet.Cells["D22"].Value = extOrderTypeModel.OrderDescription;
            WorkSheet.Cells["D22"].Style.Font.Weight = ExcelFont.BoldWeight;

            for (int i = 2; i <= 7; i++)
            {

                for (int j = 19; j <= 22; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                }
            }

            WorkSheet.Cells["B18"].Style.Borders.SetBorders(
                MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top,
                Color.Black, LineStyle.Medium);
            for (int i = 19; i <= 22; i++)
            {
                WorkSheet.Cells["B" + i].Style.Borders.SetBorders(MultipleBorders.Left,
                    Color.Black, LineStyle.Medium);

                WorkSheet.Cells["G" + i].Style.Borders.SetBorders(MultipleBorders.Right,
                    Color.Black, LineStyle.Medium);
            }
            WorkSheet.Cells["B22"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);
            WorkSheet.Cells["D22"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);

            WorkSheet.Cells["B24"].Value = "Грузоотправитель:";
            WorkSheet.Cells["B24"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B24"].Style.Font.Italic = true;
            WorkSheet.Cells.GetSubrangeAbsolute(23, 1, 23, 6).Merged = true;
            WorkSheet.Cells["B24"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "24"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["B25"].Value = "Наименование организации";
            WorkSheet.Cells["B25"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(24, 1, 24, 2).Merged = true;

            if (extOrderTypeModel.Shipper == "")
                WorkSheet.Cells["D25"].Value = extOrderTypeModel.OrganizationLoadPoints;
            else
                WorkSheet.Cells["D25"].Value = "1) " + extOrderTypeModel.Shipper + "\n" + extOrderTypeModel.OrganizationLoadPoints;
            WorkSheet.Cells["D25"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(24, 3, 24, 6).Merged = true;
            WorkSheet.Cells["D25"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            WorkSheet.Cells["D25"].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            WorkSheet.Cells["D25"].Style.Font.Italic = true;
            WorkSheet.Cells["D25"].Style.WrapText = true;
            double cntHeight = WorkSheet.Cells["D25"].Value.ToString().Length * 2 / 75;
            if (cntHeight == 0) cntHeight = 1;
            if (cntHeight < WorkSheet.Cells["D25"].Value.ToString().Count(x => x == '\n'))
                cntHeight = WorkSheet.Cells["D25"].Value.ToString().Count(x => x == '\n') * 1.3;
            WorkSheet.Cells["D25"].Row.Height = (int)(WorkSheet.Cells["D25"].Row.Height * cntHeight);

            WorkSheet.Cells["B26"].Value = "Адрес загрузки";
            WorkSheet.Cells["B26"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(25, 1, 25, 2).Merged = true;

            string ShipperAddress = "";
            if (extOrderTypeModel.TripType == 2)
                ShipperAddress = extOrderTypeModel.ShipperCountryName + " " +
                extOrderTypeModel.ShipperCity + " " + extOrderTypeModel.ShipperAdress;
            else
                ShipperAddress = extOrderTypeModel.ShipperCity + " " + extOrderTypeModel.ShipperAdress;

            //if  (ShipperAddress == "")
            //    WorkSheet.Cells["D26"].Value = extOrderTypeModel.AddressLoadPoints;
            //else
            WorkSheet.Cells["D26"].Value = "1) " + ShipperAddress + "\n" + extOrderTypeModel.AddressLoadPoints;
            WorkSheet.Cells["D26"].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            WorkSheet.Cells["D26"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(25, 3, 25, 6).Merged = true;
            WorkSheet.Cells["D26"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            WorkSheet.Cells["D26"].Style.Font.Italic = true;
            WorkSheet.Cells["D26"].Style.WrapText = true;
            WorkSheet.Cells["B27"].Value = "Контактное лицо / тел.";
            WorkSheet.Cells["B27"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(26, 1, 26, 2).Merged = true;
            cntHeight = WorkSheet.Cells["D26"].Value.ToString().Length * 2 / 75;
            if (cntHeight == 0) cntHeight = 1;
            if (cntHeight < WorkSheet.Cells["D26"].Value.ToString().Count(x => x == '\n'))
                cntHeight = WorkSheet.Cells["D26"].Value.ToString().Count(x => x == '\n') * 1.3;
            WorkSheet.Cells["D26"].Row.Height = (int)(WorkSheet.Cells["D26"].Row.Height * cntHeight);

            string ShipperContact = "";
            ShipperContact = extOrderTypeModel.ShipperContactPerson + "/" +
                                           extOrderTypeModel.ShipperContactPersonPhone;

            //if (ShipperContact == "")
            //    WorkSheet.Cells["D27"].Value = extOrderTypeModel.ContactsLoadPoints;
            //else
            WorkSheet.Cells["D27"].Value = "1) " + ShipperContact + "\n" + extOrderTypeModel.ContactsLoadPoints;
            WorkSheet.Cells["D27"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(26, 3, 26, 6).Merged = true;
            WorkSheet.Cells["D27"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            WorkSheet.Cells["D27"].Style.Font.Italic = true;
            WorkSheet.Cells["D27"].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            cntHeight = WorkSheet.Cells["D27"].Value.ToString().Length * 2 / 75;
            if (cntHeight == 0) cntHeight = 1;
            if (cntHeight < WorkSheet.Cells["D27"].Value.ToString().Count(x => x == '\n'))
                cntHeight = WorkSheet.Cells["D27"].Value.ToString().Count(x => x == '\n') * 1.3;
            WorkSheet.Cells["D27"].Row.Height = (int)(WorkSheet.Cells["D27"].Row.Height * cntHeight);


            for (int i = 2; i <= 7; i++)
            {
                for (int j = 25; j <= 27; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                }
            }

            WorkSheet.Cells["B24"].Style.Borders.SetBorders(
                MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top,
                Color.Black, LineStyle.Medium);

            for (int i = 25; i <= 27; i++)
            {
                WorkSheet.Cells["B" + i].Style.Borders.SetBorders(MultipleBorders.Left,
                    Color.Black, LineStyle.Medium);

                WorkSheet.Cells["G" + i].Style.Borders.SetBorders(MultipleBorders.Right,
                    Color.Black, LineStyle.Medium);
            }
            WorkSheet.Cells["B27"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);
            WorkSheet.Cells["D27"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);


            WorkSheet.Cells["B29"].Value = "Грузополучатель:";
            WorkSheet.Cells["B29"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B29"].Style.Font.Italic = true;
            WorkSheet.Cells.GetSubrangeAbsolute(28, 1, 28, 6).Merged = true;
            WorkSheet.Cells["B29"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "29"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["B30"].Value = "Наименование организации";
            WorkSheet.Cells["B30"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(29, 1, 29, 2).Merged = true;

            int countUnLoadPoints = extOrderTypeModel.CountUnLoadPoints + 1;
            if (extOrderTypeModel.Consignee == "")
                WorkSheet.Cells["D30"].Value = extOrderTypeModel.OrganizationUnLoadPoints;
            else
                WorkSheet.Cells["D30"].Value = extOrderTypeModel.OrganizationUnLoadPoints + countUnLoadPoints.ToString() + ") " + extOrderTypeModel.Consignee;
            WorkSheet.Cells["D30"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(29, 3, 29, 6).Merged = true;
            WorkSheet.Cells["D30"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            WorkSheet.Cells["D30"].Style.VerticalAlignment = VerticalAlignmentStyle.Top;

            WorkSheet.Cells["D30"].Style.Font.Italic = true;
            WorkSheet.Cells["D30"].Style.WrapText = true;
            cntHeight = WorkSheet.Cells["D30"].Value.ToString().Length * 2 / 75;
            if (cntHeight == 0) cntHeight = 1;
            if (cntHeight < WorkSheet.Cells["D30"].Value.ToString().Count(x => x == '\n'))
                cntHeight = WorkSheet.Cells["D30"].Value.ToString().Count(x => x == '\n') * 1.3;
            WorkSheet.Cells["D30"].Row.Height = (int)(WorkSheet.Cells["D30"].Row.Height * cntHeight);

            WorkSheet.Cells["B31"].Value = "Адрес выгрузки";
            WorkSheet.Cells["B31"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(30, 1, 30, 2).Merged = true;
            WorkSheet.Cells["B31"].Style.WrapText = true;

            string ConsigneeAddress = "";
            if (extOrderTypeModel.TripType == 2)
                ConsigneeAddress = extOrderTypeModel.ConsigneeCountryName + " " +
                                   extOrderTypeModel.ConsigneeCity + " " + extOrderTypeModel.ConsigneeAdress;
            else
                ConsigneeAddress = extOrderTypeModel.ConsigneeCity + " " + extOrderTypeModel.ConsigneeAdress;
            if (ConsigneeAddress == "")
                WorkSheet.Cells["D31"].Value = extOrderTypeModel.AddressUnLoadPoints;
            else
                WorkSheet.Cells["D31"].Value = extOrderTypeModel.AddressUnLoadPoints + countUnLoadPoints.ToString() + ") " + ConsigneeAddress;

            WorkSheet.Cells["D31"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(30, 3, 30, 6).Merged = true;
            WorkSheet.Cells["D31"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            WorkSheet.Cells["D31"].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            WorkSheet.Cells["D31"].Style.Font.Italic = true;

            //WorkSheet.Cells["D31"].Style.WrapText = true;
            cntHeight = WorkSheet.Cells["D31"].Value.ToString().Length * 2 / 75;
            if (cntHeight == 0) cntHeight = 1;
            if (cntHeight < WorkSheet.Cells["D31"].Value.ToString().Count(x => x == '\n'))
                cntHeight = WorkSheet.Cells["D31"].Value.ToString().Count(x => x == '\n') * 1.3;
            WorkSheet.Cells["D31"].Row.Height = (int)(WorkSheet.Cells["D31"].Row.Height * cntHeight);
            //extOrderTypeModel.AddressUnLoadPoints.Count(x => x == '\n');
            // WorkSheet.Cells["D31"].Row.Height = WorkSheet.Cells["D31"].Row.Height * cntHeight;

            WorkSheet.Cells["B32"].Value = "Контактное лицо / тел.";
            WorkSheet.Cells["B32"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(31, 1, 31, 2).Merged = true;
            string ConsigneeContact = "";
            ConsigneeContact = extOrderTypeModel.ConsigneeContactPerson + "/" +
                               extOrderTypeModel.ConsigneeContactPersonPhone;
            //if (ConsigneeContact == "")
            //WorkSheet.Cells["D32"].Value = extOrderTypeModel.ContactsUnLoadPoints;
            //else
            WorkSheet.Cells["D32"].Value = extOrderTypeModel.ContactsUnLoadPoints + countUnLoadPoints.ToString() + ") " + ConsigneeContact;
            WorkSheet.Cells["D32"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(31, 3, 31, 6).Merged = true;
            WorkSheet.Cells["D32"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            WorkSheet.Cells["D32"].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            WorkSheet.Cells["D32"].Style.Font.Italic = true;

            cntHeight = WorkSheet.Cells["D32"].Value.ToString().Length * 2 / 75;
            if (cntHeight == 0) cntHeight = 1;
            if (cntHeight < WorkSheet.Cells["D32"].Value.ToString().Count(x => x == '\n'))
                cntHeight = WorkSheet.Cells["D32"].Value.ToString().Count(x => x == '\n') * 1.3;
            WorkSheet.Cells["D32"].Row.Height = (int)(WorkSheet.Cells["D32"].Row.Height * cntHeight);

            for (int i = 2; i <= 7; i++)
            {
                for (int j = 30; j <= 32; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                }
            }

            WorkSheet.Cells["B29"].Style.Borders.SetBorders(
                MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top,
                Color.Black, LineStyle.Medium);

            for (int i = 30; i <= 32; i++)
            {
                WorkSheet.Cells["B" + i].Style.Borders.SetBorders(MultipleBorders.Left,
                    Color.Black, LineStyle.Medium);

                WorkSheet.Cells["G" + i].Style.Borders.SetBorders(MultipleBorders.Right,
                    Color.Black, LineStyle.Medium);
            }
            WorkSheet.Cells["B32"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);
            WorkSheet.Cells["D32"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);

            //if (carList.Count > 0)
            //{
            WorkSheet.Cells["B34"].Value = "ЗАПОЛНЯЕТСЯ ЭКСПЕДИТОРОМ-ПЕРЕВОЗЧИКОМ:";
            WorkSheet.Cells["B34"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B34"].Style.Font.Italic = true;
            WorkSheet.Cells["B34"].Style.Font.Size = 16 * 20;
            WorkSheet.Cells.GetSubrangeAbsolute(33, 1, 33, 6).Merged = true;
            WorkSheet.Cells["B34"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + "34"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));
            }

            WorkSheet.Cells["B34"].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top,
                      Color.Black, LineStyle.Medium);

            //}
            //добавим пустую запись, чтобы даже если нет выбранного перевозчика/экспедитора выводить пустую часть блока "ЗАПОЛНЯЕТСЯ ЭКСПЕДИТОРОМ"
            if (carList.Count == 0)
            {
                //OrderUsedCarViewModel car = new OrderUsedCarViewModel();
                carList.Add(new OrderUsedCarViewModel());

            }

            int RowCount = 35;
            int RowCountStart = 35;

            foreach (var car in carList)
            {
                RowCountStart = RowCount;

                WorkSheet.Cells["B" + RowCount].Value = "Экспедитор:  " + car.ExpeditorName + " согласно договора " + car.ContractInfo;
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["B" + RowCount].Style.Font.Italic = true;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 6).Merged = true;

                for (int i = 2; i <= 7; i++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.FillPattern.SetPattern(
                        FillPatternStyle.Solid, SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                        SpreadsheetColor.FromName(ColorName.Automatic));
                }

                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = "Маршрут движения";
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 2).Merged = true;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 3, RowCount - 1, 6).Merged = true;
                WorkSheet.Cells["D" + RowCount].Value = OrderTypeModel.ShortName;
                WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["D" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = "Расстояние, км";
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 2).Merged = true;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 3, RowCount - 1, 6).Merged = true;
                if (Convert.ToDecimal(extOrderTypeModel.TotalDistanceLenght) == 0)
                    WorkSheet.Cells["D" + RowCount].Value = "";
                else
                    WorkSheet.Cells["D" + RowCount].Value = extOrderTypeModel.TotalDistanceLenght;
                WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["D" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = "Стоимость перевозки, грн.";
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 2).Merged = true;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 3, RowCount - 1, 6).Merged = true;
                if (extOrderTypeModel.TotalCost == "")
                    WorkSheet.Cells["D" + RowCount].Value = "";
                else
                    WorkSheet.Cells["D" + RowCount].Value = extOrderTypeModel.TotalCost;
                WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["D" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;


                for (int i = 2; i <= 7; i++)
                {
                    for (int j = RowCountStart; j <= RowCount; j++)
                    {
                        WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                            Color.Black, LineStyle.Thin);
                    }
                }

                WorkSheet.Cells["B" + (RowCountStart - 1)].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top,
                            Color.Black, LineStyle.Medium);

                for (int i = RowCountStart; i <= RowCount; i++)
                {
                    WorkSheet.Cells["B" + i].Style.Borders.SetBorders(MultipleBorders.Left,
                        Color.Black, LineStyle.Medium);

                    WorkSheet.Cells["G" + i].Style.Borders.SetBorders(MultipleBorders.Right,
                        Color.Black, LineStyle.Medium);
                }


                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = "Информация об автомобиле и водителе*";

                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["B" + RowCount].Style.Font.Italic = true;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 6).Merged = true;
                WorkSheet.Cells["B" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                for (int i = 2; i <= 7; i++)
                    WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.FillPattern.SetPattern(
                        FillPatternStyle.Solid, SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                        SpreadsheetColor.FromName(ColorName.Automatic));

                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = "Наименование Перевозчика";
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["B" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 2).Merged = true;

                WorkSheet.Cells["D" + RowCount].Value = "Марка, модель, тип ТС";
                WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["D" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 3, RowCount - 1, 4).Merged = true;
                WorkSheet.Cells["D" + RowCount].Row.Height = 600;
                WorkSheet.Cells["D" + RowCount].Style.WrapText = true;

                WorkSheet.Cells["F" + RowCount].Value = "Рег.номер ТС";
                WorkSheet.Cells["F" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["F" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                WorkSheet.Cells["G" + RowCount].Value = "Грузоподъемность, тн";
                WorkSheet.Cells["G" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["G" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = car.CarrierInfo;
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 2).Merged = true;

                WorkSheet.Cells["D" + RowCount].Value = car.CarModelInfo;
                WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 3, RowCount - 1, 4).Merged = true;
                WorkSheet.Cells["D" + RowCount].Row.Height = 600;

                WorkSheet.Cells["F" + RowCount].Value = car.CarRegNum;
                WorkSheet.Cells["F" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;

                WorkSheet.Cells["G" + RowCount].Value = car.CarCapacity;
                WorkSheet.Cells["G" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;
                WorkSheet.Cells["G" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                for (int i = 2; i <= 7; i++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + "41"].Style.Borders.SetBorders(
                        MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                }


                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = "ЕДРПОУ или ИНН/№ паспорта";
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["B" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 2).Merged = true;

                WorkSheet.Cells["D" + RowCount].Value = "ФИО водителя, тел., № вод.удостоверения";
                WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["D" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 3, RowCount - 1, 4).Merged = true;
                WorkSheet.Cells["D" + RowCount].Row.Height = 600;
                WorkSheet.Cells["D" + RowCount].Style.WrapText = true;

                WorkSheet.Cells["F" + RowCount].Value = "Габариты ТС(ДхШхВ), мм*";
                WorkSheet.Cells["F" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["F" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                WorkSheet.Cells["G" + RowCount].Value = "";
                WorkSheet.Cells["G" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["G" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = car.seriesPassportNumber;
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 2).Merged = true;

                WorkSheet.Cells["D" + RowCount].Value = car.CarDriverInfo + "\n" + car.DriverContactInfo + "\n" +
                                                        car.DriverCardInfo;
                WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 3, RowCount - 1, 4).Merged = true;
                WorkSheet.Cells["D" + RowCount].Row.Height = 600;

                WorkSheet.Cells["F" + RowCount].Value = car.transportDimensions;
                WorkSheet.Cells["F" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;

                WorkSheet.Cells["G" + RowCount].Value = "";
                WorkSheet.Cells["G" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;
                WorkSheet.Cells["G" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                RowCount++;
                //}
            }

            if (carList.Count > 0) RowCount--;

            for (int i = 2; i <= 7; i++)
            {
                for (int j = 39; j <= RowCount; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                }
            }


            for (int i = 39; i <= RowCount; i++)
            {
                WorkSheet.Cells["B" + i].Style.Borders.SetBorders(MultipleBorders.Left,
                    Color.Black, LineStyle.Medium);

                WorkSheet.Cells["G" + i].Style.Borders.SetBorders(MultipleBorders.Right,
                    Color.Black, LineStyle.Medium);
            }

            if (carList.Count > 0)
            {
                for (int i = 2; i <= 7; i++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.Borders.SetBorders(MultipleBorders.Bottom,
                        Color.Black, LineStyle.Medium);
                }
            }

            if (carList.Count <= 0) RowCount = 32;
            RowCount = RowCount + 3;
            WorkSheet.Cells["B" + RowCount].Value = "ЗАКАЗЧИК ___________________";
            // WorkSheet.Cells["C" + RowCount].Value = " И.О.Руководителя КР и СО";
            WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 3).Merged = true;
            WorkSheet.Cells["C" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["E" + RowCount].Value = "                          Экспедитор_____________________";

            RowCount = RowCount + 2;

            WorkSheet.Cells["C" + RowCount].Value = "ФИО";
            WorkSheet.Cells["F" + RowCount].Value = "ФИО";

            WorkSheet.Cells["B" + RowCount].Style.Borders.SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Thin);
            WorkSheet.Cells["C" + RowCount].Style.Borders.SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Thin);

            WorkSheet.Cells["E" + RowCount].Style.Borders.SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Thin);
            WorkSheet.Cells["F" + RowCount].Style.Borders.SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Thin);

            /*********** 2 лист **********/

            //ширина колонок            
            WorkSheet2.Columns[0].Width = 14 * 256;
            WorkSheet2.Columns[1].Width = 10 * 256;
            WorkSheet2.Columns[2].Width = 20 * 256;
            WorkSheet2.Columns[3].Width = 20 * 256;
            WorkSheet2.Columns[4].Width = 18 * 256;
            WorkSheet2.Columns[5].Width = 18 * 256;
            WorkSheet2.Columns[6].Width = 15 * 256;
            WorkSheet2.Columns[7].Width = 22 * 256;
            WorkSheet2.Columns[8].Width = 16 * 256;//I

            WorkSheet2.Columns[9].Width = 8 * 256;
            WorkSheet2.Columns[10].Width = 8 * 256;
            WorkSheet2.Columns[11].Width = 8 * 256;
            WorkSheet2.Columns[12].Width = 8 * 256;
            WorkSheet2.Columns[13].Width = 8 * 256;
            WorkSheet2.Columns[14].Width = 10 * 256;//O

            WorkSheet2.Columns[15].Width = 10 * 256;
            WorkSheet2.Columns[16].Width = 12 * 256;
            WorkSheet2.Columns[17].Width = 12 * 256;//I
            WorkSheet2.Columns[18].Width = 8 * 256;
            WorkSheet2.Columns[19].Width = 8 * 256;//T
            WorkSheet2.Columns[20].Width = 13 * 256;

            WorkSheet2.Columns[21].Width = 9 * 256;
            WorkSheet2.Columns[22].Width = 11 * 256;
            WorkSheet2.Columns[23].Width = 13 * 256;

            //форматирование
            for (int i = 1; i <= 24; i++)
            {
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.WrapText = true;
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Borders.SetBorders(MultipleBorders.Bottom,
                        Color.Black, LineStyle.Medium);
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Borders.SetBorders(
                       MultipleBorders.Left | MultipleBorders.Right, Color.Black, LineStyle.Thin);
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                        SpreadsheetColor.FromName(ColorName.Accent1Lighter80Pct),
                        SpreadsheetColor.FromName(ColorName.Automatic));

                WorkSheet2.Cells[GetExcelColumnName(i) + 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                WorkSheet2.Cells[GetExcelColumnName(i) + 2].Style.WrapText = true;
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Font.Size = 10 * 20;
                WorkSheet2.Cells[GetExcelColumnName(i) + 2].Style.Font.Size = 11 * 20;
            }

            WorkSheet2.Cells["A1"].Value = "Номер заявки (при разбитие заявки на строки использовать нумерацию с \" / \")";
            WorkSheet2.Cells["A2"].Value = OrderTypeModel.Id.ToString();
            WorkSheet2.Cells["B1"].Value = "Дата указанная в заявке";
            WorkSheet2.Cells["B2"].Value = OrderTypeModel.OrderDate;
            WorkSheet2.Cells["C1"].Value = "Заказчик/Плательщик (по справочнику) ";
            WorkSheet2.Cells["C2"].Value = OrderTypeModel.PayerName;
            WorkSheet2.Cells["D1"].Value = "Автор заявки";
            WorkSheet2.Cells["D2"].Value = OrderTypeModel.CreatorPosition;
            WorkSheet2.Cells["E1"].Value = "Грузоотправитель (по справочнику)";
            WorkSheet2.Cells["E2"].Value = extOrderTypeModel.Shipper;

            WorkSheet2.Cells["F1"].Value = "Грузополучатель (по справочнику)";
            WorkSheet2.Cells["F2"].Value = extOrderTypeModel.Consignee;
            WorkSheet2.Cells["G1"].Value = "Пункт отправления (по справочника)";

            if (extOrderTypeModel.TripType == 2)
                WorkSheet2.Cells["G2"].Value = extOrderTypeModel.ShipperCountryName + " " +
                                               extOrderTypeModel.ShipperCity + " " + extOrderTypeModel.ShipperAdress;
            else
                WorkSheet2.Cells["G2"].Value = extOrderTypeModel.ShipperCity + " " + extOrderTypeModel.ShipperAdress;

            WorkSheet2.Cells["H1"].Value = "Пункт прибытия (по справочнику)";
            if (extOrderTypeModel.TripType == 2)
                WorkSheet2.Cells["H2"].Value = extOrderTypeModel.ConsigneeCountryName + " " +
                                               extOrderTypeModel.ConsigneeCity + " " + extOrderTypeModel.ConsigneeAdress;
            else
                WorkSheet2.Cells["H2"].Value = extOrderTypeModel.ConsigneeCity + " " + extOrderTypeModel.ConsigneeAdress;

            WorkSheet2.Cells["I1"].Value = "Наименование груза";
            WorkSheet2.Cells["I2"].Value = extOrderTypeModel.TruckDescription;

            WorkSheet2.Cells["J1"].Value = "Вес груза";
            WorkSheet2.Cells["J2"].Value = extOrderTypeModel.Weight;
            WorkSheet2.Cells["K1"].Value = "Тип авто/кузова";
            WorkSheet2.Cells["K2"].Value = extOrderTypeModel.VehicleTypeName;
            WorkSheet2.Cells["L1"].Value = "Вид загрузки";
            WorkSheet2.Cells["L2"].Value = extOrderTypeModel.LoadingTypeName;

            WorkSheet2.Cells["M1"].Value = "Ограничения по выгрузке";
            WorkSheet2.Cells["M2"].Value = extOrderTypeModel.UnloadingTypeName;
            WorkSheet2.Cells["N1"].Value = "К-во авто к подаче";
            WorkSheet2.Cells["N2"].Value = extOrderTypeModel.CarNumber;
            WorkSheet2.Cells["O1"].Value = "Дата подачи авто по заявке";
            WorkSheet2.Cells["O2"].Value = extOrderTypeModel.FromShipperDate;

            WorkSheet2.Cells["P1"].Value = "Время подачи авто по заявке";
            WorkSheet2.Cells["P2"].Value = extOrderTypeModel.FromShipperTime;

            WorkSheet2.Cells["Q1"].Value = "Дата доставки груза по заявке";
            WorkSheet2.Cells["Q2"].Value = extOrderTypeModel.ToConsigneeDate;

            WorkSheet2.Cells["R1"].Value = "Время доставки груза по заявке";
            WorkSheet2.Cells["R2"].Value = extOrderTypeModel.ToConsigneeTime;

            WorkSheet2.Cells["S1"].Value = "Дата подачи заявки";
            WorkSheet2.Cells["S2"].Value = OrderTypeModel.CreateDatetime.ToShortDateString();

            WorkSheet2.Cells["T1"].Value = "Время подачи заявки";
            WorkSheet2.Cells["T2"].Value = OrderTypeModel.CreateDatetime.ToShortTimeString();

            WorkSheet2.Cells["U1"].Value = "Тип груза";
            WorkSheet2.Cells["U2"].Value = extOrderTypeModel.TruckTypeName;

            WorkSheet2.Cells["V1"].Value = "Сумма точек загрузки и выгрузки";
            WorkSheet2.Cells["V2"].Value = extOrderTypeModel.CountLoadAndUnLoadPoints;

            WorkSheet2.Cells["W1"].Value = "Длина марш., км";

            WorkSheet2.Cells["X1"].Value = "Тип маршрута (по справочнику)";


            byte[] fileContents;
            var options = SaveOptions.XlsxDefault;

            using (var stream = new MemoryStream())
            {
                ef.Save(stream, options);
                fileContents = stream.ToArray();
            }
            return fileContents;


        }

        private byte[] TruckOrderRenderReport(OrderBaseViewModel OrderTypeModel, string AcceptDate,
            OrderClientsViewModel orderClientInfo, RestParamsInfo Params, string AdressFrom, string AdressTo,
            string ContractName, OrdersTruckTransportViewModel extOrderTypeModel, List<OrderUsedCarViewModel> carList, DataToAndFromContragent data)
        {
            //Пример генерации QR кода
            string UrlForEncoding =
#if DEBUG

                $"http://uh218479-1.ukrdomen.com/Orders/UpdateOrder/{OrderTypeModel.Id}";
#else
                              
                                $"https://corumsource.com/Orders/UpdateOrder/{OrderTypeModel.Id}";
#endif


            ExcelFile ef = new ExcelFile();
            CultureInfo ci = new CultureInfo(Params.Language);

            ExcelWorksheet WorkSheet = ef.Worksheets.Add("Заявка грузовой");
            ExcelWorksheet WorkSheet2 = ef.Worksheets.Add("Данные для учета");

            WorkSheet.PrintOptions.PaperType = PaperType.A4;
            WorkSheet.PrintOptions.FitWorksheetWidthToPages = 1;

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(UrlForEncoding, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);


            var imgStream = new MemoryStream();
            qrCodeImage.Save(imgStream, System.Drawing.Imaging.ImageFormat.Png);

            WorkSheet.Pictures.Add(imgStream,
                PositioningMode.MoveAndSize,
                new AnchorCell(WorkSheet.Columns[6], WorkSheet.Rows[0], 10, 10, LengthUnit.Pixel),
                new AnchorCell(WorkSheet.Columns[6], WorkSheet.Rows[2], 70, 70, LengthUnit.Pixel),
                ExcelPictureFormat.Png);
            //шрифт 10 для всех ячеек
            WorkSheet.Cells.Style.Font.Size = 11 * 20;

            //ширина колонок            
            WorkSheet.Columns[0].Width = 3 * 256;
            WorkSheet.Columns[1].Width = 8 * 256;
            WorkSheet.Columns[2].Width = 25 * 256;
            WorkSheet.Columns[3].Width = 15 * 256;
            WorkSheet.Columns[4].Width = 15 * 256;
            WorkSheet.Columns[5].Width = 34 * 256;
            WorkSheet.Columns[6].Width = 27 * 256;
            WorkSheet.Columns[7].Width = 24 * 256;
            WorkSheet.Columns[8].Width = 25 * 256;
            WorkSheet.Rows[40].AutoFit();
            //WorkSheet.Rows[40].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            //WorkSheet.Rows[40].Style.VerticalAlignment = VerticalAlignmentStyle.Bottom;


            //шрифт заголовка + сделать жирным
            WorkSheet.Cells["C1"].Style.Font.Size = 14 * 20;
            WorkSheet.Cells["C1"].Style.Font.Weight = ExcelFont.BoldWeight;

            //заголовок отчета
            WorkSheet.Cells["C1"].Value = Params.MainHeader;

            //Дата и время создания файла            
            WorkSheet.Cells["E2"].Value = "Дата и время создания файла:" + DateTime.Now;
            WorkSheet.Cells["E2"].Style.Font.Size = 8 * 20;

            WorkSheet.Cells["B3"].Value = "ДАТА:";
            WorkSheet.Cells["D3"].Value = OrderTypeModel.OrderDate;
            WorkSheet.Cells["B3"].Style.Font.Size = 13 * 20;
            WorkSheet.Cells["B3"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D3"].Style.Font.Size = 14 * 20;
            for (int i = 3; i <= 6; i++)
            {
                for (int j = 3; j <= 4; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                }
            }

            WorkSheet.Cells["B4"].Value = "СОСТАВИЛ:";
            WorkSheet.Cells["D4"].Value = OrderTypeModel.CreatorPosition;
            WorkSheet.Cells["B4"].Style.Font.Size = 13 * 20;
            WorkSheet.Cells["D4"].Style.Font.Size = 12 * 20;
            WorkSheet.Cells["F4"].Style.Font.Size = 12 * 20;
            WorkSheet.Cells["F4"].Value = OrderTypeModel.CreatorContact;
            WorkSheet.Cells["B4"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D4"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["F4"].Style.Font.Weight = ExcelFont.BoldWeight;

            WorkSheet.Cells["D5"].Value = "/ФИО, Должность/";
            WorkSheet.Cells["D5"].Style.Font.Size = 8 * 20;

            WorkSheet.Cells["B6"].Value = "Заполняется заказчиком";
            WorkSheet.Cells["B6"].Style.Font.Size = 16 * 20;
            WorkSheet.Cells["B6"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B6"].Style.Font.Italic = true;
            WorkSheet.Cells["B6"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + "6"].Style.Borders.SetBorders(
                    MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);
                WorkSheet.Cells[GetExcelColumnName(i) + "6"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));
            }

            WorkSheet.Cells.GetSubrangeAbsolute(5, 1, 5, 6).Merged = true;

            WorkSheet.Cells["B7"].Value = "Заказчик (Плательщик) за транспортировку: ";
            WorkSheet.Cells["B7"].Style.Font.Size = 13 * 20;
            WorkSheet.Cells["B7"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B7"].Style.Font.Italic = true;
            WorkSheet.Cells.GetSubrangeAbsolute(6, 1, 6, 6).Merged = true;
            WorkSheet.Cells["B7"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + "7"].Style.Borders.SetBorders(
                    MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Medium);

                WorkSheet.Cells[GetExcelColumnName(i) + "7"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));
            }

            WorkSheet.Cells["B8"].Value = "Наименование организации";
            WorkSheet.Cells.GetSubrangeAbsolute(7, 1, 7, 2).Merged = true;
            WorkSheet.Cells["B8"].Style.Font.Weight = ExcelFont.BoldWeight;

            WorkSheet.Cells["D8"].Style.Borders.SetBorders(
                  MultipleBorders.Bottom,
                  Color.Black, LineStyle.Thin);


            WorkSheet.Cells["B8"].Style.Borders.SetBorders(MultipleBorders.Left, Color.Black, LineStyle.Medium);
            WorkSheet.Cells["B8"].Style.Borders.SetBorders(MultipleBorders.Right, Color.Black, LineStyle.Thin);
            WorkSheet.Cells["G8"].Style.Borders.SetBorders(MultipleBorders.Right, Color.Black, LineStyle.Medium);

            WorkSheet.Cells["B9"].Value = "Контактное лицо/ тел.";
            WorkSheet.Cells.GetSubrangeAbsolute(8, 1, 8, 2).Merged = true;
            WorkSheet.Cells["B9"].Style.Font.Weight = ExcelFont.BoldWeight;


            WorkSheet.Cells["B9"].Style.Borders.SetBorders(MultipleBorders.Left, Color.Black, LineStyle.Medium);
            WorkSheet.Cells["G9"].Style.Borders.SetBorders(MultipleBorders.Right, Color.Black, LineStyle.Medium);
            WorkSheet.Cells["D9"].Style.Borders.SetBorders(MultipleBorders.Left, Color.Black, LineStyle.Thin);

            for (int i = 2; i <= 7; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + "9"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                    Color.Black, LineStyle.Medium);
                WorkSheet.Cells[GetExcelColumnName(i) + "9"].Style.Borders.SetBorders(MultipleBorders.Top,
                 Color.Black, LineStyle.Thin);
            }

            WorkSheet.Cells["D8"].Value = OrderTypeModel.PayerName;
            WorkSheet.Cells["D8"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(7, 3, 7, 6).Merged = true;
            WorkSheet.Cells["D8"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            //WorkSheet.Cells["D9"].Style.Borders.SetBorders(MultipleBorders.Top | MultipleBorders.Bottom, Color.Black, LineStyle.Thin);

            WorkSheet.Cells["D9"].Value = OrderTypeModel.CreatorPosition + "/" + OrderTypeModel.CreatorContact;
            WorkSheet.Cells["D9"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(8, 3, 8, 6).Merged = true;
            WorkSheet.Cells["D9"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["B11"].Value = "Информация о грузе:";
            WorkSheet.Cells["B11"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B11"].Style.Font.Italic = true;
            WorkSheet.Cells.GetSubrangeAbsolute(10, 1, 10, 6).Merged = true;
            WorkSheet.Cells["B11"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "11"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["B12"].Value = "Наименование груза";
            WorkSheet.Cells["B12"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(11, 1, 11, 2).Merged = true;

            WorkSheet.Cells["D12"].Value = extOrderTypeModel.TruckDescription;
            WorkSheet.Cells["D12"].Style.WrapText = true;
            WorkSheet.Cells.GetSubrangeAbsolute(11, 3, 11, 5).Merged = true;
            WorkSheet.Cells["D12"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["D12"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D12"].Style.Font.Italic = true;

            WorkSheet.Cells["G12"].Value = extOrderTypeModel.TruckTypeName;
            WorkSheet.Cells["G12"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["G12"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            //WorkSheet.Cells["G12"].Style.FillPattern.SetPattern(FillPatternStyle.Solid, SpreadsheetColor.FromName(ColorName.LightGreen), SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["B13"].Value = "Вес, т:";
            WorkSheet.Cells["B13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["C13"].Value = data.regmesstocontrag.weightCargo;
            WorkSheet.Cells["C13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B13"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells["C13"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells["C13"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["D13"].Value = "Объем, м3";
            WorkSheet.Cells["D13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D13"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells["E13"].Value = extOrderTypeModel.Volume;
            WorkSheet.Cells["E13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["E13"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells["E13"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["F13"].Value = "Упаковка ";
            WorkSheet.Cells["G13"].Value = extOrderTypeModel.BoxingDescription;
            WorkSheet.Cells["F13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["F13"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells["G13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["G13"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells["G13"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["B14"].Value = "Габариты / L x W x H / см /негабарит";
            WorkSheet.Cells["B14"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B14"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells.GetSubrangeAbsolute(13, 1, 13, 2).Merged = true;

            WorkSheet.Cells["D14"].Value = extOrderTypeModel.DimenssionL + " x " + extOrderTypeModel.DimenssionW + " x " +
                                           extOrderTypeModel.DimenssionH;
            WorkSheet.Cells["D14"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D14"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells.GetSubrangeAbsolute(13, 3, 13, 4).Merged = true;
            WorkSheet.Cells["D14"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["F14"].Value = "Количество мест";
            WorkSheet.Cells["F14"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["F14"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells["G14"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["B15"].Value = "Необходимое кол-во автомобилей *";
            WorkSheet.Cells["B15"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B15"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells.GetSubrangeAbsolute(14, 3, 14, 4).Merged = true;
            WorkSheet.Cells.GetSubrangeAbsolute(14, 1, 14, 2).Merged = true;
            WorkSheet.Cells["D15"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells["D15"].Value = OrderTypeModel.CarNumber;

            WorkSheet.Cells["B16"].Value = "Тип авто/вид загрузки/выгрузки";
            WorkSheet.Cells["B16"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B16"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells.GetSubrangeAbsolute(15, 1, 15, 2).Merged = true;

            WorkSheet.Cells["D16"].Value = extOrderTypeModel.VehicleTypeName;
            //WorkSheet.Cells["D16"].Style.FillPattern.SetPattern(FillPatternStyle.Solid, SpreadsheetColor.FromName(ColorName.LightGreen), SpreadsheetColor.FromName(ColorName.Automatic));
            WorkSheet.Cells.GetSubrangeAbsolute(15, 3, 15, 4).Merged = true;
            WorkSheet.Cells["D16"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["F16"].Value = extOrderTypeModel.LoadingTypeName;
            //WorkSheet.Cells["F16"].Style.FillPattern.SetPattern(FillPatternStyle.Solid, SpreadsheetColor.FromName(ColorName.LightGreen), SpreadsheetColor.FromName(ColorName.Automatic));
            WorkSheet.Cells["F16"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["G16"].Value = extOrderTypeModel.UnloadingTypeName;
            //WorkSheet.Cells["G16"].Style.FillPattern.SetPattern(FillPatternStyle.Solid, SpreadsheetColor.FromName(ColorName.LightGreen), SpreadsheetColor.FromName(ColorName.Automatic));
            WorkSheet.Cells["G16"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
            {
                for (int j = 12; j <= 16; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                }
            }

            WorkSheet.Cells["B11"].Style.Borders.SetBorders(
                MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top,
                Color.Black, LineStyle.Medium);
            for (int i = 12; i <= 16; i++)
            {
                WorkSheet.Cells["B" + i].Style.Borders.SetBorders(MultipleBorders.Left,
                    Color.Black, LineStyle.Medium);

                WorkSheet.Cells["G" + i].Style.Borders.SetBorders(MultipleBorders.Right,
                    Color.Black, LineStyle.Medium);
            }

            WorkSheet.Cells["B16"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);
            WorkSheet.Cells["D16"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);
            WorkSheet.Cells["F16"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);
            WorkSheet.Cells["G16"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);

            WorkSheet.Cells["B18"].Value = "Сроки подачи/доставки";
            WorkSheet.Cells["B18"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B18"].Style.Font.Italic = true;
            WorkSheet.Cells.GetSubrangeAbsolute(17, 1, 17, 6).Merged = true;
            WorkSheet.Cells["B18"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "18"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells.GetSubrangeAbsolute(18, 1, 18, 2).Merged = true;
            WorkSheet.Cells["D19"].Value = "Дата";
            WorkSheet.Cells["D19"].Style.Font.Size = 8 * 20;
            WorkSheet.Cells["D19"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D19"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells.GetSubrangeAbsolute(18, 3, 18, 5).Merged = true;

            WorkSheet.Cells["G19"].Value = "Время";
            WorkSheet.Cells["G19"].Style.Font.Size = 8 * 20;
            WorkSheet.Cells["G19"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["G19"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["B20"].Value = "Дата и время подачи автомобиля(ей) грузоотправителю *";
            WorkSheet.Cells["B20"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B20"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells.GetSubrangeAbsolute(19, 1, 19, 2).Merged = true;
            WorkSheet.Cells["B20"].Style.WrapText = true;
            WorkSheet.Cells["B20"].Row.Height = 600;

            WorkSheet.Cells["D20"].Value = extOrderTypeModel.FromShipperDate;
            WorkSheet.Cells["D20"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D20"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells.GetSubrangeAbsolute(19, 3, 19, 5).Merged = true;

            WorkSheet.Cells["G20"].Value = extOrderTypeModel.FromShipperTime;
            WorkSheet.Cells["G20"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["G20"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["B21"].Value = "Дата и время доставки груза грузополучателю *";
            //WorkSheet.Cells["B21"].Row.AutoFit();            

            WorkSheet.Cells["B21"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B21"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells.GetSubrangeAbsolute(20, 1, 20, 2).Merged = true;
            WorkSheet.Cells["B21"].Style.WrapText = true;
            WorkSheet.Cells["B21"].Row.Height = 600;

            WorkSheet.Cells["D21"].Value = extOrderTypeModel.ToConsigneeDate;
            WorkSheet.Cells["D21"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D21"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells.GetSubrangeAbsolute(20, 3, 20, 5).Merged = true;
            WorkSheet.Cells.GetSubrangeAbsolute(21, 3, 21, 6).Merged = true;

            WorkSheet.Cells["G21"].Value = extOrderTypeModel.ToConsigneeTime;
            WorkSheet.Cells["G21"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["G21"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["B22"].Value = "Особые условия перевозки";
            WorkSheet.Cells["B22"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(21, 1, 21, 2).Merged = true;
            WorkSheet.Cells.GetSubrangeAbsolute(22, 1, 22, 2).Merged = true;

            WorkSheet.Cells["D22"].Value = extOrderTypeModel.OrderDescription;
            WorkSheet.Cells["D22"].Style.Font.Weight = ExcelFont.BoldWeight;

            for (int i = 2; i <= 7; i++)
            {

                for (int j = 19; j <= 22; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                }
            }

            WorkSheet.Cells["B18"].Style.Borders.SetBorders(
                MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top,
                Color.Black, LineStyle.Medium);
            for (int i = 19; i <= 22; i++)
            {
                WorkSheet.Cells["B" + i].Style.Borders.SetBorders(MultipleBorders.Left,
                    Color.Black, LineStyle.Medium);

                WorkSheet.Cells["G" + i].Style.Borders.SetBorders(MultipleBorders.Right,
                    Color.Black, LineStyle.Medium);
            }
            WorkSheet.Cells["B22"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);
            WorkSheet.Cells["D22"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);

            WorkSheet.Cells["B24"].Value = "Грузоотправитель:";
            WorkSheet.Cells["B24"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B24"].Style.Font.Italic = true;
            WorkSheet.Cells.GetSubrangeAbsolute(23, 1, 23, 6).Merged = true;
            WorkSheet.Cells["B24"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "24"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["B25"].Value = "Наименование организации";
            WorkSheet.Cells["B25"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(24, 1, 24, 2).Merged = true;

            if (extOrderTypeModel.Shipper == "")
                WorkSheet.Cells["D25"].Value = extOrderTypeModel.OrganizationLoadPoints;
            else
                WorkSheet.Cells["D25"].Value = "1) " + extOrderTypeModel.Shipper + "\n" + extOrderTypeModel.OrganizationLoadPoints;
            WorkSheet.Cells["D25"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(24, 3, 24, 6).Merged = true;
            WorkSheet.Cells["D25"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            WorkSheet.Cells["D25"].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            WorkSheet.Cells["D25"].Style.Font.Italic = true;
            WorkSheet.Cells["D25"].Style.WrapText = true;
            double cntHeight = WorkSheet.Cells["D25"].Value.ToString().Length * 2 / 75;
            if (cntHeight == 0) cntHeight = 1;
            if (cntHeight < WorkSheet.Cells["D25"].Value.ToString().Count(x => x == '\n'))
                cntHeight = WorkSheet.Cells["D25"].Value.ToString().Count(x => x == '\n') * 1.3;
            WorkSheet.Cells["D25"].Row.Height = (int)(WorkSheet.Cells["D25"].Row.Height * cntHeight);

            WorkSheet.Cells["B26"].Value = "Адрес загрузки";
            WorkSheet.Cells["B26"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(25, 1, 25, 2).Merged = true;

            string ShipperAddress = "";
            if (extOrderTypeModel.TripType == 2)
                ShipperAddress = extOrderTypeModel.ShipperCountryName + " " +
                extOrderTypeModel.ShipperCity + " " + extOrderTypeModel.ShipperAdress;
            else
                ShipperAddress = extOrderTypeModel.ShipperCity + " " + extOrderTypeModel.ShipperAdress;

            //if  (ShipperAddress == "")
            //    WorkSheet.Cells["D26"].Value = extOrderTypeModel.AddressLoadPoints;
            //else
            WorkSheet.Cells["D26"].Value = "1) " + ShipperAddress + "\n" + extOrderTypeModel.AddressLoadPoints;
            WorkSheet.Cells["D26"].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            WorkSheet.Cells["D26"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(25, 3, 25, 6).Merged = true;
            WorkSheet.Cells["D26"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            WorkSheet.Cells["D26"].Style.Font.Italic = true;
            WorkSheet.Cells["D26"].Style.WrapText = true;
            WorkSheet.Cells["B27"].Value = "Контактное лицо / тел.";
            WorkSheet.Cells["B27"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(26, 1, 26, 2).Merged = true;
            cntHeight = WorkSheet.Cells["D26"].Value.ToString().Length * 2 / 75;
            if (cntHeight == 0) cntHeight = 1;
            if (cntHeight < WorkSheet.Cells["D26"].Value.ToString().Count(x => x == '\n'))
                cntHeight = WorkSheet.Cells["D26"].Value.ToString().Count(x => x == '\n') * 1.3;
            WorkSheet.Cells["D26"].Row.Height = (int)(WorkSheet.Cells["D26"].Row.Height * cntHeight);

            string ShipperContact = "";
            ShipperContact = extOrderTypeModel.ShipperContactPerson + "/" +
                                           extOrderTypeModel.ShipperContactPersonPhone;

            //if (ShipperContact == "")
            //    WorkSheet.Cells["D27"].Value = extOrderTypeModel.ContactsLoadPoints;
            //else
            WorkSheet.Cells["D27"].Value = "1) " + ShipperContact + "\n" + extOrderTypeModel.ContactsLoadPoints;
            WorkSheet.Cells["D27"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(26, 3, 26, 6).Merged = true;
            WorkSheet.Cells["D27"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            WorkSheet.Cells["D27"].Style.Font.Italic = true;
            WorkSheet.Cells["D27"].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            cntHeight = WorkSheet.Cells["D27"].Value.ToString().Length * 2 / 75;
            if (cntHeight == 0) cntHeight = 1;
            if (cntHeight < WorkSheet.Cells["D27"].Value.ToString().Count(x => x == '\n'))
                cntHeight = WorkSheet.Cells["D27"].Value.ToString().Count(x => x == '\n') * 1.3;
            WorkSheet.Cells["D27"].Row.Height = (int)(WorkSheet.Cells["D27"].Row.Height * cntHeight);


            for (int i = 2; i <= 7; i++)
            {
                for (int j = 25; j <= 27; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                }
            }

            WorkSheet.Cells["B24"].Style.Borders.SetBorders(
                MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top,
                Color.Black, LineStyle.Medium);

            for (int i = 25; i <= 27; i++)
            {
                WorkSheet.Cells["B" + i].Style.Borders.SetBorders(MultipleBorders.Left,
                    Color.Black, LineStyle.Medium);

                WorkSheet.Cells["G" + i].Style.Borders.SetBorders(MultipleBorders.Right,
                    Color.Black, LineStyle.Medium);
            }
            WorkSheet.Cells["B27"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);
            WorkSheet.Cells["D27"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);


            WorkSheet.Cells["B29"].Value = "Грузополучатель:";
            WorkSheet.Cells["B29"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B29"].Style.Font.Italic = true;
            WorkSheet.Cells.GetSubrangeAbsolute(28, 1, 28, 6).Merged = true;
            WorkSheet.Cells["B29"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "29"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["B30"].Value = "Наименование организации";
            WorkSheet.Cells["B30"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(29, 1, 29, 2).Merged = true;

            int countUnLoadPoints = extOrderTypeModel.CountUnLoadPoints + 1;
            if (extOrderTypeModel.Consignee == "")
                WorkSheet.Cells["D30"].Value = extOrderTypeModel.OrganizationUnLoadPoints;
            else
                WorkSheet.Cells["D30"].Value = extOrderTypeModel.OrganizationUnLoadPoints + countUnLoadPoints.ToString() + ") " + extOrderTypeModel.Consignee;
            WorkSheet.Cells["D30"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(29, 3, 29, 6).Merged = true;
            WorkSheet.Cells["D30"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            WorkSheet.Cells["D30"].Style.VerticalAlignment = VerticalAlignmentStyle.Top;

            WorkSheet.Cells["D30"].Style.Font.Italic = true;
            WorkSheet.Cells["D30"].Style.WrapText = true;
            cntHeight = WorkSheet.Cells["D30"].Value.ToString().Length * 2 / 75;
            if (cntHeight == 0) cntHeight = 1;
            if (cntHeight < WorkSheet.Cells["D30"].Value.ToString().Count(x => x == '\n'))
                cntHeight = WorkSheet.Cells["D30"].Value.ToString().Count(x => x == '\n') * 1.3;
            WorkSheet.Cells["D30"].Row.Height = (int)(WorkSheet.Cells["D30"].Row.Height * cntHeight);

            WorkSheet.Cells["B31"].Value = "Адрес выгрузки";
            WorkSheet.Cells["B31"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(30, 1, 30, 2).Merged = true;
            WorkSheet.Cells["B31"].Style.WrapText = true;

            string ConsigneeAddress = "";
            if (extOrderTypeModel.TripType == 2)
                ConsigneeAddress = extOrderTypeModel.ConsigneeCountryName + " " +
                                   extOrderTypeModel.ConsigneeCity + " " + extOrderTypeModel.ConsigneeAdress;
            else
                ConsigneeAddress = extOrderTypeModel.ConsigneeCity + " " + extOrderTypeModel.ConsigneeAdress;
            if (ConsigneeAddress == "")
                WorkSheet.Cells["D31"].Value = extOrderTypeModel.AddressUnLoadPoints;
            else
                WorkSheet.Cells["D31"].Value = extOrderTypeModel.AddressUnLoadPoints + countUnLoadPoints.ToString() + ") " + ConsigneeAddress;

            WorkSheet.Cells["D31"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(30, 3, 30, 6).Merged = true;
            WorkSheet.Cells["D31"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            WorkSheet.Cells["D31"].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            WorkSheet.Cells["D31"].Style.Font.Italic = true;

            //WorkSheet.Cells["D31"].Style.WrapText = true;
            cntHeight = WorkSheet.Cells["D31"].Value.ToString().Length * 2 / 75;
            if (cntHeight == 0) cntHeight = 1;
            if (cntHeight < WorkSheet.Cells["D31"].Value.ToString().Count(x => x == '\n'))
                cntHeight = WorkSheet.Cells["D31"].Value.ToString().Count(x => x == '\n') * 1.3;
            WorkSheet.Cells["D31"].Row.Height = (int)(WorkSheet.Cells["D31"].Row.Height * cntHeight);
            //extOrderTypeModel.AddressUnLoadPoints.Count(x => x == '\n');
            // WorkSheet.Cells["D31"].Row.Height = WorkSheet.Cells["D31"].Row.Height * cntHeight;

            WorkSheet.Cells["B32"].Value = "Контактное лицо / тел.";
            WorkSheet.Cells["B32"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(31, 1, 31, 2).Merged = true;
            string ConsigneeContact = "";
            ConsigneeContact = extOrderTypeModel.ConsigneeContactPerson + "/" +
                               extOrderTypeModel.ConsigneeContactPersonPhone;
            //if (ConsigneeContact == "")
            //WorkSheet.Cells["D32"].Value = extOrderTypeModel.ContactsUnLoadPoints;
            //else
            WorkSheet.Cells["D32"].Value = extOrderTypeModel.ContactsUnLoadPoints + countUnLoadPoints.ToString() + ") " + ConsigneeContact;
            WorkSheet.Cells["D32"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(31, 3, 31, 6).Merged = true;
            WorkSheet.Cells["D32"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            WorkSheet.Cells["D32"].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            WorkSheet.Cells["D32"].Style.Font.Italic = true;

            cntHeight = WorkSheet.Cells["D32"].Value.ToString().Length * 2 / 75;
            if (cntHeight == 0) cntHeight = 1;
            if (cntHeight < WorkSheet.Cells["D32"].Value.ToString().Count(x => x == '\n'))
                cntHeight = WorkSheet.Cells["D32"].Value.ToString().Count(x => x == '\n') * 1.3;
            WorkSheet.Cells["D32"].Row.Height = (int)(WorkSheet.Cells["D32"].Row.Height * cntHeight);

            for (int i = 2; i <= 7; i++)
            {
                for (int j = 30; j <= 32; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                }
            }

            WorkSheet.Cells["B29"].Style.Borders.SetBorders(
                MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top,
                Color.Black, LineStyle.Medium);

            for (int i = 30; i <= 32; i++)
            {
                WorkSheet.Cells["B" + i].Style.Borders.SetBorders(MultipleBorders.Left,
                    Color.Black, LineStyle.Medium);

                WorkSheet.Cells["G" + i].Style.Borders.SetBorders(MultipleBorders.Right,
                    Color.Black, LineStyle.Medium);
            }
            WorkSheet.Cells["B32"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);
            WorkSheet.Cells["D32"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);

            //if (carList.Count > 0)
            //{
            WorkSheet.Cells["B34"].Value = "ЗАПОЛНЯЕТСЯ ЭКСПЕДИТОРОМ-ПЕРЕВОЗЧИКОМ:";
            WorkSheet.Cells["B34"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B34"].Style.Font.Italic = true;
            WorkSheet.Cells["B34"].Style.Font.Size = 16 * 20;
            WorkSheet.Cells.GetSubrangeAbsolute(33, 1, 33, 6).Merged = true;
            WorkSheet.Cells["B34"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + "34"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));
            }

            WorkSheet.Cells["B34"].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top,
                      Color.Black, LineStyle.Medium);

            //}
            //добавим пустую запись, чтобы даже если нет выбранного перевозчика/экспедитора выводить пустую часть блока "ЗАПОЛНЯЕТСЯ ЭКСПЕДИТОРОМ"
            if (carList.Count == 0)
            {
                //OrderUsedCarViewModel car = new OrderUsedCarViewModel();
                carList.Add(new OrderUsedCarViewModel());

            }

            int RowCount = 35;
            int RowCountStart = 35;

            foreach (var car in carList)
            {
                RowCountStart = RowCount;

                WorkSheet.Cells["B" + RowCount].Value = "Экспедитор:  " + data.regmesstocontrag.contragentName + " согласно договора " + car.ContractExpBkInfo;
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["B" + RowCount].Style.Font.Italic = true;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 6).Merged = true;

                for (int i = 2; i <= 7; i++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.FillPattern.SetPattern(
                        FillPatternStyle.Solid, SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                        SpreadsheetColor.FromName(ColorName.Automatic));
                }

                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = "Маршрут движения";
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 2).Merged = true;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 3, RowCount - 1, 6).Merged = true;
                WorkSheet.Cells["D" + RowCount].Value = data.regmesstocontrag.routeShort;
                WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["D" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = "Расстояние, км";
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 2).Merged = true;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 3, RowCount - 1, 6).Merged = true;
                if (Convert.ToDecimal(extOrderTypeModel.TotalDistanceLenght) == 0)
                    WorkSheet.Cells["D" + RowCount].Value = data.formFromContr.distance;
                else
                    WorkSheet.Cells["D" + RowCount].Value = data.formFromContr.distance;
                WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["D" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = "Стоимость перевозки, грн без НДС";
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 2).Merged = true;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 3, RowCount - 1, 6).Merged = true;
                if (extOrderTypeModel.TotalCost == "")
                    WorkSheet.Cells["D" + RowCount].Value = data.regmesstocontrag.cost;
                else
                    WorkSheet.Cells["D" + RowCount].Value = data.regmesstocontrag.cost;
                WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["D" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;


                for (int i = 2; i <= 7; i++)
                {
                    for (int j = RowCountStart; j <= RowCount; j++)
                    {
                        WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                            Color.Black, LineStyle.Thin);
                    }
                }

                WorkSheet.Cells["B" + (RowCountStart - 1)].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top,
                            Color.Black, LineStyle.Medium);

                for (int i = RowCountStart; i <= RowCount; i++)
                {
                    WorkSheet.Cells["B" + i].Style.Borders.SetBorders(MultipleBorders.Left,
                        Color.Black, LineStyle.Medium);

                    WorkSheet.Cells["G" + i].Style.Borders.SetBorders(MultipleBorders.Right,
                        Color.Black, LineStyle.Medium);
                }


                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = "Информация об автомобиле и водителе*";

                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["B" + RowCount].Style.Font.Italic = true;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 6).Merged = true;
                WorkSheet.Cells["B" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                for (int i = 2; i <= 7; i++)
                    WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.FillPattern.SetPattern(
                        FillPatternStyle.Solid, SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                        SpreadsheetColor.FromName(ColorName.Automatic));

                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = "Наименование Перевозчика";
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["B" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 2).Merged = true;

                WorkSheet.Cells["D" + RowCount].Value = "Марка, модель, тип ТС";
                WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["D" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 3, RowCount - 1, 4).Merged = true;
                WorkSheet.Cells["D" + RowCount].Row.Height = 600;
                WorkSheet.Cells["D" + RowCount].Style.WrapText = true;

                WorkSheet.Cells["F" + RowCount].Value = "Рег.номер ТС";
                WorkSheet.Cells["F" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["F" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                WorkSheet.Cells["G" + RowCount].Value = "Грузоподъемность, тн";
                WorkSheet.Cells["G" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["G" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = car.CarrierInfo;
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 2).Merged = true;

                WorkSheet.Cells["D" + RowCount].Value = car.CarModelInfo;
                WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 3, RowCount - 1, 4).Merged = true;
                WorkSheet.Cells["D" + RowCount].Row.Height = 600;

                WorkSheet.Cells["F" + RowCount].Value = car.CarRegNum;
                WorkSheet.Cells["F" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;

                WorkSheet.Cells["G" + RowCount].Value = car.CarCapacity;
                WorkSheet.Cells["G" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;
                WorkSheet.Cells["G" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                for (int i = 2; i <= 7; i++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + "41"].Style.Borders.SetBorders(
                        MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                }


                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = "ЕДРПОУ или ИНН/№ паспорта";
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["B" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 2).Merged = true;

                WorkSheet.Cells["D" + RowCount].Value = "ФИО водителя, тел., № вод.удостоверения";
                WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["D" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 3, RowCount - 1, 4).Merged = true;
                WorkSheet.Cells["D" + RowCount].Row.Height = 600;
                WorkSheet.Cells["D" + RowCount].Style.WrapText = true;

                WorkSheet.Cells["F" + RowCount].Value = "Габариты ТС(ДхШхВ), мм*";
                WorkSheet.Cells["F" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["F" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                WorkSheet.Cells["G" + RowCount].Value = "";
                WorkSheet.Cells["G" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["G" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = car.seriesPassportNumber;
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 2).Merged = true;

                WorkSheet.Cells["D" + RowCount].Value = car.CarDriverInfo + "\n" + car.DriverContactInfo + "\n" +
                                                        car.DriverCardInfo;
                WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 3, RowCount - 1, 4).Merged = true;
                WorkSheet.Cells["D" + RowCount].Row.Height = 600;

                WorkSheet.Cells["F" + RowCount].Value = car.transportDimensions;
                WorkSheet.Cells["F" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;

                WorkSheet.Cells["G" + RowCount].Value = "";
                WorkSheet.Cells["G" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;
                WorkSheet.Cells["G" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                RowCount++;
                //}
            }

            if (carList.Count > 0) RowCount--;

            for (int i = 2; i <= 7; i++)
            {
                for (int j = 39; j <= RowCount; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                }
            }


            for (int i = 39; i <= RowCount; i++)
            {
                WorkSheet.Cells["B" + i].Style.Borders.SetBorders(MultipleBorders.Left,
                    Color.Black, LineStyle.Medium);

                WorkSheet.Cells["G" + i].Style.Borders.SetBorders(MultipleBorders.Right,
                    Color.Black, LineStyle.Medium);
            }

            if (carList.Count > 0)
            {
                for (int i = 2; i <= 7; i++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.Borders.SetBorders(MultipleBorders.Bottom,
                        Color.Black, LineStyle.Medium);
                }
            }

            if (carList.Count <= 0) RowCount = 32;
            RowCount = RowCount + 3;
            WorkSheet.Cells["B" + RowCount].Value = "ЗАКАЗЧИК ___________________";
            // WorkSheet.Cells["C" + RowCount].Value = " И.О.Руководителя КР и СО";
            WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 3).Merged = true;
            WorkSheet.Cells["C" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["E" + RowCount].Value = "                          Экспедитор_____________________";

            RowCount = RowCount + 2;

            WorkSheet.Cells["C" + RowCount].Value = "ФИО";
            WorkSheet.Cells["F" + RowCount].Value = "ФИО";

            WorkSheet.Cells["B" + RowCount].Style.Borders.SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Thin);
            WorkSheet.Cells["C" + RowCount].Style.Borders.SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Thin);

            WorkSheet.Cells["E" + RowCount].Style.Borders.SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Thin);
            WorkSheet.Cells["F" + RowCount].Style.Borders.SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Thin);

            /*********** 2 лист **********/

            //ширина колонок            
            WorkSheet2.Columns[0].Width = 14 * 256;
            WorkSheet2.Columns[1].Width = 10 * 256;
            WorkSheet2.Columns[2].Width = 20 * 256;
            WorkSheet2.Columns[3].Width = 20 * 256;
            WorkSheet2.Columns[4].Width = 18 * 256;
            WorkSheet2.Columns[5].Width = 18 * 256;
            WorkSheet2.Columns[6].Width = 15 * 256;
            WorkSheet2.Columns[7].Width = 22 * 256;
            WorkSheet2.Columns[8].Width = 16 * 256;//I

            WorkSheet2.Columns[9].Width = 8 * 256;
            WorkSheet2.Columns[10].Width = 8 * 256;
            WorkSheet2.Columns[11].Width = 8 * 256;
            WorkSheet2.Columns[12].Width = 8 * 256;
            WorkSheet2.Columns[13].Width = 8 * 256;
            WorkSheet2.Columns[14].Width = 10 * 256;//O

            WorkSheet2.Columns[15].Width = 10 * 256;
            WorkSheet2.Columns[16].Width = 12 * 256;
            WorkSheet2.Columns[17].Width = 12 * 256;//I
            WorkSheet2.Columns[18].Width = 8 * 256;
            WorkSheet2.Columns[19].Width = 8 * 256;//T
            WorkSheet2.Columns[20].Width = 13 * 256;

            WorkSheet2.Columns[21].Width = 9 * 256;
            WorkSheet2.Columns[22].Width = 11 * 256;
            WorkSheet2.Columns[23].Width = 13 * 256;

            //форматирование
            for (int i = 1; i <= 24; i++)
            {
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.WrapText = true;
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Borders.SetBorders(MultipleBorders.Bottom,
                        Color.Black, LineStyle.Medium);
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Borders.SetBorders(
                       MultipleBorders.Left | MultipleBorders.Right, Color.Black, LineStyle.Thin);
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                        SpreadsheetColor.FromName(ColorName.Accent1Lighter80Pct),
                        SpreadsheetColor.FromName(ColorName.Automatic));

                WorkSheet2.Cells[GetExcelColumnName(i) + 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                WorkSheet2.Cells[GetExcelColumnName(i) + 2].Style.WrapText = true;
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Font.Size = 10 * 20;
                WorkSheet2.Cells[GetExcelColumnName(i) + 2].Style.Font.Size = 11 * 20;
            }

            WorkSheet2.Cells["A1"].Value = "Номер заявки (при разбитие заявки на строки использовать нумерацию с \" / \")";
            WorkSheet2.Cells["A2"].Value = OrderTypeModel.Id.ToString();
            WorkSheet2.Cells["B1"].Value = "Дата указанная в заявке";
            WorkSheet2.Cells["B2"].Value = OrderTypeModel.OrderDate;
            WorkSheet2.Cells["C1"].Value = "Заказчик/Плательщик (по справочнику) ";
            WorkSheet2.Cells["C2"].Value = OrderTypeModel.PayerName;
            WorkSheet2.Cells["D1"].Value = "Автор заявки";
            WorkSheet2.Cells["D2"].Value = OrderTypeModel.CreatorPosition;
            WorkSheet2.Cells["E1"].Value = "Грузоотправитель (по справочнику)";
            WorkSheet2.Cells["E2"].Value = extOrderTypeModel.Shipper;

            WorkSheet2.Cells["F1"].Value = "Грузополучатель (по справочнику)";
            WorkSheet2.Cells["F2"].Value = extOrderTypeModel.Consignee;
            WorkSheet2.Cells["G1"].Value = "Пункт отправления (по справочника)";

            if (extOrderTypeModel.TripType == 2)
                WorkSheet2.Cells["G2"].Value = extOrderTypeModel.ShipperCountryName + " " +
                                               extOrderTypeModel.ShipperCity + " " + extOrderTypeModel.ShipperAdress;
            else
                WorkSheet2.Cells["G2"].Value = extOrderTypeModel.ShipperCity + " " + extOrderTypeModel.ShipperAdress;

            WorkSheet2.Cells["H1"].Value = "Пункт прибытия (по справочнику)";
            if (extOrderTypeModel.TripType == 2)
                WorkSheet2.Cells["H2"].Value = extOrderTypeModel.ConsigneeCountryName + " " +
                                               extOrderTypeModel.ConsigneeCity + " " + extOrderTypeModel.ConsigneeAdress;
            else
                WorkSheet2.Cells["H2"].Value = extOrderTypeModel.ConsigneeCity + " " + extOrderTypeModel.ConsigneeAdress;

            WorkSheet2.Cells["I1"].Value = "Наименование груза";
            WorkSheet2.Cells["I2"].Value = extOrderTypeModel.TruckDescription;

            WorkSheet2.Cells["J1"].Value = "Вес груза";
            WorkSheet2.Cells["J2"].Value = extOrderTypeModel.Weight;
            WorkSheet2.Cells["K1"].Value = "Тип авто/кузова";
            WorkSheet2.Cells["K2"].Value = extOrderTypeModel.VehicleTypeName;
            WorkSheet2.Cells["L1"].Value = "Вид загрузки";
            WorkSheet2.Cells["L2"].Value = extOrderTypeModel.LoadingTypeName;

            WorkSheet2.Cells["M1"].Value = "Ограничения по выгрузке";
            WorkSheet2.Cells["M2"].Value = extOrderTypeModel.UnloadingTypeName;
            WorkSheet2.Cells["N1"].Value = "К-во авто к подаче";
            WorkSheet2.Cells["N2"].Value = extOrderTypeModel.CarNumber;
            WorkSheet2.Cells["O1"].Value = "Дата подачи авто по заявке";
            WorkSheet2.Cells["O2"].Value = extOrderTypeModel.FromShipperDate;

            WorkSheet2.Cells["P1"].Value = "Время подачи авто по заявке";
            WorkSheet2.Cells["P2"].Value = extOrderTypeModel.FromShipperTime;

            WorkSheet2.Cells["Q1"].Value = "Дата доставки груза по заявке";
            WorkSheet2.Cells["Q2"].Value = extOrderTypeModel.ToConsigneeDate;

            WorkSheet2.Cells["R1"].Value = "Время доставки груза по заявке";
            WorkSheet2.Cells["R2"].Value = extOrderTypeModel.ToConsigneeTime;

            WorkSheet2.Cells["S1"].Value = "Дата подачи заявки";
            WorkSheet2.Cells["S2"].Value = OrderTypeModel.CreateDatetime.ToShortDateString();

            WorkSheet2.Cells["T1"].Value = "Время подачи заявки";
            WorkSheet2.Cells["T2"].Value = OrderTypeModel.CreateDatetime.ToShortTimeString();

            WorkSheet2.Cells["U1"].Value = "Тип груза";
            WorkSheet2.Cells["U2"].Value = extOrderTypeModel.TruckTypeName;

            WorkSheet2.Cells["V1"].Value = "Сумма точек загрузки и выгрузки";
            WorkSheet2.Cells["V2"].Value = extOrderTypeModel.CountLoadAndUnLoadPoints;

            WorkSheet2.Cells["W1"].Value = "Длина марш., км";

            WorkSheet2.Cells["X1"].Value = "Тип маршрута (по справочнику)";


            byte[] fileContents;
            var options = SaveOptions.XlsxDefault;

            using (var stream = new MemoryStream())
            {
                ef.Save(stream, options);
                fileContents = stream.ToArray();
            }
            return fileContents;


        }

        public byte[] StatusReportRenderReport<T>(RestDataInfo<T> Data, RestParamsInfo Params, List<string> statusOrderSumm)
        {
            ExcelFile ef = new ExcelFile();
            CultureInfo ci = new CultureInfo(Params.Language);
            ExcelWorksheet WorkSheet = ef.Worksheets.Add("report");
            WorkSheet.PrintOptions.Portrait = false;
            WorkSheet.PrintOptions.PaperType = PaperType.A4;
            WorkSheet.PrintOptions.FitWorksheetWidthToPages = 1;

            //шрифт 10 для всех ячеек
            WorkSheet.Cells.Style.Font.Size = 11 * 20;

            //ширина колонок            
            WorkSheet.Columns[0].Width = 20 * 256;
            WorkSheet.Columns[1].Width = 17 * 256;
            WorkSheet.Columns[2].Width = 19 * 256;
            WorkSheet.Columns[3].Width = 24 * 256;
            WorkSheet.Columns[4].Width = 11 * 256;
            WorkSheet.Columns[5].Width = 11 * 256;
            WorkSheet.Columns[6].Width = 12 * 256;

            //шрифт заголовка + сделать жирным
            WorkSheet.Cells["A3"].Style.Font.Size = 14 * 20;
            WorkSheet.Cells["A3"].Style.Font.Weight = ExcelFont.BoldWeight;

            //заголовок отчета
            WorkSheet.Cells["A3"].Value = Params.MainHeader;

            //Дата и время создания файла            
            WorkSheet.Cells["E3"].Value = "Дата и время создания файла:" + DateTime.Now;
            WorkSheet.Cells["E3"].Style.Font.Size = 8 * 20;

            //шапка
            WorkSheet.Cells["A4"].Value = "Вид перевозки";
            WorkSheet.Cells["B4"].Value = "Заказчик";
            WorkSheet.Cells["C4"].Value = "Итог";
            WorkSheet.Cells["D4"].Value = "Плановая";
            WorkSheet.Cells["E4"].Value = "% плановых";
            WorkSheet.Cells["F4"].Value = "Срочная";
            WorkSheet.Cells["G4"].Value = "% срочных";

            //оформление стиля заголовков таблицы
            CellStyle tmpStyle = new CellStyle();
            tmpStyle.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            tmpStyle.VerticalAlignment = VerticalAlignmentStyle.Center;
            tmpStyle.FillPattern.SetSolid(Color.Chocolate);
            tmpStyle.Font.Weight = ExcelFont.BoldWeight;
            tmpStyle.Font.Color = Color.Black;
            tmpStyle.Font.Size = 10 * 20;
            tmpStyle.WrapText = true;

            for (int i = 1; i <= 7; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + 4].Style = tmpStyle;
                WorkSheet.Cells[GetExcelColumnName(i) + 4 /*+ j*/].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);

            }

            //вывод данных
            int RowCount = 5;
            //int ii = 13;
            foreach (var data in Data.Rows)
            {
                // for (int i = 1; i <= 14; i++)
                //{
                WorkSheet.Cells["A" + RowCount].Value = GetPropValue(data, "TruckTypeName"); //1

                WorkSheet.Cells["B" + RowCount].Value = GetPropValue(data, "PayerName"); //2
                WorkSheet.Cells["C" + RowCount].Value = GetPropValue(data, "CntAll"); //3 

                WorkSheet.Cells["D" + RowCount].Value = GetPropValue(data, "CntZero").ToString();
                WorkSheet.Cells["E" + RowCount].Value = GetPropValue(data, "CntZeroPercentRaw"); //4

                WorkSheet.Cells["F" + RowCount].Value = GetPropValue(data, "CntOne"); //5
                WorkSheet.Cells["G" + RowCount].Value = GetPropValue(data, "CntOnePercentRaw"); //6                

                RowCount++;
            }

            for (int i = 1; i <= 7; i++)
            {
                for (int j = 5; j <= RowCount; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.WrapText = true;

                    if (i >= 3)
                        WorkSheet.Cells[GetExcelColumnName(i) + j].Style.HorizontalAlignment =
                        HorizontalAlignmentStyle.Right;

                }
            }

            for (int i = 1; i <= 7; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.FillPattern.SetSolid(Color.Chocolate);
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
            }


            //итог
            WorkSheet.Cells["A" + RowCount].Value = "Общий итог";

            var ii = 3;
            foreach (var finStatus in statusOrderSumm)
            {
                WorkSheet.Cells[GetExcelColumnName(ii) + RowCount].Value = finStatus;
                WorkSheet.Cells[GetExcelColumnName(ii) + RowCount].Style.HorizontalAlignment =
                        HorizontalAlignmentStyle.Right;

                ii++;
            }

            byte[] fileContents;
            var options = SaveOptions.XlsxDefault;

            using (var stream = new MemoryStream())
            {
                ef.Save(stream, options);
                fileContents = stream.ToArray();
            }
            return fileContents;
        }

        public byte[] AllOrderRenderReport<T>(List<OrdersPassTransportViewModel> ordersPassList, List<OrdersTruckTransportViewModel> truckOrders, RestParamsInfo Params)
        {

            ExcelFile ef = new ExcelFile();
            CultureInfo ci = new CultureInfo(Params.Language);

            ExcelWorksheet WorkSheet = ef.Worksheets.Add("Легковые заявки");
            ExcelWorksheet WorkSheet2 = ef.Worksheets.Add("Грузовые заявки");

            WorkSheet.PrintOptions.PaperType = PaperType.A4;
            WorkSheet.PrintOptions.FitWorksheetWidthToPages = 1;
            /*********** 1 лист **********/
            //ширина колонок            
            WorkSheet.Columns[0].Width = 14 * 256;
            WorkSheet.Columns[1].Width = 10 * 256;
            WorkSheet.Columns[2].Width = 20 * 256;
            WorkSheet.Columns[3].Width = 20 * 256;
            WorkSheet.Columns[4].Width = 18 * 256;
            WorkSheet.Columns[5].Width = 18 * 256;
            WorkSheet.Columns[6].Width = 15 * 256;
            WorkSheet.Columns[7].Width = 22 * 256;
            WorkSheet.Columns[8].Width = 16 * 256;//I

            WorkSheet.Columns[9].Width = 8 * 256;
            WorkSheet.Columns[10].Width = 8 * 256;
            WorkSheet.Columns[11].Width = 8 * 256;
            WorkSheet.Columns[12].Width = 8 * 256;
            WorkSheet.Columns[13].Width = 8 * 256;
            WorkSheet.Columns[14].Width = 10 * 256;//O

            WorkSheet.Columns[15].Width = 10 * 256;
            WorkSheet.Columns[16].Width = 12 * 256;
            WorkSheet.Columns[17].Width = 12 * 256;//I
            WorkSheet.Columns[18].Width = 8 * 256;
            WorkSheet.Columns[19].Width = 8 * 256;//T
            WorkSheet.Columns[20].Width = 13 * 256;

            WorkSheet.Columns[21].Width = 9 * 256;
            WorkSheet.Columns[22].Width = 11 * 256;
            //WorkSheet.Columns[23].Width = 13 * 256;


            //форматирование
            for (int i = 1; i <= 23; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + 1].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells[GetExcelColumnName(i) + 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                WorkSheet.Cells[GetExcelColumnName(i) + 1].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                WorkSheet.Cells[GetExcelColumnName(i) + 1].Style.WrapText = true;
                WorkSheet.Cells[GetExcelColumnName(i) + 1].Style.Borders.SetBorders(MultipleBorders.Bottom,
                        Color.Black, LineStyle.Medium);
                WorkSheet.Cells[GetExcelColumnName(i) + 1].Style.Borders.SetBorders(
                       MultipleBorders.Left | MultipleBorders.Right, Color.Black, LineStyle.Thin);
                WorkSheet.Cells[GetExcelColumnName(i) + 1].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                        SpreadsheetColor.FromName(ColorName.Accent1Lighter80Pct),
                        SpreadsheetColor.FromName(ColorName.Automatic));

                WorkSheet.Cells[GetExcelColumnName(i) + 1].Style.Font.Size = 10 * 20;

                for (int j = 2; j <= ordersPassList.Count() + 1; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.WrapText = true;
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Font.Size = 11 * 20;
                }
            }

            WorkSheet.Cells["A1"].Value =
                    "Номер заявки (при разбитие заявки на строки использовать нумерацию с \" / \")";
            WorkSheet.Cells["B1"].Value = "Дата указанная в заявке";
            WorkSheet.Cells["C1"].Value = "Заказчик/Плательщик (по справочнику) ";
            WorkSheet.Cells["D1"].Value = "Автор заявки";
            WorkSheet.Cells["E1"].Value = "Организация отправитель";
            WorkSheet.Cells["F1"].Value = "Организация прибытия";
            WorkSheet.Cells["G1"].Value = "Пункт отправления (по справочника)";
            WorkSheet.Cells["H1"].Value = "Пункт прибытия (по справочнику)";
            WorkSheet.Cells["I1"].Value = "Список пассажиров";
            WorkSheet.Cells["J1"].Value = "Цель поездки";

            WorkSheet.Cells["K1"].Value = "Дата отправления";
            WorkSheet.Cells["L1"].Value = "Время отправления";
            WorkSheet.Cells["M1"].Value = "Дата прибытия";
            WorkSheet.Cells["N1"].Value = "Время прибытия";

            WorkSheet.Cells["O1"].Value = "Дата обратного отправления";
            WorkSheet.Cells["P1"].Value = "Время обратного отправления";
            WorkSheet.Cells["Q1"].Value = "Дата окончания поездки";
            WorkSheet.Cells["R1"].Value = "Время окончания поездки";
            WorkSheet.Cells["S1"].Value = "Дата подачи заявки";
            WorkSheet.Cells["T1"].Value = "Время подачи заявки";

            WorkSheet.Cells["U1"].Value = "Сумма точек загрузки и выгрузки";
            WorkSheet.Cells["V1"].Value = "Длина марш., км";
            WorkSheet.Cells["W1"].Value = "Тип маршрута (по справочнику)";

            int PassRowCount = 2;
            foreach (var OrderTypeModel in ordersPassList)
            {

                WorkSheet.Cells["A" + PassRowCount].Value = OrderTypeModel.Id.ToString();
                WorkSheet.Cells["B" + PassRowCount].Value = OrderTypeModel.OrderDate;
                WorkSheet.Cells["C" + PassRowCount].Value = OrderTypeModel.PayerName;
                WorkSheet.Cells["D" + PassRowCount].Value = OrderTypeModel.CreatorPosition;
                WorkSheet.Cells["E" + PassRowCount].Value = OrderTypeModel.OrgFrom;
                WorkSheet.Cells["F" + PassRowCount].Value = OrderTypeModel.OrgTo;
                if (OrderTypeModel.TripType == 2)
                    WorkSheet.Cells["G" + PassRowCount].Value = OrderTypeModel.CountryFromName + " " +
                                                   OrderTypeModel.CityFrom + " " + OrderTypeModel.AdressFrom;
                else
                    WorkSheet.Cells["G" + PassRowCount].Value = OrderTypeModel.CityFrom + " " + OrderTypeModel.AdressFrom;

                if (OrderTypeModel.TripType == 2)
                    WorkSheet.Cells["H" + PassRowCount].Value = OrderTypeModel.CountryToName + " " +
                                                   OrderTypeModel.CityTo + " " +
                                                   OrderTypeModel.AdressTo;
                else
                    WorkSheet.Cells["H" + PassRowCount].Value = OrderTypeModel.CityTo + " " +
                                                   OrderTypeModel.AdressTo;
                WorkSheet.Cells["I" + PassRowCount].Value = OrderTypeModel.PassInfo;

                WorkSheet.Cells["J" + PassRowCount].Value = OrderTypeModel.TripDescription;

                WorkSheet.Cells["K" + PassRowCount].Value = OrderTypeModel.StartDateTimeOfTrip;
                WorkSheet.Cells["L" + PassRowCount].Value = OrderTypeModel.StartDateTimeExOfTrip;
                WorkSheet.Cells["M" + PassRowCount].Value = OrderTypeModel.FinishDateTimeOfTrip;
                WorkSheet.Cells["N" + PassRowCount].Value = OrderTypeModel.FinishDateTimeExOfTrip;

                WorkSheet.Cells["O" + PassRowCount].Value = OrderTypeModel.ReturnStartDateTimeOfTrip;
                WorkSheet.Cells["P" + PassRowCount].Value = OrderTypeModel.ReturnStartDateTimeExOfTrip;
                WorkSheet.Cells["Q" + PassRowCount].Value = OrderTypeModel.ReturnFinishDateTimeOfTrip;
                WorkSheet.Cells["R" + PassRowCount].Value = OrderTypeModel.ReturnFinishDateTimeExOfTrip;

                WorkSheet.Cells["S" + PassRowCount].Value = OrderTypeModel.CreateDatetime.ToShortDateString();
                WorkSheet.Cells["T" + PassRowCount].Value = OrderTypeModel.CreateDatetime.ToShortTimeString();


                PassRowCount++;
            }

            /*********** 2 лист **********/

            //ширина колонок            
            WorkSheet2.Columns[0].Width = 14 * 256;
            WorkSheet2.Columns[1].Width = 10 * 256;
            WorkSheet2.Columns[2].Width = 20 * 256;
            WorkSheet2.Columns[3].Width = 20 * 256;
            WorkSheet2.Columns[4].Width = 18 * 256;
            WorkSheet2.Columns[5].Width = 18 * 256;
            WorkSheet2.Columns[6].Width = 15 * 256;
            WorkSheet2.Columns[7].Width = 22 * 256;
            WorkSheet2.Columns[8].Width = 16 * 256;//I

            WorkSheet2.Columns[9].Width = 8 * 256;
            WorkSheet2.Columns[10].Width = 8 * 256;
            WorkSheet2.Columns[11].Width = 8 * 256;
            WorkSheet2.Columns[12].Width = 8 * 256;
            WorkSheet2.Columns[13].Width = 8 * 256;
            WorkSheet2.Columns[14].Width = 10 * 256;//O

            WorkSheet2.Columns[15].Width = 10 * 256;
            WorkSheet2.Columns[16].Width = 12 * 256;
            WorkSheet2.Columns[17].Width = 12 * 256;//I
            WorkSheet2.Columns[18].Width = 8 * 256;
            WorkSheet2.Columns[19].Width = 8 * 256;//T
            WorkSheet2.Columns[20].Width = 13 * 256;

            WorkSheet2.Columns[21].Width = 9 * 256;
            WorkSheet2.Columns[22].Width = 11 * 256;
            WorkSheet2.Columns[23].Width = 13 * 256;

            //форматирование
            for (int i = 1; i <= 24; i++)
            {
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.WrapText = true;
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Borders.SetBorders(MultipleBorders.Bottom,
                        Color.Black, LineStyle.Medium);
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Borders.SetBorders(
                       MultipleBorders.Left | MultipleBorders.Right, Color.Black, LineStyle.Thin);
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                        SpreadsheetColor.FromName(ColorName.Accent1Lighter80Pct),
                        SpreadsheetColor.FromName(ColorName.Automatic));

                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Font.Size = 10 * 20;

                for (int j = 2; j <= truckOrders.Count() + 1; j++)
                {
                    WorkSheet2.Cells[GetExcelColumnName(i) + j].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    WorkSheet2.Cells[GetExcelColumnName(i) + j].Style.WrapText = true;
                    WorkSheet2.Cells[GetExcelColumnName(i) + j].Style.Font.Size = 11 * 20;
                }
            }

            WorkSheet2.Cells["A1"].Value =
                    "Номер заявки (при разбитие заявки на строки использовать нумерацию с \" / \")";
            WorkSheet2.Cells["B1"].Value = "Дата указанная в заявке";
            WorkSheet2.Cells["C1"].Value = "Заказчик/Плательщик (по справочнику) ";
            WorkSheet2.Cells["D1"].Value = "Автор заявки";
            WorkSheet2.Cells["E1"].Value = "Грузоотправитель (по справочнику)";
            WorkSheet2.Cells["F1"].Value = "Грузополучатель (по справочнику)";
            WorkSheet2.Cells["G1"].Value = "Пункт отправления (по справочника)";
            WorkSheet2.Cells["H1"].Value = "Пункт прибытия (по справочнику)";
            WorkSheet2.Cells["I1"].Value = "Наименование груза";
            WorkSheet2.Cells["J1"].Value = "Вес груза";
            WorkSheet2.Cells["K1"].Value = "Тип авто/кузова";
            WorkSheet2.Cells["L1"].Value = "Вид загрузки";
            WorkSheet2.Cells["M1"].Value = "Ограничения по выгрузке";
            WorkSheet2.Cells["N1"].Value = "К-во авто к подаче";
            WorkSheet2.Cells["O1"].Value = "Дата подачи авто по заявке";
            WorkSheet2.Cells["P1"].Value = "Время подачи авто по заявке";
            WorkSheet2.Cells["Q1"].Value = "Дата доставки груза по заявке";
            WorkSheet2.Cells["R1"].Value = "Время доставки груза по заявке";
            WorkSheet2.Cells["S1"].Value = "Дата подачи заявки";
            WorkSheet2.Cells["T1"].Value = "Время подачи заявки";
            WorkSheet2.Cells["U1"].Value = "Тип груза";
            WorkSheet2.Cells["V1"].Value = "Сумма точек загрузки и выгрузки";
            WorkSheet2.Cells["W1"].Value = "Длина марш., км";
            WorkSheet2.Cells["X1"].Value = "Тип маршрута (по справочнику)";

            int RowCount = 2;
            foreach (var OrderTypeModel in truckOrders)
            {

                WorkSheet2.Cells["A" + RowCount].Value = OrderTypeModel.Id.ToString();
                WorkSheet2.Cells["B" + RowCount].Value = OrderTypeModel.OrderDate;
                WorkSheet2.Cells["C" + RowCount].Value = OrderTypeModel.PayerName;
                WorkSheet2.Cells["D" + RowCount].Value = OrderTypeModel.CreatorPosition;
                WorkSheet2.Cells["E" + RowCount].Value = OrderTypeModel.Shipper;
                WorkSheet2.Cells["F" + RowCount].Value = OrderTypeModel.Consignee;
                if (OrderTypeModel.TripType == 2)
                    WorkSheet2.Cells["G" + RowCount].Value = OrderTypeModel.ShipperCountryName + " " +
                                                   OrderTypeModel.ShipperCity + " " + OrderTypeModel.ShipperAdress;
                else
                    WorkSheet2.Cells["G" + RowCount].Value = OrderTypeModel.ShipperCity + " " + OrderTypeModel.ShipperAdress;
                if (OrderTypeModel.TripType == 2)
                    WorkSheet2.Cells["H" + RowCount].Value = OrderTypeModel.ConsigneeCountryName + " " +
                                                   OrderTypeModel.ConsigneeCity + " " +
                                                   OrderTypeModel.ConsigneeAdress;
                else
                    WorkSheet2.Cells["H" + RowCount].Value = OrderTypeModel.ConsigneeCity + " " +
                                                   OrderTypeModel.ConsigneeAdress;
                WorkSheet2.Cells["I" + RowCount].Value = OrderTypeModel.TruckDescription;
                WorkSheet2.Cells["J" + RowCount].Value = OrderTypeModel.Weight;
                WorkSheet2.Cells["K" + RowCount].Value = OrderTypeModel.VehicleTypeName;
                WorkSheet2.Cells["L" + RowCount].Value = OrderTypeModel.LoadingTypeName;
                WorkSheet2.Cells["M" + RowCount].Value = OrderTypeModel.UnloadingTypeName;
                WorkSheet2.Cells["N" + RowCount].Value = OrderTypeModel.CarNumber;
                WorkSheet2.Cells["O" + RowCount].Value = OrderTypeModel.FromShipperDate;
                WorkSheet2.Cells["P" + RowCount].Value = OrderTypeModel.FromShipperTime;
                WorkSheet2.Cells["Q" + RowCount].Value = OrderTypeModel.ToConsigneeDate;
                WorkSheet2.Cells["R" + RowCount].Value = OrderTypeModel.ToConsigneeTime;
                WorkSheet2.Cells["S" + RowCount].Value = OrderTypeModel.CreateDatetime.ToShortDateString();
                WorkSheet2.Cells["T" + RowCount].Value = OrderTypeModel.CreateDatetime.ToShortTimeString();
                WorkSheet2.Cells["U" + RowCount].Value = OrderTypeModel.TruckTypeName;

                RowCount++;
            }

            byte[] fileContents;
            var options = SaveOptions.XlsxDefault;

            using (var stream = new MemoryStream())
            {
                ef.Save(stream, options);
                fileContents = stream.ToArray();
            }
            return fileContents;
        }


        public byte[] TruckReportRenderReport<T>(RestDataInfo<T> DataOtgruzka, RestDataInfo<T> DataPoluchenie, RestParamsInfo Params, int SumOtgruzka, int SumPoluchenie)
        {
            ExcelFile ef = new ExcelFile();
            CultureInfo ci = new CultureInfo(Params.Language);
            ExcelWorksheet WorkSheet = ef.Worksheets.Add("report");
            WorkSheet.PrintOptions.Portrait = false;
            WorkSheet.PrintOptions.PaperType = PaperType.A4;
            WorkSheet.PrintOptions.FitWorksheetWidthToPages = 1;

            //шрифт 10 для всех ячеек
            WorkSheet.Cells.Style.Font.Size = 11 * 20;

            //ширина колонок            
            WorkSheet.Columns[0].Width = 25 * 256;
            WorkSheet.Columns[1].Width = 40 * 256;
            WorkSheet.Columns[2].Width = 3 * 256;

            WorkSheet.Columns[3].Width = 17 * 256;
            WorkSheet.Columns[4].Width = 17 * 256;
            WorkSheet.Columns[5].Width = 17 * 256;
            WorkSheet.Columns[6].Width = 17 * 256;
            WorkSheet.Columns[7].Width = 19 * 256;

            WorkSheet.Columns[8].Width = 27 * 256;
            WorkSheet.Columns[9].Width = 2 * 256;
            WorkSheet.Columns[10].Width = 17 * 256;
            WorkSheet.Columns[11].Width = 17 * 256;
            WorkSheet.Columns[12].Width = 2 * 256;
            WorkSheet.Columns[13].Width = 17 * 256;
            WorkSheet.Columns[14].Width = 17 * 256;
            WorkSheet.Columns[15].Width = 2 * 256;
            WorkSheet.Columns[16].Width = 17 * 256;
            WorkSheet.Columns[17].Width = 17 * 256;

            //шрифт заголовка + сделать жирным
            WorkSheet.Cells["D2"].Style.Font.Size = 14 * 20;
            WorkSheet.Cells["D2"].Style.Font.Weight = ExcelFont.BoldWeight;

            //заголовок отчета
            WorkSheet.Cells["D2"].Value = Params.MainHeader;

            WorkSheet.Cells["D3"].Value = Params.Address;

            //Дата и время создания файла            
            WorkSheet.Cells["K1"].Value = "Дата и время создания файла:" + DateTime.Now.Date.ToString("dd.MM.yyyy")
                ;
            WorkSheet.Cells["K1"].Style.Font.Size = 8 * 20;


            //шапка
            WorkSheet.Cells["A4"].Value = "Заказчик/плательщик";
            WorkSheet.Cells["B4"].Value = "Наименование груза";
            WorkSheet.Cells["D4"].Value = "План";
            WorkSheet.Cells["I4"].Value = "Примечание";
            WorkSheet.Cells["K4"].Value = "Плановая подача на загрузку/выгрузку";
            WorkSheet.Cells["N4"].Value = "Фактическая подача на загрузку/выгрузку";
            WorkSheet.Cells["Q4"].Value = "Фактическая загрузка/выгрузка";

            WorkSheet.Cells["K5"].Value = "ДАТА";
            WorkSheet.Cells["L5"].Value = "ВРЕМЯ";

            WorkSheet.Cells["N5"].Value = "ДАТА";
            WorkSheet.Cells["O5"].Value = "ВРЕМЯ";

            WorkSheet.Cells["Q5"].Value = "ДАТА";
            WorkSheet.Cells["R5"].Value = "ВРЕМЯ";

            //оформление стиля заголовков таблицы
            CellStyle tmpStyle = new CellStyle();
            tmpStyle.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            tmpStyle.VerticalAlignment = VerticalAlignmentStyle.Center;
            // tmpStyle.FillPattern.SetSolid(Color.Chocolate);
            tmpStyle.Font.Weight = ExcelFont.BoldWeight;
            tmpStyle.Font.Color = Color.Black;
            tmpStyle.Font.Size = 10 * 20;
            tmpStyle.WrapText = true;

            for (int i = 1; i <= 18; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + 4].Style = tmpStyle;
                WorkSheet.Cells[GetExcelColumnName(i) + 4].Style.Font.Color = Color.Blue;
                if ((i >= 1 && i <= 3) || (i == 9))
                    WorkSheet.Cells[GetExcelColumnName(i) + 4].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);

                if ((i == 10) || (i == 13) || (i == 16))
                    WorkSheet.Cells[GetExcelColumnName(i) + 4].Style.Borders.SetBorders(
                        MultipleBorders.Left, Color.Black, LineStyle.Thin);

                if ((i == 11) || (i == 12) || (i == 14) || (i == 15) || (i == 17) || (i == 18))
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + 4].Style.Font.Color = Color.Black;
                    WorkSheet.Cells[GetExcelColumnName(i) + 4].Style.Borders.SetBorders(
                    MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.Black, LineStyle.Thin);
                    WorkSheet.Cells[GetExcelColumnName(i) + 5].Style.Borders.SetBorders(
                    MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.Black, LineStyle.Thin);
                }

                if (i == 14)
                    WorkSheet.Cells[GetExcelColumnName(i) + 4].Style.Borders.SetBorders(
                        MultipleBorders.Left, Color.Black, LineStyle.Thin);



                if (i == 4)
                    WorkSheet.Cells[GetExcelColumnName(i) + 4].Style.Borders.SetBorders(
                   MultipleBorders.Left,
                    Color.Black, LineStyle.Thin);

                if ((i >= 4 && i <= 8))
                    WorkSheet.Cells[GetExcelColumnName(i) + 4].Style.Borders.SetBorders(
                   MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);

            }
            WorkSheet.Cells["A4"].Style.Font.Color = Color.Red;
            WorkSheet.Cells.GetSubrangeAbsolute(3, 10, 3, 11).Merged = true;
            WorkSheet.Cells.GetSubrangeAbsolute(3, 13, 3, 14).Merged = true;
            WorkSheet.Cells.GetSubrangeAbsolute(3, 16, 3, 17).Merged = true;
            WorkSheet.Cells.GetSubrangeAbsolute(3, 3, 3, 7).Merged = true;
            WorkSheet.Cells["A4"].Row.Height = 600;
            WorkSheet.Cells["D4"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            //шапка            
            WorkSheet.Cells["D5"].Value = "Экспедитор";
            WorkSheet.Cells["E5"].Value = "вид ТС";
            WorkSheet.Cells["F5"].Value = "Марка ТС";
            WorkSheet.Cells["G5"].Value = "гос. номер";
            WorkSheet.Cells["H5"].Value = "Водитель";
            tmpStyle.Font.Color = Color.Black;
            for (int i = 1; i <= 9; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + 5].Style = tmpStyle;
                WorkSheet.Cells[GetExcelColumnName(i) + 5 /*+ j*/].Style.Borders.SetBorders(
                    MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);
            }

            WorkSheet.Cells["B6"].Value = "Отгрузка";
            WorkSheet.Cells["D6"].Value = SumOtgruzka + " машин(ы)";
            tmpStyle.FillPattern.SetSolid(Color.DarkKhaki);

            for (int i = 1; i <= 9; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + 6].Style = tmpStyle;
                WorkSheet.Cells[GetExcelColumnName(i) + 6 /*+ j*/].Style.Borders.SetBorders(
                    MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);
            }
            //вывод данных
            int RowCount = 7;
            foreach (var data in DataOtgruzka.Rows)
            {
                WorkSheet.Cells["A" + RowCount].Value = GetPropValue(data, "BalanceKeeper").ToString() + " / " + GetPropValue(data, "CreatorByUserName").ToString();
                WorkSheet.Cells["B" + RowCount].Value = GetPropValue(data, "TruckDescription"); //2
                WorkSheet.Cells["D" + RowCount].Value = GetPropValue(data, "ExpeditorName"); //1
                WorkSheet.Cells["E" + RowCount].Value = GetPropValue(data, "CarCapacity"); //2
                WorkSheet.Cells["F" + RowCount].Value = GetPropValue(data, "CarModelInfo"); //3 
                WorkSheet.Cells["G" + RowCount].Value = GetPropValue(data, "CarRegNum");
                WorkSheet.Cells["H" + RowCount].Value = GetPropValue(data, "CarDriverInfo"); //4   
                WorkSheet.Cells["K" + RowCount].Value = GetPropValue(data, "PlanDate"); //6             
                WorkSheet.Cells["L" + RowCount].Value = GetPropValue(data, "PlanTime"); //6
                WorkSheet.Cells["N" + RowCount].Value = GetPropValue(data, "FactDate"); //7
                WorkSheet.Cells["O" + RowCount].Value = GetPropValue(data, "FactTime"); //7

                WorkSheet.Cells["Q" + RowCount].Value = GetPropValue(data, "DateFactConsignee"); //7
                WorkSheet.Cells["R" + RowCount].Value = GetPropValue(data, "TimeFactConsignee"); //7
                RowCount++;
            }


            for (int i = 1; i <= 18; i++)
            {
                for (int j = 7; j <= RowCount; j++)
                {
                    if ((i != 10) && (i != 13) && (i != 16))
                        WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                            MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                            Color.Black, LineStyle.Thin);
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.WrapText = true;
                }
            }
            RowCount++;
            WorkSheet.Cells["B" + RowCount].Value = "Поступление";
            WorkSheet.Cells["D" + RowCount].Value = SumPoluchenie + " машин(ы)";
            tmpStyle.FillPattern.SetSolid(Color.DarkKhaki);

            for (int i = 1; i <= 9; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style = tmpStyle;
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.Borders.SetBorders(
                    MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);
            }
            RowCount++;
            int RowCount2 = RowCount;
            foreach (var data in DataPoluchenie.Rows)
            {
                WorkSheet.Cells["A" + RowCount].Value = GetPropValue(data, "BalanceKeeper").ToString() + " / " + GetPropValue(data, "CreatorByUserName").ToString();
                WorkSheet.Cells["B" + RowCount].Value = GetPropValue(data, "TruckDescription"); //2
                WorkSheet.Cells["D" + RowCount].Value = GetPropValue(data, "ExpeditorName"); //1
                WorkSheet.Cells["E" + RowCount].Value = GetPropValue(data, "CarCapacity"); //2
                WorkSheet.Cells["F" + RowCount].Value = GetPropValue(data, "CarModelInfo"); //3 
                WorkSheet.Cells["G" + RowCount].Value = GetPropValue(data, "CarRegNum");
                WorkSheet.Cells["H" + RowCount].Value = GetPropValue(data, "CarDriverInfo"); //4   
                WorkSheet.Cells["K" + RowCount].Value = GetPropValue(data, "PlanDate"); //6             
                WorkSheet.Cells["L" + RowCount].Value = GetPropValue(data, "PlanTime"); //6
                WorkSheet.Cells["N" + RowCount].Value = GetPropValue(data, "FactDate"); //7
                WorkSheet.Cells["O" + RowCount].Value = GetPropValue(data, "FactTime"); //7

                WorkSheet.Cells["Q" + RowCount].Value = GetPropValue(data, "DateFactConsignee"); //7
                WorkSheet.Cells["R" + RowCount].Value = GetPropValue(data, "TimeFactConsignee"); //7
                RowCount++;
            }

            for (int i = 1; i <= 18; i++)
            {
                for (int j = RowCount2; j < RowCount; j++)
                {
                    if ((i != 10) && (i != 13) && (i != 16))
                        WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                            MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                            Color.Black, LineStyle.Thin);
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.WrapText = true;
                }
            }
            int RowItog = RowCount;
            int SumAvto = SumOtgruzka + SumPoluchenie;

            WorkSheet.Cells["B" + RowCount].Value = "Итого " + SumAvto + " машин";
            WorkSheet.Cells["D" + RowCount].Value = SumAvto;
            WorkSheet.Cells["E" + RowCount].Value = " машин(ы)";
            WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["E" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
            RowCount++;
            WorkSheet.Cells["B" + RowCount].Value = "Отгрузка - " + SumOtgruzka + " авто";
            WorkSheet.Cells["D" + RowCount].Value = SumOtgruzka;
            WorkSheet.Cells["E" + RowCount].Value = " машин(ы)";
            RowCount++;
            WorkSheet.Cells["B" + RowCount].Value = "Разгрузка - " + SumPoluchenie + " авто";
            WorkSheet.Cells["D" + RowCount].Value = SumPoluchenie;
            WorkSheet.Cells["E" + RowCount].Value = " машин(ы)";

            for (int i = RowItog; i <= RowCount; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(j) + i].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right,
                        Color.Black, LineStyle.Thin);
                    WorkSheet.Cells[GetExcelColumnName(j) + i].Style.WrapText = true;
                }
                WorkSheet.Cells[GetExcelColumnName(9) + i].Style.Borders.SetBorders(
                       MultipleBorders.Left | MultipleBorders.Right,
                       Color.Black, LineStyle.Thin);
                WorkSheet.Cells[GetExcelColumnName(9) + i].Style.WrapText = true;

            }
            for (int i = 1; i <= 9; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.Borders.SetBorders(
                          MultipleBorders.Bottom,
                          Color.Black, LineStyle.Thin);

            byte[] fileContents;
            var options = SaveOptions.XlsxDefault;

            using (var stream = new MemoryStream())
            {
                ef.Save(stream, options);
                fileContents = stream.ToArray();
            }
            return fileContents;
        }

        private byte[] TruckOrderRenderReport(OrderBaseViewModel OrderTypeModel, string AcceptDate,
            OrderClientsViewModel orderClientInfo, RestParamsInfo Params, string AdressFrom, string AdressTo,
            string ContractName, OrdersTruckTransportViewModel extOrderTypeModel, List<OrderUsedCarViewModel> carList, List<DataToAndFromContragent> data)
        {
            //Пример генерации QR кода
            string UrlForEncoding =
#if DEBUG

                $"http://uh218479-1.ukrdomen.com/Orders/UpdateOrder/{OrderTypeModel.Id}";
#else
                              
                                $"https://corumsource.com/Orders/UpdateOrder/{OrderTypeModel.Id}";
#endif


            ExcelFile ef = new ExcelFile();
            CultureInfo ci = new CultureInfo(Params.Language);

            ExcelWorksheet WorkSheet = ef.Worksheets.Add("Заявка грузовой");
            ExcelWorksheet WorkSheet2 = ef.Worksheets.Add("Данные для учета");

            WorkSheet.PrintOptions.PaperType = PaperType.A4;
            WorkSheet.PrintOptions.FitWorksheetWidthToPages = 1;

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(UrlForEncoding, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);


            var imgStream = new MemoryStream();
            qrCodeImage.Save(imgStream, System.Drawing.Imaging.ImageFormat.Png);

            WorkSheet.Pictures.Add(imgStream,
                PositioningMode.MoveAndSize,
                new AnchorCell(WorkSheet.Columns[6], WorkSheet.Rows[0], 10, 10, LengthUnit.Pixel),
                new AnchorCell(WorkSheet.Columns[6], WorkSheet.Rows[2], 70, 70, LengthUnit.Pixel),
                ExcelPictureFormat.Png);
            //шрифт 10 для всех ячеек
            WorkSheet.Cells.Style.Font.Size = 11 * 20;

            //ширина колонок            
            WorkSheet.Columns[0].Width = 3 * 256;
            WorkSheet.Columns[1].Width = 8 * 256;
            WorkSheet.Columns[2].Width = 25 * 256;
            WorkSheet.Columns[3].Width = 15 * 256;
            WorkSheet.Columns[4].Width = 15 * 256;
            WorkSheet.Columns[5].Width = 34 * 256;
            WorkSheet.Columns[6].Width = 27 * 256;
            WorkSheet.Columns[7].Width = 24 * 256;
            WorkSheet.Columns[8].Width = 25 * 256;
            WorkSheet.Rows[40].AutoFit();
            //WorkSheet.Rows[40].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            //WorkSheet.Rows[40].Style.VerticalAlignment = VerticalAlignmentStyle.Bottom;


            //шрифт заголовка + сделать жирным
            WorkSheet.Cells["C1"].Style.Font.Size = 14 * 20;
            WorkSheet.Cells["C1"].Style.Font.Weight = ExcelFont.BoldWeight;

            //заголовок отчета
            WorkSheet.Cells["C1"].Value = Params.MainHeader;

            //Дата и время создания файла            
            WorkSheet.Cells["E2"].Value = "Дата и время создания файла:" + DateTime.Now;
            WorkSheet.Cells["E2"].Style.Font.Size = 8 * 20;

            WorkSheet.Cells["B3"].Value = "ДАТА:";
            WorkSheet.Cells["D3"].Value = OrderTypeModel.OrderDate;
            WorkSheet.Cells["B3"].Style.Font.Size = 13 * 20;
            WorkSheet.Cells["B3"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D3"].Style.Font.Size = 14 * 20;
            for (int i = 3; i <= 6; i++)
            {
                for (int j = 3; j <= 4; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                }
            }

            WorkSheet.Cells["B4"].Value = "СОСТАВИЛ:";
            WorkSheet.Cells["D4"].Value = OrderTypeModel.CreatorPosition;
            WorkSheet.Cells["B4"].Style.Font.Size = 13 * 20;
            WorkSheet.Cells["D4"].Style.Font.Size = 12 * 20;
            WorkSheet.Cells["F4"].Style.Font.Size = 12 * 20;
            WorkSheet.Cells["F4"].Value = OrderTypeModel.CreatorContact;
            WorkSheet.Cells["B4"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D4"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["F4"].Style.Font.Weight = ExcelFont.BoldWeight;

            WorkSheet.Cells["D5"].Value = "/ФИО, Должность/";
            WorkSheet.Cells["D5"].Style.Font.Size = 8 * 20;

            WorkSheet.Cells["B6"].Value = "Заполняется заказчиком";
            WorkSheet.Cells["B6"].Style.Font.Size = 16 * 20;
            WorkSheet.Cells["B6"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B6"].Style.Font.Italic = true;
            WorkSheet.Cells["B6"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + "6"].Style.Borders.SetBorders(
                    MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Thin);
                WorkSheet.Cells[GetExcelColumnName(i) + "6"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));
            }

            WorkSheet.Cells.GetSubrangeAbsolute(5, 1, 5, 6).Merged = true;

            WorkSheet.Cells["B7"].Value = "Заказчик (Плательщик) за транспортировку: ";
            WorkSheet.Cells["B7"].Style.Font.Size = 13 * 20;
            WorkSheet.Cells["B7"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B7"].Style.Font.Italic = true;
            WorkSheet.Cells.GetSubrangeAbsolute(6, 1, 6, 6).Merged = true;
            WorkSheet.Cells["B7"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + "7"].Style.Borders.SetBorders(
                    MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                    Color.Black, LineStyle.Medium);

                WorkSheet.Cells[GetExcelColumnName(i) + "7"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));
            }

            WorkSheet.Cells["B8"].Value = "Наименование организации";
            WorkSheet.Cells.GetSubrangeAbsolute(7, 1, 7, 2).Merged = true;
            WorkSheet.Cells["B8"].Style.Font.Weight = ExcelFont.BoldWeight;

            WorkSheet.Cells["D8"].Style.Borders.SetBorders(
                  MultipleBorders.Bottom,
                  Color.Black, LineStyle.Thin);


            WorkSheet.Cells["B8"].Style.Borders.SetBorders(MultipleBorders.Left, Color.Black, LineStyle.Medium);
            WorkSheet.Cells["B8"].Style.Borders.SetBorders(MultipleBorders.Right, Color.Black, LineStyle.Thin);
            WorkSheet.Cells["G8"].Style.Borders.SetBorders(MultipleBorders.Right, Color.Black, LineStyle.Medium);

            WorkSheet.Cells["B9"].Value = "Контактное лицо/ тел.";
            WorkSheet.Cells.GetSubrangeAbsolute(8, 1, 8, 2).Merged = true;
            WorkSheet.Cells["B9"].Style.Font.Weight = ExcelFont.BoldWeight;


            WorkSheet.Cells["B9"].Style.Borders.SetBorders(MultipleBorders.Left, Color.Black, LineStyle.Medium);
            WorkSheet.Cells["G9"].Style.Borders.SetBorders(MultipleBorders.Right, Color.Black, LineStyle.Medium);
            WorkSheet.Cells["D9"].Style.Borders.SetBorders(MultipleBorders.Left, Color.Black, LineStyle.Thin);

            for (int i = 2; i <= 7; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + "9"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                    Color.Black, LineStyle.Medium);
                WorkSheet.Cells[GetExcelColumnName(i) + "9"].Style.Borders.SetBorders(MultipleBorders.Top,
                 Color.Black, LineStyle.Thin);
            }

            WorkSheet.Cells["D8"].Value = OrderTypeModel.PayerName;
            WorkSheet.Cells["D8"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(7, 3, 7, 6).Merged = true;
            WorkSheet.Cells["D8"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            //WorkSheet.Cells["D9"].Style.Borders.SetBorders(MultipleBorders.Top | MultipleBorders.Bottom, Color.Black, LineStyle.Thin);

            WorkSheet.Cells["D9"].Value = OrderTypeModel.CreatorPosition + "/" + OrderTypeModel.CreatorContact;
            WorkSheet.Cells["D9"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(8, 3, 8, 6).Merged = true;
            WorkSheet.Cells["D9"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["B11"].Value = "Информация о грузе:";
            WorkSheet.Cells["B11"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B11"].Style.Font.Italic = true;
            WorkSheet.Cells.GetSubrangeAbsolute(10, 1, 10, 6).Merged = true;
            WorkSheet.Cells["B11"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "11"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["B12"].Value = "Наименование груза";
            WorkSheet.Cells["B12"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(11, 1, 11, 2).Merged = true;

            WorkSheet.Cells["D12"].Value = extOrderTypeModel.TruckDescription;
            WorkSheet.Cells["D12"].Style.WrapText = true;
            WorkSheet.Cells.GetSubrangeAbsolute(11, 3, 11, 5).Merged = true;
            WorkSheet.Cells["D12"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["D12"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D12"].Style.Font.Italic = true;

            WorkSheet.Cells["G12"].Value = extOrderTypeModel.TruckTypeName;
            WorkSheet.Cells["G12"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["G12"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            //WorkSheet.Cells["G12"].Style.FillPattern.SetPattern(FillPatternStyle.Solid, SpreadsheetColor.FromName(ColorName.LightGreen), SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["B13"].Value = "Вес, т:";
            WorkSheet.Cells["B13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["C13"].Value = extOrderTypeModel.Weight;
            WorkSheet.Cells["C13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B13"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells["C13"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells["C13"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["D13"].Value = "Объем, м3";
            WorkSheet.Cells["D13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D13"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells["E13"].Value = extOrderTypeModel.Volume;
            WorkSheet.Cells["E13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["E13"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells["E13"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["F13"].Value = "Упаковка ";
            WorkSheet.Cells["G13"].Value = extOrderTypeModel.BoxingDescription;
            WorkSheet.Cells["F13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["F13"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells["G13"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["G13"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells["G13"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["B14"].Value = "Габариты / L x W x H / см /негабарит";
            WorkSheet.Cells["B14"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B14"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells.GetSubrangeAbsolute(13, 1, 13, 2).Merged = true;

            WorkSheet.Cells["D14"].Value = extOrderTypeModel.DimenssionL + " x " + extOrderTypeModel.DimenssionW + " x " +
                                           extOrderTypeModel.DimenssionH;
            WorkSheet.Cells["D14"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D14"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells.GetSubrangeAbsolute(13, 3, 13, 4).Merged = true;
            WorkSheet.Cells["D14"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["F14"].Value = "Количество мест";
            WorkSheet.Cells["F14"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["F14"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells["G14"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["B15"].Value = "Необходимое кол-во автомобилей *";
            WorkSheet.Cells["B15"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B15"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells.GetSubrangeAbsolute(14, 3, 14, 4).Merged = true;
            WorkSheet.Cells.GetSubrangeAbsolute(14, 1, 14, 2).Merged = true;
            WorkSheet.Cells["D15"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells["D15"].Value = OrderTypeModel.CarNumber;

            WorkSheet.Cells["B16"].Value = "Тип авто/вид загрузки/выгрузки";
            WorkSheet.Cells["B16"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B16"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells.GetSubrangeAbsolute(15, 1, 15, 2).Merged = true;

            WorkSheet.Cells["D16"].Value = extOrderTypeModel.VehicleTypeName;
            //WorkSheet.Cells["D16"].Style.FillPattern.SetPattern(FillPatternStyle.Solid, SpreadsheetColor.FromName(ColorName.LightGreen), SpreadsheetColor.FromName(ColorName.Automatic));
            WorkSheet.Cells.GetSubrangeAbsolute(15, 3, 15, 4).Merged = true;
            WorkSheet.Cells["D16"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["F16"].Value = extOrderTypeModel.LoadingTypeName;
            //WorkSheet.Cells["F16"].Style.FillPattern.SetPattern(FillPatternStyle.Solid, SpreadsheetColor.FromName(ColorName.LightGreen), SpreadsheetColor.FromName(ColorName.Automatic));
            WorkSheet.Cells["F16"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["G16"].Value = extOrderTypeModel.UnloadingTypeName;
            //WorkSheet.Cells["G16"].Style.FillPattern.SetPattern(FillPatternStyle.Solid, SpreadsheetColor.FromName(ColorName.LightGreen), SpreadsheetColor.FromName(ColorName.Automatic));
            WorkSheet.Cells["G16"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
            {
                for (int j = 12; j <= 16; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                }
            }

            WorkSheet.Cells["B11"].Style.Borders.SetBorders(
                MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top,
                Color.Black, LineStyle.Medium);
            for (int i = 12; i <= 16; i++)
            {
                WorkSheet.Cells["B" + i].Style.Borders.SetBorders(MultipleBorders.Left,
                    Color.Black, LineStyle.Medium);

                WorkSheet.Cells["G" + i].Style.Borders.SetBorders(MultipleBorders.Right,
                    Color.Black, LineStyle.Medium);
            }

            WorkSheet.Cells["B16"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);
            WorkSheet.Cells["D16"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);
            WorkSheet.Cells["F16"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);
            WorkSheet.Cells["G16"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);

            WorkSheet.Cells["B18"].Value = "Сроки подачи/доставки";
            WorkSheet.Cells["B18"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B18"].Style.Font.Italic = true;
            WorkSheet.Cells.GetSubrangeAbsolute(17, 1, 17, 6).Merged = true;
            WorkSheet.Cells["B18"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "18"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells.GetSubrangeAbsolute(18, 1, 18, 2).Merged = true;
            WorkSheet.Cells["D19"].Value = "Дата";
            WorkSheet.Cells["D19"].Style.Font.Size = 8 * 20;
            WorkSheet.Cells["D19"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D19"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells.GetSubrangeAbsolute(18, 3, 18, 5).Merged = true;

            WorkSheet.Cells["G19"].Value = "Время";
            WorkSheet.Cells["G19"].Style.Font.Size = 8 * 20;
            WorkSheet.Cells["G19"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["G19"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["B20"].Value = "Дата и время подачи автомобиля(ей) грузоотправителю *";
            WorkSheet.Cells["B20"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B20"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells.GetSubrangeAbsolute(19, 1, 19, 2).Merged = true;
            WorkSheet.Cells["B20"].Style.WrapText = true;
            WorkSheet.Cells["B20"].Row.Height = 600;

            WorkSheet.Cells["D20"].Value = extOrderTypeModel.FromShipperDate;
            WorkSheet.Cells["D20"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D20"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells.GetSubrangeAbsolute(19, 3, 19, 5).Merged = true;

            WorkSheet.Cells["G20"].Value = extOrderTypeModel.FromShipperTime;
            WorkSheet.Cells["G20"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["G20"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["B21"].Value = "Дата и время доставки груза грузополучателю *";
            //WorkSheet.Cells["B21"].Row.AutoFit();            

            WorkSheet.Cells["B21"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B21"].Style.Font.Size = 10 * 20;
            WorkSheet.Cells.GetSubrangeAbsolute(20, 1, 20, 2).Merged = true;
            WorkSheet.Cells["B21"].Style.WrapText = true;
            WorkSheet.Cells["B21"].Row.Height = 600;

            WorkSheet.Cells["D21"].Value = extOrderTypeModel.ToConsigneeDate;
            WorkSheet.Cells["D21"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["D21"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            WorkSheet.Cells.GetSubrangeAbsolute(20, 3, 20, 5).Merged = true;
            WorkSheet.Cells.GetSubrangeAbsolute(21, 3, 21, 6).Merged = true;

            WorkSheet.Cells["G21"].Value = extOrderTypeModel.ToConsigneeTime;
            WorkSheet.Cells["G21"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["G21"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["B22"].Value = "Особые условия перевозки";
            WorkSheet.Cells["B22"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(21, 1, 21, 2).Merged = true;
            WorkSheet.Cells.GetSubrangeAbsolute(22, 1, 22, 2).Merged = true;

            WorkSheet.Cells["D22"].Value = extOrderTypeModel.OrderDescription;
            WorkSheet.Cells["D22"].Style.Font.Weight = ExcelFont.BoldWeight;

            for (int i = 2; i <= 7; i++)
            {

                for (int j = 19; j <= 22; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                }
            }

            WorkSheet.Cells["B18"].Style.Borders.SetBorders(
                MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top,
                Color.Black, LineStyle.Medium);
            for (int i = 19; i <= 22; i++)
            {
                WorkSheet.Cells["B" + i].Style.Borders.SetBorders(MultipleBorders.Left,
                    Color.Black, LineStyle.Medium);

                WorkSheet.Cells["G" + i].Style.Borders.SetBorders(MultipleBorders.Right,
                    Color.Black, LineStyle.Medium);
            }
            WorkSheet.Cells["B22"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);
            WorkSheet.Cells["D22"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);

            WorkSheet.Cells["B24"].Value = "Грузоотправитель:";
            WorkSheet.Cells["B24"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B24"].Style.Font.Italic = true;
            WorkSheet.Cells.GetSubrangeAbsolute(23, 1, 23, 6).Merged = true;
            WorkSheet.Cells["B24"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "24"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["B25"].Value = "Наименование организации";
            WorkSheet.Cells["B25"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(24, 1, 24, 2).Merged = true;

            if (extOrderTypeModel.Shipper == "")
                WorkSheet.Cells["D25"].Value = extOrderTypeModel.OrganizationLoadPoints;
            else
                WorkSheet.Cells["D25"].Value = "1) " + extOrderTypeModel.Shipper + "\n" + extOrderTypeModel.OrganizationLoadPoints;
            WorkSheet.Cells["D25"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(24, 3, 24, 6).Merged = true;
            WorkSheet.Cells["D25"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            WorkSheet.Cells["D25"].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            WorkSheet.Cells["D25"].Style.Font.Italic = true;
            WorkSheet.Cells["D25"].Style.WrapText = true;
            double cntHeight = WorkSheet.Cells["D25"].Value.ToString().Length * 2 / 75;
            if (cntHeight == 0) cntHeight = 1;
            if (cntHeight < WorkSheet.Cells["D25"].Value.ToString().Count(x => x == '\n'))
                cntHeight = WorkSheet.Cells["D25"].Value.ToString().Count(x => x == '\n') * 1.3;
            WorkSheet.Cells["D25"].Row.Height = (int)(WorkSheet.Cells["D25"].Row.Height * cntHeight);

            WorkSheet.Cells["B26"].Value = "Адрес загрузки";
            WorkSheet.Cells["B26"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(25, 1, 25, 2).Merged = true;

            string ShipperAddress = "";
            if (extOrderTypeModel.TripType == 2)
                ShipperAddress = extOrderTypeModel.ShipperCountryName + " " +
                extOrderTypeModel.ShipperCity + " " + extOrderTypeModel.ShipperAdress;
            else
                ShipperAddress = extOrderTypeModel.ShipperCity + " " + extOrderTypeModel.ShipperAdress;

            //if  (ShipperAddress == "")
            //    WorkSheet.Cells["D26"].Value = extOrderTypeModel.AddressLoadPoints;
            //else
            WorkSheet.Cells["D26"].Value = "1) " + ShipperAddress + "\n" + extOrderTypeModel.AddressLoadPoints;
            WorkSheet.Cells["D26"].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            WorkSheet.Cells["D26"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(25, 3, 25, 6).Merged = true;
            WorkSheet.Cells["D26"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            WorkSheet.Cells["D26"].Style.Font.Italic = true;
            WorkSheet.Cells["D26"].Style.WrapText = true;
            WorkSheet.Cells["B27"].Value = "Контактное лицо / тел.";
            WorkSheet.Cells["B27"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(26, 1, 26, 2).Merged = true;
            cntHeight = WorkSheet.Cells["D26"].Value.ToString().Length * 2 / 75;
            if (cntHeight == 0) cntHeight = 1;
            if (cntHeight < WorkSheet.Cells["D26"].Value.ToString().Count(x => x == '\n'))
                cntHeight = WorkSheet.Cells["D26"].Value.ToString().Count(x => x == '\n') * 1.3;
            WorkSheet.Cells["D26"].Row.Height = (int)(WorkSheet.Cells["D26"].Row.Height * cntHeight);

            string ShipperContact = "";
            ShipperContact = extOrderTypeModel.ShipperContactPerson + "/" +
                                           extOrderTypeModel.ShipperContactPersonPhone;

            //if (ShipperContact == "")
            //    WorkSheet.Cells["D27"].Value = extOrderTypeModel.ContactsLoadPoints;
            //else
            WorkSheet.Cells["D27"].Value = "1) " + ShipperContact + "\n" + extOrderTypeModel.ContactsLoadPoints;
            WorkSheet.Cells["D27"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(26, 3, 26, 6).Merged = true;
            WorkSheet.Cells["D27"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            WorkSheet.Cells["D27"].Style.Font.Italic = true;
            WorkSheet.Cells["D27"].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            cntHeight = WorkSheet.Cells["D27"].Value.ToString().Length * 2 / 75;
            if (cntHeight == 0) cntHeight = 1;
            if (cntHeight < WorkSheet.Cells["D27"].Value.ToString().Count(x => x == '\n'))
                cntHeight = WorkSheet.Cells["D27"].Value.ToString().Count(x => x == '\n') * 1.3;
            WorkSheet.Cells["D27"].Row.Height = (int)(WorkSheet.Cells["D27"].Row.Height * cntHeight);


            for (int i = 2; i <= 7; i++)
            {
                for (int j = 25; j <= 27; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                }
            }

            WorkSheet.Cells["B24"].Style.Borders.SetBorders(
                MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top,
                Color.Black, LineStyle.Medium);

            for (int i = 25; i <= 27; i++)
            {
                WorkSheet.Cells["B" + i].Style.Borders.SetBorders(MultipleBorders.Left,
                    Color.Black, LineStyle.Medium);

                WorkSheet.Cells["G" + i].Style.Borders.SetBorders(MultipleBorders.Right,
                    Color.Black, LineStyle.Medium);
            }
            WorkSheet.Cells["B27"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);
            WorkSheet.Cells["D27"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);


            WorkSheet.Cells["B29"].Value = "Грузополучатель:";
            WorkSheet.Cells["B29"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B29"].Style.Font.Italic = true;
            WorkSheet.Cells.GetSubrangeAbsolute(28, 1, 28, 6).Merged = true;
            WorkSheet.Cells["B29"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
                WorkSheet.Cells[GetExcelColumnName(i) + "29"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));

            WorkSheet.Cells["B30"].Value = "Наименование организации";
            WorkSheet.Cells["B30"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(29, 1, 29, 2).Merged = true;

            int countUnLoadPoints = extOrderTypeModel.CountUnLoadPoints + 1;
            if (extOrderTypeModel.Consignee == "")
                WorkSheet.Cells["D30"].Value = extOrderTypeModel.OrganizationUnLoadPoints;
            else
                WorkSheet.Cells["D30"].Value = extOrderTypeModel.OrganizationUnLoadPoints + countUnLoadPoints.ToString() + ") " + extOrderTypeModel.Consignee;
            WorkSheet.Cells["D30"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(29, 3, 29, 6).Merged = true;
            WorkSheet.Cells["D30"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            WorkSheet.Cells["D30"].Style.VerticalAlignment = VerticalAlignmentStyle.Top;

            WorkSheet.Cells["D30"].Style.Font.Italic = true;
            WorkSheet.Cells["D30"].Style.WrapText = true;
            cntHeight = WorkSheet.Cells["D30"].Value.ToString().Length * 2 / 75;
            if (cntHeight == 0) cntHeight = 1;
            if (cntHeight < WorkSheet.Cells["D30"].Value.ToString().Count(x => x == '\n'))
                cntHeight = WorkSheet.Cells["D30"].Value.ToString().Count(x => x == '\n') * 1.3;
            WorkSheet.Cells["D30"].Row.Height = (int)(WorkSheet.Cells["D30"].Row.Height * cntHeight);

            WorkSheet.Cells["B31"].Value = "Адрес выгрузки";
            WorkSheet.Cells["B31"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(30, 1, 30, 2).Merged = true;
            WorkSheet.Cells["B31"].Style.WrapText = true;

            string ConsigneeAddress = "";
            if (extOrderTypeModel.TripType == 2)
                ConsigneeAddress = extOrderTypeModel.ConsigneeCountryName + " " +
                                   extOrderTypeModel.ConsigneeCity + " " + extOrderTypeModel.ConsigneeAdress;
            else
                ConsigneeAddress = extOrderTypeModel.ConsigneeCity + " " + extOrderTypeModel.ConsigneeAdress;
            if (ConsigneeAddress == "")
                WorkSheet.Cells["D31"].Value = extOrderTypeModel.AddressUnLoadPoints;
            else
                WorkSheet.Cells["D31"].Value = extOrderTypeModel.AddressUnLoadPoints + countUnLoadPoints.ToString() + ") " + ConsigneeAddress;

            WorkSheet.Cells["D31"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(30, 3, 30, 6).Merged = true;
            WorkSheet.Cells["D31"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            WorkSheet.Cells["D31"].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            WorkSheet.Cells["D31"].Style.Font.Italic = true;

            //WorkSheet.Cells["D31"].Style.WrapText = true;
            cntHeight = WorkSheet.Cells["D31"].Value.ToString().Length * 2 / 75;
            if (cntHeight == 0) cntHeight = 1;
            if (cntHeight < WorkSheet.Cells["D31"].Value.ToString().Count(x => x == '\n'))
                cntHeight = WorkSheet.Cells["D31"].Value.ToString().Count(x => x == '\n') * 1.3;
            WorkSheet.Cells["D31"].Row.Height = (int)(WorkSheet.Cells["D31"].Row.Height * cntHeight);
            //extOrderTypeModel.AddressUnLoadPoints.Count(x => x == '\n');
            // WorkSheet.Cells["D31"].Row.Height = WorkSheet.Cells["D31"].Row.Height * cntHeight;

            WorkSheet.Cells["B32"].Value = "Контактное лицо / тел.";
            WorkSheet.Cells["B32"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(31, 1, 31, 2).Merged = true;
            string ConsigneeContact = "";
            ConsigneeContact = extOrderTypeModel.ConsigneeContactPerson + "/" +
                               extOrderTypeModel.ConsigneeContactPersonPhone;
            //if (ConsigneeContact == "")
            //WorkSheet.Cells["D32"].Value = extOrderTypeModel.ContactsUnLoadPoints;
            //else
            WorkSheet.Cells["D32"].Value = extOrderTypeModel.ContactsUnLoadPoints + countUnLoadPoints.ToString() + ") " + ConsigneeContact;
            WorkSheet.Cells["D32"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells.GetSubrangeAbsolute(31, 3, 31, 6).Merged = true;
            WorkSheet.Cells["D32"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            WorkSheet.Cells["D32"].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            WorkSheet.Cells["D32"].Style.Font.Italic = true;

            cntHeight = WorkSheet.Cells["D32"].Value.ToString().Length * 2 / 75;
            if (cntHeight == 0) cntHeight = 1;
            if (cntHeight < WorkSheet.Cells["D32"].Value.ToString().Count(x => x == '\n'))
                cntHeight = WorkSheet.Cells["D32"].Value.ToString().Count(x => x == '\n') * 1.3;
            WorkSheet.Cells["D32"].Row.Height = (int)(WorkSheet.Cells["D32"].Row.Height * cntHeight);

            for (int i = 2; i <= 7; i++)
            {
                for (int j = 30; j <= 32; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                }
            }

            WorkSheet.Cells["B29"].Style.Borders.SetBorders(
                MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top,
                Color.Black, LineStyle.Medium);

            for (int i = 30; i <= 32; i++)
            {
                WorkSheet.Cells["B" + i].Style.Borders.SetBorders(MultipleBorders.Left,
                    Color.Black, LineStyle.Medium);

                WorkSheet.Cells["G" + i].Style.Borders.SetBorders(MultipleBorders.Right,
                    Color.Black, LineStyle.Medium);
            }
            WorkSheet.Cells["B32"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);
            WorkSheet.Cells["D32"].Style.Borders.SetBorders(MultipleBorders.Bottom,
                Color.Black, LineStyle.Medium);

            //if (carList.Count > 0)
            //{
            WorkSheet.Cells["B34"].Value = "ЗАПОЛНЯЕТСЯ ЭКСПЕДИТОРОМ-ПЕРЕВОЗЧИКОМ:";
            WorkSheet.Cells["B34"].Style.Font.Weight = ExcelFont.BoldWeight;
            WorkSheet.Cells["B34"].Style.Font.Italic = true;
            WorkSheet.Cells["B34"].Style.Font.Size = 16 * 20;
            WorkSheet.Cells.GetSubrangeAbsolute(33, 1, 33, 6).Merged = true;
            WorkSheet.Cells["B34"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            for (int i = 2; i <= 7; i++)
            {
                WorkSheet.Cells[GetExcelColumnName(i) + "34"].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                    SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                    SpreadsheetColor.FromName(ColorName.Automatic));
            }

            WorkSheet.Cells["B34"].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top,
                      Color.Black, LineStyle.Medium);

            //}
            //добавим пустую запись, чтобы даже если нет выбранного перевозчика/экспедитора выводить пустую часть блока "ЗАПОЛНЯЕТСЯ ЭКСПЕДИТОРОМ"
            if (carList.Count == 0)
            {
                //OrderUsedCarViewModel car = new OrderUsedCarViewModel();
                carList.Add(new OrderUsedCarViewModel());

            }

            int RowCount = 35;
            int RowCountStart = 35;
            int count = 0;
            foreach (var car in carList)
            {
                bool flagData = false; bool flagRegMess = false; bool flagFrom = false;
                if (data.Count != 0 && data[count] != null)
                {
                    flagData = true;
                }
                if (flagData && data[count].regmesstocontrag != null)
                {
                    flagRegMess = true;
                }
                if (flagData && data[count].formFromContr != null)
                {
                    flagFrom = true;
                }

                RowCountStart = RowCount;

                WorkSheet.Cells["B" + RowCount].Value = "Экспедитор:  " + ((flagData && flagRegMess) ? (data[count].regmesstocontrag.contragentName != "") ? data[count].regmesstocontrag.contragentName : car.ExpeditorName : car.ExpeditorName) + " согласно договора " + car.ContractInfo;
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["B" + RowCount].Style.Font.Italic = true;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 6).Merged = true;

                for (int i = 2; i <= 7; i++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.FillPattern.SetPattern(
                        FillPatternStyle.Solid, SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                        SpreadsheetColor.FromName(ColorName.Automatic));
                }

                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = "Маршрут движения";
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 2).Merged = true;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 3, RowCount - 1, 6).Merged = true;
                WorkSheet.Cells["D" + RowCount].Value = ((flagData && flagRegMess) ? (data[count].regmesstocontrag.routeShort != "") ? data[count].regmesstocontrag.routeShort : OrderTypeModel.ShortName : OrderTypeModel.ShortName);
                WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["D" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = "Расстояние, км";
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 2).Merged = true;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 3, RowCount - 1, 6).Merged = true;
                if (Convert.ToDecimal(extOrderTypeModel.TotalDistanceLenght) == 0)
                    WorkSheet.Cells["D" + RowCount].Value = ((flagData && flagFrom) ? (data[count].formFromContr.distance != null) ? data[count].formFromContr.distance : null : null);
                else
                    WorkSheet.Cells["D" + RowCount].Value = ((flagData && flagFrom) ? (data[count].formFromContr.distance != null) ? data[count].formFromContr.distance : Convert.ToDouble(extOrderTypeModel.TotalDistanceLenght) : Convert.ToDouble(extOrderTypeModel.TotalDistanceLenght));
                WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["D" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = "Стоимость перевозки, грн без НДС";
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 2).Merged = true;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 3, RowCount - 1, 6).Merged = true;
                if (extOrderTypeModel.TotalCost == "")
                    WorkSheet.Cells["D" + RowCount].Value = ((flagData && flagRegMess) ? (data[count].regmesstocontrag.cost != null) ? data[count].regmesstocontrag.cost : null : null);
                else
                    WorkSheet.Cells["D" + RowCount].Value = ((flagData && flagRegMess) ? (data[count].regmesstocontrag.cost != null) ? data[count].regmesstocontrag.cost : Convert.ToDouble(extOrderTypeModel.TotalCost) : Convert.ToDouble(extOrderTypeModel.TotalCost));
                WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["D" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;


                for (int i = 2; i <= 7; i++)
                {
                    for (int j = RowCountStart; j <= RowCount; j++)
                    {
                        WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                            Color.Black, LineStyle.Thin);
                    }
                }

                WorkSheet.Cells["B" + (RowCountStart - 1)].Style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top,
                            Color.Black, LineStyle.Medium);

                for (int i = RowCountStart; i <= RowCount; i++)
                {
                    WorkSheet.Cells["B" + i].Style.Borders.SetBorders(MultipleBorders.Left,
                        Color.Black, LineStyle.Medium);

                    WorkSheet.Cells["G" + i].Style.Borders.SetBorders(MultipleBorders.Right,
                        Color.Black, LineStyle.Medium);
                }


                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = "Информация об автомобиле и водителе*";

                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["B" + RowCount].Style.Font.Italic = true;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 6).Merged = true;
                WorkSheet.Cells["B" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                for (int i = 2; i <= 7; i++)
                    WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.FillPattern.SetPattern(
                        FillPatternStyle.Solid, SpreadsheetColor.FromName(ColorName.Background1Darker15Pct),
                        SpreadsheetColor.FromName(ColorName.Automatic));

                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = "Наименование Перевозчика";
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["B" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 2).Merged = true;

                WorkSheet.Cells["D" + RowCount].Value = "Марка, модель, тип ТС";
                WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["D" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 3, RowCount - 1, 4).Merged = true;
                WorkSheet.Cells["D" + RowCount].Row.Height = 600;
                WorkSheet.Cells["D" + RowCount].Style.WrapText = true;

                WorkSheet.Cells["F" + RowCount].Value = "Рег.номер ТС";
                WorkSheet.Cells["F" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["F" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                WorkSheet.Cells["G" + RowCount].Value = "Грузоподъемность, тн";
                WorkSheet.Cells["G" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["G" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = car.CarrierInfo;
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 2).Merged = true;

                WorkSheet.Cells["D" + RowCount].Value = car.CarModelInfo;
                WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 3, RowCount - 1, 4).Merged = true;
                WorkSheet.Cells["D" + RowCount].Row.Height = 600;

                WorkSheet.Cells["F" + RowCount].Value = car.CarRegNum;
                WorkSheet.Cells["F" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;

                WorkSheet.Cells["G" + RowCount].Value = car.CarCapacity;
                WorkSheet.Cells["G" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;
                WorkSheet.Cells["G" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                for (int i = 2; i <= 7; i++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + "41"].Style.Borders.SetBorders(
                        MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                }


                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = "ЕДРПОУ или ИНН/№ паспорта";
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["B" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 2).Merged = true;

                WorkSheet.Cells["D" + RowCount].Value = "ФИО водителя, тел., № вод.удостоверения";
                WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["D" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 3, RowCount - 1, 4).Merged = true;
                WorkSheet.Cells["D" + RowCount].Row.Height = 600;
                WorkSheet.Cells["D" + RowCount].Style.WrapText = true;

                WorkSheet.Cells["F" + RowCount].Value = "Габариты ТС(ДхШхВ), мм*";
                WorkSheet.Cells["F" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["F" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                WorkSheet.Cells["G" + RowCount].Value = "";
                WorkSheet.Cells["G" + RowCount].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet.Cells["G" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                RowCount++;
                WorkSheet.Cells["B" + RowCount].Value = car.seriesPassportNumber;
                WorkSheet.Cells["B" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 2).Merged = true;

                WorkSheet.Cells["D" + RowCount].Value = car.CarDriverInfo + "\n" + car.DriverContactInfo + "\n" +
                                                        car.DriverCardInfo;
                WorkSheet.Cells["D" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;
                WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 3, RowCount - 1, 4).Merged = true;
                WorkSheet.Cells["D" + RowCount].Row.Height = 600;

                WorkSheet.Cells["F" + RowCount].Value = car.transportDimensions;
                WorkSheet.Cells["F" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;

                WorkSheet.Cells["G" + RowCount].Value = "";
                WorkSheet.Cells["G" + RowCount].Style.Font.Weight = ExcelFont.NormalWeight;
                WorkSheet.Cells["G" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;


                RowCount++;
                count++;
                //}
            }

            if (carList.Count > 0) RowCount--;

            for (int i = 2; i <= 7; i++)
            {
                for (int j = 39; j <= RowCount; j++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + j].Style.Borders.SetBorders(
                        MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom,
                        Color.Black, LineStyle.Thin);
                }
            }


            for (int i = 39; i <= RowCount; i++)
            {
                WorkSheet.Cells["B" + i].Style.Borders.SetBorders(MultipleBorders.Left,
                    Color.Black, LineStyle.Medium);

                WorkSheet.Cells["G" + i].Style.Borders.SetBorders(MultipleBorders.Right,
                    Color.Black, LineStyle.Medium);
            }

            if (carList.Count > 0)
            {
                for (int i = 2; i <= 7; i++)
                {
                    WorkSheet.Cells[GetExcelColumnName(i) + RowCount].Style.Borders.SetBorders(MultipleBorders.Bottom,
                        Color.Black, LineStyle.Medium);
                }
            }

            if (carList.Count <= 0) RowCount = 32;
            RowCount = RowCount + 3;
            WorkSheet.Cells["B" + RowCount].Value = "ЗАКАЗЧИК ___________________";
            // WorkSheet.Cells["C" + RowCount].Value = " И.О.Руководителя КР и СО";
            WorkSheet.Cells.GetSubrangeAbsolute(RowCount - 1, 1, RowCount - 1, 3).Merged = true;
            WorkSheet.Cells["C" + RowCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            WorkSheet.Cells["E" + RowCount].Value = "                          Экспедитор_____________________";

            RowCount = RowCount + 2;

            WorkSheet.Cells["C" + RowCount].Value = "ФИО";
            WorkSheet.Cells["F" + RowCount].Value = "ФИО";

            WorkSheet.Cells["B" + RowCount].Style.Borders.SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Thin);
            WorkSheet.Cells["C" + RowCount].Style.Borders.SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Thin);

            WorkSheet.Cells["E" + RowCount].Style.Borders.SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Thin);
            WorkSheet.Cells["F" + RowCount].Style.Borders.SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Thin);

            /*********** 2 лист **********/

            //ширина колонок            
            WorkSheet2.Columns[0].Width = 14 * 256;
            WorkSheet2.Columns[1].Width = 10 * 256;
            WorkSheet2.Columns[2].Width = 20 * 256;
            WorkSheet2.Columns[3].Width = 20 * 256;
            WorkSheet2.Columns[4].Width = 18 * 256;
            WorkSheet2.Columns[5].Width = 18 * 256;
            WorkSheet2.Columns[6].Width = 15 * 256;
            WorkSheet2.Columns[7].Width = 22 * 256;
            WorkSheet2.Columns[8].Width = 16 * 256;//I

            WorkSheet2.Columns[9].Width = 8 * 256;
            WorkSheet2.Columns[10].Width = 8 * 256;
            WorkSheet2.Columns[11].Width = 8 * 256;
            WorkSheet2.Columns[12].Width = 8 * 256;
            WorkSheet2.Columns[13].Width = 8 * 256;
            WorkSheet2.Columns[14].Width = 10 * 256;//O

            WorkSheet2.Columns[15].Width = 10 * 256;
            WorkSheet2.Columns[16].Width = 12 * 256;
            WorkSheet2.Columns[17].Width = 12 * 256;//I
            WorkSheet2.Columns[18].Width = 8 * 256;
            WorkSheet2.Columns[19].Width = 8 * 256;//T
            WorkSheet2.Columns[20].Width = 13 * 256;

            WorkSheet2.Columns[21].Width = 9 * 256;
            WorkSheet2.Columns[22].Width = 11 * 256;
            WorkSheet2.Columns[23].Width = 13 * 256;

            //форматирование
            for (int i = 1; i <= 24; i++)
            {
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Font.Weight = ExcelFont.BoldWeight;
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.WrapText = true;
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Borders.SetBorders(MultipleBorders.Bottom,
                        Color.Black, LineStyle.Medium);
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Borders.SetBorders(
                       MultipleBorders.Left | MultipleBorders.Right, Color.Black, LineStyle.Thin);
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.FillPattern.SetPattern(FillPatternStyle.Solid,
                        SpreadsheetColor.FromName(ColorName.Accent1Lighter80Pct),
                        SpreadsheetColor.FromName(ColorName.Automatic));

                WorkSheet2.Cells[GetExcelColumnName(i) + 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                WorkSheet2.Cells[GetExcelColumnName(i) + 2].Style.WrapText = true;
                WorkSheet2.Cells[GetExcelColumnName(i) + 1].Style.Font.Size = 10 * 20;
                WorkSheet2.Cells[GetExcelColumnName(i) + 2].Style.Font.Size = 11 * 20;
            }

            WorkSheet2.Cells["A1"].Value = "Номер заявки (при разбитие заявки на строки использовать нумерацию с \" / \")";
            WorkSheet2.Cells["A2"].Value = OrderTypeModel.Id.ToString();
            WorkSheet2.Cells["B1"].Value = "Дата указанная в заявке";
            WorkSheet2.Cells["B2"].Value = OrderTypeModel.OrderDate;
            WorkSheet2.Cells["C1"].Value = "Заказчик/Плательщик (по справочнику) ";
            WorkSheet2.Cells["C2"].Value = OrderTypeModel.PayerName;
            WorkSheet2.Cells["D1"].Value = "Автор заявки";
            WorkSheet2.Cells["D2"].Value = OrderTypeModel.CreatorPosition;
            WorkSheet2.Cells["E1"].Value = "Грузоотправитель (по справочнику)";
            WorkSheet2.Cells["E2"].Value = extOrderTypeModel.Shipper;

            WorkSheet2.Cells["F1"].Value = "Грузополучатель (по справочнику)";
            WorkSheet2.Cells["F2"].Value = extOrderTypeModel.Consignee;
            WorkSheet2.Cells["G1"].Value = "Пункт отправления (по справочника)";

            if (extOrderTypeModel.TripType == 2)
                WorkSheet2.Cells["G2"].Value = extOrderTypeModel.ShipperCountryName + " " +
                                               extOrderTypeModel.ShipperCity + " " + extOrderTypeModel.ShipperAdress;
            else
                WorkSheet2.Cells["G2"].Value = extOrderTypeModel.ShipperCity + " " + extOrderTypeModel.ShipperAdress;

            WorkSheet2.Cells["H1"].Value = "Пункт прибытия (по справочнику)";
            if (extOrderTypeModel.TripType == 2)
                WorkSheet2.Cells["H2"].Value = extOrderTypeModel.ConsigneeCountryName + " " +
                                               extOrderTypeModel.ConsigneeCity + " " + extOrderTypeModel.ConsigneeAdress;
            else
                WorkSheet2.Cells["H2"].Value = extOrderTypeModel.ConsigneeCity + " " + extOrderTypeModel.ConsigneeAdress;

            WorkSheet2.Cells["I1"].Value = "Наименование груза";
            WorkSheet2.Cells["I2"].Value = extOrderTypeModel.TruckDescription;

            WorkSheet2.Cells["J1"].Value = "Вес груза";
            WorkSheet2.Cells["J2"].Value = extOrderTypeModel.Weight;
            WorkSheet2.Cells["K1"].Value = "Тип авто/кузова";
            WorkSheet2.Cells["K2"].Value = extOrderTypeModel.VehicleTypeName;
            WorkSheet2.Cells["L1"].Value = "Вид загрузки";
            WorkSheet2.Cells["L2"].Value = extOrderTypeModel.LoadingTypeName;

            WorkSheet2.Cells["M1"].Value = "Ограничения по выгрузке";
            WorkSheet2.Cells["M2"].Value = extOrderTypeModel.UnloadingTypeName;
            WorkSheet2.Cells["N1"].Value = "К-во авто к подаче";
            WorkSheet2.Cells["N2"].Value = extOrderTypeModel.CarNumber;
            WorkSheet2.Cells["O1"].Value = "Дата подачи авто по заявке";
            WorkSheet2.Cells["O2"].Value = extOrderTypeModel.FromShipperDate;

            WorkSheet2.Cells["P1"].Value = "Время подачи авто по заявке";
            WorkSheet2.Cells["P2"].Value = extOrderTypeModel.FromShipperTime;

            WorkSheet2.Cells["Q1"].Value = "Дата доставки груза по заявке";
            WorkSheet2.Cells["Q2"].Value = extOrderTypeModel.ToConsigneeDate;

            WorkSheet2.Cells["R1"].Value = "Время доставки груза по заявке";
            WorkSheet2.Cells["R2"].Value = extOrderTypeModel.ToConsigneeTime;

            WorkSheet2.Cells["S1"].Value = "Дата подачи заявки";
            WorkSheet2.Cells["S2"].Value = OrderTypeModel.CreateDatetime.ToShortDateString();

            WorkSheet2.Cells["T1"].Value = "Время подачи заявки";
            WorkSheet2.Cells["T2"].Value = OrderTypeModel.CreateDatetime.ToShortTimeString();

            WorkSheet2.Cells["U1"].Value = "Тип груза";
            WorkSheet2.Cells["U2"].Value = extOrderTypeModel.TruckTypeName;

            WorkSheet2.Cells["V1"].Value = "Сумма точек загрузки и выгрузки";
            WorkSheet2.Cells["V2"].Value = extOrderTypeModel.CountLoadAndUnLoadPoints;

            WorkSheet2.Cells["W1"].Value = "Длина марш., км";

            WorkSheet2.Cells["X1"].Value = "Тип маршрута (по справочнику)";


            byte[] fileContents;
            var options = SaveOptions.XlsxDefault;

            using (var stream = new MemoryStream())
            {
                ef.Save(stream, options);
                fileContents = stream.ToArray();
            }
            return fileContents;
        }
    }
}