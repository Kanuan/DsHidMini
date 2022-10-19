using Nefarius.DsHidMini.ControlApp.JsonSettings;
using Nefarius.DsHidMini.ControlApp.UserData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupRumbleRightConversionAdjustsVM : GroupSettingsVM
    {
        // -------------------------------------------- RIGHT MOTOR CONVERSION GROUP

        public static readonly BackingData_VariablaRightRumbleEmulAdjusts defaultSettings = new()
        {

        };


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

        private void PrepareForLoad()
        {
            // Necessary to set these vars to their max/min so their properties setting conditions don't interfere with the loading
            rightRumbleConversionUpperRange = 255;
            rightRumbleConversionLowerRange = 1;
    }

        public override void CopySettingsFromBackingData(SettingsBackingData rightRumbleEmulAdjustsBackingData, bool invertCopyDirection = false)
        {
            base.CopySettingsFromBackingData(rightRumbleEmulAdjustsBackingData, invertCopyDirection);

            var specific = (BackingData_VariablaRightRumbleEmulAdjusts)rightRumbleEmulAdjustsBackingData;

            if (invertCopyDirection)
            {
                specific.IsGroupEnabled = this.IsGroupEnabled;
                specific.RightRumbleConversionLowerRange = this.RightRumbleConversionLowerRange;
                specific.RightRumbleConversionUpperRange = this.RightRumbleConversionUpperRange;
                // Right rumble (light) threshold
                specific.IsForcedRightMotorLightThresholdEnabled = this.IsForcedRightMotorLightThresholdEnabled;
                specific.ForcedRightMotorLightThreshold = this.ForcedRightMotorLightThreshold;
                // Left rumble (Heavy) threshold
                specific.IsForcedRightMotorHeavyThreasholdEnabled = this.IsForcedRightMotorHeavyThreasholdEnabled;
                specific.ForcedRightMotorHeavyThreshold = this.ForcedRightMotorHeavyThreshold;
            }
            else
            {
                PrepareForLoad();
                this.IsGroupEnabled = specific.IsGroupEnabled;
                this.RightRumbleConversionLowerRange = specific.RightRumbleConversionLowerRange;
                this.RightRumbleConversionUpperRange = specific.RightRumbleConversionUpperRange;
                // Right rumble (light) threshold
                this.IsForcedRightMotorLightThresholdEnabled = specific.IsForcedRightMotorLightThresholdEnabled;
                this.ForcedRightMotorLightThreshold = specific.ForcedRightMotorLightThreshold;
                // Left rumble (Heavy) threshold
                this.IsForcedRightMotorHeavyThreasholdEnabled = specific.IsForcedRightMotorHeavyThreasholdEnabled;
                this.ForcedRightMotorHeavyThreshold = specific.ForcedRightMotorHeavyThreshold;
            }
        }
    }


}
