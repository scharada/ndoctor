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
namespace Probel.NDoctor.Plugins.Administration.ViewModel
{
    using System;

    using Probel.NDoctor.Domain.DTO.Collections;
    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.View.Plugins.Helpers;

    internal class TagViewModel : TagDto
    {
        #region Fields

        private readonly ErrorHandler Handle;

        #endregion Fields

        #region Constructors

        public TagViewModel()
            : base(TagCategory.Appointment)
        {
            this.Handle = new ErrorHandler(this);
            this.Categories = TagCategoryCollection.Build();
        }

        #endregion Constructors

        #region Properties

        public TagCategoryCollection Categories
        {
            get;
            private set;
        }

        public Tuple<string, TagCategory> SelectedTag
        {
            get { return new Tuple<string, TagCategory>(this.Category.Translate(), this.Category); }
            set
            {
                this.Category = value.Item2;
                this.OnPropertyChanged(() => SelectedTag);
            }
        }

        #endregion Properties
    }
}