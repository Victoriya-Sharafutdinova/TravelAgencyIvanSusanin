using System;
using System.Collections.Generic;
using TravelAgencyIvanSusaninDAL.BindingModel;
using TravelAgencyIvanSusaninDAL.Interfaces;
using TravelAgencyIvanSusaninDAL.ViewModel;
using TravelAgencyIvanSusaninModel;
using System.Linq;
using System.IO;
using Microsoft.Office.Interop.Word;

namespace TravelAgencyIvanSusaninImplementDataBase.Implementations
{
    public class RequestServiceDB : IRequestService
    {
        private AbstractDbContext context;

        public RequestServiceDB(AbstractDbContext context)
        {
            this.context = context;
        }

        public void CreateRequest(RequestBindingModel model)
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
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public List<RequestViewModel> GetList()
        {
            throw new NotImplementedException();
        }

        public void Request(RequestBindingModel model)
        {
            /*if (File.Exists("C:\\Users\\aniky\\Desktop\\request\\file.doc"))
            {
                File.Delete("C:\\Users\\aniky\\Desktop\\request\\file.doc");
            }
            var winword = new Application();
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
                var products = context.Engines.ToList();
                //создаем таблицу
                var paragraphTable = document.Paragraphs.Add(Type.Missing);
                var rangeTable = paragraphTable.Range;
                var table = document.Tables.Add(rangeTable, products.Count, 2, ref missing, ref missing);
                font = table.Range.Font;
                font.Size = 14;
                font.Name = "Times New Roman";
                var paragraphTableFormat = table.Range.ParagraphFormat;
                paragraphTableFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
                paragraphTableFormat.SpaceAfter = 0;
                paragraphTableFormat.SpaceBefore = 0;
                for (int i = 0; i < products.Count; ++i)
                {
                    table.Cell(i + 1, 1).Range.Text = products[i].EngineName;
                    table.Cell(i + 1, 2).Range.Text = products[i].Cost.ToString();
                }
                //задаем границы таблицы
                table.Borders.InsideLineStyle = WdLineStyle.wdLineStyleInset;
                table.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
                paragraph = document.Paragraphs.Add(missing);
                range = paragraph.Range;
                range.Text = "Дата: " + DateTime.Now.ToLongDateString();
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
                document.SaveAs("C:\\Users\\aniky\\Desktop\\request\\file.doc", ref fileFormat, ref missing,
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
            }*/
        }
    }
}
