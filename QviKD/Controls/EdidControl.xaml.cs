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
using QviKD.Types;

namespace QviKD.Controls
{
    /// <summary>
    /// Interaction logic for EdidControl.xaml
    /// </summary>
    public partial class EdidControl : UserControl
    {
        private readonly Display Display;

        public EdidControl(Display display)
        {
            InitializeComponent();
            Display = display;
        }

        private void EdidControl_Loaded(object sender, RoutedEventArgs e)
        {
            EdidControlInformationIsPrimary.Text = Display.IsPrimary ? "Yes" : "No";
            EdidControlInformationResolution.Text = $"{Display.Rect.right - Display.Rect.left} x {Display.Rect.bottom - Display.Rect.top}";
            EdidControlInformationPosition.Text = $"({Display.Rect.left}, {Display.Rect.top})";

            EdidControlInformationDescription.Text = Display.Description;

            EdidControlInformationDeviceName.Text = Display.DeviceName;
            EdidControlInformationDeviceID.Text = Display.DeviceID;

            /*for (; ; )
            {
                EdidControlInformationRawTable.RowDefinitions.Add(new RowDefinition());
            }*/
        }
    }
}
