using System.Collections.Generic;
using UnityEngine;

namespace SimpleFramework.GAS
{
    /// <summary>
    /// 游戏玩法效果 - 定义对属性、标签、能力等的影响
    /// 类似于 Unreal GAS 中的 GameplayEffect
    /// </summary>
    [CreateAssetMenu(fileName = "New GameplayEffect", menuName = "GAS/GameplayEffect")]
    public class GameplayEffect : ScriptableObject
    {
        #region 基础设置

        /// <summary> 效果名称 </summary>
        [Header("基础设置")]
        public string EffectName;
        /// <summary> 效果的标签集合（用于分类和搜索） </summary>
        public GameplayTagContainer AssetTags = new GameplayTagContainer();
        /// <summary> 拥有此效果的标签 </summary>
        public GameplayTagContainer OwningTag = new GameplayTagContainer();

        #endregion

        #region 持续时间

        /// <summary> 持续时间类型（瞬间/持续/永久） </summary>
        [Header("持续时间")]
        public EGameplayEffectDurationType DurationPolicy = EGameplayEffectDurationType.Instant;
        /// <summary> 效果持续时间（秒） </summary>
        public float Duration = 1f;
        /// <summary> 周期性 tick 间隔（0 表示不 tick） </summary>
        public float Period = 0f; 

        #endregion

        #region 堆叠

        /// <summary> 堆叠类型 </summary>
        [Header("堆叠")]
        public EGameplayEffectStackingType StackingType = EGameplayEffectStackingType.None;
        /// <summary> 最大堆叠层数 </summary>
        public int StackLimit = 1;

        #endregion

        #region 属性修改器

        /// <summary> 属性修改器列表（定义如何修改属性） </summary>
        [Header("属性修改器")]
        public List<GameplayModifierEvaluation> Modifiers = new List<GameplayModifierEvaluation>();

        #endregion

        #region 属性捕获

        /// <summary> 属性捕获定义列表（从源或目标捕获属性值） </summary>
        [Header("属性捕获")]
        public List<GameplayEffectAttributeCaptureDefinition> AttributeCaptureDefinitions = new List<GameplayEffectAttributeCaptureDefinition>();

        #endregion

        #region 执行器

        /// <summary> 执行计算列表（用于复杂的效果计算） </summary>
        [Header("执行器")]
        public List<ExecutionCalculation> ExecutionCalculations = new List<ExecutionCalculation>();

        #endregion

        #region 其他

        /// <summary> 是否可以加法堆叠 </summary>
        [Header("其他")]
        public bool bCanStackAdditively = true;
        /// <summary> 应用效果时授予的标签 </summary>
        public GameplayTagContainer GrantedTags = new GameplayTagContainer();
        /// <summary> 持续存在的标签要求（不满足时效果会被移除） </summary>
        public GameplayTagContainer OngoingTagRequirements = new GameplayTagContainer();
        /// <summary> 应用时的标签要求（不满足时无法应用） </summary>
        public GameplayTagContainer ApplicationTagRequirements = new GameplayTagContainer();

        #endregion

        /// <summary>
        /// 是否为瞬间效果
        /// </summary>
        public bool IsInstant() => DurationPolicy == EGameplayEffectDurationType.Instant;

        /// <summary>
        /// 是否为持续效果
        /// </summary>
        public bool IsDuration() => DurationPolicy == EGameplayEffectDurationType.Duration;

        /// <summary>
        /// 是否为永久效果
        /// </summary>
        public bool IsInfinite() => DurationPolicy == EGameplayEffectDurationType.Infinite;

        /// <summary>
        /// 计算效果的基础量级
        /// </summary>
        public float CalculateBaseMagnitude(GameplayEffectSpec spec)
        {
            float magnitude = 1f;

            // 累加所有加法修改器的值
            foreach (var modifier in Modifiers)
            {
                if (modifier.ModifierOp == EGameplayModOp.Addition)
                {
                    magnitude += modifier.Value;
                }
            }

            return magnitude;
        }
    }

    /// <summary>
    /// 执行计算 - 用于在特定时机执行自定义计算逻辑
    /// </summary>
    [System.Serializable]
    public class ExecutionCalculation
    {
        /// <summary> 计算类的名称（用于反射创建实例） </summary>
        public string CalculationClassName;
        /// <summary> 执行时机 </summary>
        public ExecutionType ExecutionType = ExecutionType.OnApplication;
    }

    /// <summary>
    /// 执行类型 - 定义执行计算的时机
    /// </summary>
    public enum ExecutionType
    {
        /// <summary> 应用效果时执行 </summary>
        OnApplication,
        /// <summary> 周期性 tick 时执行 </summary>
        OnPeriodicTick,
        /// <summary> 移除效果时执行 </summary>
        OnRemove
    }
}
