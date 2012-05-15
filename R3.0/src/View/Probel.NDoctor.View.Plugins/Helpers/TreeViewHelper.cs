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
namespace Probel.NDoctor.View.Plugins.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class TreeViewHelper
    {
        #region Fields

        public static readonly DependencyProperty SelectedItemChangedProperty = 
            DependencyProperty.RegisterAttached("SelectedItemChanged", typeof(ICommand), typeof(TreeViewHelper), new UIPropertyMetadata(null, SelectedItemChanged));

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty = 
            DependencyProperty.RegisterAttached("SelectedItem", typeof(object), typeof(TreeViewHelper), new UIPropertyMetadata(null, SelectedItemChanged));

        private static Dictionary<DependencyObject, TreeViewSelectedItemBehavior> behaviours = new Dictionary<DependencyObject, TreeViewSelectedItemBehavior>();

        #endregion Fields

        #region Methods

        public static object GetSelectedItem(DependencyObject target)
        {
            return (object)target.GetValue(TreeViewHelper.SelectedItemProperty);
        }

        public static void SetSelectedItem(DependencyObject target, object value)
        {
            target.SetValue(TreeViewHelper.SelectedItemProperty, value);
        }

        public static void SetSelectedItemChanged(DependencyObject target, ICommand command)
        {
            target.SetValue(TreeViewHelper.SelectedItemChangedProperty, command);
        }

        private static void SelectedItemChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (!(target is TreeView))
                return;

            if (!behaviours.ContainsKey(target))
                behaviours.Add(target, new TreeViewSelectedItemBehavior(target as TreeView));
        }

        #endregion Methods

        #region Nested Types

        private class TreeViewSelectedItemBehavior
        {
            #region Fields

            TreeView view;

            #endregion Fields

            #region Constructors

            public TreeViewSelectedItemBehavior(TreeView view)
            {
                this.view = view;
                view.SelectedItemChanged += (sender, e) =>
                {
                    TreeViewHelper.SetSelectedItem(view, e.NewValue);
                    ExecuteCommand(sender);
                };
            }

            #endregion Constructors

            #region Methods

            private static void ExecuteCommand(object sender)
            {
                var element = (UIElement)sender;
                var command = (ICommand)element.GetValue(TreeViewHelper.SelectedItemChangedProperty);

                if (command.CanExecute(null))
                {
                    command.Execute(null);
                }
            }

            #endregion Methods
        }

        #endregion Nested Types
    }
}