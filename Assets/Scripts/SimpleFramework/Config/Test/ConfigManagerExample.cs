//using UnityEngine;

//namespace SimpleFramework.Config
//{
//    /// <summary>
//    /// 配置表管理器使用示例
//    /// </summary>
//    public class ConfigManagerExample : MonoBehaviour
//    {
//        private void Start()
//        {
//            // 示例 1: 从 Resources 加载 XML 配置
//            LoadItemConfigFromResources();

//            // 示例 2: 从 Resources 加载 JSON 配置
//            LoadSkillConfigFromResources();

//            // 示例 3: 从文件加载配置
//            LoadCharacterConfigFromFile();

//            // 示例 4: 异步加载配置
//            LoadConfigAsync();

//            // 示例 5: 使用配置数据
//            UseConfigData();
//        }

//        /// <summary>
//        /// 示例 1: 从 Resources 加载 XML 配置
//        /// 假设 Resources/Configs/ItemConfig.xml 存在
//        /// </summary>
//        private void LoadItemConfigFromResources()
//        {
//            var result = ConfigManager.Instance.LoadConfigFromResources<ItemConfig>("Configs/ItemConfig");
            
//            if (result != null && result.Success)
//            {
//                Debug.Log($"物品配置加载成功，共 {result.Count} 条数据");
                
//                // 获取 ID 为 1001 的物品
//                var item = result.GetById(1001);
//                if (item != null)
//                {
//                    Debug.Log($"物品 ID: {item.Id}, 名称：{item.ItemName}, 价格：{item.Price}");
//                }
//            }
//        }

//        /// <summary>
//        /// 示例 2: 从 Resources 加载 JSON 配置
//        /// 假设 Resources/Configs/SkillConfig.json 存在
//        /// </summary>
//        private void LoadSkillConfigFromResources()
//        {
//            var result = ConfigManager.Instance.LoadConfigFromResources<SkillConfig>("Configs/SkillConfig");
            
//            if (result != null && result.Success)
//            {
//                Debug.Log($"技能配置加载成功，共 {result.Count} 条数据");
                
//                // 遍历所有技能
//                foreach (var skill in result.GetAll())
//                {
//                    Debug.Log($"技能 ID: {skill.Id}, 名称：{skill.SkillName}, 冷却：{skill.Cooldown}s");
//                }
//            }
//        }

//        /// <summary>
//        /// 示例 3: 从文件加载配置
//        /// 假设 StreamingAssets/Configs/CharacterConfig.xml 存在
//        /// </summary>
//        private void LoadCharacterConfigFromFile()
//        {
//            var filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "Configs/CharacterConfig.xml");
//            var result = ConfigManager.Instance.LoadConfigFromFile<CharacterConfig>(filePath);
            
//            if (result != null && result.Success)
//            {
//                Debug.Log($"角色配置加载成功，共 {result.Count} 条数据");
//            }
//        }

//        /// <summary>
//        /// 示例 4: 异步加载配置
//        /// </summary>
//        private void LoadConfigAsync()
//        {
//            ConfigManager.Instance.LoadConfigFromResourcesAsync<ItemConfig>("Configs/ItemConfig", (result) =>
//            {
//                if (result != null && result.Success)
//                {
//                    Debug.Log($"异步加载物品配置完成，共 {result.Count} 条数据");
//                }
//            });
//        }

//        /// <summary>
//        /// 示例 5: 使用已加载的配置数据
//        /// </summary>
//        private void UseConfigData()
//        {
//            // 获取已加载的物品配置
//            var itemConfig = ConfigManager.Instance.GetConfig<ItemConfig>("ItemConfig");
//            if (itemConfig != null)
//            {
//                // 查找所有品质为 3 的物品
//                var quality3Items = itemConfig.GetAll().FindAll(item => item.Quality == 3);
//                Debug.Log($"品质为 3 的物品数量：{quality3Items.Count}");
//            }

//            // 直接通过 ID 获取配置
//            var skillConfig = ConfigManager.Instance.GetConfigById<SkillConfig>("SkillConfig", 2001);
//            if (skillConfig != null)
//            {
//                Debug.Log($"技能 2001: {skillConfig.SkillName}, 伤害倍率：{skillConfig.DamageMultiplier}");
//            }
//        }

//        /// <summary>
//        /// 示例 6: 检查配置是否已加载
//        /// </summary>
//        private void CheckConfigLoaded()
//        {
//            if (ConfigManager.Instance.IsConfigLoaded("ItemConfig"))
//            {
//                Debug.Log("物品配置已加载");
//            }
//        }

//        /// <summary>
//        /// 示例 7: 卸载配置
//        /// </summary>
//        private void UnloadConfig()
//        {
//            // 卸载单个配置
//            ConfigManager.Instance.UnloadConfig("ItemConfig");
            
//            // 卸载所有配置
//            // ConfigManager.Instance.UnloadConfig("", unloadAll: true);
//        }

//        /// <summary>
//        /// 示例 8: 注册自定义解析器
//        /// </summary>
//        private void RegisterCustomParser()
//        {
//            // 注册自定义的 CSV 解析器
//            // ConfigParserFactory.RegisterParser(new CsvConfigParser());
            
//            // 检查是否支持某种格式
//            bool supportsXml = ConfigParserFactory.SupportsFormat(".xml");
//            Debug.Log($"是否支持 XML: {supportsXml}");
            
//            // 获取所有支持的格式
//            var formats = ConfigParserFactory.GetSupportedFormats();
//            Debug.Log($"支持的格式：{string.Join(", ", formats)}");
//        }
//    }

//    /// <summary>
//    /// 自定义 CSV 解析器示例（未实现）
//    /// </summary>
//    public class CsvConfigParser : IConfigParserStrategy
//    {
//        public bool SupportsFormat(string extension)
//        {
//            return extension.Equals(".csv", System.StringComparison.OrdinalIgnoreCase);
//        }

//        public ConfigDataResult<T> Parse<T>(string filePath, string content) where T : ConfigDataBase, new()
//        {
//            // TODO: 实现 CSV 解析逻辑
//            var result = new ConfigDataResult<T>();
//            result.Success = true;
//            return result;
//        }
//    }
//}
