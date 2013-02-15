namespace Probel.NDoctor.View.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Probel.NDoctor.Domain.DTO.Components;

    public class DbConfiguration
    {
        #region Fields

        private const string APP_KEY = "AppKey";
        private const string ISDEBUG = "IsDebug";
        private const string IS_REMOTE_STATISTICS_ENABLED = "IsRemoteStatisticsEnabled";
        private const string NOTIFY_ON_NEW_VERSION = "NotifyOnNewVersion";
        private const string THUMBNAIL = "AreThumbnailsCreated";

        private static readonly IDbSettingsComponent Settings = PluginContext.ComponentFactory.GetInstance<IDbSettingsComponent>();

        #endregion Fields

        #region Properties

        public Guid AppKey
        {
            get
            {
                if (!Settings.Exists(APP_KEY))
                {
                    var guid = Guid.NewGuid();
                    Settings[APP_KEY] = guid.ToString();
                    return guid;
                }
                else { return Guid.Parse(Settings[APP_KEY]); }
            }
            set { Settings[APP_KEY] = value.ToString(); }
        }

        public bool AreThumbnailCreated
        {
            get
            {
                if (!Settings.Exists(THUMBNAIL))
                {
                    Settings[THUMBNAIL] = false.ToString();
                    return false;
                }
                else { return bool.Parse(Settings[THUMBNAIL]); }
            }
            set { Settings[THUMBNAIL] = value.ToString(); }
        }

        public bool IsDebug
        {
            get
            {
                if (!Settings.Exists(ISDEBUG))
                {
                    Settings[ISDEBUG] = false.ToString();
                    return false;
                }
                else { return bool.Parse(Settings[ISDEBUG]); }
            }
            set { Settings[ISDEBUG] = value.ToString(); }
        }

        public bool IsRemoteStatisticsEnabled
        {
            get
            {
                if (!Settings.Exists(IS_REMOTE_STATISTICS_ENABLED))
                {
                    Settings[IS_REMOTE_STATISTICS_ENABLED] = true.ToString();
                    return true;
                }
                else { return bool.Parse(Settings[IS_REMOTE_STATISTICS_ENABLED]); }
            }
            set { Settings[IS_REMOTE_STATISTICS_ENABLED] = value.ToString(); }
        }

        public bool NotifyOnNewVersion
        {
            get
            {
                if (!Settings.Exists(NOTIFY_ON_NEW_VERSION))
                {
                    Settings[NOTIFY_ON_NEW_VERSION] = true.ToString();
                    return true;
                }
                else { return bool.Parse(Settings[NOTIFY_ON_NEW_VERSION]); }
            }
            set { Settings[NOTIFY_ON_NEW_VERSION] = value.ToString(); }
        }

        #endregion Properties
    }
}