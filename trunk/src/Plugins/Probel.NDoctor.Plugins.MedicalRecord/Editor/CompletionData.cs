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

namespace Probel.NDoctor.Plugins.MedicalRecord.Editor
{
    using System;
    using System.Collections.Generic;

    using ICSharpCode.AvalonEdit.CodeCompletion;
    using ICSharpCode.AvalonEdit.Document;
    using ICSharpCode.AvalonEdit.Editing;

    using Probel.NDoctor.Domain.DTO.Objects;

    /// <summary>
    /// Implements AvalonEdit ICompletionData interface to provide the entries in the completion drop down.
    /// </summary>
    public class CompletionData : ICompletionData
    {
        #region Constructors

        private CompletionData(string text)
        {
            this.Text = text;
        }

        #endregion Constructors

        #region Properties

        // Use this property if you want to show a fancy UIElement in the drop down list.
        public object Content
        {
            get { return this.Text; }
        }

        public object Description
        {
            get { return ""; }
        }

        public System.Windows.Media.ImageSource Image
        {
            get { return null; }
        }

        public double Priority
        {
            get { return 0; }
        }

        public string Text
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        public static IList<ICompletionData> GetCompletion()
        {
            var completion = new List<ICompletionData>();

            foreach (var keyword in Markups.All())
            {
                completion.Add(new CompletionData(keyword));
            }
            return completion as IList<ICompletionData>;
        }

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, this.Text);
        }

        #endregion Methods
    }
}