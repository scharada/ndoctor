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

    using Probel.NDoctor.Domain.DAL;

    /// <summary>
    /// This class manage authorisation. If the execution of the method is unauthorised, an exception is thrown.
    /// 
    /// If no attribute is set to a method or a class, this implicit authorisation is done:
    /// * Read : every method that contains "Find" or "GetAll"
    /// * Write: every method that contains "Create", "Remove" or "Update"    
    /// </summary>
    internal class AuthorisationInterceptor : BaseInterceptor
    {
        #region Fields

        private static readonly ILog Logger = LogManager.GetLogger(typeof(AuthorisationInterceptor));
        private static readonly string[] ReadAuthorisations = new string[] { "find", "getall" };
        private static readonly string[] WriteAuthorisations = new string[] { "create", "remove", "update" };

        #endregion Fields

        #region Methods

        public override void Intercept(IInvocation invocation)
        {
            if (!this.Ignore(invocation))
            {
                var name = invocation.MethodInvocationTarget.Name.ToLower();
                if (this.HasAuthAttribute(invocation))
                {
                    Logger.DebugFormat("\t\t'{0}' is granted to {1} thanks to an attribute"
                        , invocation.MethodInvocationTarget.Name
                        , this.GetAuthAttribute(invocation));
                }
                if (this.IsWriteMethod(name))
                {
                    Logger.DebugFormat("\t\t'{0}' is granted to write"
                        , invocation.MethodInvocationTarget.Name);
                }
                if (this.IsReadMethod(name))
                {
                    Logger.DebugFormat("\t\t'{0}' is granted to read"
                        , invocation.MethodInvocationTarget.Name);
                }
            }

            invocation.Proceed();
        }

        private string GetAuthAttribute(IInvocation invocation)
        {
            var authorisation = string.Empty;
            var attributes = invocation.MethodInvocationTarget.GetCustomAttributes(typeof(GrantedAttribute), true);
            if (attributes.Length > 0)
            {
                return ((GrantedAttribute)attributes[0]).Task;
            }

            return authorisation;
        }

        private bool HasAuthAttribute(IInvocation invocation)
        {
            var attributes = invocation.MethodInvocationTarget.GetCustomAttributes(typeof(GrantedAttribute), true);
            return attributes.Length > 0;
        }

        private bool IsReadMethod(string name)
        {
            foreach (var authorisation in ReadAuthorisations)
            {
                if (name.Contains(authorisation)) { return true; }
            }
            return false;
        }

        private bool IsWriteMethod(string name)
        {
            foreach (var authorisation in WriteAuthorisations)
            {
                if (name.Contains(authorisation)) { return true; }
            }
            return false;
        }

        #endregion Methods
    }
}