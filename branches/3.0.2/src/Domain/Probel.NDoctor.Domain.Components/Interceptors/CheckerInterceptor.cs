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

    using Castle.DynamicProxy;

    using log4net;

    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DAL.Helpers;

    internal class CheckerInterceptor : BaseInterceptor
    {
        #region Fields

        private readonly ILog log = LogManager.GetLogger(typeof(CheckerInterceptor));

        #endregion Fields

        #region Methods

        public override void Intercept(IInvocation invocation)
        {
            try
            {
                if (invocation.InvocationTarget is BaseComponent)
                {
                    if (!this.Ignore(invocation))
                    {
                        var component = invocation.InvocationTarget as BaseComponent;

                        new ComponentDecorator(component).CheckSession();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Warn(string.Format("An error occured when intercepting the method '{0}' of the component '{1}'", invocation.Method.Name, invocation.TargetType.Name)
                    , ex);
                throw;
            }
            finally { invocation.Proceed(); }
        }

        #endregion Methods
    }
}