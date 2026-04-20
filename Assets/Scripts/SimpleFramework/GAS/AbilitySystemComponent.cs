using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleFramework.GAS
{
    /// <summary>
    /// 能力系统组件 - GAS 的核心组件
    /// 管理属性、标签、效果、能力等所有 GameplayAbilitySystem 功能
    /// 类似于 Unreal GAS 中的 AbilitySystemComponent (ASC)
    /// </summary>
    public class AbilitySystemComponent : MonoBehaviour
    {
        #region 成员变量

        /// <summary> 属性集列表 </summary>
        [Header("属性集")]
        public List<AttributeSet> AttributeSets = new List<AttributeSet>();

        /// <summary> 拥有的标签容器 </summary>
        [Header("标签")]
        public GameplayTagContainer OwnedTags = new GameplayTagContainer();
        /// <summary> 所有标签容器（包括从效果获得的标签） </summary>
        public GameplayTagContainer AllTags = new GameplayTagContainer();

        /// <summary> 激活中的效果字典（句柄 -> 效果） </summary>
        private Dictionary<int, ActiveGameplayEffect> activeEffects = new Dictionary<int, ActiveGameplayEffect>();
        /// <summary> 按标签组织的效果字典 </summary>
        private Dictionary<GameplayTag, List<ActiveGameplayEffect>> effectByTag = new Dictionary<GameplayTag, List<ActiveGameplayEffect>>();
        /// <summary> 聚合修改器字典（用于计算最终属性值） </summary>
        private Dictionary<string, float> aggregatedModifiers = new Dictionary<string, float>();
        
        /// <summary> 已给予的能力列表 </summary>
        private List<GameplayAbility> abilities = new List<GameplayAbility>();
        /// <summary> 按标签索引的能力字典 </summary>
        private Dictionary<GameplayTag, GameplayAbility> abilitiesByTag = new Dictionary<GameplayTag, GameplayAbility>();
        
        /// <summary> 被阻塞的能力计数字典 </summary>
        private Dictionary<GameplayTag, int> blockedAbilities = new Dictionary<GameplayTag, int>();
        
        /// <summary> 下一个效果句柄 ID </summary>
        private int nextEffectHandle = 1;
        /// <summary> 下一个能力句柄 ID </summary>
        private int nextAbilityHandle = 1;

        #endregion

        #region 事件

        /// <summary> 效果应用时触发 </summary>
        public event Action<GameplayEffectSpec, object> OnEffectApplied;
        /// <summary> 效果移除时触发 </summary>
        public event Action<GameplayEffectSpec, object> OnEffectRemoved;
        /// <summary> 效果堆叠变化时触发 </summary>
        public event Action<GameplayEffectSpec, object> OnEffectStackChanged;
        /// <summary> 能力激活时触发 </summary>
        public event Action<GameplayAbility> OnAbilityActivated;
        /// <summary> 能力结束时触发 </summary>
        public event Action<GameplayAbility> OnAbilityEnded;
        /// <summary> 属性变化时触发 </summary>
        public event Action<string, float, float> OnAttributeChanged;
        /// <summary> 标签变化时触发 </summary>
        public event Action<GameplayTag, int> OnTagChanged;

        #endregion

        /// <summary>
        /// Unity Update - 更新激活的效果和能力
        /// </summary>
        private void Update()
        {
            UpdateActiveEffects(Time.deltaTime);
            UpdateAbilities(Time.deltaTime);
        }

        #region 属性系统

        /// <summary>
        /// 初始化 ASC - 初始化所有属性集
        /// </summary>
        public void Initialize()
        {
            foreach (var attributeSet in AttributeSets)
            {
                if (attributeSet != null)
                {
                    attributeSet.Init();
                    attributeSet.OnAttributeChanged += HandleAttributeChanged;
                }
            }
        }

        /// <summary>
        /// 处理属性变化事件
        /// </summary>
        private void HandleAttributeChanged(string attributeName, float oldValue, float newValue)
        {
            OnAttributeChanged?.Invoke(attributeName, oldValue, newValue);
        }

        /// <summary>
        /// 获取属性值（通过属性对）
        /// </summary>
        public float GetNumericAttribute(AttributePair attributePair)
        {
            foreach (var attributeSet in AttributeSets)
            {
                if (attributeSet.GetAttributeName() == attributePair.AttributeSetName)
                {
                    return attributeSet.GetAttributeValue(attributePair.AttributeName);
                }
            }
            return 0f;
        }

        /// <summary>
        /// 获取属性值（通过属性名，使用反射）
        /// </summary>
        public float GetNumericAttribute(string attributeName)
        {
            foreach (var attributeSet in AttributeSets)
            {
                var field = attributeSet.GetType().GetField(attributeName, 
                    System.Reflection.BindingFlags.NonPublic | 
                    System.Reflection.BindingFlags.Public | 
                    System.Reflection.BindingFlags.Static);
                    
                if (field != null && field.FieldType == typeof(string))
                {
                    string actualName = field.GetValue(null) as string;
                    return attributeSet.GetAttributeValue(actualName);
                }
            }
            return 0f;
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        public void SetNumericAttribute(AttributePair attributePair, float newValue)
        {
            foreach (var attributeSet in AttributeSets)
            {
                if (attributeSet.GetAttributeName() == attributePair.AttributeSetName)
                {
                    attributeSet.SetAttributeValue(attributePair.AttributeName, newValue);
                    return;
                }
            }
        }

        /// <summary>
        /// 应用修改器到属性
        /// </summary>
        /// <param name="attributePair">要修改的属性</param>
        /// <param name="op">修改操作类型</param>
        /// <param name="value">修改值</param>
        public void ApplyModiferToAttribute(AttributePair attributePair, EGameplayModOp op, float value)
        {
            string key = $"{attributePair.AttributeSetName}.{attributePair.AttributeName}";
            
            if (!aggregatedModifiers.ContainsKey(key))
            {
                aggregatedModifiers[key] = 0f;
            }

            float currentValue = aggregatedModifiers[key];
            
            // 根据操作类型计算新的修改值
            switch (op)
            {
                case EGameplayModOp.Addition:
                    aggregatedModifiers[key] = currentValue + value;
                    break;
                case EGameplayModOp.Multiply:
                    aggregatedModifiers[key] = currentValue * value;
                    break;
                case EGameplayModOp.Division:
                    aggregatedModifiers[key] = value != 0 ? currentValue / value : currentValue;
                    break;
                case EGameplayModOp.Override:
                    aggregatedModifiers[key] = value;
                    break;
                case EGameplayModOp.Max:
                    aggregatedModifiers[key] = Mathf.Max(currentValue, value);
                    break;
            }

            // 计算最终值 = 基础值 + 修改值
            float baseValue = GetNumericAttribute(attributePair);
            float newValue = baseValue + aggregatedModifiers[key];
            
            SetNumericAttribute(attributePair, newValue);
        }

        #endregion

        #region 标签系统

        /// <summary>
        /// 添加松散标签（临时标签）
        /// </summary>
        /// <param name="tag">要添加的标签</param>
        /// <param name="count">添加次数</param>
        public void AddLooseGameplayTag(GameplayTag tag, int count = 1)
        {
            if (tag == null) return;

            bool hadTag = OwnedTags.HasTag(tag);
            
            for (int i = 0; i < count; i++)
            {
                OwnedTags.AddTag(tag);
            }

            // 如果从无到有，触发事件
            if (!hadTag && OwnedTags.HasTag(tag))
            {
                OnTagChanged?.Invoke(tag, count);
            }
        }

        /// <summary>
        /// 移除松散标签
        /// </summary>
        public void RemoveLooseGameplayTag(GameplayTag tag, int count = 1)
        {
            if (tag == null) return;

            for (int i = 0; i < count; i++)
            {
                OwnedTags.RemoveTag(tag);
            }

            OnTagChanged?.Invoke(tag, -count);
        }

        /// <summary>
        /// 检查是否拥有匹配的标签
        /// </summary>
        public bool HasMatchingGameplayTag(GameplayTag tag)
        {
            return OwnedTags.HasTag(tag);
        }

        /// <summary>
        /// 检查是否拥有任意匹配的标签
        /// </summary>
        public bool HasAnyMatchingGameplayTag(IEnumerable<GameplayTag> tags)
        {
            return OwnedTags.HasAnyTags(tags);
        }

        /// <summary>
        /// 检查是否拥有所有匹配的标签
        /// </summary>
        public bool HasAllMatchingGameplayTags(IEnumerable<GameplayTag> tags)
        {
            return OwnedTags.HasAllTags(tags);
        }

        #endregion

        #region GameplayEffect 系统

        /// <summary>
        /// 对自身应用效果
        /// </summary>
        public int ApplyGameplayEffectToSelf(GameplayEffect gameplayEffect, float level = 1f)
        {
            return ApplyGameplayEffect(gameplayEffect, this, level);
        }

        /// <summary>
        /// 应用效果（可指定施加者）
        /// </summary>
        /// <param name="gameplayEffect">效果定义</param>
        /// <param name="instigatorASC">施加者的 ASC</param>
        /// <param name="level">效果等级</param>
        public int ApplyGameplayEffect(GameplayEffect gameplayEffect, AbilitySystemComponent instigatorASC, float level = 1f)
        {
            if (gameplayEffect == null)
            {
                Debug.LogWarning("Applied null gameplay effect");
                return -1;
            }

            // 创建效果实例
            GameplayEffectSpec spec = new GameplayEffectSpec(gameplayEffect, instigatorASC, this, level);
            
            if (!spec.IsValid())
            {
                Debug.LogWarning("Invalid gameplay effect spec");
                return -1;
            }

            // 检查是否可以应用
            if (!CanApplyGameplayEffect(spec))
            {
                Debug.LogWarning("Cannot apply gameplay effect due to tag requirements");
                return -1;
            }

            return ApplyGameplayEffectSpec(spec);
        }

        /// <summary>
        /// 检查是否可以应用效果（标签要求）
        /// </summary>
        public bool CanApplyGameplayEffect(GameplayEffectSpec spec)
        {
            // 检查持续存在的标签要求
            if (spec.Def.OngoingTagRequirements.Count > 0)
            {
                if (!OwnedTags.HasAllTags(spec.Def.OngoingTagRequirements.tags))
                {
                    return false;
                }
            }

            // 检查应用时的标签要求
            if (spec.Def.ApplicationTagRequirements.Count > 0)
            {
                if (!OwnedTags.HasAllTags(spec.Def.ApplicationTagRequirements.tags))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 应用效果实例
        /// </summary>
        public int ApplyGameplayEffectSpec(GameplayEffectSpec spec)
        {
            int effectHandle = nextEffectHandle++;
            spec.EffectHandle = effectHandle;

            ActiveGameplayEffect activeEffect = new ActiveGameplayEffect(spec, effectHandle);
            activeEffect.OnStackChangedCallbacks.Add(() => OnEffectStackChanged?.Invoke(spec, null));

            if (activeEffects.ContainsKey(effectHandle))
            {
                Debug.LogWarning($"Effect handle {effectHandle} already exists");
                return -1;
            }

            activeEffects[effectHandle] = activeEffect;

            // 添加到标签字典
            AddEffectToTagDictionary(activeEffect);

            // 应用修改器
            ApplyGameplayEffectModifiers(spec);

            // 应用标签
            ApplyGameplayEffectTags(spec);

            // 触发事件
            OnEffectApplied?.Invoke(spec, null);

            return effectHandle;
        }

        /// <summary>
        /// 将效果添加到标签字典（用于按标签查询效果）
        /// </summary>
        private void AddEffectToTagDictionary(ActiveGameplayEffect effect)
        {
            foreach (var tag in effect.Spec.AssetsTags.tags)
            {
                if (!effectByTag.ContainsKey(tag))
                {
                    effectByTag[tag] = new List<ActiveGameplayEffect>();
                }
                effectByTag[tag].Add(effect);
            }
        }

        /// <summary>
        /// 应用效果的属性修改器
        /// </summary>
        private void ApplyGameplayEffectModifiers(GameplayEffectSpec spec)
        {
            foreach (var modifier in spec.Modifiers)
            {
                ApplyModiferToAttribute(
                    modifier.attributePair,
                    modifier.ModifierOp,
                    modifier.Value * spec.BaseMagnitude * spec.StackCount
                );
            }
        }

        /// <summary>
        /// 应用效果授予的标签
        /// </summary>
        private void ApplyGameplayEffectTags(GameplayEffectSpec spec)
        {
            foreach (var tag in spec.Def.GrantedTags.tags)
            {
                AddLooseGameplayTag(tag);
            }
        }

        /// <summary>
        /// 移除激活的效果
        /// </summary>
        public void RemoveActiveGameplayEffect(int effectHandle)
        {
            if (!activeEffects.ContainsKey(effectHandle))
            {
                return;
            }

            ActiveGameplayEffect effect = activeEffects[effectHandle];
            
            // 移除标签
            RemoveGameplayEffectTags(effect.Spec);

            // 从字典中移除
            activeEffects.Remove(effectHandle);
            RemoveEffectFromTagDictionary(effect);

            // 触发事件
            OnEffectRemoved?.Invoke(effect.Spec, null);
            effect.InvokeOnExpired();
        }

        /// <summary>
        /// 移除效果授予的标签
        /// </summary>
        private void RemoveGameplayEffectTags(GameplayEffectSpec spec)
        {
            foreach (var tag in spec.Def.GrantedTags.tags)
            {
                RemoveLooseGameplayTag(tag);
            }
        }

        /// <summary>
        /// 从标签字典中移除效果
        /// </summary>
        private void RemoveEffectFromTagDictionary(ActiveGameplayEffect effect)
        {
            foreach (var tag in effect.Spec.AssetsTags.tags)
            {
                if (effectByTag.ContainsKey(tag))
                {
                    effectByTag[tag].Remove(effect);
                    if (effectByTag[tag].Count == 0)
                    {
                        effectByTag.Remove(tag);
                    }
                }
            }
        }

        /// <summary>
        /// 更新所有激活的效果（每帧调用）
        /// </summary>
        private void UpdateActiveEffects(float deltaTime)
        {
            List<int> effectsToRemove = new List<int>();

            foreach (var kvp in activeEffects)
            {
                ActiveGameplayEffect effect = kvp.Value;
                effect.Update(deltaTime);

                if (effect.IsExpired())
                {
                    effectsToRemove.Add(kvp.Key);
                }
            }

            foreach (int handle in effectsToRemove)
            {
                RemoveActiveGameplayEffect(handle);
            }
        }

        /// <summary>
        /// 获取激活的效果
        /// </summary>
        public ActiveGameplayEffect GetActiveGameplayEffect(int handle)
        {
            activeEffects.TryGetValue(handle, out var effect);
            return effect;
        }

        /// <summary>
        /// 根据标签获取激活的效果列表
        /// </summary>
        public List<ActiveGameplayEffect> GetActiveEffectsByTag(GameplayTag tag)
        {
            if (effectByTag.ContainsKey(tag))
            {
                return new List<ActiveGameplayEffect>(effectByTag[tag]);
            }
            return new List<ActiveGameplayEffect>();
        }

        #endregion

        #region GameplayAbility 系统

        /// <summary>
        /// 给予能力
        /// </summary>
        public int GiveAbility(GameplayAbility ability)
        {
            if (ability == null)
            {
                Debug.LogWarning("Given null ability");
                return -1;
            }

            int abilityHandle = nextAbilityHandle++;
            
            abilities.Add(ability);
            ability.Initialize(this, abilityHandle);
            
            if (ability.AbilityTag != null && !abilitiesByTag.ContainsKey(ability.AbilityTag))
            {
                abilitiesByTag[ability.AbilityTag] = ability;
            }

            return abilityHandle;
        }

        /// <summary>
        /// 批量给予能力
        /// </summary>
        public void GiveAbilities(List<GameplayAbility> abilitiesToGive)
        {
            foreach (var ability in abilitiesToGive)
            {
                GiveAbility(ability);
            }
        }

        /// <summary>
        /// 尝试激活能力（通过标签）
        /// </summary>
        public bool TryActivateAbility(GameplayTag abilityTag)
        {
            if (abilitiesByTag.ContainsKey(abilityTag))
            {
                return TryActivateAbility(abilitiesByTag[abilityTag]);
            }
            return false;
        }

        /// <summary>
        /// 尝试激活能力
        /// </summary>
        public bool TryActivateAbility(GameplayAbility ability)
        {
            if (ability == null || !abilities.Contains(ability))
            {
                return false;
            }

            // 检查能力是否被阻塞
            if (IsAbilityBlocked(ability.AbilityTag))
            {
                return false;
            }

            // 检查是否可以激活
            if (!CanActivateAbility(ability))
            {
                return false;
            }

            return ability.TryActivate();
        }

        /// <summary>
        /// 检查能力是否可以激活
        /// </summary>
        public bool CanActivateAbility(GameplayAbility ability)
        {
            if (ability == null) return false;
            return ability.CanActivate();
        }

        /// <summary>
        /// 检查能力是否被阻塞
        /// </summary>
        public bool IsAbilityBlocked(GameplayTag abilityTag)
        {
            return blockedAbilities.ContainsKey(abilityTag) && blockedAbilities[abilityTag] > 0;
        }

        /// <summary>
        /// 阻塞能力（增加阻塞计数）
        /// </summary>
        public void BlockAbility(GameplayTag abilityTag)
        {
            if (!blockedAbilities.ContainsKey(abilityTag))
            {
                blockedAbilities[abilityTag] = 0;
            }
            blockedAbilities[abilityTag]++;
        }

        /// <summary>
        /// 解除阻塞能力（减少阻塞计数）
        /// </summary>
        public void UnblockAbility(GameplayTag abilityTag)
        {
            if (blockedAbilities.ContainsKey(abilityTag))
            {
                blockedAbilities[abilityTag]--;
                if (blockedAbilities[abilityTag] <= 0)
                {
                    blockedAbilities.Remove(abilityTag);
                }
            }
        }

        /// <summary>
        /// 取消指定能力
        /// </summary>
        public void CancelAbility(GameplayAbility ability)
        {
            if (ability != null && ability.IsActive())
            {
                ability.Cancel();
            }
        }

        /// <summary>
        /// 取消所有能力
        /// </summary>
        public void CancelAllAbilities()
        {
            foreach (var ability in abilities)
            {
                if (ability.IsActive())
                {
                    ability.Cancel();
                }
            }
        }

        /// <summary>
        /// 更新所有能力（每帧调用）
        /// </summary>
        private void UpdateAbilities(float deltaTime)
        {
            foreach (var ability in abilities)
            {
                if (ability.IsActive())
                {
                    ability.TickAbility(deltaTime);
                }
            }
        }

        /// <summary>
        /// 获取所有可激活的能力
        /// </summary>
        public List<GameplayAbility> GetActivatableAbilities()
        {
            List<GameplayAbility> activatable = new List<GameplayAbility>();
            foreach (var ability in abilities)
            {
                if (CanActivateAbility(ability))
                {
                    activatable.Add(ability);
                }
            }
            return activatable;
        }

        /// <summary>
        /// 内部方法 - 通知能力已激活
        /// </summary>
        internal void NotifyAbilityActivated(GameplayAbility ability)
        {
            OnAbilityActivated?.Invoke(ability);
        }

        /// <summary>
        /// 内部方法 - 通知能力已结束
        /// </summary>
        internal void NotifyAbilityEnded(GameplayAbility ability)
        {
            OnAbilityEnded?.Invoke(ability);
        }

        #endregion

        /// <summary>
        /// 组件销毁时清理事件订阅
        /// </summary>
        private void OnDestroy()
        {
            foreach (var attributeSet in AttributeSets)
            {
                if (attributeSet != null)
                {
                    attributeSet.OnAttributeChanged -= HandleAttributeChanged;
                }
            }
        }

        /// <summary>
        /// 编辑器验证回调
        /// </summary>
        private void OnValidate()
        {
            if (AttributeSets == null)
            {
                AttributeSets = new List<AttributeSet>();
            }
        }
    }
}
