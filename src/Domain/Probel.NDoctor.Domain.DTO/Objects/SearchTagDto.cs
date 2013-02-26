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
    /// <summary>
    /// Represents a search tag
    /// </summary>
    public class SearchTagDto : BaseDto
    {
        #region Fields

        private string explanations;
        private string name;

        #endregion Fields

        #region Properties

        public string Explanations
        {
            get { return this.explanations; }
            set
            {
                this.explanations = value;
                this.OnPropertyChanged(() => Explanations);
            }
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.OnPropertyChanged(() => Name);
            }
        }

        #endregion Properties
    }
}