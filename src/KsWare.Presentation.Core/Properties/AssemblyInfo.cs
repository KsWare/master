﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("KsWare Presentation - Core Library")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("KsWare")]
[assembly: AssemblyProduct("Presentation Framework")]
[assembly: AssemblyCopyright("Copyright © 2002-2016 by KsWare. All rights reserved.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("4341f602-848b-42fc-ad38-e86f0820e3f4")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("1.0.0.0")]

[assembly: InternalsVisibleTo("KsWare.Presentation.BusinessFramework, PublicKey=002400000480000094000000060200000024000052534131000400000100010041A176886DD69E39D1B9A10017A286FBF650E8F0CB84879B097856B3DFDD194CB06561F36780A9AD61BA8A69DEC80B4FAE69D8723BD67ED3052E82A10E221159DF072118B887BE867A299EB12A1256741E0230FDECF6BA9A806034E1C543EABD9E2CC21DCBE9B61463DB6635B0867EDA7E588409A155D97E17162257B61DECB4")]
[assembly: InternalsVisibleTo("KsWare.Presentation.ViewModelFramework, PublicKey=002400000480000094000000060200000024000052534131000400000100010041A176886DD69E39D1B9A10017A286FBF650E8F0CB84879B097856B3DFDD194CB06561F36780A9AD61BA8A69DEC80B4FAE69D8723BD67ED3052E82A10E221159DF072118B887BE867A299EB12A1256741E0230FDECF6BA9A806034E1C543EABD9E2CC21DCBE9B61463DB6635B0867EDA7E588409A155D97E17162257B61DECB4")]
[assembly: InternalsVisibleTo("KsWare.Presentation.ViewFramework, PublicKey=002400000480000094000000060200000024000052534131000400000100010041A176886DD69E39D1B9A10017A286FBF650E8F0CB84879B097856B3DFDD194CB06561F36780A9AD61BA8A69DEC80B4FAE69D8723BD67ED3052E82A10E221159DF072118B887BE867A299EB12A1256741E0230FDECF6BA9A806034E1C543EABD9E2CC21DCBE9B61463DB6635B0867EDA7E588409A155D97E17162257B61DECB4")]
[assembly: InternalsVisibleTo("KsWare.Presentation.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100D3A0B493BDA975526F908712B964179DF870A583523EB8470B1AA09D97F61B665D1F96D4F495751C3F0D6FF08DC49BF4B22731BE741E1C8861AF28F7453B1DE06BBFD4B5EBAF5F3BF928480EBE8A47877EE1C3E48BA4F906729EC9234934E5AFFB8E7E46B7451FFB0FF1298DC09BDEA2DD0C1B1DCAE7C409C1B8868C5AF2A1BA")]
[assembly: InternalsVisibleTo("KsWare.Presentation.Labs, PublicKey=002400000480000094000000060200000024000052534131000400000100010041A176886DD69E39D1B9A10017A286FBF650E8F0CB84879B097856B3DFDD194CB06561F36780A9AD61BA8A69DEC80B4FAE69D8723BD67ED3052E82A10E221159DF072118B887BE867A299EB12A1256741E0230FDECF6BA9A806034E1C543EABD9E2CC21DCBE9B61463DB6635B0867EDA7E588409A155D97E17162257B61DECB4")]


// namespace must equal to assembly name
// ReSharper disable once CheckNamespace
namespace KsWare.Presentation.Core {

	public static class AssemblyInfo {

		public static Assembly Assembly => Assembly.GetExecutingAssembly();

	}
}