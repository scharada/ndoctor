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

namespace Probel.NDoctor.Plugins.PrescriptionManager.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.PrescriptionManager.Helpers;

    /// <summary>
    /// Interaction logic for PrescriptionBox.xaml
    /// </summary>
    public partial class PrescriptionBox : UserControl
    {
        #region Fields

        public static DependencyProperty PrescriptionProperty = DependencyProperty.RegisterAttached("Prescription"
            , typeof(PrescriptionDto)
            , typeof(PrescriptionBox)
            , new PropertyMetadata(null));

        #endregion Fields

        #region Constructors

        public PrescriptionBox()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public PrescriptionDto Prescription
        {
            get { return GetPrescription(this); }
            set { SetPrescription(this, value); }
        }

        #endregion Properties

        #region Methods

        public static PrescriptionDto GetPrescription(DependencyObject target)
        {
            return (PrescriptionDto)target.GetValue(PrescriptionProperty);
        }

        public static void SetPrescription(DependencyObject target, PrescriptionDto value)
        {
            target.SetValue(PrescriptionProperty, value);
        }

        #endregion Methods

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Notifyer.OnPrescriptionRemoving(this, this.Prescription);
        }
    }
}