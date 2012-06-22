using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Probel.NDoctor.Plugins.MeetingManager.View;
using Probel.NDoctor.View.Plugins.Helpers;
using Probel.NDoctor.Plugins.MeetingManager.ViewModel;
using Probel.NDoctor.Domain.DAL.Components;
using Probel.NDoctor.Domain.DTO.Objects;
using Probel.Mvvm.DataBinding;

namespace Probel.NDoctor.Plugins.MeetingManager.Helpers
{
    public static class ViewFactory
    {
        private static ICalendarComponent Component = PluginContext.ComponentFactory.GetInstance<ICalendarComponent>();
        private static AddMeetingView addMeetingView;
        private static RemoveMeetingView removeMeetingView;
        private static AddCategoryView addCategoryView;
        static ViewFactory()
        {
            PluginContext.Host.Invoke(() =>
            {
                addCategoryView = new AddCategoryView();
                addMeetingView = new AddMeetingView();
                removeMeetingView = new RemoveMeetingView();
            });
        }
        public static AddMeetingView AddMeetingView
        {
            get
            {
                IList<TagDto> tags;
                using (Component.UnitOfWork)
                {
                    tags = Component.FindTags(TagCategory.Appointment);
                }
                ((AddMeetingViewModel)addMeetingView.DataContext).AppointmentTags.Refill(tags);
                return addMeetingView;
            }
        }
        public static AddCategoryView AddCategoryView
        {
            get { return addCategoryView; }
        }
        public static RemoveMeetingView RemoveMeetingView
        {
            get { return removeMeetingView; }
        }
    }
}
