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

            FileHelper.ReadFromFile(txtInputFile.Text);
            foreach (string[] item in FileHelper.nodes)
            {
                txtOutput.Text += $"{item[0]} {item[1]}\r\n";
            }

        }
    }

}