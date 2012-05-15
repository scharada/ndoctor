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
namespace Probel.NDoctor.Domain.DTO.Collections
{
    using System;
    using System.Collections.ObjectModel;

    using Probel.NDoctor.Domain.DTO.Helpers;
    using Probel.NDoctor.Domain.DTO.Objects;

    public class TagCategoryCollection : ObservableCollection<Tuple<string, TagCategory>>
    {
        #region Methods

        /// <summary>
        /// Builds this instance with all the different values.
        /// </summary>
        /// <returns></returns>
        public static TagCategoryCollection Build()
        {
            var collection = new TagCategoryCollection();
            collection.Add(new Tuple<string, TagCategory>(TagCategory.Appointment.Translate(), TagCategory.Appointment));
            collection.Add(new Tuple<string, TagCategory>(TagCategory.Doctor.Translate(), TagCategory.Doctor));
            collection.Add(new Tuple<string, TagCategory>(TagCategory.Drug.Translate(), TagCategory.Drug));
            collection.Add(new Tuple<string, TagCategory>(TagCategory.MedicalRecord.Translate(), TagCategory.MedicalRecord));
            collection.Add(new Tuple<string, TagCategory>(TagCategory.Pathology.Translate(), TagCategory.Pathology));
            collection.Add(new Tuple<string, TagCategory>(TagCategory.Patient.Translate(), TagCategory.Patient));
            collection.Add(new Tuple<string, TagCategory>(TagCategory.Picture.Translate(), TagCategory.Picture));
            collection.Add(new Tuple<string, TagCategory>(TagCategory.Prescription.Translate(), TagCategory.Prescription));
            collection.Add(new Tuple<string, TagCategory>(TagCategory.PrescriptionDocument.Translate(), TagCategory.PrescriptionDocument));
            return collection;
        }

        #endregion Methods
    }
}