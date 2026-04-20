using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleFramework.GAS
{
    /// <summary>
    /// 游戏玩法效果实例 - GameplayEffect 的运行时实例
    /// 包含效果的所有运行时数据和状态
    /// </summary>
    public class GameplayEffectSpec
    {
        /// <summary> 效果的定义（ScriptableObject 资源） </summary>
        public GameplayEffect Def;
        /// <summary> 修改器列表 </summary>
        public List<GameplayModifierEvaluation> Modifiers = new List<GameplayModifierEvaluation>();
        /// <summary> 属性捕获列表 </summary>
        public List<GameplayEffectAttributeCaptureSpec> AttributeCaptures = new List<GameplayEffectAttributeCaptureSpec>();
        
        /// <summary> 效果等级 </summary>
        public float Level = 1f;
        /// <summary> 持续时间 </summary>
        public float Duration = 0f;
        /// <summary> 周期性 tick 间隔 </summary>
        public float Period = 0f;
        /// <summary> 基础量级 </summary>
        public float BaseMagnitude = 0f;
        /// <summary> 堆叠层数 </summary>
        public int StackCount = 1;
        
        /// <summary> 效果的标签集合 </summary>
        public GameplayTagContainer AssetsTags = new GameplayTagContainer();
        /// <summary> 目标的标签集合 </summary>
        public GameplayTagContainer TargetTags = new GameplayTagContainer();
        /// <summary> 来源的标签集合 </summary>
        public GameplayTagContainer SourceTags = new GameplayTagContainer();
        
        /// <summary> 效果的施加者 </summary>
        public GameObject EffectCauser;
        /// <summary> 效果的目标 </summary>
        public GameObject Target;
        /// <summary> 效果的句柄（唯一标识） </summary>
        public int EffectHandle;
        /// <summary> 是否为永久效果 </summary>
        public bool bIsInfinite;
        /// <summary> 剩余时间 </summary>
        public float RemainingTime;
        /// <summary> 已经过的时间 </summary>
        public float TimeElapsed;

        /// <summary> 拥有此效果的 ASC（目标） </summary>
        public AbilitySystemComponent OwningASC;
        /// <summary> 施加此效果的 ASC（来源） </summary>
        public AbilitySystemComponent InstigatorASC;

        /// <summary>
        /// 构造函数 - 从效果定义创建实例
        /// </summary>
        public GameplayEffectSpec(GameplayEffect effect, AbilitySystemComponent sourceASC, AbilitySystemComponent targetASC, float inLevel = 1f)
        {
            Def = effect;
            OwningASC = targetASC;
            InstigatorASC = sourceASC;
            Level = inLevel;
            
            if (Def != null)
            {
                // 复制标签
                AssetsTags.CopyFrom(Def.AssetTags);
                Duration = Def.Duration;
                Period = Def.Period;
                bIsInfinite = Def.IsInfinite();
                
                // 复制修改器
                foreach (var modifier in Def.Modifiers)
                {
                    Modifiers.Add(new GameplayModifierEvaluation(
                        modifier.ModifierTag,
                        modifier.ModifierOp,
                        modifier.Value,
                        modifier.attributePair
                    ));
                }
                
                // 计算基础量级
                BaseMagnitude = Def.CalculateBaseMagnitude(this);
            }
        }

        /// <summary>
        /// 添加修改器标签
        /// </summary>
        public void AddModifierTag(GameplayTag tag)
        {
            AssetsTags.AddTag(tag);
        }

        /// <summary>
        /// 添加目标标签
        /// </summary>
        public void AddTargetTag(GameplayTag tag)
        {
            TargetTags.AddTag(tag);
        }

        /// <summary>
        /// 添加来源标签
        /// </summary>
        public void AddSourceTag(GameplayTag tag)
        {
            SourceTags.AddTag(tag);
        }

        /// <summary>
        /// 检查实例是否有效
        /// </summary>
        public bool IsValid()
        {
            return Def != null && OwningASC != null;
        }

        /// <summary>
        /// 获取持续时间
        /// </summary>
        public float GetDuration()
        {
            if (bIsInfinite) return float.MaxValue;
            return Duration;
        }

        /// <summary>
        /// 获取周期时间
        /// </summary>
        public float GetPeriod()
        {
            return Period;
        }

        /// <summary>
        /// 获取基础量级
        /// </summary>
        public float GetBaseMagnitude()
        {
            return BaseMagnitude;
        }

        /// <summary>
        /// 获取堆叠层数
        /// </summary>
        public float GetStackCount()
        {
            return StackCount;
        }

        /// <summary>
        /// 设置堆叠层数
        /// </summary>
        public void SetStackCount(int count)
        {
            StackCount = Mathf.Max(1, count);
        }

        /// <summary>
        /// 增加堆叠层数
        /// </summary>
        public void IncrementStack()
        {
            if (Def.StackingType != EGameplayEffectStackingType.None)
            {
                if (StackCount < Def.StackLimit)
                {
                    StackCount++;
                }
            }
        }

        /// <summary>
        /// 检查是否可以堆叠
        /// </summary>
        public bool CanStack()
        {
            if (Def.StackingType == EGameplayEffectStackingType.None) return false;
            return StackCount < Def.StackLimit;
        }

        /// <summary>
        /// 创建副本
        /// </summary>
        public GameplayEffectSpec CreateCopy()
        {
            var copy = new GameplayEffectSpec(Def, InstigatorASC, OwningASC, Level);
            copy.Modifiers = new List<GameplayModifierEvaluation>(Modifiers);
            copy.AssetsTags = new GameplayTagContainer();
            copy.AssetsTags.CopyFrom(AssetsTags);
            copy.TargetTags = new GameplayTagContainer();
            copy.TargetTags.CopyFrom(TargetTags);
            copy.SourceTags = new GameplayTagContainer();
            copy.SourceTags.CopyFrom(SourceTags);
            copy.EffectCauser = EffectCauser;
            copy.Target = Target;
            copy.StackCount = StackCount;
            return copy;
        }
    }

    /// <summary>
    /// 激活中的游戏玩法效果 - 正在生效的效果实例
    /// </summary>
    public struct ActiveGameplayEffect
    {
        /// <summary> 效果实例 </summary>
        public GameplayEffectSpec Spec;
        /// <summary> 效果句柄 </summary>
        public int Handle;
        /// <summary> 开始时间 </summary>
        public float StartTime;
        /// <summary> 持续时间 </summary>
        public float Duration;
        /// <summary> 剩余时间 </summary>
        public float RemainingTime;
        /// <summary> 是否为永久效果 </summary>
        public bool bIsInfinite;
        /// <summary> 堆叠层数 </summary>
        public int StackCount;
        /// <summary> 效果过期时的回调列表 </summary>
        public List<Action> OnExpiredCallbacks;
        /// <summary> 堆叠变化时的回调列表 </summary>
        public List<Action> OnStackChangedCallbacks;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ActiveGameplayEffect(GameplayEffectSpec spec, int handle)
        {
            Spec = spec;
            Handle = handle;
            StartTime = Time.time;
            Duration = spec.GetDuration();
            RemainingTime = Duration;
            bIsInfinite = spec.bIsInfinite;
            StackCount = spec.StackCount;
            OnExpiredCallbacks = new List<Action>();
            OnStackChangedCallbacks = new List<Action>();
        }

        /// <summary>
        /// 更新效果（每帧调用）
        /// </summary>
        public void Update(float deltaTime)
        {
            if (!bIsInfinite && RemainingTime > 0 && RemainingTime < float.MaxValue)
            {
                RemainingTime -= deltaTime;
                if (RemainingTime < 0) RemainingTime = 0;
            }
        }

        /// <summary>
        /// 检查效果是否已过期
        /// </summary>
        public bool IsExpired()
        {
            return !bIsInfinite && RemainingTime <= 0;
        }

        /// <summary>
        /// 添加过期回调
        /// </summary>
        public void AddOnExpiredCallback(Action callback)
        {
            if (!OnExpiredCallbacks.Contains(callback))
            {
                OnExpiredCallbacks.Add(callback);
            }
        }

        /// <summary>
        /// 添加堆叠变化回调
        /// </summary>
        public void AddOnStackChangedCallback(Action callback)
        {
            if (!OnStackChangedCallbacks.Contains(callback))
            {
                OnStackChangedCallbacks.Add(callback);
            }
        }

        /// <summary>
        /// 触发所有过期回调
        /// </summary>
        public void InvokeOnExpired()
        {
            foreach (var callback in OnExpiredCallbacks)
            {
                callback?.Invoke();
            }
        }

        /// <summary>
        /// 触发所有堆叠变化回调
        /// </summary>
        public void InvokeOnStackChanged()
        {
            foreach (var callback in OnStackChangedCallbacks)
            {
                callback?.Invoke();
            }
        }
    }
}
