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

namespace Probel.Helpers.WPF.Behaviours
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;

    using Probel.Mvvm.DataBinding;

    public class WindowBehaviour
    {
        #region Fields

        public static readonly DependencyProperty LoadedProperty = 
            DependencyProperty.RegisterAttached("Loaded", typeof(ICommand), typeof(WindowBehaviour), new UIPropertyMetadata(null, CallbackAction));

        private static Dictionary<DependencyObject, Behaviour> behaviours = new Dictionary<DependencyObject, Behaviour>();

        #endregion Fields

        #region Methods

        [AttachedPropertyBrowsableForChildren]
        public static void SetLoaded(DependencyObject target, ICommand command)
        {
            target.SetValue(WindowBehaviour.LoadedProperty, command);
        }

        private static void CallbackAction(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (!(target is Window))
                return;

            if (!behaviours.ContainsKey(target))
                behaviours.Add(target, new Behaviour(target as Window));
        }

        #endregion Methods

        #region Nested Types

        private class Behaviour
        {
            #region Fields

            Window view;

            #endregion Fields

            #region Constructors

            public Behaviour(Window view)
            {
                this.view = view;
                this.view.Loaded += (sender, e) => LoadedExecuteCommand(sender);
            }

            #endregion Constructors

            #region Methods

            private static void LoadedExecuteCommand(object sender)
            {
                var element = (Window)sender;
                var command = (ICommand)element.GetValue(WindowBehaviour.LoadedProperty);

                if (command != null) { command.TryExecute(); }
            }

            #endregion Methods
        }

        #endregion Nested Types
    }
}