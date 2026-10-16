using System.Collections.Generic;
using UnityEngine;

namespace SnowballAdventure
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        private AudioSource sfxSource;
        private AudioSource musicSource;
        private string currentMusicName = null;

        private readonly Dictionary<string, AudioClip> audioCache = new Dictionary<string, AudioClip>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;

            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.playOnAwake = false;
            musicSource.loop = true;

            PreloadAudio();
        }

        private void PreloadAudio()
        {
            string[] sfxNames = new string[]
            {
                "sfx_jump", "sfx_coin", "sfx_spring", "sfx_crumble",
                "sfx_pop", "sfx_checkpoint", "sfx_victory", "sfx_click",
                "music_menu", "music_gameplay"
            };

            foreach (string name in sfxNames)
            {
                AudioClip clip = Resources.Load<AudioClip>("Audio/" + name);
                if (clip != null)
                {
                    audioCache[name] = clip;
                }
            }
        }

        public void PlaySfx(string soundName, float volume = 1f)
        {
            if (!GameDataManager.Data.isSfxEnabled) return;

            if (!audioCache.TryGetValue(soundName, out AudioClip clip))
            {
                clip = Resources.Load<AudioClip>("Audio/" + soundName);
                if (clip != null) audioCache[soundName] = clip;
            }

            if (clip != null && sfxSource != null)
            {
                sfxSource.PlayOneShot(clip, volume);
            }
        }

        public void PlayJump() => PlaySfx("sfx_jump", 0.75f);
        public void PlayCoin() => PlaySfx("sfx_coin", 0.85f);
        public void PlaySpring() => PlaySfx("sfx_spring", 0.9f);
        public void PlayCrumble() => PlaySfx("sfx_crumble", 0.8f);
        public void PlayPop() => PlaySfx("sfx_pop", 1f);
        public void PlayCheckpoint() => PlaySfx("sfx_checkpoint", 0.9f);
        public void PlayVictory() => PlaySfx("sfx_victory", 1f);
        public void PlayClick() => PlaySfx("sfx_click", 0.6f);

        // Music playback
        public void PlayMenuMusic()
        {
            PlayMusic("music_menu", 0.65f);
        }

        public void PlayGameplayMusic()
        {
            PlayMusic("music_gameplay", 0.65f);
        }

        public void PlayMusic(string trackName, float volume = 0.65f)
        {
            if (musicSource == null) return;
            if (currentMusicName == trackName && musicSource.isPlaying) return;

            currentMusicName = trackName;
            if (!audioCache.TryGetValue(trackName, out AudioClip clip))
            {
                clip = Resources.Load<AudioClip>("Audio/" + trackName);
                if (clip != null) audioCache[trackName] = clip;
            }

            if (clip != null)
            {
                musicSource.clip = clip;
                musicSource.volume = volume;
                musicSource.loop = true;
                musicSource.mute = !GameDataManager.Data.isMusicEnabled;
                musicSource.Play();
            }
        }

        public void StopMusic()
        {
            if (musicSource != null)
            {
                musicSource.Stop();
                currentMusicName = null;
            }
        }

        public void SetMusicEnabled(bool enabled)
        {
            GameDataManager.Data.isMusicEnabled = enabled;
            if (musicSource != null)
            {
                musicSource.mute = !enabled;
                if (enabled && !musicSource.isPlaying && !string.IsNullOrEmpty(currentMusicName))
                {
                    musicSource.Play();
                }
            }
            GameDataManager.Save();
        }

        public void SetSfxEnabled(bool enabled)
        {
            GameDataManager.Data.isSfxEnabled = enabled;
            if (sfxSource != null)
            {
                sfxSource.mute = !enabled;
            }
            GameDataManager.Save();
        }

        public static SoundManager EnsureInstance()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject("SoundManager");
                Instance = go.AddComponent<SoundManager>();
            }
            return Instance;
        }
    }
}
