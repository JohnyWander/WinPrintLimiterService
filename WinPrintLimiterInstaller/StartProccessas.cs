using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace WinPrintLimiterInstaller {
class StartProcessAs
{
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

    const uint CREATE_UNICODE_ENVIRONMENT = 0x00000400;

    const uint SE_PRIVILEGE_ENABLED = 0x00000002;

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

    public void EnablePrivilege(string privilegeName)
    {
        OpenProcessToken(Process.GetCurrentProcess().Handle, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out IntPtr tokenHandle);
        LookupPrivilegeValue(null, privilegeName, out LUID luid);

        TOKEN_PRIVILEGES tp = new TOKEN_PRIVILEGES
        {
            PrivilegeCount = 1,
            Luid = luid,
            Attributes = SE_PRIVILEGE_ENABLED
        };

        AdjustTokenPrivileges(tokenHandle, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
        CloseHandle(tokenHandle);
    }

    public void Main()
    {
        EnablePrivilege("SeIncreaseQuotaPrivilege");
        EnablePrivilege("SeAssignPrimaryTokenPrivilege");

        uint sessionId = WTSGetActiveConsoleSessionId();

        if (sessionId == 0xFFFFFFFF)
        {
            Console.WriteLine("No active session found.");
            return;
        }

        if (!WTSQueryUserToken(sessionId, out IntPtr userToken))
        {
            Console.WriteLine("WTSQueryUserToken failed. Error: " + Marshal.GetLastWin32Error());
            return;
        }

        if (!DuplicateTokenEx(
            userToken,
            TOKEN_ASSIGN_PRIMARY | TOKEN_DUPLICATE | TOKEN_QUERY | TOKEN_ADJUST_DEFAULT | TOKEN_ADJUST_SESSIONID,
            IntPtr.Zero,
            SecurityImpersonation,
            TokenPrimary,
            out IntPtr duplicatedToken))
        {
            Console.WriteLine("DuplicateTokenEx failed. Error: " + Marshal.GetLastWin32Error());
            CloseHandle(userToken);
            return;
        }

        STARTUPINFO si = new STARTUPINFO();
        si.cb = Marshal.SizeOf(si);
        si.lpDesktop = @"winsta0\default"; // Required for GUI apps to be visible

        PROCESS_INFORMATION pi;

        string command = "notepad.exe";

        bool result = CreateProcessAsUser(
            duplicatedToken,
            null,
            command,
            IntPtr.Zero,
            IntPtr.Zero,
            false,
            CREATE_UNICODE_ENVIRONMENT,
            IntPtr.Zero,
            null,
            ref si,
            out pi);

        if (!result)
        {
            Console.WriteLine("CreateProcessAsUser failed. Error: " + Marshal.GetLastWin32Error());
        }
        else
        {
            Console.WriteLine("Process started successfully as user.");
            CloseHandle(pi.hProcess);
            CloseHandle(pi.hThread);
        }

        CloseHandle(userToken);
        CloseHandle(duplicatedToken);
    }
}
}