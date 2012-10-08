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

    using Probel.NDoctor.Domain.DAL.Cfg;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.DAL.Helpers;

    internal class TransactionInterceptor : BaseInterceptor
    {
        #region Fields

        private static readonly object locker = new object();

        #endregion Fields

        #region Methods

        public override void Intercept(IInvocation invocation)
        {
            if (invocation.InvocationTarget is BaseComponent)
            {
                lock (locker)
                {
                    var component = invocation.InvocationTarget as BaseComponent;

                    if (!this.IsDecoratedWith<ExcludeFromTransactionAttribute>(invocation))
                    {
                        this.WrapInTransaction(invocation, component);
                    }
                    else
                    {
                        this.WrapInSession(invocation, component);
                    }
                }
            }
            else
            {
                this.Logger.DebugFormat("Method '{0}' of component '{1}' is not executed into a transaction."
                    , invocation.Method.Name
                    , invocation.TargetType.Name);
                invocation.Proceed();
            }
        }

        private void WrapInSession(IInvocation invocation, BaseComponent component)
        {
            using (component.Session = DalConfigurator.SessionFactory.OpenSession())
            {
                invocation.Proceed();
            }
        }

        private void WrapInTransaction(IInvocation invocation, BaseComponent component)
        {
            using (component.Session = DalConfigurator.SessionFactory.OpenSession())
            using (var tx = component.Session.BeginTransaction())
            {
                invocation.Proceed();
                tx.Commit();
            }
        }

        #endregion Methods
    }
}