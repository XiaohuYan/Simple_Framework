using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleFramework.GAS
{
    /// <summary>
    /// 游戏玩法能力输入 ID - 定义各种输入动作
    /// </summary>
    public enum EGameplayAbilityInputID
    {
        None = 0,
        Ability1,
        Ability2,
        Ability3,
        Ability4,
        Ultimate,
        Jump,
        Sprint,
        Crouch,
        Reload,
        Interact,
        Max
    }

    /// <summary>
    /// 游戏玩法能力触发源 - 定义能力如何被触发
    /// </summary>
    public enum EGameplayAbilityTriggerSource
    {
        GameplayEvent,              // 游戏事件触发
        AttributeValueBaseChanged,  // 属性基础值改变
        AttributeValueRatioChanged, // 属性比例改变
        AttributeValueThreshold,    // 属性阈值
        GameplayTagAdded,           // 标签添加
        GameplayTagRemoved,         // 标签移除
        InputPressed,               // 输入按下
        InputReleased,              // 输入释放
        InputHeld                   // 输入按住
    }

    /// <summary>
    /// 游戏玩法能力 - 定义角色可以执行的动作
    /// 类似于 Unreal GAS 中的 GameplayAbility
    /// </summary>
    [CreateAssetMenu(fileName = "New GameplayAbility", menuName = "GAS/GameplayAbility")]
    public class GameplayAbility : ScriptableObject
    {
        #region 基础设置

        /// <summary> 能力名称 </summary>
        [Header("基础设置")]
        public string AbilityName;
        /// <summary> 能力的标签 </summary>
        public GameplayTag AbilityTag;
        /// <summary> 能力的标签集合 </summary>
        public GameplayTagContainer AbilityTags = new GameplayTagContainer();
        /// <summary> 会取消此能力的标签 </summary>
        public GameplayTagContainer CancelAbilityTags = new GameplayTagContainer();
        /// <summary> 会阻塞此能力的标签 </summary>
        public GameplayTagContainer BlockAbilityTags = new GameplayTagContainer();

        #endregion

        #region 触发设置

        /// <summary> 触发类型 </summary>
        [Header("触发设置")]
        public EGameplayAbilityTriggerSource TriggerType = EGameplayAbilityTriggerSource.InputPressed;
        /// <summary> 输入 ID </summary>
        public EGameplayAbilityInputID InputID = EGameplayAbilityInputID.None;

        #endregion

        #region 消耗设置

        /// <summary> 消耗数值 </summary>
        [Header("消耗设置")]
        public float Cost = 0f;
        /// <summary> 消耗的属性 </summary>
        public AttributePair CostAttribute;
        /// <summary> 是否无消耗 </summary>
        public bool bNoCost = false;

        #endregion

        #region 冷却设置

        /// <summary> 冷却时间 </summary>
        [Header("冷却设置")]
        public float CooldownTime = 0f;
        /// <summary> 冷却标签（冷却期间会添加此标签） </summary>
        public GameplayTag CooldownTag;

        #endregion

        #region 执行设置

        /// <summary> 激活持续时间 </summary>
        [Header("执行设置")]
        public float ActivationDuration = 0f;
        /// <summary> 是否实例化 </summary>
        public bool bInstanced = true;
        /// <summary> 服务器是否重置远程取消 </summary>
        public bool bServerResetsRemoteCancellation = true;
        /// <summary> 是否忽略拥有的标签 </summary>
        public bool bIgnoreOwnedTags = false;

        #endregion

        #region 目标设置

        /// <summary> 是否需要目标 </summary>
        [Header("目标设置")]
        public bool bRequiresTarget = false;
        /// <summary> 目标范围 </summary>
        public float TargetRange = 100f;

        #endregion

        #region 运行时数据

        /// <summary> 能力系统组件 </summary>
        protected AbilitySystemComponent AbilitySystemComponent;
        /// <summary> 能力句柄 </summary>
        protected int AbilityHandle;
        /// <summary> 激活的角色 </summary>
        protected GameObject ActivationActor;
        /// <summary> 激活的目标 </summary>
        protected GameObject ActivationTarget;
        
        /// <summary> 是否正在激活 </summary>
        protected bool bIsActive = false;
        /// <summary> 是否已结束 </summary>
        protected bool bIsEnded = false;
        /// <summary> 激活时间 </summary>
        protected float ActivationTime;
        /// <summary> 剩余时间 </summary>
        protected float TimeRemaining;
        /// <summary> 剩余冷却时间 </summary>
        protected float CooldownRemaining;
        
        /// <summary> 已应用的效果列表 </summary>
        protected List<GameplayEffectSpec> appliedEffects = new List<GameplayEffectSpec>();
        /// <summary> 能力结束时的回调列表 </summary>
        protected List<Action> OnAbilityEndedCallbacks = new List<Action>();

        #endregion

        /// <summary>
        /// 初始化能力
        /// </summary>
        public virtual void Initialize(AbilitySystemComponent asc, int handle)
        {
            AbilitySystemComponent = asc;
            AbilityHandle = handle;
        }

        /// <summary>
        /// 检查是否可以激活
        /// </summary>
        public virtual bool CanActivate()
        {
            if (AbilitySystemComponent == null) return false;
            if (bIsActive) return false;
            if (CooldownRemaining > 0) return false;

            if (!bIgnoreOwnedTags)
            {
                // 检查是否有取消标签
                if (AbilitySystemComponent.OwnedTags.HasAnyTags(CancelAbilityTags.tags))
                {
                    return false;
                }

                // 检查是否有阻塞标签
                if (AbilitySystemComponent.OwnedTags.HasAnyTags(BlockAbilityTags.tags))
                {
                    return false;
                }
            }

            // 检查消耗是否足够
            if (!bNoCost && Cost > 0)
            {
                float currentValue = AbilitySystemComponent.GetNumericAttribute(CostAttribute);
                if (currentValue < Cost) return false;
            }

            return true;
        }

        /// <summary>
        /// 尝试激活能力
        /// </summary>
        public virtual bool TryActivate()
        {
            if (!CanActivate())
            {
                return false;
            }

            // 扣除消耗
            if (!bNoCost && Cost > 0)
            {
                AbilitySystemComponent.SetNumericAttribute(CostAttribute, 
                    AbilitySystemComponent.GetNumericAttribute(CostAttribute) - Cost);
            }

            ActivateAbility();
            return true;
        }

        /// <summary>
        /// 激活能力（由子类重写）
        /// </summary>
        protected virtual void ActivateAbility()
        {
            bIsActive = true;
            bIsEnded = false;
            ActivationTime = Time.time;
            TimeRemaining = ActivationDuration > 0 ? ActivationDuration : float.MaxValue;

            if (ActivationDuration > 0)
            {
                StartCooldown();
            }

            // 应用激活效果
            ApplyActivationEffects();

            // 通知 ASC
            AbilitySystemComponent.NotifyAbilityActivated(this);
        }

        /// <summary>
        /// 应用激活效果（由子类重写）
        /// </summary>
        protected virtual void ApplyActivationEffects()
        {
        }

        /// <summary>
        /// 对自身应用效果
        /// </summary>
        protected void ApplyGameplayEffectToOwner(GameplayEffect effect, float level = 1f)
        {
            if (effect != null)
            {
                int handle = AbilitySystemComponent.ApplyGameplayEffectToSelf(effect, level);
                if (handle > 0)
                {
                    GameplayEffectSpec spec = AbilitySystemComponent.GetActiveGameplayEffect(handle).Spec;
                    appliedEffects.Add(spec);
                }
            }
        }

        /// <summary>
        /// 对目标应用效果
        /// </summary>
        protected void ApplyGameplayEffectToTarget(GameplayEffect effect, AbilitySystemComponent targetASC, float level = 1f)
        {
            if (effect != null && targetASC != null)
            {
                targetASC.ApplyGameplayEffect(effect, AbilitySystemComponent, level);
            }
        }

        /// <summary>
        /// 取消能力
        /// </summary>
        public virtual void Cancel()
        {
            if (!bIsActive) return;

            CancelActivationEffects();

            EndAbility();
        }

        /// <summary>
        /// 取消激活效果（由子类重写）
        /// </summary>
        protected virtual void CancelActivationEffects()
        {
            foreach (var effectSpec in appliedEffects)
            {
                // 瞬间效果无法取消
                if (effectSpec.Def.IsInstant())
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// 结束能力
        /// </summary>
        public virtual void EndAbility()
        {
            if (bIsEnded) return;

            bIsActive = false;
            bIsEnded = true;

            // 如果没有冷却时间，结束时开始冷却
            if (ActivationDuration > 0 && CooldownTime <= 0)
            {
                StartCooldown();
            }

            // 通知 ASC
            AbilitySystemComponent.NotifyAbilityEnded(this);

            // 触发结束回调
            foreach (var callback in OnAbilityEndedCallbacks)
            {
                callback?.Invoke();
            }
        }

        /// <summary>
        /// 开始冷却
        /// </summary>
        protected void StartCooldown()
        {
            if (CooldownTime > 0)
            {
                CooldownRemaining = CooldownTime;
                
                // 添加冷却标签
                if (CooldownTag != null)
                {
                    AbilitySystemComponent.AddLooseGameplayTag(CooldownTag);
                }
            }
        }

        /// <summary>
        /// 更新能力（每帧调用）
        /// </summary>
        public virtual void TickAbility(float deltaTime)
        {
            if (!bIsActive) return;

            // 更新持续时间
            if (ActivationDuration > 0 && ActivationDuration < float.MaxValue)
            {
                TimeRemaining -= deltaTime;
                if (TimeRemaining <= 0)
                {
                    EndAbility();
                }
            }

            // 更新冷却时间
            if (CooldownRemaining > 0)
            {
                CooldownRemaining -= deltaTime;
                if (CooldownRemaining <= 0)
                {
                    CooldownRemaining = 0;
                    
                    // 冷却结束，移除标签
                    if (CooldownTag != null)
                    {
                        AbilitySystemComponent.RemoveLooseGameplayTag(CooldownTag);
                    }
                }
            }
        }

        /// <summary>
        /// 检查能力是否正在激活
        /// </summary>
        public bool IsActive() => bIsActive;

        /// <summary>
        /// 检查能力是否已结束
        /// </summary>
        public bool IsEnded() => bIsEnded;

        /// <summary>
        /// 获取剩余冷却时间
        /// </summary>
        public float GetRemainingCooldown() => Mathf.Max(0, CooldownRemaining);

        /// <summary>
        /// 获取冷却进度（0-1）
        /// </summary>
        public float GetCooldownProgress()
        {
            if (CooldownTime <= 0) return 0;
            return 1f - (CooldownRemaining / CooldownTime);
        }

        /// <summary>
        /// 添加能力结束回调
        /// </summary>
        public void AddOnEndedCallback(Action callback)
        {
            if (!OnAbilityEndedCallbacks.Contains(callback))
            {
                OnAbilityEndedCallbacks.Add(callback);
            }
        }

        /// <summary>
        /// 移除能力结束回调
        /// </summary>
        public void RemoveOnEndedCallback(Action callback)
        {
            OnAbilityEndedCallbacks.Remove(callback);
        }

        /// <summary>
        /// 输入按下时调用
        /// </summary>
        public virtual void InputPressed()
        {
            if (TriggerType == EGameplayAbilityTriggerSource.InputPressed)
            {
                TryActivate();
            }
        }

        /// <summary>
        /// 输入释放时调用
        /// </summary>
        public virtual void InputReleased()
        {
            if (TriggerType == EGameplayAbilityTriggerSource.InputReleased)
            {
                TryActivate();
            }
            else if (TriggerType == EGameplayAbilityTriggerSource.InputHeld && bIsActive)
            {
                Cancel();
            }
        }

        /// <summary>
        /// 输入按住时调用
        /// </summary>
        public virtual void OnInputHeld()
        {
            if (TriggerType == EGameplayAbilityTriggerSource.InputHeld && !bIsActive)
            {
                TryActivate();
            }
        }
    }
}
