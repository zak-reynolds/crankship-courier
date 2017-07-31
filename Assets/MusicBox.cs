using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicBox : MonoBehaviour {

    public enum Song { Intro, Fly, Boss }

    [SerializeField]
    private AudioMixer mixer;
    [SerializeField]
    private AudioClip introMusic;
    [SerializeField]
    private AudioClip flyMusic;
    [SerializeField]
    private AudioClip bossMusic;

    private AudioSource aTrack;
    [SerializeField]
    private AudioMixerGroup aTrackMixerGroup;
    private AudioSource bTrack;
    [SerializeField]
    private AudioMixerGroup bTrackMixerGroup;

    private AudioMixerSnapshot aSnapshot;
    private AudioMixerSnapshot bSnapshot;

    private bool aTrackActive = true;

    private static MusicBox instance;

    private void Awake()
    {
        instance = this;
    }

    void Start () {
        aTrack = gameObject.AddComponent<AudioSource>();
        bTrack = gameObject.AddComponent<AudioSource>();

        aTrack.clip = introMusic;
        aTrack.loop = true;
        aTrack.outputAudioMixerGroup = aTrackMixerGroup;
        aTrack.Play();

        bTrack.clip = flyMusic;
        bTrack.loop = true;
        bTrack.outputAudioMixerGroup = bTrackMixerGroup;
        aSnapshot = mixer.FindSnapshot("TrackAPlaying");
        bSnapshot = mixer.FindSnapshot("TrackBPlaying");
    }
	
	public static void ChangeMusic(Song song)
    {
        if (!instance)
        {
            instance = GameObject.Find("GameDirector").GetComponent<MusicBox>();
        }
        instance.changeMusic(song);
    }

    private void changeMusic(Song song)
    {
        AudioClip nextClip = introMusic;
        switch (song)
        {
            case Song.Intro: nextClip = introMusic; break;
            case Song.Fly: nextClip = flyMusic; break;
            case Song.Boss: nextClip = bossMusic; break;
        }
        if (aTrackActive)
        {
            bTrack.clip = nextClip;
            bTrack.Play();
            mixer.TransitionToSnapshots(new AudioMixerSnapshot[] { bSnapshot }, new float[] { 1 }, 0.5f);
        }
        else
        {
            aTrack.clip = nextClip;
            aTrack.Play();
            mixer.TransitionToSnapshots(new AudioMixerSnapshot[] { aSnapshot }, new float[] { 1 }, 0.5f);
        }
    }
}
