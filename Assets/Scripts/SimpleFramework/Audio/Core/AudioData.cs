using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SimpleFramework.Audio
{
    [Serializable]
    public class AudioData
    {
        public AudioClip clip;
        public AudioMixerGroup audioMixerGroup;
        public bool isLoop;
        public bool isPlayerOnAwake;
        public bool isFrequentSound;
    }
}