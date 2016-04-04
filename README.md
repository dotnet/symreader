# Microsoft.DiaSymReader

Provides managed definitions for COM interfaces exposed by DiaSymReader APIs ([ISymUnmanagedReader](https://msdn.microsoft.com/en-us/library/isymunmanagedreader.aspx), [ISymUnmanagedBinder](https://msdn.microsoft.com/en-us/library/isymunmanagedbinder.aspx), etc.) for reading Windows and Portable PDBs.

The implementation of these interfaces for Windows PDBs is provided by [Microsoft.DiaSymReader.Native](https://www.nuget.org/packages/Microsoft.DiaSymReader.Native) package. The implementation for Portable PDBs is provided by [Microsoft.DiaSymReader.PortablePdb](https://www.nuget.org/packages/Microsoft.DiaSymReader.PortablePdb) package. 

It is recommended that new applications and libraries read Portable PDBs directly using APIs provided by [System.Reflection.Metadata](https://www.nuget.org/packages/System.Reflection.Metadata) package. These APIs are much more efficient than DiaSymReader APIs. Microsoft.DiaSymReader.PortablePdb bridge is recommended for existings apps that already use DiaSymReader APIs and need to be able to read Portable PDBs without significant changes to their source.
