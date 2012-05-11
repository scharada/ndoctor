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
    using System.Linq;

    using Probel.Helpers.Conversions;
    using Probel.Mvvm.DataBinding;

    public class RibbonTabData : RibbonBase
    {
        #region Fields

        private string contextualTabGroupHeader;
        private string header;
        private bool isSelected;
        private ObservableCollection<RibbonGroupData> ribbonGroupData;

        #endregion Fields

        #region Constructors

        public RibbonTabData()
        {
            this.ribbonGroupData = new ObservableCollection<RibbonGroupData>();
        }

        public RibbonTabData(string header, ObservableCollection<RibbonGroupData> ribbonGroupData)
        {
            this.Header = header;

            this.ribbonGroupData = new ObservableCollection<RibbonGroupData>();
            foreach (var group in ribbonGroupData) this.ribbonGroupData.Add(group);
        }

        public RibbonTabData(string header, RibbonGroupData ribbonGroupData)
            : this(header, new ObservableCollection<RibbonGroupData>() { ribbonGroupData })
        {
        }

        public RibbonTabData(string header)
        {
            this.Header = header;
            this.ContextualTabGroupHeader = header;
            this.ribbonGroupData = new ObservableCollection<RibbonGroupData>();
        }

        #endregion Constructors

        #region Properties

        public string ContextualTabGroupHeader
        {
            get { return this.contextualTabGroupHeader; }
            set
            {
                this.contextualTabGroupHeader = value;
                this.OnPropertyChanged(() => ContextualTabGroupHeader);
            }
        }

        public ObservableCollection<RibbonGroupData> GroupDataCollection
        {
            get
            {
                var sorted = this.ribbonGroupData.OrderBy(e => e.Order).ToList();
                this.ribbonGroupData.Refill(sorted);
                return this.ribbonGroupData;
            }
        }

        public string Header
        {
            get { return this.header; }
            set
            {
                this.header = value;
                this.OnPropertyChanged(() => Header);
            }
        }

        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                this.OnPropertyChanged(() => IsSelected);
            }
        }

        #endregion Properties
    }
}