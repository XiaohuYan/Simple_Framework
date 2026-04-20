using System.Collections.Generic;

namespace SimpleFramework.Config
{
    /// <summary>
    /// 配置表解析结果
    /// </summary>
    /// <typeparam name="T">配置数据类型</typeparam>
    public class ConfigDataResult<T> where T : ConfigDataBase, new()
    {
        /// <summary>
        /// 配置数据列表
        /// </summary>
        public List<T> DataList { get; set; } = new List<T>();

        /// <summary>
        /// 配置数据字典（按 ID 索引）
        /// </summary>
        public Dictionary<int, T> DataDict { get; set; } = new Dictionary<int, T>();

        /// <summary>
        /// 是否解析成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 根据 ID 获取配置
        /// </summary>
        public T GetById(int id)
        {
            return DataDict.TryGetValue(id, out var data) ? data : null;
        }

        /// <summary>
        /// 获取所有配置
        /// </summary>
        public List<T> GetAll()
        {
            return DataList;
        }

        /// <summary>
        /// 获取配置数量
        /// </summary>
        public int Count => DataList.Count;
    }
}