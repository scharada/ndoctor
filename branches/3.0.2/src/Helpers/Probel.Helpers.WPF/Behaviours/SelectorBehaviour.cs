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
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    public class SelectorBehaviour
    {
        #region Fields

        public static readonly DependencyProperty SelectionChangedProperty = DependencyProperty.RegisterAttached("SelectionChanged"
            , typeof(ICommand)
            , typeof(SelectorBehaviour)
            , new UIPropertyMetadata(null, SelectionChanged));

        private static Dictionary<DependencyObject, Behaviour> behaviours = new Dictionary<DependencyObject, Behaviour>();

        #endregion Fields

        #region Methods

        [AttachedPropertyBrowsableForChildren]
        public static void SetSelectionChanged(DependencyObject target, ICommand command)
        {
            target.SetValue(SelectorBehaviour.SelectionChangedProperty, command);
        }

        private static void SelectionChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is Selector)
            {
                if (!behaviours.ContainsKey(target))
                {
                    behaviours.Add(target, new Behaviour(target as Selector));
                }
            }
        }

        #endregion Methods

        #region Nested Types

        private class Behaviour
        {
            #region Fields

            private Selector selector;

            #endregion Fields

            #region Constructors

            public Behaviour(Selector selector)
            {
                this.selector = selector;
                this.selector.SelectionChanged += (sender, e) =>
                {
                    var element = sender as UIElement;
                    if (sender == null) throw new NullReferenceException("Sender is null");

                    var command = (ICommand)element.GetValue(SelectorBehaviour.SelectionChangedProperty);

                    if (command.CanExecute(null))
                    {
                        command.Execute(null);
                    }
                };
            }

            #endregion Constructors
        }

        #endregion Nested Types
    }
}