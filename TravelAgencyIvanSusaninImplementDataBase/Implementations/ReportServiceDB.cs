using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.util;
using System.Text;
using System.Threading.Tasks;
using TravelAgencyIvanSusaninDAL.BindingModel;
using TravelAgencyIvanSusaninDAL.ViewModel;
using System.IO;
using TravelAgencyIvanSusaninDAL.Interfaces;

namespace TravelAgencyIvanSusaninImplementDataBase.Implementations
{
    public class ReportServiceDB : IReportService
    {
        private AbstractDbContext context;

        public ReportServiceDB(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<TravelsReservationsViewModel> GetClientTravels()
        {
            return context.Travels
                .ToList()
                .GroupJoin(
                context.TourTravels
                    .Include(r => r.Tour)
                    .ToList()
                    .Join(
                    context.TourReservations.ToList(),
                travel => travel,
                travelReservations => travelReservations.Reservation,
                (travel, travelReservList) => new TravelsReservationsViewModel
                {
                    TravelId = travel.Id,
                    Total = travelReservList.Sum (r => r.NumberReservations),
                    Reservations = travelReservList.Select(r =>
                    new Tuple<string, int>(r.Reservation.Name, r.NumberReservations))
                }))
                    .ToList();




            //    return context.Travels.Include(rec => rec.T).Include(rec => rec.TourTravels)
            //    .Where(rec => rec.DateCreate >= model.DateFrom && rec.DateCreate <= model.DateTo)
            //     .Select(rec => new ClientTravelsViewModel
            //     {
            //         ClientName = rec.Client.FIO,
            //         DateCreateTravel = SqlFunctions.DateName("dd", rec.DateCreate)
            //            + " " +
            //             SqlFunctions.DateName("mm", rec.DateCreate) +
            //            " " +
            //             SqlFunctions.DateName("yyyy", rec.DateCreate),
            //         TotalSum = rec.TotalCost,
            //         StatusTravel = rec.TravelStatus.ToString(),
            //         TourTravels = 
            //     })
            //    .ToList();
        }

        public List<ClientTravelsViewModel> GetReservationReguest(ReportBindingModel model)
        {
            throw new NotImplementedException();
        }

        public void SaveClientTravels(ReportBindingModel model)
        {
            if (!File.Exists("TIMCYR.TTF"))
            {
                File.WriteAllBytes("TIMCYR.TTF", Properties.Resources.TIMCYR);
            }
            //открываем файл для работы
            FileStream fs = new FileStream(model.FileName, FileMode.OpenOrCreate,
           FileAccess.Write);
            //создаем документ, задаем границы, связываем документ и поток
            Document doc = new Document();
            doc.SetMargins(0.5f, 0.5f, 0.5f, 0.5f);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();
            BaseFont baseFont = BaseFont.CreateFont("TIMCYR.TTF", BaseFont.IDENTITY_H,
           BaseFont.NOT_EMBEDDED);
            //вставляем заголовок
            var phraseTitle = new Phrase("Заказы клиентов", new Font(baseFont, 16, Font.BOLD));
            Paragraph paragraph = new Paragraph(phraseTitle)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 12
            };
            doc.Add(paragraph);
            var phrasePeriod = new Phrase("c " + model.DateFrom.Value.ToShortDateString()
           +
            " по " + model.DateTo.Value.ToShortDateString(),
           new Font(baseFont, 14,
           Font.BOLD));
            paragraph = new Paragraph(phrasePeriod)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 12
            };
            doc.Add(paragraph);
            //вставляем таблицу, задаем количество столбцов, и ширину колонок
            PdfPTable table = new PdfPTable(3)
            {
                TotalWidth = 800F
            };
            table.SetTotalWidth(new float[] { 160, 140, 160, 100, 100, 140 });
            //вставляем шапку
            PdfPCell cell = new PdfPCell();
            var fontForCellBold = new Font(baseFont, 10, Font.BOLD);
            table.AddCell(new PdfPCell(new Phrase("Номер путешествия", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Название брони", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            table.AddCell(new PdfPCell(new Phrase("Количество", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            
            //заполняем таблицу
            var list = GetClientTravels();
            var fontForCells = new Font(baseFont, 10);
            for (int i = 0; i < list.Count; i++)
            {
                cell = new PdfPCell(new Phrase(list[i].TravelId.ToString(), fontForCells));
                table.AddCell(cell);
                foreach (var reserv in list[i].Reservations)
                {
                    cell = new PdfPCell(new Phrase(reserv.Item1, fontForCells));
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(reserv.Item2.ToString(), fontForCells));
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(""));
                    table.AddCell(cell);
                }

            }
            //вставляем итого
            cell = new PdfPCell(new Phrase("Итого:", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Colspan = 4,
                Border = 0
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(list.Sum(rec => rec.Total).ToString(), fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Border = 0
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("", fontForCellBold))
            {
                Border = 0
            };
            table.AddCell(cell);
            //вставляем таблицу
            doc.Add(table);
            doc.Close();
        }

        public void SaveReservationReguest(ReportBindingModel model)
        {
            throw new NotImplementedException();
        }



    }
}