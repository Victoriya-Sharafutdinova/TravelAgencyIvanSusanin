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
                    listTravelsReservations.Add(new TravelsReservationsViewModel
                    {
                        TravelId = element.TravelId,
                        Reservations = new List<TourReservationViewModel>()
                    });
                    foreach (var travelReservations in listTravelsReservations)
                    {
                        if (travelReservations.TravelId == element.TravelId)
                        {
                            travelReservations.Reservations.Add(new TourReservationViewModel
                            {
                                ReservationName = element.Reservation.Name,
                                NumberReservations = element.NumberReservations
                            });
                        }
                    }
                }
            }
            return listTravelsReservations;
        }

        public void SaveClientTravels(int id)
        {
            //из ресрусов получаем шрифт для кирилицы
            if (!File.Exists("C:\\Users\\Public\\Documents\\TIMCYR.TTF"))
            {
                File.WriteAllBytes("C:\\Users\\Public\\Documents\\TIMCYR.TTF", Properties.Resources.TIMCYR);
            }

            string fileName = "C:\\Users\\Public\\Documents\\file.pdf";
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            doc.SetMargins(0.5f, 0.5f, 0.5f, 0.5f);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

           
            BaseFont baseFont = BaseFont.CreateFont("C:\\Users\\Public\\Documents\\TIMCYR.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            doc.Open();
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
                160, 140, 160
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
                
            }doc.Add(table);

                doc.Close();
        }

        public List<TourRequestViewModel> GetTourRequest(ReportBindingModel model)
        {
            var list = context.Tours.Where(x => x.DateCreate >= model.DateFrom && x.DateCreate <= model.DateTo)
                .Join(context.TourReservations,
                tour => tour.Id,
                tourReservation => tourReservation.TourId,
                (tour, tourReservation) => new TourRequestViewModel
                {
                    Id = tour.Id,
                    TourName = tour.Name,
                    TourDateCreate = tour.DateCreate,
                    Reservations = context.Reservations.Where(x => x.Id == tourReservation.ReservationId).Select(y => new TourReservationViewModel
                    {
                        ReservationName = y.Name,
                        NumberReservations = tourReservation.NumberReservations,
                        ReservationRequests = context.ReservationRequests.Where(x => x.ReservationId == y.Id)
                            .Join(context.Requests,
                            reservationRequest => reservationRequest.RequestId,
                            request => request.Id,
                            (reservationRequest, request) => new ReservationRequestViewModel {
                                DateCreate = request.DateCreate,
                                NumberReservation = reservationRequest.NumberReservation
                            })
                            .Where(x => x.DateCreate >= model.DateFrom && x.DateCreate <= model.DateTo).ToList()
                    }).ToList()
                }).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (list[i].Id == list[j].Id)
                    {
                        list[i].Reservations = list[i].Reservations.Concat(list[j].Reservations).ToList();
                        list.RemoveAt(j);
                        j--;
                    }
                }
            }
            return list;
        }

        public void SaveTourRequest(ReportBindingModel model)
        {
            //из ресрусов получаем шрифт для кирилицы
            if (!File.Exists("C:\\Users\\Public\\Documents\\TIMCYR.TTF"))
            {
                File.WriteAllBytes("C:\\Users\\Public\\Documents\\TIMCYR.TTF", Properties.Resources.TIMCYR);
            }
            //открываем файл для работы
            FileStream fs = new FileStream("C:\\Users\\Public\\Documents\\fileAdmin.pdf", FileMode.OpenOrCreate, FileAccess.Write);
            //создаем документ, задаем границы, связываем документ и поток
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            doc.SetMargins(0.5f, 0.5f, 0.5f, 0.5f);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();
            BaseFont baseFont = BaseFont.CreateFont("C:\\Users\\Public\\Documents\\TIMCYR.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //вставляем заголовок
            var phraseTitle = new Phrase("Туры и заявки с расшифровкой по броням",
            new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD));
            iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph(phraseTitle)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 12
            };
            doc.Add(paragraph);
            var phrasePeriod = new Phrase("c " + model.DateFrom.Value.ToShortDateString() +
            " по " + model.DateTo.Value.ToShortDateString(), new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD));
            paragraph = new iTextSharp.text.Paragraph(phrasePeriod)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 12
            };
            doc.Add(paragraph);
            //вставляем таблицу, задаем количество столбцов, и ширину колонок
            PdfPTable table = new PdfPTable(6)
            {
                TotalWidth = 800F
            };
            table.SetTotalWidth(new float[] { 160, 140, 160, 100, 140, 100 });
            //вставляем шапку
            PdfPCell cell = new PdfPCell();
            var fontForCellBold = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD);
            table.AddCell(new PdfPCell(new Phrase("Тур", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Дата создания тура", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Бронь", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Количество броней", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Заявка", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Кол-во броней в заявке", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            //заполняем таблицу
            var list = GetTourRequest(model);
            var fontForCells = new iTextSharp.text.Font(baseFont, 10);
            for (int i = 0; i < list.Count; i++)
            {
                cell = new PdfPCell(new Phrase(list[i].TourName, fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].TourDateCreate.ToShortDateString(), fontForCells));
                table.AddCell(cell);

                bool firstLine = true;
                foreach (var reservation in list[i].Reservations)
                {
                    if (!firstLine)
                    {
                        cell = new PdfPCell(new Phrase("", fontForCells));
                        table.AddCell(cell);
                        table.AddCell(cell);
                        
                    }
                    cell = new PdfPCell(new Phrase(reservation.ReservationName, fontForCells));
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(reservation.NumberReservations.ToString(), fontForCells));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);

                    bool firstLineSecond = true;
                    foreach (var request in reservation.ReservationRequests)
                    {
                        if (!firstLineSecond)
                        {
                            cell = new PdfPCell(new Phrase("", fontForCells));
                            table.AddCell(cell);
                            table.AddCell(cell);
                            table.AddCell(cell);
                            table.AddCell(cell);
                        }
                        cell = new PdfPCell(new Phrase(request.DateCreate.ToShortDateString(), fontForCells));
                        table.AddCell(cell);
                        cell = new PdfPCell(new Phrase(request.NumberReservation.ToString(), fontForCells));
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        table.AddCell(cell);

                        firstLineSecond = false;
                    }
                    firstLine = false;
                }
            }
            doc.Add(table);
            doc.Close();

            Mail.SendEmail(null, "Отчет","Туры и заявки с расшифровкой по броням", "C:\\Users\\Public\\Documents\\fileAdmin.pdf");
        }
    }
}