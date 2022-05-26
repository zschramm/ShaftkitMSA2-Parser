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
            this.btnParse = new System.Windows.Forms.Button();
            this.txtInputFile = new System.Windows.Forms.TextBox();
            this.txtOutputDir = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnParse
            // 
            this.btnParse.Location = new System.Drawing.Point(86, 169);
            this.btnParse.Name = "btnParse";
            this.btnParse.Size = new System.Drawing.Size(309, 93);
            this.btnParse.TabIndex = 0;
            this.btnParse.Text = "Run";
            this.btnParse.UseVisualStyleBackColor = true;
            this.btnParse.Click += new System.EventHandler(this.btnParse_Click);
            // 
            // txtInputFile
            // 
            this.txtInputFile.Location = new System.Drawing.Point(86, 55);
            this.txtInputFile.Name = "txtInputFile";
            this.txtInputFile.Size = new System.Drawing.Size(892, 31);
            this.txtInputFile.TabIndex = 1;
            this.txtInputFile.Text = "C:\\Users\\ZacSchramm\\LamaLo\\LL - General\\Software\\Z-Shaftkit\\Modeling\\DYNEXE\\SHAFT" +
    ".OUT";
            // 
            // txtOutputDir
            // 
            this.txtOutputDir.Location = new System.Drawing.Point(86, 108);
            this.txtOutputDir.Name = "txtOutputDir";
            this.txtOutputDir.Size = new System.Drawing.Size(892, 31);
            this.txtOutputDir.TabIndex = 2;
            this.txtOutputDir.Text = "c:\\temp\\Parser";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 310);
            this.Controls.Add(this.txtOutputDir);
            this.Controls.Add(this.txtInputFile);
            this.Controls.Add(this.btnParse);
            this.Name = "Main";
            this.Text = "Shaftkit MSA 2.0 - SHAFT.OUT Parser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnParse;
        private TextBox txtInputFile;
        private TextBox txtOutputDir;
    }
}