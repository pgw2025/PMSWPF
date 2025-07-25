namespace DMS.Application.DTOs;

/// <summary>
/// 用于创建新设备及其关联的变量表和菜单的DTO。
/// </summary>
public class CreateDeviceWithDetailsDto
{
    public CreateDeviceDto Device { get; set; }
    public VariableTableDto VariableTable { get; set; }
    // public MenuBeanDto Menu { get; set; } // 如果需要包含菜单信息
}