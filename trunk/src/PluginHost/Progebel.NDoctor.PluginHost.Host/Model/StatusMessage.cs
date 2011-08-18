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
namespace Progebel.NDoctor.PluginHost.Host.Model
{
    using Progebel.NDoctor.PluginHost.Core;

    /// <summary>
    /// Represents a status message displayed in the status bar
    /// </summary>
    public class StatusMessage
    {
        #region Properties

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message to display.
        /// </value>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the status message.
        /// </summary>
        /// <value>
        /// The type of the status message.
        /// </value>
        public StatusType Type
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Sets the message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void SetMessage(string format, params object[] args)
        {
            this.Message = string.Format(format, args);
        }

        #endregion Methods
    }
}