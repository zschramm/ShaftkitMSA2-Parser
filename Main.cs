using System;
using System.Collections;
using System.Security;
using System.Windows.Forms.DataVisualization.Charting;
using System.Configuration;

namespace ShaftkitMSA2_Parser
{
    public partial class Main : Form
    {
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings1.Default.InputPath = txtInputFile.Text;
            Properties.Settings1.Default.OutputPath = txtOutputFolder.Text;
            Properties.Settings1.Default.Save();
        }


        public Main()
        {
            InitializeComponent();
            this.FormClosed += Main_FormClosed;

            txtInputFile.Text = Properties.Settings1.Default.InputPath;
            txtOutputFolder.Text = Properties.Settings1.Default.OutputPath;

        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            // make sure file exists
            if (File.Exists(txtInputFile.Text))
            {
                FileHelper helper = new FileHelper();

                // read data from input file
                helper.ReadFromFile(txtInputFile.Text);
                helper.WriteCSV(txtOutputFolder.Text + "\\output.csv");

                helper.CreatePlots(txtOutputFolder.Text);
            }
            else
            {
                MessageBox.Show("The input file does not exist or there is no permission to access it.");
            }
        }

        private void btnOpenFileDialog_Click(object sender, EventArgs e)
        {
            
            if (txtInputFile.Text != null);
            {
                if (Directory.Exists(Path.GetDirectoryName(txtInputFile.Text)))
                {
                    openFileDialog1.InitialDirectory = Path.GetDirectoryName(txtInputFile.Text);
                    openFileDialog1.FileName = Path.GetFileName(txtInputFile.Text);
                }
                else
                {
                    openFileDialog1.InitialDirectory = System.Environment.GetEnvironmentVariable("USERPROFILE");
                    openFileDialog1.FileName = "";
                }
            }
            
            openFileDialog1.Filter = "Output File (*.OUT)|*.OUT|All Files (*.*)|*.*";
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtInputFile.Text = openFileDialog1.FileName;
            }
        }

        private void btnSaveFileDialog_Click(object sender, EventArgs e)
        {

            if (txtOutputFolder.Text != null) ;
            {
                if (Directory.Exists(Path.GetDirectoryName(txtOutputFolder.Text)))
                {
                    folderBrowserDialog1.InitialDirectory = Path.GetDirectoryName(txtOutputFolder.Text);
                }
                else
                {
                    folderBrowserDialog1.InitialDirectory = System.Environment.GetEnvironmentVariable("USERPROFILE");
                }
            }

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtOutputFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }

}