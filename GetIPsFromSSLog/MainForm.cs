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
                var ipList = new List<string>();
                var ipTimes = new Dictionary<string, Times>();
                var ipInfoBuilder = new StringBuilder("First time that this IP shows\tIP\t\t\tLast time that this IP shows\tSpan"); // Initial StringBuilder with table header "First time that this IP shows   IP        Last time that this IP shows  Span"
                ipInfoBuilder.AppendLine();
                ipInfoBuilder.AppendLine();

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

                                // Get the UTC time
                                var timeStr = lineText.Substring(0, 19);
                                var timeInUtc = DateTime.SpecifyKind(DateTime.Parse(timeStr), DateTimeKind.Utc);

                                // Append this IP if it wasn't contained in the list
                                if (!ipTimes.ContainsKey(ip))
                                {
                                    ipList.Add(ip);
                                    ipTimes.Add(ip, new Times { FirstTime = timeInUtc, LastTime = timeInUtc });
                                }
                                else
                                {
                                    ipTimes[ip].LastTime = timeInUtc;
                                }
                            }

                            lineNum++;
                        }
                    }
                }

                // Build the result
                foreach (var ip in ipList)
                {
                    var times = ipTimes[ip];
                    ipInfoBuilder.AppendLine($"{times.FirstTime.ToLocalTime()}\t{ip}\t\t{times.LastTime.ToLocalTime()}\t{(times.LastTime - times.FirstTime).Days} day(s)");
                }

                // Get the total counts in the information
                ipInfoBuilder.AppendLine();
                ipInfoBuilder.AppendFormat("Total ip count: {0}", ipList.Count);
                ipInfoBuilder.AppendLine();
                ipInfoBuilder.AppendFormat("Total line count: {0}", lineNum);

                txtIPs.Text = ipInfoBuilder.ToString();
            }
        }
    }

    class Times
    {
        internal DateTime FirstTime { get; set; }

        internal DateTime LastTime { get; set; }
    }
}
