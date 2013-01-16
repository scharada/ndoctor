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

namespace Probel.NDoctor.View.Toolbox.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class LogEvent
    {
        #region Properties

        public string ExeptionMessage
        {
            get; set;
        }

        public string LevelName
        {
            get;
            set;
        }

        public string LoggerName
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public string ThreadName
        {
            get;
            set;
        }

        public DateTime TimeStamp
        {
            get;
            set;
        }

        #endregion Properties
    }
}