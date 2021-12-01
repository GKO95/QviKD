using System;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace QviKD
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex _mutex;
        private readonly string appName = $"{Assembly.GetExecutingAssembly().GetName().Name}";

        /// <summary>
        /// Raises the Application.Startup event for a single instance only.
        /// </summary>
        protected override void OnStartup(StartupEventArgs e)
        {
            _mutex = new Mutex(true, appName, out bool createdNew);

            if (!createdNew)
            {
                // App is already running! Exiting the application...
                Current.Shutdown();
            }

            base.OnStartup(e);
        }
    }
}
