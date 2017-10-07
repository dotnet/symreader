// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Reflection;

namespace Microsoft.DiaSymReader.Tools
{
    public sealed class DummySymWriterMetadataProvider : ISymWriterMetadataProvider
    {
        public static readonly ISymWriterMetadataProvider Instance = new DummySymWriterMetadataProvider();

        public bool TryGetEnclosingType(int nestedTypeToken, out int enclosingTypeToken) 
            => throw new NotImplementedException();

        public bool TryGetMethodInfo(int methodDefinitionToken, out string methodName, out int declaringTypeToken)
            => throw new NotImplementedException();

        public bool TryGetTypeDefinitionInfo(int typeDefinitionToken, out string namespaceName, out string typeName, out TypeAttributes attributes)
            => throw new NotImplementedException();
    }
}
