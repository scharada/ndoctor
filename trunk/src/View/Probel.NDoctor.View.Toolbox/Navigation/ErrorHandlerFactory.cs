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

namespace Probel.NDoctor.View.Toolbox.Navigation
{
    using log4net;

    using Probel.Mvvm.Gui;
    using Probel.NDoctor.View.Toolbox.View;
    using Probel.NDoctor.View.Toolbox.ViewModel;

    public class ErrorHandlerFactory
    {
        #region Fields

        private static IStatusWriter StatusWriter;

        #endregion Fields

        #region Constructors

        static ErrorHandlerFactory()
        {
            ViewService.Configure(e => e.Bind<ExceptionView, ExceptionViewModel>());
        }

        public ErrorHandlerFactory()
        {
        }

        #endregion Constructors

        #region Methods

        public static void ConfigureStatusWriter(IStatusWriter statusWriter)
        {
            StatusWriter = statusWriter;
        }

        public IErrorHandler New(object caller)
        {
            return new ErrorHandler(caller, StatusWriter);
        }

        #endregion Methods
    }
}