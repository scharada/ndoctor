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
namespace Probel.NDoctor.View.Toolbox.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Interaction logic for PracticeBox.xaml
    /// </summary>
    public partial class PracticeBox : UserControl
    {
        #region Fields

        public static DependencyProperty ButtonNameProperty = DependencyProperty.RegisterAttached("ButtonName", typeof(string)
            , typeof(PracticeBox)
            , new UIPropertyMetadata(null));
        public static DependencyProperty OkCommandProperty = DependencyProperty.RegisterAttached("OkCommand", typeof(ICommand)
            , typeof(PracticeBox)
            , new UIPropertyMetadata(null));
        public static DependencyProperty PracticeProperty = DependencyProperty.RegisterAttached("Practice", typeof(PracticeDto)
            , typeof(PracticeBox)
            , new UIPropertyMetadata(null));

        #endregion Fields

        #region Constructors

        public PracticeBox()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string ButtonName
        {
            get { return PracticeBox.GetButtonName(this); }
            set { PracticeBox.SetButtonName(this, value); }
        }

        public ICommand OkCommand
        {
            get { return PracticeBox.GetOkCommand(this); }
            set { PracticeBox.SetOkCommand(this, value); }
        }

        public PracticeDto Profession
        {
            get { return PracticeBox.GetPractice(this); }
            set { PracticeBox.SetPractice(this, value); }
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

        public static PracticeDto GetPractice(DependencyObject target)
        {
            return target.GetValue(PracticeProperty) as PracticeDto;
        }

        public static void SetButtonName(DependencyObject target, string value)
        {
            target.SetValue(ButtonNameProperty, value);
        }

        public static void SetOkCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(OkCommandProperty, value);
        }

        public static void SetPractice(DependencyObject target, PracticeDto value)
        {
            target.SetValue(PracticeProperty, value);
        }

        private void this_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(this.focused);
        }

        #endregion Methods
    }
}