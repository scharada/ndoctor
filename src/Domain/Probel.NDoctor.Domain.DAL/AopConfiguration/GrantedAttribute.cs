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
namespace Probel.NDoctor.Domain.DAL.AopConfiguration
{
    using System;

    /// <summary>
    /// Explicit authorisation are done throught this Attribute. An attribute override implicit rules
    /// 
    /// If no attribute is set to a method or a class, this implicit authorisation is done:
    /// * Read : every method that contains "Find" or "GetAll"
    /// * Write: every method that contains "Create", "Remove" or "Update"    
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class GrantedAttribute : Attribute
    {
        #region Constructors

        public GrantedAttribute(string to)
        {
            this.Task = to;
        }

        #endregion Constructors

        #region Properties

        public string Task
        {
            get;
            private set;
        }

        #endregion Properties
    }
}