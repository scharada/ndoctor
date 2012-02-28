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
namespace Probel.Helpers.Conversions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    [Serializable]
    public static class ObservableCollectionFiller
    {
        #region Methods

        /// <summary>
        /// Adds specified collection into the ObservableCollection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oCollection">The o collection.</param>
        /// <param name="collection">The collection.</param>
        public static void AddRange<T>(this ObservableCollection<T> oCollection, IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                oCollection.Add(item);
            }
        }

        /// <summary>
        /// Clears the ObservableCollection and refill it with the specified collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oCollection">The o collection.</param>
        /// <param name="collection">The collection.</param>
        public static void Refill<T>(this ObservableCollection<T> oCollection, IEnumerable<T> collection)
        {
            oCollection.Clear();
            oCollection.AddRange(collection);
        }

        #endregion Methods
    }
}