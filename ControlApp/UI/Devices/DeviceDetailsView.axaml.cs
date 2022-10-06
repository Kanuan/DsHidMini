using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Templates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Linq;


namespace ControlApp.UI.Devices
{
    public enum SettingsContext
    {
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
        Specific_SDF,
        /// <summary>
        /// 	GPJ.PressureExposureMode <br/>
        ///     GPJ.DPadExposureMode <br/>
        /// </summary>
        Specific_GPJ,
        Specific_DS4W,
        Specific_XInput,
    }


    public class TemplateSelector : IDataTemplate
    {
        public IControl Build(object param)
        {
            string tempString = "";

            switch (param)
            {
                case SettingsModeGroups.LEDsControl:
                    tempString = "Template_LEDsSettings";
                    break;
                case SettingsModeGroups.WirelessSettings:
                    tempString = "Template_WirelessSettings";
                    break;
                case SettingsModeGroups.SticksDeadzone:
                    tempString = "Template_SticksDeadZone";
                    break;
                case SettingsModeGroups.RumbleBasicFunctions:
                    tempString = "Template_RumbleBasicFunctions";
                    break;
                case SettingsModeGroups.OutputReportControl:
                    tempString = "Template_OutputRateControl";
                    break;
                case SettingsModeGroups.RumbleHeavyStrRescale:
                    tempString = "Template_RumbleHeavyStrRescale";
                    break;
                case SettingsModeGroups.RumbleLightConversion:
                    tempString = "Template_RumbleVariableLightEmuTuning";
                    break;
                case SettingsModeGroups.Specific_SDF:
                case SettingsModeGroups.Specific_GPJ:
                case SettingsModeGroups.Specific_DS4W:
                case SettingsModeGroups.Specific_XInput:
                default:
                    tempString = "Test 3";
                    break;
            }

            var resultingCtrl = ((IDataTemplate)Application.Current.Resources[tempString]).Build(0);
            return resultingCtrl;
        }

        public bool Match(object data)
        {
            // Check if we can accept the provided data
            return data is SettingsModeGroups;
        }
    }

    public class ThisIsATest
    {

        public string IPropertyGroupName { get; set; }

        public string ISettingData { get; set; }
        public SettingsModeGroups settingsMGroup;
        public SettingsModeGroups SettingsMGroup
        {
            get => settingsMGroup;
            set
            {
                settingsMGroup = value;
            }
        }

        public ThisIsATest(string iPropertyGroupName, string iSettingData, SettingsModeGroups settingsModeGroups)
        {
            IPropertyGroupName = iPropertyGroupName;
            ISettingData = iSettingData;
            SettingsMGroup = settingsModeGroups;
        }
    }

    public class DeviceModesSettings
    {

        public SettingsContext currentSettingContext { get; set; } = SettingsContext.DS4W;

        private bool override_GroupSticksDeadzone = false;

        public bool Override_GroupSticksDeadzone { get => override_GroupSticksDeadzone; set => override_GroupSticksDeadzone = value; }
        /*
         * 
         * 
        Config->HidDeviceMode = DsHidMiniDeviceModeXInputHIDCompatible;
	Config->DisableAutoPairing = FALSE;

	Config->LEDSettings.Mode = DsLEDModeBatteryIndicatorPlayerIndex;

	const PDS_LED pPlayerSlots[] =
    {
        &Config->LEDSettings.CustomPatterns.Player1,
        &Config->LEDSettings.CustomPatterns.Player2,
        &Config->LEDSettings.CustomPatterns.Player3,
        &Config->LEDSettings.CustomPatterns.Player4,
    };

	for (ULONGLONG playerIndex = 0; playerIndex<_countof(pPlayerSlots); playerIndex++)
	{
		pPlayerSlots[playerIndex]->Duration = 0xFF;
		pPlayerSlots[playerIndex]->IntervalDuration = 0xFF;
		pPlayerSlots[playerIndex]->Enabled = 0x10;
		pPlayerSlots[playerIndex]->IntervalPortionOff = 0x00;
		pPlayerSlots[playerIndex]->IntervalPortionOn = 0xFF;
	}
        */
    }

    public partial class DeviceDetailsView : UserControl , INotifyPropertyChanged
    {
        DeviceModesSettings devModeSettings = new();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public int TestDeleteLater
        {
            get => LEDsMode;
            set
            {
                LEDsMode = value;
                OnPropertyChanged("LEDsMode");
            }
        }

        public int LEDsMode{ get; set; }

        public int CurrentContext
        {
            get => (int)devModeSettings.currentSettingContext;
            set
            {
                devModeSettings.currentSettingContext = (SettingsContext)value;
               OnPropertyChanged(String.Empty);
                //"OverrideCheckboxVisibility"
            }

        }

        public bool TestBool { get; set; } = false;

        public bool OverrideCheckboxVisibility
        { get => (devModeSettings.currentSettingContext == SettingsContext.General) ? false : true;
            //set => OnPropertyChanged();
        }

        public  ObservableCollection<ThisIsATest> _modesSettingsBasicList = new ObservableCollection<ThisIsATest>
        {
            new ThisIsATest("LEDs customization","s",SettingsModeGroups.LEDsControl),
            new ThisIsATest("Wireless settings","s",SettingsModeGroups.WirelessSettings),
            new ThisIsATest("Sticks deadzone","Template_SticksDeadZone", SettingsModeGroups.SticksDeadzone),
            new ThisIsATest("Rumble basic functions","s",SettingsModeGroups.RumbleBasicFunctions),
    };

        public ObservableCollection<ThisIsATest> _ModesSettingsAdvancedList = new ObservableCollection<ThisIsATest>
        {
            new ThisIsATest("Output report control","s",SettingsModeGroups.OutputReportControl),
            new ThisIsATest("Rumble strength control","s",SettingsModeGroups.RumbleHeavyStrRescale),
            new ThisIsATest("Variable light rumble emulation adjustments","s",SettingsModeGroups.RumbleLightConversion),
    };

        public ObservableCollection<ThisIsATest> ModeSettingsBasicList
        { get => _modesSettingsBasicList; set => _modesSettingsBasicList = value; }

        public ObservableCollection<ThisIsATest> ModeSettingsAdvancedList
        { get => _ModesSettingsAdvancedList; set => _ModesSettingsAdvancedList = value; }

        public DeviceDetailsView()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


    }
}
