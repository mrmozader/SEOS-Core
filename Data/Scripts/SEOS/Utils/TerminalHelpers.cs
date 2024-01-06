namespace SEOS.Control
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Sandbox.ModAPI;
    using Sandbox.ModAPI.Interfaces.Terminal;
    using VRage.ModAPI;
    using VRage.Utils;
    using VRageMath;

    public static class TerminalHelpers
    {

        /// <summary>
        /// Adds a Separator control to the terminal UI for a given block type.
        /// </summary>
        /// <typeparam name="T">The type of terminal block for which the control is being added.</typeparam>
        /// <param name="block">The terminal block instance.</param>
        /// <param name="name">The internal name of the control, used for identification.</param>
        /// <param name="enabledGetter">Optional function to determine if the control should be enabled. Defaults to null.</param>
        /// <param name="visibleGetter">Optional function to determine if the control should be visible. Defaults to null.</param>
        /// <returns>Returns the created IMyTerminalControlSeparator control.</returns>
        internal static IMyTerminalControlSeparator Separator<T>(
            T block,
            string name,
            Func<IMyTerminalBlock, bool> enabledGetter = null,
            Func<IMyTerminalBlock, bool> visibleGetter = null
        ) where T : IMyTerminalBlock
        {
            // Create a new separator control for the specified block type.
            var c = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, T>(name);
            // Get the default enabled status.
            var d = GetDefaultEnabled();
            // Set the enabled status of the separator using the provided function, or use the default if none is provided.
            c.Enabled = enabledGetter ?? d;
            // Set the visibility of the separator using the provided function, or use the default if none is provided.
            c.Visible = visibleGetter ?? d;
            // Register the newly created separator control to the terminal controls for the specific block type.
            MyAPIGateway.TerminalControls.AddControl<T>(c);
            // Return the created separator control.
            return c;
        }

        /// <summary>
        /// Adds a Label control to the terminal UI for a specific block type.
        /// </summary>
        /// <typeparam name="T">The type of terminal block for which the control is being added.</typeparam>
        /// <param name="block">The terminal block instance.</param>
        /// <param name="title">The title of the control, which will also be used for the label text displayed on the UI.</param>
        /// <param name="visibleGetter">Function to determine if the label should be visible.</param>
        /// <param name="enabledGetter">Optional function to determine if the label should be enabled. Defaults to null.</param>
        /// <returns>Returns the created IMyTerminalControlLabel control.</returns>
        internal static IMyTerminalControlLabel AddControlLabel<T>(
            T block,
            string title,
            Func<IMyTerminalBlock, bool> visibleGetter,
            Func<IMyTerminalBlock, bool> enabledGetter = null
        )
        {
            // Create a new Label control for the specified block type.
            var c = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlLabel, T>(title);
            // Get the default enabled status.
            var d = GetDefaultEnabled();
            // Set the enabled state of the label, using the provided function or default if none is provided.
            c.Enabled = enabledGetter ?? d;
            // Set the visibility of the label, using the provided function.
            c.Visible = visibleGetter ?? d;
            // Set the text for the Label control.
            c.Label = MyStringId.GetOrCompute(title);
            // Add the control to the terminal controls collection for the block type.
            MyAPIGateway.TerminalControls.AddControl<T>(c);
            // Return the created Label control.
            return c;
        }

        /// <summary>
        /// Adds a Textbox control to the terminal UI for a specific block type (T).
        /// </summary>
        /// <typeparam name="T">The type of terminal block for which the Textbox control is being added.</typeparam>
        /// <param name="block">The terminal block instance.</param>
        /// <param name="name">The name of the Textbox control, used for identification.</param>
        /// <param name="title">The title of the Textbox, displayed on the UI.</param>
        /// <param name="tooltip">Tooltip text to show when hovering over the Textbox.</param>
        /// <param name="getter">Function to get the current value of the Textbox.</param>
        /// <param name="setter">Function to set the new value of the Textbox.</param>
        /// <param name="visibleGetter">Function to determine if the Textbox should be visible. Defaults to null.</param>
        /// <param name="enabledGetter">Optional function to determine if the Textbox should be enabled. Defaults to null.</param>
        /// <returns>Returns the created IMyTerminalControlTextbox control.</returns>
        internal static IMyTerminalControlTextbox AddTextBox<T>(
            T block,
            string name,
            string title,
            string tooltip,
            Func<IMyTerminalBlock, StringBuilder> getter,
            Action<IMyTerminalBlock, StringBuilder> setter,
            Func<IMyTerminalBlock, bool> visibleGetter,
            Func<IMyTerminalBlock, bool> enabledGetter = null
        )
        {
            // Create a new Textbox control for the specific block type.
            var c = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlTextbox, T>(title);
            // Get the default enabled status if none is specified.
            var d = GetDefaultEnabled();
            // Set the enabled state, using the provided function or default if none is provided.
            c.Enabled = enabledGetter ?? d;
            // Set the visibility, using the provided function or default if none is provided.
            c.Visible = visibleGetter ?? d;
            // Set the Getter and Setter functions for the Textbox's value.
            c.Getter = getter;
            c.Setter = setter;
            // Add the Textbox control to the terminal controls for the specific block type.
            MyAPIGateway.TerminalControls.AddControl<T>(c);
            // Return the created Textbox control.
            return c;
        }

        /// <summary>
        /// Adds a checkbox control to the terminal UI for a given block type.
        /// </summary>
        /// <typeparam name="T">The type of terminal block for which the control is being added.</typeparam>
        /// <param name="block">The terminal block instance.</param>
        /// <param name="name">The name of the control, used for identification.</param>
        /// <param name="title">The title of the control, displayed on the UI.</param>
        /// <param name="tooltip">The tooltip text to show when hovering over the control.</param>
        /// <param name="getter">The function to get the current state of the checkbox.</param>
        /// <param name="setter">The function to set the new state of the checkbox.</param>
        /// <param name="enabledGetter">Optional function to determine if the control should be enabled. Defaults to null.</param>
        /// <param name="visibleGetter">Optional function to determine if the control should be visible. Defaults to null.</param>
        /// <returns>Returns the created IMyTerminalControlCheckbox control.</returns>
        internal static IMyTerminalControlCheckbox AddCheckbox<T>(
            T block,
            string name,
            string title,
            string tooltip,
            Func<IMyTerminalBlock, bool> getter,
            Action<IMyTerminalBlock, bool> setter,
            Func<IMyTerminalBlock, bool> enabledGetter = null,
            Func<IMyTerminalBlock, bool> visibleGetter = null
        ) where T : IMyTerminalBlock
        {
            // Create a new checkbox control.
            var c = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlCheckbox, T>(name);
            // Get the default enabled condition.
            var d = GetDefaultEnabled();
            // Set properties for the control.
            c.Title = MyStringId.GetOrCompute(title);
            c.Tooltip = MyStringId.GetOrCompute(tooltip);
            c.Getter = getter;
            c.Setter = setter;
            // Set the visibility and enabled state, using the provided functions or default if not specified.
            c.Visible = visibleGetter ?? d;
            c.Enabled = enabledGetter ?? d;
            // Add the control to the terminal controls collection for the block type.
            MyAPIGateway.TerminalControls.AddControl<T>(c);
            return c;
        }

        /// <summary>
        /// Adds an On-Off Switch control to the terminal UI for a specific block type.
        /// </summary>
        /// <typeparam name="T">The type of terminal block for which the control is being added.</typeparam>
        /// <param name="block">The terminal block instance.</param>
        /// <param name="name">The internal name of the control, used for identification.</param>
        /// <param name="title">The title of the control, displayed on the UI.</param>
        /// <param name="tooltip">The tooltip text to show when hovering over the control.</param>
        /// <param name="onText">The text to display when the switch is in the 'On' state.</param>
        /// <param name="offText">The text to display when the switch is in the 'Off' state.</param>
        /// <param name="getter">The function to get the current state of the switch.</param>
        /// <param name="setter">The function to set the new state of the switch.</param>
        /// <param name="enabledGetter">Optional function to determine if the control should be enabled. Defaults to null.</param>
        /// <param name="visibleGetter">Optional function to determine if the control should be visible. Defaults to null.</param>
        /// <returns>Returns the created IMyTerminalControlOnOffSwitch control.</returns>
        internal static IMyTerminalControlOnOffSwitch AddOnOff<T>(
            T block,
            string name,
            string title,
            string tooltip,
            string onText,
            string offText,
            Func<IMyTerminalBlock, bool> getter,
            Action<IMyTerminalBlock, bool> setter,
            Func<IMyTerminalBlock, bool> enabledGetter = null,
            Func<IMyTerminalBlock, bool> visibleGetter = null
        ) where T : IMyTerminalBlock
        {
            // Create a new On-Off Switch control for the specified block type.
            var c = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlOnOffSwitch, T>(name);
            // Get the default enabled condition.
            var d = GetDefaultEnabled();
            // Set properties for the control.
            c.Title = MyStringId.GetOrCompute(title);
            c.Tooltip = MyStringId.GetOrCompute(tooltip);
            c.OnText = MyStringId.GetOrCompute(onText);
            c.OffText = MyStringId.GetOrCompute(offText);
            // Assign the getter and setter functions.
            c.Getter = getter;
            c.Setter = setter;
            // Set the visibility and enabled state, using the provided functions or default if not specified.
            c.Enabled = enabledGetter ?? d;
            c.Visible = visibleGetter ?? d;
            // Add the control to the terminal controls collection for the block type.
            MyAPIGateway.TerminalControls.AddControl<T>(c);
            // Return the created On-Off Switch control.
            return c;
        }

        /// <summary>
        /// Adds a Button control to the terminal UI for a specific block type.
        /// </summary>
        /// <typeparam name="T">The type of terminal block for which the control is being added.</typeparam>
        /// <param name="block">The terminal block instance.</param>
        /// <param name="name">The internal name of the control, used for identification.</param>
        /// <param name="title">The title of the control, displayed on the UI.</param>
        /// <param name="tooltip">The tooltip text to show when hovering over the control.</param>
        /// <param name="visibleGetter">Optional function to determine if the control should be visible. Defaults to null.</param>
        /// <returns>Returns the created IMyTerminalControlButton control.</returns>
        internal static IMyTerminalControlButton AddButton<T>(
            T block,
            string name,
            string title,
            string tooltip,
            Func<IMyTerminalBlock, bool> visibleGetter = null
        ) where T : IMyTerminalBlock
        {
            // Create a new button control for the specified block type.
            var c = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, T>(name);
            // Get the default enabled condition.
            var d = GetDefaultEnabled();
            // Set properties for the control.
            c.Title = MyStringId.GetOrCompute(title);
            c.Tooltip = MyStringId.GetOrCompute(tooltip);
            // Set the visibility, using the provided function or default if not specified.
            c.Visible = visibleGetter ?? d;
            // Add the control to the terminal controls collection for the block type.
            MyAPIGateway.TerminalControls.AddControl<T>(c);
            // Return the created button control.
            return c;
        }

        /// <summary>
        /// Adds a Slider control to the terminal UI for a given block type.
        /// </summary>
        /// <typeparam name="T">The type of terminal block for which the control is being added.</typeparam>
        /// <param name="block">The terminal block instance.</param>
        /// <param name="name">The internal name of the control, used for identification.</param>
        /// <param name="title">The title of the control, displayed on the UI.</param>
        /// <param name="tooltip">The tooltip text to show when hovering over the control.</param>
        /// <param name="getter">The function to get the current state of the slider.</param>
        /// <param name="setter">The function to set the new state of the slider.</param>
        /// <param name="enabledGetter">Optional function to determine if the control should be enabled. Defaults to null.</param>
        /// <param name="visibleGetter">Optional function to determine if the control should be visible. Defaults to null.</param>
        /// <returns>Returns the created IMyTerminalControlSlider control.</returns>
        internal static IMyTerminalControlSlider AddSlider<T>(
            T block,
            string name,
            string title,
            string tooltip,
            Func<IMyTerminalBlock, float> getter,
            Action<IMyTerminalBlock, float> setter,
            Func<IMyTerminalBlock, bool> enabledGetter = null,
            Func<IMyTerminalBlock, bool> visibleGetter = null
        ) where T : IMyTerminalBlock
        {
            // Create a new slider control for the specified block type.
            var s = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSlider, T>(name);
            // Get the default enabled status.
            var d = GetDefaultEnabled();
            // Set the title and tooltip for the slider.
            s.Title = MyStringId.GetOrCompute(title);
            s.Tooltip = MyStringId.GetOrCompute(tooltip);
            // Set the enabled status of the slider using the provided function, or use the default if none is provided.
            s.Enabled = enabledGetter ?? d;
            // Set the visibility of the slider using the provided function, or use the default if none is provided.
            s.Visible = visibleGetter ?? d;
            // Assign the getter and setter functions for the slider value.
            s.Getter = getter;
            s.Setter = setter;
            // Assign the writer function to display the slider value (formatted to two decimal places).
            s.Writer = (b, v) => v.Append(getter(b).ToString("N2"));
            // Register the newly created slider control to the terminal controls for the specific block type.
            MyAPIGateway.TerminalControls.AddControl<T>(s);
            // Return the created slider control.
            return s;
        }

        internal static IMyTerminalControlSlider AddDynamicSlider<T>(
            T block,
            string name,
            string title,
            string tooltip,
            Func<IMyTerminalBlock, float> getter,
            Action<IMyTerminalBlock, float> setter,
            Func<IMyTerminalBlock, float> minGetter,
            Func<IMyTerminalBlock, float> maxGetter,
            Func<IMyTerminalBlock, bool> enabledGetter = null,
            Func<IMyTerminalBlock, bool> visibleGetter = null,
            string writerFormat = "0.##"
        ) where T : IMyTerminalBlock
        {
            var d = GetDefaultEnabled();  // Default enabled state

            // Create a new slider control.
            var slider = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSlider, T>(name);
            slider.Title = MyStringId.GetOrCompute(title);
            slider.Tooltip = MyStringId.GetOrCompute(tooltip);
            slider.Getter = getter;
            slider.Setter = setter;
            slider.Enabled = enabledGetter ?? d;
            slider.Visible = visibleGetter ?? d;

            // Dynamic min and max values
            slider.SetLimits((b) => minGetter(b), (b) => maxGetter(b));

            // Optional value writer
            if (!string.IsNullOrEmpty(writerFormat))
            {
                slider.Writer = (b, sb) => sb.Append(getter(b).ToString(writerFormat));
            }

            // Add the control to the terminal controls collection for the block type
            MyAPIGateway.TerminalControls.AddControl<T>(slider);
            return slider;
        }



        /// <summary>
        /// Creates a ComboBox control in the terminal UI for a specific block type.
        /// </summary>
        /// <typeparam name="T">The type of terminal block for which the control is being added.</typeparam>
        /// <param name="block">The terminal block instance.</param>
        /// <param name="name">The internal name for the ComboBox control.</param>
        /// <param name="title">The display title for the ComboBox control.</param>
        /// <param name="tooltip">Tooltip text to display when hovering over the ComboBox control.</param>
        /// <param name="getter">Function to get the current selected value.</param>
        /// <param name="setter">Function to set the new selected value.</param>
        /// <param name="fillAction">Function to populate the ComboBox items.</param>
        /// <param name="enabledGetter">Optional function to determine if the ComboBox should be enabled. Defaults to null.</param>
        /// <param name="visibleGetter">Optional function to determine if the ComboBox should be visible. Defaults to null.</param>
        /// <returns>Returns the created IMyTerminalControlCombobox control.</returns>
        internal static IMyTerminalControlCombobox AddCombobox<T>(
            T block,
            string name,
            string title,
            string tooltip,
            Func<IMyTerminalBlock, long> getter,
            Action<IMyTerminalBlock, long> setter,
            Action<List<MyTerminalControlComboBoxItem>> fillAction,
            Func<IMyTerminalBlock, bool> enabledGetter = null,
            Func<IMyTerminalBlock, bool> visibleGetter = null
        ) where T : IMyTerminalBlock
        {
            // Create a new ComboBox control for the specified block type.
            var cmb = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlCombobox, T>(name);
            // Get the default enabled condition.
            var d = GetDefaultEnabled();
            // Set properties for the control.
            cmb.Title = MyStringId.GetOrCompute(title);
            cmb.Tooltip = MyStringId.GetOrCompute(tooltip);
            // Set the visibility and enabled state, using the provided functions or default if not specified.
            cmb.Enabled = enabledGetter ?? d;
            cmb.Visible = visibleGetter ?? d;
            // Set the list content and item selected actions.
            cmb.ComboBoxContent = fillAction;
            cmb.Getter = getter;
            cmb.Setter = setter;
            // Add the control to the terminal controls collection for the block type.
            MyAPIGateway.TerminalControls.AddControl<T>(cmb);
            return cmb;
        }

        /// <summary>
        /// Adds a listbox control to the terminal UI for a given block type.
        /// </summary>
        /// <typeparam name="T">The type of terminal block for which the control is being added.</typeparam>
        /// <param name="block">The terminal block instance.</param>
        /// <param name="name">The name of the control, used for identification.</param>
        /// <param name="title">The title of the control, displayed on the UI.</param>
        /// <param name="tooltip">The tooltip text to show when hovering over the control.</param>
        /// <param name="itemsSelected">The action to execute when items are selected in the listbox.</param>
        /// <param name="listContent">The action to populate the listbox with items.</param>
        /// <param name="enabledGetter">Optional function to determine if the control should be enabled. Defaults to null.</param>
        /// <param name="visibleGetter">Optional function to determine if the control should be visible. Defaults to null.</param>
        /// <returns>Returns the created IMyTerminalControlListbox control.</returns>
        internal static IMyTerminalControlListbox AddListBox<T>(
            T block,
            string name,
            string title,
            string tooltip,
            Action<IMyTerminalBlock, List<MyTerminalControlListBoxItem>> itemsSelected,
            Action<IMyTerminalBlock, List<MyTerminalControlListBoxItem>, List<MyTerminalControlListBoxItem>> listContent,
            Func<IMyTerminalBlock, bool> enabledGetter = null,
            Func<IMyTerminalBlock, bool> visibleGetter = null
        ) where T : IMyTerminalBlock
        {
            // Create a new listbox control.
            var listBox = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlListbox, T>(name);
            // Get the default enabled condition.
            var d = GetDefaultEnabled();
            // Set properties for the control.
            listBox.Title = MyStringId.GetOrCompute(title);
            listBox.Tooltip = MyStringId.GetOrCompute(tooltip);
            // Set the visibility and enabled state, using the provided functions or default if not specified.
            listBox.Enabled = enabledGetter ?? d;
            listBox.Visible = visibleGetter ?? d;
            // Set the list content and item selected actions.
            listBox.ListContent = listContent;
            listBox.ItemSelected = itemsSelected;
            // Add the control to the terminal controls collection for the block type.
            MyAPIGateway.TerminalControls.AddControl<T>(listBox);
            return listBox;
        }


        /// <summary>
        /// Returns a default function that determines whether a terminal control should be enabled.
        /// This is often used as a fallback function when no specific enable condition is provided.
        /// </summary>
        /// <returns>A Func delegate that takes an IMyTerminalBlock and returns a bool.</returns>
        private static Func<IMyTerminalBlock, bool> GetDefaultEnabled()
        {
            // Define and return the function here.
            // For example, enabling the control only if the block's SubtypeId starts with "Variable".
            return b => b.BlockDefinition.SubtypeId.StartsWith("Variable");
        }

    }

    public static class AdvancedTerminalHelpers
    {
        /// <summary>
        /// Adds a Button control to the terminal UI for a given block type.
        /// </summary>
        /// <typeparam name="T">The type of terminal block for which the control is being added.</typeparam>
        /// <param name="name">The internal name of the control, used for identification.</param>
        /// <param name="title">The title of the control, displayed on the UI.</param>
        /// <param name="tooltip">The tooltip text to show when hovering over the control.</param>
        /// <param name="visibleGetter">Function to determine if the control should be visible.</param>
        /// <param name="myAction">The action to perform when the button is clicked.</param>
        /// <returns>Returns the created IMyTerminalControlButton control.</returns>
        internal static IMyTerminalControlButton CreateButton<T>(
            string name,
            string title,
            string tooltip,
            Func<IMyTerminalBlock, bool> visibleGetter,
            Action<IMyTerminalBlock> myAction
        ) where T : IMyTerminalBlock
        {
            // Create a new button control for the specified block type.
            var c = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, T>(name);
            // Set the title and tooltip for the button.
            c.Title = MyStringId.GetOrCompute(title);
            c.Tooltip = MyStringId.GetOrCompute(tooltip);
            // Set the visibility of the button using the provided function.
            c.Visible = visibleGetter;
            // Set the action to be performed when the button is clicked.
            c.Action = (block) => myAction(block);
            // Register the newly created button control to the terminal controls for the specific block type.
            MyAPIGateway.TerminalControls.AddControl<T>(c);
            // Return the created button control.
            return c;
        }
    }

    public static class TerminalHelperTemplates
    {
        /// <summary>
        /// Returns a default function that determines whether a terminal control should be enabled.
        /// This is often used as a fallback function when no specific enable condition is provided.
        /// </summary>
        /// <returns>A Func delegate that takes an IMyTerminalBlock and returns a bool.</returns>
        private static Func<IMyTerminalBlock, bool> GetDefaultEnabled()
        {
            // Define and return the function here.
            // For example, enabling the control only if the block's SubtypeId starts with "Variable".
            return b => b.BlockDefinition.SubtypeId.StartsWith("Variable");
        }

        /// <summary>
        /// Adds a Color Editor control to the terminal UI for a given block type.
        /// </summary>
        /// <typeparam name="T">The type of terminal block for which the control is being added.</typeparam>
        /// <param name="block">The terminal block instance.</param>
        /// <param name="name">The internal name of the control, used for identification.</param>
        /// <param name="title">The title of the control, displayed on the UI.</param>
        /// <param name="tooltip">The tooltip text to show when hovering over the control.</param>
        /// <param name="getter">The function to get the current color value.</param>
        /// <param name="setter">The function to set the new color value.</param>
        /// <param name="enabledGetter">Optional function to determine if the control should be enabled. Defaults to null.</param>
        /// <param name="visibleGetter">Optional function to determine if the control should be visible. Defaults to null.</param>
        /// <returns>Returns the created IMyTerminalControlColor control.</returns>
        internal static IMyTerminalControlColor AddColorEditor<T>(
            T block,
            string name,
            string title,
            string tooltip,
            Func<IMyTerminalBlock, Color> getter,
            Action<IMyTerminalBlock, Color> setter,
            Func<IMyTerminalBlock, bool> enabledGetter = null,
            Func<IMyTerminalBlock, bool> visibleGetter = null
        ) where T : IMyTerminalBlock
        {
            // Create a new color editor control for the specified block type.
            var c = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlColor, T>(name);
            // Get the default enabled status.
            var d = GetDefaultEnabled();
            // Set the title and tooltip for the color editor.
            c.Title = MyStringId.GetOrCompute(title);
            c.Tooltip = MyStringId.GetOrCompute(tooltip);
            // Set the enabled status of the color editor using the provided function, or use the default if none is provided.
            c.Enabled = enabledGetter ?? d;
            // Set the visibility of the color editor using the provided function, or use the default if none is provided.
            c.Visible = visibleGetter ?? d;
            // Assign the getter and setter functions for the color value.
            c.Getter = getter;
            c.Setter = setter;
            // Register the newly created color editor control to the terminal controls for the specific block type.
            MyAPIGateway.TerminalControls.AddControl<T>(c);
            // Return the created color editor control.
            return c;
        }

        internal static IMyTerminalControl[] ConfigManagerCreateTerminalMainPage<T>(
            T block,
            string namePrefix,
            Func<IMyTerminalBlock, StringBuilder> selectionHelperGetter,
            Action<IMyTerminalBlock, StringBuilder> selectionHelperSetter,
            Action<IMyTerminalBlock, List<MyTerminalControlListBoxItem>> configSelected,
            Action<IMyTerminalBlock, List<MyTerminalControlListBoxItem>, List<MyTerminalControlListBoxItem>> configContent,
            Func<IMyTerminalBlock, StringBuilder> searchTextGetter,
            Action<IMyTerminalBlock, StringBuilder> searchTextSetter,
            Action<IMyTerminalBlock> searchAction,
            Action<IMyTerminalBlock> editConfigAction,
            Action<IMyTerminalBlock> createConfigAction
        ) where T : IMyTerminalBlock
        {
            var controls = new IMyTerminalControl[7];
            var defaultEnabled = GetDefaultEnabled();  // Assume this function returns the default enabled state for your controls

            // 1. Title Label
            var titleLabelMainPage = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlLabel, T>(namePrefix + "_TitleLabel");
            titleLabelMainPage.Label = MyStringId.GetOrCompute("SEOS Terminal: Main Page");
            titleLabelMainPage.Enabled = defaultEnabled;
            titleLabelMainPage.Visible = defaultEnabled;
            MyAPIGateway.TerminalControls.AddControl<T>(titleLabelMainPage);
            controls[0] = titleLabelMainPage;

            // 2. Selection Helper TextBox
            var selectionHelperTextBoxMainPage = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlTextbox, T>(namePrefix + "_SelectionHelperTextBox");
            selectionHelperTextBoxMainPage.Title = MyStringId.GetOrCompute("Selection Helper");
            selectionHelperTextBoxMainPage.Tooltip = MyStringId.GetOrCompute("Helper text for the current selection");
            selectionHelperTextBoxMainPage.Enabled = defaultEnabled;
            selectionHelperTextBoxMainPage.Visible = defaultEnabled;
            selectionHelperTextBoxMainPage.Getter = selectionHelperGetter;
            selectionHelperTextBoxMainPage.Setter = selectionHelperSetter;
            MyAPIGateway.TerminalControls.AddControl<T>(selectionHelperTextBoxMainPage);
            controls[1] = selectionHelperTextBoxMainPage;

            // 3. ListBox for Known Configs
            var configListboxMainPage = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlListbox, T>(namePrefix + "_ConfigListBox");
            configListboxMainPage.Title = MyStringId.GetOrCompute("Known Configs");
            configListboxMainPage.Tooltip = MyStringId.GetOrCompute("");
            configListboxMainPage.VisibleRowsCount = 10;
            configListboxMainPage.Multiselect = false;  // Only one config can be selected at a time
            configListboxMainPage.Enabled = defaultEnabled;
            configListboxMainPage.Visible = defaultEnabled;
            configListboxMainPage.ListContent = configContent;
            configListboxMainPage.ItemSelected = configSelected;
            MyAPIGateway.TerminalControls.AddControl<T>(configListboxMainPage);
            controls[2] = configListboxMainPage;

            // 4. Search TextBox
            var searchTextBoxMainPage = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlTextbox, T>(namePrefix + "_SearchTextBox");
            searchTextBoxMainPage.Title = MyStringId.GetOrCompute("Search");
            searchTextBoxMainPage.Tooltip = MyStringId.GetOrCompute("Search for a specific config");
            searchTextBoxMainPage.Enabled = defaultEnabled;
            searchTextBoxMainPage.Visible = defaultEnabled;
            searchTextBoxMainPage.Getter = searchTextGetter;
            searchTextBoxMainPage.Setter = searchTextSetter;
            MyAPIGateway.TerminalControls.AddControl<T>(searchTextBoxMainPage);
            controls[3] = searchTextBoxMainPage;

            // 5. Search Button
            var searchButtonMainPage = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, T>(namePrefix + "_SearchButton");
            searchButtonMainPage.Title = MyStringId.GetOrCompute("Search");
            searchButtonMainPage.Tooltip = MyStringId.GetOrCompute("Perform search");
            searchButtonMainPage.Action = searchAction;
            MyAPIGateway.TerminalControls.AddControl<T>(searchButtonMainPage);
            controls[4] = searchButtonMainPage;

            // 6. Edit Config Button
            var editButtonMainPage = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, T>(namePrefix + "_EditButton");
            editButtonMainPage.Title = MyStringId.GetOrCompute("Edit Config");
            editButtonMainPage.Tooltip = MyStringId.GetOrCompute("Edit the selected config");
            editButtonMainPage.Action = editConfigAction;
            MyAPIGateway.TerminalControls.AddControl<T>(editButtonMainPage);
            controls[5] = editButtonMainPage;

            // 6. Edit Config Button
            var createNewButtonMainPage = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, T>(namePrefix + "_CreateNewButton");
            createNewButtonMainPage.Title = MyStringId.GetOrCompute("New Config");
            createNewButtonMainPage.Tooltip = MyStringId.GetOrCompute("Create New Config");
            createNewButtonMainPage.Action = createConfigAction;
            MyAPIGateway.TerminalControls.AddControl<T>(createNewButtonMainPage);
            controls[6] = createNewButtonMainPage;
            // Add any separators or additional layout controls you may need
            // ...

            return controls;
        }



        /// <summary>
        /// The ConfigManagerCreateTemplatePage<T> method serves as a cornerstone function within the SEOS Config Manager,
        /// orchestrating the creation and configuration of a wide array of terminal controls specifically for the Template Creation Page.
        /// As a generic method parameterized on the terminal block type T, it offers flexibility, allowing it to work with various types of blocks.
        /// The method takes in an extensive list of parameters, each serving a particular role in shaping the user interface.
        /// These parameters include various Func and Action delegates for getter and setter operations, list population, and event handling for controls
        /// like labels, textboxes, listboxes, and buttons. All these controls are stored in an array, ensuring a structured way to manage them.
        /// The method also leverages a default enablement state, defaultEnabled, to uniformly apply visibility and enablement settings across all controls.
        /// At the end of its execution, the method returns this array of initialized controls, making it a one-stop function for setting up the entire UI
        /// for template creation in the Config Manager.
        /// </summary>
        /// <typeparam name="T">Generic type parameter that represents the terminal block.</typeparam>
        /// <param name="block">The terminal block object for which the controls are created.</param>
        /// <param name="namePrefix">Prefix used for control IDs to ensure uniqueness.</param>
        /// <param name="title">Title appearing on the control UI.</param>
        /// <param name="typeHelperGetter">Function to get the text for the type helper textbox.</param>
        /// <param name="typeHelperSetter">Function to set the text for the type helper textbox.</param>
        /// <param name="typeSelected">Action to execute when a type is selected in the type listbox.</param>
        /// <param name="typeContent">Function to populate the type listbox.</param>
        /// <param name="textGetter">Function to get the search text.</param>
        /// <param name="textSetter">Function to set the search text.</param>
        /// <param name="searchConfigAction">Action to perform when the search button is clicked.</param>
        /// <param name="subTypeSelected">Action to execute when a subtype is selected.</param>
        /// <param name="subTypeContent">Function to populate the subtype listbox.</param>
        /// <param name="subtypeHelperGetter">Function to get the text for the subtype helper textbox.</param>
        /// <param name="subTypeHelperSetter">Function to set the text for the subtype helper textbox.</param>
        /// <param name="createNewTemplateAction">Action to create a new template.</param>
        /// <param name="backToConfigManagerMainAction">Action to navigate back to the main Config Manager page.</param>
        /// <returns>An array of initialized terminal controls.</returns>
        internal static IMyTerminalControl[] ConfigManagerCreateTemplatePage<T>(
            T block,
            string namePrefix,
            string title,
            Func<IMyTerminalBlock, StringBuilder> typeHelperGetter,
            Action<IMyTerminalBlock, StringBuilder> typeHelperSetter,
            Action<IMyTerminalBlock, List<MyTerminalControlListBoxItem>> typeSelected,
            Action<IMyTerminalBlock, List<MyTerminalControlListBoxItem>, List<MyTerminalControlListBoxItem>> typeContent,
            Func<IMyTerminalBlock, StringBuilder> textGetter,
            Action<IMyTerminalBlock, StringBuilder> textSetter,
            Action<IMyTerminalBlock> searchConfigAction,
            Action<IMyTerminalBlock, List<MyTerminalControlListBoxItem>> subTypeSelected,
            Action<IMyTerminalBlock, List<MyTerminalControlListBoxItem>, List<MyTerminalControlListBoxItem>> subTypeContent,
            Func<IMyTerminalBlock, StringBuilder> subtypeHelperGetter,
            Action<IMyTerminalBlock, StringBuilder> subTypeHelperSetter,
            Action<IMyTerminalBlock> createNewTemplateAction,
            Action<IMyTerminalBlock> backToConfigManagerMainAction
        ) where T : IMyTerminalBlock
        {
            var controls = new IMyTerminalControl[17];
            var defaultEnabled = GetDefaultEnabled();  

            var titleLabel = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlLabel, T>(namePrefix + "_TitleLabel");
            titleLabel.Label = MyStringId.GetOrCompute("Config Manager: Template Creation");
            titleLabel.Enabled = defaultEnabled;
            titleLabel.Visible = defaultEnabled;
            MyAPIGateway.TerminalControls.AddControl<T>(titleLabel);
            controls[0] = titleLabel;

            var upperPageSeparator = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, T>(namePrefix + "_UpperPageSeparator");
            MyAPIGateway.TerminalControls.AddControl<T>(upperPageSeparator);
            controls[1] = upperPageSeparator;

            var TypeLabel = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlLabel, T>(namePrefix + "_TitleLabel");
            TypeLabel.Label = MyStringId.GetOrCompute("CubeBlock Type");
            TypeLabel.Enabled = defaultEnabled;
            TypeLabel.Visible = defaultEnabled;
            MyAPIGateway.TerminalControls.AddControl<T>(TypeLabel);
            controls[2] = TypeLabel;

            var typeHelperTextBox = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlTextbox, T>(namePrefix + "_TypeHelperTextBox");
            typeHelperTextBox.Title = MyStringId.GetOrCompute("Selected Type");
            typeHelperTextBox.Tooltip = MyStringId.GetOrCompute("Currently Selected Type");
            typeHelperTextBox.Enabled = defaultEnabled;
            typeHelperTextBox.Visible = defaultEnabled;
            typeHelperTextBox.Getter = typeHelperGetter;
            typeHelperTextBox.Setter = typeHelperSetter;
            MyAPIGateway.TerminalControls.AddControl<T>(typeHelperTextBox);
            controls[3] = typeHelperTextBox;

            var typeListbox = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlListbox, T>(namePrefix + "_TypeListBox");
            typeListbox.Title = MyStringId.GetOrCompute("Type List:");
            typeListbox.Tooltip = MyStringId.GetOrCompute("");
            typeListbox.VisibleRowsCount = 5;
            typeListbox.Multiselect = true;
            typeListbox.Enabled = defaultEnabled;
            typeListbox.Visible = defaultEnabled;
            typeListbox.ItemSelected = typeSelected;
            typeListbox.ListContent = typeContent;
            MyAPIGateway.TerminalControls.AddControl<T>(typeListbox);
            controls[4] = typeListbox;

            var upperSearchSeparator = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, T>(namePrefix + "_UpperSearchSeparator");
            MyAPIGateway.TerminalControls.AddControl<T>(upperSearchSeparator);
            controls[5] = upperSearchSeparator;

            var searchTypeTextBox = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlTextbox, T>(namePrefix + "_SearchTypeTextBox");
            searchTypeTextBox.Title = MyStringId.GetOrCompute("Type Name");
            searchTypeTextBox.Tooltip = MyStringId.GetOrCompute("Searching empty field will refresh the Type Listbox");
            searchTypeTextBox.Enabled = defaultEnabled;
            searchTypeTextBox.Visible = defaultEnabled;
            searchTypeTextBox.Getter = textGetter;
            searchTypeTextBox.Setter = textSetter;
            MyAPIGateway.TerminalControls.AddControl<T>(searchTypeTextBox);
            controls[6] = searchTypeTextBox;

            var searchTypeButton = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, T>(namePrefix + "_SearchTypeButton");
            searchTypeButton.Action = searchConfigAction;
            searchTypeButton.Title = MyStringId.GetOrCompute("Search");
            searchTypeButton.Tooltip = MyStringId.GetOrCompute("Search the list of known CubeBlock Types");
            MyAPIGateway.TerminalControls.AddControl<T>(searchTypeButton);
            controls[7] = searchTypeButton;

            var lowerSearchSeparator = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, T>(namePrefix + "_LowerSearchSeparator");
            MyAPIGateway.TerminalControls.AddControl<T>(lowerSearchSeparator);
            controls[8] = lowerSearchSeparator;

            var subtypeLabel = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlLabel, T>(namePrefix + "_SubtypeLabel");
            subtypeLabel.Label = MyStringId.GetOrCompute("CubeBlock Subtypes");
            subtypeLabel.Enabled = defaultEnabled;
            subtypeLabel.Visible = defaultEnabled;
            MyAPIGateway.TerminalControls.AddControl<T>(subtypeLabel);
            controls[9] = subtypeLabel;

            var subtypeListbox = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlListbox, T>(namePrefix + "_SubtypeListBox");
            subtypeListbox.Title = MyStringId.GetOrCompute("Subtype List:");
            subtypeListbox.Tooltip = MyStringId.GetOrCompute("");
            subtypeListbox.VisibleRowsCount = 5;
            subtypeListbox.Multiselect = true;
            subtypeListbox.Enabled = defaultEnabled;
            subtypeListbox.Visible = defaultEnabled;
            subtypeListbox.ItemSelected = subTypeSelected;
            subtypeListbox.ListContent = subTypeContent;
            MyAPIGateway.TerminalControls.AddControl<T>(subtypeListbox);
            controls[10] = subtypeListbox;

            var upperCreateConfigSeparator = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, T>(namePrefix + "_UpperCreateConfigSeparator");
            MyAPIGateway.TerminalControls.AddControl<T>(upperCreateConfigSeparator);
            controls[11] = upperCreateConfigSeparator;

            var subtypeHelperTextBox = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlTextbox, T>(namePrefix + "_SubtypeHelperTextBox");
            subtypeHelperTextBox.Title = MyStringId.GetOrCompute("Selected Subtype");
            subtypeHelperTextBox.Tooltip = MyStringId.GetOrCompute("Currently Selected Subtype");
            subtypeHelperTextBox.Enabled = defaultEnabled;
            subtypeHelperTextBox.Visible = defaultEnabled;
            subtypeHelperTextBox.Getter = subtypeHelperGetter;
            subtypeHelperTextBox.Setter = subTypeHelperSetter;
            MyAPIGateway.TerminalControls.AddControl<T>(subtypeHelperTextBox);
            controls[12] = subtypeHelperTextBox;

            var createNewConfigButton = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, T>(namePrefix + "_CreateConfig");
            createNewConfigButton.Action = createNewTemplateAction;
            createNewConfigButton.Title = MyStringId.GetOrCompute("Create New");
            createNewConfigButton.Tooltip = MyStringId.GetOrCompute("Creates a new template from the selected subtype, and brings you to the Template Editor Page");
            MyAPIGateway.TerminalControls.AddControl<T>(createNewConfigButton);
            controls[13] = createNewConfigButton;

            var lowerEditSeparator = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, T>(namePrefix + "_LowerEditSeparator");
            MyAPIGateway.TerminalControls.AddControl<T>(lowerEditSeparator);
            controls[14] = lowerEditSeparator;

            var backToConfigManagerButton = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, T>(namePrefix + "_BackToConfigManagerMainPage");
            backToConfigManagerButton.Action = backToConfigManagerMainAction;
            backToConfigManagerButton.Title = MyStringId.GetOrCompute("Main Menu");
            backToConfigManagerButton.Tooltip = MyStringId.GetOrCompute("Brings you back to the Config Manager Main Page");
            MyAPIGateway.TerminalControls.AddControl<T>(backToConfigManagerButton);
            controls[15] = backToConfigManagerButton;

            var lowerPageSeparator = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, T>(namePrefix + "_LowerSeparator");
            MyAPIGateway.TerminalControls.AddControl<T>(lowerPageSeparator);
            controls[16] = lowerPageSeparator;

            return controls;
        }

        /// <summary>
        /// Creates a set of terminal controls to manage a range through three sliders: Minimum Value, Current Value, and Maximum Value.
        /// The method generates a label, upper and lower separators, and three sliders for setting these values.
        /// </summary>
        /// <typeparam name="T">The type of the terminal block for which the controls are being created.</typeparam>
        /// <param name="block">The terminal block instance.</param>
        /// <param name="namePrefix">A unique prefix for naming the controls.</param>
        /// <param name="title">The title that will be displayed on the UI.</param>
        /// <param name="min">The minimum limit for the sliders.</param>
        /// <param name="max">The maximum limit for the sliders.</param>
        /// <param name="getMinValue">A function to get the current minimum value.</param>
        /// <param name="setMinValue">An action to set a new minimum value.</param>
        /// <param name="getValue">A function to get the current value.</param>
        /// <param name="setValue">An action to set a new value.</param>
        /// <param name="getMaxValue">A function to get the current maximum value.</param>
        /// <param name="setMaxValue">An action to set a new maximum value.</param>
        /// <param name="enabledGetter">Optional function to determine if the control should be enabled.</param>
        /// <param name="visibleGetter">Optional function to determine if the control should be visible.</param>
        /// <returns>An array of IMyTerminalControl that contains the generated controls.</returns>
        internal static IMyTerminalControl[] DynamicRangeSliders<T>(
            T block,
            string namePrefix,
            string title,
            float min,
            float max,
            Func<IMyTerminalBlock, float> getMinValue,
            Action<IMyTerminalBlock, float> setMinValue,
            Func<IMyTerminalBlock, float> getValue,
            Action<IMyTerminalBlock, float> setValue,
            Func<IMyTerminalBlock, float> getMaxValue,
            Action<IMyTerminalBlock, float> setMaxValue,
            Func<IMyTerminalBlock, bool> enabledGetter = null,
            Func<IMyTerminalBlock, bool> visibleGetter = null
        ) where T : IMyTerminalBlock
        {
            // Initialize controls array and default state
            var controls = new IMyTerminalControl[6];
            var defaultEnabled = GetDefaultEnabled();

            // Create title label
            var titleLabel = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlLabel, T>(namePrefix + "_TitleLabel");
            titleLabel.Label = MyStringId.GetOrCompute(title);
            titleLabel.Enabled = defaultEnabled;
            titleLabel.Visible = defaultEnabled;
            MyAPIGateway.TerminalControls.AddControl<T>(titleLabel);
            controls[0] = titleLabel;

            // Create upper separator
            var upperSeparator = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, T>(namePrefix + "_UpperSeparator");
            MyAPIGateway.TerminalControls.AddControl<T>(upperSeparator);
            controls[1] = upperSeparator;

            // Create Min Value Slider
            var minValueSlider = TerminalHelpers.AddSlider(
                block,
                "MinValue",
                "Minimum Value",
                "Set the minimum value",
                getMinValue,
                setMinValue,
                enabledGetter,
                visibleGetter
            );
            controls[2] = minValueSlider;

            // Create Value Slider
            var valueSlider = TerminalHelpers.AddDynamicSlider(
                block,
                "Value",
                "Value",
                "Set the value",
                getValue,
                setValue,
                getMinValue,
                getMaxValue,
                enabledGetter,
                visibleGetter
            );
            controls[3] = valueSlider;

            // Create Max Value Slider
            var maxValueSlider = TerminalHelpers.AddSlider(
                block,
                "MaxValue",
                "Maximum Value",
                "Set the maximum value",
                getMaxValue,
                setMaxValue,
                enabledGetter,
                visibleGetter
            );
            controls[4] = maxValueSlider;

            // Set the initial limits for each of the sliders
            minValueSlider.SetLimits(min, max);
            maxValueSlider.SetLimits(min, max);
            valueSlider.SetLimits(min, max);

            // Dynamically update the limits of the "Value" slider based on changes in "Min" and "Max" sliders
            minValueSlider.Setter = (b, v) => { setMinValue(b, v); valueSlider.SetLimits(getMinValue(b), getMaxValue(b)); };
            maxValueSlider.Setter = (b, v) => { setMaxValue(b, v); valueSlider.SetLimits(getMinValue(b), getMaxValue(b)); };

            // Create lower separator
            var lowerSeparator = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, T>(namePrefix + "_LowerSeparator");
            MyAPIGateway.TerminalControls.AddControl<T>(lowerSeparator);
            controls[5] = lowerSeparator;

            return controls;
        }





        internal static IMyTerminalControl[] AddOriginalVectorEditor<T>(T block, string name, string title, string tooltip, Func<IMyTerminalBlock, Vector3> getter, Action<IMyTerminalBlock, Vector3> setter, float min = -10, float max = 10, Func<IMyTerminalBlock, bool> enabledGetter = null, Func<IMyTerminalBlock, bool> visibleGetter = null, string writerFormat = "0.##") where T : IMyTerminalBlock
        {
            var controls = new IMyTerminalControl[4];

            var d = GetDefaultEnabled();

            var lb = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlLabel, T>(name + "_Label");
            lb.Label = MyStringId.GetOrCompute(title);
            lb.Enabled = enabledGetter ?? d;
            lb.Visible = visibleGetter ?? d;
            MyAPIGateway.TerminalControls.AddControl<T>(lb);
            controls[0] = lb;

            var x = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSlider, T>(name + "_X");
            x.Title = MyStringId.GetOrCompute("X");
            x.Tooltip = MyStringId.GetOrCompute(tooltip);
            x.Writer = (b, s) => s.Append(getter(b).X.ToString(writerFormat));
            x.Getter = b => getter(b).X;
            x.Setter = (b, v) =>
            {
                var vc = getter(b);
                vc.X = v;
                setter(b, vc);
            };
            x.Enabled = enabledGetter ?? d;
            x.Visible = visibleGetter ?? d;
            x.SetLimits(min, max);
            MyAPIGateway.TerminalControls.AddControl<T>(x);
            controls[1] = x;

            var y = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSlider, T>(name + "_Y");
            y.Title = MyStringId.GetOrCompute("Y");
            y.Tooltip = MyStringId.GetOrCompute(tooltip);
            y.Writer = (b, s) => s.Append(getter(b).Y.ToString(writerFormat));
            y.Getter = b => getter(b).Y;
            y.Setter = (b, v) =>
            {
                var vc = getter(b);
                vc.Y = v;
                setter(b, vc);
            };
            y.Enabled = enabledGetter ?? d;
            y.Visible = visibleGetter ?? d;
            y.SetLimits(min, max);
            MyAPIGateway.TerminalControls.AddControl<T>(y);
            controls[2] = y;

            var z = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSlider, T>(name + "_Z");
            z.Title = MyStringId.GetOrCompute("Z");
            z.Tooltip = MyStringId.GetOrCompute(tooltip);
            z.Writer = (b, s) => s.Append(getter(b).Z.ToString(writerFormat));
            z.Getter = b => getter(b).Z;
            z.Setter = (b, v) =>
            {
                var vc = getter(b);
                vc.Z = v;
                setter(b, vc);
            };
            z.Enabled = enabledGetter ?? d;
            z.Visible = visibleGetter ?? d;
            z.SetLimits(min, max);
            MyAPIGateway.TerminalControls.AddControl<T>(z);
            controls[3] = z;

            return controls;
        }
    }
}
