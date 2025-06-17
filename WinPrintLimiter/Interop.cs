using System.Runtime.InteropServices;
namespace WinPrintLimiter
{
    internal static class Interop
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool AllocConsole();
    }
}
