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
    public partial class FormRequest : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IRequestService service;

        private List<ReservationRequestViewModel> requestReservations;

        public FormRequest(IRequestService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormRequest_Load(object sender, EventArgs e)
        {
            requestReservations = new List<ReservationRequestViewModel>();
            /*comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "type";
            comboBox1.da
            comboBox1.SelectedItem = null;*/
        }

        private void LoadData()
        {
            try
            {
                if (requestReservations != null)
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = requestReservations;
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].Visible = false;
                    dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            if (requestReservations == null || requestReservations.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Выберите формат файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<ReservationRequestBindingModel> requestReservationsBM = new List<ReservationRequestBindingModel>();
                for (int i = 0; i < requestReservations.Count; ++i)
                {
                    requestReservationsBM.Add(new ReservationRequestBindingModel
                    {
                        Id = requestReservations[i].Id,
                        RequestId = requestReservations[i].RequestId,
                        ReservationId = requestReservations[i].ReservationId,
                        NumberReservation = requestReservations[i].NumberReservation,
                    });
                }
                service.CreateRequest(new RequestBindingModel
                {
                    DateCreate = DateTime.Now,
                    RequestReservations = requestReservationsBM
                }, comboBox1.SelectedItem.ToString() == "doc");
                MessageBox.Show("Формирование заявок на брони прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormReservationRequest>();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.Model != null)
                {
                    requestReservations.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormReservationRequest>();
                form.Model = requestReservations[dataGridView1.SelectedRows[0].Cells[0].RowIndex];
                if (form.ShowDialog() == DialogResult.OK)
                {
                    requestReservations[dataGridView1.SelectedRows[0].Cells[0].RowIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        requestReservations.RemoveAt(dataGridView1.SelectedRows[0].Cells[0].RowIndex);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
