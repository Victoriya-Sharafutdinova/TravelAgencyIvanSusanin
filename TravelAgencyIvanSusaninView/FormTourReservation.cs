using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using TravelAgencyIvanSusaninDAL.Interfaces;
using TravelAgencyIvanSusaninDAL.ViewModel;
using TravelAgencyIvanSusaninDAL.BindingModel;

namespace TravelAgencyIvanSusaninView
{
    public partial class FormTourReservation : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public TourReservationViewModel Model
        {
            set { model = value; }
            get { return model; }
        }

        private readonly IReservationService service;

        private TourReservationViewModel model;

        public FormTourReservation(IReservationService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Заполните поле количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Выберите бронь", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (model == null)
                {
                    model = new TourReservationViewModel
                    {
                        ReservationId = Convert.ToInt32(comboBox1.SelectedValue),
                        ReservationName = comboBox1.Text,
                        ReservationDescription = service.GetElement(Convert.ToInt32(comboBox1.SelectedValue)).Description,
                        NumberReservations = Convert.ToInt32(textBox1.Text)
                    };
                }
                else
                {
                    model.NumberReservations = Convert.ToInt32(textBox1.Text);
                }
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormTourReservation_Load(object sender, EventArgs e)
        {
            try
            {
                List<ReservationViewModel> list = service.GetList();
                if (list != null)
                {
                    comboBox1.DisplayMember = "Name";
                    comboBox1.ValueMember = "Id";
                    comboBox1.DataSource = list;
                    comboBox1.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (model != null)
            {
                comboBox1.Enabled = false;
                comboBox1.SelectedValue = model.ReservationId;
                textBox1.Text = model.NumberReservations.ToString();
            }
        }
    }
}
