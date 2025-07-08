using System.IO;
using System.Xml.Linq;
using System.Printing;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Windows.Documents;
using System.Diagnostics;
using System;
using System.Management.Automation;
using OxpsPrintWrapper;

namespace WinPrintLimiter.PrintControl
{
    internal class XPSprintControl
    {
        const string xpsdocpath = @$"C:\ProgramData\WPLXPS\";
        SharedInt currentCount;
        SharedInt Max;

        List<PrinterContext> printerList = new List<PrinterContext>();

        public XPSprintControl(List<PrinterContext> printersAvaiable,RemoteUserContext userContext,int GlobalLimit)
        {
            currentCount = userContext.GlobalJobsCounter;
            Max = new SharedInt(GlobalLimit);
            printerList = printersAvaiable;


            Thread monitor = new Thread(() =>
            {
                Console.WriteLine("Starting monitoring for xps changes..");
                XPSfilesystemmonitor();
            });
            monitor.SetApartmentState(ApartmentState.STA);
            monitor.Start();
        }


        public void ForwardXPSToPrinter(string printServer,string PrinterName)
        {
            PrintQueue queue;
            if (printServer == "local")
            {
                var printserver = new LocalPrintServer();
                queue = printserver.GetPrintQueue(PrinterName);
            }
            else
            {
                var printserver = new PrintServer(printServer);
                queue = printserver.GetPrintQueue(PrinterName);
            }

            using (Process xpsconvert = new Process())
            {
                xpsconvert.StartInfo.FileName = "C:\\Program Files (x86)\\Windows Kits\\10\\Tools\\x64\\xpsconverter.exe";
                xpsconvert.StartInfo.Arguments = "/XPS /InputFile=C:\\ProgramData\\WPLXPS\\job.xps /OutputFile=C:\\ProgramData\\WPLXPS\\jobc.xps";
                xpsconvert.StartInfo.RedirectStandardError = true;
                xpsconvert.StartInfo.RedirectStandardOutput = true;

                xpsconvert.Start();
                string output = xpsconvert.StandardOutput.ReadToEnd();
                string error = xpsconvert.StandardError.ReadToEnd();
                xpsconvert.WaitForExit();
                Thread.Sleep(5000);

                Debug.WriteLine("Output: " + output);
                Debug.WriteLine("Error: " + error);

            }

            // PrintOxpsToPrinter("C:\\ProgramData\\WPLXPS\\job.xps", PrinterName);
            OxpsPrinter.PrintOxps("C:\\ProgramData\\WPLXPS\\job.xps", $"{printServer}\\{PrinterName}");

           

        }



        public void XPSfilesystemmonitor()
        {
           
                FileSystemWatcher Watcher = new FileSystemWatcher(xpsdocpath, "*.xps");
                Watcher.EnableRaisingEvents = true;
                Watcher.NotifyFilter = NotifyFilters.LastWrite;


                while (true)
                {
                    Watcher.WaitForChanged(WatcherChangeTypes.Changed);

                     XPSreprintForm xPSreprintForm = BuildForm();
                xPSreprintForm.FormClosing += (object sender, FormClosingEventArgs e) =>
                {
                    string selected = xPSreprintForm.printersCombo.SelectedItem?.ToString();
                    Console.WriteLine("SELECTED: " + selected);
                    PrinterContext selectedPrinter;
                    selectedPrinter = printerList.Where(x => x.PrinterName == selected || x.FriendlyName == selected).FirstOrDefault();
                    ForwardXPSToPrinter(selectedPrinter.PrintServer, selectedPrinter.PrinterName);
                };
                xPSreprintForm.ShowDialog();
                     

                    
                }
                


            
        }


        public XPSreprintForm BuildForm()
        {
            XPSreprintForm form = new XPSreprintForm();

            this.printerList.ForEach(x =>
            {
                string text = x.FriendlyName != "" ? x.FriendlyName : x.PrinterName;
                form.printersCombo.Items.Add(text);
            });

            form.used.Text = this.currentCount.value.ToString();
            form.max.Text = this.Max.value.ToString();


            return form;
        }



    }
}
