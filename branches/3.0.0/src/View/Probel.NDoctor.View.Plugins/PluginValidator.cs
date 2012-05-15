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
namespace Probel.NDoctor.View.Plugins
{
    using System;

    using Probel.Helpers.Assertion;

    /// <summary>
    /// This validator checks the version of the plugin with the version constraint specified.
    /// </summary>
    public class PluginValidator
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginValidator"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="mode">The validation mode.</param>
        public PluginValidator(Version version, ValidationMode mode)
        {
            this.Version = version;
            this.Mode = mode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginValidator"/> class.
        /// </summary>
        /// <param name="version">The version represented as a string.</param>
        /// <param name="mode">The validation mode.</param>
        /// <exception cref="System.ArgumentException">version has fewer than two components or more than four components.</exception>
        /// <exception cref="System.ArgumentNullException">version is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">A major, minor, build, or revision component is less than zero.</exception>
        /// <exception cref="System.FormatException">At least one component of version does not parse to an integer.</exception>
        /// <exception cref="System.OverflowException">At least one component of version represents a number greater than System.Int32.MaxValue.</exception>   
        public PluginValidator(string version, ValidationMode mode)
            : this(new Version(version), mode)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the mode the constraint should be tested.
        /// </summary>
        public ValidationMode Mode
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the version the plugin should have.
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