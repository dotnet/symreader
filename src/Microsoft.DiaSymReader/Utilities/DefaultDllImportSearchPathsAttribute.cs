// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.
#if NET20
namespace System.Runtime.InteropServices
{
    [Flags]
    internal enum DllImportSearchPath
    {
        UseDllDirectoryForDependencies = 0x100,
        ApplicationDirectory = 0x200,
        UserDirectories = 0x400,
        System32 = 0x800,
        SafeDirectories = 0x1000,
        AssemblyDirectory = 0x2,
        LegacyBehavior = 0x0
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Method, AllowMultiple = false)]
    internal sealed class DefaultDllImportSearchPathsAttribute : Attribute
    {
        internal DllImportSearchPath _paths;
        public DefaultDllImportSearchPathsAttribute(DllImportSearchPath paths)
        {
            _paths = paths;
        }

        public DllImportSearchPath Paths { get { return _paths; } }
    }
}
#endif