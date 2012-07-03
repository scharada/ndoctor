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

    using Probel.Mvvm.Validation;
    using Probel.NDoctor.Domain.DTO.Exceptions;
    using Probel.NDoctor.Domain.DTO.Properties;

    [Serializable]
    internal abstract class Validator<T> : IValidator
        where T : ValidatableObject
    {
        #region Properties

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>
        public string Error
        {
            get { return Messages.Invalid_Data; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Sets the validation rules for the specified instance.
        /// </summary>
        /// <param name="item">The item.</param>
        public abstract void SetValidationLogic(T item);

        /// <summary>
        /// Sets the validation rules for the specified instance.
        /// </summary>
        /// <param name="item">The item.</param>
        public void SetValidationLogic(ValidatableObject item)
        {
            T toValidate = item as T;
            if (toValidate == null) throw new ValidationException("Impossible to cast to a validateable type. Are you sure you specified the good item to validate?");
            this.SetValidationLogic(toValidate);
        }

        #endregion Methods
    }
}