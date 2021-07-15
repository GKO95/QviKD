using System.Runtime.InteropServices;

using DWORD = System.UInt32;
using HANDLE = System.IntPtr;
using HMONITOR = System.IntPtr;

namespace QviKDLib.WinAPI
{
    public static class Dxva2
    {
        [DllImport("Dxva2.dll", SetLastError = true, EntryPoint = "GetNumberOfPhysicalMonitorsFromHMONITOR")]
        internal static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(HMONITOR hMonitor, out DWORD pdwNumberOfPhysicalMonitors);

        [DllImport("Dxva2.dll", SetLastError = true, EntryPoint = "GetPhysicalMonitorsFromHMONITOR")]
        internal static extern bool GetPhysicalMonitorsFromHMONITOR(HMONITOR hMonitor, DWORD dwPhysicalMonitorArraySize, ref PHYSICAL_MONITOR pPhysicalMonitorArray);

        [DllImport("Dxva2.dll", SetLastError = true, EntryPoint = "DestroyPhysicalMonitor")]
        internal static extern bool DestroyPhysicalMonitor(HANDLE hMonitor);

        [DllImport("Dxva2.dll", SetLastError = true, EntryPoint = "DestroyPhysicalMonitors")]
        internal static extern bool DestroyPhysicalMonitors(DWORD dwPhysicalMonitorArraySize, ref PHYSICAL_MONITOR pPhysicalMonitorArray);

        [DllImport("Dxva2.dll", EntryPoint = "GetVCPFeatureAndVCPFeatureReply")]
        internal static extern bool GetVCPFeatureAndVCPFeatureReply(HANDLE hMonitor, byte bVCPCode, _MC_VCP_CODE_TYPE pvct, ref DWORD pdwCurrentValue, ref DWORD pdwMaximumValue);

        [DllImport("Dxva2.dll", EntryPoint = "SetVCPFeature")]
        internal static extern bool SetVCPFeature(HANDLE hMonitor, byte bVCPCode, DWORD dwNewValue);
    }
}
