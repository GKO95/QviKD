using System.Runtime.InteropServices;

using HANDLE = System.IntPtr;
using DWORD = System.UInt32;

namespace QviKDLib.WinAPI
{
    public static class Kernel32
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "CreateFileW")]
        internal static extern HANDLE CreatFileW(string lpFileName, DWORD dwDesiredAccess, DWORD dwShareMode, ref SECURITY_ATTRIBUTES lpSecurityAttributes, DWORD dwCreationDisposition, DWORD dwFlagsAndAttributes, HANDLE hTemplatefile);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "CreateFileW")]
        internal static extern HANDLE CreatFileW(string lpFileName, DWORD dwDesiredAccess, DWORD dwShareMode, int nulllpSecurityAttributes, DWORD dwCreationDisposition, DWORD dwFlagsAndAttributes, HANDLE hTemplatefile);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "WriteFile")]
        internal static extern bool WriteFile(HANDLE hFile, ref byte lpBuffer, DWORD nNumberOfBytesToWrite, out DWORD lpNumberOfBytesWritten, [In] HANDLE nulllpOverlapped);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "ReadFile")]
        internal static extern bool ReadFile(HANDLE hFile, ref byte lpBuffer, DWORD nNumberOfBytesToRead, out DWORD lpNumberOfBytesRead, [In] HANDLE nulllpOverlapped);

        [DllImport("Kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "CloseHandle")]
        internal static extern bool CloseHandle(HANDLE hMonitor);
    }
}
