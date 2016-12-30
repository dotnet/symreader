// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.


namespace Microsoft.DiaSymReader
{
    /// <summary>
    /// Line deltas allow a compiler to omit functions that have not been modified from
    /// the pdb stream provided the line information meets the following condition.
    /// The correct line information can be determined with the old pdb line info and
    /// one delta for all lines in the function.
    /// </summary>
    public struct SymUnmanagedLineDelta
    {
        public readonly int MethodToken;
        public readonly int Delta;

        public SymUnmanagedLineDelta(int methodToken, int delta)
        {
            MethodToken = methodToken;
            Delta = delta;
        }
    }
}
