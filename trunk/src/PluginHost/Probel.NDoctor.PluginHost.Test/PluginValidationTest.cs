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
namespace Probel.NDoctor.PluginHost.Test
{
    using System;

    using NUnit.Framework;

    using Probel.NDoctor.PluginHost.Core;
    using Probel.NDoctor.PluginHost.Test.Stubs;

    using Rhino.Mocks;

    [TestFixture]
    public class PluginValidationTest
    {
        #region Methods

        [Test]
        public void Invalid_DBVersion_Above_PluginVersion_Strict()
        {
            var plugin = this.CreatePlugin(new Version("1.0.0.0"), Constraints.Strict);
            plugin.Validate(new Version("2.0.0.0"));
            Assert.IsTrue(plugin.OnError);
        }

        [Test]
        public void Invalid_DBVersion_Below_PluginVersion_Minimum()
        {
            var plugin = this.CreatePlugin(new Version("2.0.0.0"), Constraints.Minimum);
            plugin.Validate(new Version("1.0.0.0"));
            Assert.IsTrue(plugin.OnError);
        }

        [Test]
        public void Invalid_DBVersion_Below_PluginVersion_Strict()
        {
            var plugin = this.CreatePlugin(new Version("2.0.0.0"), Constraints.Strict);
            plugin.Validate(new Version("1.0.0.0"));
            Assert.IsTrue(plugin.OnError);
        }

        [Test]
        public void Valid_DBVersion_Above_PluginVersion_Minimum()
        {
            var plugin = this.CreatePlugin(new Version("1.0.0.0"), Constraints.Minimum);
            plugin.Validate(new Version("2.0.0.0"));
            Assert.IsFalse(plugin.OnError);
        }

        [Test]
        public void Valid_DBVersion_Equals_PluginVersion_Minimum()
        {
            var plugin = this.CreatePlugin(new Version("1.0.0.0"), Constraints.Minimum);
            plugin.Validate(new Version("1.0.0.0"));
            Assert.IsFalse(plugin.OnError);
        }

        [Test]
        public void Valid_DBVersion_Equals_PluginVersion_Strict()
        {
            var plugin = this.CreatePlugin(new Version("1.0.0.0"), Constraints.Strict);
            plugin.Validate(new Version("1.0.0.0"));
            Assert.IsFalse(plugin.OnError);
        }

        private PluginStub CreatePlugin(Version version, Constraints constraint)
        {
            var host = MockRepository.GenerateStub<IPluginHost>();
            var plugin = new PluginStub(host, version, constraint);
            return plugin;
        }

        #endregion Methods
    }
}