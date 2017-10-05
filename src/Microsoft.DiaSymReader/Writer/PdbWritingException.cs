// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;

namespace Microsoft.DiaSymReader
{
    /// <summary>
    /// Exception reported when PDB write operation fails.
    /// </summary>
    public sealed class PdbWritingException : Exception
    {
        public PdbWritingException()
        {
        }

        public PdbWritingException(string message) : base(message)
        {
        }

        public PdbWritingException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        public PdbWritingException(Exception innerException) :
            base(innerException.Message, innerException)
        {
        }
    }
}
