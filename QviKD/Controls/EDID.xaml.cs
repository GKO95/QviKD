using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Runtime.CompilerServices;
using System.ComponentModel;
using QviKD.Types;

namespace QviKD.Controls
{
    /// <summary>
    /// Interaction logic for EDID.xaml
    /// </summary>
    public partial class EDID : UserControl
    {
        private Display _Display = null;
        public Display Display
        {
            get => _Display;
            set
            {
                if (_Display is null)
                {
                    _Display = value;
                    InitializeEDID();
                }
            }
        }

        public EDID()
        {
            InitializeComponent();
        }

        private void InitializeEDID()
        {
            EDIDControlContentIsPrimaryData.Text = Display.IsPrimary ? "Yes" : "No";
            EDIDControlContentResolutionData.Text = $"{Display.Rect.right - Display.Rect.left} x {Display.Rect.bottom - Display.Rect.top}";
            EDIDControlContentPositionData.Text = $"({Display.Rect.left}, {Display.Rect.top})";
            EDIDControlContentDescriptionData.Text = Display.Description;
            EDIDControlContentDeviceNameData.Text = Display.DeviceName;
            EDIDControlContentDeviceIDData.Text = Display.DeviceID;
        }
    }
}
