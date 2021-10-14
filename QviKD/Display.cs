using System;
using System.Text;
using System.Runtime.InteropServices;

using Microsoft.Win32;
using QviKD.WinAPI;

using HMONITOR = System.IntPtr;
using HANDLE = System.IntPtr;

namespace QviKD
{
    public record Display
    {
        /// <summary>
        /// Handle to the display monitor.
        /// </summary>
        internal HMONITOR hMonitor { get; }

        /// <summary>
        /// Handle to the physical display device.
        /// </summary>
        internal HANDLE hPhysical { get; }

        /// <summary>
        /// A string identifying the device name. This is either the adapter device or the monitor device.
        /// </summary>
        internal string DeviceName { get; }

        /// <summary>
        /// Unique identification of hardware device.
        /// </summary>
        internal string DeviceID { get; }

        /// <summary>
        /// Registry key to the hardware device.
        /// </summary>
        internal string DeviceKey { get; }

        /// <summary>
        /// A class that stores EDID and its processed information via members.
        /// </summary>
        internal EDID EDID { get; }

        /// <summary>
        /// A structure that defines a rectangle by the coordinates of its upper-left and lower-right corners.
        /// </summary>
        internal RECT Rect { get; }

        /// <summary>
        /// Text description of the physical monitor.
        /// </summary>
        internal string Description { get; }

        /// <summary>
        /// Information that identifies primary monitor.
        /// </summary>
        internal bool IsPrimary { get; }

        /// <summary>
        /// Information that identifies whether the monitor is available to use a module.
        /// </summary>
        internal bool IsAvailable { get; } = true;

        /*
        internal Window Module
        {
            get { return Module; }
            set
            {
                Module = Activator.CreateInstance((Type)value) as Window;
                if (Activator.CreateInstance((Type)value) is Window Module)
                {
                    obj.Show();
                    return true;
                }
                else return false;
            }
        }
        */

        //internal Window Module;

        //internal void Run(Module module)
        //{
        //    Module = Activator.CreateInstance(module.Type) as Window;
        //    Module.Show();
        //}

        internal Display(HMONITOR hMonitor, PHYSICAL_MONITOR PhysicalMonitor, MONITORINFOEXA MonitorInfoA, DISPLAY_DEVICEA DisplayDeviceA)
        {
            this.hMonitor = hMonitor;

            hPhysical = PhysicalMonitor.hPhysicalMonitor;
            Description = PhysicalMonitor.szPhysicalMonitorDescription;

            Rect = MonitorInfoA.rcMonitor;
            IsPrimary = MonitorInfoA.dwFlags == (int)MONITORINFOF.PRIMARY;

            DeviceName = DisplayDeviceA.DeviceName;
            DeviceID = DisplayDeviceA.DeviceID;
            DeviceKey = DisplayDeviceA.DeviceKey;

            EDID = new EDID(DeviceID);
        }

        ~Display()
        {
            if (!Dxva2.DestroyPhysicalMonitor(hPhysical))
            {
                Console.Error.WriteLine($"Failed to destroy a handle to the physical monitor: {Marshal.GetLastWin32Error()}");
            }
        }

        internal string Print() => string.Format(
            "Device Name:\t{0}\n* HMONITOR:\t\t0x{1:X16}\n* PHYSICAL:\t\t0x{2:X16}\n* RESOLUTION:\t{3}x{4} ({5}, {6})\n* PRIMARY:\t\t{7}",
            DeviceName, hMonitor, hPhysical, Rect.right - Rect.left, Rect.bottom - Rect.top, Rect.left, Rect.top, IsPrimary);
    }

    public record EDID
    {
        /// <summary>
        /// EDID version specified in the aquired EDID information.
        /// </summary>
        public double Version { get; }

        /// <summary>
        /// Name of the display device provided by manufacturer.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Serial number of the display device provided by manufacturer.
        /// </summary>
        public string DisplaySerialNumber { get; }

        /// <summary>
        /// Array of EDID information sliced to appropriate length based on the EDID version.
        /// </summary>
        public byte[] Raw { get; }

        public EDID(string DeviceID)
        {
            try
            {
                Raw = (byte[])Registry.GetValue(
                    $"HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Enum\\DISPLAY\\{DeviceID.Split('#')[1]}\\{DeviceID.Split('#')[2]}\\Device Parameters",
                    "EDID", "");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.Error.WriteLine("Incorrect DeviceID; cannot find EDID registry location.");
                return;
            }

            Version = Convert.ToDouble(Parser(META.EDID_VERSION));
            DisplaySerialNumber = Parser(META.DISPLAY_SERIALNUMBER);
            DisplayName = Parser(META.DISPLAY_NAME);
        }

        /// <summary>
        /// EDID parser in accordance with VESA specification document.
        /// </summary>
        private string Parser(META Metadata)
        {
            string temp;

            switch (Metadata)
            {
                case META.EDID_VERSION:
                    switch (Raw[(byte)BYTE.VERSION_INTEGER])
                    {
                        case 1:
                            return Raw[(byte)BYTE.VERSION_DECIMAL] > 4
                                ? "(invalid)"
                                : $"{Raw[(byte)BYTE.VERSION_INTEGER] + (Raw[(byte)BYTE.VERSION_DECIMAL] / 10.0)}";
                        case 2:
                            return Raw[(byte)BYTE.VERSION_DECIMAL] > 0
                                ? "(invalid)"
                                : $"{Raw[(byte)BYTE.VERSION_INTEGER] + (Raw[(byte)BYTE.VERSION_DECIMAL] / 10.0)}";
                        default:
                            return "(invalid)";
                    }

                case META.DISPLAY_NAME:
                case META.DISPLAY_SERIALNUMBER:
                    for (byte address = (byte)BYTE.DESCRIPTOR1; address < (byte)BYTE.EXTENSION; address += 18)
                    {
                        if (((Metadata == META.DISPLAY_NAME) && (Raw[address + 0x03] == (byte)DESCRIPTOR_TYPE.DISPLAY_NAME)) ||
                            ((Metadata == META.DISPLAY_SERIALNUMBER) && (Raw[address + 0x03] == (byte)DESCRIPTOR_TYPE.DISPLAY_SERIALNUMBER)))
                        {
                            temp = Encoding.ASCII.GetString(Raw[(address + 5)..(address + 18)]).Trim();
                            return temp.Length == 0 ? "(empty)" : temp;
                        }
                    }

                    return "(undefined)";

                default:
                    return null;
            }
        }

        public enum META
        {
            EDID_VERSION,
            DISPLAY_NAME,
            DISPLAY_SERIALNUMBER,
        }

        private enum DESCRIPTOR_TYPE : byte
        {
            DISPLAY_NAME = 0xFC,
            DISPLAY_SERIALNUMBER = 0xFF,
        }

        private enum BYTE : byte
        {
            VERSION_INTEGER = 0x12,
            VERSION_DECIMAL = 0x13,

            DESCRIPTOR1 = 0x36,
            DESCRIPTOR2 = 0x48,
            DESCRIPTOR3 = 0x5A,
            DESCRIPTOR4 = 0x6C,

            EXTENSION = 0x7E,
            CHECKSUM = 0x7F,
        }
    }
}
