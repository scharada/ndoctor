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
namespace Probel.NDoctor.Plugins.MedicalRecord.View
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Controls;
    using System.Xml;

    using ICSharpCode.AvalonEdit.CodeCompletion;
    using ICSharpCode.AvalonEdit.Highlighting;

    using log4net;

    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Plugins.MedicalRecord.Editor;
    using Probel.NDoctor.Plugins.MedicalRecord.ViewModel;

    /// <summary>
    /// Interaction logic for MacroEditorView.xaml
    /// </summary>
    public partial class MacroEditorView : UserControl
    {
        #region Fields

        CompletionWindow completionWindow;

        #endregion Fields

        #region Constructors

        public MacroEditorView()
        {
            this.PreSetupAvalonEdit();

            this.InitializeComponent();
            this.DataContext = new MacroEditorViewModel();

            this.SetupAvalonEdit();
        }

        #endregion Constructors

        #region Methods

        private void PreSetupAvalonEdit()
        {
            // Load our custom highlighting definition
            IHighlightingDefinition customHighlighting;
            using (Stream stream = typeof(MacroEditorView).Assembly.GetManifestResourceStream("Probel.NDoctor.Plugins.MedicalRecord.Editor.MacroHighlighting.xshd"))
            {
                if (stream == null) { throw new InvalidOperationException("Could not Get embedded resource"); }

                using (XmlReader reader = new XmlTextReader(stream))
                {
                    customHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                        HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
            // and register it in the HighlightingManager
            HighlightingManager.Instance.RegisterHighlighting("Macro Highlighting", new string[] { ".cool" }, customHighlighting);
        }

        private void SetupAvalonEdit()
        {
            textEditor.TextArea.TextEntering += (sender, e) =>
            {
                if (e.Text.Length > 0 && completionWindow != null)
                {
                    if (!char.IsLetterOrDigit(e.Text[0]))
                    {
                        // Whenever a non-letter is typed while the completion window is open,
                        // insert the currently selected element.
                        completionWindow.CompletionList.RequestInsertion(e);
                    }
                }
                // do not set e.Handled=true - we still want to insert the character that was typed
            };
            textEditor.TextArea.TextEntered += (sender, e) =>
            {
                if (e.Text == "$")
                {
                    // open code completion after the user has pressed dot:
                    completionWindow = new CompletionWindow(textEditor.TextArea);

                    // provide AvalonEdit with the data:
                    IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
                    foreach (var keyword in CompletionData.GetCompletion()) { data.Add(keyword); }
                    completionWindow.Show();
                    completionWindow.Closed += delegate
                    {
                        completionWindow = null;
                    };
                }

            };
        }

        #endregion Methods
    }
}