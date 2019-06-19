using GemBox.Spreadsheet;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TravelAgencyIvanSusaninDAL.Interfaces;
using TravelAgencyIvanSusaninDAL.ViewModel;
using TravelAgencyIvanSusaninImplementDataBase;

namespace TravelAgencyIvanSusaninMVC
{
    public partial class Report : System.Web.UI.Page
    {
        private AbstractDbContext context = new AbstractDbContext();
        private IReportService reportService;
        protected void Page_Load(object sender, EventArgs e)
        {
            reportService = Globals.ReportService;
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            FontSettings.FontsBaseDirectory = Server.MapPath("Fonts/");

            if (!Page.IsPostBack)
            {
                DataTable reserv = new DataTable();

                reserv.Columns.Add("Id", typeof(string));
                reserv.Columns.Add("Название брони", typeof(string));
                reserv.Columns.Add("Количество", typeof(string));

                Session["reserv"] = reserv;
                LoadDataTable();
                this.SetDataBinding();
            }
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
                listTravelsReservations.Add(new TravelsReservationsViewModel
                {
                    TravelId = element.TravelId,
                    Reservations = new List<TourReservationViewModel>()
                });
            }
            foreach (var element in list)
            {
                foreach (var travelReservations in listTravelsReservations)
                {
                    if (travelReservations.TravelId == element.TravelId)
                    {
                        var tourCount = context.TourTravels.FirstOrDefault(x => element.TravelId == x.TravelId).Count;
                        travelReservations.Reservations.Add(new TourReservationViewModel
                        {
                            ReservationName = element.Reservation.Name,
                            NumberReservations = element.NumberReservations * tourCount
                        });
                    }
                }            
            }
            return listTravelsReservations;
        }

        private DataTable LoadDataTable()
        {
            DataTable people = (DataTable)Session["reserv"];
            var dict = GetClientTravels(Globals.AuthClient.Id);
            if (dict != null)
            {
                people.Rows.Clear();
                foreach (var elem in dict)
                {
                    people.Rows.Add(new object[]
                    {
                        elem.TravelId.ToString(),"", ""
                    });
                    foreach (var listElem in elem.Reservations)
                    {
                        people.Rows.Add(new object[]
                        {
                            "", listElem.ReservationName, listElem.NumberReservations.ToString()
                        });
                    }
                   // people.Rows.Add(new object[] { "Итого", "", elem.TotalAmount });
                    people.Rows.Add(new object[] { });
                }
            }
            return people;
        }

        private void SetDataBinding()
        {
            DataTable people = (DataTable)Session["reserv"];
            DataView peopleDataView = people.DefaultView;

            this.GridView1.DataSource = peopleDataView;
            peopleDataView.AllowDelete = true;
            this.GridView1.DataBind();
        }

        protected void Export_Click(object sender, EventArgs e)
        {
            int id = Globals.AuthClient.Id;
            reportService.SaveClientTravels(id);
            
        }
    }
}