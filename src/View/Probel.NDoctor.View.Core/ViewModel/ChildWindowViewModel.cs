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
    using Xceed.Wpf.Toolkit;

    public class ChildWindowViewModel : BaseViewModel
    {
        #region Constructors

        public ChildWindowViewModel()
        {
            InnerWindow.CaptionChanged += (sender, e) => this.OnPropertyChanged(() => Caption);
            InnerWindow.CloseButtonVisibilityChanged += (sender, e) => this.OnPropertyChanged(() => CloseButtonVisibility);
            InnerWindow.ContentChanged += (sender, e) => this.OnPropertyChanged(() => Content);
            InnerWindow.IsModalChanged += (sender, e) => this.OnPropertyChanged(() => IsModal);
            InnerWindow.WindowStartupLocationChanged += (sender, e) => this.OnPropertyChanged(() => WindowStartupLocation);
            InnerWindow.WindowStateChanged += (sender, e) => this.OnPropertyChanged(() => WindowState);
        }

        #endregion Constructors

        #region Properties

        public string Caption
        {
            get { return InnerWindow.Caption; }
            set { InnerWindow.Caption = value; }
        }

        public System.Windows.Visibility CloseButtonVisibility
        {
            get { return InnerWindow.CloseButtonVisibility; }
            set { InnerWindow.CloseButtonVisibility = value; }
        }

        public System.Windows.Controls.UserControl Content
        {
            get { return InnerWindow.Content; }
            set { InnerWindow.Content = value; }
        }

        public bool IsModal
        {
            get { return InnerWindow.IsModal; }
            set { InnerWindow.IsModal = value; }
        }

        public WindowStartupLocation WindowStartupLocation
        {
            get { return InnerWindow.WindowStartupLocation; }
            set { InnerWindow.WindowStartupLocation = value; }
        }

        public WindowState WindowState
        {
            get { return InnerWindow.WindowState; }
            set { InnerWindow.WindowState = value; }
        }

        #endregion Properties
    }
}