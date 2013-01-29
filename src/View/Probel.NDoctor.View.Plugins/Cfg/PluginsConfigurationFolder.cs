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

namespace Probel.NDoctor.View.Plugins.Cfg
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    using Probel.NDoctor.View.Plugins.Properties;

    [Serializable]
    public class PluginsConfigurationFolder
    {
        #region Fields

        private readonly List<PluginConfiguration> values;

        #endregion Fields

        #region Constructors

        private PluginsConfigurationFolder(List<PluginConfiguration> values)
        {
            this.values = values;
        }

        #endregion Constructors

        #region Properties

        public IEnumerable<PluginConfiguration> Values
        {
            get { return this.values; }
        }

        #endregion Properties

        #region Indexers

        public PluginConfiguration this[string key]
        {
            get
            {
                try
                {
                    var id = Guid.Parse(key);
                    return (from value in this.values
                            where value.Id == id
                            select value).Single();
                }
                catch (NotSupportedException ex)
                {
                    throw new NotSupportedException(string.Format(
                        "The element with id '{0}' is not or is more than once in the plugin configuration folder", key)
                        , ex);
                }
            }
        }

        #endregion Indexers

        #region Methods

        /// <summary>
        /// Loads the configuration contained into the specified file
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static PluginsConfigurationFolder Load(string path)
        {
            using (var stream = new FileInfo(path).OpenRead())
            using (var reader = new StreamReader(stream))
            {
                return Load(reader);
            }
        }

        /// <summary>
        /// Loads the specified configuration contained into the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static PluginsConfigurationFolder Load(TextReader stream)
        {
            var data = (List<PluginConfiguration>)new XmlSerializer(typeof(List<PluginConfiguration>)).Deserialize(stream);
            return new PluginsConfigurationFolder(data);
        }

        /// <summary>
        /// Loads the default configuration.
        /// </summary>
        /// <returns></returns>
        public static PluginsConfigurationFolder LoadDefault()
        {
            var folder = new PluginsConfigurationFolder(new List<PluginConfiguration>());
            folder.values.Add(new PluginConfiguration()
            {
                Id = Guid.Parse("{C4706773-CF41-49E9-8F47-6FCEA7A86456}"),
                IsActivated = true,
                Name = Messages.Plugin_Administration,
                Explanations = Messages.Plugin_Administration_Explanations,
                IsRecommended = true,
                IsMandatory = true,
            });

            folder.values.Add(new PluginConfiguration()
            {
                Id = Guid.Parse("{584D7616-248E-4985-AC3F-66C07958E646}"),
                IsActivated = true,
                Name = Messages.Plugin_Authorisation,
                Explanations = Messages.Plugin_Authorisation_Explanations,
                IsRecommended = true,
                IsMandatory = false,
            });

            folder.values.Add(new PluginConfiguration()
            {
                Id = Guid.Parse("{AF500BD6-A57A-476A-B42E-8D667E5270C3}"),
                IsActivated = true,
                Name = Messages.Plugin_Bmi,
                Explanations = Messages.Plugin_Bmi_Explanations,
                IsRecommended = false,
                IsMandatory = false,
            });

            folder.values.Add(new PluginConfiguration()
            {
                Id = Guid.Parse("{1A5224ED-3E37-4AD8-AB2B-FBC0115434FA}"),
                IsActivated = false,
                Name = Messages.Plugin_DbImport,
                Explanations = Messages.Plugin_DbImport_Explanations,
                IsRecommended = false,
                IsMandatory = false,
            });

            folder.values.Add(new PluginConfiguration()
            {
                Id = Guid.Parse("{6185C382-52D0-4E44-B854-BC2B619DE849}"),
                IsActivated = true,
                Name = Messages.Plugin_FamilyManager,
                Explanations = Messages.Plugin_FamilyManager_Explanations,
                IsRecommended = false,
                IsMandatory = false,
            });

            folder.values.Add(new PluginConfiguration()
            {
                Id = Guid.Parse("{1B564972-40E6-45EA-B7BE-C1FF1B84D016}"),
                IsActivated = true,
                Name = Messages.Plugin_MedicalRecordManager,
                Explanations = Messages.Plugin_MedicalRecordManager_Explanations,
                IsRecommended = true,
                IsMandatory = false,
            });

            folder.values.Add(new PluginConfiguration()
            {
                Id = Guid.Parse("{B2C14EF3-8B01-4D0F-B74F-8B26D87E274C}"),
                IsActivated = true,
                Name = Messages.Plugin_MeetingManager,
                Explanations = Messages.Plugin_MeetingManager_Explanations,
                IsRecommended = true,
                IsMandatory = false,
            });

            folder.values.Add(new PluginConfiguration()
            {
                Id = Guid.Parse("{7E999CF5-886D-4BEC-A7B8-903AE2047A6C}"),
                IsActivated = true,
                Name = Messages.Plugin_PathologyManager,
                Explanations = Messages.Plugin_PathologyManager_Explanations,
                IsRecommended = false,
                IsMandatory = false,
            });

            folder.values.Add(new PluginConfiguration()
            {
                Id = Guid.Parse("{13DADC37-7A9B-4126-971D-AB73DAE601C0}"),
                IsActivated = false,
                Name = Messages.Plugin_PatientDataManager,
                Explanations = Messages.Plugin_PatientDataManager_Explanations,
                IsRecommended = false,
                IsMandatory = false,
            });

            folder.values.Add(new PluginConfiguration()
            {
                Id = Guid.Parse("{7D16F7FE-87D8-4435-AF23-7593379E4986}"),
                IsActivated = true,
                Name = Messages.Plugin_PatientDataManager2,
                Explanations = Messages.Plugin_PatientDataManager_Explanations,
                IsRecommended = true,
                IsMandatory = false,
            });

            folder.values.Add(new PluginConfiguration()
            {
                Id = Guid.Parse("{FD1502BB-8A85-4705-A7B1-49B5CCE1E7FD}"),
                IsActivated = true,
                Name = Messages.Plugin_PatientSessionManager,
                Explanations = Messages.Plugin_PatientSessionManager_Explanations,
                IsRecommended = true,
                IsMandatory = true,
            });

            folder.values.Add(new PluginConfiguration()
            {
                Id = Guid.Parse("{8A4CF3E8-DA69-4568-8387-6F175457DD02}"),
                IsActivated = true,
                Name = Messages.Plugin_PictureManager,
                Explanations = Messages.Plugin_PictureManager_Explanations,
                IsRecommended = false,
                IsMandatory = false,
            });

            folder.values.Add(new PluginConfiguration()
            {
                Id = Guid.Parse("{283FDC8B-FA71-44D1-9750-3C0413B36008}"),
                IsActivated = true,
                Name = Messages.Plugin_PrescriptionManager,
                Explanations = Messages.Plugin_PrescriptionManager_Explanations,
                IsRecommended = false,
                IsMandatory = false,
            });

            folder.values.Add(new PluginConfiguration()
            {
                Id = Guid.Parse("{AAC4444F-40C0-4D53-9CAA-F615751D87C3}"),
                IsActivated = true,
                Name = Messages.Plugin_UserSessionManager,
                Explanations = Messages.Plugin_UserSessionManager_Explanations,
                IsRecommended = true,
                IsMandatory = true,
            });

            return folder;
        }

        /// <summary>
        /// Adds the specified plugin configuration into the configuration folder.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void Add(PluginConfiguration configuration)
        {
            this.values.Add(configuration);
        }

        /// <summary>
        /// Adds the specified plugin configurations into the configuration folder.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void AddRange(IEnumerable<PluginConfiguration> cfg)
        {
            foreach (var c in cfg)
            {
                this.Add(c);
            }
        }

        /// <summary>
        /// Clears all the configurations
        /// </summary>
        public void Clear()
        {
            this.values.Clear();
        }

        /// <summary>
        /// Removes the specified configuration from the folder.  If the configuration
        /// is not in the folder, nothing is done and the method returns <c>False</c>. Otherwise,
        /// the specified configuration is removed and <c>True</c> is returned.
        /// </summary>
        /// <param name="cfg">The configuration to remove.</param>
        /// <returns></returns>
        public bool Remove(PluginConfiguration cfg)
        {
            var found = (from value in this.values
                         where value.Id == cfg.Id
                         select value).SingleOrDefault();
            if (found == null) { return false; }
            else
            {
                this.values.Remove(found);
                return true;
            }
        }

        /// <summary>
        /// Saves the plugin configuration into the specified file.
        /// </summary>
        /// <param name="path">The temp file.</param>
        public void Save(string path)
        {
            using (var stream = File.OpenWrite(path))
            using (var writer = new StreamWriter(stream))
            {
                this.Save(writer);
            }
        }

        /// <summary>
        /// Saves the plugin configuration into the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Save(TextWriter stream)
        {
            new XmlSerializer(typeof(List<PluginConfiguration>)).Serialize(stream, this.values);
        }

        #endregion Methods
    }
}