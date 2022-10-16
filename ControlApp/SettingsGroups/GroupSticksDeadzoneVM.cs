using Nefarius.DsHidMini.ControlApp.JsonSettings;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupSticksDeadzoneVM : GroupSettingsVM
    {
        // -------------------------------------------- STICKS DEADZONE GROUP
        public const bool DEFAULT_applyLeftStickDeadzone = true;
        public const bool DEFAULT_applyRightStickDeadzone = true;
        public const int DEFAULT_leftStickDeadzone = 0;
        public const int DEFAULT_rightStickDeadzone = 0;

        private bool isGroupEnabled;
        private bool applyLeftStickDeadZone;
        private bool applyRightStickDeadZone;

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.SticksDeadzone;
        public bool IsGroupEnabled
        {
            get => isGroupEnabled;
            set
            {
                isGroupEnabled = Context == SettingsContext.DS4W ? true : value;
                this.RaisePropertyChanged();
            }
        }
        public bool ApplyLeftStickDeadZone
        {
            get => applyLeftStickDeadZone;
            set
            {
                applyLeftStickDeadZone = Context == SettingsContext.DS4W ? false : value;
                this.RaisePropertyChanged();
            }
        }
        public bool ApplyRightStickDeadZone
        {
            get => applyRightStickDeadZone;
            set
            {
                applyRightStickDeadZone = Context == SettingsContext.DS4W ? false : value;
                this.RaisePropertyChanged();
            }
        }
        [Reactive] public int LeftStickDeadZone { get; set; } // in %
        [Reactive] public int RightStickDeadZone { get; set; } // in %
        [Reactive] public bool IsSettingLocked { get; set; }

        public GroupSticksDeadzoneVM(SettingsContext context) : base(context)
        {
            AdjustSettingsBasedOnContext();
        }

        public override void ChangeContext(SettingsContext context)
        {
            base.ChangeContext(context);
            AdjustSettingsBasedOnContext();
        }

        public void AdjustSettingsBasedOnContext()
        {
            if (Context == SettingsContext.DS4W)
            {
                IsOverrideCheckboxVisible = false;
                IsSettingLocked = true;
            }
        }

        public override void ResetGroupToOriginalDefaults()
        {
            IsGroupEnabled = ShouldGroupBeEnabledOnReset();

            ApplyRightStickDeadZone = DEFAULT_applyRightStickDeadzone;
            ApplyLeftStickDeadZone = DEFAULT_applyLeftStickDeadzone;
            LeftStickDeadZone = DEFAULT_leftStickDeadzone;
            RightStickDeadZone = DEFAULT_rightStickDeadzone;
        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.DeadZoneSettings dshmLeftDZSettings = dshmContextSettings.DeadZoneLeft;
            DSHM_Format_ContextSettings.DeadZoneSettings dshmRightDZSettings = dshmContextSettings.DeadZoneRight;

            if (!this.IsGroupEnabled)
            {
                dshmLeftDZSettings = null;
                dshmRightDZSettings = null;
                return;
            }

            dshmLeftDZSettings.Apply = this.ApplyLeftStickDeadZone;
            dshmLeftDZSettings.PolarValue = (this.LeftStickDeadZone / 141.0) * 180; // Necessary do conversion

            dshmRightDZSettings.Apply = this.ApplyRightStickDeadZone;
            dshmRightDZSettings.PolarValue = (this.RightStickDeadZone / 141.0) * 180; // Necessary do conversion
        }

        public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.DeadZoneSettings dshmLeftDZSettings = dshmContextSettings.DeadZoneLeft;
            DSHM_Format_ContextSettings.DeadZoneSettings dshmRightDZSettings = dshmContextSettings.DeadZoneRight;

            if (dshmLeftDZSettings == null
                || dshmRightDZSettings == null)
            {
                this.isGroupEnabled = false;
            }

            dshmLeftDZSettings.Apply = this.ApplyLeftStickDeadZone;
            dshmLeftDZSettings.PolarValue = (this.LeftStickDeadZone / 141.0) * 180; // Necessary do conversion

            dshmRightDZSettings.Apply = this.ApplyRightStickDeadZone;
            dshmRightDZSettings.PolarValue = (this.RightStickDeadZone / 141.0) * 180; // Necessary do conversion


        }
    }


}
