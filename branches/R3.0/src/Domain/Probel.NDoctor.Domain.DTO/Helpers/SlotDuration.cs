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
    #region Enumerations

    /// <summary>
    /// Represents a slot duration. Tha's how much time last a meeting
    /// </summary>
    public enum SlotDuration
    {
        /// <summary>
        /// A slot duration's 30 minutes
        /// </summary>
        ThirtyMinutes,
        /// <summary>
        /// A slot duration's 1 hour
        /// </summary>
        OneHour,
        /// <summary>
        /// The duration is not configured
        /// </summary>
        NotConfigured,
    }

    #endregion Enumerations
}