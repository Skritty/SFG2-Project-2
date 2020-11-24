using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioSettings", menuName = "Settings/Audio Settings")]
public class AudioSettings : ScriptableObject
{
    [Header("Basic Settings")]
    [SerializeField] GameObject audioPlayer;
    public GameObject AudioPlayer => audioPlayer;

    [Header("Music")]
    [SerializeField] AudioClip menuMusic;
    public AudioClip MenuMusic => menuMusic;

    [SerializeField] AudioClip playerMusic;
    public AudioClip PlayerMusic => playerMusic;

    [SerializeField] AudioClip botMusic;
    public AudioClip BotMusic => botMusic;

    [Header("Audio Clips")]
    [SerializeField] AudioClip turnChange;
    public AudioClip TurnChange => turnChange;

    [SerializeField] AudioClip cardHover;
    public AudioClip CardHover => cardHover;

    [SerializeField] AudioClip cardDraw;
    public AudioClip CardDraw => cardDraw;

    [SerializeField] AudioClip cardPickup;
    public AudioClip CardPickup => cardPickup;

    [SerializeField] AudioClip cardDrop;
    public AudioClip CardDrop => cardDrop;

    [SerializeField] AudioClip tileSelect;
    public AudioClip TileSelect => tileSelect;

    [SerializeField] AudioClip tileHover;
    public AudioClip TileHover => tileHover;
}
