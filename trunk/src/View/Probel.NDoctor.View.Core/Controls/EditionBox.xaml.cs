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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    using Probel.NDoctor.View.Core.Helpers;

    /// <summary>
    /// Interaction logic for EditionBox.xaml
    /// </summary>
    public partial class EditionBox : UserControl
    {
        #region Fields

        public static DependencyProperty ButtonNameProperty = DependencyProperty.RegisterAttached("ButtonName", typeof(string)
            , typeof(EditionBox)
            , new UIPropertyMetadata(null));
        public static DependencyProperty OkCommandProperty = DependencyProperty.RegisterAttached("OkCommand", typeof(ICommand)
            , typeof(EditionBox)
            , new UIPropertyMetadata(null));
        public static DependencyProperty ValueProperty = DependencyProperty.RegisterAttached("Value", typeof(ReferenceObject<string>)
            , typeof(EditionBox)
            , new UIPropertyMetadata(null));

        #endregion Fields

        #region Constructors

        public EditionBox()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string ButtonName
        {
            get { return EditionBox.GetButtonName(this); }
            set { EditionBox.SetButtonName(this, value); }
        }

        public ICommand OkCommand
        {
            get { return EditionBox.GetOkCommand(this); }
            set { EditionBox.SetOkCommand(this, value); }
        }

        public ReferenceObject<string> Value
        {
            get { return EditionBox.GetValue(this); }
            set { EditionBox.SetValue(this, value); }
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

        public static ReferenceObject<string> GetValue(DependencyObject target)
        {
            return target.GetValue(ValueProperty) as ReferenceObject<string>;
        }

        public static void SetButtonName(DependencyObject target, string value)
        {
            target.SetValue(ButtonNameProperty, value);
        }

        public static void SetOkCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(OkCommandProperty, value);
        }

        public static void SetValue(DependencyObject target, ReferenceObject<string> value)
        {
            target.SetValue(ValueProperty, value);
        }

        private void this_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(this.focused);
        }

        #endregion Methods
    }
}