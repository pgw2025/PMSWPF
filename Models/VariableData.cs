﻿using CommunityToolkit.Mvvm.ComponentModel;
using PMSWPF.Enums;

namespace PMSWPF.Models;

/// <summary>
/// 表示变量数据信息。
/// </summary>
public partial class VariableData : ObservableObject
{
    /// <summary>
    /// 变量唯一标识符。
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 变量名称。
    /// </summary>
    [ObservableProperty]
    private string name;

    /// <summary>
    /// 节点ID，用于标识变量在设备或系统中的唯一路径。
    /// </summary>
    public string NodeId { get; set; }
    
    /// <summary>
    /// 节点ID，用于标识变量在设备或系统中的唯一路径。
    /// </summary>
    public string S7Address { get; set; }

    /// <summary>
    /// OPC UA Endpoint URL。
    /// </summary>
    public string? OpcUaEndpointUrl { get; set; }

    /// <summary>
    /// OPC UA Node ID。
    /// </summary>
    public string? OpcUaNodeId { get; set; }


    /// <summary>
    /// 变量描述。
    /// </summary>
    [ObservableProperty]
    private string description = String.Empty;

    /// <summary>
    /// 协议类型，例如Modbus、OPC UA等。
    /// </summary>
    public ProtocolType ProtocolType { get; set; }

    /// <summary>
    /// 信号类型，例如模拟量、数字量等。
    /// </summary>
    public SignalType SignalType { get; set; }

    /// <summary>
    /// 数据类型，例如Int、Float、String等。
    /// </summary>
    public string DataType { get; set; }

    /// <summary>
    /// 变量当前原始数据值。
    /// </summary>
    [ObservableProperty]
    private string dataValue  = String.Empty;

    /// <summary>
    /// 变量经过转换或格式化后的显示值。
    /// </summary>
    [ObservableProperty]
    private string displayValue  = String.Empty;

    /// <summary>
    /// 指示变量是否处于激活状态。
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// 指示变量是否被选中
    /// </summary>
    [ObservableProperty]
    public bool isSelect;

    /// <summary>
    /// 指示是否需要保存变量数据。
    /// </summary>
    public bool IsSave { get; set; }

    /// <summary>
    /// 指示是否需要对变量进行报警监测。
    /// </summary>
    public bool IsAlarm { get; set; }

    /// <summary>
    /// 轮询级别，例如1秒、5秒等。
    /// </summary>
    [ObservableProperty]
    private PollLevelType pollLevelType = PollLevelType.ThirtySeconds;

    /// <summary>
    /// OPC UA更新类型，例如轮询或订阅。
    /// </summary>
    [ObservableProperty]
    private OpcUaUpdateType opcUaUpdateType = OpcUaUpdateType.OpcUaPoll;

    /// <summary>
    /// 最后一次轮询时间。
    /// </summary>
    public DateTime LastPollTime { get; set; } = DateTime.MinValue;

    /// <summary>
    /// 指示变量是否已被逻辑删除。
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// 指示变量是否已被修改了。
    /// </summary>
    [ObservableProperty]
    private bool isModified;

    /// <summary>
    /// 报警的最大值阈值。
    /// </summary>
    public double AlarmMax { get; set; }

    /// <summary>
    /// 报警的最小值阈值。
    /// </summary>
    public double AlarmMin { get; set; }

    /// <summary>
    /// 数据转换规则或表达式。
    /// </summary>
    [ObservableProperty]
    private string converstion  = String.Empty;

    /// <summary>
    /// 数据保存的范围或阈值。
    /// </summary>
    public double SaveRange { get; set; }

    /// <summary>
    /// 变量数据创建时间。
    /// </summary>
    [ObservableProperty]
    private DateTime createTime;


    /// <summary>
    /// 变量数据最后更新时间。
    /// </summary>
   [ObservableProperty]
    private DateTime updateTime  = DateTime.Now;

    /// <summary>
    /// 最后更新变量数据的用户。
    /// </summary>
    public User UpdateUser { get; set; }

    /// <summary>
    /// 关联的变量表ID。
    /// </summary>
    public int VariableTableId { get; set; }

    /// <summary>
    /// 关联的变量表实体。
    /// </summary>
    public VariableTable? VariableTable { get; set; }

    /// <summary>
    /// 关联的MQTT配置列表。
    /// </summary>
    public List<Mqtt> Mqtts { get; set; }

    partial void OnPollLevelTypeChanged(PollLevelType value)
    {
        IsModified = true;
    }
}