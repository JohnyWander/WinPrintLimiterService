using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WinPrintLimiterInstaller
{
    internal static class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();


        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AllocConsole();
            //Unpack unpack = new Unpack();
            //unpack.UnpackWindPrintLimiter();
          //  SC sc = new SC();
            
           // Debug.WriteLine(sc.DeleteServiceIfExists());
           // Debug.WriteLine(sc.CreateService("C:\\Users\\P0407026\\Desktop\\jm\\cs\\WinPrintLimiter\\WinPrintLimiterService\\bin\\Debug\\WinPrintLimiterService.exe"));
           // Debug.WriteLine(sc.SecureService());
            


            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}