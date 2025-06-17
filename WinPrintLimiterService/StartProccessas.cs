using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Threading;

namespace WinPrintLimiterService
{
    class StartProcessAs
    {
        [DllImport("Wtsapi32.dll")]
        static extern void WTSFreeMemory(IntPtr pointer);
        [DllImport("Wtsapi32.dll", SetLastError = true)]
        static extern bool WTSQuerySessionInformation(
    IntPtr hServer,
    int sessionId,
    WTS_INFO_CLASS wtsInfoClass,
    out IntPtr ppBuffer,
    out int pBytesReturned);

        enum WTS_INFO_CLASS
        {
            WTSUserName = 5,
            WTSDomainName = 7,
        }

        string GetUsernameForSession(int sessionId)
        {
            IntPtr buffer;
            int strLen;

            if (WTSQuerySessionInformation(IntPtr.Zero, sessionId, WTS_INFO_CLASS.WTSUserName, out buffer, out strLen) && strLen > 1)
            {
                string user = Marshal.PtrToStringAnsi(buffer);
                WTSFreeMemory(buffer);
                return user;
            }

            return null;
        }
        // P/Invoke declarations
        [DllImport("Wtsapi32.dll")]
        static extern bool WTSQueryUserToken(uint sessionId, out IntPtr Token);

