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

namespace Probel.NDoctor.Domain.DAL.Macro
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Probel.NDoctor.Domain.DAL.Entities;
    using Probel.NDoctor.Domain.DTO.Objects;

    public class MacroBuilder
    {
        #region Fields

        private const string TEMPLATE = @"\$\w*\$";

        private Patient patient;

        #endregion Fields

        #region Constructors

        public MacroBuilder(Patient patient)
        {
            this.patient = patient;
        }

        #endregion Constructors

        #region Methods

        public static bool IsValidExpression(string macro)
        {
            var regex = new Regex(TEMPLATE);
            var markups = Markups.All();

            foreach (var match in regex.Matches(macro))
            {
                var count = (from m in markups
                             where m.ToLower() == match.ToString().ToLower()
                             select m).Count();
                if (count == 0) return false;
            }
            return true;
        }

        public string Resolve(string macro)
        {
            macro = this.Standardise(macro);

            if (MacroBuilder.IsValidExpression(macro))
            {
                macro = macro.Replace(Markups.FirstName, patient.FirstName);
                macro = macro.Replace(Markups.LastName, patient.LastName);
                macro = macro.Replace(Markups.Birthdate, patient.BirthDate.ToShortDateString());
                macro = macro.Replace(Markups.Height, patient.Height.ToString());
                macro = macro.Replace(Markups.Now, DateTime.Now.ToString("HH:mm"));
                macro = macro.Replace(Markups.Today, DateTime.Today.ToShortDateString());
                macro = macro.Replace(Markups.Age, this.GetAge(patient));
                return macro;
            }
            else { throw new InvalidMacroException(); }
        }

        private string GetAge(Patient patient)
        {
            var timeSpan = DateTime.Now - patient.BirthDate;
            return (timeSpan.Days / 365).ToString();
        }

        private string Standardise(string macro)
        {
            return Regex.Replace(macro, TEMPLATE, e => e.ToString().ToLower());
        }

        #endregion Methods
    }
}