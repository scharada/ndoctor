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
namespace Probel.NDoctor.Plugins.PrescriptionManager.Helpers
{
    using System;

    using Probel.Helpers.Events;
    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Static repository that fires statis events
    /// </summary>
    internal static class Notifyer
    {
        #region Events

        public static event EventHandler<EventArgs<DrugDto>> DrugSelected;

        /// <summary>
        /// Occurs when an item is added/removed/updated.
        /// </summary>
        public static event EventHandler ItemChanged;

        /// <summary>
        /// Occurs when the prescription search is done and prescriptions are found.
        /// </summary>
        public static event EventHandler<EventArgs<PrescriptionResultDto>> PrescriptionFound;

        /// <summary>
        /// Occurs when the user is removing a prescription.
        /// </summary>
        public static event EventHandler<EventArgs<PrescriptionDto>> PrescriptionRemoving;

        #endregion Events

        #region Methods

        public static void OnDrugSelected(ViewModel.SearchDrugViewModel sender, DrugDto selectedDrug)
        {
            if (DrugSelected != null)
            {
                DrugSelected(sender, new EventArgs<DrugDto>(selectedDrug));
            }
        }

        /// <summary>
        /// Called when an item was added/removed/updated.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public static void OnItemChanged(object sender)
        {
            if (ItemChanged != null)
                ItemChanged(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Called when user the prescription search is done and results are found.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="result">The document.</param>
        public static void OnPrescriptionFound(object sender, PrescriptionResultDto result)
        {
            if (PrescriptionFound != null)
            {
                PrescriptionFound(sender, new EventArgs<PrescriptionResultDto>(result));
            }
        }

        /// <summary>
        /// Called when user is removing a prescription.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public static void OnPrescriptionRemoving(object sender, PrescriptionDto prescription)
        {
            if (PrescriptionRemoving != null)
            {
                PrescriptionRemoving(sender, new EventArgs<PrescriptionDto>(prescription));
            }
        }

        #endregion Methods
    }
}