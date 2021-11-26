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

namespace QviKD.Modules.QvPattern.Patterns
{
    /// <summary>
    /// Interaction logic for SolidFullWhite.xaml
    /// </summary>
    public partial class SolidFullWhite : Page
    {
        private byte ucIndex = 255;

        public SolidFullWhite()
        {
            InitializeComponent();
            SolidFullWhiteContent.Background = new SolidColorBrush(Color.FromRgb(ucIndex, ucIndex, ucIndex));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is ModuleWindow wnd)
                wnd.KeyDown += Page_KeyDown;
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (Window.GetWindow(this) is ModuleWindow wnd)
            {
                switch (e.Key)
                {
                    case Key.Up:
                        if (ucIndex != 255) ucIndex++;
                        else ucIndex = 255;
                        wnd.Notification.Show($"DDL {ucIndex}");
                        break;

                    case Key.Down:
                        if (ucIndex != 0) ucIndex--;
                        else ucIndex = 0;
                        wnd.Notification.Show($"DDL {ucIndex}");
                        break;

                    case Key.Enter:
                        wnd.Notification.Show($"DDL {ucIndex}");
                        break;

                    default:
                        return;
                }
                SolidFullWhiteContent.Background = new SolidColorBrush(Color.FromRgb(ucIndex, ucIndex, ucIndex));
            }
        }
    }
}