        [DllImport("kernel32.dll")]
        static extern uint WTSGetActiveConsoleSessionId();

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool DuplicateTokenEx(
            IntPtr hExistingToken,
            uint dwDesiredAccess,
            IntPtr lpTokenAttributes,
            int ImpersonationLevel,
            int TokenType,
            out IntPtr phNewToken);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool CreateProcessAsUser(
            IntPtr hToken,
            string lpApplicationName,
            string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, out LUID lpLuid);

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges,
            ref TOKEN_PRIVILEGES NewState, uint Zero, IntPtr Null1, IntPtr Null2);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hObject);

        // Constants
        const uint TOKEN_QUERY = 0x0008;
        const uint TOKEN_DUPLICATE = 0x0002;
        const uint TOKEN_ASSIGN_PRIMARY = 0x0001;
        const uint TOKEN_ADJUST_DEFAULT = 0x0080;
        const uint TOKEN_ADJUST_SESSIONID = 0x0100;
        const uint TOKEN_ADJUST_PRIVILEGES = 0x0020;

        const int SecurityImpersonation = 2;
        const int TokenPrimary = 1;

        const uint CREATE_NEW_CONSOLE = 0x10;
        const uint CREATE_UNICODE_ENVIRONMENT = 0x00000400;
        const uint LOGON_WITH_PROFILE = 0x00000001;
        const uint SE_PRIVILEGE_ENABLED = 0x00000002;
        const uint TOKEN_ALL_ACCESS = 0xF01FF;

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool SetTokenInformation(
    IntPtr TokenHandle,
    TOKEN_INFORMATION_CLASS TokenInformationClass,
    ref uint TokenInformation,
    int TokenInformationLength);

        enum TOKEN_INFORMATION_CLASS
        {
            TokenSessionId = 12,
        }

        // Structs
        [StructLayout(LayoutKind.Sequential)]
        struct STARTUPINFO
        {
            public int cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public uint dwX, dwY, dwXSize, dwYSize, dwXCountChars, dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput, hStdOutput, hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public uint dwProcessId;
            public uint dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct LUID
        {
            public uint LowPart;
            public int HighPart;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct TOKEN_PRIVILEGES
        {
            public uint PrivilegeCount;
            public LUID Luid;
            public uint Attributes;
        }

        void EnablePrivilege(string privilege)
        {
            const uint TOKEN_ADJUST_PRIVILEGES = 0x0020;
            const uint TOKEN_QUERY = 0x0008;
            const uint SE_PRIVILEGE_ENABLED = 0x00000002;

            if (!OpenProcessToken(Process.GetCurrentProcess().Handle, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out IntPtr token))
            {
                int err = Marshal.GetLastWin32Error();
                Log($"OpenProcessToken failed. Error: {err}");
                return;
            }

            if (!LookupPrivilegeValue(null, privilege, out LUID luid))
            {
                int err = Marshal.GetLastWin32Error();
                Log($"LookupPrivilegeValue failed for {privilege}. Error: {err}");
                CloseHandle(token);
                return;
            }

            TOKEN_PRIVILEGES tp = new TOKEN_PRIVILEGES
            {
                PrivilegeCount = 1,
                Luid = luid,
                Attributes = SE_PRIVILEGE_ENABLED
            };

            if (!AdjustTokenPrivileges(token, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero))
            {
                int err = Marshal.GetLastWin32Error();
                Log($"AdjustTokenPrivileges failed for {privilege}. Error: {err}");
            }
            else
            {
                Log($"Privilege {privilege} enabled.");
            }

            CloseHandle(token);
        }

        [DllImport("userenv.dll", SetLastError = true)]
        static extern bool CreateEnvironmentBlock(out IntPtr lpEnvironment, IntPtr hToken, bool bInherit);

        [DllImport("userenv.dll", SetLastError = true)]
        static extern bool DestroyEnvironmentBlock(IntPtr lpEnvironment);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool CreateProcessWithTokenW(
    IntPtr hToken,
    uint dwLogonFlags,
    string lpApplicationName,
    string lpCommandLine,
    uint dwCreationFlags,
    IntPtr lpEnvironment,
    string lpCurrentDirectory,
    ref STARTUPINFO lpStartupInfo,
    out PROCESS_INFORMATION lpProcessInformation);

        const short SW_SHOWNORMAL = 1;

        public void Main()
        {
            EnablePrivilege("SeIncreaseQuotaPrivilege");
            EnablePrivilege("SeAssignPrimaryTokenPrivilege");
            EnablePrivilege("SeImpersonatePrivilege");

            uint sessionId = WTSGetActiveConsoleSessionId();
            Log("Session Id is:" + sessionId);

            string user = GetUsernameForSession((int)sessionId);
            Log(user == null ? "No user logged in for session 1" : $"Session 1 user: {user}");

            if (sessionId == 0xFFFFFFFF)
            {
               Log("No active session found.");
                return;
            }

            Thread.Sleep(2000);



            if (!WTSQueryUserToken(sessionId, out IntPtr userToken))
            {
                Log("WTSQueryUserToken failed. Error: " + Marshal.GetLastWin32Error());
                return;
            }


            
            if (!DuplicateTokenEx(
                userToken,
                TOKEN_ASSIGN_PRIMARY | TOKEN_DUPLICATE | TOKEN_QUERY | TOKEN_ADJUST_DEFAULT | TOKEN_ADJUST_SESSIONID | TOKEN_ALL_ACCESS,
                IntPtr.Zero,
                SecurityImpersonation,
                TokenPrimary,
                out IntPtr duplicatedToken))
            {
                Log("DuplicateTokenEx failed. Error: " + Marshal.GetLastWin32Error());
                CloseHandle(userToken);
                return;
            }

            SetTokenInformation(duplicatedToken, TOKEN_INFORMATION_CLASS.TokenSessionId, ref sessionId, sizeof(uint));

            const uint STARTF_USESHOWWINDOW = 0x00000001;
            STARTUPINFO si = new STARTUPINFO();
            
            si.lpDesktop = @"winsta0\default"; // Required for GUI apps to be visible
                                // Optional: use STARTF_USESHOWWINDOW if needed
            si.wShowWindow = SW_SHOWNORMAL;
            si.dwFlags = STARTF_USESHOWWINDOW;
            si.cb = Marshal.SizeOf(si);
            


            PROCESS_INFORMATION pi;
           
            string workingDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            string appName = null;
            string cmdLine = @"notepad.exe";

            IntPtr env = IntPtr.Zero;
            if (!CreateEnvironmentBlock(out env, duplicatedToken, false))
            {
                Log("CreateEnvironmentBlock failed: " + Marshal.GetLastWin32Error());
            }

            /*
            bool result = CreateProcessWithTokenW(
            duplicatedToken,
            LOGON_WITH_PROFILE,
            appName,
            cmdLine,
            CREATE_UNICODE_ENVIRONMENT | CREATE_NEW_CONSOLE,
            env,
            workingDir,
             ref si,
            out pi);

            */
            bool result = CreateProcessWithTokenW(
    duplicatedToken,
    LOGON_WITH_PROFILE,
    null,
    "notepad.exe",
    CREATE_UNICODE_ENVIRONMENT | CREATE_NEW_CONSOLE,
    env,
    workingDir,
    ref si,
    out pi);




            Log($"StartupInfo: desktop={si.lpDesktop}, session={sessionId}");

            /*bool result = CreateProcessAsUser(
            duplicatedToken,
            null,
            cmdLine,
            IntPtr.Zero,
            IntPtr.Zero,
            false,
            CREATE_UNICODE_ENVIRONMENT | CREATE_NEW_CONSOLE,
            env,
            workingDir,
            ref si,
            out pi);
           */

            if (env != IntPtr.Zero)
            {
                DestroyEnvironmentBlock(env);
            }

            if (result == false)
            {
                Log("CreateProcessAsUser failed. Error: " + Marshal.GetLastWin32Error());
            }
            else
            {
                Log("Process started successfully as user." + pi.dwProcessId);
                //CloseHandle(pi.hProcess);
               // CloseHandle(pi.hThread);
            }

            CloseHandle(userToken);
            CloseHandle(duplicatedToken);
        }


        static void Log(string message)
        {
            // New-EventLog -LogName Application -Source "WinPrintLimiter5"


            EventLog.WriteEntry("WinPrintLimiter5", message, EventLogEntryType.Information);
        }
    }





}

