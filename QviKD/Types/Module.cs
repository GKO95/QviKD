using System;
using System.Reflection;
using QviKD.Modules;

namespace QviKD.Types
{
    public record Module
    {
        /// <summary>
        /// A class to instantiate from the assembly.
        /// </summary>
        internal Type Type { get; }

        /// <summary>
        /// Assembly's unique identity: {Name, Version, Culture, PublicKeyToken}
        /// </summary>
        internal AssemblyName AssemblyName { get; }

        /// <summary>
        /// Assembly itself from the .DLL module file.
        /// </summary>
        private Assembly Assembly { get; }

        internal Module(Assembly assembly)
        {
            Assembly = assembly;
            AssemblyName = Assembly.GetName();
            Type = GetModuleType(Assembly);
        }

        /// <summary>
        /// Identifies whether the module is available for the monitor.
        /// </summary>
        internal bool IsAvailable(Display display) 
            => (Assembly.CreateInstance($"{Type.FullName}") as ModuleControl).IsAvailable(display);

        /// <summary>
        /// Get a class with valid format from the assembly to instantiate; otherwise, return null.
        /// </summary>
        private static Type GetModuleType(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                // Filters ModuleControl class defined in other assemblies.
                if (type.Namespace.Contains(typeof(ModuleControl).Namespace)
                    && type.IsSubclassOf(typeof(ModuleControl))) 
                    return type;
            }
            return null;
        }

        /// <summary>
        /// Determine whether the assembly is valid as a module.
        /// </summary>
        internal static bool IsValid(Assembly assembly) 
            => GetModuleType(assembly) != null;
    }
}
