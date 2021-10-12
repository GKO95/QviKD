using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace QviKD.Functions
{
    class EnumModules
    {
        public EnumModules(string directory = null)
        {
            // Set QviKD.exe directory by default.
            if (directory == null)
            {
                directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }

            // Acquire DLL assemblies excluding QviKD.dll.
            List<string> modules = Directory.EnumerateFiles(
                directory,
                "*.dll",
                SearchOption.TopDirectoryOnly).Where(path => path != Assembly.GetExecutingAssembly().Location
                ).ToList();

            // Enumerate over the DLL assemblies.
            for (int index = 0; index < modules.Count; index++)
            {
                Database.Modules.Add(Assembly.LoadFile(modules[index]));
            }

        }

    }
}
