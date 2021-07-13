using System;
using System.Runtime.InteropServices;

using HDEVINFO = System.IntPtr;
using DWORD = System.UInt32;
using HWND = System.IntPtr;
using HKEY = System.IntPtr;

namespace QviKDLib.WinAPI
{
    internal static class SetupAPI
    {
        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "SetupDiClassGuidsFromNameW")]
        internal static extern bool SetupDiClassGuidsFromNameW(string ClassName, ref Guid ClassGuidList, DWORD ClassGuidListSize, ref DWORD RequiredSize);

        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, EntryPoint = "SetupDiGetClassDevsW")]
        internal static extern HDEVINFO SetupDiGetClassDevsW(ref Guid ClassGuid, string Enumerator, HWND hwndParent, DWORD Flags);

        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, EntryPoint = "SetupDiEnumDeviceInfo")]
        internal static extern bool SetupDiEnumDeviceInfo(HDEVINFO DeviceInfoSet, DWORD MemberIndex, ref SP_DEVINFO_DATA DeviceInfoData);

        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, EntryPoint = "SetupDiOpenDevRegKey")]
        internal static extern HKEY SetupDiOpenDevRegKey(HDEVINFO DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, DWORD Scope, DWORD HwProfile, DWORD KeyType, DWORD samDesired);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SP_DEVINFO_DATA
    {
        public DWORD cbSize;
        public Guid ClassGuid;
        public DWORD DevInst;
        public IntPtr Reserved;
    }

    [Flags]
    internal enum ERROR : int
    {
        SUCCESS = 0x0,
        INVALID_HANDLE = 0x6,
        INSUFFICIENT_BUFFER = 0x7A,
    }

    [Flags]
    internal enum DIGCF : DWORD
    {
        DEFAULT = 0x00000001,
        PRESENT = 0x00000002,
        ALLCLASSES = 0x00000004,
        PROFILE = 0x00000008,
        DEVICEINTERFACE = 0x00000010
    }

    [Flags]
    internal enum DICS_FLAG : DWORD
    {
        GLOBAL = 0x00000001,
        CONFIGSPECIFIC = 0x00000002,
        CONFIGGENERAL = 0x00000004
    }

    [Flags]
    internal enum DIREG : DWORD
    {
        DEV = 0x00000001,
        DRV = 0x00000002,
        BOTH = 0x00000004
    }

    [Flags]
    internal enum KEY : long
    {
        QUERY_VALUE = 0x0001,
        SET_VALUE = 0x0002,
        CREATE_SUB_KEY = 0x0004,
        ENUMERATE_SUB_KEYS = 0x0008,
        NOTIFY = 0x0010,
        CREATE_LINK = 0x0020,
        WOW64_32KEY = 0x0200,
        WOW64_64KEY = 0x0100,
        WOW64_RES = 0x0300,
        READ = (0x00020000L | QUERY_VALUE | ENUMERATE_SUB_KEYS | NOTIFY) & (~0x00100000L),
        WRITE = (0x00020000L | SET_VALUE | CREATE_SUB_KEY) & (~0x00100000L),
        EXECUTE = (READ) & (~0x00100000L),
        ALL_ACCESS = (0x001F0000L | QUERY_VALUE | SET_VALUE | CREATE_SUB_KEY | ENUMERATE_SUB_KEYS | NOTIFY | CREATE_LINK) & (~0x00100000L)
    }

    [Flags]
    internal enum REG : ulong
    {
        NONE = 0ul,
        SZ = 1ul,
        EXPAND_SZ = 2ul,
        BINARY = 3ul,
        DWORD = 4ul,
        DWORD_LITTLE_ENDIAN = 4ul,
        DWORD_BIG_ENDIAN = 5ul,
        LINK = 6ul,
        MULTI_SZ = 7ul,
        RESOURCE_LIST = 8ul,
        FULL_RESOURCE_DESCRIPTOR = 9ul,
        RESOURCE_REQUIREMENTS_LIST = 10ul,
        QWORD = 11ul,
        QWORD_LITTLE_ENDIAN = QWORD
    }
}
