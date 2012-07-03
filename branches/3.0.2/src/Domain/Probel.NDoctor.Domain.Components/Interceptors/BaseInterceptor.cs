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

namespace Probel.NDoctor.Domain.Components.Interceptors
{
    using Castle.DynamicProxy;

    using Probel.NDoctor.Domain.DAL.Helpers;

    internal abstract class BaseInterceptor : IInterceptor
    {
        #region Methods

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        public abstract void Intercept(IInvocation invocation);

        /// <summary>
        /// Indicates whether the invocated method should be invoked or not.
        /// A method should be ignored when it is eihter:
        ///  - a non public method
        ///  - a ctor
        ///  - a method marked with the <see cref="InspectionIngnoredAttribute"/> attribute
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <returns><c>True</c> if this method should be ignored; otherwise <c>False</c></returns>
        protected bool Ignore(IInvocation invocation)
        {
            var attributes = invocation.MethodInvocationTarget.GetCustomAttributes(typeof(InspectionIgnoredAttribute), true);
            return attributes.Length > 0 && invocation.Method.IsPublic && !invocation.Method.IsConstructor;
        }

        #endregion Methods
    }
}