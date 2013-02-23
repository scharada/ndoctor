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
    using System;
    using System.Diagnostics;

    using Castle.DynamicProxy;

    using log4net;

    [DebuggerStepThrough]
    public abstract class BaseInterceptor : IInterceptor
    {
        #region Fields

        protected readonly ILog Logger;

        #endregion Fields

        #region Constructors

        public BaseInterceptor()
        {
            this.Logger = LogManager.GetLogger(this.GetType());
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        public abstract void Intercept(IInvocation invocation);

        /// <summary>
        /// Get an array of all the specified attribute. If not decorated, returns <c>Null</c>
        /// A method should be ignored when it is eihter:
        /// - a non public method
        /// - a ctor
        /// - a method marked with the <see cref="InspectionIngnoredAttribute"/> attribute
        /// </summary>
        /// <typeparam name="T">The type of the attribute to check</typeparam>
        /// <param name="invocation">The invocation.</param>
        /// <returns>
        ///   The array of all the specified attribute. If not decorated, returns <c>Null</c>
        /// </returns>
        protected T[] GetAttribute<T>(IInvocation invocation)
            where T : Attribute
        {
            var memberAttributes = invocation.MethodInvocationTarget.GetCustomAttributes(typeof(T), true) as T[];
            var objectAttributes = invocation.TargetType.GetCustomAttributes(typeof(T), true) as T[];

            return (objectAttributes != null && objectAttributes.Length > 0)
                ? objectAttributes
                : memberAttributes;
        }

        /// <summary>
        /// Indicates whether the invocated method is decorated with the specified attribute.
        /// A method should be ignored when it is eihter:
        /// - a non public method
        /// - a ctor
        /// - a method marked with the <see cref="InspectionIngnoredAttribute"/> attribute
        /// </summary>
        /// <typeparam name="T">The type of the attribute to check</typeparam>
        /// <param name="invocation">The invocation.</param>
        /// <returns>
        ///   <c>True</c> if this method should be ignored; otherwise <c>False</c>
        /// </returns>
        protected bool IsDecoratedWith<T>(IInvocation invocation)
            where T : Attribute
        {
            var attributes = invocation.MethodInvocationTarget.GetCustomAttributes(typeof(T), true);
            return attributes.Length > 0 && invocation.Method.IsPublic && !invocation.Method.IsConstructor;
        }

        #endregion Methods
    }
}