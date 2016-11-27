using UnityEngine;
using System.Collections;

public class AudioScript : MonoBehaviour
{
    AudioClip BackGroundMusic;
    AudioClip BattleMusic;
    AudioSource AudioPlayer;
    // play another track
    bool TrackChange;
    AudioClip CurrentTrack;
    // Use this for initialization
    void Start()
    {
        AudioPlayer = GetComponent<AudioSource>();
        AudioPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (TrackChange)
        {
            FadeOut();
        }
        else if (AudioPlayer.volume < 1)
        {

        }

    }

    public void Track(int Type)
    {
        switch (Type)
        {
            case 0:
                {
                    CurrentTrack = BackGroundMusic;
                }
                break;
            case 1:
                {
                    CurrentTrack = BattleMusic;
                }
                break;
            default:
                break;
        }
        TrackChange = true;
    }


    void Fade()
    {

    }
    void FadeOut()
    {

    }
    void FadeIn()
    {

    }
}
