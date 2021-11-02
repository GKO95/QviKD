using System;
using System.Reflection;
using System.Collections.Generic;

namespace QviKD
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

        internal Module(Assembly assembly)
        {
            AssemblyName = assembly.GetName();
            Type = InstantiateFrom(assembly);
        }

        /// <summary>
        /// Determine whether the assembly is valid as a module.
        /// </summary>
        internal static bool IsValidModule(Assembly assembly) => InstantiateFrom(assembly) != null;

        /// <summary>
        /// Get a class with valid format from the assembly to instantiate; otherwise, return null.
        /// </summary>
        private static Type InstantiateFrom(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.BaseType.FullName == $"{typeof(ModuleWindow)}"
                    && type.Namespace.Contains(Constant.MODULE_NAMESPACE)
                    && type.Name == Constant.MODULE_ENTRY)
                {
                    return type;
                }
            }
            return null;
        }
    }

    internal record Constant
    {
        internal static readonly string MODULE_NAMESPACE = $"{Assembly.GetExecutingAssembly().GetName().Name}.Module";
        internal static readonly string MODULE_ENTRY = "MainWindow";
    }

    public enum ModuleFilter : byte
    {
        NONE        = 0x00,
        INCLUDE     = 0x01,
        EXCLUDE     = 0x02,
        ALL         = 0x03,
    }
}
