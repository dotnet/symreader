## About

<!-- A description of the package and where one can find more documentation -->

Microsoft.DiaSymReader provides managed definitions of the COM interfaces exposed by DiaSymReader APIs ([ISymUnmanagedReader](https://learn.microsoft.com/dotnet/framework/unmanaged-api/diagnostics/isymunmanagedreader-interface), [ISymUnmanagedWriter](https://learn.microsoft.com/dotnet/framework/unmanaged-api/diagnostics/isymunmanagedwriter-interface), [ISymUnmanagedBinder](https://learn.microsoft.com/dotnet/framework/unmanaged-api/diagnostics/isymunmanagedbinder-interface), etc.) for reading Windows and [Portable PDBs](https://github.com/dotnet/designs/blob/main/accepted/2020/diagnostics/portable-pdb.md) and writing Windows PDBs, along with factories and extension methods that make these interfaces easy to use from .NET code.

This package doesn't implement the interfaces itself:

* The implementation for Windows PDBs is provided by `Microsoft.DiaSymReader.Native.{platform}.dll`, which is available from the [Microsoft.DiaSymReader.Native](https://www.nuget.org/packages/Microsoft.DiaSymReader.Native) package. It is only supported on Windows.
* The implementation for Portable PDBs is provided by the [Microsoft.DiaSymReader.PortablePdb](https://www.nuget.org/packages/Microsoft.DiaSymReader.PortablePdb) package.

New applications and libraries should read Portable PDBs directly using the APIs provided by the [System.Reflection.Metadata](https://www.nuget.org/packages/System.Reflection.Metadata) package, which are much more efficient than DiaSymReader APIs.

## Key Features

<!-- The key features of this package -->

* Managed definitions of `ISymUnmanagedReader`, `ISymUnmanagedWriter`, `ISymUnmanagedBinder` and related COM interfaces.
* Factories that locate the native DiaSymReader implementation and create PDB readers and writers.
* Extension methods that wrap the buffer-based COM APIs and return documents, methods, sequence points, scopes, local variables, constants, and embedded source as managed objects.
* A managed PDB writer abstraction that supports deterministic PDBs and Source Link.

## How to Use

<!-- A compelling example on how to use this package with code, as well as any specific guidelines for when to use the package -->

The following example opens a Windows PDB and prints the source documents it references, followed by the sequence points and local variables of each method that has debug information.

A PDB reader needs access to the metadata of the corresponding assembly, which it gets through an `ISymReaderMetadataProvider` implementation. The example implements it using `System.Reflection.Metadata`. `ISymReaderMetadataProvider` returns signatures as pointers, so the project needs `<AllowUnsafeBlocks>true</AllowUnsafeBlocks>`.

```cs
using System;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using Microsoft.DiaSymReader;

class Program
{
    static void Main()
    {
        using var peStream = File.OpenRead("Example.dll");
        using var peReader = new PEReader(peStream);
        MetadataReader metadataReader = peReader.GetMetadataReader();

        using var pdbStream = File.OpenRead("Example.pdb");

        // Loads Microsoft.DiaSymReader.Native.{platform}.dll and initializes a reader with the PDB.
        // The PDB stream and the metadata must stay available until the reader is destroyed.
        var symReader = SymUnmanagedReaderFactory.CreateReader<ISymUnmanagedReader5>(
            pdbStream,
            new MetadataProvider(metadataReader));

        try
        {
            foreach (ISymUnmanagedDocument document in symReader.GetDocuments())
            {
                Console.WriteLine($"Document: {document.GetName()}");
            }

            foreach (MethodDefinitionHandle methodHandle in metadataReader.MethodDefinitions)
            {
                // Returns null if the PDB doesn't contain debug information for the method.
                ISymUnmanagedMethod method = symReader.GetMethod(MetadataTokens.GetToken(methodHandle));
                if (method == null)
                {
                    continue;
                }

                MethodDefinition methodDefinition = metadataReader.GetMethodDefinition(methodHandle);
                Console.WriteLine($"Method: {metadataReader.GetString(methodDefinition.Name)}");

                foreach (SymUnmanagedSequencePoint sequencePoint in method.GetSequencePoints())
                {
                    if (!sequencePoint.IsHidden)
                    {
                        Console.WriteLine(
                            $"  IL_{sequencePoint.Offset:X4}: {sequencePoint.Document.GetName()} " +
                            $"({sequencePoint.StartLine}, {sequencePoint.StartColumn})-({sequencePoint.EndLine}, {sequencePoint.EndColumn})");
                    }
                }

                ISymUnmanagedScope rootScope = method.GetRootScope();
                if (rootScope != null)
                {
                    PrintLocals(rootScope);
                }
            }
        }
        finally
        {
            // Releases all resources held by the reader.
            ((ISymUnmanagedDispose)symReader).Destroy();
        }
    }

    static void PrintLocals(ISymUnmanagedScope scope)
    {
        foreach (ISymUnmanagedVariable local in scope.GetLocals())
        {
            Console.WriteLine(
                $"  Local '{local.GetName()}' in slot {local.GetSlot()}, " +
                $"IL_{scope.GetStartOffset():X4}-IL_{scope.GetEndOffset():X4}");
        }

        foreach (ISymUnmanagedScope child in scope.GetChildren())
        {
            PrintLocals(child);
        }
    }
}

sealed class MetadataProvider : ISymReaderMetadataProvider
{
    private readonly MetadataReader _reader;

    public MetadataProvider(MetadataReader reader)
    {
        _reader = reader;
    }

    public unsafe bool TryGetStandaloneSignature(int standaloneSignatureToken, out byte* signature, out int length)
    {
        var handle = (StandaloneSignatureHandle)MetadataTokens.Handle(standaloneSignatureToken);
        if (handle.IsNil)
        {
            signature = null;
            length = 0;
            return false;
        }

        BlobReader blobReader = _reader.GetBlobReader(_reader.GetStandaloneSignature(handle).Signature);
        signature = blobReader.StartPointer;
        length = blobReader.Length;
        return true;
    }

    public bool TryGetTypeDefinitionInfo(int typeDefinitionToken, out string namespaceName, out string typeName, out TypeAttributes attributes)
    {
        var handle = (TypeDefinitionHandle)MetadataTokens.Handle(typeDefinitionToken);
        if (handle.IsNil)
        {
            namespaceName = null;
            typeName = null;
            attributes = 0;
            return false;
        }

        TypeDefinition typeDefinition = _reader.GetTypeDefinition(handle);
        namespaceName = _reader.GetString(typeDefinition.Namespace);
        typeName = _reader.GetString(typeDefinition.Name);
        attributes = typeDefinition.Attributes;
        return true;
    }

    public bool TryGetTypeReferenceInfo(int typeReferenceToken, out string namespaceName, out string typeName)
    {
        var handle = (TypeReferenceHandle)MetadataTokens.Handle(typeReferenceToken);
        if (handle.IsNil)
        {
            namespaceName = null;
            typeName = null;
            return false;
        }

        TypeReference typeReference = _reader.GetTypeReference(handle);
        namespaceName = _reader.GetString(typeReference.Namespace);
        typeName = _reader.GetString(typeReference.Name);
        return true;
    }
}
```

`SymUnmanagedReaderFactory` looks for `Microsoft.DiaSymReader.Native.{platform}.dll` in the directory `Microsoft.DiaSymReader.dll` is loaded from, the application directory, the `%WinDir%\System32` directory, and user directories in the DLL search path. The `SymUnmanagedReaderCreationOptions.UseAlternativeLoadPath` option also searches the directory specified by the `MICROSOFT_DIASYMREADER_NATIVE_ALT_LOAD_PATH` environment variable, and the `SymUnmanagedReaderCreationOptions.UseComRegistry` option falls back to the PDB reader registered by .NET Framework, which only supports interfaces up to `ISymUnmanagedReader3`.

Windows PDBs are written in a similar way: `SymUnmanagedWriterFactory.CreateWriter` takes an `ISymWriterMetadataProvider` that provides method and type information, and returns a `SymUnmanagedWriter` that can define documents, methods, sequence points, scopes and local variables, and write the PDB to a `Stream`.

## Main Types

<!-- The main types provided in this library -->

The main types provided by this library are:

* `Microsoft.DiaSymReader.SymUnmanagedReaderFactory`
* `Microsoft.DiaSymReader.ISymUnmanagedReader` (and `ISymUnmanagedReader2` through `ISymUnmanagedReader5`)
* `Microsoft.DiaSymReader.ISymReaderMetadataProvider`
* `Microsoft.DiaSymReader.SymUnmanagedExtensions`
* `Microsoft.DiaSymReader.SymUnmanagedWriterFactory`
* `Microsoft.DiaSymReader.SymUnmanagedWriter`
* `Microsoft.DiaSymReader.ISymWriterMetadataProvider`

## Additional Documentation

<!-- Links to further documentation -->

* [Diagnostics symbol store interfaces](https://learn.microsoft.com/dotnet/framework/unmanaged-api/diagnostics/diagnostics-symbol-store-interfaces)
* [Portable PDB format specification](https://github.com/dotnet/runtime/blob/main/docs/design/specs/PortablePdb-Metadata.md)

## Related Packages

<!-- The related packages associated with this package -->

* Windows PDB reader and writer implementation: [Microsoft.DiaSymReader.Native](https://www.nuget.org/packages/Microsoft.DiaSymReader.Native)
* Portable PDB reader implementation: [Microsoft.DiaSymReader.PortablePdb](https://www.nuget.org/packages/Microsoft.DiaSymReader.PortablePdb)
* Reading Portable PDBs directly: [System.Reflection.Metadata](https://www.nuget.org/packages/System.Reflection.Metadata)

## Feedback & Contributing

<!-- How to provide feedback on this package and contribute to it -->

Microsoft.DiaSymReader is released as open source under the [MIT license](https://licenses.nuget.org/MIT). Bug reports and contributions are welcome at [the GitHub repository](https://github.com/dotnet/symreader).
