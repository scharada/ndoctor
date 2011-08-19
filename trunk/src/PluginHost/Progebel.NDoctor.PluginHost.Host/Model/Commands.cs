namespace Probel.NDoctor.PluginHost.Host.Model
{
    using System.Windows;
    using System.Windows.Input;

    using Probel.NDoctor.PluginHost.Core;

    public static class Commands
    {
        #region Properties

        public static ICommand Shutdown
        {
            get
            {
                return new RelayCommand(() => Application.Current.Shutdown(), () => true);
            }
        }

        #endregion Properties
    }
}