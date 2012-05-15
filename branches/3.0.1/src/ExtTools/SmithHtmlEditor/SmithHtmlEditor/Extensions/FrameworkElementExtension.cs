namespace Smith.WPF.HtmlEditor
{
    using System.Windows;

    internal static class FrameworkElementExtension
    {
        #region Methods

        /// <summary>
        /// Get the window container of framework element.
        /// </summary>
        public static Window GetParentWindow(this FrameworkElement element)
        {
            DependencyObject dp = element;
            while (dp != null)
            {
                DependencyObject tp = LogicalTreeHelper.GetParent(dp);
                if (tp is Window) return tp as Window;
                else dp = tp;
            }
            return null;
        }

        #endregion Methods
    }
}