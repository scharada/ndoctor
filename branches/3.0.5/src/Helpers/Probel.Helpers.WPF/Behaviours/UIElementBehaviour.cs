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

    public class UIElementBehaviour
    {
        #region Fields

        public static readonly DependencyProperty IsVisibleProperty = 
            DependencyProperty.RegisterAttached("IsVisible", typeof(ICommand), typeof(UIElementBehaviour), new UIPropertyMetadata(null, CallbackAction));

        private static Dictionary<DependencyObject, Behaviour> behaviours = new Dictionary<DependencyObject, Behaviour>();

        #endregion Fields

        #region Methods

        [AttachedPropertyBrowsableForChildren]
        public static void SetIsVisible(DependencyObject target, ICommand command)
        {
            target.SetValue(UIElementBehaviour.IsVisibleProperty, command);
        }

        private static void CallbackAction(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (!(target is UIElement))
                return;

            if (!behaviours.ContainsKey(target))
                behaviours.Add(target, new Behaviour(target as UIElement));
        }

        #endregion Methods

        #region Nested Types

        private class Behaviour
        {
            #region Fields

            private readonly UIElement View;

            #endregion Fields

            #region Constructors

            public Behaviour(UIElement view)
            {
                this.View = view;
                this.View.IsVisibleChanged += (sender, e) => IsVisibleChanged(sender, e);
            }

            #endregion Constructors

            #region Methods

            private void IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
            {
                var element = (UIElement)sender;
                var command = (ICommand)element.GetValue(UIElementBehaviour.IsVisibleProperty);

                if ((bool)e.NewValue && command != null) { command.TryExecute(); }
            }

            #endregion Methods
        }

        #endregion Nested Types
    }
}