using System;
using System.Diagnostics;
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
        private readonly List<RECT> rcArrayOfMonitorsRect = new();
        private readonly List<DWORD> rcNumberOfPhysicalMonitors = new();
        private readonly List<PHYSICAL_MONITOR[]> rcArrayOfPhysicalMonitors = new();
        private readonly List<string> rcArrayOfDeviceNames = new();

        /// <summary>
        /// Callback function of MONITORENUMPROC parameter in "User32.EnumDisplayMonitors".
        /// </summary>
        private bool EnumMonitorDelegate(HMONITOR hMon, HDC hdc, ref RECT lprcMonitor, LPARAM dwData)
        {
            if (hMon == (HMONITOR)(-1))
            {
                Console.Error.WriteLine("Unable to retreive the HMONITOR of the display monitor.");
                return false;
            }
            rcArrayOfMonitorsRect.Add(lprcMonitor);
            rcHMONITOR.Add(hMon);
            return true;
        }

        /// <summary>
        /// A monitor enumeration class used whenever enumeration needs to be refreshed; instance is to be discard.
        /// </summary>
        public EnumMonitors()
        {
            Collections.Monitors.Clear();

            EnumHANDLEs();
            EnumEDIDs();

            foreach (HMONITOR hMon in rcHMONITOR)
            {
                int index = rcHMONITOR.IndexOf(hMon);

                for (int nMonitor = 0; nMonitor < rcNumberOfPhysicalMonitors[index]; nMonitor++)
                {
                    Collections.Monitors.Add(new Monitor(hMon,
                        rcArrayOfPhysicalMonitors[index][nMonitor].hPhysicalMonitor,
                        rcArrayOfPhysicalMonitors[index][nMonitor].szPhysicalMonitorDescription)
                    {
                        Rect = rcArrayOfMonitorsRect[index]
                    }); ;
                }
            }
        }

        ~EnumMonitors()
        {
            DebugMessage("EnumMonitor instance destroyed.");
        }

        /// <summary>
        /// Enumerates HMONITORs and HANDLEs to physical display device necessary for DDC/CI.
        /// </summary>
        private void EnumHANDLEs()
        {
            DebugMessage("Begin HANDLE enumeration...");

            MONITORINFOEXW rcMonitorInfoExW = new()
            {
                cbSize = 104,
                rcMonitor = new RECT(),
                rcWork = new RECT(),
                dwFlags = 0,
                szDevice = null
            };

            if (!User32.EnumDisplayMonitors(HDC.Zero, LPRECT.Zero, EnumMonitorDelegate, HANDLE.Zero))
                Console.Error.WriteLine("Unable to retreive a display monitor from the enumeration.");

            foreach (HMONITOR hMon in rcHMONITOR.ToArray())
            {
                int index = rcHMONITOR.IndexOf(hMon);

                if (!Dxva2.GetNumberOfPhysicalMonitorsFromHMONITOR(hMon, out DWORD NumberOfPhysicalMonitors))
                {
                    Console.Error.WriteLine($"System Error Code: {Marshal.GetLastWin32Error()}");
                    rcArrayOfMonitorsRect.RemoveAt(index);
                    rcHMONITOR.Remove(hMon);
                }
                else rcNumberOfPhysicalMonitors.Add(NumberOfPhysicalMonitors);
            }

            foreach (HMONITOR hMon in rcHMONITOR.ToArray())
            {
                int index = rcHMONITOR.IndexOf(hMon);

                PHYSICAL_MONITOR[] ArrayOfPhysicalMonitors = new PHYSICAL_MONITOR[rcNumberOfPhysicalMonitors[index]];
                if (!Dxva2.GetPhysicalMonitorsFromHMONITOR(rcHMONITOR[index], rcNumberOfPhysicalMonitors[index], ref ArrayOfPhysicalMonitors[0]))
                {
                    Console.Error.WriteLine($"System Error Code: {Marshal.GetLastWin32Error()}");
                    rcNumberOfPhysicalMonitors.RemoveAt(index);
                    rcArrayOfMonitorsRect.RemoveAt(index);
                    rcHMONITOR.Remove(hMon);
                }
                else
                {
                    if (!User32.GetMonitorInfoW(hMon, ref rcMonitorInfoExW))
                        Console.Error.WriteLine("Failed to retrieve monitor information.");
                       
                    rcArrayOfPhysicalMonitors.Add(ArrayOfPhysicalMonitors);
                    rcArrayOfDeviceNames.Add(rcMonitorInfoExW.szDevice);
                }
            }
        }

        /// <summary>
        /// Enumerates EDID that identifies a display device information and capabilities.
        /// </summary>
        private void EnumEDIDs()
        {
            DebugMessage("Begin EDID enumeration...");

            DISPLAY_DEVICEW devDISPLAY = new() { cb = 840, DeviceName = null, DeviceString = null, StateFlags = 0, DeviceID = null, DeviceKey = null };

            for (DWORD index = 0; ;)
            {
                if (User32.EnumDisplayDevicesW(null, index, ref devDISPLAY, 1))
                {
                    for (DWORD idx = 0; User32.EnumDisplayDevicesW(devDISPLAY.DeviceName, idx, ref devDISPLAY, 1); idx++)
                    {
                        
                    }
                    index++;
                    continue;
                }
                break;
            }
        }

        private void DebugMessage(string msg)
        {
            Debug.WriteLine($"'{GetType().Name}.cs' {msg}");
        }
    }

    public record Monitor
    {
        /// <summary>
        /// A handle to the display monitor.
        /// </summary>
        public HMONITOR hMonitor { get; }

        /// <summary>
        /// A handle to the physical display device.
        /// </summary>
        public HANDLE hPhysical { get; }

        public string DeviceName { get; }

        /// <summary>
        /// A handle to the physical display device.
        /// </summary>
        public EDID EDID { get; set; }

        /// <summary>
        /// A structure that defines a rectangle by the coordinates of its upper-left and lower-right corners.
        /// </summary>
        public RECT Rect { get; set; }

        /// <summary>
        /// Text description of the physical monitor.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// An information that identifies primary monitor.
        /// </summary>
        public bool IsPrimary { get; set; }

        public Monitor(HMONITOR hMonitor, HANDLE hPhysical, string Description)
        {
            this.hMonitor = hMonitor;
            this.hPhysical = hPhysical;
            this.Description = Description;
        }

        ~Monitor()
        {
            if (!Dxva2.DestroyPhysicalMonitor(hPhysical))
                Console.Error.WriteLine($"Failed to destroy a handle to the physical monitor: {Marshal.GetLastWin32Error()}");
        }
    }

    public record EDID {

        /// <summary>
        /// Hidden readonly field containing aquired EDID.
        /// </summary>
        private readonly byte[] _edid = new byte[256];

        /// <summary>
        /// EDID version specified in the aquired EDID information.
        /// </summary>
        public double Version { get; }

        /// <summary>
        /// EDID length based on the EDID version.
        /// </summary>
        public int Length { get => Version >= 2.0 ? 256 : 128; }

        /// <summary>
        /// Array of EDID information sliced to appropriate length based on the EDID version.
        /// </summary>
        public byte[] Raw { get => _edid[0..Length]; }

        public EDID(byte[] edid)
        {
            _edid = edid;
        }
    }
}
