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
            txtOutput.Text += "ConcMass\r\n";
            foreach (string item in FileHelper.ConcMassVal)
            {
                txtOutput.Text += $"{item}\r\n";
            }
            txtOutput.Text += "\r\n";

            //// write out displacement & slope
            //txtOutput.Text += "Displacement & Slope\r\n";
            //for (int i = 0; i < FileHelper.disp.Count; i++)
            //{
            //    txtOutput.Text += $"{FileHelper.NodeNum[i]} {FileHelper.disp[i]} {FileHelper.slope[i]} \r\n";
            //}
            //txtOutput.Text += "\r\n";

            // write out bending, shear, stress
            txtOutput.Text += "Moment, Shear\r\n";
            for (int i = 0; i < FileHelper.disp.Count; i++)
            {
                txtOutput.Text += $"{FileHelper.NodeNum[i]} {FileHelper.moment[i]} {FileHelper.shear[i]}\r\n";
            }
            txtOutput.Text += "\r\n";

            // check spring reactions, influence output

        }
    }

}