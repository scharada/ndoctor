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
namespace Probel.NDoctor.Plugins.UserSession.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    using Probel.NDoctor.Plugins.UserSession.Properties;

    /// <summary>
    /// Interaction logic for BusinessCard.xaml
    /// </summary>
    public partial class BusinessCard : UserControl
    {
        #region Constructors

        public BusinessCard()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        public void Print()
        {
            var size = new Size(350, 250);
            this.Measure(size);
            this.Arrange(new Rect(size));
            this.UpdateLayout();

            new PrintDialog().PrintVisual(this, Messages.Title_BusinessCard);
        }

        #endregion Methods
    }
}