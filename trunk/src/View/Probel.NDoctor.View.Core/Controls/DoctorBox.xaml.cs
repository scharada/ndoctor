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
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.DTO.Helpers;

    /// <summary>
    /// Interaction logic for DoctorBox.xaml
    /// </summary>
    public partial class DoctorBox : UserControl
    {
        #region Fields

        public static DependencyProperty ButtonNameProperty = DependencyProperty.RegisterAttached("ButtonName", typeof(string)
            , typeof(DoctorBox)
            , new UIPropertyMetadata(null));
        public static DependencyProperty DoctorProperty = DependencyProperty.RegisterAttached("Doctor", typeof(DoctorDto)
            , typeof(DoctorBox)
            , new UIPropertyMetadata(null));
        public static DependencyProperty OkCommandProperty = DependencyProperty.RegisterAttached("OkCommand", typeof(ICommand)
            , typeof(DoctorBox)
            , new UIPropertyMetadata(null));
        public static DependencyProperty SpecialisationsProperty = DependencyProperty.RegisterAttached("Specialisations", typeof(ObservableCollection<TagDto>)
            , typeof(DoctorBox)
            , new UIPropertyMetadata(null));

        #endregion Fields

        #region Constructors

        public DoctorBox()
        {
            this.Genders = new ObservableCollection<Tuple<string, Gender>>();
            this.Genders.Add(new Tuple<string, Gender>(Gender.Male.Translate(), Gender.Male));
            this.Genders.Add(new Tuple<string, Gender>(Gender.Female.Translate(), Gender.Female));

            InitializeComponent();
        }
        #endregion Constructors

        #region Properties

        public string ButtonName
        {
            get { return DoctorBox.GetButtonName(this); }
            set { DoctorBox.SetButtonName(this, value); }
        }

        public DoctorDto Doctor
        {
            get { return DoctorBox.GetDoctor(this); }
            set { DoctorBox.SetDoctor(this, value); }
        }

        public ICommand OkCommand
        {
            get { return DoctorBox.GetOkCommand(this); }
            set { DoctorBox.SetOkCommand(this, value); }
        }

        public ObservableCollection<TagDto> Specialisations
        {
            get { return DoctorBox.GetSpecialisations(this); }
            set { DoctorBox.SetSpecialisations(this, value); }
        }

        private ObservableCollection<Tuple<string, Gender>> Genders { get; set; }

        #endregion Properties

        #region Methods

        public static string GetButtonName(DependencyObject target)
        {
            return target.GetValue(ButtonNameProperty) as string ?? "No value";
        }

        public static DoctorDto GetDoctor(DependencyObject target)
        {
            return target.GetValue(DoctorProperty) as DoctorDto;
        }

        public static ICommand GetOkCommand(DependencyObject target)
        {
            return target.GetValue(OkCommandProperty) as ICommand;
        }

        public static void SetButtonName(DependencyObject target, string value)
        {
            target.SetValue(ButtonNameProperty, value);
        }

        public static void SetDoctor(DependencyObject target, DoctorDto value)
        {
            target.SetValue(DoctorProperty, value);
        }

        public static void SetOkCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(OkCommandProperty, value);
        }

        private static ObservableCollection<TagDto> GetSpecialisations(DependencyObject target)
        {
            return target.GetValue(DoctorBox.SpecialisationsProperty) as ObservableCollection<TagDto>;
        }

        private static void SetSpecialisations(DependencyObject target, ObservableCollection<TagDto> value)
        {
            target.SetValue(DoctorBox.SpecialisationsProperty, value);
        }

        #endregion Methods

    }
}