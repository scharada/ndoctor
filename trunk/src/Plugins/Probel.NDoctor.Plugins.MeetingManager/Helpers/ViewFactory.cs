namespace Probel.NDoctor.Plugins.MeetingManager.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MeetingManager.View;
    using Probel.NDoctor.Plugins.MeetingManager.ViewModel;
    using Probel.NDoctor.View.Plugins.Helpers;

    public static class ViewFactory
    {
        #region Fields

        private static ICalendarComponent Component = PluginContext.ComponentFactory.GetInstance<ICalendarComponent>();

        #endregion Fields

        #region Properties

        public static AddCategoryView AddCategoryView
        {
            get { return new AddCategoryView(); }
        }

        public static AddMeetingView AddMeetingView
        {
            get
            {
                var addMeetingView = new AddMeetingView();
                IList<TagDto> tags;
                using (Component.UnitOfWork)
                {
                    tags = Component.FindTags(TagCategory.Appointment);
                }
                ((AddMeetingViewModel)addMeetingView.DataContext).AppointmentTags.Refill(tags);
                return addMeetingView;
            }
        }

        public static RemoveMeetingView RemoveMeetingView
        {
            get { return new RemoveMeetingView(); }
        }

        #endregion Properties
    }
}