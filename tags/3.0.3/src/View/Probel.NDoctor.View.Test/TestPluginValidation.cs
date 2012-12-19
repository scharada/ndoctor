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
namespace Probel.NDoctor.View.Test
{
    using System;

    using NSubstitute;

    using NUnit.Framework;

    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;

    [TestFixture]
    [Category("View")]
    public class TestPluginValidation
    {
        #region Methods

        [Test]
        public void CanCheckConstraint_MinimimMode_HostSameVersion()
        {
            var constraint = new PluginValidator("2.0.0.0", ValidationMode.Minimum);

            Assert.IsTrue(constraint.IsValid(PluginContext.Host));
        }

        [Test]
        public void CanCheckConstraint_MinimumMode_HostBiggerVersion()
        {
            var constraint = new PluginValidator("1.0.0.0", ValidationMode.Minimum);
            Assert.IsTrue(constraint.IsValid(PluginContext.Host));
        }

        [Test]
        public void CanCheckConstraint_StrictMode_HostSameVersion()
        {
            var constraint = new PluginValidator("2.0.0.0", ValidationMode.Strict);
            Assert.IsTrue(constraint.IsValid(PluginContext.Host));

            Assert.IsTrue(constraint.IsValid(PluginContext.Host));
        }

        [Test]
        public void FailCheckingConstraint_MinimumMode_HostSmallerVersion()
        {
            var constraint = new PluginValidator("3.0.0.0", ValidationMode.Strict);

            Assert.IsFalse(constraint.IsValid(PluginContext.Host));
        }

        [Test]
        public void FailCheckingConstraint_StrictMode_HostSmallerVersion()
        {
            var constraint = new PluginValidator("3.0.0.0", ValidationMode.Strict);
            Assert.IsFalse(constraint.IsValid(PluginContext.Host));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void FailOnVersionCtor()
        {
            var constraint = new PluginValidator("bonjour", ValidationMode.Strict);
        }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            PluginContext.Host = Substitute.For<IPluginHost>();
            PluginContext.Host.HostVersion.Returns(new Version("2.0.0.0"));
        }

        #endregion Methods
    }
}