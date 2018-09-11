namespace VegAutomation
{
    partial class frmunits
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
            this.dtgrdUnits = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dtgrdUnits)).BeginInit();
            this.SuspendLayout();
            // 
            // dtgrdUnits
            // 
            this.dtgrdUnits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgrdUnits.Location = new System.Drawing.Point(190, 28);
            this.dtgrdUnits.Name = "dtgrdUnits";
            this.dtgrdUnits.Size = new System.Drawing.Size(240, 150);
            this.dtgrdUnits.TabIndex = 0;
            // 
            // frmunits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 326);
            this.Controls.Add(this.dtgrdUnits);
            this.Name = "frmunits";
            this.Text = "frmunits";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmunits_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgrdUnits)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dtgrdUnits;
    }
}