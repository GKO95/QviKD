﻿using System;
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
    /// Interaction logic for GradientRGBW256.xaml
    /// </summary>
    public partial class GradientRGBW256 : Page
    {
        public GradientRGBW256()
        {
            InitializeComponent();

            for (int index = 0; index < MAX(DEPTH.BIT8); index++)
            {
                Rectangle rectangle;
                GradientRGBW256Content.ColumnDefinitions.Add(new ColumnDefinition());

                rectangle = new()
                {
                    Fill = new SolidColorBrush(Color.FromRgb((byte)index, 0, 0)),
                    Margin = new Thickness(0),
                };
                rectangle.SetValue(Grid.ColumnProperty, index);
                rectangle.SetValue(Grid.RowProperty, (int)ColorComponent.RED);
                GradientRGBW256Content.Children.Add(rectangle);

                rectangle = new()
                {
                    Fill = new SolidColorBrush(Color.FromRgb(0, (byte)index, 0)),
                    Margin = new Thickness(0),
                };
                rectangle.SetValue(Grid.ColumnProperty, index);
                rectangle.SetValue(Grid.RowProperty, (int)ColorComponent.GREEN);
                GradientRGBW256Content.Children.Add(rectangle);

                rectangle = new()
                {
                    Fill = new SolidColorBrush(Color.FromRgb(0, 0, (byte)index)),
                    Margin = new Thickness(0),
                };
                rectangle.SetValue(Grid.ColumnProperty, index);
                rectangle.SetValue(Grid.RowProperty, (int)ColorComponent.BLUE);
                GradientRGBW256Content.Children.Add(rectangle);

                rectangle = new()
                {
                    Fill = new SolidColorBrush(Color.FromRgb((byte)index, (byte)index, (byte)index)),
                    Margin = new Thickness(0),
                };
                rectangle.SetValue(Grid.ColumnProperty, index);
                rectangle.SetValue(Grid.RowProperty, (int)ColorComponent.WHITE);
                GradientRGBW256Content.Children.Add(rectangle);
            }
        }

        private static uint MAX(DEPTH depth) => Convert.ToUInt32(Math.Pow(2, (double)depth));

        enum ColorComponent
        { 
            RED,
            GREEN,
            BLUE,
            WHITE,
        }
    }
}