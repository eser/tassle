// --------------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="-">
// Copyright (c) 2008-2016 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/larukedi
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

//// This program is free software: you can redistribute it and/or modify
//// it under the terms of the GNU General Public License as published by
//// the Free Software Foundation, either version 3 of the License, or
//// (at your option) any later version.
//// 
//// This program is distributed in the hope that it will be useful,
//// but WITHOUT ANY WARRANTY; without even the implied warranty of
//// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//// GNU General Public License for more details.
////
//// You should have received a copy of the GNU General Public License
//// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: NeutralResourcesLanguage("en")]

[assembly: CLSCompliant(true)]

[assembly: ComVisible(false)]


//// Information about this assembly is defined by the following attributes. 
//// Change them to the values specific to your project.

[assembly: AssemblyTitle("Tasslehoff.Logging")]
[assembly: AssemblyDescription("")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Tasslehoff")]
[assembly: AssemblyCopyright("(c) 2008-2016 Eser Ozvataf (eser@ozvataf.com). All rights reserved.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

//// The assembly version has the format "{Major}.{Minor}.{Build}.{Revision}".
//// The form "{Major}.{Minor}.*" will automatically update the build and revision,
//// and "{Major}.{Minor}.{Build}.*" will update just the revision.

[assembly: AssemblyVersion("0.9.14")]
[assembly: AssemblyFileVersion("0.9.14")]
[assembly: AssemblyInformationalVersion("0.9")]

//// The following attributes are used to specify the signing key for the assembly, 
//// if desired. See the Mono documentation for more information about signing.

//// [assembly: AssemblyDelaySign(false)]
//// [assembly: AssemblyKeyFile("")]
