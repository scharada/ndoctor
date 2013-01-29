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

namespace Probel.NDoctor.View.Test
{
    using System;
    using System.IO;
    using System.Linq;

    using NUnit.Framework;

    using Probel.NDoctor.View.Plugins.Cfg;

    [TestFixture]
    public class TestPluginConfiguration
    {
        #region Fields

        private const string ID_ADMINISTRATION = "{C4706773-CF41-49E9-8F47-6FCEA7A86456}";
        private const int PLUGINCOUNT = 14;

        PluginsConfigurationFolder Configuration;

        #endregion Fields

        #region Methods

        [Test]
        public void Configuration_LoadConfiguration_Loaded()
        {
            Assert.AreEqual(this.Configuration.Values.Count(), PLUGINCOUNT);
        }

        [Test]
        public void SaveConfiguration_SaveDefaultData_CanReloadFromStream()
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                this.Configuration.Save(writer);

                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    var folder = PluginsConfigurationFolder.Load(reader);
                    Assert.AreEqual(folder.Values.Count(), PLUGINCOUNT);
                }
            }
        }

        [SetUp]
        public void Setup()
        {
            this.Configuration = PluginsConfigurationFolder.LoadDefault();
        }

        [Test]
        public void UseConfiguration_AddConfiguration_ConfigurationAdded()
        {
            var randomText = Guid.NewGuid().ToString();
            this.Configuration.Add(new PluginConfiguration()
            {
                Id = Guid.NewGuid(),
                Explanations = randomText,
                IsActivated = true,
                IsMandatory = true,
                Name = randomText,
                IsRecommended = true,
            });

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                this.Configuration.Save(writer);

                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    var config = PluginsConfigurationFolder.Load(reader);
                    Assert.AreEqual(PLUGINCOUNT + 1, config.Values.Count());
                }
            }
        }

        [Test]
        public void UseConfiguration_FindFromId_IdFound()
        {
            var pluginCfg = this.Configuration[ID_ADMINISTRATION];

            Assert.NotNull(pluginCfg, "No plugin configuration found");
        }

        [Test]
        public void UseConfiguration_RemoveConfiguration_ConfigurationRemoved()
        {
            var randomText = Guid.NewGuid().ToString();
            var cfg = new PluginConfiguration()
            {
                Id = Guid.NewGuid(),
                Explanations = randomText,
                IsActivated = true,
                IsMandatory = true,
                Name = randomText,
                IsRecommended = true,
            };
            this.Configuration.Add(cfg);
            Assert.IsTrue(this.Configuration.Remove(cfg));

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                this.Configuration.Save(writer);

                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    var config = PluginsConfigurationFolder.Load(reader);
                    Assert.AreEqual(PLUGINCOUNT, config.Values.Count());
                }
            }
        }

        [Test]
        public void UseConfiguration_RemoveNotExistingConfiguration_FalseIsReturned()
        {
            var randomText = Guid.NewGuid().ToString();
            var cfg = new PluginConfiguration()
            {
                Id = Guid.NewGuid(),
                Explanations = randomText,
                IsActivated = true,
                IsMandatory = true,
                Name = randomText,
                IsRecommended = true,
            };
            this.Configuration.Add(cfg);
            Assert.IsFalse(this.Configuration.Remove(new PluginConfiguration() { Id = Guid.NewGuid() }));
        }

        #endregion Methods
    }
}