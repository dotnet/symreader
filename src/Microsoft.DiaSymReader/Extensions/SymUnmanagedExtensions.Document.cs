// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.
using System;

namespace Microsoft.DiaSymReader
{
    using static InteropUtilities;

    partial class SymUnmanagedExtensions
    {
        public static string GetName(this ISymUnmanagedDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return BufferToString(GetItems(document,
                (ISymUnmanagedDocument a, int b, out int c, char[] d) => a.GetUrl(b, out c, d)));
        }

        public static byte[] GetChecksum(this ISymUnmanagedDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return NullToEmpty(GetItems(document,
                (ISymUnmanagedDocument a, int b, out int c, byte[] d) => a.GetChecksum(b, out c, d)));
        }

        public static Guid GetLanguage(this ISymUnmanagedDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            Guid result = default(Guid);
            ThrowExceptionForHR(document.GetLanguage(ref result));
            return result;
        }

        public static Guid GetLanguageVendor(this ISymUnmanagedDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            Guid result = default(Guid);
            ThrowExceptionForHR(document.GetLanguageVendor(ref result));
            return result;
        }

        public static Guid GetDocumentType(this ISymUnmanagedDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            Guid result = default(Guid);
            ThrowExceptionForHR(document.GetDocumentType(ref result));
            return result;
        }

        public static Guid GetHashAlgorithm(this ISymUnmanagedDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            Guid result = default(Guid);
            ThrowExceptionForHR(document.GetChecksumAlgorithmId(ref result));
            return result;
        }
    }
}
