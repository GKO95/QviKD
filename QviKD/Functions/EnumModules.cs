using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

namespace QviKD.Functions
{
    class EnumModules
    {
        public EnumModules(string directory = null)
        {
            Database.Modules.Clear();
            
            // Set QviKD.exe directory by default.
            if (directory == null)
            {
                directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
            DebugMessage($"Module directory: {directory}");

            // Acquire DLL assemblies excluding QviKD.dll.
            List<string> module = Directory.EnumerateFiles(
                directory,
                "*.dll",
                SearchOption.TopDirectoryOnly).Where(path => path != Assembly.GetExecutingAssembly().Location
                ).ToList();

            // Enumerate over the DLL assemblies.
            for (int index = 0; index < module.Count; index++)
            {
                Assembly assembly = Assembly.LoadFile(module[index]);

                // Add valid module assembly to the List.
                if (Module.IsValidModule(assembly))
                {
                    Database.Modules.Add(new Module(assembly));
                    DebugMessage($"Module: {assembly.FullName}");
                }
            }
        }

        /// <summary>
        /// Print message for debugging; DEBUG-mode exclusive.
        /// </summary>
        private void DebugMessage(string msg)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"'{GetType().Name}.cs' {msg}");
#endif
        }
    }
}
