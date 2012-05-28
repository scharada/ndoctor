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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Interaction logic for InsuranceBox.xaml
    /// </summary>
    public partial class InsuranceBox : UserControl
    {
        #region Fields

        public static DependencyProperty ButtonNameProperty = DependencyProperty.RegisterAttached("ButtonName", typeof(string)
            , typeof(InsuranceBox)
            , new UIPropertyMetadata(null));
        public static DependencyProperty InsuranceProperty = DependencyProperty.RegisterAttached("Insurance", typeof(InsuranceDto)
            , typeof(InsuranceBox)
            , new UIPropertyMetadata(null));
        public static DependencyProperty OkCommandProperty = DependencyProperty.RegisterAttached("OkCommand", typeof(ICommand)
            , typeof(InsuranceBox)
            , new UIPropertyMetadata(null));

        #endregion Fields

        #region Constructors

        public InsuranceBox()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string ButtonName
        {
            get { return InsuranceBox.GetButtonName(this); }
            set { InsuranceBox.SetButtonName(this, value); }
        }

        public InsuranceDto Insurance
        {
            get { return InsuranceBox.GetInsurance(this); }
            set { InsuranceBox.SetInsurance(this, value); }
        }

        public ICommand OkCommand
        {
            get { return InsuranceBox.GetOkCommand(this); }
            set { InsuranceBox.SetOkCommand(this, value); }
        }

        #endregion Properties

        #region Methods

        public static string GetButtonName(DependencyObject target)
        {
            return target.GetValue(ButtonNameProperty) as string ?? "No value";
        }

        public static InsuranceDto GetInsurance(DependencyObject target)
        {
            return target.GetValue(InsuranceProperty) as InsuranceDto;
        }

        public static ICommand GetOkCommand(DependencyObject target)
        {
            return target.GetValue(OkCommandProperty) as ICommand;
        }

        public static void SetButtonName(DependencyObject target, string value)
        {
            target.SetValue(ButtonNameProperty, value);
        }

        public static void SetInsurance(DependencyObject target, InsuranceDto value)
        {
            target.SetValue(InsuranceProperty, value);
        }

        public static void SetOkCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(OkCommandProperty, value);
        }

        #endregion Methods
    }
}