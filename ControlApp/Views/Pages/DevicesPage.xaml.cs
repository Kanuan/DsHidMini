﻿// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Nefarius.DsHidMini.ControlApp.ViewModels.Pages;

using Wpf.Ui.Controls;

namespace Nefarius.DsHidMini.ControlApp.Views.Pages;

public partial class DevicesPage : INavigableView<DevicesViewModel>
{
    public DevicesPage(DevicesViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }

    public DevicesViewModel ViewModel { get; }
}