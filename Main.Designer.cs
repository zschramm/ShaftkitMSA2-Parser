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
            this.txtOutputDir = new System.Windows.Forms.TextBox();
            this.fileHelperBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.txtOutput = new System.Windows.Forms.TextBox();
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
            this.txtInputFile.Size = new System.Drawing.Size(714, 27);
            this.txtInputFile.TabIndex = 1;
            this.txtInputFile.Text = "C:\\Users\\ZacSchramm\\LamaLo\\LL - General\\Software\\Z-Shaftkit\\Modeling\\DYNEXE\\SHAFT" +
    ".OUT";
            // 
            // txtOutputDir
            // 
            this.txtOutputDir.Location = new System.Drawing.Point(69, 86);
            this.txtOutputDir.Margin = new System.Windows.Forms.Padding(2);
            this.txtOutputDir.Name = "txtOutputDir";
            this.txtOutputDir.Size = new System.Drawing.Size(714, 27);
            this.txtOutputDir.TabIndex = 2;
            this.txtOutputDir.Text = "c:\\temp\\Parser";
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(74, 240);
            this.txtOutput.Margin = new System.Windows.Forms.Padding(2);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(624, 311);
            this.txtOutput.TabIndex = 3;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 654);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.txtOutputDir);
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
        private TextBox txtOutputDir;
        private BindingSource fileHelperBindingSource;
        private TextBox txtOutput;
    }
}