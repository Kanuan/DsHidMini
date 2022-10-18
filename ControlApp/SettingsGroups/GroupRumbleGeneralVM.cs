using Nefarius.DsHidMini.ControlApp.JsonSettings;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupRumbleGeneralVM : GroupSettingsVM
    {
        // -------------------------------------------- DEFAULT GENERAL RUMBLE SETTINGS 
        public const bool DEFAULT_isVariableLightRumbleEmulationEnabled = false;
        public const bool DEFAULT_isLeftMotorDisabled = false;
        public const bool DEFAULT_isRightMotorDisabled = false;
        public const bool DEFAULT_isVariableRightEmulToggleComboEnabled = false;

        // --------------------------------------------

        private bool isVariableLightRumbleEmulationEnabled;
        private bool isLeftMotorDisabled;
        private bool isRightMotorDisabled;

        public static readonly ButtonsCombo DEFAULT_VariableRightEmuToggleCombo = new()
        {
            Button1 = ControlApp_ComboButtons.PS,
            Button2 = ControlApp_ComboButtons.SELECT,
            Button3 = ControlApp_ComboButtons.None,
        };



        public override SettingsModeGroups Group { get; } = SettingsModeGroups.RumbleGeneral;
        [Reactive] public bool IsGroupEnabled { get; set; }
        public bool IsVariableLightRumbleEmulationEnabled
        {
            get => isVariableLightRumbleEmulationEnabled;

            set
            {
                if (value)
                {
                    IsLeftMotorDisabled = IsRightMotorDisabled = false;
                }
                this.RaiseAndSetIfChanged(ref isVariableLightRumbleEmulationEnabled, value);
            }
        }

        [Reactive] public bool IsVariableRightEmulToggleComboEnabled { get; set; }

        [Reactive] public ButtonsCombo VariableRightEmulToggleCombo { get; set; } = new();



        public bool IsLeftMotorDisabled
        {
            get => isLeftMotorDisabled;

            set
            {
                if (value) IsVariableLightRumbleEmulationEnabled = false;
                this.RaiseAndSetIfChanged(ref isLeftMotorDisabled, value);
            }
        }
        public bool IsRightMotorDisabled
        {
            get => isRightMotorDisabled;

            set
            {
                if (value) IsVariableLightRumbleEmulationEnabled = false;
                this.RaiseAndSetIfChanged(ref isRightMotorDisabled, value);
            }
        }

        public override void ResetGroupToOriginalDefaults()
        {
            IsGroupEnabled = ShouldGroupBeEnabledOnReset();

            IsVariableLightRumbleEmulationEnabled = DEFAULT_isVariableLightRumbleEmulationEnabled;
            IsLeftMotorDisabled = DEFAULT_isLeftMotorDisabled;
            IsRightMotorDisabled = DEFAULT_isRightMotorDisabled;
            IsVariableRightEmulToggleComboEnabled = DEFAULT_isVariableRightEmulToggleComboEnabled;
            VariableRightEmulToggleCombo.copyCombo(DEFAULT_VariableRightEmuToggleCombo);
        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.AllRumbleSettings dshmRumbleSettings = dshmContextSettings.RumbleSettings;

            /* Not necessary anymore I think
            if(SettingsContainer.GroupModeUnique.PreventRemappingConflictsInSXSMode)
            {
                dshmRumbleSettings.SMToBMConversion.Enabled = false;
                dshmRumbleSettings.DisableBM = false;
                dshmRumbleSettings.DisableSM = false;
                return;
            }
            */

            if (!this.IsGroupEnabled)
            {
                dshmRumbleSettings.SMToBMConversion.Enabled = null;
                dshmRumbleSettings.DisableBM = null;
                dshmRumbleSettings.DisableSM = null;
                return;
            }

            dshmRumbleSettings.SMToBMConversion.Enabled = this.IsVariableLightRumbleEmulationEnabled;
            dshmRumbleSettings.DisableBM = this.IsLeftMotorDisabled;
            dshmRumbleSettings.DisableSM = this.IsLeftMotorDisabled;
        }

        public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.AllRumbleSettings dshmRumbleSettings = dshmContextSettings.RumbleSettings;

            /* Not necessary anymore I think
            bool tempPreventSXSConflicts =
                dshmContextSettings.PreventRemappingConflitsInSXSMode == null ?
                false : dshmContextSettings.PreventRemappingConflitsInSXSMode.GetValueOrDefault();
            if (tempPreventSXSConflicts) return;
            */

            if (
                dshmRumbleSettings.SMToBMConversion.Enabled == null
                || dshmRumbleSettings.DisableBM == null
                || dshmRumbleSettings.DisableSM == null
               )
            {
                this.IsGroupEnabled = false;
                return;
            }
            this.IsGroupEnabled = true;

            this.IsVariableLightRumbleEmulationEnabled = dshmRumbleSettings.SMToBMConversion.Enabled.GetValueOrDefault();
            this.IsLeftMotorDisabled = dshmRumbleSettings.DisableBM.GetValueOrDefault();
            this.IsLeftMotorDisabled = dshmRumbleSettings.DisableSM.GetValueOrDefault();
        }

        public GroupRumbleGeneralVM(SettingsContext context, SettingsContainer containter) : base(context, containter) { }
    }


}
