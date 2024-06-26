﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.DiaSymReader
{
    [Guid("28AD3D43-B601-4d26-8A1B-25F9165AF9D7")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(false)]
    [GeneratedWhenPossibleComInterface]
    public partial interface ISymUnmanagedBinder3 : ISymUnmanagedBinder2
    {
        // .NET 8+ COM source generators respect COM interface inheritance
        // so re-declaration of inherited method is not needed.
#if NETSTANDARD2_0
        #region ISymUnmanagedBinder methods

        /// <summary>
        /// Given a metadata interface and a file name, returns the
        /// correct <see cref="ISymUnmanagedReader"/> that will read the debugging symbols
        /// associated with the module.
        /// </summary>
        /// <param name="metadataImporter">An instance of IMetadataImport providing metadata for the specified PE file.</param>
        /// <param name="fileName">PE file path.</param>
        /// <param name="searchPath">Alternate path to search for debug data.</param>
        /// <param name="reader">The new reader instance.</param>
        [PreserveSig]
        new int GetReaderForFile(
            [MarshalAs(UnmanagedType.Interface)]object metadataImporter,
            [MarshalAs(UnmanagedType.LPWStr)]string fileName,
            [MarshalAs(UnmanagedType.LPWStr)]string searchPath,
            [MarshalAs(UnmanagedType.Interface)]out ISymUnmanagedReader reader);

        /// <summary>
        /// Given a metadata interface and a stream that contains
        /// the symbol store, returns the <see cref="ISymUnmanagedReader"/>
        /// that will read the debugging symbols from the given
        /// symbol store.
        /// </summary>
        /// <param name="metadataImporter">An instance of IMetadataImport providing metadata for the corresponding PE file.</param>
        /// <param name="stream">PDB stream.</param>
        /// <param name="reader">The new reader instance.</param>
        [PreserveSig]
        new int GetReaderFromStream(
            [MarshalAs(UnmanagedType.Interface)]object metadataImporter,
            [MarshalAs(UnmanagedType.Interface)]object stream,
            [MarshalAs(UnmanagedType.Interface)]out ISymUnmanagedReader reader);

        #endregion

        #region ISymUnmanagedBinder2 methods

        /// <summary>
        /// Given a metadata interface and a file name, returns the
        /// <see cref="ISymUnmanagedReader"/> interface that will read the debugging symbols associated
        /// with the module.
        /// </summary>
        /// <remarks>
        /// This version of the function can search for the PDB in areas other than
        /// right next to the module, controlled by the <paramref name="searchPolicy"/>.
        /// If a <paramref name="searchPath"/> is provided, those directories will always be searched.
        /// </remarks>
        /// <param name="metadataImporter">An instance of IMetadataImport providing metadata for the specified PE file.</param>
        /// <param name="fileName">PE file path.</param>
        /// <param name="searchPath">Alternate path to search for debug data.</param>
        /// <param name="searchPolicy">Search policy.</param>
        /// <param name="reader">The new reader instance.</param>
        [PreserveSig]
        new int GetReaderForFile2(
            [MarshalAs(UnmanagedType.Interface)]object metadataImporter,
            [MarshalAs(UnmanagedType.LPWStr)]string fileName,
            [MarshalAs(UnmanagedType.LPWStr)]string searchPath,
            SymUnmanagedSearchPolicy searchPolicy,
            [MarshalAs(UnmanagedType.Interface)]out ISymUnmanagedReader reader);

        #endregion
#endif

        [PreserveSig]
        int GetReaderFromCallback(
            [MarshalAs(UnmanagedType.Interface)] object metadataImporter,
            [MarshalAs(UnmanagedType.LPWStr)]string fileName,
            [MarshalAs(UnmanagedType.LPWStr)]string searchPath,
            SymUnmanagedSearchPolicy searchPolicy,
            [MarshalAs(UnmanagedType.Interface)] object callback,
            [MarshalAs(UnmanagedType.Interface)]out ISymUnmanagedReader reader);
    }
}
