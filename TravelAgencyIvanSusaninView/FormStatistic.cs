using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TravelAgencyIvanSusaninDAL.Interfaces;
using Unity;

namespace TravelAgencyIvanSusaninView
{
    public partial class FormStatistic : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IStatisticService service;

        public FormStatistic(IStatisticService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormStatistic_Load(object sender, EventArgs e)
        {
            label1.Text = "Самая популярная бронь: " + service.GetMostPopularReservation();
            label4.Text = "Самая невостребованная бронь: " + service.GetLessPopularReservation();
            label2.Text = "Среднее число заказанных броней: " + service.GetAverageReservationRequestsNumber();
            label3.Text = "Средняя стоимость тура: " + service.GetAverageTourCost();
        }
    }
}
