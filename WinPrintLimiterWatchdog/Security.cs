using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;

namespace WinPrintLimiterWatchdog
{
    class Security
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool SetKernelObjectSecurity(IntPtr Handle, int securityInformation, IntPtr pSecurityDescriptor);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hObject);

        const uint PROCESS_ALL_ACCESS = 0x1F0FFF;
        const int DACL_SECURITY_INFORMATION = 0x00000004;

        public void OwnSec()
        {
            int pid = Process.GetCurrentProcess().Id;
            SetSec(pid);
        }

        public void LimiterSec()
        {
            int pid =  Process.GetProcessesByName("WinPrintLimiter").First().Id;
            SetSec(pid);
        }

        public void TestSec()
        {
            int pid = Process.GetProcessesByName("notepad").First().Id;
            SetSec(pid);
        }

        public void SetSec(int pid)
        {
            // Open current process
            
            IntPtr handle = OpenProcess(PROCESS_ALL_ACCESS, false, pid);
            if (handle == IntPtr.Zero)
            {
                Console.WriteLine("Failed to open process.");
                return;
            }

            try
            {
                // Build a new Discretionary ACL
                RawSecurityDescriptor rsd = new RawSecurityDescriptor(ControlFlags.DiscretionaryAclPresent, null, null, null, new RawAcl(2, 1));

                // Deny PROCESS_TERMINATE to "Users"
                SecurityIdentifier users = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
                rsd.DiscretionaryAcl.InsertAce(0, new CommonAce(
                    AceFlags.None,
                    AceQualifier.AccessDenied,
                    0x0001, // PROCESS_TERMINATE
                    users,
                    false,
                    null));

                // Allow full access to Administrators
                SecurityIdentifier admins = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
                rsd.DiscretionaryAcl.InsertAce(1, new CommonAce(
                    AceFlags.None,
                    AceQualifier.AccessAllowed,
                    (int)PROCESS_ALL_ACCESS,
                    admins,
                    false,
                    null));

                // Convert to binary
                byte[] rawSD = new byte[rsd.BinaryLength];
                rsd.GetBinaryForm(rawSD, 0);
                IntPtr pSD = Marshal.AllocHGlobal(rawSD.Length);
                Marshal.Copy(rawSD, 0, pSD, rawSD.Length);

                // Apply security descriptor
                if (SetKernelObjectSecurity(handle, DACL_SECURITY_INFORMATION, pSD))
                {
                    Console.WriteLine("Process ACL updated successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to set security.");
                }

                Marshal.FreeHGlobal(pSD);
            }
            finally
            {
                CloseHandle(handle);
            }
        }
    }
}