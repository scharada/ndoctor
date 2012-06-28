namespace Probel.NDoctor.Plugins.BmiRecord.Helpers
{
    using Probel.NDoctor.Plugins.BmiRecord.View;
    using Probel.NDoctor.View.Plugins.Helpers;

    public class ViewService
    {
        #region Fields

        private static AddBmiView addBmiView;
        private static WorkbenchView workbenchView;

        #endregion Fields

        #region Properties

        public AddBmiView AddBmiView
        {
            get
            {
                if (addBmiView == null) { addBmiView = new AddBmiView(); }
                return addBmiView;
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
            this.CloseAddBmiView();
            this.CloseWorkbenchView();
        }

        private void CloseAddBmiView()
        {
            addBmiView = null;
        }

        private void CloseWorkbenchView()
        {
            workbenchView = null;
        }

        #endregion Methods
    }
}