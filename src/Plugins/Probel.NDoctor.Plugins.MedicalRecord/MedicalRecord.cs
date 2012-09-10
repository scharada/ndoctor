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
namespace Probel.NDoctor.Plugins.MedicalRecord
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    using AutoMapper;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DTO;
    using Probel.NDoctor.Domain.DTO.Objects;
    using Probel.NDoctor.Plugins.MedicalRecord.Dto;
    using Probel.NDoctor.Plugins.MedicalRecord.Helpers;
    using Probel.NDoctor.Plugins.MedicalRecord.Properties;
    using Probel.NDoctor.Plugins.MedicalRecord.View;
    using Probel.NDoctor.View.Core.Helpers;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;

    [Export(typeof(IPlugin))]
    public class MedicalRecord : StaticViewPlugin<WorkbenchView>
    {
        #region Fields

        private const string imgUri = @"\Probel.NDoctor.Plugins.MedicalRecord;component/Images\{0}.png";

        private readonly ICommand AddFolderCommand;
        private readonly ICommand AddRecordCommand;
        private readonly ICommand NavigateWorkbenchCommand;
        private readonly ViewService ViewService = new ViewService();

        //DOTO: replace with the window manager
        private static MacroEditorView __macroEditorView; //Dont use the variable, use the property instead

        private RibbonToggleButtonData boldButton;
        private RibbonButtonData bulletsButton;
        private RibbonToggleButtonData centerAllignButton;
        private RibbonComboBoxData fontsComboBox;
        private RibbonComboBoxData fontsSizeComboBox;
        private RibbonToggleButtonData italicButton;
        private RibbonToggleButtonData justifyAllignButton;
        private RibbonToggleButtonData leftAllignButton;
        private RibbonButtonData numberingButton;
        private RibbonToggleButtonData rightAllignButton;
        private RibbonToggleButtonData underlineButton;

        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public MedicalRecord([Import("version")] Version version)
            : base(version)
        {
            this.Validator = new PluginValidator("3.0.0.0", ValidationMode.Minimum);

            this.InitialiseFontButtons();
            this.InitialiseParagraphButtons();
            this.ConfigureAutoMapper();

            this.NavigateWorkbenchCommand = new RelayCommand(() => this.NavigateWorkbench(), () => this.CanNavigateWorkbench());
            this.AddRecordCommand = new RelayCommand(() => InnerWindow.Show(Messages.Title_AddRecord, new AddRecordView()), () => PluginContext.DoorKeeper.IsUserGranted(To.Write));
            this.AddFolderCommand = new RelayCommand(() => InnerWindow.Show(Messages.Title_AddFolder, new AddFolderView()), () => PluginContext.DoorKeeper.IsUserGranted(To.Write));
        }

        #endregion Constructors

        #region Properties

        public static double[] FontSizes
        {
            get
            {
                return new double[] {
                    3.0, 4.0, 5.0, 6.0, 6.5, 7.0, 7.5, 8.0, 8.5, 9.0, 9.5,
                    10.0, 10.5, 11.0, 11.5, 12.0, 12.5, 13.0, 13.5, 14.0, 15.0,
                    16.0, 17.0, 18.0, 19.0, 20.0, 22.0, 24.0, 26.0, 28.0, 30.0,
                    32.0, 34.0, 36.0, 38.0, 40.0, 44.0, 48.0, 52.0, 56.0, 60.0, 64.0, 68.0, 72.0, 76.0,
                    80.0, 88.0, 96.0, 104.0, 112.0, 120.0, 128.0, 136.0, 144.0
                    };
            }
        }

        private static MacroEditorView MacroEditorView
        {
            get
            {
                if (__macroEditorView == null) __macroEditorView = new MacroEditorView();
                return __macroEditorView;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initialises this plugin. Basicaly it should configure the menus into the PluginHost
        /// Every task that could throw exception should be in this method and not in the ctor.
        /// </summary>
        public override void Initialise()
        {
            Assert.IsNotNull(PluginContext.Host, "To initialise the plugin, IPluginHost should be set.");

            PluginContext.Host.Invoke(() =>
            {
                this.BuildButtons();
                this.BuildContextMenu();
                new SettingsConfigurator().Add(Messages.Title_MedicalRecord, () => new SettingsView());
            });

            TextEditor.Control.SelectionChanged += (sender, e) => this.UpdateToggleButtonState();
        }

        private void BuildButtons()
        {
            var navigateButton = new RibbonButtonData(Messages.Title_MedicalRecord
                , imgUri.FormatWith("MedicalRecord")
                , this.NavigateWorkbenchCommand) { Order = 2 };

            PluginContext.Host.AddInHome(navigateButton, Groups.Managers);
        }

        private void BuildContextMenu()
        {
            var tab = new RibbonTabData()
            {
                Header = Messages.Menu_File,
                ContextualTabGroupHeader = Messages.Title_MedicalRecord
            };

            this.ConfigureSaveMenu(tab);
            this.ConfigureFontGroup(tab);
            this.ConfigureParagraphGroup(tab);
            PluginContext.Host.AddTab(tab);

            this.contextualMenu = new RibbonContextualTabGroupData(Messages.Title_MedicalRecord, tab)
            {
                Background = Brushes.OrangeRed,
                IsVisible = false,
            };

            PluginContext.Host.AddContextualMenu(this.contextualMenu);
        }

        private bool CanNavigateWorkbench()
        {
            return PluginContext.Host.SelectedPatient != null;
        }

        private void ConfigureAutoMapper()
        {
            Mapper.CreateMap<MedicalRecordCabinetDto, TitledMedicalRecordCabinetDto>();
            Mapper.CreateMap<MedicalRecordDto, TitledMedicalRecordDto>();
            Mapper.CreateMap<MedicalRecordFolderDto, TitledMedicalRecordFolderDto>();
        }

        private void ConfigureFontGroup(RibbonTabData tab)
        {
            var fontGroup = new RibbonGroupData()
            {
                Header = Messages.Menu_Font,
                Order = 50,
            };
            fontGroup.ButtonDataCollection.Add(this.boldButton);
            fontGroup.ButtonDataCollection.Add(this.italicButton);
            fontGroup.ButtonDataCollection.Add(this.underlineButton);
            fontGroup.ButtonDataCollection.Add(this.bulletsButton);
            fontGroup.ButtonDataCollection.Add(this.numberingButton);
            //fontGroup.ButtonDataCollection.Add(this.fontsComboBox);
            //fontGroup.ButtonDataCollection.Add(this.fontsSizeComboBox);
            tab.GroupDataCollection.Add(fontGroup);
        }

        private void ConfigureParagraphGroup(RibbonTabData tab)
        {
            var paragraphGroup = new RibbonGroupData()
            {
                Header = Messages.Menu_Paragraph,
                Order = 51,
            };
            paragraphGroup.ButtonDataCollection.Add(this.centerAllignButton);
            paragraphGroup.ButtonDataCollection.Add(this.justifyAllignButton);
            paragraphGroup.ButtonDataCollection.Add(this.leftAllignButton);
            paragraphGroup.ButtonDataCollection.Add(this.rightAllignButton);

            tab.GroupDataCollection.Add(paragraphGroup);
        }

        private void ConfigureSaveMenu(RibbonTabData tab)
        {
            var revisionsButton = new RibbonButtonData(Messages.Btn_Revisions, imgUri.FormatWith("Undo"), this.ViewService.GetViewModel(this.View).ShowRevisionsCommand);
            var saveButton = new RibbonButtonData(Messages.Title_Save, imgUri.FormatWith("Save"), this.ViewService.GetViewModel(this.View).SaveCommand);
            var splitButton = this.ConfigureSplitButton();
            var macroButton = new RibbonButtonData(Messages.Title_Macro, imgUri.FormatWith("Edit"), new RelayCommand(
                () => this.EditMacro(), () => PluginContext.DoorKeeper.IsUserGranted(To.Administer)));

            var cgroup = new RibbonGroupData(Messages.Menu_Actions, 1);
            cgroup.ButtonDataCollection.Add(saveButton);
            cgroup.ButtonDataCollection.Add(revisionsButton);
            cgroup.ButtonDataCollection.Add(splitButton);
            cgroup.ButtonDataCollection.Add(macroButton);
            tab.GroupDataCollection.Add(cgroup);
        }

        private RibbonMenuButtonData ConfigureSplitButton()
        {
            var splitButton = new RibbonMenuButtonData(Messages.Title_BtnAdd, imgUri.FormatWith("Add"), AddRecordCommand);
            var addRecordButton = new RibbonMenuItemData(Messages.Title_AddRecord, imgUri.FormatWith("Add"), AddRecordCommand);
            var addFolderButton = new RibbonMenuItemData(Messages.Title_AddFolder, imgUri.FormatWith("Add"), AddFolderCommand);

            splitButton.ControlDataCollection.Add(addRecordButton);
            splitButton.ControlDataCollection.Add(addFolderButton);
            return splitButton;
        }

        private void EditMacro()
        {
            try
            {
                this.ViewService.GetViewModel(MacroEditorView).RefreshCommand.TryExecute();
                InnerWindow.Show(Messages.Title_Macro, MacroEditorView);
            }
            catch (Exception ex) { this.HandleError(ex); }
        }

        private void InitialiseFontButtons()
        {
            this.leftAllignButton = new RibbonToggleButtonData()
            {
                Label = Messages.Edition_LeftAlign,
                SmallImage = new Uri(imgUri.FormatWith("LeftAlign"), UriKind.Relative),
                Command = EditingCommands.AlignLeft,
                Order = 1,
            };
            this.rightAllignButton = new RibbonToggleButtonData()
            {
                Label = Messages.Edition_RightAlign,
                SmallImage = new Uri(imgUri.FormatWith("RightAlign"), UriKind.Relative),
                Command = EditingCommands.AlignRight,
                Order = 2,
            };
            this.centerAllignButton = new RibbonToggleButtonData()
            {
                Label = Messages.Edition_CenterAlign,
                SmallImage = new Uri(imgUri.FormatWith("CenterAlign"), UriKind.Relative),
                Command = EditingCommands.AlignCenter,
                Order = 3,
            };
            this.justifyAllignButton = new RibbonToggleButtonData()
            {
                Label = Messages.Edition_JustifyAlign,
                SmallImage = new Uri(imgUri.FormatWith("JustifyAlign"), UriKind.Relative),
                Command = EditingCommands.AlignJustify,
                Order = 4,
            };

            this.fontsComboBox = new RibbonComboBoxData()
            {
                Label = "Fonts",
                SelectionBoxWidth = 150,
                Order = 5,
                DataContext = Fonts.SystemFontFamilies,
            };

            this.fontsSizeComboBox = new RibbonComboBoxData()
            {
                Label = "Font size",
                SelectionBoxWidth = 40,
                Order = 6,
            };
        }

        private void InitialiseParagraphButtons()
        {
            this.boldButton = new RibbonToggleButtonData()
            {
                Label = Messages.Edition_Bold,
                SmallImage = new Uri(imgUri.FormatWith("Bold"), UriKind.Relative),
                Command = EditingCommands.ToggleBold,
            };
            this.italicButton = new RibbonToggleButtonData()
            {
                Label = Messages.Edition_Italic,
                SmallImage = new Uri(imgUri.FormatWith("Italic"), UriKind.Relative),
                Command = EditingCommands.ToggleItalic,
            };
            this.underlineButton = new RibbonToggleButtonData()
            {
                Label = Messages.Edition_Underline,
                SmallImage = new Uri(imgUri.FormatWith("Underline"), UriKind.Relative),
                Command = EditingCommands.ToggleUnderline,
            };
            this.bulletsButton = new RibbonButtonData()
            {
                Label = Messages.Edition_Bullets,
                SmallImage = new Uri(imgUri.FormatWith("Bullets"), UriKind.Relative),
                Command = EditingCommands.ToggleBullets,
            };
            this.numberingButton = new RibbonButtonData()
            {
                Label = Messages.Edition_Numbering,
                SmallImage = new Uri(imgUri.FormatWith("Numbering"), UriKind.Relative),
                Command = EditingCommands.ToggleNumbering,
            };
        }

        private void NavigateWorkbench()
        {
            try
            {
                this.ViewService.GetViewModel(this.View).RefreshCommand.TryExecute();
                PluginContext.Host.Navigate(this.View);

                this.ShowContextMenu();
            }
            catch (Exception ex)
            {
                this.HandleError(ex, Messages.Msg_FailToLoadMedicalRecords.FormatWith(ex.Message));
            }
        }

        private void UpdateItemCheckedState(RibbonToggleButtonData button, DependencyProperty formattingProperty, object expectedValue)
        {
            object currentValue = TextEditor.Control.Selection.GetPropertyValue(formattingProperty);
            button.IsChecked = (currentValue == DependencyProperty.UnsetValue) ? false : currentValue != null && currentValue.Equals(expectedValue);
        }

        private void UpdateToggleButtonState()
        {
            UpdateItemCheckedState(this.boldButton, TextElement.FontWeightProperty, FontWeights.Bold);
            UpdateItemCheckedState(this.italicButton, TextElement.FontStyleProperty, FontStyles.Italic);
            UpdateItemCheckedState(this.underlineButton, Inline.TextDecorationsProperty, TextDecorations.Underline);

            UpdateItemCheckedState(this.leftAllignButton, Paragraph.TextAlignmentProperty, TextAlignment.Left);
            UpdateItemCheckedState(this.centerAllignButton, Paragraph.TextAlignmentProperty, TextAlignment.Center);
            UpdateItemCheckedState(this.rightAllignButton, Paragraph.TextAlignmentProperty, TextAlignment.Right);
            UpdateItemCheckedState(this.justifyAllignButton, Paragraph.TextAlignmentProperty, TextAlignment.Justify);
        }

        #endregion Methods
    }
}