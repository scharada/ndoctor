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
        /// Indicates whether the invocated method should be invoked or not
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <returns></returns>
        protected bool Ignore(IInvocation invocation)
        {
            var attributes = invocation.MethodInvocationTarget.GetCustomAttributes(typeof(InspectionIgnoredAttribute), true);
            return attributes.Length > 0;
        }

        #endregion Methods
    }
}