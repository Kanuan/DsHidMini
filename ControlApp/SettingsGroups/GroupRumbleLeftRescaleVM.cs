using Nefarius.DsHidMini.ControlApp.JsonSettings;
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
        [Reactive] public bool IsLeftMotorStrRescalingEnabled { get; set; }
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

        public GroupRumbleLeftRescaleVM(SettingsContext context) : base(context) { }

        public override void ResetGroupToOriginalDefaults()
        {
            IsGroupEnabled = ShouldGroupBeEnabledOnReset();

            IsLeftMotorStrRescalingEnabled = DEFAULT_isLeftMotorStrRescalingEnabled;
            LeftMotorStrRescalingUpperRange = DEFAULT_leftMotorStrRescalingUpperRange;
            LeftMotorStrRescalingLowerRange = DEFAULT_leftMotorStrRescalingLowerRange;
        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.BMStrRescaleSettings dshmLeftRumbleRescaleSettings = dshmContextSettings.RumbleSettings.BMStrRescale;

            if (!this.IsGroupEnabled)
            {
                dshmLeftRumbleRescaleSettings = null;
                return;
            }
            dshmLeftRumbleRescaleSettings.Enabled = this.IsLeftMotorStrRescalingEnabled;
            dshmLeftRumbleRescaleSettings.MinValue = this.LeftMotorStrRescalingLowerRange;
            dshmLeftRumbleRescaleSettings.MaxValue = this.LeftMotorStrRescalingUpperRange;
        }

        public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.BMStrRescaleSettings dshmLeftRumbleRescaleSettings = dshmContextSettings.RumbleSettings.BMStrRescale;

            if(dshmLeftRumbleRescaleSettings == null)
            {
                this.IsGroupEnabled = false;
                return;
            }
            this.IsGroupEnabled = true;

            this.IsLeftMotorStrRescalingEnabled = dshmLeftRumbleRescaleSettings.Enabled.GetValueOrDefault();
            this.LeftMotorStrRescalingLowerRange = dshmLeftRumbleRescaleSettings.MinValue.GetValueOrDefault();
            this.LeftMotorStrRescalingUpperRange = dshmLeftRumbleRescaleSettings.MaxValue.GetValueOrDefault();

        }
    }


}
