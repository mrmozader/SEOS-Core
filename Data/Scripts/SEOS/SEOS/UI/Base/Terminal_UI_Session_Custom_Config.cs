namespace SEOS.Core
{
    using System;
    using System.Collections.Generic;
    using Sandbox.ModAPI;
    using VRage.Game.Components;
    using Sandbox.ModAPI.Interfaces.Terminal;

    public partial class Session : MySessionComponentBase
    {
        static void TerminalControls_CustomActionGetter(IMyTerminalBlock block, List<IMyTerminalAction> actions)
        {
            if (block is IMyAssembler)
            {
                string subtype = (block as IMyAssembler).BlockDefinition.SubtypeId;
                var itemsToRemove = new List<IMyTerminalAction>();

                foreach (var action in actions)
                {
                    //SessionLog.Line("Action: " + action.Id);
                    switch (subtype)
                    {
                        case "OSBurner":
                            if (
                            action.Id.StartsWith("OnOff") ||
                            action.Id.StartsWith("ShowInTerminal") ||
                            action.Id.StartsWith("ShowInToolbarConfig") ||
                            action.Id.StartsWith("ShowInInventory") ||
                            action.Id.StartsWith("ShowOnHUD") ||
                            action.Id.StartsWith("UseConveyor") ||
                            action.Id.StartsWith("SEOS")
                            )
                            {
                                itemsToRemove.Add(action);
                            }
                        break;

                        case "ROMBurner":
                            if (
                            action.Id.StartsWith("OnOff") ||
                            action.Id.StartsWith("ShowInTerminal") ||
                            action.Id.StartsWith("ShowInToolbarConfig") ||
                            action.Id.StartsWith("ShowInInventory") ||
                            action.Id.StartsWith("ShowOnHUD") ||
                            action.Id.StartsWith("UseConveyor") ||
                            action.Id.StartsWith("SEOS")
                            )
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
        static void TerminalControls_CustomControlGetter(IMyTerminalBlock block, List<IMyTerminalControl> controls)
        {
            if (block is IMyAssembler)
            {
                string subtype = (block as IMyAssembler).BlockDefinition.SubtypeId;
                var itemsToRemove = new List<IMyTerminalControl>();
                int separatorsToKeep = 3;

                foreach (var control in controls)
                {
                    //SessionLog.Line("Control: " + control.Id);
                    switch (subtype)
                    {
                        case "OSBurner":
                            switch (control.Id)
                            {
                                //case "OnOff":
                                case "ShowInTerminal":
                                case "ShowInToolbarConfig":
                                case "ShowInInventory":
                                //case "Name":
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
                                    //itemsToRemove.Add(control);
                                    break;
                            }
                            break;
                        case "ROMBurner":
                            switch (control.Id)
                            {
                                //case "OnOff":
                                case "ShowInTerminal":
                                case "ShowInToolbarConfig":
                                case "ShowInInventory":
                                //case "Name":
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
                                    //itemsToRemove.Add(control);
                                    break;
                            }
                            break;

                    }
                }

                foreach (var action in itemsToRemove)
                {
                    controls.Remove(action);
                }
            }

            if (block is IMyUpgradeModule)
            {
                string subtype = (block as IMyUpgradeModule).BlockDefinition.SubtypeId;
                var itemsToRemove = new List<IMyTerminalControl>();
                int separatorsToKeep = 3;

                foreach (var control in controls)
                {
                   // SessionLog.Line("Control: " + control.Id);
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
                                    //itemsToRemove.Add(control);
                                    break;
                            }
                            break;


                    }
                }

                foreach (var action in itemsToRemove)
                {
                    controls.Remove(action);
                }
            }
        }
        public static void AppendConditionToAction<T>(Func<IMyTerminalAction, bool> actionFindCondition, Func<IMyTerminalAction, IMyTerminalBlock, bool> actionEnabledAppend)
        {
            List<IMyTerminalAction> actions;
            MyAPIGateway.TerminalControls.GetActions<T>(out actions);
            foreach (var a in actions)
            {
                if (actionFindCondition(a))
                {
                    var existingAction = a.Enabled;

                    a.Enabled = (b) => (existingAction == null ? true : existingAction.Invoke(b)) && actionEnabledAppend(a, b);
                }
            }
        }
    }
}

