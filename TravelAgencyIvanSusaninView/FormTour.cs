using TravelAgencyIvanSusaninDAL.BindingModel;
using TravelAgencyIvanSusaninDAL.ViewModel;
using TravelAgencyIvanSusaninDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;

namespace TravelAgencyIvanSusaninView
{
    public partial class FormTour : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly ITourService service;

        private int? id;

        private List<TourReservationViewModel> tourReservations;

        public FormTour(ITourService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormTour_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    TourViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.Name;
                        textBoxDescription.Text = view.Description;
                        textBoxCost.Text = view.Cost.ToString();
                        tourReservations = view.TourReservations;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                tourReservations = new List<TourReservationViewModel>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (tourReservations != null)
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = tourReservations;
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxDescription.Text))
            {
                MessageBox.Show("Заполните описание", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxCost.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tourReservations == null || tourReservations.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<TourReservationBindingModel> tourReservationBM = new List<TourReservationBindingModel>();
                for (int i = 0; i < tourReservations.Count; ++i)
                {
                    tourReservationBM.Add(new TourReservationBindingModel
                    {
                        Id = tourReservations[i].Id,
                        TourId = tourReservations[i].TourId,
                        ReservationId = tourReservations[i].ReservationId,
                        NumberReservations = tourReservations[i].NumberReservations
                    });
                }
                if (id.HasValue)
                {
                    service.UpdElement(new TourBindingModel
                    {
                        Id = id.Value,
                        Name = textBoxName.Text,
                        Description = textBoxDescription.Text,
                        Cost = Convert.ToInt32(textBoxCost.Text),
                        TourReservations = tourReservationBM
                    });
                }
                else
                {
                    service.AddElement(new TourBindingModel
                    {
                        Name = textBoxName.Text,
                        Description = textBoxDescription.Text,
                        Cost = Convert.ToInt32(textBoxCost.Text),
                        TourReservations = tourReservationBM
                    });
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

        private void buttonClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormTourReservation>();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                    {
                        form.Model.TourId = id.Value;
                    }
                    tourReservations.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormTourReservation>();
                form.Model = tourReservations[dataGridView1.SelectedRows[0].Cells[0].RowIndex];
                if (form.ShowDialog() == DialogResult.OK)
                {
                    tourReservations[dataGridView1.SelectedRows[0].Cells[0].RowIndex] = form.Model;
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
                        tourReservations.RemoveAt(dataGridView1.SelectedRows[0].Cells[0].RowIndex);
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
