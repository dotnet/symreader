// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using System.IO;

namespace Microsoft.DiaSymReader
{
    using static InteropUtilities;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static partial class SymUnmanagedExtensions
    {
        public static ISymUnmanagedReader GetReaderFromStream(this ISymUnmanagedBinder binder, Stream stream, object metadataImporter)
        {
            if (binder == null)
            {
                throw new ArgumentNullException(nameof(binder));
            }

            ISymUnmanagedReader symReader;
            ThrowExceptionForHR(binder.GetReaderFromStream(metadataImporter, SymUnmanagedStreamFactory.CreateStream(stream), out symReader));
            return symReader;
        }

        public static ISymUnmanagedReader GetReaderFromPdbStream(this ISymUnmanagedBinder4 binder, Stream stream, IMetadataImportProvider metadataImportProvider)
        {
            if (binder == null)
            {
                throw new ArgumentNullException(nameof(binder));
            }

            ISymUnmanagedReader symReader;
            ThrowExceptionForHR(binder.GetReaderFromPdbStream(metadataImportProvider, SymUnmanagedStreamFactory.CreateStream(stream), out symReader));
            return symReader;
        }
    }
}
