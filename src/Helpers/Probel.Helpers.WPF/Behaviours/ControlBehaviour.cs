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

    public class ControlBehaviour
    {
        #region Fields

        public static readonly DependencyProperty GotFocusProperty = 
            DependencyProperty.RegisterAttached("GotFocus", typeof(ICommand), typeof(ControlBehaviour), new UIPropertyMetadata(null, CallbackAction));
        public static readonly DependencyProperty LoadedProperty = 
            DependencyProperty.RegisterAttached("Loaded", typeof(ICommand), typeof(ControlBehaviour), new UIPropertyMetadata(null, CallbackAction));
        public static readonly DependencyProperty LostFocusProperty = 
            DependencyProperty.RegisterAttached("LostFocus", typeof(ICommand), typeof(ControlBehaviour), new UIPropertyMetadata(null, CallbackAction));
        public static readonly DependencyProperty MouseDoubleClickProperty = 
            DependencyProperty.RegisterAttached("MouseDoubleClick", typeof(ICommand), typeof(ControlBehaviour), new UIPropertyMetadata(null, CallbackAction));

        private static Dictionary<DependencyObject, Behaviour> behaviours = new Dictionary<DependencyObject, Behaviour>();

        #endregion Fields

        #region Methods

        [AttachedPropertyBrowsableForChildren]
        public static void SetGotFocus(DependencyObject target, ICommand command)
        {
            target.SetValue(ControlBehaviour.GotFocusProperty, command);
        }

        [AttachedPropertyBrowsableForChildren]
        public static void SetLoaded(DependencyObject target, ICommand command)
        {
            target.SetValue(ControlBehaviour.LoadedProperty, command);
        }

        [AttachedPropertyBrowsableForChildren]
        public static void SetLostFocus(DependencyObject target, ICommand command)
        {
            target.SetValue(ControlBehaviour.LostFocusProperty, command);
        }

        [AttachedPropertyBrowsableForChildren]
        public static void SetMouseDoubleClick(DependencyObject target, ICommand command)
        {
            target.SetValue(ControlBehaviour.MouseDoubleClickProperty, command);
        }

        private static void CallbackAction(DependencyObject target, DependencyPropertyChangedEventArgs e)
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
                this.view.Loaded += (sender, e) => LoadedExecuteCommand(sender);
                this.view.MouseDoubleClick += (sender, e) => MouseDoubleClickCommand(sender);
            }

            #endregion Constructors

            #region Methods

            private static void GotFocusExecuteCommand(object sender)
            {
                var element = (Control)sender;
                var command = (ICommand)element.GetValue(ControlBehaviour.GotFocusProperty);

                if (command != null) { command.TryExecute(); }
            }

            private static void LoadedExecuteCommand(object sender)
            {
                var element = (Control)sender;
                var command = (ICommand)element.GetValue(ControlBehaviour.LoadedProperty);

                if (command != null) { command.TryExecute(); }
            }

            private static void LostFocusExecuteCommand(object sender)
            {
                var element = (Control)sender;
                var command = (ICommand)element.GetValue(ControlBehaviour.LostFocusProperty);

                if (command != null) { command.TryExecute(); }
            }

            private static void MouseDoubleClickCommand(object sender)
            {
                var element = (Control)sender;
                var command = (ICommand)element.GetValue(ControlBehaviour.MouseDoubleClickProperty);

                if (command != null) { command.TryExecute(); }
            }

            #endregion Methods
        }

        #endregion Nested Types
    }
}