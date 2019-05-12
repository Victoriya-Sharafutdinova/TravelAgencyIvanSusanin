using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TravelAgencyIvanSusaninView
{
    public partial class FormTours : Form
    {
        public FormTours()
        {
            InitializeComponent();
        }

        private void buttonRequest_Click(object sender, EventArgs e)
        {

        }

        private void buttonReservation_Click(object sender, EventArgs e)
        {

        }

        private void buttonTour_Click(object sender, EventArgs e)
        {
            var formAddTour = new FormAddTour();
        }
    }
}
