// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.
#if NET46

namespace System.Runtime.Versioning
{
    [AttributeUsageAttribute(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    internal sealed class TargetFrameworkAttribute : Attribute
    {
        public string FrameworkName { get; }
        public string FrameworkDisplayName { get; set; }

        public TargetFrameworkAttribute(string frameworkName) 
            => FrameworkName = frameworkName;
    }
}
#endif