using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TravelAgencyIvanSusaninDAL.BindingModel;
using TravelAgencyIvanSusaninDAL.Interfaces;
using TravelAgencyIvanSusaninDAL.ViewModel;
using Unity;

namespace TravelAgencyIvanSusaninView
{
    public partial class FormReport : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IReportService service;

        public FormReport(IReportService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void buttonToPdf_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.Value.Date >= dateTimePickerTo.Value.Date)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                service.SaveTourRequest(new ReportBindingModel
                {
                    DateFrom = dateTimePickerFrom.Value,
                    DateTo = dateTimePickerTo.Value
                });
                MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.Value.Date >= dateTimePickerTo.Value.Date)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                dataGridView1.DataSource = GetData(new ReportBindingModel
                {
                    DateFrom = dateTimePickerFrom.Value,
                    DateTo = dateTimePickerTo.Value
                });
                MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<TourRequestReportViewModel> GetData(ReportBindingModel report)
        {
            var list = service.GetTourRequest(report);
            var data = new List<TourRequestReportViewModel>();
            for (int i = 0; i < list.Count; i++)
            {
                var elem = new TourRequestReportViewModel();
                elem.TourName = list[i].TourName;
                elem.TourDateCreate = list[i].TourDateCreate;

                bool firstLine = true;
                foreach (var reservation in list[i].Reservations)
                {
                    if (!firstLine)
                    {
                        elem = new TourRequestReportViewModel();
                    }
                    elem.ReservationName = reservation.ReservationName;
                    elem.NumberReservations = reservation.NumberReservations;

                    bool firstLineSecond = true;
                    foreach (var request in reservation.ReservationRequests)
                    {
                        if (!firstLineSecond)
                        {
                            elem = new TourRequestReportViewModel();
                        }
                        elem.RequestDateCreate = request.DateCreate;
                        elem.NumberReservationsRequest = request.NumberReservation;
                        data.Add(elem);

                        firstLineSecond = false;
                    }
                    firstLine = false;
                }
            }

            return data;
        }
    }
}
