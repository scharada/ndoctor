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

    public class ViewService
    {
        #region Fields

        private static AddCategoryView addCategoryView;
        private static AddMeetingView addMeetingView;
        private static ICalendarComponent Component = PluginContext.ComponentFactory.GetInstance<ICalendarComponent>();
        private static RemoveMeetingView removeMeetingView;
        private static WorkbenchView workbenchView;

        #endregion Fields

        #region Properties

        public AddCategoryView AddCategoryView
        {
            get
            {
                if (addCategoryView == null) { addCategoryView = new AddCategoryView(); }
                return addCategoryView;
            }
        }

        public AddMeetingView AddMeetingView
        {
            get
            {
                if (addCategoryView == null) { addMeetingView = new AddMeetingView(); }
                IList<TagDto> tags;
                using (Component.UnitOfWork)
                {
                    tags = Component.FindTags(TagCategory.Appointment);
                }
                this.AddMeetingViewModel.AppointmentTags.Refill(tags);
                return addMeetingView;
            }
        }

        public AddMeetingViewModel AddMeetingViewModel
        {
            get
            {
                return (AddMeetingViewModel)this.AddMeetingView.DataContext;
            }
        }

        public RemoveMeetingView RemoveMeetingView
        {
            get
            {
                if (removeMeetingView == null) { removeMeetingView = new RemoveMeetingView(); }
                return removeMeetingView;
            }
        }

        public RemoveMeetingViewModel RemoveMeetingViewModel
        {
            get
            {
                return (RemoveMeetingViewModel)RemoveMeetingView.DataContext;
            }
        }

        public WorkbenchView WorkbenchView
        {
            get
            {
                if (workbenchView == null) { workbenchView = new WorkbenchView(); }
                return workbenchView;
            }
        }

        #endregion Properties

        #region Methods

        public void CloseAll()
        {
            this.CloseAddCategoryView();
            this.CloseAddMeetingView();
            this.CloseRemoveMeetingView();
            this.CloseWorkbenchView();
        }

        private void CloseAddCategoryView()
        {
            addCategoryView = null;
        }

        private void CloseAddMeetingView()
        {
            addMeetingView = null;
        }

        private void CloseRemoveMeetingView()
        {
            removeMeetingView = null;
        }

        private void CloseWorkbenchView()
        {
            workbenchView = null;
        }

        #endregion Methods
    }
}