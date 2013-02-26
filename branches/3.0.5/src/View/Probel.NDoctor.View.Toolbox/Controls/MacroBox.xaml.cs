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
    /// Interaction logic for MacroBox.xaml
    /// </summary>
    public partial class MacroBox : UserControl
    {
        #region Fields

        public static DependencyProperty ButtonNameProperty = DependencyProperty.RegisterAttached("ButtonName", typeof(string)
            , typeof(MacroBox)
            , new UIPropertyMetadata(null));
        public static DependencyProperty OkCommandProperty = DependencyProperty.RegisterAttached("OkCommand", typeof(ICommand)
            , typeof(MacroBox)
            , new UIPropertyMetadata(null));
        public static DependencyProperty ProfessionProperty = DependencyProperty.RegisterAttached("Macro", typeof(MacroDto)
            , typeof(MacroBox)
            , new UIPropertyMetadata(null));

        #endregion Fields

        #region Constructors

        public MacroBox()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string ButtonName
        {
            get { return MacroBox.GetButtonName(this); }
            set { MacroBox.SetButtonName(this, value); }
        }

        public MacroDto Macro
        {
            get { return MacroBox.GetMacro(this); }
            set { MacroBox.SetMacro(this, value); }
        }

        public ICommand OkCommand
        {
            get { return MacroBox.GetOkCommand(this); }
            set { MacroBox.SetOkCommand(this, value); }
        }

        #endregion Properties

        #region Methods

        public static string GetButtonName(DependencyObject target)
        {
            return target.GetValue(ButtonNameProperty) as string ?? "No value";
        }

        public static MacroDto GetMacro(DependencyObject target)
        {
            return target.GetValue(ProfessionProperty) as MacroDto;
        }

        public static ICommand GetOkCommand(DependencyObject target)
        {
            return target.GetValue(OkCommandProperty) as ICommand;
        }

        public static void SetButtonName(DependencyObject target, string value)
        {
            target.SetValue(ButtonNameProperty, value);
        }

        public static void SetMacro(DependencyObject target, MacroDto value)
        {
            target.SetValue(ProfessionProperty, value);
        }

        public static void SetOkCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(OkCommandProperty, value);
        }

        private void this_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(this.focused);
        }

        #endregion Methods
    }
}