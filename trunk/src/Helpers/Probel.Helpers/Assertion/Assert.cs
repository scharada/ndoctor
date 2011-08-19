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
namespace Probel.Helpers.Assertion
{
    using System;

    using Probel.Helpers.Assertion.Constraint;
    using Probel.Helpers.Assertion.Message;

    /// <summary>
    /// Makes assertions in the code to check the values
    /// </summary>
    public class Assert
    {
        #region Methods

        /// <summary>
        /// Provides an assertion failure.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The args.</param>
        public static void Fail(object message, params object[] args)
        {
            throw new AssertionException(string.Format(message.ToString(), args));
        }

        /// <summary>
        /// Provide an assertion failure due to unsopported enum type.
        /// </summary>
        /// <param name="enumeration">The enumeration.</param>
        public static void FailOnEnumeration(Enum enumeration)
        {
            throw new AssertionException(
                string.Format("The item '{0}' of the enumeration '{1}' is not supported."
                , enumeration.ToString()
                , enumeration.GetType().FullName));
        }

        /// <summary>
        /// Provide an assertion failure due to unsopported enum type.
        /// </summary>
        /// <param name="value">The unsupported value.</param>
        public static void FailOnEnumeration(string value)
        {
            throw new AssertionException(
                string.Format("The value '{0}' is not supported in the switch-case."
                , value.ToString()));
        }

        /// <summary>
        /// Determines whether the specified expression is false.
        /// </summary>
        /// <param name="expression">if set to <c>true</c> expression result is true.</param>
        public static void IsFalse(bool expression)
        {
            IsFalse(expression, null, null);
        }

        /// <summary>
        /// Determines whether the specified expression is false.
        /// </summary>
        /// <param name="expression">if set to <c>true</c> expression result is false.</param>
        /// <param name="message">The message to dislpay is assertion failed.</param>
        /// <param name="args">An <see cref="System.Object"/> array containing zero or more objects to format.</param>
        public static void IsFalse(bool expression, object message, params object[] args)
        {
            That(expression, Is.False, message, args);
        }

        /// <summary>
        /// Determines whether the specified expression is not null.
        /// </summary>
        /// <param name="expression">if set to <c>true</c>, the expression is not null.</param>
        /// <param name="message">The message to dislpay is assertion failed.</param>
        /// <param name="args">An <see cref="System.Object"/> array containing zero or more objects to format.</param>
        public static void IsNotNull(object expression, object message, params object[] args)
        {
            That(expression, Is.NotNull, message, args);
        }

        /// <summary>
        /// Determines whether the specified expression is not null.
        /// </summary>
        /// <param name="expression">if set to <c>true</c>, the expression is not null.</param>
        public static void IsNotNull(object expression)
        {
            IsNotNull(expression, null, null);
        }

        /// <summary>
        /// Determines whether the specified expression is null.
        /// </summary>
        /// <param name="expression">if set to <c>true</c>, the expression is null.</param>
        /// <param name="message">The message to dislpay is assertion failed.</param>
        /// <param name="args">An <see cref="System.Object"/> array containing zero or more objects to format.</param>
        public static void IsNull(object expression, object message, params object[] args)
        {
            That(expression, Is.Null, message, args);
        }

        /// <summary>
        /// Determines whether the specified expression is null.
        /// </summary>
        /// <param name="expression">if set to <c>true</c>, the expression is null.</param>
        public static void IsNull(object expression)
        {
            IsNull(expression, null, null);
        }

        /// <summary>
        /// Determines whether the specified expression is true.
        /// </summary>
        /// <param name="expression">if set to <c>true</c> [expression].</param>
        /// <param name="message">The message to dislpay is assertion failed.</param>
        /// <param name="args">An <see cref="System.Object"/> array containing zero or more objects to format.</param>
        public static void IsTrue(bool expression, object message, params object[] args)
        {
            That(expression, Is.True, message, args);
        }

        /// <summary>
        /// Determines whether the specified expression is true.
        /// </summary>
        /// <param name="expression">if set to <c>true</c> [expression].</param>
        public static void IsTrue(bool expression)
        {
            IsTrue(expression, null, null);
        }

        /// <summary>
        /// Determine whether the specified expression is of the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="message">The message to dislpay is assertion failed.</param>
        /// <param name="args">An <see cref="System.Object"/> array containing zero or more objects to format.</param>
        public static void OfType(Type type, object expression, object message, params object[] args)
        {
            That(expression, Is.OfType(type), message, args);
        }

        /// <summary>
        /// Determine whether the specified expression is of the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="expression">The expression.</param>
        public static void OfType(Type type, object expression)
        {
            OfType(type, expression, null, null);
        }

        /// <summary>
        /// Apply a constraint to an actual value, succeeding if the constraint
        /// is satisfied and throwing an assertion exception on failure.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="constraint">The constraint.</param>
        /// <param name="message">The message to dislpay is assertion failed.</param>
        /// <param name="args">An <see cref="System.Object"/> array containing zero or more objects to format.</param>
        public static void That(object expression, IConstraint constraint, object message, params object[] args)
        {
            if (!constraint.Match(expression))
            {
                IMessageWriter writer = new TextMessageWriter(message ?? "");
                writer.BuildMessage(constraint);
                throw new AssertionException(writer.ToString());
            }
        }

        /// <summary>
        /// Apply a constraint to an actual value, succeeding if the constraint
        /// is satisfied and throwing an assertion exception on failure.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="constraint">The constraint.</param>
        public static void That(object expression, IConstraint constraint)
        {
            That(expression, constraint, null, null);
        }

        #endregion Methods
    }
}