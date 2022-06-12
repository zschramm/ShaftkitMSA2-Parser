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
            foreach (float item in FileHelper.ConcMassVal)
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
            txtOutput.Text += "Influence\r\n";
            for (int i = 0; i < FileHelper.inf.Count; i++)
            {
                for (int j = 0; j < FileHelper.inf[i].Count; j++)
                {
                    txtOutput.Text += $"{FileHelper.inf[i][j]} ";
                }
                txtOutput.Text += $"\r\n";
            }
            txtOutput.Text += "\r\n";
        }
    }

}