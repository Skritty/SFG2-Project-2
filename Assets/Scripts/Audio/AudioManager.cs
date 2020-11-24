using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public AudioSettings settings;
    AudioSource musicPlayer;

    private void Start()
    {
        musicPlayer = GetComponent<AudioSource>();
        musicPlayer.loop = true;
        musicPlayer.clip = settings.MenuMusic;
        musicPlayer.Play();
        GameInput.OnCardHoverEnter += CardHover;
        GameInput.OnCardPickup += CardPickup;
        GameInput.OnCardDropped += CardDrop;
        GameInput.OnTileSelect += TileSelect;
        GameInput.OnTileHoverEnter += TileHover;
    }

    public void CardHover(CardUI card)
    {
        PlayAudioClip(settings.CardHover, card.transform.position);
    }

    public void CardPickup(CardUI card, Tile tile)
    {
        PlayAudioClip(settings.CardPickup, card.transform.position);
    }

    public void CardDrop(CardUI card, Tile tile)
    {
        PlayAudioClip(settings.CardDrop, card.transform.position);
    }

    public void TileSelect(Tile tile)
    {
        PlayAudioClip(settings.TileSelect, tile.transform.position);
    }

    public void TileHover(Tile tile)
    {
        //PlayAudioClip(settings.TileHover, tile.transform.position);
    }

    public void PlayAudioClip(AudioClip clip, Vector3 position)
    {
        AudioSource source = Instantiate(settings.AudioPlayer, position, Quaternion.identity, transform).GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        Destroy(source.gameObject, clip.length);
    }

    public void SwitchSongs(AudioClip song)
    {
        musicPlayer.clip = song;
        musicPlayer.Play();
    }
}
