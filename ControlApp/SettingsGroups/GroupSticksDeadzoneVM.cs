using Nefarius.DsHidMini.ControlApp.JsonSettings;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupSticksDeadzoneVM : GroupSettingsVM
    {
        // -------------------------------------------- STICKS DEADZONE GROUP
        public const bool DEFAULT_applyLeftStickDeadzone = true;
        public const bool DEFAULT_applyRightStickDeadzone = true;
        public const int DEFAULT_leftStickDeadzone = 0;
        public const int DEFAULT_rightStickDeadzone = 0;

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.SticksDeadzone;

        public override bool IsOverrideCheckboxVisible
        {
            get
            {
                if (IsGroupLocked) return false;
                return base.IsOverrideCheckboxVisible;
            }

            set => base.IsOverrideCheckboxVisible = value;
        }

        public override bool IsGroupLocked
        {
            get
            {
                if (SettingsContainer.GroupModeUnique.PreventRemappingConflictsInSXSMode
                    || Context == SettingsContext.DS4W)
                    return true;
                return base.IsGroupLocked;
            }
            set
            {
                base.IsGroupLocked = value;
                this.RaisePropertyChanged(nameof(IsGroupLocked));
            }
        }

        [Reactive] public bool IsGroupEnabled { get; set; }
        [Reactive] public bool ApplyLeftStickDeadZone { get; set; }
        [Reactive] public bool ApplyRightStickDeadZone { get; set; }
        [Reactive] public byte LeftStickDeadZone { get; set; }
        [Reactive] public byte RightStickDeadZone { get; set; }

        readonly ObservableAsPropertyHelper<int> leftStickDeadZoneInpercent;
        public int LeftStickDeadZoneInPercent => leftStickDeadZoneInpercent.Value;

        readonly ObservableAsPropertyHelper<int> rightStickDeadZoneInpercent;
        public int RightStickDeadZoneInPercent => rightStickDeadZoneInpercent.Value;


        public GroupSticksDeadzoneVM(SettingsContext context, SettingsContainer containter) : base(context, containter)
        {


            AdjustSettingsBasedOnContext();
            leftStickDeadZoneInpercent = this
                .WhenAnyValue(x => x.LeftStickDeadZone)
                .Select(LeftStickDeadZone => LeftStickDeadZone * 141 / 180)
                .ToProperty(this, x => x.LeftStickDeadZoneInPercent);

            rightStickDeadZoneInpercent = this
                .WhenAnyValue(x => x.RightStickDeadZone)
                .Select(RightStickDeadZone => RightStickDeadZone * 141 / 180)
                .ToProperty(this, x => x.RightStickDeadZoneInPercent);

        }

        public override void ChangeContext(SettingsContext context)
        {
            base.ChangeContext(context);
            this.RaisePropertyChanged(nameof(IsOverrideCheckboxVisible));
            this.RaisePropertyChanged(nameof(IsGroupLocked));
            AdjustSettingsBasedOnContext();
        }

        public void AdjustSettingsBasedOnContext()
        {
            /* Unecessary anymore?
            if (Context == SettingsContext.DS4W)
            {
                IsOverrideCheckboxVisible = false;
                IsGroupLocked = true;
            }
            */
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

            if (this.Context == SettingsContext.DS4W
                || SettingsContainer.GroupModeUnique.PreventRemappingConflictsInSXSMode)
            {
                dshmLeftDZSettings.Apply = dshmRightDZSettings.Apply = false;
                return;
            }

            if (!this.IsGroupEnabled)
            {
                dshmLeftDZSettings = null;
                dshmRightDZSettings = null;
                return;
            }

            dshmLeftDZSettings.Apply = this.ApplyLeftStickDeadZone;
            dshmLeftDZSettings.PolarValue = this.LeftStickDeadZone;

            dshmRightDZSettings.Apply = this.ApplyRightStickDeadZone;
            dshmRightDZSettings.PolarValue = this.RightStickDeadZone;
        }

        public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.DeadZoneSettings dshmLeftDZSettings = dshmContextSettings.DeadZoneLeft;
            DSHM_Format_ContextSettings.DeadZoneSettings dshmRightDZSettings = dshmContextSettings.DeadZoneRight;

            // Checks if the "prevent conflicts" properties exist
            // They will only exist if the settings were saved in their respective HID Device Mode
            // If they exist and are true then we can skip the rest of the loading because this group will be disabled
            bool preventDS4WConflicts = dshmContextSettings.PreventRemappingConflitsInDS4WMode == null ? true : dshmContextSettings.PreventRemappingConflitsInSXSMode.GetValueOrDefault();
            bool preventSXSConflicts = dshmContextSettings.PreventRemappingConflitsInSXSMode == null ? false : dshmContextSettings.PreventRemappingConflitsInSXSMode.GetValueOrDefault();
            if (preventDS4WConflicts
                || preventSXSConflicts)
            {
                return;
            }


            if (dshmLeftDZSettings == null
                || dshmRightDZSettings == null)
            {
                this.IsGroupEnabled = false;
                return;
            }

            // left
            this.ApplyLeftStickDeadZone = dshmLeftDZSettings.Apply.GetValueOrDefault();
            this.LeftStickDeadZone = dshmLeftDZSettings.PolarValue.GetValueOrDefault();

            // Right
            this.ApplyRightStickDeadZone = dshmRightDZSettings.Apply.GetValueOrDefault();
            this.RightStickDeadZone = dshmRightDZSettings.PolarValue.GetValueOrDefault();


        }
    }


}
