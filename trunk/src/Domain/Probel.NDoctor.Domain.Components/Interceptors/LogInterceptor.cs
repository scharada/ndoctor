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

    using log4net;

    internal class LogInterceptor : BaseInterceptor
    {
        #region Fields

        private readonly ILog log = LogManager.GetLogger(typeof(LogInterceptor));

        #endregion Fields

        #region Methods

        public override void Intercept(IInvocation invocation)
        {
            if (!this.Ignore(invocation))
                log.Debug(string.Format("Intercepting the method '{0}' of the component '{1}'", invocation.Method.Name, invocation.TargetType.Name));
            invocation.Proceed();
        }

        #endregion Methods
    }
}