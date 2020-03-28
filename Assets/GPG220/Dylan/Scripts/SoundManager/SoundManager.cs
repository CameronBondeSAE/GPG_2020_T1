using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace SoundManager
{
    public static class SoundManager
    {
        //simply add in sound name to enum and add clip to sound assets
        public enum Sound
        {
            meleeAtt,
            rangeAtt,
            die,
            moving,
        }


        //use this version of the function to spawn in 3D space
        public static void PlaySound(Sound sound, Vector3 position)
        {
            if (SoundAssets.instance.CanPlaySound(sound))
            {
                GameObject soundGameObject = new GameObject("Sound");
                soundGameObject.transform.position = position;
                AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
                audioSource.clip = GetAudioClip(sound, audioSource);
                audioSource.Play();
                Object.Destroy(soundGameObject, audioSource.clip.length);
            }
        }

        //simply call SoundManager.Play sound to play desired sound
        public static void PlaySound(Sound sound)
        {
            if (SoundAssets.instance.CanPlaySound(sound))
            {
                GameObject soundGameObject = new GameObject("Sound");
                AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
                audioSource.PlayOneShot(GetAudioClip(sound, audioSource));
                Object.Destroy(soundGameObject, audioSource.clip.length);
            }
        }


        private static AudioClip GetAudioClip(Sound sound, AudioSource audioSource)
        {
            foreach (SoundAssets.SoundAudioClip soundAudioClip in SoundAssets.instance.soundAudioClipArray)
            {
                if (soundAudioClip.sound == sound)
                {
                    audioSource.volume = soundAudioClip.volume;

                    return soundAudioClip.audioClip;
                }
            }

            Debug.LogError("Sound " + sound + " not found!");
            Object.Destroy(audioSource);
            return null;
        }
    }
}