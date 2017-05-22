// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.DiaSymReader
{
    public static class SymUnmanagedReaderFactory
    {
        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory | DllImportSearchPath.SafeDirectories)]
        [DllImport("Microsoft.DiaSymReader.Native.x86.dll", EntryPoint = "CreateSymReader")]
        private extern static void CreateSymReader32(ref Guid id, [MarshalAs(UnmanagedType.IUnknown)]out object symReader);

        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory | DllImportSearchPath.SafeDirectories)]
        [DllImport("Microsoft.DiaSymReader.Native.amd64.dll", EntryPoint = "CreateSymReader")]
        private extern static void CreateSymReader64(ref Guid id, [MarshalAs(UnmanagedType.IUnknown)]out object symReader);

        /// <summary>
        /// Creates <see cref="ISymUnmanagedReader"/> instance and initializes it with specified PDB stream and metadata import object.
        /// </summary>
        /// <param name="pdbStream">Windows PDB stream.</param>
        /// <param name="metadataImport">IMetadataImport implementation.</param>
        /// <remarks>
        /// The SymReader is loaded from Microsoft.DiaSymReader.Native.x86.dll or Microsoft.DiaSymReader.Native.amd64.dll 
        /// depending on the current process architecture.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="pdbStream"/>is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="metadataImport"/>is null.</exception>
        public static ISymUnmanagedReader5 CreateReaderWithMetadataImport(Stream pdbStream, object metadataImport)
        {
            if (pdbStream == null)
            {
                throw new ArgumentNullException(nameof(pdbStream));
            }

            if (metadataImport == null)
            {
                throw new ArgumentNullException(nameof(metadataImport));
            }

            object symReader = null;

            var guid = default(Guid);
            if (IntPtr.Size == 4)
            {
                CreateSymReader32(ref guid, out symReader);
            }
            else
            {
                CreateSymReader64(ref guid, out symReader);
            }

            var reader = (ISymUnmanagedReader5)symReader;
            reader.Initialize(pdbStream, metadataImport);
            return reader;
        }

        /// <summary>
        /// Creates <see cref="ISymUnmanagedReader"/> instance and initializes it with specified PDB stream and metadata provider.
        /// </summary>
        /// <param name="pdbStream">Windows PDB stream.</param>
        /// <param name="metadataProvider"><see cref="ISymReaderMetadataProvider"/> implementation.</param>
        /// <remarks>
        /// The SymReader is loaded from Microsoft.DiaSymReader.Native.x86.dll or Microsoft.DiaSymReader.Native.amd64.dll 
        /// depending on the current process architecture.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="pdbStream"/>is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="metadataProvider"/>is null.</exception>
        public static ISymUnmanagedReader5 CreateReader(Stream pdbStream, ISymReaderMetadataProvider metadataProvider)
        {
            if (metadataProvider == null)
            {
                throw new ArgumentNullException(nameof(metadataProvider));
            }

            return CreateReaderWithMetadataImport(pdbStream, CreateSymReaderMetadataImport(metadataProvider));
        }

        /// <summary>
        /// Creates a minimal implementation of IMetadataImport required for reading PDBs based on given <see cref="ISymReaderMetadataProvider"/>.
        /// </summary>
        /// <param name="metadataProvider">Reads metadata.</param>
        /// <returns>An instance of IMetadataImport.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="metadataProvider"/>is null.</exception>
        public static object CreateSymReaderMetadataImport(ISymReaderMetadataProvider metadataProvider)
        {
            if (metadataProvider == null)
            {
                throw new ArgumentNullException(nameof(metadataProvider));
            }

            return new SymReaderMetadataAdapter(metadataProvider);
        }
    }
}
