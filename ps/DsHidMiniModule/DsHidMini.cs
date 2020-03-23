﻿using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Device.Net;
using Hid.Net.Windows;

namespace DsHidMiniModule
{
    [Cmdlet(VerbsCommon.Get, "HidDevice")]
    [OutputType(typeof(ConnectedDeviceDefinition))]
    public class GetHidDevice : Cmdlet
    {
        private static readonly DebugLogger Logger = new DebugLogger();
        private static readonly DebugTracer Tracer = new DebugTracer();

        [Parameter] public ushort VendorId { get; set; } = 0x054C;

        [Parameter] public ushort ProductId { get; set; } = 0x0268;

        protected override void ProcessRecord()
        {
            WindowsHidDeviceFactory.Register(Logger, Tracer);

            var deviceDefinitions = new List<FilterDeviceDefinition>
            {
                new FilterDeviceDefinition
                {
                    DeviceType = DeviceType.Hid, VendorId = VendorId, ProductId = ProductId
                }
            };

            var devices = DeviceManager.Current.GetDevicesAsync(deviceDefinitions).Result;

            foreach (var device in devices)
            {
                device.InitializeAsync().Wait();
            }

            devices.Select(d => d.ConnectedDeviceDefinition).ToList().ForEach(WriteObject);
        }
    }
}