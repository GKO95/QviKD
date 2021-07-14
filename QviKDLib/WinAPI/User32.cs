using System;
using System.Runtime.InteropServices;

using HMONITOR = System.IntPtr;
using LPARAM = System.IntPtr;
using LPRECT = System.IntPtr;
using HDC = System.IntPtr;
using DWORD = System.UInt32;

namespace QviKDLib.WinAPI
{
    public static class User32
    {
        [DllImport("User32.dll", CharSet = CharSet.Unicode, EntryPoint = "EnumDisplayMonitors")]
        internal static extern bool EnumDisplayMonitors(HDC hdc, LPRECT lprcClip, MONITORENUMPROC lpfnEnum, LPARAM dwData);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetMonitorInfoW")]
        internal static extern bool GetMonitorInfoW(HMONITOR hMonitor, ref MONITORINFO lpmi);

        public delegate bool MONITORENUMPROC(HMONITOR unnamedParam1, HDC unnamedParam2, ref RECT unnamedParam3, LPARAM unnamedParam4);
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct MONITORINFO
    {
        public DWORD cbSize;
        public RECT rcMonitor;
        public RECT rcWork;
        public DWORD dwFlags;
    }

    [Flags]
    internal enum MONITORINFOF : int
    {
        PRIMARY = 0x00000001,
    }
}
