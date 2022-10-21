using Nefarius.DsHidMini.ControlApp.DSHM_JsonData_Json;
using Nefarius.DsHidMini.ControlApp.UserData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupSticksDeadzoneVM : GroupSettingsVM
    {
        // -------------------------------------------- STICKS DEADZONE GROUP
        private BackingData_SticksDZ _tempBackingData = new();

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.SticksDeadzone;

        /*
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
        */

        public bool IsGroupEnabled
        {
            get => _tempBackingData.IsGroupEnabled; set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.IsGroupEnabled, value);
            }
        }

        public bool ApplyLeftStickDeadZone
        {
            get => _tempBackingData.ApplyLeftStickDeadZone; set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.ApplyLeftStickDeadZone, value);
            }
        }

        public bool ApplyRightStickDeadZone
        {
            get => _tempBackingData.ApplyRightStickDeadZone; set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.ApplyRightStickDeadZone, value);
            }
        }

        public int LeftStickDeadZone
        {
            get => _tempBackingData.LeftStickDeadZone; set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.LeftStickDeadZone, value);
            }
        }

        public int RightStickDeadZone
        {
            get => _tempBackingData.RightStickDeadZone; set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.RightStickDeadZone, value);
            }
        }


        readonly ObservableAsPropertyHelper<int> leftStickDeadZoneInpercent;
        public int LeftStickDeadZoneInPercent => leftStickDeadZoneInpercent.Value;

        readonly ObservableAsPropertyHelper<int> rightStickDeadZoneInpercent;


        public int RightStickDeadZoneInPercent => rightStickDeadZoneInpercent.Value;


        public GroupSticksDeadzoneVM(BackingDataContainer backingDataContainer, VMGroupsContainer vmGroupsContainter) : base(backingDataContainer, vmGroupsContainter)
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
            _tempBackingData.ResetToDefault();
            this.RaisePropertyChanged(string.Empty);
        }

        public override void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            SaveSettingsToBackingData(dataContainerSource.sticksDZData);
        }
        public void SaveSettingsToBackingData(BackingData_SticksDZ dataSource)
        {
            BackingData_SticksDZ.CopySettings(dataSource, _tempBackingData);
        }

        public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            LoadSettingsFromBackingData(dataContainerSource.sticksDZData);
        }

        public void LoadSettingsFromBackingData(BackingData_SticksDZ dataTarget)
        {
            BackingData_SticksDZ.CopySettings(_tempBackingData, dataTarget);
            this.RaisePropertyChanged(string.Empty);
        }
    }
}
