using System;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Microsoft.Win32;
using QviKD.WinAPI;

using HMONITOR = System.IntPtr;
using HANDLE = System.IntPtr;

namespace QviKD.Types
{
    public record Display : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
        /// Identifies whether the monitor is in use by a module.
        /// </summary>
        public bool InUse
        {
            get => _InUse;
            set
            {
                _InUse = value;
                OnPropertyChanged();
            }
        }
        private bool _InUse = false;

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

        /// <summary>
        /// Triggers PropertyChanged event that is sent to the targeted binding client.
        /// </summary>
        private void OnPropertyChanged([CallerMemberName] string memberName = "")
        {
            // CallerMemberName attribute assigns the calling member as its arugment.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }

        /// <summary>
        /// Return a string of monitor information.
        /// </summary>
        internal string Print() => string.Format(
            "Device Name:\t{0}\n* HMONITOR:\t\t0x{1:X16}\n* PHYSICAL:\t\t0x{2:X16}\n* RESOLUTION:\t{3}x{4} ({5}, {6})\n* PRIMARY:\t\t{7}",
            DeviceName, hMonitor, hPhysical, Rect.right - Rect.left, Rect.bottom - Rect.top, Rect.left, Rect.top, IsPrimary);
    }

    public record EDID
    {
        /// <summary>
        /// EDID version specified in the aquired EDID information.
        /// </summary>
        public double Version
        {
            get
            {
                return Raw[(byte)BYTE.VERSION_INTEGER] switch
                {
                    1 => Raw[(byte)BYTE.VERSION_DECIMAL] > 4 ? double.NaN : Raw[(byte)BYTE.VERSION_INTEGER] + (Raw[(byte)BYTE.VERSION_DECIMAL] / 10.0),
                    2 => Raw[(byte)BYTE.VERSION_DECIMAL] > 0 ? double.NaN : Raw[(byte)BYTE.VERSION_INTEGER] + (Raw[(byte)BYTE.VERSION_DECIMAL] / 10.0),
                    _ => double.NaN,
                };
            }
        }

        /// <summary>
        /// Name of the display device provided by manufacturer.
        /// </summary>
        public string DisplayName
        {
            get
            {
                for (byte address = (byte)BYTE.DESCRIPTOR1; address < (byte)BYTE.EXTENSION; address += 18)
                {
                    if (Raw[address + 0x03] == (byte)DESCRIPTOR_TYPE.DISPLAY_NAME)
                    {
                        string strTemp = Encoding.ASCII.GetString(Raw[(address + 5)..(address + 18)]).Trim();
                        return strTemp.Length == 0 ? string.Empty : strTemp;
                    }
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Serial number of the display device provided by manufacturer.
        /// </summary>
        public string DisplaySerialNumber
        {
            get
            {
                for (byte address = (byte)BYTE.DESCRIPTOR1; address < (byte)BYTE.EXTENSION; address += 18)
                {
                    if (Raw[address + 0x03] == (byte)DESCRIPTOR_TYPE.DISPLAY_SERIALNUMBER)
                    {
                        string strTemp = Encoding.ASCII.GetString(Raw[(address + 5)..(address + 18)]).Trim();
                        return strTemp.Length == 0 ? string.Empty : strTemp;
                    }
                }
                return string.Empty;
            }
        }

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
        }

        private enum BYTE : byte
        {
            ID_MANUFACTURER         = 0x08,
            ID_PRODUCT              = 0x0A,
            ID_SERIAL               = 0x0C,
            MANUFACTURE_WEEK        = 0x10,
            MANUFACTURE_YEAR        = 0x11,
            VERSION_INTEGER         = 0x12,
            VERSION_DECIMAL         = 0x13,

            DESCRIPTOR1             = 0x36,
            DESCRIPTOR2             = 0x48,
            DESCRIPTOR3             = 0x5A,
            DESCRIPTOR4             = 0x6C,

            EXTENSION               = 0x7E,
            CHECKSUM                = 0x7F,
        }

        private enum DESCRIPTOR_TYPE : byte
        {
            DISPLAY_NAME            = 0xFC,
            DISPLAY_SERIALNUMBER    = 0xFF,
        }
    }
}
