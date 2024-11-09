using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongManager : MonoBehaviour
{
    public AudioSource source;
    public List<AudioClip> playlist; //List so I can change during runtime
    public bool shuffle;
    public bool saveSong;
    AudioClip currentSong;
    bool playing = true;
    public TextMeshProUGUI upNext;
    public TextMeshProUGUI current;
    public Slider time;
    private void Start()
    {
        time.onValueChanged.AddListener(SliderValueChanged);
        if (saveSong)
        {
            if (PlayerPrefs.HasKey("currentSong"))
                SwapSong(playlist[PlayerPrefs.GetInt("currentSong")]);
            else
            {
                PlayerPrefs.SetInt("currentSong", 0);
                SwapSong(playlist[PlayerPrefs.GetInt("currentSong")]);
            }
        }
        else
            SwapSong(playlist[0]);
        UpdateText();
        source.Play();
    }

    public void PreparePlaylistSwap()
    {
        if (!shuffle)
        {
            int song = playlist.IndexOf(currentSong) + 1;
            if (song > playlist.Count | song < 0 | playlist[song] == null)
            {
                SwapSong(playlist[0]);
            }
            else
                SwapSong(playlist[song]);
        }
        else
        {
            SwapSong(playlist[UnityEngine.Random.Range(0, playlist.Count)]);
        }
        UpdateText();
    }

    public void Rewind()
    {
        return;
    }

    public void FastForward()
    {
        return;
    }

    public void Pause()
    {
        playing = false;
        source.Pause();
        CancelInvoke();
    }

    public void Play()
    {
        playing = true;
        source.UnPause();
        Invoke(nameof(PreparePlaylistSwap), source.time);
    }

    public void SwapSong(AudioClip clip)
    {
        currentSong = clip;
        source.clip = clip;
        Invoke(nameof(PreparePlaylistSwap), clip.length);
        source.Play();
    }

    public void UpdateText()
    {
        upNext.text = "UP NEXT: " + playlist[playlist.IndexOf(currentSong) + 1].name;
        current.text = "PLAYING: " + playlist[playlist.IndexOf(currentSong)].name;
    }

    private void Update() => UpdateTime();
    public void UpdateTime()
    {
        time.maxValue = currentSong.length;
        time.value = source.time;
    }

    public void SliderValueChanged(Single amt)
    {
        return;
    }
}