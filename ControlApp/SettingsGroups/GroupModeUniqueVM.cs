using Nefarius.DsHidMini.ControlApp.DSHM_JsonData_Json;
using Nefarius.DsHidMini.ControlApp.UserData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.ComponentModel;
using System.Reactive.Linq;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupModeUniqueVM : GroupSettingsVM
    {
        private BackingData_ModesUnique _tempBackingData = new();

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.Unique_All;

        public bool IsGroupEnabled
        {
            get => _tempBackingData.IsGroupEnabled;
            set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.IsGroupEnabled, value);
            }
        }
        // General
        // public DSHM_HidDeviceModes HIDDeviceMode { get => _tempBackingData.; set => this.RaiseAndSetIfChanged(ref _tempBackingData.IsGroupEnabled, value); }

        // SDF and GPJ
        readonly ObservableAsPropertyHelper<bool> arePressureaNDPadOptionsVisible;
        public bool ArePressureaNDPadOptionsVisible => arePressureaNDPadOptionsVisible.Value;

        public ControlApp_DsPressureMode PressureExposureMode
        {
            get => _tempBackingData.PressureExposureMode;
            set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.PressureExposureMode, value);
            }
        }
        public ControlApp_DPADModes DPadExposureMode
        {
            get => _tempBackingData.DPadExposureMode;
            set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.DPadExposureMode, value);
            }
        }

        // SXS 
        public bool AreSXSRelatedOptionsVisible
        {
            get => Context == SettingsContext.SXS ? true : false;
        }
        public bool PreventRemappingConflictsInSXSMode
        {
            get => _tempBackingData.PreventRemappingConflictsInSXSMode;
            set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.PreventRemappingConflictsInSXSMode, value);
            }
        }

        // XInput
        public bool IsLEDsAsXInputSlotEnabled
        {
            get => _tempBackingData.IsLEDsAsXInputSlotEnabled;
            set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.IsLEDsAsXInputSlotEnabled, value);
            }
        }

        // DS4Windows
        public bool IsDS4LightbarTranslationEnabled
        {
            get => _tempBackingData.IsDS4LightbarTranslationEnabled;
            set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.IsDS4LightbarTranslationEnabled, value);
            }
        }

        public GroupModeUniqueVM(BackingDataContainer backingDataContainer, VMGroupsContainer vmGroupsContainter) : base(backingDataContainer, vmGroupsContainter)
        {
            arePressureaNDPadOptionsVisible = this
                .WhenAnyValue(x => x.Context)
                .Select(ArePressureaNDPadOptionsVisible => (( Context == SettingsContext.SDF ) || (Context == SettingsContext.GPJ)) ? true : false)
                .ToProperty(this, x => x.ArePressureaNDPadOptionsVisible);
        }

        public override void ChangeContext(SettingsContext context)
        {
            base.ChangeContext(context);
            _tempBackingData.SettingsContext = context;

        }

        public override void ResetGroupToOriginalDefaults()
        {
            _tempBackingData.ResetToDefault();
            this.RaisePropertyChanged(string.Empty);
        }

        public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            LoadSettingsFromBackingData(dataContainerSource.modesUniqueData);
        }

        public void LoadSettingsFromBackingData(BackingData_ModesUnique dataTarget)
        {
            BackingData_ModesUnique.CopySettings(_tempBackingData, dataTarget);
            this.RaisePropertyChanged(string.Empty);
        }

        public override void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            SaveSettingsToBackingData(dataContainerSource.modesUniqueData);
        }

        public void SaveSettingsToBackingData(BackingData_ModesUnique dataSource)
        {
            BackingData_ModesUnique.CopySettings(dataSource, _tempBackingData);
        }




    }


}
