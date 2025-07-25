using DMS.WPF.Models;

namespace DMS.Models
{
    public class VariableContext
    {
        public Variable Data { get; set; }
        public bool IsHandled { get; set; }

        public VariableContext(Variable data)
        {
            Data = data;
            IsHandled = false; // 默认未处理
        }
    }
}