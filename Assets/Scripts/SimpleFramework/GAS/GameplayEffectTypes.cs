using System;
using UnityEngine;

namespace SimpleFramework.GAS
{
    /// <summary>
    /// 游戏玩法修改器操作类型
    /// </summary>
    public enum EGameplayModOp
    {
        /// <summary> 加法：Base + Delta </summary>
        Addition,       
        /// <summary> 乘法：Base * Delta </summary>
        Multiply,       
        /// <summary> 除法：Base / Delta </summary>
        Division,       
        /// <summary> 覆盖：直接替换为 Delta </summary>
        Override,       
        /// <summary> 取最大值：Max(Base, Delta) </summary>
        Max             
    }

    /// <summary>
    /// 游戏玩法修改器评估 - 定义如何修改一个属性
    /// </summary>
    [Serializable]
    public struct GameplayModifierEvaluation
    {
        /// <summary> 修改器的标签 </summary>
        public GameplayTag ModifierTag;
        /// <summary> 修改操作类型（加/乘/除等） </summary>
        public EGameplayModOp ModifierOp;
        /// <summary> 修改的数值 </summary>
        public float Value;
        /// <summary> 要修改的属性对（属性集名称。属性名） </summary>
        public AttributePair attributePair;

        public GameplayModifierEvaluation(GameplayTag tag, EGameplayModOp op, float val, AttributePair attrPair)
        {
            ModifierTag = tag;
            ModifierOp = op;
            Value = val;
            attributePair = attrPair;
        }
    }

    /// <summary>
    /// 属性对 - 用于定位特定的属性（属性集名称 + 属性名）
    /// </summary>
    [Serializable]
    public struct AttributePair
    {
        /// <summary> 属性集的名称（如 "HealthSet"） </summary>
        public string AttributeSetName;
        /// <summary> 属性的名称（如 "Health"） </summary>
        public string AttributeName;

        public AttributePair(string assetName, string attrName)
        {
            AttributeSetName = assetName;
            AttributeName = attrName;
        }
    }

    /// <summary>
    /// 游戏玩法效果属性捕获规范 - 定义如何从源或目标捕获属性值
    /// </summary>
    [Serializable]
    public struct GameplayEffectAttributeCaptureSpec
    {
        /// <summary> 要捕获的后端属性名称 </summary>
        public string BackendAttribute; 
        /// <summary> 是否应该四舍五入到最大值 </summary>
        public bool bShouldSnapToMax;

        public GameplayEffectAttributeCaptureSpec(string backendAttr, bool snapToMax)
        {
            BackendAttribute = backendAttr;
            bShouldSnapToMax = snapToMax;
        }
    }

    /// <summary>
    /// 游戏玩法效果属性捕获定义 - 完整定义属性捕获的规则
    /// </summary>
    [Serializable]
    public struct GameplayEffectAttributeCaptureDefinition
    {
        /// <summary> 要捕获的属性 </summary>
        public AttributePair AttributeToCapture;
        /// <summary> 源（施加者）的捕获规范 </summary>
        public GameplayEffectAttributeCaptureSpec SourceSpec;
        /// <summary> 目标（承受者）的捕获规范 </summary>
        public GameplayEffectAttributeCaptureSpec TargetSpec;
        /// <summary> 是否使用改变后的快照 </summary>
        public bool bAlteredSnapshot;

        public GameplayEffectAttributeCaptureDefinition(
            AttributePair attrPair,
            GameplayEffectAttributeCaptureSpec sourceSpec,
            GameplayEffectAttributeCaptureSpec targetSpec,
            bool alteredSnapshot)
        {
            AttributeToCapture = attrPair;
            SourceSpec = sourceSpec;
            TargetSpec = targetSpec;
            bAlteredSnapshot = alteredSnapshot;
        }
    }

    /// <summary>
    /// 游戏玩法效果持续时间类型
    /// </summary>
    public enum EGameplayEffectDurationType
    {
        /// <summary> 瞬间生效，立即应用并结束 </summary>
        Instant,    
        /// <summary> 持续一段时间，期间持续生效 </summary>
        Duration,   
        /// <summary> 永久生效，直到被手动移除 </summary>
        Infinite    
    }

    /// <summary>
    /// 游戏玩法效果堆叠类型 - 定义相同效果如何堆叠
    /// </summary>
    public enum EGameplayEffectStackingType
    {
        /// <summary> 不堆叠，新效果替换旧效果 </summary>
        None,           
        /// <summary> 按来源堆叠（同一来源的效果可堆叠） </summary>
        AggregateBySource,  
        /// <summary> 按目标堆叠（同一目标的效果可堆叠） </summary>
        AggregateByTarget   
    }
}
