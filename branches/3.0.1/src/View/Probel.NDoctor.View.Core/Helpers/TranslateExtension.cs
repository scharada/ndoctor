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
namespace Probel.NDoctor.View.Core.Helpers
{
    using System;
    using System.Resources;
    using System.Windows.Markup;

    using Probel.Helpers.Assertion;
    using Probel.NDoctor.View.Core.Properties;

    /// <summary>
    /// Extension class to manage I18N
    /// </summary>    
    //[MarkupExtensionReturnType(typeof(string), typeof(string))]
    public class TranslateExtension : MarkupExtension
    {
        #region Fields

        private const string NotFoundError = "#StringNotFound#";

        private string key;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateExtension"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public TranslateExtension(string key)
        {
            this.key = key;
        }

        #endregion Constructors

        #region Properties

        public static ResourceManager ResourceManager
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Returns an object that is set as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        /// <returns>
        /// The object value to set on the property where the extension is applied.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Assert.IsNotNull(ResourceManager, "The resource manager wasn't configured");
            var messages = (ResourceManager == null)
                ? Messages.ResourceManager
                : ResourceManager;

            if (string.IsNullOrEmpty(this.key)) return NotFoundError;
            else return messages.GetString(this.key) ?? NotFoundError;
        }

        #endregion Methods
    }
}