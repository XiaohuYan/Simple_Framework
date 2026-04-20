namespace SimpleFramework.Config
{
    /// <summary>
    /// 配置表解析器策略接口
    /// </summary>
    public interface IConfigParserStrategy
    {
        /// <summary>
        /// 解析配置表文件
        /// </summary>
        /// <typeparam name="T">配置数据类型</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <param name="content">文件内容</param>
        /// <returns>解析结果</returns>
        ConfigDataResult<T> Parse<T>(string filePath, string content) where T : ConfigDataBase, new();

        /// <summary>
        /// 是否支持该文件格式
        /// </summary>
        /// <param name="extension">文件扩展名</param>
        /// <returns>是否支持</returns>
        bool SupportsFormat(string extension);
    }
}
