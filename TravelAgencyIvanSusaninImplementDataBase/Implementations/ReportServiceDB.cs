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
using TravelAgencyIvanSusaninModel;

namespace TravelAgencyIvanSusaninImplementDataBase.Implementations
{
    public class ReportServiceDB : IReportService
    {
        private AbstractDbContext context;

        public ReportServiceDB(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<TravelsReservationsViewModel> GetClientTravels(int id)
        {
            var list = context.Travels.Where(x => x.ClientId == id)
            .Join(context.TourTravels,
            travel => travel.Id,
            tourTravels => tourTravels.TravelId,
            (travel, tourTravels) => new { TravelId = travel.Id, tourTravels.TourId })
            .Join(context.Tours,
            last => last.TourId,
            tour => tour.Id,
            (last, tour) => new { last.TravelId, TourId = tour.Id })
            .Join(context.TourReservations,
            last => last.TourId,
            tourReservation => tourReservation.TourId,
            (last, tourReservation) => new { last.TravelId, TourReservation = tourReservation })
            .Join(context.Reservations,
            last => last.TourReservation.ReservationId,
            reservation => reservation.Id,
            (last, reservation) => new { last.TravelId, last.TourReservation.NumberReservations, Reservation = reservation })
            .ToList();

            var listTravelsReservations = new List<TravelsReservationsViewModel>();
            foreach (var element in list)
            {
                bool h = false;
                foreach (var travelReservations in listTravelsReservations)
                {
                    if (travelReservations.TravelId == element.TravelId)
                    {
                        h = true;
                        travelReservations.Reservations.Add(new TourReservationViewModel
                        {
                            ReservationName = element.Reservation.Name,
                            NumberReservations = element.NumberReservations
                        });
                    }
                }
                if (!h)
                {
                    listTravelsReservations.Add(new TravelsReservationsViewModel {
                        TravelId = element.TravelId,
                        Reservations = new List<TourReservationViewModel>()
                    });
                }
            }
            return listTravelsReservations;
        }

        public List<ClientTravelsViewModel> GetReservationReguest(ReportBindingModel model)
        {
            throw new NotImplementedException();
        }

        public void SaveClientTravels(int id)
        {
            if (!File.Exists("B:\\политех\\2 курс 2 семестр\\ТП\\TIMCYR.TTF"))
            {
                File.WriteAllBytes("B:\\политех\\2 курс 2 семестр\\ТП\\TIMCYR.TTF", Properties.Resources.TIMCYR);
            }
            string fileName = "C:\\Users\\Public\\Documents\\file.pdf";
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            doc.SetMargins(0.5f, 0.5f, 0.5f, 0.5f);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            doc.Open();
            BaseFont baseFont = BaseFont.CreateFont("TIMCYR.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            var phraseTitle = new Phrase("Путешствия клиентов",
            new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD));
            iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph(phraseTitle)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 12
            };
            doc.Add(paragraph);

            PdfPTable table = new PdfPTable(3)
            {
                TotalWidth = 800F
            };
            table.SetTotalWidth(new float[]
            {
                160, 140, 160, 100, 100, 140
            });

            PdfPCell cell = new PdfPCell();
            var fontForCellBold = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD);
            table.AddCell(new PdfPCell(new Phrase("Id путешествия", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Брони", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Количество", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            var list = GetClientTravels(id);
            var fontForCells = new Font(baseFont, 10);
            for (int i = 0; i < list.Count; i++)
            {
                cell = new PdfPCell(new Phrase(list[i].TravelId.ToString(), fontForCells));
                table.AddCell(cell);
                foreach (var reserv in list[i].Reservations)
                {
                    cell = new PdfPCell(new Phrase(reserv.ReservationName, fontForCells));
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(reserv.NumberReservations.ToString(), fontForCells));
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(""));
                    table.AddCell(cell);
                }
                table.AddCell(cell);
                //вставляем таблицу          
                doc.Add(table);

                doc.Close();
            }
        }

        public void SaveReservationReguest(ReportBindingModel model)
        {
            throw new NotImplementedException();
        }



    }
}