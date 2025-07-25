using DMS.Core.Enums;
using System;

namespace DMS.Application.DTOs;

/// <summary>
/// 用于在UI上显示变量基本信息的DTO。
/// </summary>
public class VariableDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public SignalType DataType { get; set; }
    public PollLevelType PollLevel { get; set; }
    public bool IsActive { get; set; }
    public int VariableTableId { get; set; }
    public string OpcUaNodeId { get; set; }
    public bool IsHistoryEnabled { get; set; }
    public double HistoryDeadband { get; set; }
    public bool IsAlarmEnabled { get; set; }
    public double AlarmMinValue { get; set; }
    public double AlarmMaxValue { get; set; }
    public double AlarmDeadband { get; set; }
    public ProtocolType Protocol { get; set; }
    public CSharpDataType CSharpDataType { get; set; }
    public string ConversionFormula { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsModified { get; set; }
}