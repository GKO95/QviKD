using System.Runtime.InteropServices;

using LRESULT = System.Int64;
using DWORD = System.UInt32;
using HKEY = System.IntPtr;

namespace QviKD.WinAPI
{
    internal static class Advapi32
    {
        [DllImport("Advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "RegQueryValueExW")]
        internal static extern LRESULT RegQueryValueExW(HKEY hKey, string lpValueName, DWORD lpReserved, ref DWORD lpType, ref byte lpData, ref DWORD lpcbData);
    }
}
