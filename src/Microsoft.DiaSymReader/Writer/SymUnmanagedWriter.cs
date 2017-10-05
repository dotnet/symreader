// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.DiaSymReader
{
    /// <summary>
    /// Windows PDB writer.
    /// </summary>
    public abstract class SymUnmanagedWriter : IDisposable
    {
        public abstract void Dispose();

        /// <summary>
        /// Gets the raw data blobs that comprise the written PDB content so far.
        /// </summary>
        public abstract IEnumerable<ArraySegment<byte>> GetUnderlyingData();

        /// <summary>
        /// Writes the PDB data to specified stream. Once called no more changes to the data can be made using this writer.
        /// May be called multiple times. Always writes the same data. 
        /// </summary>
        /// <param name="stream">Stream to write PDB data to.</param>
        /// <exception cref="PdbWritingException">Error occurred while writing data to the stream.</exception>
        public abstract void WriteTo(Stream stream);

        public abstract int DocumentTableCapacity { get; set; }
        
        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        /// <exception cref="InvalidOperationException">Writes are not allowed to the underlying stream.</exception>
        /// <exception cref="PdbWritingException">Error occurred while writing PDB data.</exception>
        public abstract int DefineDocument(string name, Guid language, Guid vendor, Guid type, Guid algorithmId, byte[] checksum, byte[] source);

        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        /// <exception cref="InvalidOperationException">Writes are not allowed to the underlying stream.</exception>
        /// <exception cref="PdbWritingException">Error occurred while writing PDB data.</exception>
        public abstract void DefineSequencePoints(int documentIndex, int count, int[] offsets, int[] startLines, int[] startColumns, int[] endLines, int[] endColumns);

        /// <summary>
        /// Opens a method.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        /// <exception cref="InvalidOperationException">Writes are not allowed to the underlying stream.</exception>
        /// <exception cref="PdbWritingException">Error occurred while writing PDB data.</exception>
        public abstract void OpenMethod(int methodToken);

        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        /// <exception cref="InvalidOperationException">Writes are not allowed to the underlying stream.</exception>
        /// <exception cref="PdbWritingException">Error occurred while writing PDB data.</exception>
        public abstract void CloseMethod();
        
        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        /// <exception cref="InvalidOperationException">Writes are not allowed to the underlying stream.</exception>
        public abstract void OpenScope(int startOffset);

        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        /// <exception cref="InvalidOperationException">Writes are not allowed to the underlying stream.</exception>
        /// <exception cref="PdbWritingException">Error occurred while writing PDB data.</exception>
        public abstract void CloseScope(int endOffset);

        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        /// <exception cref="InvalidOperationException">Writes are not allowed to the underlying stream.</exception>
        /// <exception cref="PdbWritingException">Error occurred while writing PDB data.</exception>
        public abstract void DefineLocalVariable(int index, string name, int attributes, int localSignatureToken);

        /// <summary>
        /// Defines a local constant.
        /// </summary>
        /// <param name="name">Name of the constant.</param>
        /// <param name="value">Value.</param>
        /// <param name="constantSignatureToken">Standalone signature token encoding the static type of the constant.</param>
        /// <returns>False if the constant representation is too long (e.g. long string).</returns>
        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        /// <exception cref="InvalidOperationException">Writes are not allowed to the underlying stream.</exception>
        /// <exception cref="PdbWritingException">Error occurred while writing PDB data.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null</exception>
        public abstract bool DefineLocalConstant(string name, object value, int constantSignatureToken);

        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        /// <exception cref="InvalidOperationException">Writes are not allowed to the underlying stream.</exception>
        /// <exception cref="PdbWritingException">Error occurred while writing PDB data.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="importString"/> is null</exception>
        public abstract void UsingNamespace(string importString);

        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        /// <exception cref="InvalidOperationException">Writes are not allowed to the underlying stream.</exception>
        /// <exception cref="PdbWritingException">Error occurred while writing PDB data.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="yieldOffsets"/> or <paramref name="resumeOffsets"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="yieldOffsets"/> or <paramref name="resumeOffsets"/> differ in length.</exception>
        public abstract void SetAsyncInfo(int moveNextMethodToken, int kickoffMethodToken, int catchHandlerOffset, int[] yieldOffsets, int[] resumeOffsets);

        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        /// <exception cref="InvalidOperationException">Writes are not allowed to the underlying stream.</exception>
        /// <exception cref="PdbWritingException">Error occurred while writing PDB data.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="metadata"/> is null</exception>
        public abstract void DefineCustomMetadata(byte[] metadata);
        
        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        /// <exception cref="InvalidOperationException">Writes are not allowed to the underlying stream.</exception>
        /// <exception cref="PdbWritingException">Error occurred while writing PDB data.</exception>
        public abstract void SetEntryPoint(int entryMethodToken);

        /// <summary>
        /// Updates the current PDB signature.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        /// <exception cref="InvalidOperationException">Writes are not allowed to the underlying stream.</exception>
        /// <exception cref="PdbWritingException">Error occurred while writing PDB data.</exception>
        public abstract void UpdateSignature(Guid guid, uint stamp, int age);

        /// <summary>
        /// Gets the current PDB signature.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        /// <exception cref="PdbWritingException">Error occurred while writing PDB data.</exception>
        public abstract void GetSignature(out Guid guid, out uint stamp, out int age);

        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        /// <exception cref="InvalidOperationException">Writes are not allowed to the underlying stream.</exception>
        /// <exception cref="PdbWritingException">Error occurred while writing PDB data.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null</exception>
        public abstract void SetSourceServerData(byte[] data);

        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null</exception>
        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        /// <exception cref="InvalidOperationException">Writes are not allowed to the underlying stream.</exception>
        /// <exception cref="PdbWritingException">Error occurred while writing PDB data.</exception>
        /// <exception cref="NotSupportedException">Source Link is not supported by this writer.</exception>
        public abstract void SetSourceLinkData(byte[] data);

        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        /// <exception cref="InvalidOperationException">Writes are not allowed to the underlying stream.</exception>
        /// <exception cref="PdbWritingException">Error occurred while writing PDB data.</exception>
        public abstract void OpenTokensToSourceSpansMap();

        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        /// <exception cref="InvalidOperationException">Writes are not allowed to the underlying stream.</exception>
        /// <exception cref="PdbWritingException">Error occurred while writing PDB data.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="documentIndex"/> doesn't correspond to any defined document.</exception>
        public abstract void MapTokenToSourceSpan(int token, int documentIndex, int startLine, int startColumn, int endLine, int endColumn);

        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        /// <exception cref="InvalidOperationException">Writes are not allowed to the underlying stream.</exception>
        /// <exception cref="PdbWritingException">Error occurred while writing PDB data.</exception>
        public abstract void CloseTokensToSourceSpansMap();
    }
}
