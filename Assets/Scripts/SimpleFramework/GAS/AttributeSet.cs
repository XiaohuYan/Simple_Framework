using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SimpleFramework.GAS
{
    /// <summary>
    /// 属性集基类 - 用于组织和管理相关的属性
    /// 类似于 Unreal GAS 中的 AttributeSet
    /// </summary>
    public abstract class AttributeSet : ScriptableObject
    {
        /// <summary> 存储所有属性值的字典 </summary>
        protected Dictionary<string, float> attributes = new Dictionary<string, float>();
        /// <summary> 存储所有属性最大值的字典 </summary>
        protected Dictionary<string, float> attributeMaxValues = new Dictionary<string, float>();
        
        /// <summary> 属性值改变时触发的事件（属性名，旧值，新值） </summary>
        public event Action<string, float, float> OnAttributeChanged;

        /// <summary>
        /// 初始化属性集
        /// </summary>
        public virtual void Init()
        {
            InitializeAttributes();
        }

        /// <summary>
        /// 初始化属性（由子类实现）
        /// </summary>
        protected abstract void InitializeAttributes();

        /// <summary>
        /// 注册一个属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="initialValue">初始值</param>
        /// <param name="maxValue">最大值</param>
        protected void RegisterAttribute(string name, float initialValue, float maxValue = float.MaxValue)
        {
            attributes[name] = initialValue;
            attributeMaxValues[name] = maxValue;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        public float GetAttributeValue(string attributeName)
        {
            if (attributes.TryGetValue(attributeName, out float value))
            {
                return value;
            }
            return 0f;
        }

        /// <summary>
        /// 获取属性最大值
        /// </summary>
        public float GetAttributeMaxValue(string attributeName)
        {
            if (attributeMaxValues.TryGetValue(attributeName, out float maxValue))
            {
                return maxValue;
            }
            return float.MaxValue;
        }

        /// <summary>
        /// 设置属性值（会触发事件）
        /// </summary>
        public void SetAttributeValue(string attributeName, float newValue)
        {
            if (!attributes.ContainsKey(attributeName))
            {
                Debug.LogWarning($"Attribute {attributeName} not found in {GetType().Name}");
                return;
            }

            float oldValue = attributes[attributeName];
            
            // 值没有变化则直接返回
            if (Math.Abs(oldValue - newValue) < 0.001f) return;

            // 限制不超过最大值
            float maxValue = GetAttributeMaxValue(attributeName);
            newValue = Mathf.Min(newValue, maxValue);

            attributes[attributeName] = newValue;

            // 触发属性改变事件
            OnAttributeChanged?.Invoke(attributeName, oldValue, newValue);
        }

        /// <summary>
        /// 修改属性值（增加或减少）
        /// </summary>
        public void ModifyAttributeValue(string attributeName, float deltaValue)
        {
            float currentValue = GetAttributeValue(attributeName);
            SetAttributeValue(attributeName, currentValue + deltaValue);
        }

        /// <summary>
        /// 属性值改变前的回调（可用于验证或修改新值）
        /// </summary>
        public virtual void PreAttributeChange(string attributeName, ref float newValue) { }

        /// <summary>
        /// 属性基础值改变前的回调
        /// </summary>
        public virtual void PreAttributeBaseChange(string attributeName, ref float newValue) { }

        /// <summary>
        /// 获取所有属性值（返回副本）
        /// </summary>
        public Dictionary<string, float> GetAllAttributes()
        {
            return new Dictionary<string, float>(attributes);
        }

        /// <summary>
        /// 获取属性集名称
        /// </summary>
        public string GetAttributeName()
        {
            return GetType().Name;
        }
    }

    /// <summary>
    /// 生命值属性集 - 包含生命、最大生命、生命回复
    /// </summary>
    [CreateAssetMenu(fileName = "New HealthSet", menuName = "GAS/AttributeSets/HealthSet")]
    public class HealthSet : AttributeSet
    {
        private const string Health = "Health";
        private const string MaxHealth = "MaxHealth";
        private const string HealthRegen = "HealthRegen";

        protected override void InitializeAttributes()
        {
            // 注册生命值（初始 100，最大 100）
            RegisterAttribute(Health, 100f, 100f);
            // 注册最大生命值
            RegisterAttribute(MaxHealth, 100f);
            // 注册生命回复
            RegisterAttribute(HealthRegen, 0f);
        }

        /// <summary>
        /// 属性改变前的处理 - 限制生命值在 0 到最大值之间
        /// </summary>
        public override void PreAttributeChange(string attributeName, ref float newValue)
        {
            base.PreAttributeChange(attributeName, ref newValue);

            if (attributeName == Health)
            {
                float maxHealth = GetAttributeMaxValue(Health);
                newValue = Mathf.Clamp(newValue, 0f, maxHealth);
            }
        }
    }

    /// <summary>
    /// 伤害属性集 - 包含攻击、攻速、暴击、防御等
    /// </summary>
    [CreateAssetMenu(fileName = "New DamageSet", menuName = "GAS/AttributeSets/DamageSet")]
    public class DamageSet : AttributeSet
    {
        private const string AttackDamage = "AttackDamage";
        private const string AttackSpeed = "AttackSpeed";
        private const string CriticalChance = "CriticalChance";
        private const string CriticalMultiplier = "CriticalMultiplier";
        private const string Armor = "Armor";
        private const string MagicResist = "MagicResist";

        protected override void InitializeAttributes()
        {
            RegisterAttribute(AttackDamage, 10f);
            RegisterAttribute(AttackSpeed, 1f);
            RegisterAttribute(CriticalChance, 0.05f, 1f); // 5% 暴击率，最大 100%
            RegisterAttribute(CriticalMultiplier, 2f);    // 200% 暴击伤害
            RegisterAttribute(Armor, 0f);
            RegisterAttribute(MagicResist, 0f);
        }
    }

    /// <summary>
    /// 移动属性集 - 包含移动速度、加速度等
    /// </summary>
    [CreateAssetMenu(fileName = "New MovementSet", menuName = "GAS/AttributeSets/MovementSet")]
    public class MovementSet : AttributeSet
    {
        private const string MoveSpeed = "MoveSpeed";
        private const string Acceleration = "Acceleration";
        private const string BrakingDeceleration = "BrakingDeceleration";
        private const string AirControl = "AirControl";

        protected override void InitializeAttributes()
        {
            RegisterAttribute(MoveSpeed, 600f);
            RegisterAttribute(Acceleration, 2048f);
            RegisterAttribute(BrakingDeceleration, 768f);
            RegisterAttribute(AirControl, 30f);
        }
    }
}
