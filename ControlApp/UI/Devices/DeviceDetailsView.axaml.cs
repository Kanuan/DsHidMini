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

 


    /*
    public class SettingsGroupDetails
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
    
       
        public SettingsGroupDetails(string iPropertyGroupName, string iSettingData, SettingsModeGroups settingsModeGroups)
        {
            IPropertyGroupName = iPropertyGroupName;
            ISettingData = iSettingData;
            SettingsMGroup = settingsModeGroups;
        }
   
    }
     */

        public partial class DeviceDetailsView : UserControl , INotifyPropertyChanged
    {
        // DeviceModesSettings devModeSettings = new();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        /*
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


     public bool OverrideCheckboxVisibility
     { get => (devModeSettings.currentSettingContext == SettingsContext.General) ? false : true;
         //set => OnPropertyChanged();
     }

         public ObservableCollection<SettingsGroupDetails> _modesSettingsBasicList = new ObservableCollection<SettingsGroupDetails>
     {
         new SettingsGroupDetails("LEDs customization","s",SettingsModeGroups.LEDsControl),
         new SettingsGroupDetails("Wireless settings","s",SettingsModeGroups.WirelessSettings),
         new SettingsGroupDetails("Sticks deadzone","Template_SticksDeadZone", SettingsModeGroups.SticksDeadzone),
         new SettingsGroupDetails("Rumble basic functions","s",SettingsModeGroups.RumbleBasicFunctions),
 };

     public ObservableCollection<SettingsGroupDetails> _ModesSettingsAdvancedList = new ObservableCollection<SettingsGroupDetails>
     {
         new SettingsGroupDetails("Output report control","s",SettingsModeGroups.OutputReportControl),
         new SettingsGroupDetails("Rumble strength control","s",SettingsModeGroups.RumbleHeavyStrRescale),
         new SettingsGroupDetails("Variable light rumble emulation adjustments","s",SettingsModeGroups.RumbleLightConversion),
 };


     public ObservableCollection<SettingsGroupDetails> ModeSettingsBasicList
     { get => _modesSettingsBasicList; set => _modesSettingsBasicList = value; }

     public ObservableCollection<SettingsGroupDetails> ModeSettingsAdvancedList
     { get => _ModesSettingsAdvancedList; set => _ModesSettingsAdvancedList = value; }
     */
        public DeviceDetailsView()
        {
            InitializeComponent();
            Nefarius.DsHidMini.ControlApp.MVVM.TestViewModel testViewModel = new();
            this.DataContext = testViewModel;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


    }
}
