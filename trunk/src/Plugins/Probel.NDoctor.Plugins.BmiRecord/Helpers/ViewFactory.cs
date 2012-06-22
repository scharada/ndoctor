namespace Probel.NDoctor.Plugins.BmiRecord.Helpers
{
    using Probel.NDoctor.Plugins.BmiRecord.View;
    using Probel.NDoctor.View.Plugins.Helpers;

    public static class ViewFactory
    {
        #region Properties

        public static AddBmiView AddBmiView
        {
            get { return new AddBmiView(); }
        }

        public static Workbench Workbench
        {
            get { return new Workbench(); }
        }

        #endregion Properties
    }
}