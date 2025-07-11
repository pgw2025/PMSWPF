﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PMSWPF.Data.Repositories;
using PMSWPF.Enums;
using PMSWPF.Helper;
using PMSWPF.Message;
using PMSWPF.Models;
using PMSWPF.ViewModels;
using SqlSugar;

namespace PMSWPF.Services;

/// <summary>
/// 数据服务类，负责从数据库加载和管理各种数据，并提供数据变更通知。
/// 继承自ObservableRecipient，可以接收消息；实现IRecipient<LoadMessage>，处理加载消息。
/// </summary>
public partial class DataServices : ObservableRecipient, IRecipient<LoadMessage>
{
    // 设备列表，使用ObservableProperty特性，当值改变时会自动触发属性变更通知。
    [ObservableProperty]
    private List<Device> _devices;

    // 变量表列表。
    [ObservableProperty]
    private List<VariableTable> _variableTables;

    // 变量数据列表。
    [ObservableProperty]
    private List<VariableData> _variableDatas;

    // 菜单树列表。
    [ObservableProperty]
    private List<MenuBean> menuTrees;

    // MQTT配置列表。
    [ObservableProperty]
    private List<Mqtt> _mqtts;

    // 设备数据仓库，用于设备数据的CRUD操作。
    private readonly DeviceRepository _deviceRepository;

    // 菜单数据仓库，用于菜单数据的CRUD操作。
    private readonly MenuRepository _menuRepository;

    // MQTT数据仓库，用于MQTT配置数据的CRUD操作。
    private readonly MqttRepository _mqttRepository;

    // 变量数据仓库，用于变量数据的CRUD操作。
    private readonly VarDataRepository _varDataRepository;

    // 设备列表变更事件，当设备列表数据更新时触发。
    public event EventHandler<List<Device>> OnDeviceListChanged;

    // 菜单树列表变更事件，当菜单树数据更新时触发。
    public event EventHandler<List<MenuBean>> OnMenuTreeListChanged;

    // MQTT列表变更事件，当MQTT配置数据更新时触发。
    public event EventHandler<List<Mqtt>> OnMqttListChanged;

    // 变量数据变更事件，当变量数据更新时触发。
    public event Action<List<VariableData>> OnVariableDataChanged;

    /// <summary>
    /// 当_devices属性值改变时触发的局部方法，用于调用OnDeviceListChanged事件。
    /// </summary>
    /// <param name="devices">新的设备列表。</param>
    partial void OnDevicesChanged(List<Device> devices)
    {
        OnDeviceListChanged?.Invoke(this, devices);
        
        
        VariableDatas.Clear();
        foreach (Device device in devices)
        {
            foreach (VariableTable variableTable in device.VariableTables)
            {
                foreach (VariableData variableData in variableTable.DataVariables)
                {
                    VariableDatas.Add(variableData);
                }
            }
        }
        OnVariableDataChanged?.Invoke(VariableDatas);
    }

    /// <summary>
    /// 当menuTrees属性值改变时触发的局部方法，用于调用OnMenuTreeListChanged事件。
    /// </summary>
    /// <param name="MenuTrees">新的菜单树列表。</param>
    partial void OnMenuTreesChanged(List<MenuBean> MenuTrees)
    {
        OnMenuTreeListChanged?.Invoke(this, MenuTrees);
    }

    /// <summary>
    /// 当_mqtts属性值改变时触发的局部方法，用于调用OnMqttListChanged事件。
    /// </summary>
    /// <param name="mqtts">新的MQTT配置列表。</param>
    partial void OnMqttsChanged(List<Mqtt> mqtts)
    {
        OnMqttListChanged?.Invoke(this, mqtts);
    }

    // 注释掉的代码块，可能用于变量数据变更事件的触发，但目前未启用。
    // {
    //     OnVariableDataChanged?.Invoke(this, value);
    // }

    /// <summary>
    /// DataServices类的构造函数。
    /// 注入ILogger<DataServices>，并初始化各个数据仓库。
    /// </summary>
    /// <param name="logger">日志记录器实例。</param>
    public DataServices()
    {
        IsActive = true; // 激活消息接收器
        _deviceRepository = new DeviceRepository();
        _menuRepository = new MenuRepository();
        _mqttRepository = new MqttRepository();
        _varDataRepository = new VarDataRepository();
        _variableDatas = new List<VariableData>();
    }

    /// <summary>
    /// 接收加载消息，根据消息类型从数据库加载对应的数据。
    /// </summary>
    /// <param name="message">加载消息，包含要加载的数据类型。</param>
    /// <exception cref="ArgumentException">如果加载类型未知，可能会抛出此异常（尽管当前实现中未显式抛出）。</exception>
    public async void Receive(LoadMessage message)
    {
        try
        {
            switch (message.LoadType)
            {
                case LoadTypes.All: // 加载所有数据
                    await LoadDevices();
                    await LoadMenus();
                    await LoadMqtts();
                    break;
                case LoadTypes.Devices: // 仅加载设备数据
                    // await LoadDevices();
                    break;
                case LoadTypes.Menu: // 仅加载菜单数据
                    await LoadMenus();
                    break;
                case LoadTypes.Mqtts: // 仅加载MQTT配置数据
                    await LoadMqtts();
                    break;
            }
        }
        catch (Exception e)
        {
            // 捕获加载数据时发生的异常，并通过通知和日志记录错误信息。
            NotificationHelper.ShowError($"加载数据出现了错误：{e.Message}", e);
        }
    }

    /// <summary>
    /// 异步加载设备数据。
    /// </summary>
    /// <returns>表示异步操作的任务。</returns>
    private async Task LoadDevices()
    {
        Devices = await _deviceRepository.GetAll();
    }

    /// <summary>
    /// 异步加载菜单数据，并进行父级关联和排序。
    /// </summary>
    /// <returns>表示异步操作的任务。</returns>
    private async Task LoadMenus()
    {
        MenuTrees = await _menuRepository.GetMenuTrees();
        foreach (MenuBean menu in MenuTrees)
        {
            MenuHelper.MenuAddParent(menu); // 为菜单添加父级引用
            DataServicesHelper.SortMenus(menu); // 排序菜单
        }
    }

    /// <summary>
    /// 异步获取所有MQTT配置。
    /// </summary>
    /// <returns>包含所有MQTT配置的列表。</returns>
    public async Task<List<Mqtt>> GetMqttsAsync()
    {
        return await _mqttRepository.GetAll();
    }

    /// <summary>
    /// 异步加载MQTT配置数据。
    /// </summary>
    /// <returns>表示异步操作的任务。</returns>
    private async Task LoadMqtts()
    {
        Mqtts = await _mqttRepository.GetAll();
    }


    /// <summary>
    /// 异步根据ID获取设备数据。
    /// </summary>
    /// <param name="id">设备ID。</param>
    /// <returns>设备对象，如果不存在则为null。</returns>
    public async Task<Device> GetDeviceByIdAsync(int id)
    {
        return await _deviceRepository.GetById(id);
    }

    /// <summary>
    /// 异步加载变量数据。
    /// </summary>
    /// <returns>表示异步操作的任务。</returns>
    private async Task LoadVariableDatas()
    {
        VariableDatas = await _varDataRepository.GetAllAsync();
    }

    /// <summary>
    /// 异步更新变量数据。
    /// </summary>
    /// <param name="variableData">要更新的变量数据。</param>
    /// <returns>表示异步操作的任务。</returns>
    public async Task UpdateVariableDataAsync(VariableData variableData)
    {
        await _varDataRepository.UpdateAsync(variableData);
    }
}