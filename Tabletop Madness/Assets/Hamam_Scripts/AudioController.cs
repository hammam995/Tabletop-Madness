using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip[] menuMusic;
    public AudioClip[] gameplayMusic;

    public AudioClip specialCharged;
    public AudioClip won, lost;

    private AudioSource source;
    private enum MusicState { Menu, Gameplay};
    private MusicState musicState;

    private void Awake()
    {
        source = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(!source.isPlaying)
        {
            switch (musicState)
            {
                case MusicState.Menu:
                    PlayMenuMusic();
                    break;
                case MusicState.Gameplay:
                    PlayGameplayMusic();
                    break;
            }
        }        
    }

    public void PlayMenuMusic()
    {
        source.Stop();
        musicState = MusicState.Menu;
        source.PlayOneShot(menuMusic[Random.Range(0, menuMusic.Length)]);
    }

    public void PlayGameplayMusic()
    {
        source.Stop();
        musicState = MusicState.Gameplay;
        source.PlayOneShot(gameplayMusic[Random.Range(0, gameplayMusic.Length)]);
    }

    public void PlaySpecialCharged()
    {
        source.PlayOneShot(specialCharged);
    }

    public void PlayWon()
    {
        source.PlayOneShot(won);
    }

    public void PlayLost()
    {
        source.PlayOneShot(lost);
    }
}
