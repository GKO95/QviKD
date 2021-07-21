using System;
using System.Runtime.InteropServices;

using HDEVINFO = System.IntPtr;
using DWORD = System.UInt32;
using HWND = System.IntPtr;
using HKEY = System.IntPtr;

namespace WinAPI
{
    internal static class SetupAPI
    {
        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "SetupDiClassGuidsFromNameW")]
        internal static extern bool SetupDiClassGuidsFromNameW(string ClassName, ref Guid ClassGuidList, DWORD ClassGuidListSize, ref DWORD RequiredSize);

        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, EntryPoint = "SetupDiGetClassDevsW")]
        internal static extern HDEVINFO SetupDiGetClassDevsW(ref Guid ClassGuid, string Enumerator, HWND hwndParent, DWORD Flags);

        [DllImport("setupapi.dll", EntryPoint = "SetupDiEnumDeviceInfo")]
        internal static extern bool SetupDiEnumDeviceInfo(HDEVINFO DeviceInfoSet, DWORD MemberIndex, ref SP_DEVINFO_DATA DeviceInfoData);

        [DllImport("setupapi.dll", EntryPoint = "SetupDiOpenDevRegKey")]
        internal static extern HKEY SetupDiOpenDevRegKey(HDEVINFO DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, DWORD Scope, DWORD HwProfile, DWORD KeyType, DWORD samDesired);
    }
}
