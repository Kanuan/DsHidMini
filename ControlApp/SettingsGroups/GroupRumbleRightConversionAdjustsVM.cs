using Nefarius.DsHidMini.ControlApp.JsonSettings;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupRumbleRightConversionAdjustsVM : GroupSettingsVM
    {
        // -------------------------------------------- RIGHT MOTOR CONVERSION GROUP

        public const byte DEFAULT_rightRumbleConversionUpperRange = 140;
        public const byte DEFAULT_rightRumbleConversionLowerRange = 1;
        public const bool DEFAULT_isForcedRightMotorLightThresholdEnabled = false;
        public const bool DEFAULT_isForcedRightMotorHeavyThreasholdEnabled = false;
        public const byte DEFAULT_forcedRightMotorLightThreshold = 230;
        public const byte DEFAULT_forcedRightMotorHeavyThreshold = 230;


        public override SettingsModeGroups Group { get; } = SettingsModeGroups.RumbleRightConversion;

        private byte rightRumbleConversionUpperRange;
        private byte rightRumbleConversionLowerRange;

        [Reactive] public bool IsGroupEnabled { get; set; }
        public byte RightRumbleConversionUpperRange
        {
            get => rightRumbleConversionUpperRange;
            set
            {
                byte tempByte = (value < RightRumbleConversionLowerRange) ? (byte)(RightRumbleConversionLowerRange + 1) : value;
                this.RaiseAndSetIfChanged(ref rightRumbleConversionUpperRange, tempByte);
            }
        }
        public byte RightRumbleConversionLowerRange
        {
            get => rightRumbleConversionLowerRange;
            set
            {
                byte tempByte = (value > RightRumbleConversionUpperRange) ? (byte)(RightRumbleConversionUpperRange - 1) : value;
                this.RaiseAndSetIfChanged(ref rightRumbleConversionLowerRange, tempByte);
            }
        }
        [Reactive] public bool IsForcedRightMotorLightThresholdEnabled { get; set; }
        [Reactive] public bool IsForcedRightMotorHeavyThreasholdEnabled { get; set; }
        [Reactive] public byte ForcedRightMotorLightThreshold { get; set; }
        [Reactive] public byte ForcedRightMotorHeavyThreshold { get; set; }

        public GroupRumbleRightConversionAdjustsVM(SettingsContext context, SettingsContainer containter) : base(context, containter) { }

        public override void ResetGroupToOriginalDefaults()
        {
            IsGroupEnabled = ShouldGroupBeEnabledOnReset();
            RightRumbleConversionUpperRange = DEFAULT_rightRumbleConversionUpperRange;
            RightRumbleConversionLowerRange = DEFAULT_rightRumbleConversionLowerRange;
            IsForcedRightMotorLightThresholdEnabled = DEFAULT_isForcedRightMotorLightThresholdEnabled;
            IsForcedRightMotorHeavyThreasholdEnabled = DEFAULT_isForcedRightMotorHeavyThreasholdEnabled;
            ForcedRightMotorLightThreshold = DEFAULT_forcedRightMotorLightThreshold;
            ForcedRightMotorHeavyThreshold = DEFAULT_forcedRightMotorHeavyThreshold;
        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.SMToBMConversionSettings dshmSMConversionSettings = dshmContextSettings.RumbleSettings.SMToBMConversion;
            DSHM_Format_ContextSettings.ForcedSMSettings dshmForcedSMSettings = dshmContextSettings.RumbleSettings.ForcedSM;

            if (!this.IsGroupEnabled)
            {
                dshmSMConversionSettings.RescaleMinValue = null;
                dshmSMConversionSettings.RescaleMaxValue = null;

                dshmForcedSMSettings = null;
                return;
            }

            // Right rumble conversion rescaling adjustment
            dshmSMConversionSettings.RescaleMinValue = this.RightRumbleConversionLowerRange;
            dshmSMConversionSettings.RescaleMaxValue = this.RightRumbleConversionUpperRange;

            // Right rumble (light) threshold
            dshmForcedSMSettings.SMThresholdEnabled = this.IsForcedRightMotorLightThresholdEnabled;
            dshmForcedSMSettings.SMThresholdValue = this.ForcedRightMotorLightThreshold;

            // Left rumble (Heavy) threshold
            dshmForcedSMSettings.BMThresholdEnabled = this.IsForcedRightMotorHeavyThreasholdEnabled;
            dshmForcedSMSettings.BMThresholdValue = this.ForcedRightMotorHeavyThreshold;
        }

        public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.SMToBMConversionSettings dshmSMConversionSettings = dshmContextSettings.RumbleSettings.SMToBMConversion;
            DSHM_Format_ContextSettings.ForcedSMSettings dshmForcedSMSettings = dshmContextSettings.RumbleSettings.ForcedSM;

            if (dshmSMConversionSettings.RescaleMinValue == null
                || dshmSMConversionSettings.RescaleMaxValue == null
                || dshmForcedSMSettings == null
                )
            {
                this.IsGroupEnabled = false;
                return;
            }

            // Right rumble conversion rescaling adjustment
            this.RightRumbleConversionLowerRange = dshmSMConversionSettings.RescaleMinValue.GetValueOrDefault();
            this.RightRumbleConversionUpperRange = dshmSMConversionSettings.RescaleMaxValue.GetValueOrDefault();

            // Right rumble (light) threshold
            this.IsForcedRightMotorLightThresholdEnabled = dshmForcedSMSettings.SMThresholdEnabled.GetValueOrDefault();
            this.ForcedRightMotorLightThreshold = dshmForcedSMSettings.SMThresholdValue.GetValueOrDefault();


            // Left rumble (Heavy) threshold
            this.IsForcedRightMotorHeavyThreasholdEnabled = dshmForcedSMSettings.BMThresholdEnabled.GetValueOrDefault();
            this.ForcedRightMotorHeavyThreshold = dshmForcedSMSettings.BMThresholdValue.GetValueOrDefault();
        }
    }


}
