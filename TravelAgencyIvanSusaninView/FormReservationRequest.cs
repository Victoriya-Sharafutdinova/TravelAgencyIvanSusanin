using TravelAgencyIvanSusaninDAL.BindingModel;
using TravelAgencyIvanSusaninDAL.ViewModel;
using TravelAgencyIvanSusaninDAL.Interfaces;
using System.Windows.Forms;
using Unity;
using System.Collections.Generic;
using System;

namespace TravelAgencyIvanSusaninView
{
    public partial class FormReservationRequest : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IReservationService service;

        private ReservationRequestViewModel model;

        public ReservationRequestViewModel Model
        {
            set { model = value; }
            get { return model; }
        }

        public FormReservationRequest(IReservationService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void buttonSave_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (model == null)
                {
                    model = new ReservationRequestViewModel
                    {
                        ReservationId = Convert.ToInt32(comboBox1.SelectedValue),
                        ReservationName = comboBox1.Text,
                        NumberReservation = Convert.ToInt32(textBox1.Text)
                    };
                }
                else
                {
                    model.NumberReservation = Convert.ToInt32(textBox1.Text);
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

        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void FormRequest_Load(object sender, System.EventArgs e)
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
                textBox1.Text = model.NumberReservation.ToString();
            }
        }
    }
}
