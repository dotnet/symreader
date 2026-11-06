// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.DiaSymReader.UnitTests
{
    internal unsafe class SymWriterMock : ISymUnmanagedWriter5
    {
        public virtual ISymUnmanagedDocumentWriter DefineDocument(string url, ref Guid language, ref Guid languageVendor, ref Guid documentType) => throw new NotImplementedException();
        public virtual void SetUserEntryPoint(int entryMethodToken) => throw new NotImplementedException();
        public virtual void OpenMethod(uint methodToken) => throw new NotImplementedException();
        public virtual void CloseMethod() => throw new NotImplementedException();
        public virtual uint OpenScope(int startOffset) => throw new NotImplementedException();
        public virtual void CloseScope(int endOffset) => throw new NotImplementedException();
        public virtual void SetScopeRange(uint scopeID, uint startOffset, uint endOffset) => throw new NotImplementedException();
        public virtual void DefineLocalVariable(string name, uint attributes, uint sig, byte* signature, uint addrKind, uint addr1, uint addr2, uint startOffset, uint endOffset) => throw new NotImplementedException();
        public virtual void DefineParameter(string name, uint attributes, uint sequence, uint addrKind, uint addr1, uint addr2, uint addr3) => throw new NotImplementedException();
        public virtual void DefineField(uint parent, string name, uint attributes, uint sig, byte* signature, uint addrKind, uint addr1, uint addr2, uint addr3) => throw new NotImplementedException();
        public virtual void DefineGlobalVariable(string name, uint attributes, uint sig, byte* signature, uint addrKind, uint addr1, uint addr2, uint addr3) => throw new NotImplementedException();
        public virtual void Close() => throw new NotImplementedException();
        public virtual void SetSymAttribute(uint parent, string name, int length, byte* data) => throw new NotImplementedException();
        public virtual void OpenNamespace(string name) => throw new NotImplementedException();
        public virtual void CloseNamespace() => throw new NotImplementedException();
        public virtual void UsingNamespace(string fullName) => throw new NotImplementedException();
        public virtual void SetMethodSourceRange(ISymUnmanagedDocumentWriter startDoc, uint startLine, uint startColumn, ISymUnmanagedDocumentWriter endDoc, uint endLine, uint endColumn) => throw new NotImplementedException();
        public virtual void Initialize([MarshalAs(UnmanagedType.Interface)] object emitter, string filename, [MarshalAs(UnmanagedType.Interface)] object ptrIStream, [MarshalAs(UnmanagedType.Bool)] bool fullBuild) => throw new NotImplementedException();
        public virtual void GetDebugInfo(ref ImageDebugDirectory debugDirectory, uint dataCount, out uint dataCountPtr, byte* data) => throw new NotImplementedException();
        public virtual void DefineSequencePoints(ISymUnmanagedDocumentWriter document, int count, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] offsets, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] lines, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] columns, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] endLines, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] endColumns) => throw new NotImplementedException();
        public virtual void RemapToken(uint oldToken, uint newToken) => throw new NotImplementedException();
        public virtual void Initialize2([MarshalAs(UnmanagedType.Interface)] object emitter, string tempfilename, [MarshalAs(UnmanagedType.Interface)] object ptrIStream, [MarshalAs(UnmanagedType.Bool)] bool fullBuild, string finalfilename) => throw new NotImplementedException();
        public virtual void DefineConstant(string name, [MarshalAs(UnmanagedType.Struct)] object value, uint sig, byte* signature) => throw new NotImplementedException();
        public virtual void Abort() => throw new NotImplementedException();
        public virtual void DefineLocalVariable2(string name, int attributes, int localSignatureToken, uint addrKind, int index, uint addr2, uint addr3, uint startOffset, uint endOffset) => throw new NotImplementedException();
        public virtual void DefineGlobalVariable2(string name, int attributes, int sigToken, uint addrKind, uint addr1, uint addr2, uint addr3) => throw new NotImplementedException();
        public virtual void DefineConstant2([MarshalAs(UnmanagedType.LPWStr)] string name, VariantStructure value, int constantSignatureToken) => throw new NotImplementedException();
        public virtual void OpenMethod2(uint methodToken, int sectionIndex, int offsetRelativeOffset) => throw new NotImplementedException();
        public virtual void Commit() => throw new NotImplementedException();
        public virtual void GetDebugInfoWithPadding(ref ImageDebugDirectory debugDirectory, uint dataCount, out uint dataCountPtr, byte* data) => throw new NotImplementedException();
        public virtual void OpenMapTokensToSourceSpans() => throw new NotImplementedException();
        public virtual void CloseMapTokensToSourceSpans() => throw new NotImplementedException();
        public virtual void MapTokenToSourceSpan(int token, ISymUnmanagedDocumentWriter document, int startLine, int startColumn, int endLine, int endColumn) => throw new NotImplementedException();
    }
}
