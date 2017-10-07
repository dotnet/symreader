// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;

namespace Microsoft.DiaSymReader
{
    /// <summary>
    /// <see cref="ISymUnmanagedReader"/> creation options.
    /// </summary>
    [Flags]
    public enum SymUnmanagedReaderCreationOptions
    {
        /// <summary>
        /// Default options.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Use environment variable MICROSOFT_DIASYMREADER_NATIVE_ALT_LOAD_PATH to locate Microsoft.DiaSymReader.Native.{platform}.dll.
        /// </summary>
        UseAlternativeLoadPath = 1 << 1,

        /// <summary>
        /// Use COM registry to locate an implementation of the reader.
        /// </summary>
        UseComRegistry = 1 << 2,
    }
}
