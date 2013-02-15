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

namespace Probel.NDoctor.View.Core.Helpers
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;

    using Probel.Helpers.Strings;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.View.Core.Properties;
    using Probel.NDoctor.View.Plugins;

    internal class UserInteraction
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInteraction"/> class.
        /// </summary>
        public UserInteraction()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInteraction"/> class.
        /// </summary>
        /// <param name="culture">The culture to be specified to a thread if the event is called within a thread.</param>
        public UserInteraction(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        #endregion Constructors

        #region Methods

        public void NotifyNewVersion(Version remoteVersion)
        {
            var yes = ViewService.MessageBox.Question(Messages.Msg_NewVersion.FormatWith(remoteVersion));
            if (yes)
            {
                Process.Start(PluginContext.Configuration.DownloadSite);
            }
        }

        #endregion Methods
    }
}