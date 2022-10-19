using Nefarius.DsHidMini.ControlApp.JsonSettings;
using Nefarius.DsHidMini.ControlApp.UserData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupRumbleLeftRescaleVM : GroupSettingsVM
    {
        // -------------------------------------------- LEFT MOTOR RESCALING GROUP
        public const bool DEFAULT_isLeftMotorStrRescalingEnabled = true;
        public const byte DEFAULT_leftMotorStrRescalingUpperRange = 255;
        public const byte DEFAULT_leftMotorStrRescalingLowerRange = 64;

        private byte leftMotorStrRescalingUpperRange;
        private byte leftMotorStrRescalingLowerRange;

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.RumbleLeftStrRescale;

        [Reactive] public bool IsGroupEnabled { get; set; }

        public bool IsLeftMotorStrRescalingEnabled { get; set; }
        public byte LeftMotorStrRescalingUpperRange
        {
            get => leftMotorStrRescalingUpperRange;
            set
            {
                byte tempByte = (value < leftMotorStrRescalingLowerRange) ? (byte)(leftMotorStrRescalingLowerRange + 1) : value;
                this.RaiseAndSetIfChanged(ref leftMotorStrRescalingUpperRange, tempByte);
            }
        }
        public byte LeftMotorStrRescalingLowerRange
        {
            get => leftMotorStrRescalingLowerRange;
            set
            {
                byte tempByte = (value > leftMotorStrRescalingUpperRange) ? (byte)(leftMotorStrRescalingUpperRange - 1) : value;
                this.RaiseAndSetIfChanged(ref leftMotorStrRescalingLowerRange, tempByte);
            }
        }

        public GroupRumbleLeftRescaleVM(SettingsContext context, SettingsContainer containter) : base(context, containter) { }

        public override void ResetGroupToOriginalDefaults()
        {
            IsGroupEnabled = ShouldGroupBeEnabledOnReset();

            IsLeftMotorStrRescalingEnabled = DEFAULT_isLeftMotorStrRescalingEnabled;
            LeftMotorStrRescalingUpperRange = DEFAULT_leftMotorStrRescalingUpperRange;
            LeftMotorStrRescalingLowerRange = DEFAULT_leftMotorStrRescalingLowerRange;
        }

        private void PrepareForLoad()
        {
            // Necessary to set these vars to their max/min so their properties setting conditions don't interfere with the loading
            leftMotorStrRescalingUpperRange = 255;
            leftMotorStrRescalingLowerRange = 1;
        }

        public override void CopySettingsFromBackingData(SettingsBackingData leftRumbleRescaleBackingData, bool invertCopyDirection = false)
        {
            base.CopySettingsFromBackingData(leftRumbleRescaleBackingData, invertCopyDirection);

            var specific = (BackingData_LeftRumbleRescale)leftRumbleRescaleBackingData;

            if(invertCopyDirection)
            {
                specific.IsGroupEnabled = this.IsGroupEnabled;
                specific.IsLeftMotorStrRescalingEnabled = this.IsLeftMotorStrRescalingEnabled;
                specific.LeftMotorStrRescalingLowerRange = this.LeftMotorStrRescalingLowerRange;
                specific.LeftMotorStrRescalingUpperRange = this.LeftMotorStrRescalingUpperRange;
            }
            else
            {
                PrepareForLoad();
                this.IsGroupEnabled = specific.IsGroupEnabled;
                this.IsLeftMotorStrRescalingEnabled = specific.IsLeftMotorStrRescalingEnabled;
                this.LeftMotorStrRescalingLowerRange = specific.LeftMotorStrRescalingLowerRange;
                this.LeftMotorStrRescalingUpperRange = specific.LeftMotorStrRescalingUpperRange;
            }


        }
    }


}
