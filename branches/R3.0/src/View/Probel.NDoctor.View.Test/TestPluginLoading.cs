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
namespace Probel.NDoctor.View.Test
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Test.Stubs;

    using Rhino.Mocks;
    using Probel.NDoctor.View.Plugins.Helpers;

    [TestFixture]
    [Category("View")]
    public class TestPluginLoading
    {
        #region Fields

        private PluginContainer container;
        private IPluginLoader loader;

        #endregion Fields

        #region Methods

        [Test]
        public void CanLoadValidPlugins()
        {
            PluginContext.Host.Stub(x => x.HostVersion).Return(new Version("1.0.0.0"));
            container.Plugins = this.CreatePlugins("1.0.0.0", new PluginValidator("1.0.0.0", ValidationMode.Strict));
            container.LoadPlugins();

            foreach (var plugin in container.Plugins) Assert.IsTrue(plugin.IsActive);
        }

        [Test]
        public void FailToLoadInvalidPlugins()
        {
            PluginContext.Host.Stub(x => x.HostVersion).Return(new Version("2.0.0.0"));
            container.Plugins = this.CreatePlugins("1.0.0.0", new PluginValidator("4.0.0.0", ValidationMode.Strict));
            container.LoadPlugins();

            foreach (var plugin in container.Plugins) Assert.IsFalse(plugin.IsActive);
        }

        [TestFixtureSetUp]
        public void PluginFixture()
        {
            PluginContext.Host = MockRepository.GenerateMock<IPluginHost>();
            this.loader = MockRepository.GenerateMock<IPluginLoader>();
            this.container = new PluginContainer(PluginContext.Host, loader);
        }

        private IList<IPlugin> CreatePlugins(string version, PluginValidator validator)
        {
            return new List<IPlugin>()
            {
                new PluginStub(version,PluginContext.Host, validator),
                new PluginStub(version,PluginContext.Host, validator),
                new PluginStub(version,PluginContext.Host, validator),
                new PluginStub(version,PluginContext.Host, validator),
                new PluginStub(version,PluginContext.Host, validator),
            };
        }

        private PluginValidator CreatePluginValidator(string version)
        {
            return new PluginValidator(version, ValidationMode.Strict);
        }

        #endregion Methods
    }
}