using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Microsoft.Win32;
using QviKDLib.WinAPI;

using HMONITOR = System.IntPtr;
using LPRECT = System.IntPtr;
using LPARAM = System.IntPtr;
using HANDLE = System.IntPtr;
using HDC = System.IntPtr;

using DWORD = System.UInt32;

namespace QviKDLib
{
    public class EnumDisplays
    {
        /// <summary>
        /// List of handles to a display monitor; aka HMONITOR.
        /// </summary>
        private readonly List<HMONITOR> _HMONITORs = new();

        /// <summary>
        /// List of handles to a "physical" display device and its description.
        /// </summary>
        private readonly List<PHYSICAL_MONITOR[]> _HPHYSICALs = new();

        /// <summary>
        /// List of display monitor information including "szDevice" device name and its "rcMonitor" RECT position.
        /// </summary>
        private readonly List<MONITORINFOEXA> _MONITORINFOEXAs = new();

        /// <summary>
        /// List of display device information including "DeviceName" device name and its "DeviceID" device registry key.
        /// </summary>
        private readonly List<DISPLAY_DEVICEA[]> _DISPLAY_DEVICEAs = new();

        /// <summary>
        /// Callback function of MONITORENUMPROC parameter in "User32.EnumDisplayMonitors".
        /// </summary>
        private bool EnumMonitors(HMONITOR hMon, HDC hdc, ref RECT lprcMonitor, LPARAM dwData)
        {
            if (hMon == (HMONITOR)(-1))
            {
                Console.Error.WriteLine("Unable to retreive the HMONITOR of the display monitor.");
                return false;
            }
            _HMONITORs.Add(hMon);
            return true;
        }

        /// <summary>
        /// A monitor enumeration class used whenever enumeration needs to be refreshed; instance is to be discard.
        /// </summary>
        public EnumDisplays()
        {
            Database.Displays.Clear();

            EnumPhysicals();
            EnumDevices();

            for (int nDisplay = 0, index = 0; nDisplay < _MONITORINFOEXAs.Count; nDisplay++)
                for (int nMonitor = 0; nMonitor < _HPHYSICALs[nDisplay].Length; nMonitor++)
                {
                    Database.Displays.Add(new Display(
                        _HMONITORs[nDisplay],
                        _HPHYSICALs[nDisplay][nMonitor],
                        _MONITORINFOEXAs[nDisplay],
                        _DISPLAY_DEVICEAs[nDisplay][nMonitor]
                        ));
                    DebugMessage($"Database.Displays[{index++}]\n{Database.Displays[^1].Print()}");
                }
        }

        ~EnumDisplays()
        {
            DebugMessage("EnumMonitor instance destroyed.");
        }

        /// <summary>
        /// Enumerates HMONITORs and HANDLEs to physical display device necessary for DDC/CI.
        /// </summary>
        private void EnumPhysicals()
        {
            DebugMessage("Begin monitor enumeration...");

            MONITORINFOEXA MonitorInfoExA = new()
            {
                cbSize = 72,
                rcMonitor = new RECT(),
                rcWork = new RECT(),
                dwFlags = 0,
                szDevice = null
            };

            if (!User32.EnumDisplayMonitors(HDC.Zero, LPRECT.Zero, EnumMonitors, HANDLE.Zero))
                Console.Error.WriteLine("Unable to retreive a display monitor from the enumeration.");

            foreach (HMONITOR hMon in _HMONITORs.ToArray())
            {
                int index = _HMONITORs.IndexOf(hMon);

                if (!Dxva2.GetNumberOfPhysicalMonitorsFromHMONITOR(hMon, out DWORD NumberOfPhysicalMonitors))
                {
                    Console.Error.WriteLine($"System Error Code: {Marshal.GetLastWin32Error()}");
                    _HMONITORs.Remove(hMon);
                }
                else
                {
                    if (!User32.GetMonitorInfoA(hMon, ref MonitorInfoExA))
                        Console.Error.WriteLine("Failed to retrieve monitor information.");

                    PHYSICAL_MONITOR[] ArrayOfPhysicalMonitors = new PHYSICAL_MONITOR[NumberOfPhysicalMonitors];
                    DISPLAY_DEVICEA[] ArrayOfDisplayDeviceA = new DISPLAY_DEVICEA[NumberOfPhysicalMonitors];

                    if (!Dxva2.GetPhysicalMonitorsFromHMONITOR(_HMONITORs[index], NumberOfPhysicalMonitors, ref ArrayOfPhysicalMonitors[0]))
                    {
                        Console.Error.WriteLine($"System Error Code: {Marshal.GetLastWin32Error()}");
                        _HMONITORs.Remove(hMon);
                    }
                    else
                    {
                        _HPHYSICALs.Add(ArrayOfPhysicalMonitors);
                        _DISPLAY_DEVICEAs.Add(ArrayOfDisplayDeviceA);
                        _MONITORINFOEXAs.Add(MonitorInfoExA.Clone());
                    }
                }
            }

            DebugMessage("...monitor enumeration complete!");
        }

