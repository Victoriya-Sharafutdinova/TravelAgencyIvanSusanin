using System;
using System.Collections.Generic;
using TravelAgencyIvanSusaninDAL.BindingModel;
using TravelAgencyIvanSusaninDAL.Interfaces;
using TravelAgencyIvanSusaninDAL.ViewModel;
using TravelAgencyIvanSusaninModel;
using System.Linq;
using System.IO;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace TravelAgencyIvanSusaninImplementDataBase.Implementations
{
    public class RequestServiceDB : IRequestService
    {
        private AbstractDbContext context;

        public RequestServiceDB(AbstractDbContext context)
        {
            this.context = context;
        }

        public void CreateRequest(RequestBindingModel model, bool type)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Request element = new Request
                    {
                        DateCreate = model.DateCreate
                    };
                    context.Requests.Add(element);
                    context.SaveChanges();

                    var groupComponents = model.RequestReservations
                                                .GroupBy(rec => rec.ReservationId)
                                                .Select(rec => new
                                                {
                                                    ReservationId = rec.Key,
                                                    NumberReservation = rec.Sum(r => r.NumberReservation)
                                                });
                    foreach (var groupComponent in groupComponents)
                    {
                        context.ReservationRequests.Add(new ReservationRequest
                        {
                            
                            RequestId = element.Id,
                            ReservationId = groupComponent.ReservationId,
                            NumberReservation = groupComponent.NumberReservation
                        });
                        context.SaveChanges();

                        var reservation = context.Reservations.FirstOrDefault(rec => rec.Id == groupComponent.ReservationId);
                        reservation.Number += groupComponent.NumberReservation;
                        context.SaveChanges();
                    }

                    string typeMessage = "";
                    string fName = "";
                    if (type)
                    {
                        RequestWord(model);
                        typeMessage = "word";
                        fName = "C:\\Users\\Public\\Documents\\file.doc";
                    }
                    else
                    {
                        RequestExel(model);
                        typeMessage = "xls";
                        fName = "C:\\Users\\Public\\Documents\\file.xls";
                    }
                    Mail.SendEmail( null, "Заявка на брони", "Заявка на брони в формате " + typeMessage, fName);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private void RequestWord(RequestBindingModel model)
        {
            Console.WriteLine();
            if (File.Exists("C:\\Users\\Public\\Documents\\file.doc"))
            {
                File.Delete("C:\\Users\\Public\\Documents\\file.doc");
            }
            var winword = new Microsoft.Office.Interop.Word.Application();
            try
            {
                object missing = System.Reflection.Missing.Value;
                //создаем документ
                Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                //получаем ссылку на параграф
                var paragraph = document.Paragraphs.Add(missing);
                var range = paragraph.Range;
                //задаем текст
                range.Text = "Заявка на брони";
                //задаем настройки шрифта
                var font = range.Font;
                font.Size = 16;
                font.Name = "Times New Roman";
                font.Bold = 1;
                //задаем настройки абзаца
                var paragraphFormat = range.ParagraphFormat;
                paragraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                paragraphFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
                paragraphFormat.SpaceAfter = 10;
                paragraphFormat.SpaceBefore = 0;
                //добавляем абзац в документ
                range.InsertParagraphAfter();

                var requestReservations = model.RequestReservations;
                var reservations = context.Reservations.Select(rec => new ReservationViewModel
                {
                    Id = rec.Id,
                    Name = rec.Name,
                    Description = rec.Description,
                    Number = rec.Number
                }).ToList();
                //создаем таблицу
                var paragraphTable = document.Paragraphs.Add(Type.Missing);
                var rangeTable = paragraphTable.Range;
                var table = document.Tables.Add(rangeTable, requestReservations.Count, 2, ref missing, ref missing);
                font = table.Range.Font;
                font.Size = 14;
                font.Name = "Times New Roman";
                var paragraphTableFormat = table.Range.ParagraphFormat;
                paragraphTableFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
                paragraphTableFormat.SpaceAfter = 0;
                paragraphTableFormat.SpaceBefore = 0;
                for (int i = 0; i < requestReservations.Count; ++i)
                {
                    table.Cell(i + 1, 1).Range.Text = reservations.FirstOrDefault(rec => rec.Id == requestReservations[i].ReservationId).Name;
                    table.Cell(i + 1, 2).Range.Text = requestReservations[i].NumberReservation.ToString();
                }
                //задаем границы таблицы
                table.Borders.InsideLineStyle = WdLineStyle.wdLineStyleInset;
                table.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
                paragraph = document.Paragraphs.Add(missing);
                range = paragraph.Range;
                range.Text = "Дата: " + model.DateCreate.ToLongDateString();
                font = range.Font;
                font.Size = 12;
                font.Name = "Times New Roman";
                paragraphFormat = range.ParagraphFormat;
                paragraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                paragraphFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
                paragraphFormat.SpaceAfter = 10;
                paragraphFormat.SpaceBefore = 10;
                range.InsertParagraphAfter();
                //сохраняем
                object fileFormat = WdSaveFormat.wdFormatXMLDocument;
                document.SaveAs("C:\\Users\\Public\\Documents\\file.doc", ref fileFormat, ref missing,
                ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing,
                ref missing);
                document.Close(ref missing, ref missing, ref missing);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                winword.Quit();
            }
        }

        private void RequestExel (RequestBindingModel model)
        {
            var excel = new Microsoft.Office.Interop.Excel.Application();
            try
            {
                //или создаем excel-файл, или открываем существующий
                if (File.Exists("C:\\Users\\Public\\Documents\\file.xls"))
                {
                    excel.Workbooks.Open("C:\\Users\\Public\\Documents\\file.xls", Type.Missing, Type.Missing,
                   Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, 
                   Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }
                else
                {
                    excel.SheetsInNewWorkbook = 1;
                    excel.Workbooks.Add(Type.Missing);
                    excel.Workbooks[1].SaveAs("C:\\Users\\Public\\Documents\\file.xls", XlFileFormat.xlExcel8,
                    Type.Missing, Type.Missing, false, false, XlSaveAsAccessMode.xlNoChange, Type.Missing, 
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }
                Sheets excelsheets = excel.Workbooks[1].Worksheets;
                //Получаем ссылку на лист
                var excelworksheet = (Worksheet)excelsheets.get_Item(1);
                //очищаем ячейки
                excelworksheet.Cells.Clear();
                //настройки страницы
                excelworksheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;
                excelworksheet.PageSetup.CenterHorizontally = true;
                excelworksheet.PageSetup.CenterVertically = true;
                //получаем ссылку на первые 3 ячейки
                Microsoft.Office.Interop.Excel.Range excelcells = excelworksheet.get_Range("A1", "B1");
                //объединяем их
                excelcells.Merge(Type.Missing);
                //задаем текст, настройки шрифта и ячейки
                excelcells.Font.Bold = true;
                excelcells.Value2 = "Заявка на брони";
                excelcells.RowHeight = 25;
                excelcells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                excelcells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                excelcells.Font.Name = "Times New Roman";
                excelcells.Font.Size = 14;
                excelcells = excelworksheet.get_Range("A2", "B2");
                excelcells.Merge(Type.Missing);
                excelcells.Value2 = "Дата:" + model.DateCreate.ToShortDateString();
                excelcells.RowHeight = 20;
                excelcells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                excelcells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                excelcells.Font.Name = "Times New Roman";
                excelcells.Font.Size = 12;

                var reservations = context.Reservations.Select(rec => new ReservationViewModel
                {
                    Id = rec.Id,
                    Name = rec.Name,
                    Description = rec.Description,
                    Number = rec.Number
                }).ToList();
                var requestReservations = model.RequestReservations;

                excelcells = excelworksheet.get_Range("A3", "A3");
                //обводим границы
                //получаем ячейки для выбеления рамки под таблицу
                var excelBorder = excelworksheet.get_Range(excelcells, excelcells.get_Offset(requestReservations.Count() - 1, 1));
                excelBorder.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                excelBorder.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                excelBorder.HorizontalAlignment = Constants.xlCenter;
                excelBorder.VerticalAlignment = Constants.xlCenter;
                excelBorder.BorderAround(Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);
                foreach (var requestReservation in requestReservations)
                {
                    excelcells.Value2 = reservations.FirstOrDefault(rec => rec.Id == requestReservation.ReservationId).Name;
                    excelcells.ColumnWidth = 20;
                    excelcells.get_Offset(0, 1).Value2 = requestReservation.NumberReservation;
                    excelcells = excelcells.get_Offset(1, 0);
                }
                //сохраняем
                excel.Workbooks[1].Save();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //закрываем
                excel.Quit();
            }
        }
    }
}
