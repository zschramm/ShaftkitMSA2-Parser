using ScottPlot;
using System;
using System.Collections;
using System.Security;
using System.Windows.Forms.DataVisualization.Charting;

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
            FileHelper data = new FileHelper();

            // read data from input file
            data.ReadFromFile(txtInputFile.Text);
            data.WriteCSV(txtOutputFile.Text);

            List<xy> srs = new List<xy>();
            srs = data.PrepareSeries(data.NodeX, data.Disp);
            
            chart1.DataSource = srs;
            chart1.Series[0].XValueMember = "x";
            chart1.Series[0].YValueMembers = "y";
            chart1.Series[0].AxisLabel = "Displacement (mm)";
            chart1.Series[0].ChartType = SeriesChartType.Line;
            chart1.DataBind();
            chart1.Update();



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

            // check influence
            txtOutput.Text += "influence\r\n";
            List<List<string>> inf = new List<List<string>>();
            inf = data.inf;
            for (int i = 0; i < inf.Count; i++)
            {
                for (int j = 0; j < inf[i].Count; j++)
                {
                    txtOutput.Text += $"{inf[i][j]} \r\n";
                }
                txtOutput.Text += $"\r\n";
            }
            txtOutput.Text += "\r\n";

            //series d = new series("displacment (mm)", 1);
            //d.charttype = seriescharttype.line;
            //d.points.addxy(0, 0);
            //d.points.addxy(1, 1);
            //d.points.addxy(2, 2);
            //chart1.series.add(d);
            //d.enabled = true;


            //chart1.Series.Add("Shear").YValueMembers = "Shear";
            //chart1.Series["Shear"].ChartType = SeriesChartType.Line;
            //chart1.Series["Shear"].XValueType = ChartValueType.Int32;
            //chart1.Series["Shear"].YValueType = ChartValueType.Int32;

            //chart1.Series.Add("Disp").YValueMembers = "Disp";
            //chart1.Series["Disp"].ChartType = SeriesChartType.Line;
            //chart1.Series["Disp"].XValueType = ChartValueType.Int32;
            //chart1.Series["Disp"].YValueType = ChartValueType.Int32;

            //chart1.Series.Add("Slope").YValueMembers = "Slope";
            //chart1.Series["Slope"].ChartType = SeriesChartType.Line;
            //chart1.Series["Slope"].XValueType = ChartValueType.Int32;
            //chart1.Series["Slope"].YValueType = ChartValueType.Int32;

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