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
namespace Probel.NDoctor.Domain.DTO.Helpers
{
    using Probel.Helpers.Assertion;
    using Probel.NDoctor.Domain.DTO.Components;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.DTO.Properties;

    public static class TranslateEnum
    {
        #region Methods

        public static string Translate(this TagCategory tagtype)
        {
            switch (tagtype)
            {
                case TagCategory.Appointment:
                    return Messages.TagType_Appointment;
                case TagCategory.Doctor:
                    return Messages.TagType_Doctor;
                case TagCategory.Picture:
                    return Messages.TagType_Picture;
                case TagCategory.MedicalRecord:
                    return Messages.TagType_MedicalRecord;
                case TagCategory.Patient:
                    return Messages.TagType_Patient;
                case TagCategory.Drug:
                    return Messages.TagType_Drug;
                case TagCategory.Prescription:
                    return Messages.TagType_Prescription;
                case TagCategory.PrescriptionDocument:
                    return Messages.TagType_PrescriptionDocument;
                case TagCategory.Pathology:
                    return Messages.TagType_Pathology;
                default:
                    Assert.FailOnEnumeration(tagtype);
                    return null;
            }
        }

        public static string Translate(this FamilyRelations tagtype)
        {
            switch (tagtype)
            {
                case FamilyRelations.Parent:
                    return Messages.TagType_Parent;
                case FamilyRelations.Child:
                    return Messages.TagType_Child;

                default:
                    Assert.FailOnEnumeration(tagtype);
                    return null;
            }
        }

        public static string Translate(this Gender gender)
        {
            var result = string.Empty;
            switch (gender)
            {
                case Gender.Male:
                    result = Messages.Gender_Male;
                    break;
                case Gender.Female:
                    result = Messages.Gender_Female;
                    break;
                default:
                    Assert.FailOnEnumeration(gender);
                    break;
            }
            return result;
        }

        public static string Translate(this SearchOn searchType)
        {
            var result = string.Empty;
            switch (searchType)
            {
                case SearchOn.FirstName:
                    result = Messages.SearchOn_FirstName;
                    break;
                case SearchOn.LastName:
                    result = Messages.SearchOn_LastName;
                    break;
                case SearchOn.FirstAndLastName:
                    result = Messages.SearchOn_Both;
                    break;
                default:
                    Assert.FailOnEnumeration(searchType);
                    break;
            }
            return result;
        }

        #endregion Methods
    }
}