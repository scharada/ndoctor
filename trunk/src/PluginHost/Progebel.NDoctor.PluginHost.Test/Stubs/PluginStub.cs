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
    along with Foobar.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace Progebel.NDoctor.PluginHost.Test.Stubs
{
    using System;

    using Progebel.NDoctor.PluginHost.Core;

    public class PluginStub : Plugin
    {
        #region Constructors

        public PluginStub(IPluginHost host, Version version)
            : this(host, version, Constraints.Strict)
        {
        }

        public PluginStub(IPluginHost host, Version version, Constraints constraint)
        {
            this.Host = host;
            this.Constraint = new DatabaseConstraint()
            {
                Constraint = constraint,
                Version = version,
            };
        }

        #endregion Constructors

        #region Methods

        protected override void DisplayGuiOnError()
        {
            //Do nothing
        }

        /// <summary>
        /// Save the state of the plugin.
        /// </summary>
        public override void Save()
        {
            //Do nothing
        }

        #endregion Methods
    }
}