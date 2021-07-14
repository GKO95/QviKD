using System;
using System.Runtime.InteropServices;
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
        private readonly List<HMONITOR> rcHMONITOR = new();
        private readonly List<RECT> rcArrayOfMonitorRects = new();
        private readonly List<DWORD> rcNumberOfPhysicalMonitors = new();
        private readonly List<PHYSICAL_MONITOR[]> rcArrayOfPhysicalMonitors = new();
        private readonly MONITORINFO MonitorInfo = new MONITORINFO()
        {
            cbSize = 40,
            rcMonitor = new RECT(),
            rcWork = new RECT(),
            dwFlags = 0
        };

        private bool MonitorEnum(HMONITOR hMon, HDC hdc, ref RECT lprcMonitor, LPARAM dwData)
        {
            if (hMon == (HMONITOR)(-1))
            {
                Console.Error.WriteLine("INVALID_HANDLE_VALUE");
                return false;
            }
            rcHMONITOR.Add(hMon);
            rcArrayOfMonitorRects.Add(lprcMonitor);
            return true;
        }

        public EnumMonitors()
        {
            Collections.Monitors.Clear();

            if (!User32.EnumDisplayMonitors(HDC.Zero, LPRECT.Zero, MonitorEnum, HANDLE.Zero))
                Console.Error.WriteLine("Unable to retreive a display monitor from the enumeration.");

            /*
                FIRST, GET the number of physical monitors associated to the HMONITOR. The number of the associated physical monitor
                is passed to the "rcNumberOfPhysicalMonitors" list corresponding to the HMONITOR order.
            */
            foreach (HMONITOR hMon in rcHMONITOR.ToArray())
            {
                if (!Dxva2.GetNumberOfPhysicalMonitorsFromHMONITOR(hMon, out DWORD NumberOfPhysicalMonitors))
                {
                    Console.Error.WriteLine($"System Error Code: {Marshal.GetLastWin32Error()}");
                    rcArrayOfMonitorRects.RemoveAt(rcHMONITOR.IndexOf(hMon));
                    rcHMONITOR.Remove(hMon);
                }
                else
                {
                    rcNumberOfPhysicalMonitors.Add(NumberOfPhysicalMonitors);
                }
            }

            /*
                SECOND, ACQUIRE the array of physical monitor information associated to the HMONITOR.
                Each element of the "rcNumberOfPhysicalMonitors" list acts as an array of the
                corresponding HMONITOR containing the "rcNumberOfPhysicalMonitors" number of physical
                monitor information.
            */
            foreach (HMONITOR hMon in rcHMONITOR.ToArray())
            {
                int index = rcHMONITOR.IndexOf(hMon);

                PHYSICAL_MONITOR[] ArrayOfPhysicalMonitors = new PHYSICAL_MONITOR[rcNumberOfPhysicalMonitors[index]];
                if (!Dxva2.GetPhysicalMonitorsFromHMONITOR(rcHMONITOR[index], rcNumberOfPhysicalMonitors[index], ref ArrayOfPhysicalMonitors[0]))
                {
                    Console.Error.WriteLine($"System Error Code: {Marshal.GetLastWin32Error()}");
                    rcNumberOfPhysicalMonitors.RemoveAt(index);
                    rcArrayOfMonitorRects.RemoveAt(index);
                    rcHMONITOR.Remove(hMon);
                }
                else
                {
                    rcArrayOfPhysicalMonitors.Add(ArrayOfPhysicalMonitors);
                }
            }

            /*
                FINALLY, MERGE THE GATHER information on hMonitor, hPhysicalMonitorHandle,
                resolution & position, and more to a single collection "Collections.Monitors".
            */
            foreach (HMONITOR hMon in rcHMONITOR)
            {
                if (!User32.GetMonitorInfoW(hMon, ref MonitorInfo))
                    Console.Error.WriteLine("Failed to retrieve monitor information: unable to identify whether it is the primary or secondary.");

                int index = rcHMONITOR.IndexOf(hMon);
                for (int nMonitor = 0; nMonitor < rcNumberOfPhysicalMonitors[index]; nMonitor++)
                {
                    Collections.Monitors.Add(new Monitor()
                    {
                        hMonitor = hMon,
                        Rect = rcArrayOfMonitorRects[index],
                        hPhysical = rcArrayOfPhysicalMonitors[index][nMonitor].hPhysicalMonitor,
                        Description = rcArrayOfPhysicalMonitors[index][nMonitor].szPhysicalMonitorDescription,
                        IsPrimary = (int)MonitorInfo.dwFlags == (int)MONITORINFOF.PRIMARY
                    });
                }
            }
        }

    }

    public class Monitor
    {
        public HMONITOR hMonitor { get; set; }
        public HANDLE hPhysical { get; set; }
        public RECT Rect { get; set; }

        public string Description { get; set; }
        public bool IsPrimary { get; set; }

        public Monitor Clone()
        {
            return (Monitor)MemberwiseClone();
        }
    }
}
