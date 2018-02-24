namespace GetIPsFromSSLog
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ofdSsLog = new System.Windows.Forms.OpenFileDialog();
            this.btnChoseFile = new System.Windows.Forms.Button();
            this.txtIPs = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // ofdSsLog
            // 
            this.ofdSsLog.FileName = "SS Log";
            this.ofdSsLog.Filter = "Log files (*.log)|*.log";
            this.ofdSsLog.FileOk += new System.ComponentModel.CancelEventHandler(this.ofdSsLog_FileOk);
            // 
            // btnChoseFile
            // 
            this.btnChoseFile.Location = new System.Drawing.Point(61, 46);
            this.btnChoseFile.Name = "btnChoseFile";
            this.btnChoseFile.Size = new System.Drawing.Size(108, 45);
            this.btnChoseFile.TabIndex = 0;
            this.btnChoseFile.Text = "Chose File";
            this.btnChoseFile.UseVisualStyleBackColor = true;
            this.btnChoseFile.Click += new System.EventHandler(this.btnChoseFile_Click);
            // 
            // txtIPs
            // 
            this.txtIPs.Location = new System.Drawing.Point(25, 115);
            this.txtIPs.Multiline = true;
            this.txtIPs.Name = "txtIPs";
            this.txtIPs.ReadOnly = true;
            this.txtIPs.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtIPs.Size = new System.Drawing.Size(935, 599);
            this.txtIPs.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1033, 769);
            this.Controls.Add(this.txtIPs);
            this.Controls.Add(this.btnChoseFile);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Get IPs from SS log";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofdSsLog;
        private System.Windows.Forms.Button btnChoseFile;
        private System.Windows.Forms.RichTextBox txtIPs;
    }
}

