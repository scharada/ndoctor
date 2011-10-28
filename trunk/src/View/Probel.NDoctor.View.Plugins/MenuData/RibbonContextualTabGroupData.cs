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
namespace Probel.NDoctor.View.Plugins.MenuData
{
    using System.Collections.ObjectModel;
    using System.Windows.Media;

    using Probel.Helpers.Events;

    public class RibbonContextualTabGroupData : RibbonBase
    {
        #region Fields

        private Brush background;
        private string header;
        private bool isVisible;
        private ObservableCollection<RibbonTabData> tabs = new ObservableCollection<RibbonTabData>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonContextualTabGroupData"/> class.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="tabData">The tab data.</param>
        public RibbonContextualTabGroupData(string header, RibbonTabData tabData)
            : this(header, new ObservableCollection<RibbonTabData>() { tabData })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonContextualTabGroupData"/> class.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="groups">The groups.</param>
        public RibbonContextualTabGroupData(string header, ObservableCollection<RibbonTabData> tabDataCollection)
        {
            this.IsVisible = true;
            this.Background = Brushes.Orange;
            this.Header = header;

            this.tabs = new ObservableCollection<RibbonTabData>();
            foreach (var tab in tabDataCollection) this.TabDataCollection.Add(tab);
        }

        public RibbonContextualTabGroupData(string header)
            : this(header, new ObservableCollection<RibbonTabData>())
        {
        }

        #endregion Constructors

        #region Properties

        public Brush Background
        {
            get { return this.background; }
            set
            {
                this.background = value;
                this.OnPropertyChanged("Background");
            }
        }

        public string Header
        {
            get { return this.header; }
            set
            {
                this.header = value;
                this.OnPropertyChanged("Header");
            }
        }

        public bool IsVisible
        {
            get { return this.isVisible; }
            set
            {
                this.isVisible = value;
                this.OnPropertyChanged("IsVisible");
            }
        }

        public ObservableCollection<RibbonTabData> TabDataCollection
        {
            get { return this.tabs; }
        }

        #endregion Properties
    }
}