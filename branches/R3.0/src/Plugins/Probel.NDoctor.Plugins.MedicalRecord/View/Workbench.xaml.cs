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
namespace Probel.NDoctor.Plugins.MedicalRecord.View
{
    using System.Windows;
    using System.Windows.Controls;

    using Probel.NDoctor.Plugins.MedicalRecord.Dto;
    using Probel.NDoctor.Plugins.MedicalRecord.Helpers;
    using Probel.NDoctor.Plugins.MedicalRecord.ViewModel;

    /// <summary>
    /// Interaction logic for Workbench.xaml
    /// </summary>
    public partial class Workbench : Page
    {
        #region Constructors

        public Workbench()
        {
            InitializeComponent();
            Context.RichTextBox = this.richTextBox;
            this.richTextBox.IsEnabled = false;
        }

        #endregion Constructors

        #region Methods

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.DataContext is WorkbenchViewModel)
            {
                var viewModel = this.DataContext as WorkbenchViewModel;
                if (this.treeView.SelectedItem is TitledMedicalRecordDto)
                {
                    viewModel.SelectedRecord = this.treeView.SelectedItem as TitledMedicalRecordDto;
                    this.richTextBox.IsEnabled = true;
                }
            }
        }

        #endregion Methods
    }
}