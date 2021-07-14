using System.Collections.Generic;

using QviKDLib.WinAPI;

using HMONITOR = System.IntPtr;
using LPRECT = System.IntPtr;
using LPARAM = System.IntPtr;
using HANDLE = System.IntPtr;
using HDC = System.IntPtr;

using DWORD = System.UInt32;

namespace QviKDLib
{
    public class EnumMonitors
    {
        readonly List<HMONITOR> rcHMONITOR = new();
        readonly List<DWORD> rcNumberOfPhysicalMonitors = new();
        readonly List<PHYSICAL_MONITOR[]> rcArrayOfPhysicalMonitors = new();

        private bool MonitorEnum(HMONITOR hMon, HDC hdc, ref RECT lprcMonitor, LPARAM dwData)
        {
            return true;
        }

        public EnumMonitors()
        {
            User32.EnumDisplayMonitors(HDC.Zero, LPRECT.Zero, MonitorEnum, HANDLE.Zero);
        }


    }
}
