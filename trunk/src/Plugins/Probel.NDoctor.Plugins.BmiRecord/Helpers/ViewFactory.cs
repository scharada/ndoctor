namespace Probel.NDoctor.Plugins.BmiRecord.Helpers
{
    using Probel.NDoctor.Plugins.BmiRecord.View;
    using Probel.NDoctor.View.Plugins.Helpers;

    public static class ViewFactory
    {
        #region Fields

        private static AddBmiView addBmiView;
        private static Workbench workbench;

        #endregion Fields

        #region Constructors

        static ViewFactory()
        {
            PluginContext.Host.Invoke(() =>
            {
                addBmiView = new AddBmiView();
                workbench = new Workbench();
            });
        }

        #endregion Constructors

        #region Properties

        public static AddBmiView AddBmiView
        {
            get { return addBmiView; }
        }

        public static Workbench Workbench
        {
            get { return workbench; }
        }

        #endregion Properties
    }
}