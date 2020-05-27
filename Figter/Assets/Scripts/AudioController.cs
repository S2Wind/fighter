using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    [SerializeField] AudioClip runClip;

    [SerializeField] AudioClip slideClip;

    [SerializeField] AudioClip jumpClip;

    AudioSource audioSource;

    int attitudeBefore;

    bool isPlay;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (PlayerControl.Attitudes == PlayerControl.Run)
        {
            if (!audioSource.isPlaying || attitudeBefore != PlayerControl.Attitudes)
            {
                PlayClip(runClip);
            }
        }
        else if (PlayerControl.Attitudes == PlayerControl.Slide && PlayerControl.IsGround1)
        {
            if (!audioSource.isPlaying || attitudeBefore != PlayerControl.Attitudes)
            {
                PlayClip(slideClip);
            }
        }
        else if (PlayerControl.Attitudes == PlayerControl.Jump && PlayerControl.IsGround1)
        {
            if (!audioSource.isPlaying || attitudeBefore != PlayerControl.Attitudes)
            {
                PlayClip(jumpClip);
            }
        }
        else if(attitudeBefore != PlayerControl.Jump)
            audioSource.Stop();
    }


    public void PlayClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();

        attitudeBefore = PlayerControl.Attitudes;
    }

}
