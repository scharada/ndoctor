#region Header

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

#endregion Header

namespace Probel.NDoctor.View.Core.Controls
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Core.ViewModel;

    /// <summary>
    /// Interaction logic for IllnessBox.xaml
    /// </summary>
    public partial class PathologyBox : UserControl
    {
        #region Fields

        public static DependencyProperty ButtonNameProperty = DependencyProperty.RegisterAttached("ButtonName", typeof(string)
            , typeof(PathologyBox)
            , new UIPropertyMetadata(null));
        public static DependencyProperty OkCommandProperty = DependencyProperty.RegisterAttached("OkCommand", typeof(ICommand)
            , typeof(PathologyBox)
            , new UIPropertyMetadata(null));
        public static DependencyProperty PathologyProperty = DependencyProperty.RegisterAttached("Pathology", typeof(PathologyDto)
            , typeof(PathologyBox)
            , new UIPropertyMetadata(null));
        public static DependencyProperty TagsProperty = DependencyProperty.RegisterAttached("Tags", typeof(ObservableCollection<TagDto>)
            , typeof(PathologyBox)
            , new UIPropertyMetadata(null));

        #endregion Fields

        #region Constructors

        public PathologyBox()
        {
            this.InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string ButtonName
        {
            get { return PathologyBox.GetButtonName(this); }
            set { PathologyBox.SetButtonName(this, value); }
        }

        public ICommand OkCommand
        {
            get { return PathologyBox.GetOkCommand(this); }
            set { PathologyBox.SetOkCommand(this, value); }
        }

        public PathologyDto Pathology
        {
            get { return PathologyBox.GetPathology(this); }
            set { PathologyBox.SetPathology(this, value); }
        }

        public ObservableCollection<TagDto> Tags
        {
            get { return PathologyBox.GetTags(this); }
            set { PathologyBox.SetTags(this, value); }
        }

        #endregion Properties

        #region Methods

        public static string GetButtonName(DependencyObject target)
        {
            return target.GetValue(ButtonNameProperty) as string ?? "No value";
        }

        public static ICommand GetOkCommand(DependencyObject target)
        {
            return target.GetValue(OkCommandProperty) as ICommand;
        }

        public static PathologyDto GetPathology(DependencyObject target)
        {
            return target.GetValue(PathologyProperty) as PathologyDto;
        }

        public static ObservableCollection<TagDto> GetTags(DependencyObject target)
        {
            return target.GetValue(TagsProperty) as ObservableCollection<TagDto>;
        }

        public static void SetButtonName(DependencyObject target, string value)
        {
            target.SetValue(ButtonNameProperty, value);
        }

        public static void SetOkCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(OkCommandProperty, value);
        }

        public static void SetPathology(DependencyObject target, PathologyDto value)
        {
            target.SetValue(PathologyProperty, value);
        }

        public static void SetTags(DependencyObject target, ObservableCollection<TagDto> value)
        {
            target.SetValue(TagsProperty, value);
        }

        private void this_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(this.focused);
        }

        #endregion Methods
    }
}