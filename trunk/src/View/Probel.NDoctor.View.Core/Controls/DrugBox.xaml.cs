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

    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Core.ViewModel;

    /// <summary>
    /// Interaction logic for DrugBox.xaml
    /// </summary>
    public partial class DrugBox : UserControl
    {
        #region Fields

        public static DependencyProperty ButtonNameProperty = DependencyProperty.RegisterAttached("ButtonName", typeof(string)
            , typeof(DrugBox)
            , new UIPropertyMetadata(null));
        public static DependencyProperty DrugyProperty = DependencyProperty.RegisterAttached("Drug", typeof(DrugDto)
            , typeof(DrugBox)
            , new UIPropertyMetadata(null));
        public static DependencyProperty OkCommandProperty = DependencyProperty.RegisterAttached("OkCommand", typeof(ICommand)
            , typeof(DrugBox)
            , new UIPropertyMetadata(null));

        #endregion Fields

        #region Constructors

        public DrugBox(DrugBoxViewModel viewmodel)
        {
            InitializeComponent();
            this.DataContext = viewmodel;
        }

        public DrugBox()
            : this(new DrugBoxViewModel())
        {
        }

        #endregion Constructors

        #region Properties

        public string ButtonName
        {
            get { return DrugBox.GetButtonName(this); }
            set { DrugBox.SetButtonName(this, value); }
        }

        public DrugDto Drug
        {
            get { return DrugBox.GetDrug(this); }
            set { DrugBox.SetDrug(this, value); }
        }

        public ICommand OkCommand
        {
            get { return DrugBox.GetOkCommand(this); }
            set { DrugBox.SetOkCommand(this, value); }
        }

        #endregion Properties

        #region Methods

        public static string GetButtonName(DependencyObject target)
        {
            return target.GetValue(ButtonNameProperty) as string ?? "No value";
        }

        public static DrugDto GetDrug(DependencyObject target)
        {
            return target.GetValue(DrugyProperty) as DrugDto;
        }

        public static ICommand GetOkCommand(DependencyObject target)
        {
            return target.GetValue(OkCommandProperty) as ICommand;
        }

        public static void SetButtonName(DependencyObject target, string value)
        {
            target.SetValue(ButtonNameProperty, value);
        }

        public static void SetDrug(DependencyObject target, DrugDto value)
        {
            target.SetValue(DrugyProperty, value);
        }

        public static void SetOkCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(OkCommandProperty, value);
        }

        #endregion Methods
    }
}