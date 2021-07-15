using System.Runtime.InteropServices;

using HMONITOR = System.IntPtr;
using LPARAM = System.IntPtr;
using LPRECT = System.IntPtr;
using HDC = System.IntPtr;

namespace QviKDLib.WinAPI
{
    public static class User32
    {
        [DllImport("User32.dll", EntryPoint = "EnumDisplayMonitors")]
        internal static extern bool EnumDisplayMonitors(HDC hdc, LPRECT lprcClip, MONITORENUMPROC lpfnEnum, LPARAM dwData);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, EntryPoint = "EnumDisplayDevicesW")]
        internal static extern bool EnumDisplayDevicesW(string lpDevice, uint iDevNum, ref DISPLAY_DEVICEW lpDisplayDevice, uint dwFlags);

        [DllImport("User32.dll", CharSet = CharSet.Ansi, EntryPoint = "EnumDisplayDevicesA")]
        internal static extern bool EnumDisplayDevicesA(string lpDevice, uint iDevNum, ref DISPLAY_DEVICEA lpDisplayDevice, uint dwFlags);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetMonitorInfoW")]
        internal static extern bool GetMonitorInfoW(HMONITOR hMonitor, ref MONITORINFO lpmi);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetMonitorInfoW")]
        internal static extern bool GetMonitorInfoW(HMONITOR hMonitor, ref MONITORINFOEXW lpmi);

        public delegate bool MONITORENUMPROC(HMONITOR unnamedParam1, HDC unnamedParam2, ref RECT unnamedParam3, LPARAM unnamedParam4);
    }
}
