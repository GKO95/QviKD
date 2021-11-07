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
using System.Windows.Shapes;
using QviKD.Types;

namespace QviKD
{
    /// <summary>
    /// Interaction logic for ModuleWindow.xaml
    /// </summary>
    public partial class ModuleWindow : Window
    {
        private readonly Display Display;
        private readonly object ContentControl;

        public ModuleWindow(Display display, Module module)
        {
            InitializeComponent();
            ContentControl = Activator.CreateInstance(module.Type) as UserControl;
            Display = display;
        }

        private void ModuleWindow_Initialized(object sender, EventArgs e)
        {
            ModuleWindowContent.Content = ContentControl;
        }
        private void ModuleWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Left = Display.Rect.left;
            Top = Display.Rect.top;
            WindowState = WindowState.Maximized;
        }

        private void ModuleWindowCaptionButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void ModuleWindowCaptionButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    
        
    }

    public interface IModuleWindow
    {

    }
}
