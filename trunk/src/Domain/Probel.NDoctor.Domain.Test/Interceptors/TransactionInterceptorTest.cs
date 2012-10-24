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

namespace Probel.NDoctor.Domain.Test.Interceptors
{
    using Castle.DynamicProxy;

    using NHibernate;

    using NSubstitute;

    using NUnit.Framework;

    using Probel.NDoctor.Domain.Components.Interceptors;
    using Probel.NDoctor.Domain.DAL.Cfg;
    using Probel.NDoctor.Domain.DAL.Components;
    using Probel.NDoctor.Domain.Test.Helpers;

    [TestFixture]
    [Category(Categories.UnitTest)]
    public class TransactionInterceptorTest
    {
        #region Methods

        [Test]
        public void CheckAutoTransaction_DecoratedIsNotTransactional_NotWrappedInATransaction()
        {
            var interceptor = new TransactionInterceptor();
            var invocation = Substitute.For<IInvocation>();

            invocation.InvocationTarget.Returns(new SqlComponent());
            invocation.MethodInvocationTarget.Returns(typeof(SqlComponent).GetMethod("ExecuteNonQuery"));
            invocation.Method.Returns(typeof(SqlComponent).GetMethod("ExecuteNonQuery"));
            invocation.When(e => e.Proceed()).Do(e => Nothing());

            interceptor.Intercept(invocation);
            Assert.IsFalse(interceptor.WasTransactional);
        }

        [Test]
        public void CheckAutoTransaction_NotDecoratedIsTransactional_WrappedInATransaction()
        {
            var interceptor = new TransactionInterceptor();
            var invocation = Substitute.For<IInvocation>();

            invocation.InvocationTarget.Returns(new SqlComponent());
            invocation.MethodInvocationTarget.Returns(typeof(SqlComponent).GetMethod("GetAllDrugs"));
            invocation.Method.Returns(typeof(SqlComponent).GetMethod("GetAllDrugs"));
            invocation.When(e => e.Proceed()).Do(e => Nothing());

            interceptor.Intercept(invocation);
            Assert.IsTrue(interceptor.WasTransactional);
        }

        [SetUp]
        public void FixtureSetup()
        {
            new NUnitConfigWrapper(new DalConfigurator())
                .ConfigureForUnitTest(Substitute.For<ISessionFactory>());
        }

        /// <summary>
        /// A methods that does nothing. It is created to ease unit test reading
        /// </summary>
        private void Nothing()
        {
        }

        #endregion Methods
    }
}