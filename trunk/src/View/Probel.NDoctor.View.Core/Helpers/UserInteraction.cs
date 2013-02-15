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
using System;
using System.Diagnostics;
using Probel.Helpers.Strings;
using Probel.Mvvm.Gui;
using Probel.NDoctor.View.Core.Properties;
using Probel.NDoctor.View.Plugins;
using System.Threading;
using System.Globalization;

namespace Probel.NDoctor.View.Core.Helpers
{
    internal class UserInteraction
    {
        public UserInteraction()
        {

        }
        public UserInteraction(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentUICulture = culture;
        }
        public void NotifyNewVersion(Version remoteVersion)
        {
            var yes = ViewService.MessageBox.Question(Messages.Msg_NewVersion.FormatWith(remoteVersion));
            if (yes)
            {
                Process.Start(PluginContext.Configuration.DownloadSite);
            }
        }
    }
}
