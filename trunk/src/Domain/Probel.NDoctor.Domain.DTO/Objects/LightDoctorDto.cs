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
namespace Probel.NDoctor.Domain.DTO.Objects
{
    using System;

    /// <summary>
    /// Light version of a doctor
    /// </summary>
    [Serializable]
    public class LightDoctorDto : PersonDto
    {
        #region Fields

        private TagDto specialisation;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the name of the displayed.
        /// </summary>
        /// <value>
        /// The name of the displayed.
        /// </value>
        public string DisplayedName
        {
            get
            {
                if (DebugMode)
                {
                    return string.Format("[{0}] {1} {2}"
                        , this.Id
                        , this.FirstName
                        , this.LastName);
                }
                else
                {
                    return string.Format("{0} {1}"
                    , this.FirstName
                    , this.LastName);
                }
            }
        }

        /// <summary>
        /// Gets or sets the specialisation.
        /// </summary>
        /// <value>
        /// The specialisation.
        /// </value>
        public TagDto Specialisation
        {
            get { return this.specialisation; }
            set
            {
                this.specialisation = value;
                this.OnPropertyChanged(() => Specialisation);
            }
        }

        #endregion Properties
    }
}