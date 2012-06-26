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
namespace Probel.NDoctor.Domain.DTO.Validators
{
    using System;

    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Domain.DTO.Properties;

    [Serializable]
    internal class BmiValidator : Validator<BmiDto>
    {
        #region Methods

        public override void SetValidationLogic(BmiDto item)
        {
            item.AddValidationRule(() => item.Weight
                , () => item.Weight >= 1 && item.Weight <= 500
                , Messages.Invalid_Weight);

            item.AddValidationRule(() => item.Height
                , () => item.Height >= 10 && item.Height <= 300
                , Messages.Invalid_Height);
        }

        #endregion Methods
    }
}