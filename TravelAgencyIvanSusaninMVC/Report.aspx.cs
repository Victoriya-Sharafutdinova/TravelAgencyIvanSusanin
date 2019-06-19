using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
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
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            FontSettings.FontsBaseDirectory = Server.MapPath("Fonts/");

            if (!Page.IsPostBack)
            {
                DataTable reserv = new DataTable();

                reserv.Columns.Add("Id", typeof(int));
                reserv.Columns.Add("Название брони", typeof(string));
                reserv.Columns.Add("Количество", typeof(int));

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
                        elem.TravelId,"", 0
                    });
                    foreach (var listElem in elem.Reservations)
                    {
                        people.Rows.Add(new object[]
                        {
                            "", listElem.ReservationName, listElem.NumberReservations
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

    }
}