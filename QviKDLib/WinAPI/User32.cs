using System.Runtime.InteropServices;

using HMONITOR = System.IntPtr;
using LPARAM = System.IntPtr;
using LPRECT = System.IntPtr;
using HDC = System.IntPtr;

namespace QviKDLib.WinAPI
{
    public static class User32
    {
        [DllImport("User32.dll", CharSet = CharSet.Unicode, EntryPoint = "EnumDisplayMonitors")]
        internal static extern bool EnumDisplayMonitors(HDC hdc, LPRECT lprcClip, MONITORENUMPROC lpfnEnum, LPARAM dwData);

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
}
