#region Header

/*
    This file is part of NDoctor.

    NDoctor is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    NDoctor is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with NDoctor.  If not, see <http://www.gnu.org/licenses/>.
*/

#endregion Header

namespace Probel.NDoctor.View.Plugins
{
    using System;

    using Probel.Helpers.Assertion;

    /// <summary>
    /// The plugin is valid when this constraint is respected
    /// </summary>
    public class Constraint
    {
        #region Fields

        /// <summary>
        /// The name of the <see cref="PartMetadata"/> first argument. This is a convenience property
        /// </summary>
        public const string Name = "Constraint";

        #endregion Fields

        #region Constructors

        public Constraint(string value)
        {
            this.Mode = this.GetMode(value[0].ToString());
            this.Version = new Version(value.Substring(1));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the type of validation that will be applyed on the plugin version
        /// </summary>
        public ValidationMode Mode
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets version this plugin should have.
        /// </summary>
        public Version Version
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Determines whether the specified plugin is valid for the constraint.
        /// </summary>
        /// <param name="version">The plugin to validate.</param>
        /// <returns>
        ///   <c>true</c> if the specified version is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValid(IPluginHost host)
        {
            return this.IsValid(host.HostVersion);
        }

        private ValidationMode GetMode(string mode)
        {
            ValidationMode vmode = ValidationMode.Strict;
            switch (mode)
            {
                case ">":
                    vmode = ValidationMode.Minimum;
                    break;
                case "=":
                    vmode = ValidationMode.Strict;
                    break;
                default:
                    Assert.FailOnEnumeration(mode);
                    break;
            }
            return vmode;
        }

        /// <summary>
        /// Determines whether the specified version is valid.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns>
        ///   <c>true</c> if the specified version is valid; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValid(Version version)
        {
            var result = false;
            switch (this.Mode)
            {
                case ValidationMode.Strict:
                    result = (this.Version == version);
                    break;
                case ValidationMode.Minimum:
                    result = (this.Version <= version);
                    break;
                default:
                    Assert.FailOnEnumeration(this.Mode);
                    break;
            }
            return result;
        }

        #endregion Methods
    }
}