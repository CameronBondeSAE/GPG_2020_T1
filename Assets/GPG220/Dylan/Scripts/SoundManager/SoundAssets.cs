using System;
using UnityEngine;

namespace SoundManager
{
    public class SoundAssets : MonoBehaviour
    {
        public static SoundAssets instance = null;
        private SoundAudioClip soundAudioClip;

        public void Awake()
        {
            soundAudioClip = new SoundAudioClip();
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != null)
            {
                Destroy(gameObject);
            }
        }

        // public static SoundAssets i { get; }

        public SoundAudioClip[] soundAudioClipArray;

        [Serializable]
        public class SoundAudioClip
        {
            public SoundManager.Sound sound;
            public AudioClip audioClip;
            public float soundDelay;
            [Range(0, 100)] public float volume;
        }

        //TODO fix this
        public bool CanPlaySound(SoundManager.Sound sound)
        {
            if (soundAudioClip.soundDelay < Time.time)
            {
                return true;
            }

            return false;
        }
    }
}