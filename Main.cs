using ScottPlot;
using System;

namespace ShaftkitMSA2_Parser
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            // read data from input file
            FileHelper.ReadFromFile(txtInputFile.Text);

            FileHelper.WriteCSV(txtOutputFile.Text);

            //// write out nodeX
            //txtOutput.Text += "Nodes\r\n";
            //foreach (string item in FileHelper.NodeX)
            //{
            //    txtOutput.Text += $"{item}\r\n";
            //}
            //txtOutput.Text += "\r\n";


            //// write out element OD
            //txtOutput.Text += "Elements\r\n";
            //foreach (string item in FileHelper.ElemNum)
            //{
            //    txtOutput.Text += $"{item}\r\n";
            //}
            //txtOutput.Text += "\r\n";

            // write out concmass
            //txtOutput.Text += "ConcMass\r\n";
            //foreach (float item in FileHelper.ConcMassVal)
            //{
            //    txtOutput.Text += $"{item}\r\n";
            //}
            //txtOutput.Text += "\r\n";

            //// write out displacement & slope
            //txtOutput.Text += "Displacement & Slope\r\n";
            //for (int i = 0; i < FileHelper.disp.Count; i++)
            //{
            //    txtOutput.Text += $"{FileHelper.NodeNum[i]} {FileHelper.disp[i]} {FileHelper.slope[i]} \r\n";
            //}
            //txtOutput.Text += "\r\n";

            // write out bending, shear, stress
            //txtOutput.Text += "Moment, Shear\r\n";
            //for (int i = 0; i < FileHelper.disp.Count; i++)
            //{
            //    txtOutput.Text += $"{FileHelper.NodeNum[i]} {FileHelper.moment[i]} {FileHelper.shear[i]}\r\n";
            //}
            //txtOutput.Text += "\r\n";

            // check spring reactions
            //txtOutput.Text += "Reactions\r\n";
            //for (int i = 0; i < FileHelper.reactVal.Count; i++)
            //{
            //    txtOutput.Text += $"{FileHelper.reactNode[i]} {FileHelper.reactVal[i]}\r\n";
            //}
            //txtOutput.Text += "\r\n";

            //check influence
            //txtOutput.Text += "Influence\r\n";
            //for (int i = 0; i < FileHelper.inf.Count; i++)
            //{
            //    for (int j = 0; j < FileHelper.inf[i].Count; j++)
            //    {
            //        txtOutput.Text += $"{FileHelper.inf[i][j]} ";
            //    }
            //    txtOutput.Text += $"\r\n";
            //}
            //txtOutput.Text += "\r\n";

            double[] x = (double)FileHelper.NodeX;
            double[] disp = (double)FileHelper.Disp;
            Plot1.Plot.AddScatter(x, disp);

            // Axes can be customized
            Plot1.Plot.XAxis.Label("Position (m)");
            Plot1.Plot.YAxis.Label("Displacement (mm)");
            
            // Set axis limits to control the view
            //Plot1.SetAxisLimits(-20, 80, -2, 2);

            //Plot1.SaveFig("quickstart_axis.png");
        }

        private void txtInputFile_TextChanged(object sender, EventArgs e)
        {

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

            if (txtOutputFile.Text != null) ;
            {
                if (Directory.Exists(Path.GetDirectoryName(txtOutputFile.Text)))
                {
                    saveFileDialog1.InitialDirectory = Path.GetDirectoryName(txtOutputFile.Text);
                    saveFileDialog1.FileName = Path.GetFileName(txtOutputFile.Text);
                }
                else
                {
                    saveFileDialog1.InitialDirectory = System.Environment.GetEnvironmentVariable("USERPROFILE");
                    saveFileDialog1.FileName = "output.csv";
                }
            }

            saveFileDialog1.Filter = "Comma Separated Values (.csv)|*.csv|All Files (*.*)|*.*";
            

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtOutputFile.Text = saveFileDialog1.FileName;
            }
        }
    }

}