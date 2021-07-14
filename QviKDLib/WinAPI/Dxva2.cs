using System;
using System.Runtime.InteropServices;

using DWORD = System.UInt32;
using HANDLE = System.IntPtr;
using HMONITOR = System.IntPtr;

namespace QviKDLib.WinAPI
{
    public static class Dxva2
    {
        [DllImport("Dxva2.dll", CharSet = CharSet.Ansi, EntryPoint = "GetNumberOfPhysicalMonitorsFromHMONITOR")]
        internal static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(HMONITOR hMonitor, ref DWORD pdwNumberOfPhysicalMonitors);

        [DllImport("Dxva2.dll", CharSet = CharSet.Ansi, EntryPoint = "GetPhysicalMonitorsFromHMONITOR")]
        internal static extern bool GetPhysicalMonitorsFromHMONITOR(HMONITOR hMonitor, DWORD dwPhysicalMonitorArraySize, ref PHYSICAL_MONITOR pPhysicalMonitorArray);

        [DllImport("Dxva2.dll", CharSet = CharSet.Ansi, EntryPoint = "GetVCPFeatureAndVCPFeatureReply")]
        internal static extern bool GetVCPFeatureAndVCPFeatureReply(HANDLE hMonitor, byte bVCPCode, _MC_VCP_CODE_TYPE pvct, ref DWORD pdwCurrentValue, ref DWORD pdwMaximumValue);

        [DllImport("Dxva2.dll", CharSet = CharSet.Ansi, EntryPoint = "SetVCPFeature")]
        internal static extern bool SetVCPFeature(HANDLE hMonitor, byte bVCPCode, DWORD dwNewValue);
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct PHYSICAL_MONITOR
    {
        public HANDLE hPhysicalMonitor;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] public string szPhysicalMonitorDescription;
    }

    [Flags]
    public enum _MC_VCP_CODE_TYPE
    {
        MC_MOMENTARY,
        MC_SET_PARAMETER
    }
}
