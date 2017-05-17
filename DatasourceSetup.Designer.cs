namespace ttpim.gamemodule
{
    partial class DatasourceSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatasourceSetup));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtDatasourcePath = new Telerik.WinControls.UI.RadTextBox();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.btnOK = new Telerik.WinControls.UI.RadButton();
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.lblDbExists = new Telerik.WinControls.UI.RadLabel();
            this.lblDbPathExists = new Telerik.WinControls.UI.RadLabel();
            this.picDbPathExists = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.picDbExists = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.lblDbNameValid = new Telerik.WinControls.UI.RadLabel();
            this.picDbNameValid = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtDatasourcePath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDbExists)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDbPathExists)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDbNameValid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtDatasourcePath
            // 
            this.txtDatasourcePath.Location = new System.Drawing.Point(12, 13);
            this.txtDatasourcePath.Name = "txtDatasourcePath";
            this.txtDatasourcePath.Size = new System.Drawing.Size(607, 20);
            this.txtDatasourcePath.TabIndex = 0;
            this.txtDatasourcePath.TabStop = false;
            // 
            // radButton1
            // 
            this.radButton1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.radButton1.Location = new System.Drawing.Point(625, 12);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(82, 24);
            this.radButton1.TabIndex = 1;
            this.radButton1.Text = "Browse . . .";
            this.radButton1.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnOK.Location = new System.Drawing.Point(530, 40);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(89, 24);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Location = new System.Drawing.Point(625, 42);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(82, 24);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            // 
            // lblDbExists
            // 
            this.lblDbExists.Location = new System.Drawing.Point(45, 42);
            this.lblDbExists.Name = "lblDbExists";
            this.lblDbExists.Size = new System.Drawing.Size(93, 18);
            this.lblDbExists.TabIndex = 9;
            this.lblDbExists.Text = "Datasource exists";
            this.lblDbExists.Visible = false;
            // 
            // lblDbPathExists
            // 
            this.lblDbPathExists.Location = new System.Drawing.Point(202, 42);
            this.lblDbPathExists.Name = "lblDbPathExists";
            this.lblDbPathExists.Size = new System.Drawing.Size(119, 18);
            this.lblDbPathExists.TabIndex = 10;
            this.lblDbPathExists.Text = "Datasource path exists";
            this.lblDbPathExists.Visible = false;
            // 
            // picDbPathExists
            // 
            this.picDbPathExists.BorderShadowColor = System.Drawing.Color.Empty;
            this.picDbPathExists.Image = ((object)(resources.GetObject("picDbPathExists.Image")));
            this.picDbPathExists.Location = new System.Drawing.Point(169, 38);
            this.picDbPathExists.Name = "picDbPathExists";
            this.picDbPathExists.ScaleImage = Infragistics.Win.ScaleImage.Always;
            this.picDbPathExists.Size = new System.Drawing.Size(27, 26);
            this.picDbPathExists.TabIndex = 8;
            this.picDbPathExists.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.picDbPathExists.Visible = false;
            // 
            // picDbExists
            // 
            this.picDbExists.BorderShadowColor = System.Drawing.Color.Empty;
            this.picDbExists.Image = ((object)(resources.GetObject("picDbExists.Image")));
            this.picDbExists.Location = new System.Drawing.Point(12, 38);
            this.picDbExists.Name = "picDbExists";
            this.picDbExists.ScaleImage = Infragistics.Win.ScaleImage.Always;
            this.picDbExists.Size = new System.Drawing.Size(27, 26);
            this.picDbExists.TabIndex = 7;
            this.picDbExists.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.picDbExists.Visible = false;
            // 
            // lblDbNameValid
            // 
            this.lblDbNameValid.Location = new System.Drawing.Point(381, 42);
            this.lblDbNameValid.Name = "lblDbNameValid";
            this.lblDbNameValid.Size = new System.Drawing.Size(133, 18);
            this.lblDbNameValid.TabIndex = 12;
            this.lblDbNameValid.Text = "Datasource name is valid.";
            this.lblDbNameValid.Visible = false;
            // 
            // picDbNameValid
            // 
            this.picDbNameValid.BorderShadowColor = System.Drawing.Color.Empty;
            this.picDbNameValid.Image = ((object)(resources.GetObject("picDbNameValid.Image")));
            this.picDbNameValid.Location = new System.Drawing.Point(348, 38);
            this.picDbNameValid.Name = "picDbNameValid";
            this.picDbNameValid.ScaleImage = Infragistics.Win.ScaleImage.Always;
            this.picDbNameValid.Size = new System.Drawing.Size(27, 26);
            this.picDbNameValid.TabIndex = 11;
            this.picDbNameValid.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.picDbNameValid.Visible = false;
            // 
            // DatasourceSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 87);
            this.Controls.Add(this.lblDbNameValid);
            this.Controls.Add(this.picDbNameValid);
            this.Controls.Add(this.lblDbPathExists);
            this.Controls.Add(this.lblDbExists);
            this.Controls.Add(this.picDbPathExists);
            this.Controls.Add(this.picDbExists);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.radButton1);
            this.Controls.Add(this.txtDatasourcePath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "DatasourceSetup";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select a Datasource";
            this.ThemeName = "ControlDefault";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.txtDatasourcePath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDbExists)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDbPathExists)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDbNameValid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Telerik.WinControls.UI.RadTextBox txtDatasourcePath;
        private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.UI.RadButton btnOK;
        private Telerik.WinControls.UI.RadButton btnCancel;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox picDbExists;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox picDbPathExists;
        private Telerik.WinControls.UI.RadLabel lblDbExists;
        private Telerik.WinControls.UI.RadLabel lblDbPathExists;
        private Telerik.WinControls.UI.RadLabel lblDbNameValid;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox picDbNameValid;
    }
}

