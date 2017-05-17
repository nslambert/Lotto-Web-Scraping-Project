using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.IO;
using System.Configuration;

namespace ttpim.gamemodule
{
    public struct SetupData
    {
        public string DatasourcePath { get; set; }
        public string DatasourceName { get; set; }
        public string ApplicationDataRoot { get; set; }
        public string ApplicationBriefcasePath { get; set; }
        public bool FileExists { get; set; }
        public bool PathExists { get; set; }
    }

    public partial class DatasourceSetup : Telerik.WinControls.UI.RadForm
    {
        private SetupData _data;

        public DatasourceSetup()
        {
            InitializeComponent();
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.CancelButton;
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            _data = new SetupData();
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Data files (*.db)|*.db|Data files (*.db3)|*.db3|Data files (*.njdb)|*.njdb|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            
            openFileDialog1.Title = "Select a datasource path";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string dataSourceName = ConfigurationManager.AppSettings["DefaultDBName"].ToString();
                txtDatasourcePath.Text = openFileDialog1.FileName;
                FileInfo fileInfo = new FileInfo(openFileDialog1.FileName);
                string drive = Path.GetPathRoot(fileInfo.FullName);
                
                _data.DatasourcePath = txtDatasourcePath.Text;
                _data.FileExists = openFileDialog1.CheckFileExists;
                _data.PathExists = openFileDialog1.CheckPathExists;
                _data.DatasourceName = fileInfo.Name;
                _data.ApplicationDataRoot = drive;
                _data.ApplicationBriefcasePath = drive + "{N} - Briefcase\\{C} - Lotto";

                if (_data.DatasourceName == dataSourceName)
                {
                    picDbNameValid.Visible = true;
                    picDbNameValid.Image = global::ttpim.gamemodule.Properties.Resources.okmark;
                    picDbNameValid.AutoSize = true;
                    lblDbNameValid.Visible = true;
                    lblDbNameValid.Text = "Datasource name is valid.";
                }
                else
                {
                    picDbNameValid.Visible = true;
                    picDbNameValid.Image = global::ttpim.gamemodule.Properties.Resources.crossmark;
                    picDbNameValid.AutoSize = true;
                    lblDbNameValid.Visible = true;
                    lblDbNameValid.Text = "Datasource name is INVALID.";
                }

                if (_data.FileExists)
                {
                    picDbExists.Visible = true;
                    picDbExists.Image = global::ttpim.gamemodule.Properties.Resources.okmark;
                    picDbExists.AutoSize = true;
                    lblDbExists.Visible = true;
                    lblDbExists.Text = "Datasource file exists.";
                }
                else
                {
                    picDbExists.Visible = true;
                    picDbExists.Image = global::ttpim.gamemodule.Properties.Resources.crossmark;
                    picDbExists.AutoSize = true;
                    lblDbExists.Visible = true;
                    lblDbExists.Text = "Datasource file does NOT exist or is INVALID.";
                }

                if (_data.PathExists)
                {
                    picDbPathExists.Visible = true;
                    picDbPathExists.Image = global::ttpim.gamemodule.Properties.Resources.okmark;
                    picDbPathExists.AutoSize = true;
                    lblDbPathExists.Visible = true;
                    lblDbPathExists.Text = "Datasource file path exists.";
                }
                else
                {
                    picDbPathExists.Visible = true;
                    picDbPathExists.Image = global::ttpim.gamemodule.Properties.Resources.crossmark;
                    picDbPathExists.AutoSize = true;
                    lblDbPathExists.Visible = true;
                    lblDbPathExists.Text = "Datasource file path does NOT exist or is INVALID.";
                }
            }
            else if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                txtDatasourcePath.Text = "";
                _data.DatasourcePath = txtDatasourcePath.Text;
                picDbExists.Visible = false;
                picDbPathExists.Visible = false;
                lblDbExists.Text = "";
                lblDbExists.Visible = false;
                lblDbPathExists.Text = "";
                lblDbPathExists.Visible = false;
            }
        }

        public SetupData Execute()
        {
            this.ShowDialog();
            return _data;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
