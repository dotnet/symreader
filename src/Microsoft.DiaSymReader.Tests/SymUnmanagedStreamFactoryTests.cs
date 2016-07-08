// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using Xunit;

namespace Microsoft.DiaSymReader.UnitTests
{
    public class SymUnmanagedStreamFactoryTests
    {
        [Fact]
        public void Errors()
        {
            Assert.Throws<ArgumentNullException>(() => SymUnmanagedStreamFactory.CreateStream(null));
        }
    }
}
