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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Probel.NDoctor.View.Toolbox.Logging;
using Probel.Mvvm.DataBinding;

namespace Probel.NDoctor.View.Core.ViewModel
{
    internal class LogViewModel : BaseViewModel
    {
        public LogViewModel()
        {
            this.LogEvents = new ObservableCollection<LogEvent>();
        }
        private LogEvent selectedRow;
        public LogEvent SelectedRow
        {
            get { return this.selectedRow; }
            set
            {
                this.selectedRow = value;
                this.OnPropertyChanged(() => SelectedRow);
            }
        }
        /// <summary>
        /// Gets the list of log events recorded in this session.
        /// </summary>
        public ObservableCollection<LogEvent> LogEvents
        {
            get;
            set;
        }
        public void Refresh()
        {
            this.LogEvents.Refill(WpfAppender.GetLogs(this.Logger));
        }
    }
}
