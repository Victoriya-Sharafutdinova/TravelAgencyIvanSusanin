using TravelAgencyIvanSusaninDAL.BindingModel;
using TravelAgencyIvanSusaninDAL.ViewModel;
using TravelAgencyIvanSusaninDAL.Interfaces;
using System.Windows.Forms;
using Unity;
using System.Collections.Generic;
using System;

namespace TravelAgencyIvanSusaninView
{
    public partial class FormRequest : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IRequestService service;

        private readonly IReservationService reservationService;

        public FormRequest(IRequestService service, IReservationService reservationService)
        {
            InitializeComponent();
            this.service = service;
            this.reservationService = reservationService;
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
                service.CreateRequest(new ReservationRequestBindingModel
                {
                    ReservationId = Convert.ToInt32(comboBox1.SelectedValue),
                    NumberReservation = Convert.ToInt32(textBox1.Text)
                });
                MessageBox.Show("Формирование заявок на брони прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                List<ReservationViewModel> list = reservationService.GetList();
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
        }
    }
}
