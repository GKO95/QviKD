using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using QviKD.WinAPI;
using QviKD.Types;

using HANDLE = System.IntPtr;
using DWORD = System.UInt32;

namespace QviKD.Controls
{
    /// <summary>
    /// Interaction logic for DDCCI.xaml
    /// </summary>
    public partial class DDCCI : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Display _Display = null;
        public Display Display
        {
            get => _Display;
            set
            {
                if (_Display is null)
                {
                    _Display = value;
                    InitializeDDCCI();
                }
            }
        }

        public DDCCI()
        {
            InitializeComponent();
        }

        private void InitializeDDCCI()
        {
            
        }

        // DDCCI Get(Read) Process
        private void DDCCIControlContentCommandReadButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Wnd = (MainWindow)Window.GetWindow(this);

            byte Opcode = Convert.ToByte(DDCCIControlContentVCPCode.Text, 16);
            DWORD usCurrent = 0, usMaximum = 0;
            
            if (Dxva2.GetVCPFeatureAndVCPFeatureReply(Display.hPhysical, Opcode, _MC_VCP_CODE_TYPE.MC_MOMENTARY, ref usCurrent, ref usMaximum))
            {
                Wnd.StatusMessage($"DDC/CI Get 0x{Opcode:X2}: Success");

                DDCCIControlContentCommandReadCurrentDec.Text = Convert.ToString(usCurrent);
                DDCCIControlContentCommandReadMaximumDec.Text = Convert.ToString(usMaximum);

                DDCCIControlContentCommandReadCurrentHex.Text = usCurrent.ToString("X4");
                DDCCIControlContentCommandReadMaximumHex.Text = usMaximum.ToString("X4");

                DDCCIControlContentCommandReadFailure.Visibility = Visibility.Hidden;
                DDCCIControlContentCommandReadSuccess.Visibility = Visibility.Visible;
            }
            else
            {
                Wnd.StatusMessage($"DDC/CI Get 0x{Opcode:X2}: Error 0x{Marshal.GetLastWin32Error():X8}");

                DDCCIControlContentCommandReadFailure.Visibility = Visibility.Visible;
                DDCCIControlContentCommandReadSuccess.Visibility = Visibility.Hidden;
            }
        }

        // DDCCI Set(Write) Process
        private void DDCCIControlContentCommandWriteButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Wnd = (MainWindow)Window.GetWindow(this);

            byte Opcode = Convert.ToByte(DDCCIControlContentVCPCode.Text, 16); ;
            DWORD usValue = Convert.ToUInt16(DDCCIControlContentCommandWriteValueDec.Text);

            if (Dxva2.SetVCPFeature(Display.hPhysical, Opcode, usValue))
            {
                Wnd.StatusMessage($"DDC/CI Set 0x{Opcode:X2}: Success");

                DDCCIControlContentCommandWriteFailure.Visibility = Visibility.Hidden;
                DDCCIControlContentCommandWriteSuccess.Visibility = Visibility.Visible;
            }
            else
            {
                Wnd.StatusMessage($"DDC/CI Set 0x{Opcode:X2}: Error 0x{Marshal.GetLastWin32Error():X8}");

                DDCCIControlContentCommandWriteFailure.Visibility = Visibility.Visible;
                DDCCIControlContentCommandWriteSuccess.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Thread for retrieving capalbities of the display device.
        /// </summary>
        private void ThreadDisplayCapabilities(object data)
        {
            DWORD dwTemp = 0;
            string strTemp = string.Empty;

            DebugMessage($"{Display.DeviceName.Remove(0, 4)} Capabilities - background threading...");

            if (!Dxva2.GetCapabilitiesStringLength(Display.hPhysical, ref dwTemp))
            {
                Console.Error.WriteLine("Unable to retreive the length of monitor capabilities string from a display device.");
            }
            else
            {
                DebugMessage($"{Display.DeviceName.Remove(0, 4)} Capabilities - collecting data ({dwTemp})...");

                HANDLE ptrTemp = Marshal.AllocHGlobal((int)dwTemp);
                if (!Dxva2.CapabilitiesRequestAndCapabilitiesReply(Display.hPhysical, ptrTemp, dwTemp))
                {
                    Console.Error.WriteLine("Unable to retreive the monitor capabilities string from a display device.");
                    DebugMessage($"{Display.DeviceName.Remove(0, 4)} Capabilities - error!");
                }
                else
                {
                    strTemp = Marshal.PtrToStringAnsi(ptrTemp);
                    DebugMessage($"{Display.DeviceName.Remove(0, 4)} Capabilities: {strTemp}");
                }
                Marshal.FreeHGlobal(ptrTemp);
            }
            Display.Capabilities = strTemp;
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
        /// Print message for debugging; DEBUG-mode exclusive.
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG")]
        private void DebugMessage(string msg)
        {
            System.Diagnostics.Debug.WriteLine($"'{GetType().Name}.cs' {msg}");
        }
    }
}
