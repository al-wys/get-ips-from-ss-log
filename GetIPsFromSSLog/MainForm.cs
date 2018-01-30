using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace GetIPsFromSSLog
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnChoseFile_Click(object sender, EventArgs e)
        {
            ofdSsLog.ShowDialog();
        }

        private void ofdSsLog_FileOk(object sender, CancelEventArgs e)
        {
            if (ofdSsLog.CheckFileExists)
            {
                var ipSet = new HashSet<string>();
                var ipInfoBuilder = new StringBuilder();

                var lineNum = 0;

                using (var logFileStream = ofdSsLog.OpenFile())
                {
                    using (var reader = new StreamReader(logFileStream))
                    {
                        while (!reader.EndOfStream)
                        {
                            // sample: 2018-01-29 13:32:10 INFO     connecting x.x.x.x:xxxx from x.x.x.x:xxxx
                            var lineText = reader.ReadLine();
                            var startIndex = lineText.IndexOf("from ");

                            if (startIndex > -1)
                            {
                                startIndex += 5;
                                var endIndex = lineText.LastIndexOf(':');

                                var ip = lineText.Substring(startIndex, endIndex - startIndex);

                                // Append this IP if it wasn't contained in the list
                                if (!ipSet.Contains(ip))
                                {
                                    ipSet.Add(ip);
                                    ipInfoBuilder.AppendLine(ip);
                                }
                            }

                            lineNum++;
                        }
                    }
                }

                // Get the line number in the information
                ipInfoBuilder.AppendLine();
                ipInfoBuilder.AppendFormat("Total line number: {0}", lineNum);

                txtIPs.Text = ipInfoBuilder.ToString();
            }
        }
    }
}
