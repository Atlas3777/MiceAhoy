using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure;
using UnityEngine;
using VContainer;

namespace Game.Scripts
{
    public class SoundManager : MonoBehaviour
    {
        [Header("Settings")]
        [Range(0, 1)] [SerializeField] private float musicVolume = 0.6f; // #TODO БЕРЕТСЯ НЕ ИЗ СЕЙВА!
        [Range(0, 1)] [SerializeField] private float sfxVolume = 0.8f;
        [SerializeField] private float fadeDuration = 1.2f;

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSourceA;
        [SerializeField] private AudioSource musicSourceB;
        [SerializeField] private AudioSource sfxSource;

        private readonly Dictionary<Guid, AudioSource> _activeLoops = new();
        private readonly Stack<AudioSource> _pool = new();
        
        private bool _isSourceAActive = true;
        private CancellationTokenSource _musicFadeCts;
        
        private bool _isPaused;
        

        [Inject] private SaveService _saveService;

        private void Awake()
        {
            SetupSource(musicSourceA, true);
            SetupSource(musicSourceB, true);
            SetupSource(sfxSource, false);

            
            // musicSourceA.volume = _saveService.Data.MusicVolume;
            // musicSourceB.volume = _saveService.Data.MusicVolume;
            
            // Важно: sfxSource всегда на полной громкости, 
            // так как PlayOneShot сам регулирует громкость клипа
            sfxSource.volume = 1f; 
        }

        private void SetupSource(AudioSource source, bool loop)
        {
            source.loop = loop;
            source.playOnAwake = false;
            source.volume = 0;
        }

        // --- LOOPING SFX (Плита и т.д.) ---

        public Guid PlayLoopingSfx(AudioClip clip, float volumeFactor = 1f)
        {
            Debug.Log("PlayLoopingSfx");
            if (clip == null) return Guid.Empty;
            Debug.Log($"{clip.name}");

            var source = GetSourceFromPool();
            if(source == null) Debug.Log($"source == null");
            
            Debug.Log($"{source.gameObject.name}");
            
            source.clip = clip;
            source.loop = true;
            source.volume = sfxVolume * volumeFactor;
            source.pitch = 1f;
            
            source.Play();

            Guid id = Guid.NewGuid();
            _activeLoops.Add(id, source);
            return id;
        }

        public void StopLoopingSfx(Guid id)
        {
            if (id == Guid.Empty) return;

            if (_activeLoops.TryGetValue(id, out var source))
            {
                source.Stop();
                source.clip = null;
                _activeLoops.Remove(id);
                _pool.Push(source);
            }
        }

        private AudioSource GetSourceFromPool()
        {
            if (_pool.Count > 0)
            {
                Debug.Log("GetSourceFromPool");
                var s = _pool.Pop();
                if (s != null) return s; 
            }
            
            var go = new GameObject($"LoopingSource_{_activeLoops.Count + _pool.Count}");
            go.transform.SetParent(transform);
            var newSource = go.AddComponent<AudioSource>();
            newSource.playOnAwake = false;
            return newSource;
        }
        

        /// <summary>
        /// Ставит на паузу или возобновляет все звуки (музыку и активные цикличные SFX)
        /// </summary>
        public void SetPause(bool isPaused)
        {
            _isPaused = isPaused;

            // Пауза музыки
            if (isPaused)
            {
                musicSourceA.Pause();
                musicSourceB.Pause();
            }
            else
            {
                musicSourceA.UnPause();
                musicSourceB.UnPause();
            }

            // Пауза всех активных зацикленных звуков (плиты и т.д.)
            foreach (var source in _activeLoops.Values)
            {
                if (source == null) continue;
                if (isPaused) source.Pause();
                else source.UnPause();
            }
        }
        

        // --- ONE SHOTS (Дзынь, клики) ---

