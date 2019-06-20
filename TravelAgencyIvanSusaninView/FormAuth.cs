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

namespace TravelAgencyIvanSusaninView
{
    public partial class FormAuth : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public string password = "123";

        public FormAuth()
        {
            InitializeComponent();
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            if (textBox.Text == password)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
