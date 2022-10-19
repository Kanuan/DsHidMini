using Nefarius.DsHidMini.ControlApp.JsonSettings;
using Nefarius.DsHidMini.ControlApp.UserData;
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

        
        

        public override void CopySettingsFromBackingData(SettingsBackingData rumbleGeneralData, bool invertCopyDirection = false)
        {
            base.CopySettingsFromBackingData(rumbleGeneralData, invertCopyDirection);

            var specific = (BackingData_RumbleGeneral)rumbleGeneralData;
            if(invertCopyDirection)
            {
                specific.IsVariableLightRumbleEmulationEnabled = this.IsVariableLightRumbleEmulationEnabled;
                specific.IsLeftMotorDisabled = this.IsLeftMotorDisabled;
                specific.IsRightMotorDisabled = this.IsRightMotorDisabled;
                specific.IsVariableRightEmulToggleComboEnabled = this.IsVariableRightEmulToggleComboEnabled;
                specific.VariableRightEmulToggleCombo.copyCombo(this.VariableRightEmulToggleCombo);
            }
            else
            {
                this.IsVariableLightRumbleEmulationEnabled = specific.IsVariableLightRumbleEmulationEnabled;
                this.IsLeftMotorDisabled = specific.IsLeftMotorDisabled;
                this.IsRightMotorDisabled = specific.IsRightMotorDisabled;
                this.IsVariableRightEmulToggleComboEnabled = specific.IsVariableRightEmulToggleComboEnabled;
                this.VariableRightEmulToggleCombo.copyCombo(specific.VariableRightEmulToggleCombo);
            }
        }

        public GroupRumbleGeneralVM(SettingsContext context, SettingsContainer containter) : base(context, containter) { }
    }


}
