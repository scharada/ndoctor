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
namespace Probel.NDoctor.View.Core.ViewModel
{
    using Microsoft.Windows.Controls;

    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Core.ViewModel;

    public class ChildWindowViewModel : BaseViewModel
    {
        #region Constructors

        public ChildWindowViewModel()
        {
            ChildWindowContext.CaptionChanged += (sender, e) => this.OnPropertyChanged("Caption");
            ChildWindowContext.CloseButtonVisibilityChanged += (sender, e) => this.OnPropertyChanged("CloseButtonVisibility");
            ChildWindowContext.ContentChanged += (sender, e) => this.OnPropertyChanged("Content");
            ChildWindowContext.IsModalChanged += (sender, e) => this.OnPropertyChanged("IsModal");
            ChildWindowContext.WindowStartupLocationChanged += (sender, e) => this.OnPropertyChanged("WindowStartupLocation");
            ChildWindowContext.WindowStateChanged += (sender, e) => this.OnPropertyChanged("WindowState");
        }

        #endregion Constructors

        #region Properties

        public string Caption
        {
            get { return ChildWindowContext.Caption; }
            set { ChildWindowContext.Caption = value; }
        }

        public System.Windows.Visibility CloseButtonVisibility
        {
            get { return ChildWindowContext.CloseButtonVisibility; }
            set { ChildWindowContext.CloseButtonVisibility = value; }
        }

        public System.Windows.Controls.UserControl Content
        {
            get { return ChildWindowContext.Content; }
            set { ChildWindowContext.Content = value; }
        }

        public bool IsModal
        {
            get { return ChildWindowContext.IsModal; }
            set { ChildWindowContext.IsModal = value; }
        }

        public Microsoft.Windows.Controls.WindowStartupLocation WindowStartupLocation
        {
            get { return ChildWindowContext.WindowStartupLocation; }
            set { ChildWindowContext.WindowStartupLocation = value; }
        }

        public WindowState WindowState
        {
            get { return ChildWindowContext.WindowState; }
            set { ChildWindowContext.WindowState = value; }
        }

        #endregion Properties
    }
}