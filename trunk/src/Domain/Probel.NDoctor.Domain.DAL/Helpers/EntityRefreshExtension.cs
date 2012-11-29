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
using NHibernate;
using Probel.NDoctor.Domain.DAL.Entities;
using Probel.NDoctor.Domain.DTO.Objects;

namespace Probel.NDoctor.Domain.DAL.Helpers
{
    internal static class EntityRefreshExtension
    {
        public static void Refresh(this Patient patient, PatientDto from, ISession session)
        {
            patient.Tag = session.Get<Tag>(from.Tag.Id) ?? patient.Tag;
            patient.Insurance = session.Get<Insurance>(from.Insurance.Id) ?? patient.Insurance;
            patient.Practice = session.Get<Practice>(from.Practice.Id) ?? patient.Practice;
            patient.Profession = session.Get<Profession>(from.Profession.Id) ?? patient.Profession;
            patient.Reputation = session.Get<Reputation>(from.Reputation.Id) ?? patient.Reputation;
        }

        public static void Refresh(this Picture picture, PictureDto from, ISession session)
        {
            picture.Tag = session.Get<Tag>(from.Tag.Id) ?? picture.Tag;
        }
    }
}
