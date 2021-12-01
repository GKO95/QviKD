using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using QviKD.WinAPI;
using QviKD.Types;

using HMONITOR = System.IntPtr;
using LPRECT = System.IntPtr;
using LPARAM = System.IntPtr;
using HANDLE = System.IntPtr;
using HDC = System.IntPtr;
using DWORD = System.UInt32;

namespace QviKD.Functions
{
    class EnumDisplays
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
        /// A monitor enumeration class used whenever enumeration needs to be refreshed; instance is to be discard.
        /// </summary>
        public EnumDisplays()
        {
            Database.Displays.Clear();

            // Enumerate physical monitors.
            EnumPhysicals();

            // Enumerate display devices.
            EnumDevices();

            // Match physical monitors and display devices.
            for (int nDisplay = 0, index = 0; nDisplay < _MONITORINFOEXAs.Count; nDisplay++)
            {
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
        }

        ~EnumDisplays()
        {
            DebugMessage("EnumMonitor instance destroyed.");
        }

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
        /// Enumerates HMONITORs and HANDLEs to physical display device necessary for DDC/CI.
        /// </summary>
        private void EnumPhysicals()
        {
            DebugMessage("Begin monitor enumeration...");

            // Create an instance that can store display monitor screen information.
            MONITORINFOEXA MonitorInfoExA = new()
            {
                cbSize = 72,
                rcMonitor = new RECT(),
                rcWork = new RECT(),
                dwFlags = 0,
                szDevice = null
            };

            // Identify display monitor screens via EnumMonitors callback function.
            if (!User32.EnumDisplayMonitors(HDC.Zero, LPRECT.Zero, EnumMonitors, HANDLE.Zero))
            {
                Console.Error.WriteLine("Unable to retreive a display monitor from the enumeration.");
            }

            foreach (HMONITOR hMon in _HMONITORs.ToArray())
            {
                int index = _HMONITORs.IndexOf(hMon);

                // Find out the number of physical monitors associated to the corresponding display monitor screen.
                if (!Dxva2.GetNumberOfPhysicalMonitorsFromHMONITOR(hMon, out DWORD NumberOfPhysicalMonitors))
                {
                    Console.Error.WriteLine($"System Error Code: {Marshal.GetLastWin32Error()}");
                    _ = _HMONITORs.Remove(hMon);
                }
                else
                {
                    if (!User32.GetMonitorInfoA(hMon, ref MonitorInfoExA))
                    {
                        Console.Error.WriteLine("Failed to retrieve monitor information.");
                    }

                    PHYSICAL_MONITOR[] ArrayOfPhysicalMonitors = new PHYSICAL_MONITOR[NumberOfPhysicalMonitors];
                    DISPLAY_DEVICEA[] ArrayOfDisplayDeviceA = new DISPLAY_DEVICEA[NumberOfPhysicalMonitors];

                    // Retrieve handles to the associated physical monitors.
                    if (!Dxva2.GetPhysicalMonitorsFromHMONITOR(_HMONITORs[index], NumberOfPhysicalMonitors, ref ArrayOfPhysicalMonitors[0]))
                    {
                        Console.Error.WriteLine($"System Error Code: {Marshal.GetLastWin32Error()}");
                        _ = _HMONITORs.Remove(hMon);
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

            // Create an instance that can store display monitor device information.
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
                // Retrieve display monitor device information.
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

        /// <summary>
        /// Print message for debugging; DEBUG-mode exclusive.
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG")]
        private void DebugMessage(string msg)
        {
            System.Diagnostics.Debug.WriteLine($"'{GetType().Name}.cs' {msg}");
        }
    }
}
