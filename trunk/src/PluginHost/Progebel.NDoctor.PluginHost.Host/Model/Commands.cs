
using System.Windows;
using System.Windows.Input;
using Progebel.NDoctor.PluginHost.Core;
namespace Progebel.NDoctor.PluginHost.Host.Model
{
    public static class Commands
    {
        public static ICommand Shutdown
        {
            get
            {
                return new RelayCommand(() => Application.Current.Shutdown(), () => true);
            }
        }
    }
}
