using System;
using UnityEngine;

namespace SimpleFramework.Persistence
{
    /// <summary>
    /// 可保存的数据基类
    /// </summary>
    [Serializable]
    public abstract class SaveDataBase
    {
        /// <summary>
        /// 数据版本（用于数据迁移）
        /// </summary>
        public int DataVersion = 1;

        /// <summary>
        /// 最后保存时间
        /// </summary>
        public string LastSaveTime;

        /// <summary>
        /// 保存前调用
        /// </summary>
        public virtual void OnBeforeSave()
        {
            LastSaveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 加载后调用
        /// </summary>
        public virtual void OnAfterLoad()
        {
        }

        /// <summary>
        /// 数据验证
        /// </summary>
        public virtual bool Validate()
        {
            return true;
        }
    }

    ///// <summary>
    ///// 玩家数据示例
    ///// </summary>
    //[Serializable]
    //public class PlayerData : SaveDataBase
    //{
    //    /// <summary>
    //    /// 玩家 ID
    //    /// </summary>
    //    public string PlayerId;

    //    /// <summary>
    //    /// 玩家名称
    //    /// </summary>
    //    public string PlayerName;

    //    /// <summary>
    //    /// 玩家等级
    //    /// </summary>
    //    public int Level;

    //    /// <summary>
    //    /// 经验值
    //    /// </summary>
    //    public long Experience;

    //    /// <summary>
    //    /// 金币
    //    /// </summary>
    //    public int Gold;

    //    /// <summary>
    //    /// 钻石
    //    /// </summary>
    //    public int Diamond;

    //    /// <summary>
    //    /// 当前关卡
    //    /// </summary>
    //    public int CurrentStage;

    //    /// <summary>
    //    /// 最高关卡
    //    /// </summary>
    //    public int MaxStage;

    //    /// <summary>
    //    /// 创建时间
    //    /// </summary>
    //    public string CreateTime;

    //    /// <summary>
    //    /// 最后登录时间
    //    /// </summary>
    //    public string LastLoginTime;

    //    /// <summary>
    //    /// 总游戏时间（秒）
    //    /// </summary>
    //    public long TotalPlayTime;

    //    public PlayerData()
    //    {
    //        PlayerId = Guid.NewGuid().ToString();
    //        CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    //    }

    //    public override void OnBeforeSave()
    //    {
    //        base.OnBeforeSave();
    //        LastLoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    //    }

    //    public override void OnAfterLoad()
    //    {
    //        base.OnAfterLoad();
    //        UnityEngine.Debug.Log($"[PlayerData] 加载玩家数据：{PlayerName}, 等级：{Level}");
    //    }
    //}

    ///// <summary>
    ///// 游戏设置数据
    ///// </summary>
    //[Serializable]
    //public class GameSettingsData : SaveDataBase
    //{
    //    /// <summary>
    //    /// 音乐音量（0-1）
    //    /// </summary>
    //    public float MusicVolume = 1f;

    //    /// <summary>
    //    /// 音效音量（0-1）
    //    /// </summary>
    //    public float SFXVolume = 1f;

    //    /// <summary>
    //    /// 语音音量（0-1）
    //    /// </summary>
    //    public float VoiceVolume = 1f;

    //    /// <summary>
    //    /// 画面质量（0-5）
    //    /// </summary>
    //    public int GraphicsQuality = 3;

    //    /// <summary>
    //    /// 是否全屏
    //    /// </summary>
    //    public bool FullScreen = true;

    //    /// <summary>
    //    /// 屏幕宽度
    //    /// </summary>
    //    public int ScreenWidth = 1920;

    //    /// <summary>
    //    /// 屏幕高度
    //    /// </summary>
    //    public int ScreenHeight = 1080;

    //    /// <summary>
    //    /// 是否垂直同步
    //    /// </summary>
    //    public bool VSync = true;

    //    /// <summary>
    //    /// 帧率限制（0=无限制）
    //    /// </summary>
    //    public int FrameRateLimit = 60;

    //    /// <summary>
    //    /// 语言索引
    //    /// </summary>
    //    public int LanguageIndex = 1;

    //    /// <summary>
    //    /// 是否显示教程
    //    /// </summary>
    //    public bool ShowTutorial = true;

    //    public override void OnAfterLoad()
    //    {
    //        base.OnAfterLoad();
    //        UnityEngine.Debug.Log($"[GameSettingsData] 加载设置数据");
    //    }
    //}

    ///// <summary>
    ///// 存档数据
    ///// </summary>
    //[Serializable]
    //public class SaveGameData : SaveDataBase
    //{
    //    /// <summary>
    //    /// 存档名称
    //    /// </summary>
    //    public string SaveName;

    //    /// <summary>
    //    /// 存档索引
    //    /// </summary>
    //    public int SaveIndex;

    //    /// <summary>
    //    /// 玩家数据
    //    /// </summary>
    //    public PlayerData PlayerData;

    //    /// <summary>
    //    /// 游戏设置
    //    /// </summary>
    //    public GameSettingsData SettingsData;

    //    /// <summary>
    //    /// 额外数据（JSON 字符串）
    //    /// </summary>
    //    public string ExtraData;

    //    public override void OnBeforeSave()
    //    {
    //        base.OnBeforeSave();
    //        if (PlayerData != null)
    //        {
    //            PlayerData.OnBeforeSave();
    //        }
    //    }

    //    public override void OnAfterLoad()
    //    {
    //        base.OnAfterLoad();
    //        if (PlayerData != null)
    //        {
    //            PlayerData.OnAfterLoad();
    //        }
    //    }
    //}
}
