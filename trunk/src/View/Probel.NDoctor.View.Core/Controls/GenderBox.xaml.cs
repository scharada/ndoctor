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

    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Interaction logic for GenderBox.xaml
    /// </summary>
    public partial class GenderBox : UserControl
    {
        #region Fields

        public static DependencyProperty GenderProperty = DependencyProperty.RegisterAttached("Gender", typeof(Gender)
            , typeof(GenderBox)
            , new UIPropertyMetadata(Gender.Male));

        #endregion Fields

        #region Constructors

        public GenderBox()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public Gender Gender
        {
            get { return GetGender(this); }
            set { SetGender(this, value); }
        }

        #endregion Properties

        #region Methods

        public static Gender GetGender(DependencyObject target)
        {
            return (Gender)target.GetValue(GenderProperty);
        }

        public static void SetGender(DependencyObject target, Gender value)
        {
            target.SetValue(GenderProperty, value);
        }

        #endregion Methods
    }
}