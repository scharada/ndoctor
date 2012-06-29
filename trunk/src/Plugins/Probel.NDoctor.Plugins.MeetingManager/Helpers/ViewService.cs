namespace Probel.NDoctor.Plugins.MeetingManager.Helpers
{
    using System;
    using System.Collections.Generic;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
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

        #region Properties

        public AddMeetingView NewAddMeetingView()
        {
            var view = new AddMeetingView();
            try
            {
                IList<TagDto> tags;
                using (Component.UnitOfWork)
                {
                    tags = Component.FindTags(TagCategory.Appointment);
                }
                this.GetViewModel(view).AppointmentTags.Refill(tags);
            }
            catch (Exception ex) { new ErrorHandler(this).HandleError(ex); }

            return view;
        }

        private AddMeetingViewModel GetViewModel(AddMeetingView view)
        {
            return (AddMeetingViewModel)view.DataContext;
        }

        public WorkbenchViewModel GetViewModel(WorkbenchView view)
        {
            return (WorkbenchViewModel)view.DataContext;
        }
        #endregion Properties


    }
}