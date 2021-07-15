using System;
using System.Runtime.InteropServices;

using HANDLE = System.IntPtr;
using DWORD = System.UInt32;
using HWND = System.IntPtr;

namespace QviKDLib.WinAPI
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct PHYSICAL_MONITOR
    {
        public HANDLE hPhysicalMonitor;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] public string szPhysicalMonitorDescription;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SECURITY_ATTRIBUTES
    {
        public DWORD nLength;
        public HANDLE lpSecurityDescriptor;
        public int bInheritHandle;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SP_DEVINFO_DATA
    {
        public DWORD cbSize;
        public Guid ClassGuid;
        public DWORD DevInst;
        public HWND Reserved;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct MONITORINFO
    {
        public DWORD cbSize;
        public RECT rcMonitor;
        public RECT rcWork;
        public DWORD dwFlags;
    }

    [Flags]
    public enum _MC_VCP_CODE_TYPE
    {
        MC_MOMENTARY,
        MC_SET_PARAMETER
    }

    [Flags]
    internal enum GENERIC : DWORD
    {
        READ = 0x80000000,
        WRITE = 0x40000000,
        EXECUTE = 0x20000000,
        ALL = 0x10000000
    }

    [Flags]
    internal enum FILE_SHARE : DWORD
    {
        NONE = 0x00000000,
        DELETE = 0x00000004,
        READ = 0x00000001,
        WRITE = 0x00000002
    }

    [Flags]
    internal enum FILE_FLAG : DWORD
    {
        BACKUP_SEMANTICS = 0x02000000,
        DELETE_ON_CLOSE = 0x04000000,
        NO_BUFFERING = 0x20000000,
        OPEN_NO_RECALL = 0x00100000,
        OPEN_REPARSE_POINT = 0x00200000,
        OVERLAPPED = 0x40000000,
        POSIX_SEMANTICS = 0x01000000,
        RANDOM_ACCESS = 0x10000000,
        SESSION_AWARE = 0x00800000,
        SEQUENTIAL_SCAN = 0x08000000,
        WRITE_THROUGH = 0x80000000
    }

    [Flags]
    internal enum DISPOSITION : DWORD
    {
        CREATE_NEW = 1,
        CREATE_ALWAYS = 2,
        OPEN_EXISTING = 3,
        OPEN_ALWAYS = 4,
        TRUNCATE_EXISTING = 5
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

    [Flags]
    internal enum MONITORINFOF : int
    {
        PRIMARY = 0x00000001,
    }
}
