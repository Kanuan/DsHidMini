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
using Newtonsoft.Json.Linq;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class DeviceViewModel : ObservableObject
    {
        private readonly Timer _batteryQuery;
        private readonly PnPDevice _device;

        public DeviceViewModel(PnPDevice device)
        {
            _device = device;

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

        public SettingsContext CurrentSettingContext { get; set; } = SettingsContext.DS4W;

        private bool isGroupLEDsCustomizationEnabled = false;
        public bool isGroupWirelessSettingsEnabled = false;
        private bool isGroupSticksDeadzoneEnabled = false;
        private bool isGroupRumbleGeneralEnabled = false;
        private bool isGroupOutRepControlEnabled = false;
        private bool isGroupRumbleLeftRescaleEnabled = false;
        private bool isGroupRumbleRightConversionEnabled = false;

        private LEDsModes ledMode = LEDsModes.BatterySingleLED;
        public bool isWirelessIdleDisconnectDisabled = false;
        public byte wirelessIdleDisconnectTime;
        private QuickDisconnectCombo disconnectCombo;
        private byte disconnectComboHoldTime;
        private bool applyLeftStickDeadzone;

        public LEDCustoms currentLEDCustoms;

        private bool applyRightStickDeadzone;
        private double leftStickDeadzone;
        private double rightStickDeadzone;
        private bool isVariableLightRumbleEmulationEnabled;
        private byte rightRumbleConversionUpperRange;
        private byte rightRumbleConversionLowerRange;
        private bool isForcedRightMotorLightThresholdEnabled;
        private bool isForcedRightMotorHeavyThreasholdEnabled;
        private byte forcedRightMotorLightThreshold;
        private byte forcedRightMotorHeavyThreshold;
        private bool isLeftMotorEnabled;
        private bool isRightMotorEnabled;
        private bool isOutputReportRateControlEnabled;
        private byte maxOutputRate;
        private bool isOutputReportDeduplicatorEnabled;
        private bool isLeftMotorStrRescalingEnabled;
        private byte leftMotorStrRescalingUpperRange;
        private byte leftMotorStrRescalingLowerRange;
        private DsPressureExposureMode pressureExposureMode = DsPressureExposureMode.DsPressureExposureModeBoth;
        private DS_DPAD_EXPOSURE_MODE padExposureMode = DS_DPAD_EXPOSURE_MODE.DsDPadExposureModeHAT;


        // -------------------------------------------- LEDS GROUP

        public bool IsGroupLEDsCustomizationEnabled { get => isGroupLEDsCustomizationEnabled; set => SetProperty(ref isGroupLEDsCustomizationEnabled, value); }
        public LEDsModes LEDMode
        {
            get => ledMode;
            set => SetProperty(ref ledMode, value);
        }

        private LEDCustoms[] ledsCustoms = new LEDCustoms[4]
        {
            new LEDCustoms(0), new LEDCustoms(1), new LEDCustoms(2), new LEDCustoms(3),
        };

        public LEDCustoms[] LEDsCustoms  { get => ledsCustoms; set => SetProperty(ref ledsCustoms, value); }


        public LEDCustoms CurrentLEDCustoms { get => currentLEDCustoms; set => SetProperty(ref currentLEDCustoms, value); }
        public int CurrentLEDCustomsIndex
        {
            get => CurrentLEDCustoms.LEDIndex;
            set
            {
                CurrentLEDCustoms = LEDsCustoms[value];
                OnPropertyChanged("CurrentLEDCustomsIndex"); // Is this correct?
            }
        }



        // -------------------------------------------- WIRELESS SETTINGS GROUP

        public bool IsGroupWirelessSettingsEnabled { get => isGroupWirelessSettingsEnabled; set => SetProperty(ref isGroupWirelessSettingsEnabled, value); }
        public bool IsWirelessIdleDisconnectDisabled { get => isWirelessIdleDisconnectDisabled; set => SetProperty(ref isWirelessIdleDisconnectDisabled, value); }
        public byte WirelessIdleDisconnectTime { get => wirelessIdleDisconnectTime; set => SetProperty(ref wirelessIdleDisconnectTime, value); }
        public QuickDisconnectCombo DisconnectCombo { get => disconnectCombo; set => SetProperty(ref disconnectCombo, value); }
        public byte DisconnectComboHoldTime { get => disconnectComboHoldTime; set => SetProperty(ref disconnectComboHoldTime, value); }

        // -------------------------------------------- STICKS DEADZONE GROUP

        public bool IsGroupSticksDeadzoneEnabled { get => isGroupSticksDeadzoneEnabled; set => SetProperty(ref isGroupSticksDeadzoneEnabled, value); }
        public bool ApplyLeftStickDeadzone { get => applyLeftStickDeadzone; set => SetProperty(ref applyLeftStickDeadzone, value); }
        public bool ApplyRightStickDeadzone { get => applyRightStickDeadzone; set => SetProperty(ref applyRightStickDeadzone, value); }
        public double LeftStickDeadzone { get => leftStickDeadzone; set => SetProperty(ref leftStickDeadzone, value); }
        public double RightStickDeadzone { get => rightStickDeadzone; set => SetProperty(ref rightStickDeadzone, value); }

        // --------------------------------------------  GENERAL RUMBLE SETTINGS GROUP

        public bool IsGroupRumbleGeneralEnabled { get => isGroupRumbleGeneralEnabled; set => SetProperty(ref isGroupRumbleGeneralEnabled, value); }
        public bool IsVariableLightRumbleEmulationEnabled { get => isVariableLightRumbleEmulationEnabled; set => SetProperty(ref isVariableLightRumbleEmulationEnabled, value); }
        public bool IsLeftMotorEnabled { get => isLeftMotorEnabled; set => SetProperty(ref isLeftMotorEnabled, value); }
        public bool IsRightMotorEnabled { get => isRightMotorEnabled; set => SetProperty(ref isRightMotorEnabled, value); }

        // --------------------------------------------  OUTPUT REPORT CONTROL GROUP

        public bool IsGroupOutRepControlEnabled { get => isGroupOutRepControlEnabled; set => SetProperty(ref isGroupOutRepControlEnabled, value); }
        public bool IsOutputReportRateControlEnabled { get => isOutputReportRateControlEnabled; set => SetProperty(ref isOutputReportRateControlEnabled, value); }
        public byte MaxOutputRate { get => maxOutputRate; set => SetProperty(ref maxOutputRate, value); }
        public bool IsOutputReportDeduplicatorEnabled { get => isOutputReportDeduplicatorEnabled; set => SetProperty(ref isOutputReportDeduplicatorEnabled, value); }

        // -------------------------------------------- LEFT MOTOR RESCALING GROUP

        public bool IsGroupRumbleLeftRescaleEnabled { get => isGroupRumbleLeftRescaleEnabled; set => SetProperty(ref isGroupRumbleLeftRescaleEnabled, value); }
        public bool IsLeftMotorStrRescalingEnabled { get => isLeftMotorStrRescalingEnabled; set => SetProperty(ref isLeftMotorStrRescalingEnabled, value); }
        public byte LeftMotorStrRescalingUpperRange { get => leftMotorStrRescalingUpperRange; set => SetProperty(ref leftMotorStrRescalingUpperRange, value); }
        public byte LeftMotorStrRescalingLowerRange { get => leftMotorStrRescalingLowerRange; set => SetProperty(ref leftMotorStrRescalingLowerRange, value); }

        // -------------------------------------------- RIGHT MOTOR CONVERSION GROUP

        public bool IsGroupRumbleRightConversionEnabled { get => isGroupRumbleRightConversionEnabled; set => SetProperty(ref isGroupRumbleRightConversionEnabled, value); }
        public byte RightRumbleConversionUpperRange
        {
            get => rightRumbleConversionUpperRange;
            set
            {
                byte tempByte = ( value < RightRumbleConversionLowerRange ) ? (byte)(RightRumbleConversionLowerRange + 1 ) : value;
                SetProperty(ref rightRumbleConversionUpperRange, tempByte);
            }
        }
        public byte RightRumbleConversionLowerRange
        {
            get => rightRumbleConversionLowerRange;
            set
            {
                byte tempByte = (value > RightRumbleConversionUpperRange) ? (byte)(RightRumbleConversionUpperRange - 1) : value;
                SetProperty(ref rightRumbleConversionLowerRange, tempByte);
            }
        }
        public bool IsForcedRightMotorLightThresholdEnabled { get => isForcedRightMotorLightThresholdEnabled; set => SetProperty(ref isForcedRightMotorLightThresholdEnabled, value); }
        public bool IsForcedRightMotorHeavyThreasholdEnabled { get => isForcedRightMotorHeavyThreasholdEnabled; set => SetProperty(ref isForcedRightMotorHeavyThreasholdEnabled, value); }
        public byte ForcedRightMotorLightThreshold { get => forcedRightMotorLightThreshold; set => SetProperty(ref forcedRightMotorLightThreshold, value); }
        public byte ForcedRightMotorHeavyThreshold { get => forcedRightMotorHeavyThreshold; set => SetProperty(ref forcedRightMotorHeavyThreshold, value); }

        // -------------------------------------------- 

        public DsPressureExposureMode PressureExposureMode { get => pressureExposureMode; set => SetProperty(ref pressureExposureMode, value); }
        public DS_DPAD_EXPOSURE_MODE PadExposureMode { get => padExposureMode; set => SetProperty(ref padExposureMode, value); }

        // -------------------------------------------- 

        public DeviceModesSettings(SettingsContext settingsContext)
        {
            CurrentSettingContext = settingsContext;
            bool tempBool = (CurrentSettingContext == SettingsContext.General || CurrentSettingContext == SettingsContext.Global) ? true : false;
            IsGroupLEDsCustomizationEnabled =
                IsGroupWirelessSettingsEnabled =
                IsGroupSticksDeadzoneEnabled =
                IsGroupRumbleGeneralEnabled =
                IsGroupOutRepControlEnabled =
                IsGroupRumbleLeftRescaleEnabled =
                IsGroupRumbleRightConversionEnabled = tempBool;

            currentLEDCustoms = LEDsCustoms[0];

        }
    }

    public class LEDCustoms : ObservableObject
    {
        private int ledIndex;

        private byte isLEDEnabled = 0x10;
        private byte duration = 0xFF;
        private byte intervalDuration = 0xFF;
        private byte intervalPortionON = 0xFF;
        private byte intervalPortionOFF = 0x00;

        public bool IsLEDEnabled
        {
            get => (isLEDEnabled == 0x10) ? true : false;
            set
            {
                byte tempByte = value ? (byte)0x10 : (byte)0x00;
                SetProperty(ref isLEDEnabled, tempByte);

            }
        }

        public int LEDIndex { get => ledIndex; private set => SetProperty(ref ledIndex, value); }
        public byte Duration { get => duration; set => SetProperty(ref duration, value); }
        public byte IntervalDuration { get => intervalDuration; set => SetProperty(ref intervalDuration, value); }
        public byte IntervalPortionON { get => intervalPortionON; set => SetProperty(ref intervalPortionON, value); }
        public byte IntervalPortionOFF { get => intervalPortionOFF; set => SetProperty(ref intervalPortionOFF, value); }
        public LEDCustoms(int ledIndex)
        {
            this.ledIndex = ledIndex;
        }
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
        RumbleGeneral,
        /// <summary>
        /// 	RumbleSettings.BMStrRescale.Enabled <br/>
        ///     RumbleSettings.BMStrRescale.MinValue <br/>
        ///     RumbleSettings.BMStrRescale.MaxValue <br/>
        /// </summary>
        RumbleLeftStrRescale,
        /// <summary>
        ///     RumbleSettings.SMToBMConversion.RescaleMinValue <br/>
        ///     RumbleSettings.SMToBMConversion.RescaleMaxValue <br/>
        ///     RumbleSettings.ForcedSM.BMThresholdEnabled <br/>
        ///     RumbleSettings.ForcedSM.BMThresholdValue <br/>
        ///     RumbleSettings.ForcedSM.SMThresholdEnabled <br/>
        ///     RumbleSettings.ForcedSM.SMThresholdValue <br/>
        /// </summary>
        RumbleRightConversion,
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