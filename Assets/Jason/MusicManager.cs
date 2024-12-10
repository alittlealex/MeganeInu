using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource currentSong = null;
    [SerializeField] private AudioSource collageSong = null;
    [SerializeField] private AudioSource glitchSong = null;

    [field: SerializeField][field: ReadOnly] public bool GlassesON { get; set; } = false;

    // Update is called once per frame
    void Update()
    {
        if (!GlassesON)
        {
            CollageSong();
        } else if (GlassesON)
        {
            GlitchSong();
        }
    }

    public AudioSource CurrentSong
    {
        get => currentSong;
        set => currentSong = value;
    }

    private void GlitchSong()
    {
        if (this == null) return;
        if (this.CurrentSong == null)
        {
            this.CurrentSong = glitchSong;
            this.CurrentSong.mute = false;
            return;
        }

        StartCoroutine(FadeSongOut(this.CurrentSong));
        this.CurrentSong = glitchSong;
        StartCoroutine(FadeSongIn(this.CurrentSong));
    }

    private void CollageSong()
    {
        if (this == null) return;
        if (this.CurrentSong == null)
        {
            this.CurrentSong = collageSong;
            this.CurrentSong.mute = false;
            return;
        }

        StartCoroutine(FadeSongOut(this.CurrentSong));
        this.CurrentSong = collageSong;
        StartCoroutine(FadeSongIn(this.CurrentSong));
    }

    private IEnumerator FadeSongIn(AudioSource song)
    {
        float timeFade = 1f;
        float timeElapsed = 0;

        while (timeElapsed < timeFade)
        {
            song.volume = Mathf.Lerp(0, 1, timeElapsed / timeFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeSongOut(AudioSource song)
    {
        float timeFade = 1f;
        float timeElapsed = 0;

        while (timeElapsed < timeFade)
        {
            song.volume = Mathf.Lerp(1, 0, timeElapsed / timeFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
