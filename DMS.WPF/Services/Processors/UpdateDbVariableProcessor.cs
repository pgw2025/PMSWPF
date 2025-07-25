using System.Threading.Tasks;
using DMS.Helper;
using DMS.WPF.Models;

namespace DMS.Services.Processors
{
    public class UpdateDbVariableProcessor : IVariableProcessor
    {
        private readonly DataServices _dataServices;

        public UpdateDbVariableProcessor(DataServices dataServices)
        {
            _dataServices = dataServices;
        }

        public async Task ProcessAsync(VariableContext context)
        {
            try
            {
                // 假设 DataServices 有一个方法来更新 Variable
                await _dataServices.UpdateVariableAsync(context.Data);
                // NlogHelper.Info($"数据库变量 {context.Data.Name} 更新成功，值为: {context.Data.DataValue}");
                
                if (!_dataServices.AllVariables.TryGetValue(context.Data.Id, out Variable oldVariable))
                {
                    NlogHelper.Warn($"数据库更新完成修改变量值是否改变时在_dataServices.AllVariables中找不到Id:{context.Data.Id},Name:{context.Data.Name}的变量。");
                    context.IsHandled = true;
                }
                oldVariable.DataValue = context.Data.DataValue;
            }
            catch (Exception ex)
            {
                NlogHelper.Error($"更新数据库变量 {context.Data.Name} 失败: {ex.Message}", ex);
            }
        }
    }
}