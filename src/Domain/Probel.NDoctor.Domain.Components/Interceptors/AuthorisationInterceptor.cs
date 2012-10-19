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

    using Probel.NDoctor.Domain.Components.AuthorisationPolicies;
    using Probel.NDoctor.Domain.DAL;
    using Probel.NDoctor.Domain.DAL.Helpers;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Objects;

    using StructureMap;

    /// <summary>
    /// This class manage authorisation. If the execution of the method is unauthorised, an exception is thrown.
    /// If no attribute is set to a method or a class, this implicit authorisation is done:
    /// * Read : every method that contains "Get" or "GetAll"
    /// * Write: every method that contains "Create", "Remove" or "Update"
    /// </summary>
    internal class AuthorisationInterceptor : BaseInterceptor
    {
        #region Fields

        private static readonly IAuthorisationPolicy policy = ObjectFactory.GetInstance<IAuthorisationPolicy>();
        private static readonly string[] ReadAuthorisations = new string[] { "get", "getall", "find", "findall" }; //"find" and "findall" are not officially supported
        private static readonly string[] WriteAuthorisations = new string[] { "create", "insert", "remove", "delete", "update" };

        #endregion Fields

        #region Properties

        public LightUserDto User
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        public override void Intercept(IInvocation invocation)
        {
            var hasRight = true;
            var authAttribute = GetAuthAttribute(invocation);

            if (!this.IsDecoratedWith<InspectionIgnoredAttribute>(invocation))
            {
                var name = invocation.MethodInvocationTarget.Name.ToLower();
                if (this.User == null && authAttribute.ToLower() == To.Everyone.ToLower())
                {
                    hasRight = true;
                }
                else if (!string.IsNullOrWhiteSpace(authAttribute))
                {
                    hasRight = IsGrantedWithAttribute(invocation);
                }
                else if (this.IsWriteMethod(name))
                {
                    hasRight = policy.IsGranted(To.Write, this.User);
                }
                else if (this.IsReadMethod(name))
                {
                    hasRight = policy.IsGranted(To.Read, this.User);
                }
            }

            if (hasRight) { invocation.Proceed(); }
            else
            {
                Logger.WarnFormat("Not granted to execute {0}.{1} [Role: '{2}']"
                    , invocation.TargetType.Name
                    , invocation.Method.Name
                    , (this.User != null && this.User.AssignedRole != null) ? this.User.AssignedRole.Name : "EMPTY");
                throw new AuthorisationException(invocation.TargetType, invocation.Method);
            }
        }

        /// <summary>
        /// Gets the authoriation attribute of the invocation if exists.
        /// It'll first look for a Class Attribute that overrides every method attributes
        /// It'll then look for a Member Attribute end eventually returns an empty string if
        /// neither the Class nor the Member is decorated.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <returns></returns>
        private string GetAuthAttribute(IInvocation invocation)
        {
            var authorisation = string.Empty;
            var memberAttributes = invocation.MethodInvocationTarget.GetCustomAttributes(typeof(GrantedAttribute), true);
            var objectAttributes = invocation.TargetType.GetCustomAttributes(typeof(GrantedAttribute), true);

            if (objectAttributes.Length > 0)
            {
                return ((GrantedAttribute)objectAttributes[0]).Task.ToLower();
            }
            else if (memberAttributes.Length > 0)
            {
                return ((GrantedAttribute)memberAttributes[0]).Task.ToLower();
            }
            else { return authorisation; }
        }

        private bool IsGrantedWithAttribute(IInvocation invocation)
        {
            var granted = GetAuthAttribute(invocation);
            return policy.IsGranted(granted, this.User);
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