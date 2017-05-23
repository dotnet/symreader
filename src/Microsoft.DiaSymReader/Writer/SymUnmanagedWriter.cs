// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.IO;

namespace Microsoft.DiaSymReader
{
    /// <summary>
    /// Windows PDB writer.
    /// </summary>
    public abstract class SymUnmanagedWriter : IDisposable
    {
        public abstract void Dispose();
        public abstract void WriteTo(Stream stream);

        public abstract int DocumentTableCapacity { get; set; }
        public abstract int DefineDocument(string name, Guid language, Guid vendor, Guid type, Guid algorithmId, byte[] checksum, byte[] source);

        public abstract void DefineSequencePoints(int documentIndex, int count, int[] offsets, int[] startLines, int[] startColumns, int[] endLines, int[] endColumns);
        public abstract void OpenMethod(int methodToken);
        public abstract void CloseMethod();
        public abstract void OpenScope(int startOffset);
        public abstract void CloseScope(int endOffset);
        public abstract void DefineLocalVariable(int index, string name, int attributes, int localSignatureToken);

        /// <summary>
        /// Defines a local constant.
        /// </summary>
        /// <param name="name">Name of the constant.</param>
        /// <param name="value">Value.</param>
        /// <param name="constantSignatureToken">Standalone signature token encoding the static type of the constant.</param>
        /// <returns>False if the constant representation is too long (e.g. long string).</returns>
        public abstract bool DefineLocalConstant(string name, object value, int constantSignatureToken);

        public abstract void UsingNamespace(string importString);
        public abstract void SetAsyncInfo(int moveNextMethodToken, int kickoffMethodToken, int catchHandlerOffset, int[] yieldOffsets, int[] resumeOffsets);
        public abstract void DefineCustomMetadata(byte[] metadata);
        public abstract void SetEntryPoint(int entryMethodToken);
        public abstract void UpdateSignature(Guid guid, uint stamp, int age);
        public abstract void SetSourceServerData(byte[] data);
        public abstract void SetSourceLinkData(byte[] data);
    }
}
