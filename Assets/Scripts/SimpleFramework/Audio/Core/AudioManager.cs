using SimpleFramework.Aduio;
using UnityEngine.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleFramework.Audio
{
    public class AudioManager : IAudioManager
    {
        private int priority = 0;

        public int Priority => priority;

        /// <summary>
        /// 活跃的声音列表
        /// </summary>
        private readonly List<AudioEmitter> activeSoundEmitter = new List<AudioEmitter>();

        private readonly Queue<AudioEmitter> frequentAudioEmitters = new Queue<AudioEmitter>();

        private ObjectPool<AudioEmitter> soundEmitterPool;

        private GameObject audioEmitterPrefab;

        /// <summary>
        /// 同时播发任意声音的最大数量
        /// </summary>
        private int maxSoundInstance = 30;

        void OnDestoryPool(AudioEmitter audio)
        {
            GameObject.Destroy(audio);
        }

        void OnReturnToPool(AudioEmitter audio)
        {
            audio.gameObject.SetActive(false);
            activeSoundEmitter.Remove(audio);
        }

        void OnTakeFromPool(AudioEmitter audio)
        {
            audio.gameObject.SetActive(true);
            activeSoundEmitter.Add(audio);
        }

        AudioEmitter CreateAudioEmitter()
        {
            var audio = GameObject.Instantiate(audioEmitterPrefab);
            audio.SetActive(false);
            return audio.GetComponent<AudioEmitter>();
        }

        private void InitializePool()
        {
            soundEmitterPool = new ObjectPool<AudioEmitter>(CreateAudioEmitter, OnTakeFromPool, OnReturnToPool, OnDestoryPool, true, 10, 100);
        }


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
            return soundEmitterPool.Get();
        }

        /// <summary>
        /// 返回给对象池
        /// </summary>
        /// <param name="audioEmitter"></param>
        public void ReturnToPool(AudioEmitter audioEmitter)
        {
            soundEmitterPool.Release(audioEmitter);
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
            InitializePool();
        }

        public void AfterManagerInit() { }

        public void OnManagerDestroy() { }

    }
}