// --------------------------------------------------------------------------
// <copyright file="DynamicAssembly.cs" company="-">
// Copyright (c) 2008-2015 Eser Ozvataf (eser@sent.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/larukedi
// </copyright>
// <author>Eser Ozvataf (eser@sent.com)</author>
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
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Tasslehoff.Dynamic
{
    /// <summary>
    /// DynamicAssembly class.
    /// </summary>
    public class DynamicAssembly
    {
        // fields

        /// <summary>
        /// The application domain.
        /// </summary>
        private AppDomain appDomain;

        /// <summary>
        /// The assembly name.
        /// </summary>
        private AssemblyName assemblyName;

        /// <summary>
        /// The assembly builder.
        /// </summary>
        private AssemblyBuilder assemblyBuilder;

        /// <summary>
        /// The module builder.
        /// </summary>
        private ModuleBuilder moduleBuilder;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicAssembly"/> class.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly</param>
        public DynamicAssembly(string assemblyName)
        {
            this.appDomain = Thread.GetDomain();
            this.assemblyName = new AssemblyName(assemblyName);

            this.assemblyBuilder = this.appDomain.DefineDynamicAssembly(
                this.assemblyName,
                AssemblyBuilderAccess.RunAndSave
            );

            this.moduleBuilder = this.assemblyBuilder.DefineDynamicModule(assemblyName, assemblyName + ".dll");
        }

        // properties

        /// <summary>
        /// Gets or sets the application domain.
        /// </summary>
        /// <value>
        /// The application domain.
        /// </value>
        public AppDomain AppDomain
        {
            get
            {
                return this.appDomain;
            }
            set
            {
                this.appDomain = value;
            }
        }

        /// <summary>
        /// Gets or sets the assembly name.
        /// </summary>
        /// <value>
        /// The assembly name.
        /// </value>
        public AssemblyName AssemblyName
        {
            get
            {
                return this.assemblyName;
            }
            set
            {
                this.assemblyName = value;
            }
        }

        /// <summary>
        /// Gets or sets the assembly builder.
        /// </summary>
        /// <value>
        /// The assembly builder.
        /// </value>
        public AssemblyBuilder AssemblyBuilder
        {
            get
            {
                return this.assemblyBuilder;
            }
            set
            {
                this.assemblyBuilder = value;
            }
        }

        /// <summary>
        /// Gets or sets the module builder.
        /// </summary>
        /// <value>
        /// The module builder.
        /// </value>
        public ModuleBuilder ModuleBuilder
        {
            get
            {
                return this.moduleBuilder;
            }
            set
            {
                this.moduleBuilder = value;
            }
        }

        // methods

        /// <summary>
        /// Saves this dynamic assembly to disk.
        /// </summary>
        /// <param name="path">The file path of the assembly</param>
        public void Save(string path = null)
        {
            string assemblyPath = this.AssemblyName.Name + ".dll";
            this.AssemblyBuilder.Save(assemblyPath);

            if (!string.IsNullOrEmpty(path))
            {
                File.Move(assemblyPath, path);
            }
        }

        /// <summary>
        /// Adds a type to assembly.
        /// </summary>
        /// <param name="name">Name of the type</param>
        /// <param name="baseType">Base type of the type</param>
        /// <param name="isSerializable">Weather if serializable or not</param>
        /// <returns>Type instance</returns>
        public DynamicType AddClass(string name, Type baseType = null, bool isSerializable = false)
        {
            TypeAttributes typeAttributes = TypeAttributes.Public |
                TypeAttributes.Class;

            if (isSerializable)
            {
                typeAttributes |= TypeAttributes.Serializable;
            }


            TypeBuilder typeBuilder = this.ModuleBuilder.DefineType(name, typeAttributes, baseType ?? typeof(object));                
            return new DynamicType(typeBuilder);
        }
    }
}
