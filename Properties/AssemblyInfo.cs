using System.Reflection;
using System.Runtime.InteropServices;
using Colt.Nuget.Utilities;

[assembly:AssemblyTitle("nuget_test")]
[assembly:AssemblyProduct("nuget_test")]
[assembly:AssemblyDescription("description of nuget_test.")]
[assembly:AssemblyCompany("Colt Manufacturing Company, LLC.")]
[assembly:AssemblyCopyright("Copyright © 2016, Colt Manufacturing Company, LLC.")]
#if DEBUG
[assembly:AssemblyConfiguration("Debug version")]
#else
[assembly:AssemblyConfiguration("Release version")]
#endif
[assembly:ComVisible(false)]

//[assembly: LicenseUrl("http://no_license")]
//[assembly: ProjectUrl("http://no_project")]
[assembly: Authors("Rik Cousens")]
[assembly: Owners("Colt Manufacturing Company, LLC.")]
[assembly: ReleaseNotes("Release notes for 'nuget_test'")]


[assembly:AssemblyVersion("1.0.1.0")]
[assembly:AssemblyFileVersion("1.0.0.0")]
[assembly:AssemblyInformationalVersion("1.0.0.0")]

