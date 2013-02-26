#region Header

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

#endregion Header

namespace Probel.NDoctor.Domain.DTO.Specifications.Patients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.DTO.Specifications.Patients;

    public class PatientSpecificationFactory
    {
        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="PatientSpecificationFactory"/> class from being created.
        /// </summary>
        internal PatientSpecificationFactory()
        {
        }

        #endregion Constructors

        #region Methods

        public Specification<LightPatientDto> BirthdateIsBetween(DateTime after, DateTime before)
        {
            return new FindPatientByBirthDateSpecification(after, before);
        }

        public Specification<LightPatientDto> CityContains(string criteria)
        {
            return new FindPatientByCitySpecification(criteria);
        }

        public Specification<LightPatientDto> InscriptionIsBetween(DateTime after, DateTime before)
        {
            return new FindPatientByInscriptionSpecification(after, before);
        }

        public Specification<LightPatientDto> IsAnything()
        {
            return new EmptySpecification<LightPatientDto>();
        }

        public Specification<LightPatientDto> LastNameContains(string criteria)
        {
            return new FindPatientByLastNameSpecification(criteria);
        }

        public Specification<LightPatientDto> LastUpdateIsBetween(DateTime after, DateTime before)
        {
            return new FindPatientByLastUpdateSpecification(after, before);
        }

        public Specification<LightPatientDto> None()
        {
            return new GetNothingSpecification<LightPatientDto>();
        }

        public Specification<LightPatientDto> ProfessionIs(ProfessionDto profession)
        {
            return new FindPatientByProfessionSpecification(profession);
        }

        public Specification<LightPatientDto> ReasonContains(string criteria)
        {
            return new FindPatientByReasonSpecification(criteria);
        }

        #endregion Methods
    }
}