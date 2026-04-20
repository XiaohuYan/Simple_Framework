//using System.Collections.Generic;
//using UnityEngine;

//namespace SimpleFramework.Config
//{
//    /// <summary>
//    /// 物品配置数据
//    /// </summary>
//    [System.Serializable]
//    public class ItemConfig : ConfigDataBase
//    {
//        /// <summary>
//        /// 物品名称
//        /// </summary>
//        public string ItemName { get; set; }

//        /// <summary>
//        /// 物品类型
//        /// </summary>
//        public int ItemType { get; set; }

//        /// <summary>
//        /// 物品品质
//        /// </summary>
//        public int Quality { get; set; }

//        /// <summary>
//        /// 价格
//        /// </summary>
//        public int Price { get; set; }

//        /// <summary>
//        /// 描述
//        /// </summary>
//        public string Description { get; set; }

//        /// <summary>
//        /// 最大堆叠数
//        /// </summary>
//        public int MaxStack { get; set; }

//        /// <summary>
//        /// 图标路径
//        /// </summary>
//        public string IconPath { get; set; }

//        /// <summary>
//        /// 模型路径
//        /// </summary>
//        public string ModelPath { get; set; }

//        /// <summary>
//        /// 使用效果 ID 列表
//        /// </summary>
//        public List<int> EffectIds { get; set; } = new List<int>();

//        public override void Parse(Dictionary<string, object> data)
//        {
//            Id = GetInt(data, "Id");
//            ItemName = GetString(data, "ItemName");
//            ItemType = GetInt(data, "ItemType");
//            Quality = GetInt(data, "Quality");
//            Price = GetInt(data, "Price");
//            Description = GetString(data, "Description");
//            MaxStack = GetInt(data, "MaxStack", 1);
//            IconPath = GetString(data, "IconPath");
//            ModelPath = GetString(data, "ModelPath");
            
//            // 解析效果 ID 列表（逗号分隔）
//            var effectIdsStr = GetString(data, "EffectIds");
//            if (!string.IsNullOrEmpty(effectIdsStr))
//            {
//                var ids = effectIdsStr.Split(',');
//                EffectIds.Clear();
//                foreach (var id in ids)
//                {
//                    if (int.TryParse(id.Trim(), out var effectId))
//                    {
//                        EffectIds.Add(effectId);
//                    }
//                }
//            }
//        }

//        public override bool Validate()
//        {
//            return base.Validate() && !string.IsNullOrEmpty(ItemName);
//        }

//        #region 辅助方法

//        private string GetString(Dictionary<string, object> data, string key, string defaultValue = "")
//        {
//            return data.TryGetValue(key, out var value) ? value.ToString() : defaultValue;
//        }

//        private int GetInt(Dictionary<string, object> data, string key, int defaultValue = 0)
//        {
//            if (data.TryGetValue(key, out var value))
//            {
//                if (value is int intValue)
//                {
//                    return intValue;
//                }
//                if (int.TryParse(value.ToString(), out var parsed))
//                {
//                    return parsed;
//                }
//            }
//            return defaultValue;
//        }

//        private float GetFloat(Dictionary<string, object> data, string key, float defaultValue = 0f)
//        {
//            if (data.TryGetValue(key, out var value))
//            {
//                if (value is float floatValue)
//                {
//                    return floatValue;
//                }
//                if (float.TryParse(value.ToString(), out var parsed))
//                {
//                    return parsed;
//                }
//            }
//            return defaultValue;
//        }

//        private bool GetBool(Dictionary<string, object> data, string key, bool defaultValue = false)
//        {
//            if (data.TryGetValue(key, out var value))
//            {
//                if (value is bool boolValue)
//                {
//                    return boolValue;
//                }
//                if (bool.TryParse(value.ToString(), out var parsed))
//                {
//                    return parsed;
//                }
//            }
//            return defaultValue;
//        }

//        #endregion
//    }

//    /// <summary>
//    /// 技能配置数据
//    /// </summary>
//    [System.Serializable]
//    public class SkillConfig : ConfigDataBase
//    {
//        /// <summary>
//        /// 技能名称
//        /// </summary>
//        public string SkillName { get; set; }

//        /// <summary>
//        /// 技能类型
//        /// </summary>
//        public int SkillType { get; set; }

//        /// <summary>
//        /// 消耗魔法值
//        /// </summary>
//        public int ManaCost { get; set; }

//        /// <summary>
//        /// 冷却时间
//        /// </summary>
//        public float Cooldown { get; set; }

//        /// <summary>
//        /// 伤害倍率
//        /// </summary>
//        public float DamageMultiplier { get; set; }

//        /// <summary>
//        /// 技能范围
//        /// </summary>
//        public float Range { get; set; }

//        /// <summary>
//        /// 前置技能 ID
//        /// </summary>
//        public int PreSkillId { get; set; }

//        /// <summary>
//        /// 技能等级上限
//        /// </summary>
//        public int MaxLevel { get; set; }

//        /// <summary>
//        /// 技能描述
//        /// </summary>
//        public string Description { get; set; }

