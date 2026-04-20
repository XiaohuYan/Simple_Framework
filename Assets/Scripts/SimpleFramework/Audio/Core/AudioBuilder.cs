using SimpleFramework.Aduio;
using SimpleFramework.Entry;
using UnityEngine;

namespace SimpleFramework.Audio
{
    public class AudioBuilder
    {
        private AudioData audioData;
        private Vector3 position = Vector3.zero;
        private bool randomPitch;

        public AudioBuilder WithAudioData(AudioData audioData)
        {
            this.audioData = audioData;
            return this;
        }

        public AudioBuilder WithPosition(Vector3 position)
        {
            this.position = position;
            return this;
        }

        public AudioBuilder WithRandomPitch()
        {
            this.randomPitch = true;
            return this;
        }

        public void Play()
        {
            IAudioManager audioManager = GameFacade.Instance.GetManager<IAudioManager>();
            if (!audioManager.CanPlayeSound(audioData))
            {
                return;
            }
            AudioEmitter audioEmitter = audioManager.Get();
            audioEmitter.Initialize(audioData);
            audioEmitter.transform.position = position;

            if (randomPitch)
            {
                audioEmitter.WithRandomPitch();
            }

            if(audioData.isFrequentSound)
            {
                audioManager.EnqueueFrequentSound(audioEmitter);
            }

            audioEmitter.Play();
        }
    }
}