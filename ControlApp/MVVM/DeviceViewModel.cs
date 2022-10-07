using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Threading;
using FontAwesome5;
using Nefarius.DsHidMini.ControlApp.Drivers;
using Nefarius.DsHidMini.ControlApp.Util;
using Nefarius.DsHidMini.ControlApp.Util.Web;
using Nefarius.Utilities.DeviceManagement.PnP;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControlApp.UI.Devices;
using System.Collections.ObjectModel;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class DeviceViewModel : ObservableObject
    {
        private readonly Timer _batteryQuery;
        private readonly PnPDevice _device;

        public DeviceViewModel(PnPDevice device)
        {
            _device = device;

            DeviceModesSettings GeneralSettings = new();

            _batteryQuery = new Timer(UpdateBatteryStatus, null, 10000, 10000);

        }


        private ObservableCollection<SettingTabViewModel> _settingsTabs;
        private SettingTabViewModel _currentTab;


        /*
        public bool MuteDigitalPressureButtons
        {
            get => _device.GetProperty<byte>(DsHidMiniDriver.MuteDigitalPressureButtonsProperty) > 0;
            set
            {
                using (var evt = EventWaitHandle.OpenExisting(
                           $"Global\\DsHidMiniConfigHotReloadEvent{DeviceAddress}"
                       ))
                {
                    _device.SetProperty(DsHidMiniDriver.MuteDigitalPressureButtonsProperty, (byte)(value ? 1 : 0));

                    evt.Set();
                }
            }
        }
        */

        public bool IsHidModeChangeable =>
            SecurityUtil.IsElevated /*&& HidEmulationMode != DsHidDeviceMode.XInputHIDCompatible*/;

        /// <summary>
        ///     Current HID device emulation mode.
        /// </summary>
        public DsHidDeviceMode HidEmulationMode
        {
            get =>
                (DsHidDeviceMode)_device.GetProperty<byte>(
                    DsHidMiniDriver.HidDeviceModeProperty);
            set
            {
                _device.SetProperty(DsHidMiniDriver.HidDeviceModeProperty, (byte)value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPressureMutingSupported)));
            }
        }

        /*
        public bool IsOutputRateControlEnabled
        {
            get => _device.GetProperty<byte>(DsHidMiniDriver.IsOutputRateControlEnabledProperty) > 0;
            set => _device.SetProperty(DsHidMiniDriver.IsOutputRateControlEnabledProperty, (byte)(value ? 1 : 0));
        }

        public byte OutputRateControlPeriodMs
        {
            get => _device.GetProperty<byte>(DsHidMiniDriver.OutputRateControlPeriodMsProperty);
            set => _device.SetProperty(DsHidMiniDriver.OutputRateControlPeriodMsProperty, value);
        }

        public bool IsOutputDeduplicatorEnabled
        {
            get => _device.GetProperty<byte>(DsHidMiniDriver.IsOutputDeduplicatorEnabledProperty) > 0;
            set => _device.SetProperty(DsHidMiniDriver.IsOutputDeduplicatorEnabledProperty, (byte)(value ? 1 : 0));
        }

        public uint WirelessIdleTimeoutPeriodMs
        {
            get => _device.GetProperty<uint>(DsHidMiniDriver.WirelessIdleTimeoutPeriodMsProperty) / 60000;
            set => _device.SetProperty(DsHidMiniDriver.WirelessIdleTimeoutPeriodMsProperty, value * 60000);
        }
        */

        /// <summary>
        ///     The device Instance ID.
        /// </summary>
        public string InstanceId => _device.InstanceId;

        /// <summary>
        ///     The Bluetooth MAC address of this device.
        /// </summary>
        public string DeviceAddress => _device.GetProperty<string>(DsHidMiniDriver.DeviceAddressProperty).ToUpper();

        /// <summary>
        ///     The Bluetooth MAC address of this device.
        /// </summary>
        public string DeviceAddressFriendly
        {
            get
            {
                var friendlyAddress = DeviceAddress;

                var insertedCount = 0;
                for (var i = 2; i < DeviceAddress.Length; i = i + 2)
                    friendlyAddress = friendlyAddress.Insert(i + insertedCount++, ":");

                return friendlyAddress;
            }
        }

        /// <summary>
        ///     The Bluetooth MAC address of the host radio this device is currently paired to.
        /// </summary>
        public string HostAddress
        {
            get
            {
                var hostAddress = _device.GetProperty<ulong>(DsHidMiniDriver.HostAddressProperty).ToString("X12")
                    .ToUpper();

                var friendlyAddress = hostAddress;

                var insertedCount = 0;
                for (var i = 2; i < hostAddress.Length; i = i + 2)
                    friendlyAddress = friendlyAddress.Insert(i + insertedCount++, ":");

                return friendlyAddress;
            }
        }

        /// <summary>
        ///     Current battery status.
        /// </summary>
        public DsBatteryStatus BatteryStatus =>
            (DsBatteryStatus)_device.GetProperty<byte>(DsHidMiniDriver.BatteryStatusProperty);

        /// <summary>
        ///     Return a battery icon depending on the charge.
        /// </summary>
        public EFontAwesomeIcon BatteryIcon
        {
            get
            {
                switch (BatteryStatus)
                {
                    case DsBatteryStatus.Charged:
                    case DsBatteryStatus.Charging:
                    case DsBatteryStatus.Full:
                        return EFontAwesomeIcon.Solid_BatteryFull;
                    case DsBatteryStatus.High:
                        return EFontAwesomeIcon.Solid_BatteryThreeQuarters;
                    case DsBatteryStatus.Medium:
                        return EFontAwesomeIcon.Solid_BatteryHalf;
                    case DsBatteryStatus.Low:
                        return EFontAwesomeIcon.Solid_BatteryQuarter;
                    case DsBatteryStatus.Dying:
                        return EFontAwesomeIcon.Solid_BatteryEmpty;
                    default:
                        return EFontAwesomeIcon.Solid_BatteryEmpty;
                }
            }
        }

        public EFontAwesomeIcon LastPairingStatusIcon
        {
            get
            {
                var ntstatus = _device.GetProperty<int>(DsHidMiniDriver.LastPairingStatusProperty);

                return ntstatus == 0
                    ? EFontAwesomeIcon.Regular_CheckCircle
                    : EFontAwesomeIcon.Solid_ExclamationTriangle;
            }
        }

        public EFontAwesomeIcon GenuineIcon
        {
            get
            {
                if (Validator.IsGenuineAddress(PhysicalAddress.Parse(DeviceAddress)))
                    return EFontAwesomeIcon.Regular_CheckCircle;
                return EFontAwesomeIcon.Solid_ExclamationTriangle;
            }
        }

        /// <summary>
        ///     The friendly (product) name of this device.
        /// </summary>
        public string DisplayName
        {
            get
            {
                var name = _device.GetProperty<string>(DevicePropertyKey.Device_FriendlyName);

                return string.IsNullOrEmpty(name) ? "DS3 Compatible HID Device" : name;
            }
        }

        public bool IsWireless
        {
            get
            {
                var enumerator = _device.GetProperty<string>(DevicePropertyKey.Device_EnumeratorName);

                return !enumerator.Equals("USB", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        /// <summary>
        ///     The connection protocol used by this device.
        /// </summary>
        public EFontAwesomeIcon ConnectionType =>
            !IsWireless
                ? EFontAwesomeIcon.Brands_Usb
                : EFontAwesomeIcon.Brands_Bluetooth;

        /// <summary>
        ///     Last time this device has been seen connected (applies to Bluetooth connected devices only).
        /// </summary>
        public DateTimeOffset LastConnected =>
            _device.GetProperty<DateTimeOffset>(DsHidMiniDriver.BluetoothLastConnectedTimeProperty);

        public bool IsPressureMutingSupported =>
            HidEmulationMode is DsHidDeviceMode.Single or DsHidDeviceMode.Multi;

        public event PropertyChangedEventHandler PropertyChanged;

        private void UpdateBatteryStatus(object state)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BatteryStatus"));
        }

        /// <summary>
        ///     Apply changes by requesting device restart.
        /// </summary>
        public void ApplyChanges()
        {
            _device.Restart();
        }


    }

    public class DeviceModesSettings : ObservableObject
    {

        public SettingsContext currentSettingContext { get; set; } = SettingsContext.DS4W;

        private bool isGroupSticksDeadzoneEnabled = true;
        public bool IsGroupSticksDeadzoneEnabled { get => isGroupSticksDeadzoneEnabled; set => isGroupSticksDeadzoneEnabled = value; }
        public bool IsEnable_GroupLEDsCustomizationEnabled { get => isEnable_GroupLEDsCustomizationEnabled; set => isEnable_GroupLEDsCustomizationEnabled = value; }

        private bool isEnable_GroupLEDsCustomizationEnabled = true;
        public bool IsGroupWirelessSettingsEnabled { get; set; }
        private bool isGroupWirelessSettingsEnable = true;

        private bool isEnable_GroupRumbleBasicEnabled = true;
        public bool IsGroupRumbleBasicEnabled { get; set; }

        public bool IsGroupOutRepControlEnabled { get; set; }
        private bool isEnable_GroupOutRepControlEnabled = true;
        public bool IsGroupRumbleStrEnabled { get; set; }
        private bool isEnable_GroupRumbleStrEnabled = true;
        public bool IsGroupRumbleLightConversionEnabled { get; set; }
        private bool isGroupRumbleLightConversionEnabled = true;

        private LEDsModes ledMode = LEDsModes.BatterySingleLED;
        public LEDsModes LEDMode { get => ledMode; set => ledMode = value; }

        private LEDCustoms[] LEDsCustoms = new LEDCustoms[4]
        {
            new LEDCustoms(), new LEDCustoms(), new LEDCustoms(), new LEDCustoms(),
        };

        public bool isWirelessIdleDisconnectDisabled = false;
        public bool IsWirelessIdleDisconnectDisabled { get; set; }

        public byte WirelessIdleDisconnectTime { get; set; }

        public QuickDisconnectCombo DisconnectCombo { get; set; }
        public byte DisconnectComboHoldTime { get; set; }

        public bool ApplyLeftStickDeadzone { get; set; }
        public bool ApplyRightStickDeadzone { get; set; }
        public double LeftStickDeadzone { get; set; }
        public double RightStickDeadzone { get; set; }
        public bool IsVariableLightRumbleEmulationEnabled { get; set; }

        public byte RightRumbleConversionUpperRange { get; set; }
        public byte RightRumbleConversionLowerRange { get; set; }
        public bool IsForcedRightMotorLightThresholdEnabled { get; set; }
        public bool IsForcedRightMotorHeavyThreasholdEnabled { get; set; }
        public byte ForcedRightMotorLightThreshold { get; set; }
        public byte ForcedRightMotorHeavyThreshold { get; set; }


        public bool IsLeftMotorEnabled { get; set; }
        public bool IsRightMotorEnabled { get; set; }
        public bool IsOutputReportRateControlEnabled { get; set; }
        public byte MaxOutputRate { get; set; }
        public bool IsOutputReportDeduplicatorEnabled { get; set; }
        public bool IsLeftMotorStrRescalingEnabled { get; set; }
        public byte LeftMotorStrRescalingUpperRange { get; set; }
        public byte LeftMotorStrRescalingLowerRange { get; set; }

        public DsPressureExposureMode PressureExposureMode { get; set; } = DsPressureExposureMode.DsPressureExposureModeBoth;
        public DS_DPAD_EXPOSURE_MODE PadExposureMode { get; set; } = DS_DPAD_EXPOSURE_MODE.DsDPadExposureModeHAT;

    }

    public class LEDCustoms
    {
        private byte isLEDEnabled = 0x10;
        public bool IsLEDEnabled
        {
            get => (isLEDEnabled == 0x10) ? true : false;
            set
            {
                if (value) isLEDEnabled = 0x10;
                isLEDEnabled = 0x00; // ????

            }
        }

        public byte Duration { get; set; } = 0xFF;
        public byte IntervalDuration { get; set; } = 0xFF;
        public byte IntervalPortionON { get; set; } = 0xFF;
        public byte IntervalPortionOFF { get; set; } = 0x00;
    }

    public enum SettingsContext
    {
        Global,
        General,
        SDF,
        GPJ,
        DS4W,
        XInput,
    }

    public enum SettingsModeGroups
    {
        LEDsControl,
        /// <summary>
        ///     WirelessIdleTimeoutPeriodMs  <br/>
        ///     QuickDisconnectCombo  <br/>
        ///     QuickDisconnectHoldTime  <br/>
        /// 	DisableWirelessIdleTimeout  <br/>
        /// </summary>
        WirelessSettings,
        /// <summary>
        /// 	IsOutputRateControlEnabled  <br/>
        /// 	OutputRateControlPeriodMs  <br/>
        /// 	IsOutputDeduplicatorEnabled  <br/>
        /// </summary>
        OutputReportControl,
        /// <summary>
        /// 	ThumbSettings.DeadZoneLeft.Apply <br/>
        ///     ThumbSettings.DeadZoneLeft.PolarValue <br/>
        ///     ThumbSettings.DeadZoneRight.Apply <br/>
        ///     ThumbSettings.DeadZoneRight.PolarValue <br/>
        /// </summary>
        SticksDeadzone,
        /// <summary>
        /// 	RumbleSettings.SMToBMConversion.Enabled <br/>
        /// 	RumbleSettings.DisableBM <br/>
        ///     RumbleSettings.DisableSM <br/>
        /// </summary>
        RumbleBasicFunctions,
        /// <summary>
        /// 	RumbleSettings.BMStrRescale.Enabled <br/>
        ///     RumbleSettings.BMStrRescale.MinValue <br/>
        ///     RumbleSettings.BMStrRescale.MaxValue <br/>
        /// </summary>
        RumbleHeavyStrRescale,
        /// <summary>
        ///     RumbleSettings.SMToBMConversion.RescaleMinValue <br/>
        ///     RumbleSettings.SMToBMConversion.RescaleMaxValue <br/>
        ///     RumbleSettings.ForcedSM.BMThresholdEnabled <br/>
        ///     RumbleSettings.ForcedSM.BMThresholdValue <br/>
        ///     RumbleSettings.ForcedSM.SMThresholdEnabled <br/>
        ///     RumbleSettings.ForcedSM.SMThresholdValue <br/>
        /// </summary>
        RumbleLightConversion,
        /// <summary>
        ///     SDF.PressureExposureMode <br/>
        ///     SDF.DPadExposureMode <br/>
        /// </summary>
        Unique_SDF,
        /// <summary>
        /// 	GPJ.PressureExposureMode <br/>
        ///     GPJ.DPadExposureMode <br/>
        /// </summary>
        Unique_GPJ,
        Unique_DS4W,
        Unique_XInput,
    }

    public enum LEDsModes
    {
        BatterySingleLED,
        BatteryFillingBar,
        CustomSimple,
        CustomComplete,
    }

    public enum QuickDisconnectCombo
    {
        Disabled,
        PS_R1_L1,
        PS_Start,
        PS_Select,
        Start_R1_L1,
        Select_R1_L1,
        Start_Select,
    }

    public enum DsPressureExposureMode
    {
        DsPressureExposureModeDigital,
        DsPressureExposureModeAnalogue,
        DsPressureExposureModeBoth,
    }

    public enum DS_DPAD_EXPOSURE_MODE
    {
        DsDPadExposureModeHAT,
        DsDPadExposureModeIndividualButtons,
        DsDPadExposureModeBoth,
    }
}