//        /// <summary>
//        /// 特效路径
//        /// </summary>
//        public string EffectPath { get; set; }

//        public override void Parse(Dictionary<string, object> data)
//        {
//            Id = GetInt(data, "Id");
//            SkillName = GetString(data, "SkillName");
//            SkillType = GetInt(data, "SkillType");
//            ManaCost = GetInt(data, "ManaCost");
//            Cooldown = GetFloat(data, "Cooldown");
//            DamageMultiplier = GetFloat(data, "DamageMultiplier");
//            Range = GetFloat(data, "Range");
//            PreSkillId = GetInt(data, "PreSkillId");
//            MaxLevel = GetInt(data, "MaxLevel", 1);
//            Description = GetString(data, "Description");
//            EffectPath = GetString(data, "EffectPath");
//        }

//        public override bool Validate()
//        {
//            return base.Validate() && !string.IsNullOrEmpty(SkillName);
//        }

//        #region 辅助方法

//        private string GetString(Dictionary<string, object> data, string key, string defaultValue = "")
//        {
//            return data.TryGetValue(key, out var value) ? value.ToString() : defaultValue;
//        }

//        private int GetInt(Dictionary<string, object> data, string key, int defaultValue = 0)
//        {
//            if (data.TryGetValue(key, out var value))
//            {
//                if (value is int intValue)
//                {
//                    return intValue;
//                }
//                if (int.TryParse(value.ToString(), out var parsed))
//                {
//                    return parsed;
//                }
//            }
//            return defaultValue;
//        }

//        private float GetFloat(Dictionary<string, object> data, string key, float defaultValue = 0f)
//        {
//            if (data.TryGetValue(key, out var value))
//            {
//                if (value is float floatValue)
//                {
//                    return floatValue;
//                }
//                if (float.TryParse(value.ToString(), out var parsed))
//                {
//                    return parsed;
//                }
//            }
//            return defaultValue;
//        }

//        #endregion
//    }

//    /// <summary>
//    /// 角色配置数据
//    /// </summary>
//    [System.Serializable]
//    public class CharacterConfig : ConfigDataBase
//    {
//        /// <summary>
//        /// 角色名称
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        /// 角色等级
//        /// </summary>
//        public int Level { get; set; }

//        /// <summary>
//        /// 基础生命值
//        /// </summary>
//        public int BaseHealth { get; set; }

//        /// <summary>
//        /// 基础魔法值
//        /// </summary>
//        public int BaseMana { get; set; }

//        /// <summary>
//        /// 基础攻击力
//        /// </summary>
//        public int BaseAttack { get; set; }

//        /// <summary>
//        /// 基础防御力
//        /// </summary>
//        public int BaseDefense { get; set; }

//        /// <summary>
//        /// 经验曲线
//        /// </summary>
//        public int ExpCurve { get; set; }

//        /// <summary>
//        /// 模型路径
//        /// </summary>
//        public string ModelPath { get; set; }

//        /// <summary>
//        /// 头像路径
//        /// </summary>
//        public string AvatarPath { get; set; }

//        /// <summary>
//        /// 初始技能 ID 列表
//        /// </summary>
//        public List<int> InitialSkills { get; set; } = new List<int>();

//        public override void Parse(Dictionary<string, object> data)
//        {
//            Id = GetInt(data, "Id");
//            Name = GetString(data, "Name");
//            Level = GetInt(data, "Level", 1);
//            BaseHealth = GetInt(data, "BaseHealth");
//            BaseMana = GetInt(data, "BaseMana");
//            BaseAttack = GetInt(data, "BaseAttack");
//            BaseDefense = GetInt(data, "BaseDefense");
//            ExpCurve = GetInt(data, "ExpCurve");
//            ModelPath = GetString(data, "ModelPath");
//            AvatarPath = GetString(data, "AvatarPath");

//            // 解析初始技能 ID 列表
//            var skillsStr = GetString(data, "InitialSkills");
//            if (!string.IsNullOrEmpty(skillsStr))
//            {
//                var ids = skillsStr.Split(',');
//                InitialSkills.Clear();
//                foreach (var id in ids)
//                {
//                    if (int.TryParse(id.Trim(), out var skillId))
//                    {
//                        InitialSkills.Add(skillId);
//                    }
//                }
//            }
//        }

//        public override bool Validate()
//        {
//            return base.Validate() && !string.IsNullOrEmpty(Name);
//        }

//        #region 辅助方法

//        private string GetString(Dictionary<string, object> data, string key, string defaultValue = "")
//        {
//            return data.TryGetValue(key, out var value) ? value.ToString() : defaultValue;
//        }

//        private int GetInt(Dictionary<string, object> data, string key, int defaultValue = 0)
//        {
//            if (data.TryGetValue(key, out var value))
//            {
//                if (value is int intValue)
//                {
//                    return intValue;
//                }
//                if (int.TryParse(value.ToString(), out var parsed))
//                {
//                    return parsed;
//                }
//            }
//            return defaultValue;
//        }

//        #endregion
//    }
//}
