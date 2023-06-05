using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
using Nefarius.DsHidMini.ControlApp.UserData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupSticksVM : GroupSettingsVM
    {
        // -------------------------------------------- STICKS DEADZONE GROUP
        private BackingData_Sticks _tempBackingData = new();

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.SticksDeadzone;

        public ObservableAsPropertyHelper<bool> isGroupLockedToPreventDS4WConflicts;
        
        /*
        public override bool IsGroupLocked
        {
            get
            {        
                if (isGroupLockedToPreventDS4WConflicts.Value)
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
            get => _tempBackingData.IsGroupEnabled;
            set
            {
                _tempBackingData.IsGroupEnabled = value;
                this.RaisePropertyChanged(nameof(IsGroupEnabled));
            }
        }

        public bool ApplyLeftStickDeadZone
        {
            get => _tempBackingData.LeftStickData.IsDeadZoneEnabled;
            set
            {
                _tempBackingData.LeftStickData.IsDeadZoneEnabled = value;
                this.RaisePropertyChanged(nameof(ApplyLeftStickDeadZone));
            }
        }

        public bool ApplyRightStickDeadZone
        {
            get => _tempBackingData.RightStickData.IsDeadZoneEnabled;
            set
            {
                _tempBackingData.RightStickData.IsDeadZoneEnabled = value;
                this.RaisePropertyChanged(nameof(ApplyRightStickDeadZone));
            }
        }

        public int LeftStickDeadZone
        {
            get => _tempBackingData.LeftStickData.DeadZone;
            set
            {
                _tempBackingData.LeftStickData.DeadZone = value;
                this.RaisePropertyChanged(nameof(LeftStickDeadZone));
            }
        }

        public int RightStickDeadZone
        {
            get => _tempBackingData.RightStickData.DeadZone;
            set
            {
                _tempBackingData.RightStickData.DeadZone = value;
                this.RaisePropertyChanged(nameof(RightStickDeadZone));
            }
        }

        public bool InvertLSX
        {
            get => _tempBackingData.LeftStickData.InvertXAxis;
            set
            {
                _tempBackingData.LeftStickData.InvertXAxis = value;
                this.RaisePropertyChanged(nameof(InvertLSX));
            }
        }

        public bool InvertLSY
        {
            get => _tempBackingData.LeftStickData.InvertYAxis;
            set
            {
                _tempBackingData.LeftStickData.InvertYAxis = value;
                this.RaisePropertyChanged(nameof(InvertLSY));
            }
        }

        public bool InvertRSX
        {
            get => _tempBackingData.RightStickData.InvertXAxis;
            set
            {
                _tempBackingData.RightStickData.InvertXAxis = value;
                this.RaisePropertyChanged(nameof(InvertRSX));
            }
        }

        public bool InvertRSY
        {
            get => _tempBackingData.RightStickData.InvertYAxis;
            set
            {
                _tempBackingData.RightStickData.InvertYAxis = value;
                this.RaisePropertyChanged(nameof(InvertRSY));
            }
        }

        readonly ObservableAsPropertyHelper<int> leftStickDeadZoneInpercent;
        public int LeftStickDeadZoneInPercent => leftStickDeadZoneInpercent.Value;

        readonly ObservableAsPropertyHelper<int> rightStickDeadZoneInpercent;
        public int RightStickDeadZoneInPercent => rightStickDeadZoneInpercent.Value;


        public GroupSticksVM(BackingDataContainer backingDataContainer, VMGroupsContainer vmGroupsContainter) : base(backingDataContainer, vmGroupsContainter)
        {
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
        }

        public override void ResetGroupToOriginalDefaults()
        {
            _tempBackingData.ResetToDefault();
            this.RaisePropertyChanged(string.Empty);
        }

        public override void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            SaveSettingsToBackingData(dataContainerSource.sticksData);
        }
        public void SaveSettingsToBackingData(BackingData_Sticks dataSource)
        {
            BackingData_Sticks.CopySettings(dataSource, _tempBackingData);
        }

        public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            LoadSettingsFromBackingData(dataContainerSource.sticksData);
        }

        public void LoadSettingsFromBackingData(BackingData_Sticks dataTarget)
        {
            BackingData_Sticks.CopySettings(_tempBackingData, dataTarget);
            this.RaisePropertyChanged(string.Empty);
        }
    }
}
