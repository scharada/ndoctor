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
namespace Progebel.NDoctor.PluginHost.Test
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    using Progebel.NDoctor.PluginHost.Core;
    using Progebel.NDoctor.PluginHost.Test.Stubs;

    using Rhino.Mocks;

    [TestFixture]
    public class SetupPluginTest
    {
        #region Methods

        [Test]
        public void CanSetupValidPlugins()
        {
            var container = this.CreatePluginContainer(new Version("1.0.0.0"), new Version("1.0.0.0"));
            container.SetupPlugins();

            Assert.IsFalse(container.HasPluginOnError, "No plugin should be on error.");
        }

        [Test]
        public void FailsSetupOnInvalidPlugins()
        {
            var container = this.CreatePluginContainer(new Version("1.0.0.34"), new Version("1.0.0.0"));
            container.SetupPlugins();

            Assert.IsTrue(container.HasPluginOnError, "At least one plugin hould be on error.");
        }

        private PluginContainer CreatePluginContainer(Version databaseVersion, Version pluginVersion)
        {
            var host = MockRepository.GenerateStub<IPluginHost>();

            var loader = MockRepository.GenerateStub<IPluginLoader>();
            loader.Stub(x => x.Plugins).Return(this.GetPlugins(pluginVersion));

            var container = new PluginContainer(host, loader, databaseVersion);
            return container;
        }

        private List<IPlugin> GetPlugins(Version version)
        {
            var host = MockRepository.GenerateStub<IPluginHost>();
            return new List<IPlugin>()
            {
                new PluginStub(host,version),
                new PluginStub(host,version),
                new PluginStub(host,version),
            };
        }

        #endregion Methods
    }
}