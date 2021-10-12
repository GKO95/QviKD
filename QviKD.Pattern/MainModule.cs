using System;
using System.Windows;
using System.Windows.Controls;

namespace QviKD.Modules.Pattern
{
    public class MainModule : Window
    {
        public MainModule()
        {
            Grid grid = new();
            Content = grid;
            grid.Children.Add(new Button() { Content = "Hello World!", Height=100, Width=100, });
        }
    }


}
