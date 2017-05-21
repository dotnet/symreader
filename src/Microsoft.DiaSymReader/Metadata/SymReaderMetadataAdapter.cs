﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;

namespace Microsoft.DiaSymReader
{
    /// <summary>
    /// Minimal implementation of IMetadataImport that implements APIs used by SymReader.
    /// </summary>
    internal unsafe sealed class SymReaderMetadataAdapter : MetadataAdapterBase
    {
        private readonly ISymReaderMetadataProvider _metadataProvider;

        public SymReaderMetadataAdapter(ISymReaderMetadataProvider metadataProvider)
        {
            Debug.Assert(metadataProvider != null);
            _metadataProvider = metadataProvider;
        }

        public override int GetSigFromToken(
            int standaloneSignature, 
            [Out]byte** signature,
            [Out]int* signatureLength)
        {
            // Happens when a constant doesn't have a signature:
            // The caller expect the signature to have at least one byte on success, 
            // so we need to fail here. Otherwise AV happens.
            var hr = _metadataProvider.TryGetStandaloneSignature(standaloneSignature, out byte* sig, out int length) ? HResult.S_OK : HResult.E_INVALIDARG;

            if (signature != null)
            {
                *signature = sig;
            }

            if (signatureLength != null)
            {
                *signatureLength = length;
            }

            return hr;
        }

        public override int GetTypeDefProps(
            int typeDef,
            [Out]char* qualifiedName,
            int qualifiedNameBufferLength,
            [Out]int* qualifiedNameLength,
            [Out]TypeAttributes* attributes,
            [Out]int* baseType)
        {
            if (!_metadataProvider.TryGetTypeDefinitionInfo(typeDef, out var namespaceName, out var typeName, out var attrib, out var baseTypeToken))
            {
                return HResult.E_INVALIDARG;
            }

            if (qualifiedNameLength != null || qualifiedName != null)
            {
                InteropUtilities.CopyQualifiedTypeName(
                    qualifiedName,
                    qualifiedNameLength,
                    namespaceName,
                    typeName);
            }

            if (attributes != null)
            {
                *attributes = attrib;
            }

            if (baseType != null)
            {
                *baseType = baseTypeToken;
            }

            return HResult.S_OK;
        }

        public override int GetTypeRefProps(
            int typeRef,
            [Out]int* resolutionScope, // ModuleRef or AssemblyRef
            [Out]char* qualifiedName,
            int qualifiedNameBufferLength,
            [Out]int* qualifiedNameLength)
        {
            if (!_metadataProvider.TryGetTypeReferenceInfo(typeRef, out var namespaceName, out var typeName, out int resolutionScopeToken))
            {
                return HResult.E_INVALIDARG;
            }

            if (qualifiedNameLength != null || qualifiedName != null)
            {
                InteropUtilities.CopyQualifiedTypeName(
                    qualifiedName,
                    qualifiedNameLength,
                    namespaceName,
                    typeName);
            }

            if (resolutionScope != null)
            {
                *resolutionScope = resolutionScopeToken;
            }

            return HResult.S_OK;
        }
    }
}