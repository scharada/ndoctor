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
namespace Probel.NDoctor.View.Plugins
{
    using System;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.Linq;
    using System.Linq.Expressions;

    public class FilteredCatalog : ComposablePartCatalog, INotifyComposablePartCatalogChanged
    {
        #region Fields

        private readonly ComposablePartCatalog inner;
        private readonly INotifyComposablePartCatalogChanged innerNotifyChange;
        private readonly IQueryable<ComposablePartDefinition> partsQuery;

        #endregion Fields

        #region Constructors

        public FilteredCatalog(ComposablePartCatalog inner,
            Expression<Func<ComposablePartDefinition, bool>> expression)
        {
            this.inner = inner;
            this.innerNotifyChange = inner as INotifyComposablePartCatalogChanged;
            this.partsQuery = inner.Parts.Where(expression);
        }

        #endregion Constructors

        #region Events

        public event EventHandler<ComposablePartCatalogChangeEventArgs> Changed
        {
            add
            {
                if (this.innerNotifyChange != null)
                    this.innerNotifyChange.Changed += value;
            }
            remove
            {
                if (this.innerNotifyChange != null)
                    this.innerNotifyChange.Changed -= value;
            }
        }

        public event EventHandler<ComposablePartCatalogChangeEventArgs> Changing
        {
            add
            {
                if (this.innerNotifyChange != null)
                    this.innerNotifyChange.Changing += value;
            }
            remove
            {
                if (this.innerNotifyChange != null)
                    this.innerNotifyChange.Changing -= value;
            }
        }

        #endregion Events

        #region Properties

        public override IQueryable<ComposablePartDefinition> Parts
        {
            get
            {
                return this.partsQuery;
            }
        }

        #endregion Properties
    }
}