using Nefarius.DsHidMini.ControlApp.JsonSettings;
using Nefarius.DsHidMini.ControlApp.UserData;
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

        /*
        public override bool IsOverrideCheckboxVisible
        {
            get
            {
                if (IsGroupLocked) return false;
                return base.IsOverrideCheckboxVisible;
            }

            set => base.IsOverrideCheckboxVisible = value;
        }
        */

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
            //this.RaisePropertyChanged(nameof(IsOverrideCheckboxVisible));
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




        public override void CopySettingsFromBackingData(SettingsBackingData data, bool invertCopyDirection = false)
        {
            base.CopySettingsFromBackingData(data, invertCopyDirection);
            var specific = (BackingData_SticksDZ)data;

            if(invertCopyDirection)
            {
                specific.IsGroupEnabled = this.IsGroupEnabled;

                specific.ApplyRightStickDeadZone = this.ApplyRightStickDeadZone;
                specific.ApplyLeftStickDeadZone = this.ApplyLeftStickDeadZone;
                specific.LeftStickDeadZone = this.LeftStickDeadZone;
                specific.RightStickDeadZone = this.RightStickDeadZone;
            }
            else
            {
                this.IsGroupEnabled = specific.IsGroupEnabled;

                this.ApplyRightStickDeadZone = specific.ApplyRightStickDeadZone;
                this.ApplyLeftStickDeadZone = specific.ApplyLeftStickDeadZone;
                this.LeftStickDeadZone = specific.LeftStickDeadZone;
                this.RightStickDeadZone = specific.RightStickDeadZone;
            }

        }
    }
}
