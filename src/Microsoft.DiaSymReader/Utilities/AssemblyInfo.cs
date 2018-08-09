#if NET20

[assembly: System.Runtime.Versioning.TargetFramework(".NETFramework,Version=v2.0")]

#else

[assembly: System.Security.AllowPartiallyTrustedCallers]

#endif