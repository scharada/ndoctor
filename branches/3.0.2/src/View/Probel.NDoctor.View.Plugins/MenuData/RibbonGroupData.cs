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

    public class RibbonGroupData : RibbonBase
    {
        #region Fields

        private ObservableCollection<RibbonControlData> buttons = new ObservableCollection<RibbonControlData>();
        private string header;

        #endregion Fields

        #region Constructors

        public RibbonGroupData(string name, ObservableCollection<RibbonButtonData> buttonDataCollection)
            : this()
        {
            this.Header = name;
            foreach (var button in buttonDataCollection) this.buttons.Add(button);
        }

        public RibbonGroupData(string name, RibbonButtonData buttonData)
            : this(name, new ObservableCollection<RibbonButtonData>() { buttonData })
        {
        }

        public RibbonGroupData(string name)
            : this(name, new ObservableCollection<RibbonButtonData>())
        {
        }

        public RibbonGroupData(string name, int order)
            : this(name)
        {
            this.Order = order;
        }

        public RibbonGroupData()
        {
            this.buttons = new ObservableCollection<RibbonControlData>();
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<RibbonControlData> ButtonDataCollection
        {
            get { return this.buttons; }
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

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return string.Format("Group: {0}", this.Header);
        }

        #endregion Methods
    }
}