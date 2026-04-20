using System;
using System.Collections.Generic;

namespace SimpleFramework.GAS
{
    /// <summary>
    /// 游戏玩法标签 - 用于标识和分类游戏对象、效果、能力等
    /// 支持层次结构，例如："Ability.Movement.Dash"
    /// </summary>
    [Serializable]
    public class GameplayTag : IEquatable<GameplayTag>
    {
        /// <summary>
        /// 标签名称（支持点号分隔的层次结构）
        /// </summary>
        public string TagName;

        public GameplayTag() { }

        public GameplayTag(string tagName)
        {
            TagName = tagName;
        }

        /// <summary>
        /// 隐式转换为字符串
        /// </summary>
        public static implicit operator string(GameplayTag tag) => tag?.TagName;

        /// <summary>
        /// 隐式转换为 GameplayTag
        /// </summary>
        public static implicit operator GameplayTag(string tagName) => new GameplayTag(tagName);

        public bool Equals(GameplayTag other)
        {
            if (other is null)
            {
                return false;
            }
            return string.Equals(TagName, other.TagName);
        }

        public override bool Equals(object obj) => Equals(obj as GameplayTag);

        public override int GetHashCode() => TagName?.GetHashCode() ?? 0;

        public override string ToString() => TagName ?? "InvalidTag";

        /// <summary>
        /// 检查是否匹配指定标签（支持父标签匹配）
        /// 例："Ability.Movement.Dash" 匹配 "Ability.Movement"
        /// </summary>
        public bool MatchesTag(GameplayTag other)
        {
            if (other is null || string.IsNullOrEmpty(other.TagName)) return false;
            if (string.IsNullOrEmpty(TagName)) return false;
            
            // 完全匹配或作为子标签匹配
            return TagName == other.TagName || TagName.StartsWith(other.TagName + ".");
        }

        /// <summary>
        /// 检查是否匹配标签集合中的任意一个标签
        /// </summary>
        public bool MatchesAnyTags(IEnumerable<GameplayTag> tags)
        {
            foreach (var tag in tags)
            {
                if (MatchesTag(tag)) return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 游戏玩法标签容器 - 存储和管理多个 GameplayTag
    /// </summary>
    [Serializable]
    public class GameplayTagContainer
    {
        /// <summary>
        /// 标签列表
        /// </summary>
        public List<GameplayTag> tags = new List<GameplayTag>();

        /// <summary>
        /// 添加标签（避免重复）
        /// </summary>
        public void AddTag(GameplayTag tag)
        {
            if (tag != null && !tags.Contains(tag))
            {
                tags.Add(tag);
            }
        }

        /// <summary>
        /// 移除标签
        /// </summary>
        public void RemoveTag(GameplayTag tag)
        {
            tags.Remove(tag);
        }

        /// <summary>
        /// 检查是否包含指定标签（支持层次匹配）
        /// </summary>
        public bool HasTag(GameplayTag tag)
        {
            if (tag is null) return false;
            
            // 遍历所有标签，检查是否有匹配的
            foreach (var existingTag in tags)
            {
                if (existingTag.MatchesTag(tag)) return true;
            }
            return false;
        }

        /// <summary>
        /// 检查是否包含任意一个指定标签
        /// </summary>
        public bool HasAnyTags(IEnumerable<GameplayTag> otherTags)
        {
            foreach (var tag in otherTags)
            {
                if (HasTag(tag)) return true;
            }
            return false;
        }

        /// <summary>
        /// 检查是否包含所有指定标签
        /// </summary>
        public bool HasAllTags(IEnumerable<GameplayTag> otherTags)
        {
            foreach (var tag in otherTags)
            {
                if (!HasTag(tag)) return false;
            }
            return true;
        }

        /// <summary>
        /// 从另一个容器复制所有标签
        /// </summary>
        public void CopyFrom(GameplayTagContainer other)
        {
            tags = new List<GameplayTag>(other.tags);
        }

        /// <summary>
        /// 清空所有标签
        /// </summary>
        public void Clear()
        {
            tags.Clear();
        }

        /// <summary>
        /// 标签数量
        /// </summary>
        public int Count => tags.Count;

        /// <summary>
        /// 索引器
        /// </summary>
        public GameplayTag this[int index] => tags[index];
    }
}
