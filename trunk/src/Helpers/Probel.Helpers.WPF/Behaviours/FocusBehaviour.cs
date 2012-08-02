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
namespace Probel.Helpers.WPF.Behaviours
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;

    public class FocusBehaviour
    {
        #region Fields

        public static readonly DependencyProperty GotFocusProperty = 
            DependencyProperty.RegisterAttached("GotFocus", typeof(ICommand), typeof(FocusBehaviour), new UIPropertyMetadata(null, GetFocusPropertyCallback));
        public static readonly DependencyProperty LostFocusProperty = 
            DependencyProperty.RegisterAttached("LostFocus", typeof(ICommand), typeof(FocusBehaviour), new UIPropertyMetadata(null, LostFocusPropertyCallback));

        private static Dictionary<DependencyObject, Behaviour> behaviours = new Dictionary<DependencyObject, Behaviour>();

        #endregion Fields

        #region Methods

        [AttachedPropertyBrowsableForChildren]
        public static void SetGotFocus(DependencyObject target, ICommand command)
        {
            target.SetValue(FocusBehaviour.GotFocusProperty, command);
        }

        [AttachedPropertyBrowsableForChildren]
        public static void SetLostFocus(DependencyObject target, ICommand command)
        {
            target.SetValue(FocusBehaviour.LostFocusProperty, command);
        }

        private static void GetFocusPropertyCallback(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (!(target is Control))
                return;

            if (!behaviours.ContainsKey(target))
                behaviours.Add(target, new Behaviour(target as Control));
        }

        private static void LostFocusPropertyCallback(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (!(target is Control))
                return;

            if (!behaviours.ContainsKey(target))
                behaviours.Add(target, new Behaviour(target as Control));
        }

        #endregion Methods

        #region Nested Types

        private class Behaviour
        {
            #region Fields

            Control view;

            #endregion Fields

            #region Constructors

            public Behaviour(Control view)
            {
                this.view = view;
                this.view.LostFocus += (sender, e) => LostFocusExecuteCommand(sender);
                this.view.GotFocus += (sender, e) => GotFocusExecuteCommand(sender);
            }

            #endregion Constructors

            #region Methods

            private static void GotFocusExecuteCommand(object sender)
            {
                var element = (Control)sender;
                var command = (ICommand)element.GetValue(FocusBehaviour.GotFocusProperty);

                command.TryExecute();
            }

            private static void LostFocusExecuteCommand(object sender)
            {
                var element = (Control)sender;
                var command = (ICommand)element.GetValue(FocusBehaviour.LostFocusProperty);

                command.TryExecute();
            }

            #endregion Methods
        }

        #endregion Nested Types
    }
}