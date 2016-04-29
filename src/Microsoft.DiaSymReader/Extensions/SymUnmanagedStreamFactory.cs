// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace Microsoft.DiaSymReader
{
    public static class SymUnmanagedStreamFactory
    {
        public static IStream CreateStream(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanSeek)
            {
                // TODO: localize
                throw new ArgumentException("Stream must support seek operation.", nameof(stream));
            }

            return new ComStreamWrapper(stream);
        }
    }
}
