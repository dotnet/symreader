// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;

namespace Microsoft.DiaSymReader
{
    using static InteropUtilities;

    partial class SymUnmanagedExtensions
    {
        public static string GetName(this ISymUnmanagedNamespace @namespace)
        {
            if (@namespace == null)
            {
                throw new ArgumentNullException(nameof(@namespace));
            }

            return BufferToString(GetItems(@namespace,
                (ISymUnmanagedNamespace a, int b, out int c, char[] d) => a.GetName(b, out c, d)));
        }
    }
}
