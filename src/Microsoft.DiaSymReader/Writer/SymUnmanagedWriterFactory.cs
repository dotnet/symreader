// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.DiaSymReader
{
    public static class SymUnmanagedWriterFactory
    {
        private const string SymWriterClsid = "0AE2DEB0-F901-478b-BB9F-881EE8066788";

        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory | DllImportSearchPath.SafeDirectories)]
        [DllImport("Microsoft.DiaSymReader.Native.x86.dll", EntryPoint = "CreateSymWriter")]
        private extern static void CreateSymWriter32(ref Guid id, [MarshalAs(UnmanagedType.IUnknown)]out object symWriter);

        [DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory | DllImportSearchPath.SafeDirectories)]
        [DllImport("Microsoft.DiaSymReader.Native.amd64.dll", EntryPoint = "CreateSymWriter")]
        private extern static void CreateSymWriter64(ref Guid id, [MarshalAs(UnmanagedType.IUnknown)]out object symWriter);

        internal static ISymUnmanagedWriter8 CreateWriterWithMetadataEmit(object pdbStream, object metadataEmitAndImport)
        {
            if (pdbStream == null)
            {
                throw new ArgumentNullException(nameof(pdbStream));
            }

            if (metadataEmitAndImport == null)
            {
                throw new ArgumentNullException(nameof(metadataEmitAndImport));
            }

            object symWriter = null;
            var guid = new Guid(SymWriterClsid);
            if (IntPtr.Size == 4)
            {
                CreateSymWriter32(ref guid, out symWriter);
            }
            else
            {
                CreateSymWriter64(ref guid, out symWriter);
            }

            var writer = (ISymUnmanagedWriter8)symWriter;
            writer.InitializeDeterministic(metadataEmitAndImport, pdbStream);
            return writer;
        }

        /// <summary>
        /// Creates a Windows PDB writer.
        /// </summary>
        /// <param name="metadataProvider"><see cref="ISymWriterMetadataProvider"/> implementation.</param>
        /// <remarks>
        /// The underlying SymWriter is loaded from Microsoft.DiaSymReader.Native.x86.dll or Microsoft.DiaSymReader.Native.amd64.dll 
        /// depending on the current process architecture.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="metadataProvider"/>is null.</exception>
        public static SymUnmanagedWriter CreateWriter(ISymWriterMetadataProvider metadataProvider)
        {
            if (metadataProvider == null)
            {
                throw new ArgumentNullException(nameof(metadataProvider));
            }

            return new SymUnmanagedWriterImpl(metadataProvider);
        }
    }
}
