﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PMSWPF.Models;
using S7.Net; // Add this using directive

namespace PMSWPF.ViewModels.Dialogs;

public partial class DeviceDialogViewModel : ObservableObject
{
    [ObservableProperty]
    private Device _device;
    partial void OnDeviceChanged(Device value)
    {
        if (value != null)
        {
            System.Diagnostics.Debug.WriteLine($"Device ProtocolType changed to: {value.ProtocolType}");
        }
    }
    
    [ObservableProperty] private string title ;
    [ObservableProperty] private string primaryButContent ;

    public DeviceDialogViewModel(Device device)
    {
        _device = device;
    }

    // Add a property to expose CpuType enum values for ComboBox
    public Array CpuTypes => Enum.GetValues(typeof(CpuType));


    [RelayCommand]
    public void AddDevice()
    {

    }
}