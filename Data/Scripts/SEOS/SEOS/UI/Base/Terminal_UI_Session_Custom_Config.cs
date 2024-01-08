namespace SEOS.Core
{
    using System;
    using System.Collections.Generic;
    using Sandbox.ModAPI;
    using VRage.Game.Components;
    using Sandbox.ModAPI.Interfaces.Terminal;

    /// <summary>
    /// Partial class representing the main session component responsible for managing various functionalities.
    /// </summary>
    public partial class Session : MySessionComponentBase
    {
        /// <summary>
        /// Custom action getter for terminal controls, filtering actions based on block subtype.
        /// </summary>
        static void TerminalControls_CustomActionGetter(IMyTerminalBlock block, List<IMyTerminalAction> actions)
        {
            if (block is IMyAssembler)
            {
                string subtype = (block as IMyAssembler).BlockDefinition.SubtypeId;
                var itemsToRemove = new List<IMyTerminalAction>();

                foreach (var action in actions)
                {
                    switch (subtype)
                    {
                        case "OSBurner":
                            if (action.Id.StartsWith("OnOff") ||
                                action.Id.StartsWith("ShowInTerminal") ||
                                action.Id.StartsWith("ShowInToolbarConfig") ||
                                action.Id.StartsWith("ShowInInventory") ||
                                action.Id.StartsWith("ShowOnHUD") ||
                                action.Id.StartsWith("UseConveyor") ||
                                action.Id.StartsWith("SEOS"))
                            {
                                itemsToRemove.Add(action);
                            }
                            break;
                        case "ROMBurner":
                            if (action.Id.StartsWith("OnOff") ||
                                action.Id.StartsWith("ShowInTerminal") ||
                                action.Id.StartsWith("ShowInToolbarConfig") ||
                                action.Id.StartsWith("ShowInInventory") ||
                                action.Id.StartsWith("ShowOnHUD") ||
                                action.Id.StartsWith("UseConveyor") ||
                                action.Id.StartsWith("SEOS"))
                            {
                                itemsToRemove.Add(action);
                            }
                            break;
                    }
                }

                foreach (var action in itemsToRemove)
                {
                    actions.Remove(action);
                }
            }
        }

        /// <summary>
        /// Custom control getter for terminal controls, filtering controls based on block subtype.
        /// </summary>
        static void TerminalControls_CustomControlGetter(IMyTerminalBlock block, List<IMyTerminalControl> controls)
        {
            if (block is IMyAssembler)
            {
                string subtype = (block as IMyAssembler).BlockDefinition.SubtypeId;
                var itemsToRemove = new List<IMyTerminalControl>();
                int separatorsToKeep = 3;

                foreach (var control in controls)
                {
                    switch (subtype)
                    {
                        case "OSBurner":
                            switch (control.Id)
                            {
                                case "ShowInTerminal":
                                case "ShowInToolbarConfig":
                                case "ShowInInventory":
                                case "ShowOnHUD":
                                case "UseConveyor":
                                case "CustomData":
                                    itemsToRemove.Add(control);
                                    break;
                                default:
                                    if (control.Id.StartsWith("SEOS"))
                                        break;
                                    else if (control is IMyTerminalControlLabel)
                                        break;
                                    else if (control is IMyTerminalControlSeparator && separatorsToKeep-- >= 0)
                                        break;
                                    break;
                            }
                            break;
                        case "ROMBurner":
                            switch (control.Id)
                            {
                                case "ShowInTerminal":
                                case "ShowInToolbarConfig":
                                case "ShowInInventory":
                                case "ShowOnHUD":
                                case "UseConveyor":
                                case "CustomData":
                                    itemsToRemove.Add(control);
                                    break;
                                default:
                                    if (control.Id.StartsWith("SEOS"))
                                        break;
                                    else if (control is IMyTerminalControlLabel)
                                        break;
                                    else if (control is IMyTerminalControlSeparator && separatorsToKeep-- >= 0)
                                        break;
                                    break;
                            }
                            break;
                    }
                }

                foreach (var control in itemsToRemove)
                {
                    controls.Remove(control);
                }
            }

            if (block is IMyUpgradeModule)
            {
                string subtype = (block as IMyUpgradeModule).BlockDefinition.SubtypeId;
                var itemsToRemove = new List<IMyTerminalControl>();
                int separatorsToKeep = 3;

                foreach (var control in controls)
                {
                    switch (subtype)
                    {
                        case "SEOSTerminal":
                            switch (control.Id)
                            {
                                case "OnOff":
                                case "ShowInTerminal":
                                case "ShowInToolbarConfig":
                                case "ShowInInventory":
                                case "Name":
                                case "ShowOnHUD":
                                case "UseConveyor":
                                case "CustomData":
                                    itemsToRemove.Add(control);
                                    break;
                                default:
                                    if (control.Id.StartsWith("SEOS"))
                                        break;
                                    else if (control is IMyTerminalControlLabel)
                                        break;
                                    else if (control is IMyTerminalControlSeparator && separatorsToKeep-- >= 0)
                                        break;
                                    break;
                            }
                            break;
                    }
                }

                foreach (var control in itemsToRemove)
                {
                    controls.Remove(control);
                }
            }
        }

        /// <summary>
        /// Appends a condition to the given action based on specific criteria.
        /// </summary>
        public static void AppendConditionToAction<T>(Func<IMyTerminalAction, bool> actionFindCondition, Func<IMyTerminalAction, IMyTerminalBlock, bool> actionEnabledAppend)
        {
            List<IMyTerminalAction> actions;
            MyAPIGateway.TerminalControls.GetActions<T>(out actions);
            foreach (var action in actions)
            {
                if (actionFindCondition(action))
                {
                    var existingAction = action.Enabled;

                    action.Enabled = (block) => (existingAction == null ? true : existingAction.Invoke(block)) && actionEnabledAppend(action, block);
                }
            }
        }
    }
}