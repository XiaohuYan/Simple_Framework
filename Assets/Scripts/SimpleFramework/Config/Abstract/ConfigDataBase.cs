using System.Collections.Generic;

namespace SimpleFramework.Config
{
    /// <summary>
    /// 配置表数据基类
    /// </summary>
    public abstract class ConfigDataBase
    {
        /// <summary>
        /// 配置 ID（主键）
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 解析配置数据
        /// </summary>
        /// <param name="data">原始数据字典</param>
        public abstract void Parse(Dictionary<string, object> data);

        /// <summary>
        /// 验证数据有效性
        /// </summary>
        public virtual bool Validate()
        {
            return Id > 0;
        }
    }
}