        /// <summary>
        /// Enumerates display devices that can later linked to the display monitor information.
        /// </summary>
        private void EnumDevices()
        {
            DebugMessage("Begin device enumeration...");

            DISPLAY_DEVICEA DisplayDeviceA = new()
            {
                cb = 424,
                DeviceName = null,
                DeviceString = null,
                StateFlags = 0,
                DeviceID = null,
                DeviceKey = null
            };

            for (DWORD index = 0; ;)
            {
                if (User32.EnumDisplayDevicesA(null, index, ref DisplayDeviceA, 1))
                {
                    for (DWORD idx = 0; User32.EnumDisplayDevicesA(DisplayDeviceA.DeviceName, idx, ref DisplayDeviceA, 1); idx++)
                    {
                        for (int nDisplay = 0; nDisplay < _MONITORINFOEXAs.Count; nDisplay++)
                            for (int nMonitor = 0; nMonitor < _HPHYSICALs[nDisplay].Length; nMonitor++)
                                if (DisplayDeviceA.DeviceName == $"{_MONITORINFOEXAs[nDisplay].szDevice}\\Monitor{nMonitor}")
                                {
                                    _DISPLAY_DEVICEAs[nDisplay][nMonitor] = DisplayDeviceA;
                                    break;
                                }
                    }
                    index++;
                    continue;
                }
                break;
            }

            DebugMessage("...device enumeration complete!");
        }

        private void DebugMessage(string msg)
        {
            Debug.WriteLine($"'{GetType().Name}.cs' {msg}");
        }
    }

    public record Display
    {
        /// <summary>
        /// A handle to the display monitor.
        /// </summary>
        public HMONITOR hMonitor { get; }

        /// <summary>
        /// A handle to the physical display device.
        /// </summary>
        public HANDLE hPhysical { get; }

        /// <summary>
        /// A string identifying the device name. This is either the adapter device or the monitor device.
        /// </summary>
        public string DeviceName { get; }

        /// <summary>
        /// Unique identification of hardware device.
        /// </summary>
        public string DeviceID { get; }

        /// <summary>
        /// A registry key to the hardware device.
        /// </summary>
        public string DeviceKey { get; }

        /// <summary>
        /// A handle to the physical display device.
        /// </summary>
        public EDID EDID { get; }

        /// <summary>
        /// A structure that defines a rectangle by the coordinates of its upper-left and lower-right corners.
        /// </summary>
        public RECT Rect { get; }

        /// <summary>
        /// Text description of the physical monitor.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// An information that identifies primary monitor.
        /// </summary>
        public bool IsPrimary { get; }

        public Display(HMONITOR hMonitor, PHYSICAL_MONITOR PhysicalMonitor, MONITORINFOEXA MonitorInfoA, DISPLAY_DEVICEA DisplayDeviceA)
        {
            this.hMonitor = hMonitor;

            hPhysical = PhysicalMonitor.hPhysicalMonitor;
            Description = PhysicalMonitor.szPhysicalMonitorDescription;

            Rect = MonitorInfoA.rcMonitor;
            IsPrimary = MonitorInfoA.dwFlags == (int)MONITORINFOF.PRIMARY;

            DeviceName = DisplayDeviceA.DeviceName;
            DeviceID = DisplayDeviceA.DeviceID;
            DeviceKey = DisplayDeviceA.DeviceKey;

            EDID = new EDID(
                (byte[])Registry.GetValue(
                    $"HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Enum\\DISPLAY\\{DeviceID.Split('#')[1]}\\{DeviceID.Split('#')[2]}\\Device Parameters",
                    "EDID", "")
                );
        }

        ~Display()
        {
            if (!Dxva2.DestroyPhysicalMonitor(hPhysical))
                Console.Error.WriteLine($"Failed to destroy a handle to the physical monitor: {Marshal.GetLastWin32Error()}");
        }

        public string Print() => string.Format(
            "Device Name:\t{0}\n* HMONITOR:\t\t0x{1:X16}\n* PHYSICAL:\t\t0x{2:X16}\n* RESOLUTION:\t{3}x{4} ({5}, {6})\n* PRIMARY:\t\t{7}", 
            DeviceName, hMonitor, hPhysical, Rect.right - Rect.left, Rect.bottom - Rect.top, Rect.left, Rect.top, IsPrimary);
    }

    public record EDID {

        /// <summary>
        /// EDID version specified in the aquired EDID information.
        /// </summary>
        public double Version { get; }

        /// <summary>
        /// Array of EDID information sliced to appropriate length based on the EDID version.
        /// </summary>
        public byte[] Raw { get; }

        public EDID(byte[] edid)
        {
            Raw = edid;
            Version = Raw[(byte)FORMAT.VERSION_HBYTE] + (Raw[(byte)FORMAT.VERSION_LBYTE] / 10.0);
        }

        enum FORMAT : byte
        { 
            VERSION_HBYTE = 18,
            VERSION_LBYTE = 19,
        }
    }
}
