using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace QviKD.Controls
{
    /// <summary>
    /// Interaction logic for Notification.xaml
    /// </summary>
    public partial class Notification : Popup
    {
        private readonly DispatcherTimer DispatcherTimer;

        public Notification()
        {
            InitializeComponent();

            DispatcherTimer = new();
            DispatcherTimer.Tick += DispatcherTimer_Tick;

            Background = new SolidColorBrush(Color.FromRgb(72, 72, 72));
            Foreground = new SolidColorBrush(Colors.White);
        }

        /// <summary>
        /// Alert notification through a popup for a given TimeSpan duration.
        /// </summary>
        public void Show(string message, TimeSpan timeSpan)
        {
            NotificationContent.Text = message;

            PopupAnimation = PopupAnimation.Slide;
            IsOpen = true;

            DispatcherTimer.Interval = timeSpan;
            DispatcherTimer.Start();
        }

        /// <summary>
        /// Alert notification through a popup for a given seconds duration.
        /// </summary>
        public void Show(string message, int second) => Show(message, new TimeSpan(0, 0, second));

        /// <summary>
        /// Alert notification through a popup.
        /// </summary>
        public void Show(string message) => Show(message, 2);

        /// <summary>
        /// Hide notification.
        /// </summary>
        public void Hide()
        {
            PopupAnimation = PopupAnimation.Fade;
            IsOpen = false;

            DispatcherTimer.Stop();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e) => Hide();

        /// <summary>
        /// Gets or sets the Brush that fills the area of the popup notification.
        /// </summary>
        public Brush Background { get => NotificationArea.Background; set => NotificationArea.Background = value; }

        /// <summary>
        /// Gets or sets the Brush to apply to the text contents of the popup notification.
        /// </summary>
        public Brush Foreground { get => NotificationContent.Foreground; set => NotificationContent.Foreground = value; }

        /// <summary>
        /// Gets or sets the Brush that draws the outer border of the popup notification.
        /// </summary>
        public Brush BorderBrush { get => NotificationBorder.BorderBrush; set => NotificationBorder.BorderBrush = value; }

        /// <summary>
        /// Gets or sets the relative Thickness of a Border of the popup notification.
        /// </summary>
        public Thickness BorderThickness { get => NotificationBorder.BorderThickness; set => NotificationBorder.BorderThickness = value; }

        /// <summary>
        /// Gets or sets the top-level font size for the popup notification.
        /// </summary>
        public double FontSize { get => NotificationContent.FontSize; set => NotificationContent.FontSize = value; }

        /// <summary>
        /// Gets or sets the top-level font weight for the popup notification.
        /// </summary>
        public FontWeight FontWeight { get => NotificationContent.FontWeight; set => NotificationContent.FontWeight = value; }

        /// <summary>
        /// Gets or sets the top-level font style for the popup notification.
        /// </summary>
        public FontStyle FontStyle { get => NotificationContent.FontStyle; set => NotificationContent.FontStyle = value; }

        /// <summary>
        /// Gets or sets the top-level font family for the popup notification.
        /// </summary>
        public FontFamily FontFamily { get => NotificationContent.FontFamily; set => NotificationContent.FontFamily = value; }

        /// <summary>
        /// Gets or sets the inner padding of the popup notification.
        /// </summary>
        public Thickness Padding { get => NotificationContent.Margin; set => NotificationContent.Margin = value; }
    }
}
