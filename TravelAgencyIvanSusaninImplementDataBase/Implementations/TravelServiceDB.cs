using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TravelAgencyIvanSusaninDAL.BindingModel;
using TravelAgencyIvanSusaninDAL.Interfaces;
using TravelAgencyIvanSusaninDAL.ViewModel;
using TravelAgencyIvanSusaninModel;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Collections;
using iTextSharp.text;
using Microsoft.Office.Interop.Word;
using Document = Microsoft.Office.Interop.Word.Document;
using Microsoft.Office.Interop.Excel;

namespace TravelAgencyIvanSusaninImplementDataBase.Implementations
{
    public class TravelServiceDB : ITravelService
    {
        private readonly AbstractDbContext context;

        public TravelServiceDB(AbstractDbContext context)
        {
            this.context = context;
        }


        public TravelViewModel GetElement(int id)
        {
            var element = context.Travels.FirstOrDefault(rec => rec.Id == id);

            if (element != null)
            {
                return new TravelViewModel
                {
                    Id = element.Id,
                    ClientId = element.ClientId,
                    FIO = context.Clients.FirstOrDefault(client => client.Id == element.ClientId).FIO,
                    TotalCost = element.TotalCost,
                    TravelStatus = element.TravelStatus.ToString(),
                    DateCreate = element.DateCreate.ToString(),
                    DateImplement = element.DateImplement.ToString(),
                    TourTravels = context.TourTravels.Where(recOC => recOC.TravelId == element.Id)
                        .Select(recOC => new TourTravelViewModel
                        {
                            Id = recOC.Id,
                            TravelId = recOC.TravelId,
                            TourId = recOC.TourId,
                            TourName = recOC.Tour.Name,
                            Count = recOC.Count
                        })
                        .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void CreateTravel(TravelBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Travel element = new Travel
                    {
                        ClientId = model.ClientId,
                        DateCreate = DateTime.Now,
                        TotalCost = model.TotalCost,
                        TravelStatus = TravelStatus.Принят
                    };
                    context.Travels.Add(element);
                    context.SaveChanges();
                    var groupTours = model.TourTravels
                    .GroupBy(rec => rec.TourId)
                    .Select(rec => new
                    {
                        TourId = rec.Key,
                        Count = rec.Sum(r => r.Count), 
                        
                    });
                    foreach (var groupTour in groupTours)
                    {
                        context.TourTravels.Add(new TourTravel
                        {
                            TravelId = element.Id,
                            TourId = groupTour.TourId,
                            Count = groupTour.Count,
                        });
                        context.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void FinishTravel(TravelBindingModel model)
        {
            Travel element = context.Travels.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            if (element.TravelStatus != TravelStatus.Выполняется)
            {
                throw new Exception("Заказ не в статусе \"Выполняется\"");
            }
            element.DateImplement = DateTime.Now;
            element.TravelStatus = TravelStatus.Готов;
            context.SaveChanges();
        }

        public List<TravelViewModel> GetClientTravels(int clientId)
        {
            return GetList().Where(travel => travel.ClientId == clientId).ToList();
        }

        public List<TravelViewModel> GetList()
        {
            List<TravelViewModel> result = context.Travels.Select(rec => new TravelViewModel
            {
                Id = rec.Id,
                ClientId = rec.ClientId,
                DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
            SqlFunctions.DateName("mm", rec.DateCreate) + " " +
            SqlFunctions.DateName("yyyy", rec.DateCreate),
                DateImplement = rec.DateImplement == null ? "" :
            SqlFunctions.DateName("dd",
           rec.DateImplement.Value) + " " +
            SqlFunctions.DateName("mm",
           rec.DateImplement.Value) + " " +
            SqlFunctions.DateName("yyyy",
            rec.DateImplement.Value),
                TravelStatus = rec.TravelStatus.ToString(),
                TotalCost = rec.TotalCost,
                FIO = rec.Client.FIO,
                TourTravels = context.TourTravels
                .Where(recPC => recPC.TravelId == rec.Id)
                .Select(recPC => new TourTravelViewModel
                {
                    Id = recPC.Id,
                    TourId = recPC.TourId,
                    TravelId = recPC.TravelId,
                    TourName= recPC.Tour.Name,
                    Count = recPC.Count,
                    DateBegin = recPC.DateBegin,
                    DateEnd = recPC.DateEnd
                }).ToList()
            }).ToList();

            return result;
        }

        public void PayTravel(TravelBindingModel model)
        {
            Travel element = context.Travels.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            if (element.TravelStatus != TravelStatus.Готов)
            {
                throw new Exception("Путешествие не в статусе \"Готово\"");
            }
            element.TravelStatus = TravelStatus.Оплачен;
            context.SaveChanges();
        }

        public void TakeTravelInWork(TravelBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Travel element = context.Travels.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    if (element.TravelStatus != TravelStatus.Принят)
                    {
                        throw new Exception("Путешествие не в статусе \"Принят\"");
                    }

                    if (element.TravelStatus == TravelStatus.Принят || element.TravelStatus == TravelStatus.Зарезервирован)
                    {
                        var travelTours = context.TourTravels.Where(rec => rec.TravelId == element.Id);
                        foreach (var travelTour in travelTours)
                        {
                            var tourReservations = context.TourReservations.Where(rec => rec.TourId == travelTour.TourId);
                            foreach (var tourReservation in tourReservations)
                            {
                                if (element.TravelStatus == TravelStatus.Принят)
                                {
                                    var countReservations = context.Reservations.FirstOrDefault(reservation => reservation.Id == tourReservation.ReservationId).Number;
                                    if (tourReservation.NumberReservations > countReservations)
                                    {
                                        throw new Exception("Недостаточно броней");
                                    }
                                    else
                                    {
                                        tourReservation.Reservation.Number -= tourReservation.NumberReservations;
                                        context.SaveChanges();
                                        break;
                                    }
                                }
                                if (element.TravelStatus != TravelStatus.Зарезервирован) continue;
                                {
                                    int countReservations = tourReservation.Reservation.NumberReserve;

                                    if (tourReservation.NumberReservations > countReservations)
                                    {
                                        throw new Exception("Недостаточно броней");
                                    }
                                    else
                                    {
                                        tourReservation.Reservation.Number -= tourReservation.NumberReservations;
                                        context.SaveChanges();
                                        break;
                                    }
                                }
                            }                         
                        }
                        element.DateImplement = DateTime.Now;
                        element.TravelStatus = TravelStatus.Выполняется;
                        context.SaveChanges();
                        transaction.Commit();

                    }                  
                    else
                    {
                        throw new Exception("Заказ не в статусе \"Принят\" или \"Зарезервирован\"");
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Reservation (TravelBindingModel model, bool type)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var element = new Travel
                    {
                        ClientId = model.ClientId,
                        DateCreate = DateTime.Now,
                        TotalCost = model.TotalCost,
                        TravelStatus = TravelStatus.Зарезервирован,
                    };

                    context.Travels.Add(element);
                    context.SaveChanges();

                    var groupTours = model.TourTravels
                        .GroupBy(rec => rec.TourId)
                        .Select(rec => new { TourId = rec.Key, Count = rec.Sum(r => r.Count) });

                    foreach (var groupTour in groupTours)
                    {
                        var travelTour = new TourTravel
                        {
                            TravelId = element.Id,
                            TourId = groupTour.TourId,
                            Count = groupTour.Count

                        };

                        context.TourTravels.Add(travelTour);

                        var tourReservation = context.TourReservations.FirstOrDefault(rec => rec.TourId == travelTour.TourId);

                        var reservations = context.Reservations.FirstOrDefault(rec => rec.Id == tourReservation.ReservationId);

                        var reserveReservations = tourReservation.NumberReservations;

                        var check = reservations.Number - reservations.NumberReserve;

                        if (check >= reserveReservations)
                        {

                            
                            reservations.NumberReserve += reserveReservations;
                        }
                        else
                        {
                            throw new Exception("Недостаточно броней для резервации");
                        }

                        context.SaveChanges();
                    }
                    //string typeMessage = "";
                    //string fName = "";
                    //if (type)
                    //{
                    //    ReservWord(model);
                    //    typeMessage = "word";
                    //    fName = "C:\\Users\\Public\\Documents\\file.doc";
                    //}
                    //else
                    //{
                    //    ReservExel(model);
                    //    typeMessage = "xls";
                    //    fName = "C:\\Users\\Public\\Documents\\file.xls";

                    //}

                    transaction.Commit();

                    var client = context.Clients.FirstOrDefault(x => x.Id == model.ClientId);

                    Mail.SendEmail(client?.Email, "Оповещение по путешествиям",
                        $"Путешествие №{element.Id} от {element.DateCreate.ToShortDateString()} зарезервировано успешно", null);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            
            }
            
        }

        //private void ReservWord(TravelBindingModel model)
        //{
        //    Console.WriteLine();
        //    if (File.Exists("C:\\Users\\Public\\Documents\\file.doc"))
        //    {
        //        File.Delete("C:\\Users\\Public\\Documents\\file.doc");
        //    }
        //    var winword = new Microsoft.Office.Interop.Word.Application();
        //    try
        //    {
        //        object missing = System.Reflection.Missing.Value;
        //        //создаем документ
        //        Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);
        //        //получаем ссылку на параграф
        //        var paragraph = document.Paragraphs.Add(missing);
        //        var range = paragraph.Range;
        //        //задаем текст
        //        range.Text = "Резервирование броней по путешествию №" + model.Id;
        //        //задаем настройки шрифта
        //        var font = range.Font;
        //        font.Size = 16;
        //        font.Name = "Times New Roman";
        //        font.Bold = 1;
        //        //задаем настройки абзаца
        //        var paragraphFormat = range.ParagraphFormat;
        //        paragraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
        //        paragraphFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
        //        paragraphFormat.SpaceAfter = 10;
        //        paragraphFormat.SpaceBefore = 0;
        //        //добавляем абзац в документ
        //        range.InsertParagraphAfter();

        //        //var travelReservations = model.T;
        //        var reservations = context.TourReservations.Select(rec => new TourReservationViewModel
        //        {
        //            ReservationName = rec.Reservation.Name,
        //            NumberReservations = rec.NumberReservations * model.Count
        //        }).ToList();
        //        //создаем таблицу
        //        var paragraphTable = document.Paragraphs.Add(Type.Missing);
        //        var rangeTable = paragraphTable.Range;
        //        var table = document.Tables.Add(rangeTable, model.Count * context.TourReservations., 2, ref missing, ref missing);
        //        font = table.Range.Font;
        //        font.Size = 14;
        //        font.Name = "Times New Roman";
        //        var paragraphTableFormat = table.Range.ParagraphFormat;
        //        paragraphTableFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
        //        paragraphTableFormat.SpaceAfter = 0;
        //        paragraphTableFormat.SpaceBefore = 0;
        //        for (int i = 0; i < travelReservations.Count; ++i)
        //        {
        //            table.Cell(i + 1, 1).Range.Text = reservations.FirstOrDefault(rec => rec.Id == travelReservations[i].ReservationId).Name;
        //            table.Cell(i + 1, 2).Range.Text = travelReservations[i].NumberReservation.ToString();
        //        }
        //        //задаем границы таблицы
        //        table.Borders.InsideLineStyle = WdLineStyle.wdLineStyleInset;
        //        table.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
        //        paragraph = document.Paragraphs.Add(missing);
        //        range = paragraph.Range;
        //        range.Text = "Дата: " + model.DateCreate.ToLongDateString();
        //        font = range.Font;
        //        font.Size = 12;
        //        font.Name = "Times New Roman";
        //        paragraphFormat = range.ParagraphFormat;
        //        paragraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
        //        paragraphFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
        //        paragraphFormat.SpaceAfter = 10;
        //        paragraphFormat.SpaceBefore = 10;
        //        range.InsertParagraphAfter();
        //        //сохраняем
        //        object fileFormat = WdSaveFormat.wdFormatXMLDocument;
        //        document.SaveAs("C:\\Users\\Public\\Documents\\file.doc", ref fileFormat, ref missing,
        //        ref missing, ref missing, ref missing, ref missing,
        //        ref missing, ref missing, ref missing, ref missing,
        //        ref missing, ref missing, ref missing, ref missing,
        //        ref missing);
        //        document.Close(ref missing, ref missing, ref missing);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        winword.Quit();
        //    }
        //}

        //private void ReservExel(TravelBindingModel model)
        //{
        //    var excel = new Microsoft.Office.Interop.Excel.Application();
        //    try
        //    {
        //        //или создаем excel-файл, или открываем существующий
        //        if (File.Exists("C:\\Users\\Public\\Documents\\file.xls"))
        //        {
        //            excel.Workbooks.Open("C:\\Users\\Public\\Documents\\file.xls", Type.Missing, Type.Missing,
        //           Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
        //           Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //        }
        //        else
        //        {
        //            excel.SheetsInNewWorkbook = 1;
        //            excel.Workbooks.Add(Type.Missing);
        //            excel.Workbooks[1].SaveAs("C:\\Users\\Public\\Documents\\file.xls", XlFileFormat.xlExcel8,
        //            Type.Missing, Type.Missing, false, false, XlSaveAsAccessMode.xlNoChange, Type.Missing,
        //            Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //        }
        //        Sheets excelsheets = excel.Workbooks[1].Worksheets;
        //        //Получаем ссылку на лист
        //        var excelworksheet = (Worksheet)excelsheets.get_Item(1);
        //        //очищаем ячейки
        //        excelworksheet.Cells.Clear();
        //        //настройки страницы
        //        excelworksheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;
        //        excelworksheet.PageSetup.CenterHorizontally = true;
        //        excelworksheet.PageSetup.CenterVertically = true;
        //        //получаем ссылку на первые 3 ячейки
        //        Microsoft.Office.Interop.Excel.Range excelcells = excelworksheet.get_Range("A1", "B1");
        //        //объединяем их
        //        excelcells.Merge(Type.Missing);
        //        //задаем текст, настройки шрифта и ячейки
        //        excelcells.Font.Bold = true;
        //        excelcells.Value2 = "Заявка на брони";
        //        excelcells.RowHeight = 25;
        //        excelcells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
        //        excelcells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
        //        excelcells.Font.Name = "Times New Roman";
        //        excelcells.Font.Size = 14;
        //        excelcells = excelworksheet.get_Range("A2", "B2");
        //        excelcells.Merge(Type.Missing);
        //        excelcells.Value2 = "Дата:" + model.DateCreate.ToShortDateString();
        //        excelcells.RowHeight = 20;
        //        excelcells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
        //        excelcells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
        //        excelcells.Font.Name = "Times New Roman";
        //        excelcells.Font.Size = 12;

        //        var reservations = context.Reservations.Select(rec => new ReservationViewModel
        //        {
        //            Id = rec.Id,
        //            Name = rec.Name,
        //            Description = rec.Description,
        //            Number = rec.Number
        //        }).ToList();
        //        var travelReservations = model.RequestReservations;

        //        excelcells = excelworksheet.get_Range("A3", "A3");
        //        //обводим границы
        //        //получаем ячейки для выбеления рамки под таблицу
        //        var excelBorder = excelworksheet.get_Range(excelcells, excelcells.get_Offset(travelReservations.Count() - 1, 1));
        //        excelBorder.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //        excelBorder.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
        //        excelBorder.HorizontalAlignment = Constants.xlCenter;
        //        excelBorder.VerticalAlignment = Constants.xlCenter;
        //        excelBorder.BorderAround(Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
        //        Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
        //        Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);
        //        foreach (var travelReservation in travelReservations)
        //        {
        //            excelcells.Value2 = reservations.FirstOrDefault(rec => rec.Id == travelReservation.ReservationId).Name;
        //            excelcells.ColumnWidth = 20;
        //            excelcells.get_Offset(0, 1).Value2 = requestReservation.NumberReservation;
        //            excelcells = excelcells.get_Offset(1, 0);
        //        }
        //        //сохраняем
        //        excel.Workbooks[1].Save();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        //закрываем
        //        excel.Quit();
        //    }
        //}



       

        public void SaveDataBaseClient()
        {
            SaveEntity(context.Travels.ToList());
            SaveEntity(context.Clients.ToList());
            SaveEntity(context.TourTravels.ToList());     
        }

        public void SaveDataBaseAdmin()
        {
            
        }

        public void SaveEntity(IEnumerable entity)
        {
            var jsonFormatter = new DataContractJsonSerializer(entity.GetType());

            using (var fs = new FileStream($"backup/{GetNameEntity(entity)}.json",
                FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(fs, entity);
            }
        }

        private static string GetNameEntity(IEnumerable entity)
        {
            return entity.AsQueryable().ElementType.ToString().Split('.')[1];
        }

        public void Reservation(TravelBindingModel model)
        {
            throw new NotImplementedException();
        }
    }
}
