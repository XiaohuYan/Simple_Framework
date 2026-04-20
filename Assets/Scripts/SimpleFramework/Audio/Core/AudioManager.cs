using SimpleFramework.Aduio;
using SimpleFramework.Entry;
using SimpleFramework.ObjectPool;
using System.Collections.Generic;

namespace SimpleFramework.Audio
{
    public class AudioManager : IAudioManager
    {
        /// <summary>
        /// 活跃的声音列表
        /// </summary>
        private readonly List<AudioEmitter> activeSoundEmitter = new List<AudioEmitter>();

        private readonly Queue<AudioEmitter> frequentAudioEmitters = new Queue<AudioEmitter>();

        /// <summary>
        /// 同时播发任意声音的最大数量
        /// </summary>
        private int maxSoundInstance = 30;

        private IObjectPoolManager objectPoolManager;

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public AudioBuilder CreateSound()
        {
            return new AudioBuilder();
        }

        public void EnqueueFrequentSound(AudioEmitter soundEmitter)
        {
            frequentAudioEmitters.Enqueue(soundEmitter);
        }

        /// <summary>
        /// 从对象池取
        /// </summary>
        /// <returns></returns>
        public AudioEmitter Get()
        {
            return objectPoolManager.Get<AudioEmitter>();
        }

        /// <summary>
        /// 返回给对象池
        /// </summary>
        /// <param name="audioEmitter"></param>
        public void ReturnToPool(AudioEmitter audioEmitter)
        {
            objectPoolManager.Return<AudioEmitter>(audioEmitter);
        }

        /// <summary>
        /// 是否可以播放
        /// </summary>
        /// <param name="audioData"></param>
        /// <returns></returns>
        public bool CanPlayeSound(AudioData audioData)
        {
            if (!audioData.isFrequentSound)
            {
                return true;
            }

            if (frequentAudioEmitters.Count >= maxSoundInstance && frequentAudioEmitters.TryDequeue(out var audioEmitter))
            {
                try
                {
                    audioEmitter.Stop();
                    return true;
                }
                catch
                {
                    UnityEngine.Debug.Log("声音已经释放");
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// 创建声音发射器时
        /// </summary>
        /// <param name="soundEmitter"></param>
        private void OnCreateSoundEmitter(AudioEmitter soundEmitter)
        {
            UnityEngine.Object.Instantiate(soundEmitter);
            soundEmitter.gameObject.SetActive(false);
        }

        /// <summary>
        /// 从对象池取时
        /// </summary>
        /// <param name="soundEmitter"></param>
        private void OnGetSoundEmitter(AudioEmitter soundEmitter)
        {
            soundEmitter.gameObject.SetActive(true);
            activeSoundEmitter.Add(soundEmitter);
        }

        /// <summary>
        /// 放回对象池时
        /// </summary>
        /// <param name="soundEmitter"></param>
        private void OnReturnSoundEmitter(AudioEmitter soundEmitter)
        {
            soundEmitter.gameObject.SetActive(false);
            activeSoundEmitter.Remove(soundEmitter);
        }

        /// <summary>
        /// 销毁对象池时
        /// </summary>
        /// <param name="soundEmitter"></param>
        private void OnDestorySoundEmitter(AudioEmitter soundEmitter)
        {
            UnityEngine.Object.Destroy(soundEmitter.gameObject);
        }

        public void OnManagerInit()
        {

        }

        public void AfterManagerInit()
        {
            objectPoolManager = GameFacade.Instance.GetManager<IObjectPoolManager>();
            objectPoolManager.CreatePoolWithCallback<AudioEmitter>(100, OnCreateSoundEmitter, OnGetSoundEmitter, OnReturnSoundEmitter, OnDestorySoundEmitter);
        }

        public void OnManagerDestroy()
        {
            
        }

    }
}