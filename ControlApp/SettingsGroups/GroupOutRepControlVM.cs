using Nefarius.DsHidMini.ControlApp.JsonSettings;
using ReactiveUI.Fody.Helpers;
using System;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupOutRepControlVM : GroupSettingsVM
    {
        // --------------------------------------------  OUTPUT REPORT CONTROL GROUP
        public const bool DEFAULT_isOutputReportRateControlEnabled = true;
        public const byte DEFAULT_maxOutputRate = 150;
        public const bool DEFAULT_isOutputReportDeduplicatorEnabled = false;
        public override SettingsModeGroups Group { get; } = SettingsModeGroups.OutputReportControl;
        [Reactive] public bool IsGroupEnabled { get; set; }
        [Reactive] public bool IsOutputReportRateControlEnabled { get; set; }
        [Reactive] public byte MaxOutputRate { get; set; }
        [Reactive] public bool IsOutputReportDeduplicatorEnabled { get; set; }

        public GroupOutRepControlVM(SettingsContext context, SettingsContainer containter) : base(context, containter) { }

        public override void ResetGroupToOriginalDefaults()
        {
            IsGroupEnabled = ShouldGroupBeEnabledOnReset();

            IsOutputReportRateControlEnabled = DEFAULT_isOutputReportRateControlEnabled;
            MaxOutputRate = DEFAULT_maxOutputRate;
            IsOutputReportDeduplicatorEnabled = DEFAULT_isOutputReportDeduplicatorEnabled;
        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            if (!this.IsGroupEnabled)
            {
                dshmContextSettings.IsOutputRateControlEnabled = null;
                dshmContextSettings.OutputRateControlPeriodMs = null;
                dshmContextSettings.IsOutputDeduplicatorEnabled = null;
                return;
            }
            dshmContextSettings.IsOutputRateControlEnabled = this.IsOutputReportRateControlEnabled;
            dshmContextSettings.OutputRateControlPeriodMs = this.MaxOutputRate;
            dshmContextSettings.IsOutputDeduplicatorEnabled = this.IsOutputReportDeduplicatorEnabled;
        }

        public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            if(dshmContextSettings.IsOutputRateControlEnabled == null
                || dshmContextSettings.OutputRateControlPeriodMs == null
                || dshmContextSettings.IsOutputDeduplicatorEnabled == null)
            {
                this.IsGroupEnabled = false;
                return;
            }
            this.IsGroupEnabled = true;

            this.IsOutputReportRateControlEnabled = dshmContextSettings.IsOutputRateControlEnabled.GetValueOrDefault();
            this.MaxOutputRate = dshmContextSettings.OutputRateControlPeriodMs.GetValueOrDefault();
            this.IsOutputReportDeduplicatorEnabled = dshmContextSettings.IsOutputDeduplicatorEnabled.GetValueOrDefault();
        }
    }


}
