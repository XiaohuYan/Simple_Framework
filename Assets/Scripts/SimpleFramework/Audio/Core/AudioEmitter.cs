using SimpleFramework.Audio;
using SimpleFramework.Entry;
using SimpleFramework.Extension;
using System.Collections;
using UnityEngine;

namespace SimpleFramework.Aduio
{
    public class AudioEmitter : MonoBehaviour
    {
        private AudioSource audioSource;
        private Coroutine playeringCoroutine;
        public AudioData AudioData {  get; private set; }
        private void Awake()
        {
            audioSource = gameObject.GetOrAdd<AudioSource>();
        }

        /// <summary>
        /// 播放
        /// </summary>
        public void Play()
        {
            if (playeringCoroutine == null)
            {
                StopCoroutine(playeringCoroutine);
            }

            audioSource.Play();
            playeringCoroutine = StartCoroutine(WaitForSoundToEnd());
        }

        /// <summary>
        /// 检测是否播放完
        /// </summary>
        /// <returns></returns>
        IEnumerator WaitForSoundToEnd()
        {
            yield return new WaitWhile(() => audioSource.isPlaying);
            GameFacade.Instance.GetManager<IAudioManager>().ReturnToPool(this);
        }
        
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (playeringCoroutine != null)
            {
                StopCoroutine(playeringCoroutine);
                playeringCoroutine = null;
            }

            audioSource.Stop();
            GameFacade.Instance.GetManager<IAudioManager>().ReturnToPool(this);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="audioData"></param>
        public void Initialize(AudioData audioData)
        {
            AudioData = audioData;
            audioSource.clip = audioData.clip;
            audioSource.outputAudioMixerGroup = audioData.audioMixerGroup;
            audioSource.loop = audioSource.loop;
            audioSource.playOnAwake = audioSource.playOnAwake;
        }

        public void WithRandomPitch(float min = -0.05f, float max = 0.05f)
        {
            audioSource.pitch += Random.Range(min, max);
        }
    }

}