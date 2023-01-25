namespace ShaftkitMSA2_Parser
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnParse = new System.Windows.Forms.Button();
            this.txtInputFile = new System.Windows.Forms.TextBox();
            this.txtOutputFile = new System.Windows.Forms.TextBox();
            this.fileHelperBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnOpenFileDialog = new System.Windows.Forms.Button();
            this.btnSaveFileDialog = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.Plot1 = new ScottPlot.FormsPlot();
            ((System.ComponentModel.ISupportInitialize)(this.fileHelperBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnParse
            // 
            this.btnParse.Location = new System.Drawing.Point(69, 135);
            this.btnParse.Margin = new System.Windows.Forms.Padding(2);
            this.btnParse.Name = "btnParse";
            this.btnParse.Size = new System.Drawing.Size(247, 74);
            this.btnParse.TabIndex = 0;
            this.btnParse.Text = "Run";
            this.btnParse.UseVisualStyleBackColor = true;
            this.btnParse.Click += new System.EventHandler(this.btnParse_Click);
            // 
            // txtInputFile
            // 
            this.txtInputFile.Location = new System.Drawing.Point(69, 44);
            this.txtInputFile.Margin = new System.Windows.Forms.Padding(2);
            this.txtInputFile.Name = "txtInputFile";
            this.txtInputFile.Size = new System.Drawing.Size(698, 27);
            this.txtInputFile.TabIndex = 1;
            this.txtInputFile.Text = "C:\\Users\\ZacSchramm\\LamaLo\\LL - General\\Software\\Z-Shaftkit\\Modeling\\DYNEXE\\SHAFT" +
    ".OUT";
            this.txtInputFile.TextChanged += new System.EventHandler(this.txtInputFile_TextChanged);
            // 
            // txtOutputFile
            // 
            this.txtOutputFile.Location = new System.Drawing.Point(69, 86);
            this.txtOutputFile.Margin = new System.Windows.Forms.Padding(2);
            this.txtOutputFile.Name = "txtOutputFile";
            this.txtOutputFile.Size = new System.Drawing.Size(698, 27);
            this.txtOutputFile.TabIndex = 2;
            this.txtOutputFile.Text = "c:\\temp\\output.csv";
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(344, 135);
            this.txtOutput.Margin = new System.Windows.Forms.Padding(2);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(485, 74);
            this.txtOutput.TabIndex = 3;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnOpenFileDialog
            // 
            this.btnOpenFileDialog.Location = new System.Drawing.Point(772, 42);
            this.btnOpenFileDialog.Name = "btnOpenFileDialog";
            this.btnOpenFileDialog.Size = new System.Drawing.Size(77, 33);
            this.btnOpenFileDialog.TabIndex = 4;
            this.btnOpenFileDialog.Text = "Browse";
            this.btnOpenFileDialog.UseVisualStyleBackColor = true;
            this.btnOpenFileDialog.Click += new System.EventHandler(this.btnOpenFileDialog_Click);
            // 
            // btnSaveFileDialog
            // 
            this.btnSaveFileDialog.Location = new System.Drawing.Point(772, 80);
            this.btnSaveFileDialog.Name = "btnSaveFileDialog";
            this.btnSaveFileDialog.Size = new System.Drawing.Size(77, 33);
            this.btnSaveFileDialog.TabIndex = 5;
            this.btnSaveFileDialog.Text = "Browse";
            this.btnSaveFileDialog.UseVisualStyleBackColor = true;
            this.btnSaveFileDialog.Click += new System.EventHandler(this.btnSaveFileDialog_Click);
            // 
            // Plot1
            // 
            this.Plot1.Location = new System.Drawing.Point(60, 229);
            this.Plot1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Plot1.Name = "Plot1";
            this.Plot1.Size = new System.Drawing.Size(769, 366);
            this.Plot1.TabIndex = 6;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 654);
            this.Controls.Add(this.Plot1);
            this.Controls.Add(this.btnSaveFileDialog);
            this.Controls.Add(this.btnOpenFileDialog);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.txtOutputFile);
            this.Controls.Add(this.txtInputFile);
            this.Controls.Add(this.btnParse);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Main";
            this.Text = "Shaftkit MSA 2.0 - SHAFT.OUT Parser";
            ((System.ComponentModel.ISupportInitialize)(this.fileHelperBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnParse;
        private TextBox txtInputFile;
        private TextBox txtOutputFile;
        private BindingSource fileHelperBindingSource;
        private TextBox txtOutput;
        private OpenFileDialog openFileDialog1;
        private Button btnOpenFileDialog;
        private Button btnSaveFileDialog;
        private SaveFileDialog saveFileDialog1;
        private ScottPlot.FormsPlot Plot1;
    }
}