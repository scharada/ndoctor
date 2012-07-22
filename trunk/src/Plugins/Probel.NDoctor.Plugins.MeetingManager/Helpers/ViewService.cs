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
namespace Probel.NDoctor.Plugins.MeetingManager.Helpers
{
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Plugins.MeetingManager.View;
    using Probel.NDoctor.Plugins.MeetingManager.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class ViewService
    {
        #region Fields

        private static ICalendarComponent Component = PluginContext.ComponentFactory.GetInstance<ICalendarComponent>();

        #endregion Fields

        #region Constructors

        public ViewService()
        {
            PluginContext.Host.NewUserConnected += (sender, e) => Component = PluginContext.ComponentFactory.GetInstance<ICalendarComponent>();
        }

        #endregion Constructors

        #region Methods

        public WorkbenchViewModel GetViewModel(WorkbenchView view)
        {
            return (WorkbenchViewModel)view.DataContext;
        }

        public AddMeetingView NewAddMeetingView()
        {
            var view = new AddMeetingView();
            this.GetViewModel(view).Refresh();

            return view;
        }

        private AddMeetingViewModel GetViewModel(AddMeetingView view)
        {
            return (AddMeetingViewModel)view.DataContext;
        }

        #endregion Methods
    }
}