        public void PlaySfx(AudioClip clip, float pitchRandomness = 0.1f)
        {
            if (clip == null) return;
            // Используем sfxSource только для "выстрелил и забыл"
            float randomPitch = 1f + UnityEngine.Random.Range(-pitchRandomness, pitchRandomness);
            sfxSource.PlayOneShot(clip, sfxVolume);
            // Замечание: PlayOneShot не поддерживает смену pitch на лету для одного звука,
            // если хочешь рандомный питч для ваншотов, лучше юзать временные сорсы или менять pitch у sfxSource
            sfxSource.pitch = randomPitch; 
        }

        // --- MUSIC ---

        public async UniTask PlayMusicAsync(AudioClip clip, bool fade = true)
        {
            if (clip == null) return;

            _musicFadeCts?.Cancel();
            _musicFadeCts?.Dispose();
            _musicFadeCts = new CancellationTokenSource();

            AudioSource active = _isSourceAActive ? musicSourceA : musicSourceB;
            AudioSource next = _isSourceAActive ? musicSourceB : musicSourceA;

            if (active.clip == clip && active.isPlaying) return;

            next.clip = clip;
            next.Play();

            if (fade) await CrossfadeAsync(active, next, _musicFadeCts.Token);
            else
            {
                next.volume = musicVolume;
                active.Stop();
                active.volume = 0;
            }
            _isSourceAActive = !_isSourceAActive;
        }

        private async UniTask CrossfadeAsync(AudioSource fadeOut, AudioSource fadeIn, CancellationToken ct)
        {
            float startActiveVol = fadeOut.volume;
            float timer = 0;
            while (timer < fadeDuration)
            {
                if (ct.IsCancellationRequested) return;
                timer += Time.deltaTime;
                float progress = timer / fadeDuration;
                fadeOut.volume = Mathf.Lerp(startActiveVol, 0, progress);
                fadeIn.volume = Mathf.Lerp(0, musicVolume, progress);
                await UniTask.Yield(PlayerLoopTiming.Update, ct);
            }
            fadeOut.Stop();
            fadeOut.volume = 0;
            fadeIn.volume = musicVolume;
        }
        
        /// <summary>
        /// Останавливает текущую музыку.
        /// </summary>
        /// <param name="fade">Нужно ли плавное затухание</param>
        public async UniTask StopMusicAsync(bool fade = true)
        {
            // Отменяем любые текущие процессы перехода (Crossfade)
            _musicFadeCts?.Cancel();
            _musicFadeCts?.Dispose();
            _musicFadeCts = new CancellationTokenSource();

            AudioSource active = _isSourceAActive ? musicSourceA : musicSourceB;
            AudioSource inactive = _isSourceAActive ? musicSourceB : musicSourceA;

            if (fade)
            {
                // Плавно выключаем оба источника на случай, если они оба играли во время кроссфейда
                await UniTask.WhenAll(
                    FadeOutSourceAsync(active, _musicFadeCts.Token),
                    FadeOutSourceAsync(inactive, _musicFadeCts.Token)
                );
            }
            else
            {
                active.Stop();
                active.volume = 0;
                inactive.Stop();
                inactive.volume = 0;
            }
        }

        private async UniTask FadeOutSourceAsync(AudioSource source, CancellationToken ct)
        {
            if (source.volume <= 0 || !source.isPlaying) 
            {
                source.Stop();
                source.volume = 0;
                return;
            }

            float startVol = source.volume;
            float timer = 0;

            while (timer < fadeDuration)
            {
                if (ct.IsCancellationRequested) return;
                timer += Time.deltaTime;
                source.volume = Mathf.Lerp(startVol, 0, timer / fadeDuration);
                await UniTask.Yield(PlayerLoopTiming.Update, ct);
            }

            source.Stop();
            source.volume = 0;
        }

        private void OnDestroy()
        {
            _musicFadeCts?.Cancel();
            _musicFadeCts?.Dispose();
            // Чистим все зацикленные звуки при уничтожении менеджера
            foreach (var source in _activeLoops.Values)
            {
                if (source != null) source.Stop();
            }
        }
    }
}