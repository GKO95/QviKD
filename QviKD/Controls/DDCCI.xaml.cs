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
using QviKD.Functions;
using QviKD.Types;

namespace QviKD.Controls
{
    /// <summary>
    /// Interaction logic for DDCCI.xaml
    /// </summary>
    public partial class DDCCI : UserControl, INotifyPropertyChanged
    {
        private Display _Display = null;
        public Display Display
        {
            get => _Display;
            set
            {
                _Display = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly static VisibilityPropertyConverter Converter = new();

        public DDCCI()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetBinding(VisibilityProperty, new Binding("Display")
            {
                Source = this,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Converter = Converter,
            });
        }

        /// <summary>
        /// Triggers PropertyChanged event that is sent to the targeted binding client.
        /// </summary>
        private void OnPropertyChanged([CallerMemberName] string memberName = "")
        {
            // CallerMemberName attribute assigns the calling member as its arugment.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
    }
}
