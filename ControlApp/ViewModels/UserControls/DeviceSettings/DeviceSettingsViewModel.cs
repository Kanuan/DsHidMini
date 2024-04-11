﻿using Nefarius.DsHidMini.ControlApp.Models.DshmConfigManager;
using Nefarius.DsHidMini.ControlApp.Models.DshmConfigManager.Enums;

namespace Nefarius.DsHidMini.ControlApp.ViewModels.UserControls.DeviceSettings;

public abstract partial class DeviceSettingsViewModel : ObservableObject
{
    /// Replace with LexLoc
    private static Dictionary<SettingsModeGroups, string> DictGroupHeader = new()
    {
        { SettingsModeGroups.LEDsControl, "LEDs control" },
        { SettingsModeGroups.WirelessSettings, "Wireless" },
        { SettingsModeGroups.SticksDeadzone, "Sticks DeadZone" },
        { SettingsModeGroups.RumbleGeneral, "Rumble" },
        { SettingsModeGroups.OutputReportControl, "Output report control" },
        { SettingsModeGroups.RumbleLeftStrRescale, "Left motor rescale" },
        { SettingsModeGroups.RumbleRightConversion, "Alternative rumble mode adjuster" },
        { SettingsModeGroups.Unique_All, "HID Mode" },
        { SettingsModeGroups.Unique_Global, "Default settings" },
        { SettingsModeGroups.Unique_General, "General settings" },
        { SettingsModeGroups.Unique_SDF, "SDF mode specific settings" },
        { SettingsModeGroups.Unique_GPJ, "GPJ mode specific settings" },
        { SettingsModeGroups.Unique_SXS, "SXS mode specific settings" },
        { SettingsModeGroups.Unique_DS4W, "DS4W mode specific settings" },
        { SettingsModeGroups.Unique_XInput, "GPJ mode specific settings" },
    };

    [ObservableProperty] private List<string> _controlAppProfiles = new List<string>() { "profile 1", "profile 2", "profile 3" };

    public abstract SettingsModeGroups Group { get; }

    protected abstract IDeviceSettings _myInterface { get; }

    [ObservableProperty] private bool _isGroupLocked = false;

    public string? Header { get; }

    [RelayCommand]
    public void ResetGroupToOriginalDefaults()
    {
        _myInterface.ResetToDefault();
        NotifyAllPropertiesHaveChanged();
    }

    public virtual void NotifyAllPropertiesHaveChanged()
    {
        this.OnPropertyChanged(string.Empty);
    }

    public void LoadSettingsFromBackingDataContainer(Models.DshmConfigManager.DeviceSettings dataContainerSource)
    {
        _myInterface.CopySettingsFromContainer(dataContainerSource);
        NotifyAllPropertiesHaveChanged();
    }

    public void SaveSettingsToBackingDataContainer(Models.DshmConfigManager.DeviceSettings dataContainerSource)
    {
        _myInterface.CopySettingsToContainer(dataContainerSource);
    }

    public DeviceSettingsViewModel()
    {
        if (DictGroupHeader.TryGetValue(Group, out string groupHeader)) Header = groupHeader;
        //LoadSettingsFromBackingDataContainer(backingDataContainer);
    }
}

public partial class ButtonComboViewModel : ObservableObject
{
    private ButtonsCombo _buttonCombo;

    public bool IsEnabled
    {
        get => _buttonCombo.IsEnabled;
        set
        {
            _buttonCombo.IsEnabled = value;
            OnPropertyChanged(nameof(IsEnabled));
        }
    }

    public int HoldTime
    {
        get => _buttonCombo.HoldTime;
        set
        {
            _buttonCombo.HoldTime = value;
            OnPropertyChanged(nameof(IsEnabled));
        }
    }

    public int MinHoldTime { get; }

    public Button Button1
    {
        get => _buttonCombo.Button1;
        set
        {
            if (value != _buttonCombo.Button2 && value != _buttonCombo.Button3)
                _buttonCombo.Button1 = value;
            OnPropertyChanged(nameof(Button1));
        }
    }
    public Button Button2
    {
        get => _buttonCombo.Button2;
        set
        {
            if (value != _buttonCombo.Button1 && value != _buttonCombo.Button3)
                _buttonCombo.Button2 = value;
            OnPropertyChanged(nameof(Button2));
        }
    }
    public Button Button3
    {
        get => _buttonCombo.Button3;
        set
        {
            if (value != _buttonCombo.Button1 && value != _buttonCombo.Button2)
                _buttonCombo.Button3 = value;
            OnPropertyChanged(nameof(Button3));
        }
    }

    public ButtonComboViewModel(ButtonsCombo buttonsCombo, int minHoldTime)
    {
        _buttonCombo = buttonsCombo;
        MinHoldTime = minHoldTime;
    }

    public void NotifyAllPropertiesChanged()
    {
        this.OnPropertyChanged(nameof(string.Empty));
    }
}