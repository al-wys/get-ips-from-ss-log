using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GetIPsFromSSLog
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Append a new line text into txtIPs with color
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        private void AppendLineWithColor(string text, Color color)
        {
            txtIPs.SelectionStart = txtIPs.TextLength;
            txtIPs.SelectionLength = 0;

            txtIPs.SelectionColor = color;
            txtIPs.AppendText(text);
            txtIPs.SelectionColor = txtIPs.ForeColor;

            // Append new line at the end
            txtIPs.AppendText(Environment.NewLine);
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
                var titleColor = Color.Black;
                AppendLineWithColor("First time that this IP shows\tIP\t\t\tLast time that this IP shows\tSpan", titleColor); // Initial StringBuilder with table header "First time that this IP shows   IP        Last time that this IP shows  Span"
                txtIPs.AppendText(Environment.NewLine);

                var lineNum = 0;

                using (var logFileStream = ofdSsLog.OpenFile())
                {
                    using (var reader = new StreamReader(logFileStream))
                    {
                        var currentUtcTime = DateTime.UtcNow;
                        var warnHourSpan = 12; // Warn the info if time span is less than 12 hours
                        var defaultColor = Color.Gray;
                        var warnColor = Color.OrangeRed;

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
                                    ipTimes.Add(ip, new Times
                                    {
                                        FirstTime = timeInUtc,
                                        LastTime = timeInUtc,
                                        TextColor = (currentUtcTime - timeInUtc).TotalHours > warnHourSpan ? defaultColor : warnColor
                                    });
                                }
                                else
                                {
                                    ipTimes[ip].LastTime = timeInUtc;

                                    if ((currentUtcTime - timeInUtc).TotalHours <= warnHourSpan)
                                    {
                                        ipTimes[ip].TextColor = warnColor;
                                    }
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
                    var daySpan = (times.LastTime - times.FirstTime).TotalDays.ToString("0.00");

                    AppendLineWithColor($"{times.FirstTime.ToLocalTime()}\t{ip}\t\t{times.LastTime.ToLocalTime()}\t{daySpan} day(s)", times.TextColor);
                }

                // Get the total counts in the information
                txtIPs.AppendText(Environment.NewLine);
                AppendLineWithColor($"Total ip count: {ipList.Count}", titleColor);
                AppendLineWithColor($"Total line count: {lineNum}", titleColor);
            }
        }
    }

    class Times
    {
        internal DateTime FirstTime { get; set; }

        internal DateTime LastTime { get; set; }

        internal Color TextColor { get; set; }
    }
}
