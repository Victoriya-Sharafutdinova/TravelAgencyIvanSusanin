namespace TravelAgencyIvanSusaninView
{
    partial class FormTours
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonTour = new System.Windows.Forms.Button();
            this.buttonReservation = new System.Windows.Forms.Button();
            this.buttonRequest = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonTour
            // 
            this.buttonTour.Location = new System.Drawing.Point(673, 12);
            this.buttonTour.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonTour.Name = "buttonTour";
            this.buttonTour.Size = new System.Drawing.Size(115, 46);
            this.buttonTour.TabIndex = 0;
            this.buttonTour.Text = "Добавить тур";
            this.buttonTour.UseVisualStyleBackColor = true;
            this.buttonTour.Click += new System.EventHandler(this.buttonTour_Click);
            // 
            // buttonReservation
            // 
            this.buttonReservation.Location = new System.Drawing.Point(552, 12);
            this.buttonReservation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonReservation.Name = "buttonReservation";
            this.buttonReservation.Size = new System.Drawing.Size(115, 46);
            this.buttonReservation.TabIndex = 1;
            this.buttonReservation.Text = "Список броней";
            this.buttonReservation.UseVisualStyleBackColor = true;
            this.buttonReservation.Click += new System.EventHandler(this.buttonReservation_Click);
            // 
            // buttonRequest
            // 
            this.buttonRequest.Location = new System.Drawing.Point(431, 12);
            this.buttonRequest.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonRequest.Name = "buttonRequest";
            this.buttonRequest.Size = new System.Drawing.Size(115, 46);
            this.buttonRequest.TabIndex = 2;
            this.buttonRequest.Text = "Сформировать заявку";
            this.buttonRequest.UseVisualStyleBackColor = true;
            this.buttonRequest.Click += new System.EventHandler(this.buttonRequest_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 63);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(776, 375);
            this.dataGridView1.TabIndex = 3;
            // 
            // FormTours
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.buttonRequest);
            this.Controls.Add(this.buttonReservation);
            this.Controls.Add(this.buttonTour);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormTours";
            this.Text = "FormTours";
            this.Load += new System.EventHandler(this.FormTours_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonTour;
        private System.Windows.Forms.Button buttonReservation;
        private System.Windows.Forms.Button buttonRequest;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}